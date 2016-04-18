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
using System.Globalization;

namespace SystemInfoLibrary.Hardware
{
    internal class UnixHardwareInfo : HardwareInfo
    {
        public override string CPU_Name
        {
            get
            {
                try
                {
					var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
					var regex = new Regex(@"model name\s*:\s*(.*)");
                    var matches = regex.Matches(output);
                    return Utils.FilterCPUName(matches[0].Groups[1].Value);
                }
                catch { return "Unknown"; }
            }
        }
        public override string CPU_Brand
        {
            get
            {
                try
                {
					var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
					var regex = new Regex(@"vendor_id\s*:\s*(.*)");
                    var matches = regex.Matches(output);
                    return matches[0].Groups[1].Value;
                }
                catch { return "Unknown"; }
            }
        }
        public override string CPU_Architecture
        {
            get
            {
                try
                {
					var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
					var regex = new Regex(@"lags\s*:(.*)");
					var matches = regex.Matches(output);
					var flags = matches[0].Groups[1].Value;
					if (flags.Contains(" lm") || flags.Contains(" x86-64"))
						return "x64";
                }
                catch { /* ignored */ }

                return "x86";
            }
        }
        public override int CPU_Cores
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"cpu cores\s+:\s*(\d+)");
                    var matches = regex.Matches(output);
					return int.Parse(matches[0].Groups[1].Value, CultureInfo.InvariantCulture);
                }
                catch { return 1; } // There has to be at least 1 core, cause how would we be able reach this ???
            }
        }
        public override double CPU_Frequency
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
					var regex = new Regex(@"cpu MHz\s*:\s*([0-9]*(?:\.[0-9]+)?)");
                    var matches = regex.Matches(output);
					return double.Parse(matches[0].Groups[1].Value, CultureInfo.InvariantCulture);
                }
                catch { return 0; }
            }
        }

        public override string GPU_Name
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("glxinfo", "");
                    var regex = new Regex(@"OpenGL renderer string:\s*([A-Za-z0-9_ ()-.]*)");
                    var matches = regex.Matches(output);
                    return matches[0].Groups[1].Value;
                }
                catch { return "Unknown"; }
            }
        }
        public override string GPU_Architecture { get { return "Unknown"; } }
        public override string GPU_Brand
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("glxinfo", "");
                    var regex = new Regex(@"OpenGL vendor string:\s*([A-Za-z0-9_ ()-.]*)");
                    var matches = regex.Matches(output);
                    return matches[0].Groups[1].Value;
                }
                catch { return "Unknown"; }
            }
        }
        public override string GPU_Resolution { get { return "Unknown"; } }
        public override int GPU_RefreshRate { get { return 0; } }
        public override ulong GPU_MemoryTotal { get { return 0; } }

        public override ulong RAM_MemoryTotal
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo");
                    var regex = new Regex(@"MemTotal:\s*(\d+)");
                    var matches = regex.Matches(output);
					return ulong.Parse(matches[0].Groups[1].Value, CultureInfo.InvariantCulture);
                }
                catch { return 0; }
            }
        }
        public override ulong RAM_MemoryFree
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo");
                    var regex = new Regex(@"MemFree:\s*(\d+)");
                    var matches = regex.Matches(output);
					return ulong.Parse(matches[0].Groups[1].Value, CultureInfo.InvariantCulture);
                }
                catch { return 0; }
            }
        }
    }
}