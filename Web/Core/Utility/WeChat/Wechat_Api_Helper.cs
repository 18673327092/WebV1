using Newtonsoft.Json;
using System;
using Utility.WeChat.Model;

namespace Utility.WeChat
{
    public class Wechat_Api_Helper
    {
        static Wechat_Api_Helper apiHelper = new Wechat_Api_Helper();
        public static Wechat_Api_Helper Single => apiHelper;
        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters(WxPayData unifiedOrderResult)
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());
            string parameters = jsApiParam.ToJson();
            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }

        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="model">传入对应的消息模型</param>
        /// <returns></returns>
        public OpenApiResult SendMsg(TemplateBase model)
        {
            if (string.IsNullOrEmpty(model.token))
            {
                throw new Exception("接收消息token不能为空");
            }
            if (string.IsNullOrEmpty(model.touser))
            {
                throw new Exception("接收消息的用户Openid不能为空");
            }
            if (string.IsNullOrEmpty(model.template_id))
            {
                throw new Exception("模板消息ID不能为空");
            }
            if (model.data == null)
            {
                throw new Exception("模板消息不能为空");
            }
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", model.token);
            try
            {
                var modelstr = JsonConvert.SerializeObject(model);
                var res = Senparc.Weixin.CommonAPIs.CommonJsonSend.Send<OpenApiResult>(model.token, url, model);
                return res;
            }
            catch (Exception e)
            {
                return new OpenApiResult() { Error_code = -1, Error_msg = e.Message };
            }
        }
    }
}
