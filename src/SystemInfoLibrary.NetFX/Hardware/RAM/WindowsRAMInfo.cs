
namespace SystemInfoLibrary.Hardware.RAM
{
    internal class WindowsRAMInfo : RAMInfo
    {
        public override ulong Free => ulong.Parse(Utils.Win32_OperatingSystem["FreePhysicalMemory"]);

        public override ulong Total => ulong.Parse(Utils.Win32_OperatingSystem["TotalVisibleMemorySize"]);
    }
}