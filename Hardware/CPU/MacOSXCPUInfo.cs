namespace SystemInfoLibrary.Hardware.CPU
{
    internal class MacOSXCPUInfo : CPUInfo
    {
		public override string Name { get { return Utils.FilterCPUName(Utils.GetSysCtlPropertyString("machdep.cpu.brand_string")); } }
		public override string Brand { get { return Utils.GetSysCtlPropertyString("machdep.cpu.vendor"); } }
        public override string Architecture { get { return Utils.GetSysCtlPropertyInt32("hw.cpu64bit_capable") == 1 ? "x64" : "x86"; } }
        public override int Cores { get { return Utils.GetSysCtlPropertyInt32("hw.logicalcpu"); } }
		public override double Frequency { get { return (double) Utils.GetSysCtlPropertyInt64("hw.cpufrequency") / (double) 1024 / (double) 1024; } }
    }
}