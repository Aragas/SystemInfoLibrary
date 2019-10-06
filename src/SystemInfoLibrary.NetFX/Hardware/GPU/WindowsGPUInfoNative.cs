using System.Management;

namespace SystemInfoLibrary.Hardware.GPU
{
    internal class WindowsGPUInfoNative : WindowsGPUInfo
    {
        private readonly ManagementBaseObject _win32_videoController;

        public override string Name => _win32_videoController.GetPropertyValue("VideoProcessor").ToString() ?? "Unknown";

        public override string Brand => _win32_videoController.GetPropertyValue("Name").ToString() ?? "Unknown";

        /*
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
        */

        public override ulong MemoryTotal => ulong.Parse(_win32_videoController.GetPropertyValue("AdapterRAM").ToString() ?? "0") / 1024;

        public WindowsGPUInfoNative(ManagementBaseObject win32_videoController)
        {
            _win32_videoController = win32_videoController;
        }
    }
}