using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware.GPU
{
    // TODO: try OpenGL/CL to get the info.
    internal class MacOSXGPUInfo : GPUInfo
    {
        private string _systemProfiler;
        private string SystemProfiler => string.IsNullOrEmpty(_systemProfiler) ? (_systemProfiler = Utils.GetCommandExecutionOutput("system_profiler", "SPDisplaysDataType")) : _systemProfiler;


        public override string Name
        {
            get
            {
                var matches = new Regex(@"Processor Name\s*:\s*(.*)").Matches(SystemProfiler);
                var value = matches[0].Groups[1].Value;
                return string.IsNullOrEmpty(value) ? "Unknown" : value;
            }
        }

        public override string Brand => "Unknown";

        public override string Resolution => "Unknown";

        public override int RefreshRate => 0;

        public override ulong MemoryTotal => 0;
    }
}