using System;
using System.Globalization;
using System.Management;
using Microsoft.Win32;

namespace SystemInfoLibrary.Hardware.CPU
{
    internal class WindowsCPUInfoNative : WindowsCPUInfo
    {
        public override string Name
        {
            get
            {
                foreach (var obj in new ManagementObjectSearcher("SELECT Name FROM Win32_Processor").Get())
                    return Utils.FilterCPUName(obj["Name"].ToString());
                return "Unknown";
            }
        }

        public override string Brand
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Manufacturer FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return obj["Manufacturer"].ToString();
                return "Unknown";
            }
        }

        public override string Architecture
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Architecture FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return Enum.GetName(typeof(CPUArchitectureType), int.Parse(obj["Architecture"].ToString()));
                return "Unknown";
            }
        }

        public override int PhysicalCores
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT NumberOfCores FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return int.Parse(obj["NumberOfCores"].ToString(), CultureInfo.InvariantCulture);
                return 0;
            }
        }

        public override int LogicalCores
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT NumberOfLogicalProcessors FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return int.Parse(obj["NumberOfLogicalProcessors"].ToString(), CultureInfo.InvariantCulture);
                return 0;
            }
        }

        public override double Frequency => Convert.ToDouble(Utils.GetRegistryValue(Registry.LocalMachine, @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", 0));
    }
}