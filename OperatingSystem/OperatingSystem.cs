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

namespace LittleSoftwareStats.OperatingSystem
{
    internal abstract class OperatingSystem
    {
        public abstract Version FrameworkVersion { get; }
        public abstract int FrameworkSP { get; }
        public abstract Version JavaVersion { get; }

        public abstract int Architecture { get; }
        public abstract string Version { get; }
        public abstract int ServicePack { get; }

        public abstract Hardware.Hardware Hardware { get; }

        public int Lcid
        {
            get
            {
                try { return Thread.CurrentThread.CurrentCulture.LCID; }
                catch { return 1033; } // Just return 1033 (English - USA)
            }
        }

        public static OperatingSystem GetOperatingSystemInfo()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return new UnixOperatingSystem();
                case PlatformID.MacOSX:
                    return new MacOsxOperatingSystem();
                default:
                    return new WindowsOperatingSystem();
            }
        }
    }
}
