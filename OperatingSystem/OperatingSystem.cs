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

using System;
using System.Threading;

using SystemInfoLibrary.Hardware;

namespace SystemInfoLibrary.OperatingSystem
{
    public abstract class OperatingSystemInfo
    {
        /// <summary>
        /// Could be 16-bit 32-bit, 64-bit, ARM.
        /// </summary>
        public abstract string Architecture { get; }
        /// <summary>
        /// Operating System name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Locale ID assigned by <a href="https://msdn.microsoft.com/en-us/goglobal/bb964664.aspx">Microsoft</a>, in DEC.
        /// </summary>
        public int LocaleID
        {
            get
            {
                try { return Thread.CurrentThread.CurrentCulture.LCID; }
                catch { return 1033; } // Just return 1033 (English - USA)
            }
        }
        
        /// <summary>
        /// .NET Framework version.
        /// </summary>
        public abstract Version FrameworkVersion { get; }
        public bool IsMono { get { return Type.GetType ("Mono.Runtime") != null; } }
        /// <summary>
        /// Java version.
        /// </summary>
        public abstract Version JavaVersion { get; }


        public abstract HardwareInfo Hardware { get; }


        public abstract OperatingSystemInfo Update();


        public static OperatingSystemInfo GetOperatingSystemInfo()
        {
#if UNITY_5
            return new UnityOperatingSystemInfo();
#endif

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return Utils.GetCommandExecutionOutput("uname", "").Contains("Darwin") 
                        ? new MacOSXOperatingSystemInfo()
                        : new UnixOperatingSystemInfo();
                case PlatformID.MacOSX:
                    return new MacOSXOperatingSystemInfo();
                default:
                    return new WindowsOperatingSystemInfo();
            }
        }
    }
}
