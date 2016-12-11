using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware.GPU
{
    internal class UnixGPUInfo : GPUInfo
    {
        private string _glxinfo;
        private string Glxinfo => string.IsNullOrEmpty(_glxinfo) ? (_glxinfo = Utils.GetCommandExecutionOutput("glxinfo", "")) : _glxinfo;


        public override string Name
        {
            get
            {
                var matches = new Regex(@"OpenGL renderer string:\s*([A-Za-z0-9_ ()-.]*)").Matches(Glxinfo);
                var value = matches[0].Groups[1].Value;
                return string.IsNullOrEmpty(value) ? "Unknown" : value;
            }
        }

        public override string Brand
        {
            get
            {
                var matches = new Regex(@"OpenGL vendor string:\s*([A-Za-z0-9_ ()-.]*)").Matches(Glxinfo);
                var value = matches[0].Groups[1].Value;
                return string.IsNullOrEmpty(value) ? "Unknown" : value;
            }
        }

        public override string Resolution => "Unknown";

        public override int RefreshRate => 0;

        public override ulong MemoryTotal => 0;
    }
}