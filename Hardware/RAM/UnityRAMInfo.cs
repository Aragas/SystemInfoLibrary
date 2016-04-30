#if UNITY_5
using UnityEngine;

namespace SystemInfoLibrary.Hardware.RAM
{
    internal class UnityRAMInfo : RAMInfo
    {
        public override ulong Free { get { return 0; } }
        public override ulong Total { get { return (ulong) SystemInfo.systemMemorySize * 1024; } }
    }
}
#endif