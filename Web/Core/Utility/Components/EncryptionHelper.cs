using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Components
{
    public class EncryptionHelper
    {
        public static string StrKey = "U$erN@me";
        public static string StrIV = "P@$$W0rd";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeValue(string value)
        {
            byte[] Key_64 = new byte[8];
            byte[] Iv_64 = new byte[8];
            //key
            for (int i = 0; i < StrKey.Length; i++)
            {
                if (i < 8)
                {
                    Key_64[i] = Convert.ToByte(StrKey[i]);
                }
                else
                {
                    break;
                }
            }

            //iv
            for (int i = 0; i < StrIV.Length; i++)
            {
                if (i < 8)
                {
                    Iv_64[i] = Convert.ToByte(StrIV[i]);
                }
                else
                {
                    break;
                }
            }
            return Encode(value, Key_64, Iv_64);
        }

        //加密
        private static string Encode(string data, byte[] byKey, byte[] byIv)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIv), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cst);
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }

            catch (Exception x)
            {
                return x.Message;
            }
        }



        /// <获取解密后的字符串>
        /// 获取解密后的字符串
        /// </获取解密后的字符串>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string GetDecodeStr(string Str)
        {
            byte[] Key_64 = new byte[8];
            byte[] Iv_64 = new byte[8];

            //key
            for (int i = 0; i < StrKey.Length; i++)
            {
                if (i < 8)
                {
                    Key_64[i] = Convert.ToByte(StrKey[i]);
                }
                else
                {
                    break;
                }
            }

            //iv
            for (int i = 0; i < StrIV.Length; i++)
            {
                if (i < 8)
                {
                    Iv_64[i] = Convert.ToByte(StrIV[i]);
                }
                else
                {
                    break;
                }
            }

            string DecodeStr = Decode(Str, Key_64, Iv_64);

            return DecodeStr;
        }

        /// <解密>
        /// 解密
        /// </解密>
        /// <param name="data"></param>
        /// <param name="byKey"></param>
        /// <param name="byIv"></param>
        /// <returns></returns>
        public static string Decode(string data, byte[] byKey, byte[] byIv)
        {
            try
            {
                byte[] byEnc;

                byEnc = Convert.FromBase64String(data); //把需要解密的字符串转为8位无符号数组

                System.Security.Cryptography.DESCryptoServiceProvider cryptoProvider = new System.Security.Cryptography.DESCryptoServiceProvider();

                MemoryStream ms = new MemoryStream(byEnc);

                System.Security.Cryptography.CryptoStream cst = new System.Security.Cryptography.CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIv)
                    , System.Security.Cryptography.CryptoStreamMode.Read);

                StreamReader sr = new StreamReader(cst);

                return sr.ReadToEnd();

            }

            catch (Exception x)
            {

                return x.Message;

            }

        }


    }
}
