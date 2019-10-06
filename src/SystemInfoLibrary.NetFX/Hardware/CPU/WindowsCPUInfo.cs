using System;
using System.Globalization;

using Microsoft.Win32;

namespace SystemInfoLibrary.Hardware.CPU
{
    internal class WindowsCPUInfo : CPUInfo
    {
        protected enum CPUArchitectureType
        {
            x86 = 0,
            MIPS = 1,
            Alpha = 2,
            PowerPC = 3,
            ARM = 5,
            ia64 = 6,
            x64 = 9
        };

        public override string Name => Utils.Win32_Processor["Name"];

        public override string Brand => Utils.Win32_Processor["Manufacturer"];

        public override string Architecture => Enum.GetName(typeof(CPUArchitectureType), int.Parse(Utils.Win32_Processor["Architecture"]));

        public override int PhysicalCores => int.Parse(Utils.Win32_Processor["NumberOfCores"], CultureInfo.InvariantCulture);

        public override int LogicalCores => int.Parse(Utils.Win32_Processor["NumberOfLogicalProcessors"], CultureInfo.InvariantCulture);

        public override double Frequency => Convert.ToDouble(Utils.GetRegistryValue(Registry.LocalMachine, @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", 0));
    }
}