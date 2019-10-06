using System;
using System.Management;

namespace SystemInfoLibrary.Hardware.GPU
{
    internal class WindowsGPUInfoNative : WindowsGPUInfo
    {
        private readonly ManagementBaseObject _win32_videoController;

        public override string Name => (String) _win32_videoController.GetPropertyValue("VideoProcessor");

        public override string Brand => (String) _win32_videoController.GetPropertyValue("Name");

        public override ulong MemoryTotal => (UInt32) _win32_videoController.GetPropertyValue("AdapterRAM");

        public WindowsGPUInfoNative(ManagementBaseObject win32_videoController)
        {
            _win32_videoController = win32_videoController;
        }
    }
}