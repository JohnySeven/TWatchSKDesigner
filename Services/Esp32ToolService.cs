using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Helpers;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Services
{
    //https://github.com/espressif/esptool/releases/tag/v3.1
    public class Esp32ToolService : IEsp32ToolService
    {
        private const string FirmwareUrl = @"https://dev.dytrych.cloud/twatchsk.zip";
        private string EspToolDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TWatchDesigner", "Esptool");
        private string EspToolAppPath => Path.Combine(EspToolDirectory, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "esptool.exe" : "esptool");
        private string FirmwareDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TWatchDesigner", "Firmware");
        private string FirmwareArchivePath => Path.Combine(FirmwareDirectory, "twatchsk.zip");
        private string FirmwareInfoPath => Path.Combine(FirmwareDirectory, "firmware.json");

        public async Task<Result<FileInfo>> DownloadLatestFirmware(ITaskStatusMonitor taskMonitor)
        {
            var ret = new Result<FileInfo>();
            taskMonitor.OnProgress("Downloading firmware from " + FirmwareUrl);

            try
            {
                if(!Directory.Exists(FirmwareDirectory))
                {
                    Directory.CreateDirectory(FirmwareDirectory);
                }

                if(File.Exists(FirmwareArchivePath))
                {
                    File.Delete(FirmwareArchivePath);
                }

                var result = await GithubHelper.DownloadFile(FirmwareUrl, FirmwareArchivePath, taskMonitor);

                if (result)
                {
                    try
                    {
                        taskMonitor.OnProgress("Extracting firmware archive...");

                        await Task.Run(() =>
                        {
                            using (var zipStream = File.OpenRead(FirmwareArchivePath))
                            {
                                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Read))
                                {
                                    zip.ExtractToDirectory(FirmwareDirectory, true);
                                }
                            }
                        });

                        ret.OnSuccess(new FileInfo(FirmwareInfoPath));
                    }
                    catch (Exception ex)
                    {
                        ret.OnException(ex);
                    }
                }
                else
                {
                    ret.OnError("DOWNLOAD", "Failed to download firmware from " + FirmwareUrl);
                }
            }
            catch (Exception ex)
            {
                taskMonitor.OnFail(ex);
            }

            return ret;
        }

        public async Task<Result> FlashFirmware(string portName, string firmwareFile, bool eraseFlash, ITaskStatusMonitor statusMonitor)
        {
            var infoResult = await LoadFirmwareInfo(firmwareFile);

            if (infoResult.IsSuccess && infoResult.Data != null)
            {
                var firmwareInfo = infoResult.Data;
                var eraseCommand = $"--chip esp32 --port {portName} erase_flash";

                Result? eraseResult = null;

                if (eraseFlash)
                {
                    statusMonitor.OnProgress("Erasing flash...");

                    eraseResult = await InvokeEspTool(eraseCommand, statusMonitor);
                }

                if (!eraseFlash || eraseResult?.IsSuccess == true)
                {
                    statusMonitor.OnProgress($"Uploading new Firmware version {firmwareInfo.Version}...");
                    //sample
                    //write_flash --port "COM5" --chip esp32 --baud 800000 --before default_reset --after hard_reset -z --flash_mode dio --flash_freq 40m --flash_size detect 0x1000 C:\Users\info\source\repos\TWatchSK\.pio\build\ttgo-t-watch\bootloader.bin 0x8000 C:\Users\info\source\repos\TWatchSK\.pio\build\ttgo-t-watch\partitions.bin 0xe000 C:\Users\info\source\repos\TWatchSK\.pio\build\ttgo-t-watch\ota_data_initial.bin 0x10000 .pio\build\ttgo-t-watch\firmware.bin
                    var flashCommand = $"--port {portName}";

                    flashCommand += " " + firmwareInfo.Arguments;

                    if (firmwareInfo.Files != null)
                    {
                        foreach (var binary in firmwareInfo.Files)
                        {
                            var fullPath = Path.Combine(Path.GetDirectoryName(firmwareFile), binary.File);

                            if (File.Exists(fullPath))
                            {
                                flashCommand += $" {binary.Address} \"{fullPath}\" ";
                            }
                            else
                            {
                                return new Result().OnError("FIRMWARE", $"File {binary.File} not found in extracted firmware!");
                            }
                        }

                        var flashResult = await InvokeEspTool(flashCommand, statusMonitor);

                        if(flashResult.IsSuccess)
                        {
                            if(await OpenPort(portName))
                            {
                                return flashResult;
                            }
                            else
                            {
                                return new Result().OnError("WATCH", "No communication from Watch in last 10 seconds. Please flash firmware once again!");
                            }
                        }

                        return flashResult;
                    }
                    else
                    {
                        return new Result().OnError("JSONE", "Firmware info doesn't contain any Files section!");
                    }
                }
                else
                {
                    return eraseResult;
                }
            }
            else
            {
                return infoResult;
            }
        }

        private async Task<bool> OpenPort(string portName)
        {
            using (var port = new SerialPort(portName, 115200))
            {
                port.Open();
                port.DiscardInBuffer();

                for (int i = 0; i < 10 && port.BytesToRead == 0; i++)
                {
                    await Task.Delay(1000);
                }

                var hasBytes = port.BytesToRead > 0;
                port.Close();

                return hasBytes;
            }
        }

        private static async Task<Result<FirmwareVersionInfo>> LoadFirmwareInfo(string firmwareFile)
        {
            var ret = new Result<FirmwareVersionInfo>();

            try
            {
                var info = await Task.Run(() => JsonConvert.DeserializeObject<FirmwareVersionInfo>(File.ReadAllText(firmwareFile)));

                if (info != null)
                {
                    ret.OnSuccess(info);
                }
                else
                {
                    ret.OnError("JSONE", "Invalid version.txt json file!");
                }
            }
            catch (Exception ex)
            {
                ret.OnException(ex);
            }

            return ret;
        }

        public async Task<Result> InvokeEspTool(string arguments, ITaskStatusMonitor statusMonitor)
        {
            var ret = new Result();

            var startInfo = new ProcessStartInfo(EspToolAppPath, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = EspToolDirectory
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            if (process != null)
            {
                process.OutputDataReceived += (s, e) =>
                {
                    statusMonitor.OnProgress(e.Data ?? "");
                };

                if (process.Start())
                {
                    process.BeginOutputReadLine();

                    await process.WaitForExitAsync();

                    if (process.ExitCode == 0)
                    {
                        ret.OnSuccess();
                    }
                    else
                    {
                        ret.OnError("ESPTOOL", "Code: " + process.ExitCode);
                    }
                }

            }
            else
            {
                ret.OnError("INVALID", "Unable to start child esptool process!");
            }

            return ret;
        }

        public Task<Result<IList<string>>> GetAvailableSerialPorts()
        {
            return Task.Run(() =>
            {
                var list = SerialPort.GetPortNames().ToList();
                var ret = new Result<IList<string>>
                {
                    Data = list
                };
                ret.OnSuccess();
                return ret;
            });
        }

        public async Task<Result> Initialize(ITaskStatusMonitor statusMonitor)
        {
            var ret = new Result();

            try
            {
                statusMonitor.OnStarted();

                statusMonitor.OnProgress("Checking if esptool is downloaded...");

                if(File.Exists(EspToolAppPath))
                {
                    statusMonitor.OnProgress($"esptool exists at {EspToolAppPath}...");
                }
                else
                {
                    statusMonitor.OnProgress($"esptool doesn't exist, will download latest version from github.");

                    statusMonitor.OnProgress($"Reading releases from Github repository...");

                    var latestRelease = await GithubHelper.GetRelease("espressif", "esptool");

                    if (latestRelease != null)
                    {
                        statusMonitor.OnProgress($"Got esptool version {latestRelease.TagName}, about to download it.");
                        
                        string? assetKey = null;
                        bool isUnix = true;

                        if (RuntimeInformation.ProcessArchitecture == Architecture.X64 || RuntimeInformation.ProcessArchitecture == Architecture.X86)
                        {
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            {
                                assetKey = "linux";
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            {
                                assetKey = "macos";
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            {
                                assetKey = "win64";
                                isUnix = false;
                            }
                        }

                        if (assetKey != null)
                        {
                            var asset = latestRelease.Assets.Where(a => a.Name.Contains(assetKey)).FirstOrDefault();

                            if (asset != null)
                            {
                                statusMonitor.OnProgress("Downloading esptool from " + asset.BrowserDownloadUrl);
                                var tempFile = Path.GetTempFileName();
                                var downloadResult = await GithubHelper.DownloadAsset(asset, statusMonitor, tempFile);

                                if (downloadResult)
                                {
                                    statusMonitor.OnProgress("Download is complete. Extracting...");

                                    if (!Directory.Exists(EspToolDirectory))
                                    {
                                        Directory.CreateDirectory(EspToolDirectory);
                                    }

                                    using (var zipStream = File.OpenRead(tempFile))
                                    {
                                        using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Read))
                                        {
                                            foreach (var entry in zip.Entries)
                                            {
                                                statusMonitor.OnProgress($"Extracting {entry.Name}...");
                                                var fullPath = Path.Combine(EspToolDirectory, entry.Name);
                                                await Task.Run(() => entry.ExtractToFile(fullPath));
                                                if(isUnix && entry.Name.StartsWith("esp"))
                                                {
                                                    UnixHelper.SetFilePermissions(fullPath, UnixHelper.S_User_RWX);
                                                }
                                            }

                                            await Task.Run(() => File.WriteAllText(Path.Combine(EspToolDirectory, "version.txt"), latestRelease.TagName));
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        File.Delete(tempFile);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                        {
                            statusMonitor.OnError("Current platform isn't supported!");
                            ret.OnError("NOTSUPPORTED", $"Current platform {RuntimeInformation.RuntimeIdentifier} {RuntimeInformation.ProcessArchitecture} isn't supported!");
                        }
                    }
                    else
                    {
                        statusMonitor.OnError("Unable to load latest release from Github!");
                        ret.OnError("DOWNLOADFAIL", "Unable to download esptool from Github");
                    }
                }

                ret.OnSuccess();
            }
            catch (Exception ex)
            {
                statusMonitor.OnFail(ex);
                ret.OnException(ex);
            }

            return ret;
        }

        Task<List<string>> IEsp32ToolService.GetAvailableSerialPorts()
        {
            return Task.FromResult(SerialPort.GetPortNames().ToList());
        }
    }
}
