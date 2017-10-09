using System.Collections.Generic;
using System.Globalization;
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
                    foreach (var obj in new ManagementObjectSearcher("SELECT NumberOfProcessors FROM Win32_ComputerSystem").Get())
                    {
                        if (int.TryParse(obj["NumberOfProcessors"].ToString(), 0, CultureInfo.InvariantCulture, out var value))
                        {
                            if (value == 1)
                                _CPUs = new List<CPUInfo> { new WindowsCPUInfoNative() };
                            else
                                _CPUs = new List<CPUInfo> { new WindowsCPUInfoNative() }; // Well, I don't know what we can do here.
                        }
                    }

                    if (_CPUs == null)
                        throw new ManagementException("Could not get 'NumberOfProcessors' from 'Win32_ComputerSystem'");
                }

                return _CPUs;
            }
        }

        private IList<GPUInfo> _GPUs;
        public override IList<GPUInfo> GPUs => _GPUs ?? (_GPUs = new List<GPUInfo> { new WindowsGPUInfoNative() });
        // No idea how to detect multiple GPUs

        private RAMInfo _RAM;
        public override RAMInfo RAM => _RAM ?? (_RAM = new WindowsRAMInfoNative());
    }
}
