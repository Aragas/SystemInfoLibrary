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
using System.Reflection;

using SystemInfoLibrary.Hardware;

namespace SystemInfoLibrary.OperatingSystem
{
    internal class UnixOperatingSystemInfo : OperatingSystemInfo
    {
        private HardwareInfo _hardware;
        public override HardwareInfo Hardware { get { return _hardware ?? (_hardware = new UnixHardwareInfo()); } }

        public override string Version { get { return Utils.GetCommandExecutionOutput("uname", "-rs"); } }

        public override int ServicePack { get { return 0; } }

        private Version _frameworkVersion;
        public override Version FrameworkVersion
        {
            get
            {
                if (_frameworkVersion == null)
                {
                    try
                    {
                        var type = Type.GetType("Mono.Runtime");

                        var invokeGetDisplayName = type != null ? type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static) : null;
                        var displayName = invokeGetDisplayName != null ? invokeGetDisplayName.Invoke(null, null) as string : null;
                        if (displayName != null)
                            _frameworkVersion = new Version(displayName.Substring(0, displayName.IndexOf(" ", StringComparison.Ordinal)));
                    }
                    catch { /* ignored */ }

                    if (_frameworkVersion == null)
                        _frameworkVersion = new Version(Environment.Version.Major, Environment.Version.Minor); // Just use CLR version
                }

                return _frameworkVersion;
            }
        }

        public override int FrameworkSP { get { return 0; } }

        private Version _javaVersion;
        public override Version JavaVersion
        {
            get
            {
                if (_javaVersion != null)
                    return _javaVersion;

                try
                {
                    var j = Utils.GetCommandExecutionOutput("java", "-version 2>&1").Split('\n');
                    j = j[0].Split('"');
                    _javaVersion = new Version(j[1]);
                }
                catch { _javaVersion = new Version(); }

                return _javaVersion;
            }
        }

        public override int Architecture
        {
            get 
            {
                try
                {
                    var arch = Utils.GetCommandExecutionOutput("uname", "-m");
                    if (arch.Contains("64") || arch.Contains("686"))
                        return 64;
                }
                catch { /* ignored */ }

                return 32;
            }
        }
    }
}
