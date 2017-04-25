using System;
using System.Management;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace August
{
    class Protection
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public static bool IsSniffing()
        {
            string[] TitleList = { "http analyzer", "charles", "fiddler", "Wireshark", "wpe pro" };
            string[] ProcessList = { "httpanalyzerstdv", "charles", "Fiddler", "wireshark", "wpe" };
            Process[] Processes = Process.GetProcesses();
            foreach (Process FdProcess in Processes)
            {
                for (int i = 0; i < TitleList.Length; i++)
                {
                    try
                    {
                        if (FdProcess.ProcessName.ToLower().Contains(ProcessList[i]) || FdProcess.MainWindowTitle.ToLower().Contains(TitleList[i]))
                            return true;
                    }
                    catch { }
                }
            }
            return false;
        }
    }
}