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

using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SystemInfoLibrary.Hardware
{
    internal class UnixHardwareInfo : HardwareInfo
    {
        public override string CpuName
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"(?:model name\s+:\s*)(?<ModelName>[\w \(\)@\.]*)");
                    var matches = regex.Matches(output);
                    return matches[0].Groups["ModelName"].Value;
                }
                catch { return "Unknown"; }
            }
        }
        public override string CpuBrand
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"(?:vendor_id\s+:\s*)(?<vendor>\w*)");
                    var matches = regex.Matches(output);
                    return matches[0].Groups[1].Value;
                }
                catch { return "Unknown"; }
            }
        }
        public override double CpuFrequency
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"(?:bogomips\s+:\s*)(?<bogomips>\w*)");
                    var matches = regex.Matches(output);
                    var bogomips = int.Parse(matches[0].Groups[1].Value);
                    return (double) bogomips / (double) CpuCores;
                }
                catch { return 0; }
            }
        }
        public override int CpuArchitecture
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"flags\s+\s:[\w\s]*");
                    var matches = regex.Matches(output);
                    var flags = matches[0].Groups[0].Value;
                    if (flags.Contains(" lm"))
                        return 64;
                }
                catch { /* ignored */ }

                return 32;
            }
        }
        public override int CpuCores
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/cpuinfo");
                    var regex = new Regex(@"(?:cpu cores\s+:\s*)(?<num>\w*)");
                    var matches = regex.Matches(output);
                    return int.Parse(matches[0].Groups[1].Value);
                }
                catch { return 1; } // There has to be at least 1 core, cause how would we be able reach this ???
            }
        }

        public override ulong MemoryTotal
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo");
                    var regex = new Regex(@"(?:MemTotal:\s*)(?<memtotal>\d+)");
                    var matches = regex.Matches(output);
                    return ulong.Parse(matches[0].Groups[1].Value);
                }
                catch { return 0; }
            }
        }
        public override ulong MemoryFree
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo");
                    var regex = new Regex(@"(?:MemFree:\s*)(?<memtotal>\d+)");
                    var matches = regex.Matches(output);
                    return ulong.Parse(matches[0].Groups[1].Value);
                }
                catch { return 0; }
            }
        }

        public override long DiskTotal
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("df", "-k");
                    var regex = new Regex(@"^/[\w/]*\s*(?<total>\d+)\s*(?<used>\d+)\s*(?<available>\d+)");
                    var matches = regex.Matches(output);
                    var total = matches.Cast<Match>().Sum(match => long.Parse(match.Groups["total"].Value));
                    return total / 1024; // Convert from KB -> MB
                }
                catch { return -1; }
            }
        }
        public override long DiskFree
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("df", "-B 1k");
                    var regex = new Regex(@"^/[\w/]*\s*(?<total>\d+)\s*(?<used>\d+)\s*(?<available>\d+)");
                    var matches = regex.Matches(output);
                    var total = matches.Cast<Match>().Sum(match => long.Parse(match.Groups["available"].Value));
                    return total / 1024; // Convert from KB -> MB
                }
                catch { return 0; }
            }
        }

        public override string ScreenResolution
        {
            get
            {
                try { return $"{Screen.PrimaryScreen.Bounds.Height}x{Screen.PrimaryScreen.Bounds.Width}"; }
                catch { return "800x600"; }
            }
        } 
    }
}
