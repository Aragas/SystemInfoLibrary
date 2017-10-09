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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace SystemInfoLibrary
{
    public static class Utils
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
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        FileName = command,
                        Arguments = arguments,
                        //StandardOutputEncoding = Console.OutputEncoding,
                        //StandardErrorEncoding = Encoding.UTF8
                    }
                };

                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
                if (string.IsNullOrEmpty(output))
                    output = proc.StandardError.ReadToEnd();
                return output;
            }
            catch { return string.Empty; }
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


        [DllImport("libc", EntryPoint = "uname")]
        public static extern int UName(IntPtr unameStruct);

        #region PowerShell

        private static string GetPSUniqueFileName()
        {
            string fileName;
            do
            {
                fileName = Path.GetTempPath() + Guid.NewGuid() + ".ps1";
            } while (File.Exists(fileName));
            return fileName;
        }

        public static Dictionary<string, string> Win32_Processor => _win32_Processor ?? (_win32_Processor = GetWin32_Processor());
        public static Dictionary<string, string> _win32_Processor;
        private static Dictionary<string, string> GetWin32_Processor()
        {
            var script = "gwmi Win32_Processor | Format-List Name, Manufacturer, Architecture, NumberOfCores";
            var scriptFile = GetPSUniqueFileName();
            using (var scriptScream = File.CreateText(scriptFile))
                scriptScream.WriteLine(script);
            var output = GetCommandExecutionOutput("PowerShell.exe", $@"-Executionpolicy unrestricted {scriptFile}");
            return output.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split(':'))
                .ToDictionary(split => split[0].TrimStart().TrimEnd(), split => split[1].TrimStart().TrimEnd());
        }

        public static Dictionary<string, string> Win32_VideoController => _win32_VideoController ?? (_win32_VideoController = GetWin32_VideoController());
        public static Dictionary<string, string> _win32_VideoController;
        private static Dictionary<string, string> GetWin32_VideoController()
        {
            var script = "gwmi Win32_VideoController | Format-List VideoProcessor, Name, CurrentHorizontalResolution, CurrentVerticalResolution, CurrentRefreshRate, AdapterRAM";
            var scriptFile = GetPSUniqueFileName();
            using (var scriptScream = File.CreateText(scriptFile))
                scriptScream.WriteLine(script);
            var output = GetCommandExecutionOutput("PowerShell.exe", $@"-Executionpolicy unrestricted {scriptFile}");
            return output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split(':'))
                .ToDictionary(split => split[0].TrimStart().TrimEnd(), split => split[1].TrimStart().TrimEnd());
        }

        public static Dictionary<string, string> Win32_OperatingSystem => _win32_OperatingSystem ?? (_win32_OperatingSystem = GetWin32_OperatingSystem());
        public static Dictionary<string, string> _win32_OperatingSystem;
        private static Dictionary<string, string> GetWin32_OperatingSystem()
        {
            var script = "gwmi Win32_OperatingSystem | Format-List OSArchitecture, Caption, ServicePackMajorVersion, ServicePackMinorVersion, FreePhysicalMemory, TotalVisibleMemorySize";
            var scriptFile = GetPSUniqueFileName();
            using (var scriptScream = File.CreateText(scriptFile))
                scriptScream.WriteLine(script);
            var output = GetCommandExecutionOutput("PowerShell.exe", $@"-Executionpolicy unrestricted {scriptFile}");
            return output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split(':'))
                .ToDictionary(split => split[0].TrimStart().TrimEnd(), split => split[1].TrimStart().TrimEnd());
        }

        public static Dictionary<string, string> Win32_ComputerSystem => _win32_ComputerSystem ?? (_win32_ComputerSystem = GetWin32_ComputerSystem());
        public static Dictionary<string, string> _win32_ComputerSystem;
        private static Dictionary<string, string> GetWin32_ComputerSystem()
        {
            var script = "gwmi Win32_ComputerSystem | Format-List NumberOfProcessors";
            var scriptFile = GetPSUniqueFileName();
            using (var scriptScream = File.CreateText(scriptFile))
                scriptScream.WriteLine(script);
            var output = GetCommandExecutionOutput("PowerShell.exe", $@"-Executionpolicy unrestricted {scriptFile}");
            return output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split(':'))
                .ToDictionary(split => split[0].TrimStart().TrimEnd(), split => split[1].TrimStart().TrimEnd());
        }

        #endregion

        #region OS X

        [DllImport("libc", EntryPoint = "sysctlbyname")]
        private static extern int SysCtlByName([MarshalAs(UnmanagedType.LPStr)] string propName, IntPtr value, IntPtr oldLen, IntPtr newP, uint newLen);

        [DllImport("libc", EntryPoint = "getpagesize")]
        public static extern int GetPageSize();

        public static IntPtr GetSysCtlPropertyPtr(string propName)
        {
            try
            {
                var strLength = Marshal.AllocHGlobal(sizeof(int));
                SysCtlByName(propName, IntPtr.Zero, strLength, IntPtr.Zero, 0);
                var length = Marshal.ReadInt32(strLength);

                if (length == 0)
                {
                    Marshal.FreeHGlobal(strLength);
                    return IntPtr.Zero;
                }

                var strPtr = Marshal.AllocHGlobal(length);
                SysCtlByName(propName, strPtr, strLength, IntPtr.Zero, 0);

                Marshal.FreeHGlobal(strLength);

                return strPtr;
            }
            catch { return IntPtr.Zero; }
        }

        public static string GetSysCtlPropertyString(string propName)
        {
            var ptr = GetSysCtlPropertyPtr(propName);

            return ptr == IntPtr.Zero ? "Unknown" : Marshal.PtrToStringAnsi(ptr);
        }

        public static short GetSysCtlPropertyInt16(string propName)
        {
            var ptr = GetSysCtlPropertyPtr(propName);

            return ptr == IntPtr.Zero ? (short) 0 : Marshal.ReadInt16(ptr);
        }

        public static int GetSysCtlPropertyInt32(string propName)
        {
            var ptr = GetSysCtlPropertyPtr(propName);

            return ptr == IntPtr.Zero ? 0 : Marshal.ReadInt32(ptr);
        }

        public static long GetSysCtlPropertyInt64(string propName)
        {
            var ptr = GetSysCtlPropertyPtr(propName);

            return ptr == IntPtr.Zero ? (long) 0 : Marshal.ReadInt64(ptr);
        }

#endregion OS X
    }
}