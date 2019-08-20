using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Utility.Extension
{
    public static class StringExtensions
    {
        // Methods
        public static string AsNullIfEmpty(this string items)
        {
            return (string.IsNullOrEmpty(items) ? null : items);
        }

        public static string AsNullIfWhiteSpace(this string items)
        {
            return (string.IsNullOrWhiteSpace(items) ? null : items);
        }

      

        public static string CutString(this string str, int len)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            if (str.Length > len)
            {
                return str.Substring(0, len);
            }
            return str;
        }

        public static string DecodeBase64(this string str)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(str));
        }

        public static string Decrypt<Algorithm>(this string str, string key) where Algorithm : SymmetricAlgorithm
        {
            return str.Decrypt<Algorithm, UTF8Encoding>(key);
        }

        public static string Decrypt<Algorithm, StringEncoding>(this string str, string key)
            where Algorithm : SymmetricAlgorithm
            where StringEncoding : Encoding
        {
            Stream stream = new MemoryStream(Convert.FromBase64String(str));
            byte[] bytes = stream.Decrypt<Algorithm>(key.ToByteArray()).ToByteArray();
            return Activator.CreateInstance<StringEncoding>().GetString(bytes);
        }

        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "&#160;&#8230;");
        }

        public static string Ellipsize(this string text, int characterCount, string ellipsis)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }
            if ((characterCount < 0) || (text.Length <= characterCount))
            {
                return text;
            }
            return (Regex.Replace(text.Substring(0, characterCount + 1), @"\s+\S*$", "") + ellipsis);
        }

        public static string EncodeBase64(this string str)
        {
            return Convert.ToBase64String(str.ToByteArray<UTF8Encoding>());
        }

        public static string Encrypt<Algorithm>(this string str, string key) where Algorithm : SymmetricAlgorithm
        {
            return str.Encrypt<Algorithm, UTF8Encoding>(key);
        }

        public static string Encrypt<Algorithm, StringEncoding>(this string str, string key)
            where Algorithm : SymmetricAlgorithm
            where StringEncoding : Encoding
        {
            Stream stream = new MemoryStream(str.ToByteArray());
            return Convert.ToBase64String(stream.Encrypt<Algorithm>(key.ToByteArray()).ToByteArray());
        }

        public static string EncryptOneWay<Algorithm>(this string str) where Algorithm : HashAlgorithm
        {
            return str.EncryptOneWay<Algorithm, UTF8Encoding>();
        }

        public static string EncryptOneWay<Algorithm, StringEncoding>(this string str)
            where Algorithm : HashAlgorithm
            where StringEncoding : Encoding
        {
            byte[] bytes = Activator.CreateInstance<StringEncoding>().GetBytes(str);
            return BitConverter.ToString(Activator.CreateInstance<Algorithm>().ComputeHash(bytes)).Replace("-", "");
        }

        public static bool IsValidDate(this string input)
        {
            return (ValidateString(input, @"^[12]{1}(\d){3}[-][01]?(\d){1}[-][0123]?(\d){1}$") && (input.CompareTo("1753-01-01") >= 0));
        }

        public static bool IsValidEmail(this string input)
        {
            return ValidateString(input, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsValidEmail(this string input, string expression)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (string.IsNullOrEmpty(expression))
            {
                expression = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            }
            return ValidateString(input, expression);
        }

        public static bool IsValidMobile(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return ValidateString(input, "^0{0,1}(1[3-8])[0-9]{9}$");
        }

        public static bool IsValidNumber(this string input)
        {
            return ValidateString(input, "^[1-9]{1}[0-9]{0,9}$");
        }

        public static string MD5(this string text)
        {
            return text.EncryptOneWay<MD5CryptoServiceProvider, UTF8Encoding>().ToLower();
        }

        public static string RemoveTags(this string html)
        {
            return (string.IsNullOrEmpty(html) ? "" : Regex.Replace(html, "<[^<>]*>", "", RegexOptions.Singleline));
        }

        public static string SHA256(this string text)
        {
            return text.EncryptOneWay<SHA256Cng, UTF8Encoding>();
        }

        public static string SHA512(this string text)
        {
            return text.EncryptOneWay<SHA512Cng, UTF8Encoding>();
        }

        public static string[] Split(this string str, string strSplit)
        {
            if (str.IndexOf(strSplit) < 0)
            {
                return new string[] { str };
            }
            return Regex.Split(str, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        public static byte[] ToByteArray(this string str)
        {
            return str.ToByteArray<UTF8Encoding>();
        }

        public static byte[] ToByteArray<encoding>(this string str) where encoding : Encoding
        {
            return Activator.CreateInstance<encoding>().GetBytes(str);
        }

        public static string ToNoHtmlString(this string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }

        public static Stream ToStream(this string str)
        {
            return str.ToStream<UTF8Encoding>();
        }

        public static Stream ToStream<encoding>(this string str) where encoding : Encoding
        {
            return new MemoryStream(str.ToByteArray<encoding>());
        }

        public static string UrlDecode(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return HttpUtility.UrlDecode(str, Encoding.UTF8);
        }

        public static string UrlEncode(this string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            return HttpUtility.UrlEncode(url, Encoding.UTF8);
        }

        internal static bool ValidateString(string input, string expression)
        {
            Regex regex = new Regex(expression, RegexOptions.None);
            return regex.IsMatch(input);
        }
    }
}
