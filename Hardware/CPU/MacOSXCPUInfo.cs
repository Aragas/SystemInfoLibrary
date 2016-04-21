using System.Globalization;
using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware.CPU
{
    public class MacOSXCPUInfo : CPUInfo
    {
        private string _sysctl;
        private string Sysctl { get { return string.IsNullOrEmpty(_sysctl) ? (_sysctl = Utils.GetCommandExecutionOutput("sysctl", "-a hw")) : _sysctl; } }

        private string _systemProfiler;
        private string SystemProfiler { get { return string.IsNullOrEmpty(_systemProfiler) ? (_systemProfiler = Utils.GetCommandExecutionOutput("system_profiler", "SPHardwareDataType SPDisplaysDataType")) : _systemProfiler; } }


        public override string Name
        {
            get
            {
                var matches = new Regex(@"Processor Name\s*:\s*(.*)").Matches(SystemProfiler);
                var value = matches[0].Groups[1].Value;
                return string.IsNullOrEmpty(value) ? "Unknown" : value;
            }
        }
        public override string Brand { get { return "GenuineIntel"; } }
        public override string Architecture
        {
            get
            {
                var matches = new Regex(@"hw\.cpu64bit_capable\s*[:|=]\s*(\d+)").Matches(Sysctl);
                return matches[0].Groups[1].Value == "1" ? "x64" : "x86";
            }
        }
        public override int Cores
        {
            get
            {
                var matches = new Regex(@"hw\.logicalcpu\s*[:|=]\s*(\d+)").Matches(Sysctl);
                int value;
				return int.TryParse(matches[0].Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out value) ? value : 0;
            }
        }
        public override double Frequency
        {
            get
            {
                var matches = new Regex(@"hw\.cpufrequency\s*[:|=]\s*(\d+)").Matches(Sysctl);
                double value;
                return double.TryParse(matches[0].Groups[1].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value) ? value / 1024 / 1024 : 0;
            }
        }
    }
}