using System;
using System.Text;

namespace August
{
    public class Data
    {
        public static string Encrypt(string inputString, string keyString = null)
        {
            return Convert.ToBase64String(Bytes.Encrypt(Encoding.UTF8.GetBytes(inputString), keyString)).Replace('+', ')').Replace('=', '(').Replace('/', '-');
        }
        public static string Decrypt(string inputString, string keyString = null)
        {
            return Encoding.UTF8.GetString(Data.Bytes.Decrypt(Convert.FromBase64String(inputString.Replace(')', '+').Replace('(', '=').Replace('-', '/')), keyString));
        }
        public class Bytes
        {
            public static byte[] Encrypt(byte[] InputData, string Key = null)
            {
                byte[] CryptKey = !String.IsNullOrEmpty(Key) ? Encoding.UTF8.GetBytes(Key) : new byte[] { 0x0 };
                byte[] CryptedData = new byte[InputData.Length];
                int KById = 0;
                for (int i = 0; i < CryptedData.Length; i++)
                {
                    if (CryptKey.Length == KById) KById = 0;
                    CryptedData[i] = (byte)(InputData[i] + i + CryptKey[KById]);
                    KById++;
                }
                Array.Reverse(CryptedData);
                return CryptedData;
            }
            public static byte[] Decrypt(byte[] InputData, string Key = null)
            {
                byte[] CryptKey = !String.IsNullOrEmpty(Key) ? Encoding.UTF8.GetBytes(Key) : new byte[] { 0x0 };
                byte[] DecryptedData = new byte[InputData.Length];
                int KById = 0;
                Array.Reverse(InputData);
                for (int i = 0; i < DecryptedData.Length; i++)
                {
                    if (CryptKey.Length == KById) KById = 0;
                    DecryptedData[i] = (byte)(InputData[i] - i - CryptKey[KById]);
                    KById++;
                }
                return DecryptedData;
            }
        }
    }
}