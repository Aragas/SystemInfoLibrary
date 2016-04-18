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
        /// <summary>
        /// 
        /// </summary>
        public abstract string CPU_Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract string CPU_Brand { get; }
		/// <summary>
		/// Could be x86, x64, ARM.
		/// </summary>
        public abstract string CPU_Architecture { get; }
        /// <summary>
        /// Number of CPU cores.
        /// </summary>
        public abstract int CPU_Cores { get; }
        /// <summary>
        /// Current CPU frequency, average or of the first core.
        /// </summary>
        public abstract double CPU_Frequency { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string GPU_Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract string GPU_Architecture { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract string GPU_Brand { get; }
        /// <summary>
        /// Current resolution of the primary screen, in HxV format.
        /// </summary>
        public abstract string GPU_Resolution { get; }
        /// <summary>
        /// Current GPU Refrash Rate, in Hz.
        /// </summary>
        public abstract int GPU_RefreshRate { get; }
        /// <summary>
        /// Amount of total VRAM memory, in KB.
        /// </summary>
        public abstract ulong GPU_MemoryTotal { get; }

        /// <summary>
        /// Amount of total RAM memory, in KB.
        /// </summary>
        public abstract ulong RAM_MemoryTotal { get; }
        /// <summary>
        /// Amount of total free RAM memory, in KB.
        /// </summary>
        public abstract ulong RAM_MemoryFree { get; }
    }
}
