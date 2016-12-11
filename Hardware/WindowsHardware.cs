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

using System.Collections.Generic;
using System.Globalization;
using System.Management;

using SystemInfoLibrary.Hardware.CPU;
using SystemInfoLibrary.Hardware.GPU;
using SystemInfoLibrary.Hardware.RAM;

namespace SystemInfoLibrary.Hardware
{
    internal sealed class WindowsHardwareInfo : HardwareInfo
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
                        int value;
                        if (int.TryParse(obj["NumberOfProcessors"].ToString(), 0, CultureInfo.InvariantCulture, out value))
                        {
                            if (value == 1)
                                _CPUs = new List<CPUInfo> { new WindowsCPUInfo() };
                            else
                                _CPUs = new List<CPUInfo> { new WindowsCPUInfo() }; // Well, I don't know what we can do here.
                        }
                    }

                    if (_CPUs == null)
                        throw new ManagementException("Could not get 'NumberOfProcessors' from 'Win32_ComputerSystem'");
                }

                return _CPUs;
            }
        }

        private IList<GPUInfo> _GPUs;
        public override IList<GPUInfo> GPUs => _GPUs ?? (_GPUs = new List<GPUInfo> { new WindowsGPUInfo() });
        // No idea how to detect multiple GPUs

        private RAMInfo _RAM;
        public override RAMInfo RAM => _RAM ?? (_RAM = new WindowsRAMInfo());
    }
}