/*
 * Little Software Stats - .NET Library
 * Copyright (C) 2008-2012 Little Apps (http://www.little-apps.org)
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Globalization;
using System.Management;

using Microsoft.Win32;

namespace SystemInfoLibrary.Hardware
{
    internal enum GPUArchitectureType
    {
        Other = 1, Unknown = 2, CGA = 3, EGA = 4, VGA = 5, SVGA = 6, MDA = 7, HGC = 8, MCGA = 9,
        EightFiveOneFourA = 10, XGA = 11, LinearFrameBuffer = 12, PCEightNine = 160
    };

    internal sealed class WindowsHardwareInfo : HardwareInfo
    {
        public override string CPU_Name
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return Utils.FilterCPUName(obj["Name"].ToString());
				return "Unknown";
            }
        }
        public override string CPU_Brand
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Manufacturer FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return obj["Manufacturer"].ToString();
				return "Unknown";
            }
        }
        public override string CPU_Architecture
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Architecture FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return Enum.GetName(typeof(CPUArchitectureType), int.Parse(obj["Architecture"].ToString()));
                return "Unknown";
            }
        }
        public override int CPU_Cores
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT NumberOfCores FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return int.Parse(obj["NumberOfCores"].ToString(), CultureInfo.InvariantCulture);
                return 0;
            }
        }
        public override double CPU_Frequency { get { return Convert.ToDouble(Utils.GetRegistryValue(Registry.LocalMachine, @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", 0)); } }
        public override string GPU_Name
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT VideoProcessor FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return obj["VideoProcessor"].ToString();
				return "Unknown";
            }
        }
        public override string GPU_Architecture
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT VideoArchitecture FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                {
                    var arc = int.Parse(obj["VideoArchitecture"].ToString());
                    return Enum.GetName(typeof(GPUArchitectureType), arc);
                }
				return "Unknown";
            }
        }
        public override string GPU_Brand
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return obj["Name"].ToString();
				return "Unknown";
            }
        }
        public override string GPU_Resolution
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return string.Format("{0}x{1}", obj["CurrentHorizontalResolution"], obj["CurrentVerticalResolution"]);
				return "Unknown";
            }
        }
        public override int GPU_RefreshRate
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT CurrentRefreshRate FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return int.Parse(obj["CurrentRefreshRate"].ToString());
                return 0;
            }
        }
        public override ulong GPU_MemoryTotal
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT AdapterRAM FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["AdapterRAM"].ToString()) / 1024;
                return 0;
            }
        }

        public override ulong RAM_MemoryFree
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["FreePhysicalMemory"].ToString());
                return 0;
            }
        }
        public override ulong RAM_MemoryTotal
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["TotalVisibleMemorySize"].ToString());
                return 0;
            }
        }
    } 
}
