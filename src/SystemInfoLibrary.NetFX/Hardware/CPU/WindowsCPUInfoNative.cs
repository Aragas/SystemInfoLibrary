using System;
using System.Management;

namespace SystemInfoLibrary.Hardware.CPU
{
    internal class WindowsCPUInfoNative : WindowsCPUInfo
    {
        private readonly ManagementBaseObject _win32_processor;

        public override string Name => _win32_processor.GetPropertyValue("Name").ToString() ?? "Unknown";

        public override string Brand => _win32_processor.GetPropertyValue("Manufacturer").ToString() ?? "Unknown";

        public override string Architecture => Enum.GetName(typeof(CPUArchitectureType), int.Parse(_win32_processor.GetPropertyValue("Architecture").ToString() ?? "0"));

        public override int PhysicalCores => int.Parse(_win32_processor.GetPropertyValue("NumberOfCores").ToString() ?? "0");

        public override int LogicalCores => int.Parse(_win32_processor.GetPropertyValue("NumberOfLogicalProcessors").ToString() ?? "0");

        public override double Frequency => int.Parse(_win32_processor.GetPropertyValue("CurrentClockSpeed").ToString() ?? "0");
        //    Convert.ToDouble(Utils.GetRegistryValue(Microsoft.Win32.Registry.LocalMachine, @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", 0));

        public WindowsCPUInfoNative(ManagementBaseObject win32_processor)
        {
            _win32_processor = win32_processor;
        }
    }
}