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
using System.Management;

using SystemInfoLibrary.Hardware;

using Microsoft.Win32;

namespace SystemInfoLibrary.OperatingSystem
{
    internal class WindowsOperatingSystemInfo : OperatingSystemInfo
    {
        public sealed override string Architecture
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT OSArchitecture FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                    return obj["OSArchitecture"].ToString();
                return "Unknown";
            }
        }

        public override string Name
        {
            get
            {
                var searcher = new ManagementObjectSearcher("SELECT Caption, ServicePackMajorVersion, ServicePackMinorVersion FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                    return $"{obj["Caption"]} SP{obj["ServicePackMajorVersion"]}.{obj["ServicePackMinorVersion"]}";
                return "Unknown";
            }
        }

        public override Version FrameworkVersion => Environment.Version;
        
        public override Version JavaVersion { get; }

        private HardwareInfo _hardware;

        public override HardwareInfo Hardware => _hardware ?? (_hardware = new WindowsHardwareInfo());


        public WindowsOperatingSystemInfo()
        {
            try
            {
                var javaVersion = Architecture == "x86"
                    ? (string) Utils.GetRegistryValue(Registry.LocalMachine, @"Software\JavaSoft\Java Runtime Environment", "CurrentVersion", "")
                    : (string) Utils.GetRegistryValue(Registry.LocalMachine, @"Software\Wow6432Node\JavaSoft\Java Runtime Environment", "CurrentVersion", "");
                JavaVersion = new Version(javaVersion);
            }
            catch { JavaVersion = new Version(0, 0); }
        }


        public override OperatingSystemInfo Update()
        {
            _hardware = null;

            return this;
        }
    }
}