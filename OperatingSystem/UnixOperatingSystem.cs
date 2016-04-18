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
using System.Text.RegularExpressions;

using SystemInfoLibrary.Hardware;

namespace SystemInfoLibrary.OperatingSystem
{
    internal class UnixOperatingSystemInfo : OperatingSystemInfo
    {
        public override string Architecture
        {
            get 
            {
                try
                {
                    var arch = Utils.GetCommandExecutionOutput("uname", "-m");
                    if (arch.Contains("i386") || arch.Contains("i686"))
                        return "32-bit";
                    if (arch.Contains("x86_64"))
                        return "64-bit";
                }
                catch { /* ignored */ }

                return "Unknown";
            }
        }
		public override string Name { get { return Utils.GetCommandExecutionOutput("uname", "-rs").Replace("\n", ""); } }

        public override Version FrameworkVersion
        {
            get
            {
				try
				{
					if (IsMono) 
					{
						var type = Type.GetType("Mono.Runtime");

						var invokeGetDisplayName = type != null ? type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static) : null;
						var displayName = invokeGetDisplayName != null ? invokeGetDisplayName.Invoke(null, null) as string : null;
						if (displayName != null)
							return new Version(displayName.Substring(0, displayName.IndexOf(" ", StringComparison.Ordinal)));
					}
				}
				catch { /* ignored */ }

				return new Version(Environment.Version.Major, Environment.Version.Minor); 
            }
        }
        public override Version JavaVersion
        {
            get
            {
                try
                {
                    var output = Utils.GetCommandExecutionOutput("java", "-version");
                    var regex = new Regex(@"java version\s*""(.*)""");
                    var matches = regex.Matches(output);
                    return new Version(matches[0].Groups[1].Value.Replace("_", "."));
                }
                catch { return new Version(); }
            }
        }

        private readonly HardwareInfo _hardware = new UnixHardwareInfo();
        public override HardwareInfo Hardware { get { return _hardware; } }
    }
}
