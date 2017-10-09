
using System.Management;

namespace SystemInfoLibrary.Hardware.RAM
{
    internal class WindowsRAMInfoNative : WindowsRAMInfo
    {
        public override ulong Free
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["FreePhysicalMemory"].ToString());
                return 0;
            }
        }

        public override ulong Total
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                    return ulong.Parse(obj["TotalVisibleMemorySize"].ToString());
                return 0;
            }
        }
    }
}