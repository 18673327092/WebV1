using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.WeChat
{
    public class Wechat_Unifiedorder_Input
    {
        public string Token { get; set; }
        public string OpenID { get; set; }
        public string TradeNo { get; set; }
        public int TotalFee { get; set; }
    }
}
