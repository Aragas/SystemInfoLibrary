using System.Globalization;
using System.Text.RegularExpressions;

namespace SystemInfoLibrary.Hardware.RAM
{
    public class MacOSXRAMInfo : RAMInfo
    {
        private string _sysctl;
        private string Sysctl { get { return string.IsNullOrEmpty(_sysctl) ? (_sysctl = Utils.GetCommandExecutionOutput("sysctl", "-a hw")) : _sysctl; } }


        public override ulong Total
        {
            get
            {
                var matches = new Regex(@"hw\.memsize\s*[:|=]\s*(\d+)").Matches(Sysctl);
                ulong value;
                return ulong.TryParse(matches[0].Groups[1].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value) ? value / 1024 : 0;
            }
        }
        public override ulong Free { get { return 0; } }
    }
}
