using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;

namespace WizzAir_Checking.Utils
{
    public static class GeneralUtils
    {
        public static string GetCurSecond()
        {
            string second = DateTime.Now.Second.ToString();
            return (second.Length == 1) ? "0" + second : second;
        }

        public static string GetCurMinute()
        {
            string minute = DateTime.Now.Minute.ToString();
            return (minute.Length == 1) ? "0" + minute : minute;
        }

        public static string GetCurHour()
        {
            string hour = DateTime.Now.Hour.ToString();
            return (hour.Length == 1) ? "0" + hour : hour;
        }

        public static string GetCurDay()
        {
            string year = DateTime.Now.Day.ToString();
            return (year.Length == 1) ? "0" + year : year;
        }

        public static string GetCurMonth()
        {
            string month = DateTime.Now.Month.ToString();
            return (month.Length == 1) ? "0" + month : month;
        }

        public static int? ExtractNumberBeginningOfString(string str, char decimalSeparator)
        {
            List<char> list = new List<char>();
            int? result = null;
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    char currChar = str[i];
                    if (!char.IsNumber(currChar))
                    {
                        if (currChar != decimalSeparator)
                            if (result != null)
                                break;
                        continue;
                    }

                    list.Add(currChar);
                    string tempResult = new string(list.ToArray());
                    result = int.Parse(tempResult);
                }
            }

            return result;
        }

        public static int? ExtractNumberEndOfString(string str, char decimalSeparator)
        {
            Stack<char> stack = new Stack<char>();
            int? result = null;
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
            {
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    char currChar = str[i];
                    if (!char.IsNumber(currChar))
                    {
                        if(currChar != decimalSeparator)
                            if (result != null)
                                break;
                        continue;
                    }

                    stack.Push(currChar);
                    string tempResult = new string(stack.ToArray());
                    result = int.Parse(tempResult);
                }
            }

            return result;
        }

        public static void KillProcessByPartialName(string name)
        {
            List<Process> processes = GetProcessesByPartialName(name);
            foreach (Process process in processes)
            {
                try { KillProcessAndChildren(process.Id); } catch { }
                try { process.Close(); } catch { }
                try { process.CloseMainWindow(); } catch { }
            }
        }

        private static List<Process> GetProcessesByPartialName(string name)
        {
            List<Process> userProcesses = new List<Process>();
            List<Process> usersProcesses = Process.GetProcesses().Where(
                proc =>
                {
                    try
                    {
                        return proc.ProcessName.ToLowerInvariant().Contains(name.ToLowerInvariant());
                    }
                    catch
                    {
                        return false;
                    }
                }).ToList();
            foreach (Process proc in usersProcesses)
            {
                string owner = GetProcessOwner(proc.Id);
                if (owner == $"{Environment.UserDomainName}\\{Environment.UserName}")
                {
                    userProcesses.Add(proc);
                }
            }

            return userProcesses;
        }

        private static string GetProcessOwner(int processId)
        {
            try
            {
                string query = "Select * From Win32_Process Where ProcessID = " + processId;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();

                foreach (ManagementObject obj in processList)
                {
                    string[] argList = { string.Empty, string.Empty };
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {
                        // return DOMAIN\user
                        return $"{argList[1]}\\{argList[0]}";
                    }
                }
            }
            catch
            {
                // ignored
            }

            return "NO OWNER";
        }

        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
                return;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }

            try
            {
                Process proc = Process.GetProcessById(pid);
                //Process.Start("taskkill", $"/F /IM [{proc.ProcessName}].exe");
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }

        public static void ClearAllIEStoredData()
        {
            // Temporary Internet Files
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 8");
            Thread.Sleep(3000);

            // Cookies
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2");
            Thread.Sleep(3000);

            // History
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 1");
            Thread.Sleep(3000);

            // Form(Data)
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 16");
            Thread.Sleep(3000);

            // Passwords
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 32");
            Thread.Sleep(3000);

            // Delete(All)
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
            Thread.Sleep(3000);

            // Delete All – Also delete files and settings stored by add-ons 
            Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351");
            Thread.Sleep(3000);
        }
    }
}
