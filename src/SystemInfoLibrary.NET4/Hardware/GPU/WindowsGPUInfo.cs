using System.Management;

namespace SystemInfoLibrary.Hardware.GPU
{
    internal class WindowsGPUInfo : GPUInfo
    {
        private enum GPUArchitectureType
        {
            Other = 1,
            Unknown = 2,
            CGA = 3,
            EGA = 4,
            VGA = 5,
            SVGA = 6,
            MDA = 7,
            HGC = 8,
            MCGA = 9,
            EightFiveOneFourA = 10,
            XGA = 11,
            LinearFrameBuffer = 12,
            PCEightNine = 160
        };

        public override string Name
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT VideoProcessor FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return obj["VideoProcessor"].ToString();
                return "Unknown";
            }
        }

        public override string Brand
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return obj["Name"].ToString();
                return "Unknown";
            }
        }

        public override string Resolution
        {
            get
            {
                var searcher =
                    new ManagementObjectSearcher(
                        "SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return $"{obj["CurrentHorizontalResolution"]}x{obj["CurrentVerticalResolution"]}";
                return "Unknown";
            }
        }

        public override int RefreshRate
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT CurrentRefreshRate FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return int.Parse(obj["CurrentRefreshRate"].ToString());
                return 0;
            }
        }

        public override ulong MemoryTotal
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT AdapterRAM FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["AdapterRAM"].ToString())/1024;
                return 0;
            }
        }
    }
}