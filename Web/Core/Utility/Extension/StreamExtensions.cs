using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Extension
{
    public static class StreamExtensions
    {
        // Methods
        public static Stream Decrypt<Algorithm>(this Stream stream, byte[] key) where Algorithm : SymmetricAlgorithm
        {
            Algorithm local = Activator.CreateInstance<Algorithm>();
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(key, null);
            local.Key = bytes.GetBytes(local.KeySize / 8);
            local.GenerateIV();
            local.IV = bytes.GetBytes(local.IV.Length);
            return new CryptoStream(stream, local.CreateDecryptor(), CryptoStreamMode.Read);
        }

        public static Stream Encrypt<Algorithm>(this Stream stream, byte[] key) where Algorithm : SymmetricAlgorithm
        {
            Algorithm local = Activator.CreateInstance<Algorithm>();
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(key, null);
            local.Key = bytes.GetBytes(local.KeySize / 8);
            local.GenerateIV();
            local.IV = bytes.GetBytes(local.IV.Length);
            return new CryptoStream(stream, local.CreateEncryptor(), CryptoStreamMode.Read);
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] buffer = new byte[0x3e8];
            int count = 1;
            using (MemoryStream stream2 = new MemoryStream())
            {
                while (count > 0)
                {
                    count = stream.Read(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        stream2.Write(buffer, 0, count);
                    }
                }
                return stream2.ToArray();
            }
        }
    }
}
