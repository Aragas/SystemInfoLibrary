namespace SystemInfoLibrary.Hardware.GPU
{
    public class MacOSXGPUInfo : GPUInfo
    {
        public override string Name { get { return "Unknown"; } }
        public override string Architecture { get { return "Unknown"; } }
        public override string Brand { get { return "Unknown"; } }
        public override string Resolution { get { return "Unknown"; } }
        public override int RefreshRate { get { return 0; } }
        public override ulong MemoryTotal { get { return 0; } }
    }
}