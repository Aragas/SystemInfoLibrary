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
    internal class UnixHardwareInfo : HardwareInfo
    {
        public override string CPUName
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"model name\s+:\s*([A-Za-z0-9_ ()-.]*)");
                    var matches = regex.Matches(output);
                    return Utils.FilterCPUName(matches[0].Groups[1].Value);
                }
                catch { return "Unknown"; }
            }
        }
        public override string CPUArchitecture
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"flags\s*:([\w\s]*)");
                    var matches = regex.Matches(output);
                    var flags = matches[0].Value;
                    if (flags.Contains(" lm"))
                        return "64";
                }
                catch { /* ignored */ }

                return "32";
            }
        }
        public override string CPUBrand
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"vendor_id\s+:\s([A-Za-z0-9_ ()-.]*)");
                    var matches = regex.Matches(output);
                    return matches[0].Groups[1].Value;
                }
                catch { return "Unknown"; }
            }
        }
        public override int CPUCores
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"cpu cores\s+:\s*(\d+)");
                    var matches = regex.Matches(output);
                    return int.Parse(matches[0].Groups[1].Value);
                }
                catch { return 1; } // There has to be at least 1 core, cause how would we be able reach this ???
            }
        }
        public override double CPUFrequency
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"bogomips\s+:\s*([0-9]*(?:\.[0-9]+)?)");
                    var matches = regex.Matches(output);
                    var bogomips = double.Parse(matches[0].Groups[1].Value);
                    return bogomips / (double)CPUCores;
                }
                catch { return 0; }
            }
        }

        public override string GPUName
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
        public override string GPUArchitecture { get { return "Unknown"; } }
        public override string GPUBrand
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
        public override string GPUResolution { get { return "Unknown"; } }
        public override int GPURefreshRate { get { return 0; } }
        public override ulong GPUMemoryTotal { get { return 0; } }

        public override ulong RAMMemoryTotal
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo");
                    var regex = new Regex(@"MemTotal:\s*(\d+)");
                    var matches = regex.Matches(output);
                    return ulong.Parse(matches[0].Groups[1].Value);
                }
                catch { return 0; }
            }
        }
        public override ulong RAMMemoryFree
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo");
                    var regex = new Regex(@"MemFree:\s*(\d+)");
                    var matches = regex.Matches(output);
                    return ulong.Parse(matches[0].Groups[1].Value);
                }
                catch { return 0; }
            }
        }
    }
}