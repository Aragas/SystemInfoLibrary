namespace SystemInfoLibrary.Hardware.GPU
{
    internal class WindowsGPUInfo : GPUInfo
    {
        protected enum GPUArchitectureType
        {
            Other = 1,
            Unknown = 2,
            CGA = 3,
            EGA = 4,
            VGA = 5,
            SVGA = 6,
            MDA = 7,
            HGC = 8,
            MCGA = 9,
            EightFiveOneFourA = 10,
            XGA = 11,
            LinearFrameBuffer = 12,
            PCEightNine = 160
        };

        public override string Name => Utils.Win32_VideoController["VideoProcessor"];

        public override string Brand => Utils.Win32_VideoController["Name"];

        /*
        public override string Resolution => $"{Utils.Win32_VideoController["CurrentHorizontalResolution"]}x{Utils.Win32_VideoController["CurrentVerticalResolution"]}";

        public override int RefreshRate => int.Parse(Utils.Win32_VideoController["CurrentRefreshRate"]);
        */

        public override ulong MemoryTotal => ulong.Parse(Utils.Win32_VideoController["AdapterRAM"]);
    }
}