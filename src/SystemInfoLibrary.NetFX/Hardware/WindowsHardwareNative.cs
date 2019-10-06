using System.Collections.Generic;
using System.Linq;
using System.Management;

using SystemInfoLibrary.Hardware.CPU;
using SystemInfoLibrary.Hardware.GPU;
using SystemInfoLibrary.Hardware.RAM;

namespace SystemInfoLibrary.Hardware
{
    internal sealed class WindowsHardwareInfoNative : WindowsHardwareInfo
    {
        private IList<CPUInfo> _CPUs;
        public override IList<CPUInfo> CPUs
        {
            get
            {
                if (_CPUs == null)
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                    {
                        _CPUs = (from ManagementBaseObject processor in searcher.Get() select (CPUInfo)new WindowsCPUInfoNative(processor)).ToList();
                    }
                }

                return _CPUs;
            }
        }

        private IList<GPUInfo> _GPUs;
        public override IList<GPUInfo> GPUs
        {
            get
            {
                if (_GPUs == null)
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                    {
                        _GPUs = (from ManagementBaseObject videoController in searcher.Get() select (GPUInfo)new WindowsGPUInfoNative(videoController)).ToList();
                    }
                }

                return _GPUs;
            }
        }

        private RAMInfo _RAM;
        public override RAMInfo RAM => _RAM ?? (_RAM = new WindowsRAMInfoNative());
    }
}
