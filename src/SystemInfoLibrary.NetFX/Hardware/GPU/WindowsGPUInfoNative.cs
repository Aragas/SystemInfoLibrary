using System.Management;

namespace SystemInfoLibrary.Hardware.GPU
{
    internal class WindowsGPUInfoNative : WindowsGPUInfo
    {
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
                    return ulong.Parse(obj["AdapterRAM"].ToString()) / 1024;
                return 0;
            }
        }
    }
}