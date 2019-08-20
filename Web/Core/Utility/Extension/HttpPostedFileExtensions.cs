using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility.Extension
{
    public static class HttpPostedFileBaseExtensions
    {
        public static string To64String(this HttpPostedFileBase file)
        {
            if (file == null)
            {
                return string.Empty;
            }
            string FilePath = file.FileName;
            string FileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
            byte[] b = new byte[file.ContentLength];
            System.IO.Stream fs = (System.IO.Stream)file.InputStream;
            fs.Read(b, 0, file.ContentLength);
            return HttpUtility.UrlEncode(Convert.ToBase64String(b)).Replace("+", "%2B");
        }
    }
}
