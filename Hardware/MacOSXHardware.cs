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

using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware
{
    internal class MacOSXHardwareInfo : UnixHardwareInfo
    {
        public override string CpuName 
        {
            get {
                try
                {
                    var regex = new Regex(@"Processor Name\s*:\\s*(?<processor>[\w\s\d\.]+)");
                    var matches = regex.Matches(Utils.SystemProfilerCommandOutput);
                    return matches[0].Groups["processor"].Value;
                }
                catch { return "Generic"; }
            }
        }
        public override int CpuArchitecture
        {
            get
            {
                var regex = new Regex(@"hw\.cpu64bit_capable\s*(:|=)\s*(?<capable>\d+)");
                var matches = regex.Matches(Utils.SysctlCommandOutput);
                return matches[0].Groups["cpus"].Value == "1" ? 64 : 32;
            }
        }
        public override int CpuCores
        {
            get 
            {
                var regex = new Regex(@"hw\.availcpu\s*(:|=)\s*(?<cpus>\d+)");
                var matches = regex.Matches(Utils.SysctlCommandOutput);
                return int.Parse(matches[0].Groups["cpus"].Value);
            }
        }
        public override string CpuBrand => "GenuineIntel";
        public override double CpuFrequency 
        {
            get
            {
                var regex = new Regex(@"hw\.cpufrequency\s*(:|=)\s*(?<cpu_frequency>\d+)");
                var matches = regex.Matches(Utils.SysctlCommandOutput);
                return double.Parse(matches[0].Groups["cpu_frequency"].Value) / 1024 / 1024; // Convert from B -> MB
            }
        }

        public override ulong MemoryTotal
        {
            get 
            {
                var regex = new Regex(@"hw\.memsize\s*(:|=)\s*(?<memory>\d+)");
                var matches = regex.Matches(Utils.SysctlCommandOutput);
                return ulong.Parse(matches[0].Groups["memory"].Value) / 1024; // Convert from B -> KB
            }
        }
    }
}
