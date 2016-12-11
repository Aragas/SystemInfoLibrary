#if UNITY_5
using UnityEngine;

namespace SystemInfoLibrary.Hardware.CPU
{
    internal class UnityCPUInfo : CPUInfo
    {
        public override string Name => SystemInfo.processorType;

        public override string Brand => "Unknown";

        public override string Architecture => "Unknown";

        public override int Cores => SystemInfo.processorCount;

        public override double Frequency => SystemInfo.processorFrequency;
    }
}
#endif