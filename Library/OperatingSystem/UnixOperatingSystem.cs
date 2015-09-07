﻿/*
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

namespace LittleSoftwareStats.OperatingSystem
{
    internal class UnixOperatingSystem : OperatingSystem
    {
        public UnixOperatingSystem()
        {
        }

        Hardware.Hardware _hardware;
        public override Hardware.Hardware Hardware
        {
            get
            {
                if (_hardware == null)
                    _hardware = new Hardware.UnixHardware();
                return _hardware;
            }
        }

        public override string Version
        {
            get { return Utils.GetCommandExecutionOutput("uname", "-rs"); }
        }

        public override int ServicePack
        {
            get { return 0; }
        }

        private Version _frameworkVersion;
        public override Version FrameworkVersion
        {
            get
            {
                if (this._frameworkVersion == null)
                {
                    try
                    {
                        Type type = Type.GetType("Mono.Runtime");
                        if (type != null)
                        {
                            MethodInfo invokeGetDisplayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                            if (invokeGetDisplayName != null)
                            {
                                string displayName = invokeGetDisplayName.Invoke(null, null) as string;
                                this._frameworkVersion = new Version(displayName.Substring(0, displayName.IndexOf(" ")));
                            }
                        }
                    }
                    catch { }

                    if (this._frameworkVersion == null)
                    {
                        // Just use CLR version
                        this._frameworkVersion = new Version(Environment.Version.Major, Environment.Version.Minor);
                    }
                }

                return this._frameworkVersion;
            }
        }

        public override int FrameworkSP
        {
            get { return 0; }
        }

        private Version _javaVersion;
        public override Version JavaVersion
        {
            get
            {
                if (this._javaVersion == null)
                {
                    try
                    {
                        string[] j = Utils.GetCommandExecutionOutput("java", "-version 2>&1").Split('\n');
                        j = j[0].Split('"');
                        this._javaVersion = new Version(j[1]);
                    }
                    catch
                    {
                        this._javaVersion = new Version();
                    }
                }

                return this._javaVersion;
            }
        }

        public override int Architecture
        {
            get 
            {
                try
                {
                    string arch = Utils.GetCommandExecutionOutput("uname", "-m");
                    if (arch.Contains("64") || arch.Contains("686"))
                        return 64;
                }
                catch { }

                return 32;
            }
        }
    }
}
