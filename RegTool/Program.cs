/*
 * RegTool
 * https://github.com/kit10us/regtool
 * Copyright (c) 2014, Kit10 Studios LLC
 *
 * RegTool is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * RegTool is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Unify.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RegTool
{
    class Program
    {
        static RegistryKey GetBase(string keyName)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(keyName, "^HKEY_LOCAL_MACHINE", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return Registry.LocalMachine;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(keyName, "^HKEY_CLASSES_ROOT", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return Registry.ClassesRoot;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(keyName, "^HKEY_CURRENT_USER", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return Registry.CurrentUser;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(keyName, "^HKEY_CURRENT_CONFIG", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return Registry.CurrentConfig;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(keyName, "^HKEY_USERS", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return Registry.Users;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(keyName, "^HKEY_PERFORMANCE_DATA", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return Registry.PerformanceData;
            }
            else
            {
                return null;
            }
        }

        static string GetSubKeyName(string fullKeyName)
        {
            return System.Text.RegularExpressions.Regex.Replace( fullKeyName, @"^(HKEY_[A-Za-z_]*\\)", "" );
        }

        /// <summary>
        /// Returns the textual represenation of the registry value type. Tpyically through the C# API,
        /// this will not match that which the reg.exe command returns (ex. String == REG_SZ).
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        static string GetRegistryValueTypeName(RegistryValueKind kind)
        {
            switch (kind)
            {
                case RegistryValueKind.Binary:
                    return "REG_BINARY";
                case RegistryValueKind.DWord:
                    return "REG_DWORD"; // Not count to endianess.
                case RegistryValueKind.ExpandString:
                    return "REG_EXPAND_SZ";
                case RegistryValueKind.MultiString:
                    return "REG_MULTI_SZ";
                case RegistryValueKind.None:
                    return "REG_NONE";
                case RegistryValueKind.QWord:
                    return "REG_QWORD"; // Not count to endianess.
                case RegistryValueKind.String:
                    return "REG_SZ";
                case RegistryValueKind.Unknown:
                default:
                    return "UNKNOWN";
            }
        }

        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.WriteLine(
                    "This version of RegTool.exe is only meant to be used for the query operations,\n" +
                    "which are relatively safe, and it's use mimics the official windows reg.exe tool.\n\n");
                System.Console.WriteLine(
                    "Visual Studio's commandline compiler scripts (batch files) rely on reg.exe to\n" +
                    " query setup information, such as install directories. Even though it is only performing\n" +
                    "readonly operations, it might not be enabled, by IT, for your use.\n" +
                    "To get around this issue, we have RegTool.exe. It supports the reg.exe query\n" +
                    " operation, and returns equivalent results.\n"
                    );
                    return 1;
            }
            else if (String.Equals("query", args[0], StringComparison.CurrentCultureIgnoreCase))
            {
                string keyName =  args[ 1 ];
                keyName = System.Text.RegularExpressions.Regex.Replace( keyName, "^HKLM", "HKEY_LOCAL_MACHINE" );
                keyName = System.Text.RegularExpressions.Regex.Replace( keyName, "^HKCU", "HKEY_CURRENT_USER" );
                string valueName = args[3];
                object result = Registry.GetValue( keyName, valueName, null );
                if ( result == null )
                {
                    System.Console.Error.WriteLine("ERROR: The system was unable to find the specified registry key or value.");
                    return 1;
                }
                else
                {
                    RegistryKey baseKey = GetBase( keyName );
                    string subKeyName = GetSubKeyName(keyName);
                    RegistryKey actualKey = baseKey.OpenSubKey(subKeyName);
                    RegistryValueKind kind = actualKey.GetValueKind(args[ 3 ]);
                    System.Console.WriteLine("    " + args[3] + "    " +  GetRegistryValueTypeName( kind ) + "    " + result + "");
                    return 0;
                }
            }
            return 1;
        }
    }
}
