#if UNITY_5
using UnityEngine;

namespace SystemInfoLibrary.Hardware.GPU
{
    public class UnityGPUInfo : GPUInfo
    {
        public override string Name { get { return SystemInfo.graphicsDeviceName; } }
        public override string Architecture { get { return "Unknown"; } }
        public override string Brand { get { return SystemInfo.graphicsDeviceVendor; } }
        public override string Resolution { get { return string.Format("{0}x{1}", Screen.currentResolution.width, Screen.currentResolution.height); } }
        public override int RefreshRate { get { return Screen.currentResolution.refreshRate; } }
        public override ulong MemoryTotal { get { return (ulong) SystemInfo.graphicsMemorySize * 1024; } }
    }
}
#endif