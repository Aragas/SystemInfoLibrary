using System.Globalization;
using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware.RAM
{
    internal class UnixRAMInfo : RAMInfo
    {
        private string _ramInfo;
        private string RAM_Info { get { return string.IsNullOrEmpty(_ramInfo) ? (_ramInfo = Utils.GetCommandExecutionOutput("cat", "/proc/meminfo")) : _ramInfo; } }


        public override ulong Total
        {
            get
            {
                var matches = new Regex(@"MemTotal:\s*(\d+)").Matches(RAM_Info);
                ulong value;
				return ulong.TryParse(matches[0].Groups[1].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value) ? value : 0;
            }
        }
        public override ulong Free
        {
            get
            {
                var matches = new Regex(@"MemFree:\s*(\d+)").Matches(RAM_Info);
                ulong value;
				return ulong.TryParse(matches[0].Groups[1].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value) ? value : 0;
            }
        }
    }
}