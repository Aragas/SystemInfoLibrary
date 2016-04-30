#if UNITY_5
using UnityEngine;

namespace SystemInfoLibrary.Hardware.CPU
{
    internal class UnityCPUInfo : CPUInfo
    {
        public override string Name { get { return SystemInfo.processorType; } }
        public override string Brand { get { return "Unknown"; } }
        public override string Architecture { get { return "Unknown"; } }
        public override int Cores { get { return SystemInfo.processorCount; } }
        public override double Frequency { get { return SystemInfo.processorFrequency; } }
    }
}
#endif