using System;

namespace Utility.WeChat
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}