using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware.GPU
{
    internal class MacOSXGPUInfo : GPUInfo
    {
        private string _systemProfiler;
        private string SystemProfiler { get { return string.IsNullOrEmpty(_systemProfiler) ? (_systemProfiler = Utils.GetCommandExecutionOutput("system_profiler", "SPDisplaysDataType")) : _systemProfiler; } }

        public override string Name
        {
            get
            {
                var matches = new Regex(@"Processor Name\s*:\s*(.*)").Matches(SystemProfiler);
                var value = matches[0].Groups[1].Value;
                return string.IsNullOrEmpty(value) ? "Unknown" : value;
            }
        }
        public override string Brand { get { return "Unknown"; } }
        public override string Resolution { get { return "Unknown"; } }
        public override int RefreshRate { get { return 0; } }
        public override ulong MemoryTotal { get { return 0; } }
    }
}