﻿/*
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
using System.Linq;
using System.Text.RegularExpressions;

using SystemInfoLibrary.Hardware.CPU;
using SystemInfoLibrary.Hardware.GPU;
using SystemInfoLibrary.Hardware.RAM;

namespace SystemInfoLibrary.Hardware
{
    internal sealed class UnixHardwareInfo : HardwareInfo
    {
        private string _cpuInfo;
        private string CPU_Info => string.IsNullOrEmpty(_cpuInfo) ? (_cpuInfo = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo")) : _cpuInfo;


        private IList<CPUInfo> _CPUs;
        public override IList<CPUInfo> CPUs => _CPUs;

        private IList<GPUInfo> _GPUs;
        public override IList<GPUInfo> GPUs => _GPUs ?? (_GPUs = new List<GPUInfo> { new UnixGPUInfo() }); // No idea how to detect multiple GPUs

        private RAMInfo _RAM;
        public override RAMInfo RAM => _RAM ?? (_RAM = new UnixRAMInfo());


        public UnixHardwareInfo()
        {
            // -- CPU
            _cpuInfo = string.Empty;
            _CPUs = new List<CPUInfo>();

            var matches = new Regex(@"^\s*$", RegexOptions.Multiline).Split(CPU_Info).Where(val => !string.IsNullOrEmpty(val)).ToList();
            var procIndexes = matches.Select(match => int.Parse(new Regex(@"physical id\s*:\s*(?<pid>\d*)").Match(match).Groups["pid"].Value)).Distinct();

            foreach (var procIndex in procIndexes)
            {
                var cpuInfo = string.Join("",
                    matches.Where(match => int.Parse(new Regex(@"physical id\s*:\s*(?<pid>\d*)").Match(match).Groups["pid"].Value) == procIndex).ToArray());
                _CPUs.Add(new UnixCPUInfo(cpuInfo));
            }
            // -- CPU
        }
    }
}