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
using System.Management;
using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace SystemInfoLibrary.Hardware
{
    internal enum CPUArchitectureType { x86 = 0, MIPS = 1, Alpha = 2, PowerPC = 3, ARM = 5, ia64 = 6, x64 = 9 };
    internal enum GPUArchitectureType
    {
        Other = 1, Unknown = 2, CGA = 3, EGA = 4, VGA = 5, SVGA = 6, MDA = 7, HGC = 8, MCGA = 9,
        EightFiveOneFourA = 10, XGA = 11, LinearFrameBuffer = 12, PCEightNine = 160
    };

    internal sealed class WindowsHardwareInfo : HardwareInfo
    {
        #region P/Invoke signatures
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX() { dwLength = (uint) Marshal.SizeOf(typeof (MEMORYSTATUSEX)); }
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
        #endregion

        public override string CPUName
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return Utils.FilterCPUName(obj["Name"].ToString());
                return string.Empty;
            }
        }
        public override string CPUArchitecture
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Architecture FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                {
                    var arc = int.Parse(obj["Architecture"].ToString());
                    return Enum.GetName(typeof(CPUArchitectureType), arc);
                }
                return string.Empty;
            }
        }
        public override string CPUBrand
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Manufacturer FROM Win32_Processor");
                foreach (var obj in searcher.Get())
                    return obj["Manufacturer"].ToString();
                return string.Empty;
            }
        }
        public override int CPUCores { get { return Environment.ProcessorCount; } }
        public override double CPUFrequency { get { return Convert.ToDouble(Utils.GetRegistryValue(Registry.LocalMachine, @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "~MHz", 0)); } }
        public override string GPUName
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT VideoProcessor FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return obj["VideoProcessor"].ToString();
                return string.Empty;
            }
        }
        public override string GPUArchitecture
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT VideoArchitecture FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                {
                    var arc = int.Parse(obj["VideoArchitecture"].ToString());
                    return Enum.GetName(typeof(GPUArchitectureType), arc);
                }
                return string.Empty;
            }
        }
        public override string GPUBrand
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return obj["Name"].ToString();
                return string.Empty;
            }
        }
        public override string GPUResolution
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return string.Format("{0}x{1}", obj["CurrentHorizontalResolution"], obj["CurrentVerticalResolution"]);
                return string.Empty;
            }
        }
        public override int GPURefreshRate
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT CurrentRefreshRate FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return int.Parse(obj["CurrentRefreshRate"].ToString());
                return 0;
            }
        }
        public override ulong GPUMemoryTotal
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT AdapterRAM FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["AdapterRAM"].ToString()) / 1024;
                return 0;
            }
        }

        private ulong _ramMemoryFree;
        public override ulong RAMMemoryFree { get { return _ramMemoryFree; } }
        private ulong _ramMemoryTotal;
        public override ulong RAMMemoryTotal { get { return _ramMemoryTotal; } }

        public WindowsHardwareInfo()
        {
            var memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                // Convert from bytes -> kilobytes
                _ramMemoryFree = memStatus.ullTotalPhys / 1024;
                _ramMemoryTotal = memStatus.ullAvailPhys / 1024;
            }
        }
    } 
}
