using System;
using System.Runtime.InteropServices;

namespace August
{
    public class Crypt32
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptAcquireContext(
            ref IntPtr hProv,
            string pszContainer,
            string pszProvider,
            uint dwProvType,
            uint dwFlags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptCreateHash(
            IntPtr hProv,
            uint algId,
            IntPtr hKey,
            uint dwFlags,
            ref IntPtr phHash);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptHashData(
            IntPtr hHash,
            byte[] pbData,
            uint dataLen,
            uint flags);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptDeriveKey(
            IntPtr hProv,
            int Algid,
            IntPtr hBaseData,
            int flags,
            ref IntPtr phKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptEncrypt(
            IntPtr hKey,
            IntPtr hHash,
            int Final,
            uint dwFlags,
            byte[] pbData,
            ref uint pdwDataLen,
            uint dwBufLen);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptDecrypt(
            IntPtr hKey,
            IntPtr hHash,
            int Final,
            uint dwFlags,
            byte[] pbData,
            ref uint pdwDataLen);
    }
}
