using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Intefaces
{
    public interface IEsp32ToolService
    {
        Task<List<string>> GetAvailableSerialPorts();
        Task<Result> Initialize(ITaskStatusMonitor statusMonitor);
        Task<Result> FlashFirmware(string portName, string firmwareFile, ITaskStatusMonitor statusMonitor);
        Task<Result<FileInfo>> DownloadLatestFirmware(ITaskStatusMonitor taskMonitor);
    }
}
