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

using System.Diagnostics;

using Microsoft.Win32;

namespace SystemInfoLibrary
{
    internal static class Utils
    {
        public static string FilterCPUName(string name)
        {
            return name
                .Replace("(TM)", "")
                .Replace("(tm)", "")
                .Replace("(R)", "")
                .Replace("(r)", "");
        }

        public static string GetCommandExecutionOutput(string command, string arguments)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = command,
                    Arguments = arguments
                }
            };

            proc.Start();

            var output = proc.StandardOutput.ReadToEnd();
            if (string.IsNullOrEmpty(output))
                output = proc.StandardError.ReadToEnd();
            return output;
        }

        public static object GetRegistryValue(RegistryKey regRoot, string regPath, string valueName, object defaultValue = null)
        {
            var value = defaultValue;

            using (var regKey = regRoot.OpenSubKey(regPath))
            {
                if (regKey != null)
                    value = defaultValue != null ? regKey.GetValue(valueName, defaultValue) : regKey.GetValue(valueName);
            }

            return value;
        }

        #region MacOSX Functions
        private static string _systemProfiler;
        public static string SystemProfilerCommandOutput
        {
            get
            {
                if (string.IsNullOrEmpty(_systemProfiler))
                    _systemProfiler = GetCommandExecutionOutput("system_profiler", "");
                return _systemProfiler;
            }
        }

        private static string _sysctl;
        public static string SysctlCommandOutput
        {
            get
            {
                if (string.IsNullOrEmpty(_sysctl))
                    _sysctl = GetCommandExecutionOutput("sysctl", "-a hw");
                return _sysctl;
            }
        }
        #endregion
    }
}
