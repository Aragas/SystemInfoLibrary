namespace SystemInfoLibrary.Hardware.GPU
{
    public abstract class GPUInfo
    {
        /// <summary>
        /// GPU name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// GPU vendor.
        /// </summary>
        public abstract string Brand { get; }

        /// <summary>
        /// Current resolution of the primary screen, in HxV format.
        /// </summary>
        public abstract string Resolution { get; }

        /// <summary>
        /// Current GPU Refrash Rate, in Hz.
        /// </summary>
        public abstract int RefreshRate { get; }

        /// <summary>
        /// Amount of total VRAM memory, in KB.
        /// </summary>
        public abstract ulong MemoryTotal { get; }
    }
}