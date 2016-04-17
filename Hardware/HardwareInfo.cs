/*
 * Little Software Stats - .NET Library
 * Copyright (C) 2008-2012 Little Apps (http://www.little-apps.org)
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace SystemInfoLibrary.Hardware
{
    public abstract class HardwareInfo
    {
        public abstract string CPUName { get; }
        public abstract string CPUArchitecture { get; }
        public abstract string CPUBrand { get; }
        public abstract int CPUCores { get; }
        public abstract double CPUFrequency { get; }

        public abstract string GPUName { get; }
        public abstract string GPUArchitecture { get; }
        public abstract string GPUBrand { get; }
        public abstract string GPUResolution { get; }
        public abstract int GPURefreshRate { get; }
        public abstract ulong GPUMemoryTotal { get; }

        public abstract ulong RAMMemoryTotal { get; }
        public abstract ulong RAMMemoryFree { get; }
    }
}
