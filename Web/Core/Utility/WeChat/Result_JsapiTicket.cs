namespace Utility.WeChat
{
    public class Result_JsapiTicket : Result_Base
    {
        /// <summary>
        /// ticket
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }
}