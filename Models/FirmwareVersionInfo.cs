using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    /*{
    "version" : "0.3",
    "files":
    [
        { "address": "0x1000", "file": "bootloader.bin"},
        { "address": "0x8000", "file": "partitions.bin"},
        { "address": "0xe000", "file": "ota_data_initial.bin"},
        { "address": "0x10000", "file": "firmware.bin"}
    ],
    "arguments" : "--baud 800000 --before default_reset --after hard_reset write_flash -z --flash_mode dio --flash_freq 40m --flash_size detect"
}*/
    public class FirmwareVersionInfo
    {
        public string? Version { get; set; }
        public string? Arguments { get; set; }

        public FirmwareFileInfo[]? Files { get; set; }
    }

    public class FirmwareFileInfo
    {
        public string? Address { get; set; }
        public string? File { get; set; }
    }
}
