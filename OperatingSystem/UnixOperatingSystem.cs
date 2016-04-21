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
        private string _unameM;
        private string UnameM { get { return string.IsNullOrEmpty(_unameM) ? (_unameM = Utils.GetCommandExecutionOutput("uname", "-m")) : _unameM; } }

        private string _unameRS;
        private string UnameRS { get { return string.IsNullOrEmpty(_unameRS) ? (_unameRS = Utils.GetCommandExecutionOutput("uname", "-rs")) : _unameRS; } }

        private string _java;
        private string Java { get { return string.IsNullOrEmpty(_java) ? (_java = Utils.GetCommandExecutionOutput("java", "-version")) : _java; } }


        public override string Architecture
        {
            get
            {
                if (UnameM.Contains("i386") || UnameM.Contains("i686"))
                    return "32-bit";
                if (UnameM.Contains("x86_64"))
                    return "64-bit";

                return "Unknown";
            }
        }
		public override string Name { get { return UnameRS.Replace("\n", ""); } }

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
                    var regex = new Regex(@"java version\s*""(.*)""");
                    var matches = regex.Matches(Java);
                    return new Version(matches[0].Groups[1].Value.Replace("_", "."));
                }
                catch { return new Version(); }
            }
        }
			
		private HardwareInfo _hardware;
		public override HardwareInfo Hardware { get { return _hardware ?? (_hardware = new UnixHardwareInfo()); } }

        public override OperatingSystemInfo Update()
        {
            _hardware = null;

            _unameM = Utils.GetCommandExecutionOutput("uname", "-m");
            _unameRS = Utils.GetCommandExecutionOutput("uname", "-rs");
            _java = Utils.GetCommandExecutionOutput("java", "-version");

            return this;
        }
    }
}
