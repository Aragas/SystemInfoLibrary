#if UNITY_5
using System;

using SystemInfoLibrary.Hardware;

using UnityEngine;

namespace SystemInfoLibrary.OperatingSystem
{
    internal class UnityOperatingSystemInfo : OperatingSystemInfo
    {
        public sealed override string Architecture { get { return "Unknown"; } }
        public override string Name { get { return SystemInfo.operatingSystem; } }

        public override Version FrameworkVersion { get { return Environment.Version; } }
        private Version _javaVersion;
        public override Version JavaVersion { get { return _javaVersion ?? (_javaVersion = new Version()); } }
        
        private HardwareInfo _hardware;
        public override HardwareInfo Hardware { get { return _hardware ?? (_hardware = new UnityHardwareInfo()); } }



        public override OperatingSystemInfo Update()
        {
            _hardware = null;

            return this;
        }
    } 
}
#endif
