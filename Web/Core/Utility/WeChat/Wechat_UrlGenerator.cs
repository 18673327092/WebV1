using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.WeChat
{
    public class Wechat_UrlGenerator
    {
        #region 基本功能

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <param name="grant_type">获取access_token填写client_credential</param>
        /// <param name="appid">公众号唯一标识</param>
        /// <param name="appsecret">公众号唯一凭证密钥，即appsecret</param>
        public string Signature_GetAccessTokenUrl(string appid, string appsecret, string grant_type = "client_credential")
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}", grant_type, appid, appsecret);
        }

     

        /// <summary>
        /// 获取微信服务器IP地址列表
        /// <param name="access_token">普通access_token</param>
        /// </summary>
        public string Signature_GetIpListUrl(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}", access_token);
        }

        #endregion 基本功能

        #region 自定义菜单

        public string CustomMenu_GetUrl(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token);
        }

        public string CustomMenu_CreateUrl(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);
        }
        #endregion

        #region 消息管理

        /// <summary>
        /// 获取所有私有模板
        /// </summary>
        /// <param name="access_token">普通access_token</param>
        /// <returns></returns>
        public string TemplateMessage_GetAllPrivateTemplate(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}", access_token);
        }

        /// <summary>
        /// 发送模板消息，POST
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string TemplateMessage_Send(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", access_token);
        }

        /// <summary>
        /// 客服接口-发消息，POST
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string KF_SendMessage(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", access_token);
        }

        #endregion 消息管理

        #region 用户管理

        /// <summary>
        /// 获取用户信息Url
        /// </summary>
        /// <param name="appid">公众号唯一标识</param>
        /// <param name="appsecret">公众号唯一凭证密钥，即appsecret</param>
        /// <param name="code"></param>
        public string GetAccessInfoUrl(string appid, string appsecret, string code)
        {
            return string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, appsecret, code);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string User_GetUserinfoUrl(string access_token, string openid)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);

        }

        #endregion 用户管理

        #region 账号管理

        /// <summary>
        /// 生成场景参数二维码
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Account_GetCreateQRSceneUrl(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", access_token);
        }

        ///// <summary>
        ///// 获取二维码图片
        ///// </summary>
        ///// <param name="ticket"></param>
        ///// <returns></returns>
        //public string Account_GetQRSceneUrl(string ticket)
        //{
        //    return string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}",  .UrlEncode(ticket));
        //}

        /// <summary>
        /// 获取短地址
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Account_GetShortUrl(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", access_token);
        }

        #endregion 账号管理

        #region 网页开发

        #region 网页授权

        /// <summary>
        /// 第一步：用户授权，获取code（授权码）
        /// 在确保微信公众账号拥有授权作用域（scope参数）的权限的前提下（服务号获得高级接口后，默认拥有scope参数中的snsapi_base和snsapi_userinfo），引导关注者打开如下页面
        /// https://open.weixin.qq.com/connect/oauth2/authorize?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE#wechat_redirect 若提示“该链接无法访问”，请检查参数是否填写错误，是否拥有scope参数对应的授权作用域权限。
        /// 用户同意授权后
        /// 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri? state = STATE
        /// code说明 ： code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
        /// </summary>
        /// <param name="appid">公众号的唯一标识</param>
        /// <param name="redirect_uri">授权后重定向的回调链接地址，请使用urlencode对链接进行处理</param>
        /// <param name="response_type">返回类型，请填写code</param>
        /// <param name="scope">应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节</param>
        /// <param name="#wechat_redirect">无论直接打开还是做页面302重定向时候，必须带此参数</param>
        /// <returns></returns>
        public string Auth_GetAuthCodeUrl(string appid, string redirect_uri, string scope, string state, string response_type = "code")
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect", appid,redirect_uri, response_type, scope.ToString(), state);
        }

        /// <summary>
        /// 第二步：通过code换取网页授权access_token
        /// 首先请注意，这里通过code换取的是一个特殊的网页授权access_token,与基础支持中的access_token（该access_token用于调用其他接口）不同。公众号可通过下述接口来获取网页授权access_token。如果网页授权的作用域为snsapi_base，则本步骤中获取到网页授权access_token的同时，也获取到了openid，snsapi_base式的网页授权流程即到此为止。
        /// </summary>
        /// <param name="appid">公众号的唯一标识</param>
        /// <param name="appsecret">公众号的appsecret</param>
        /// <param name="code">填写第一步获取的code参数</param>
        /// <param name="grant_type">填写为authorization_code</param>
        /// <returns></returns>
        public string Auth_GetAuthAccessTokenUrl(string appid, string appsecret, string code, string grant_type = "authorization_code")
        {
            return string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}", appid, appsecret, code, grant_type);
        }

        /// <summary>
        /// 第三步：刷新access_token（如果需要）
        /// 由于access_token拥有较短的有效期，当access_token超时后，可以使用refresh_token进行刷新，refresh_token有效期为30天，当refresh_token失效的后，需要用户重新授权。
        /// </summary>
        /// <param name="appid">公众号的唯一标识</param>
        /// <param name="grant_type">填写为refresh_token</param>
        /// <param name="refresh_token">填写通过access_token获取到的refresh_token参数</param>
        /// <returns></returns>
        public string Auth_GetAuthRefreshTokenUrl(string appid, string refresh_token, string grant_type = "refresh_token")
        {
            return string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type={1}&refresh_token={2}", appid, grant_type, refresh_token);
        }

        /// <summary>
        /// 第四步：拉取用户信息(需scope为 snsapi_userinfo)
        /// 如果网页授权作用域为snsapi_userinfo，则此时开发者可以通过access_token和openid拉取用户信息了。
        /// </summary>
        /// <param name="access_token">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openid">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns></returns>
        public string Auth_GetAuthUserinfoUrl(string access_token, string openid, string lang = "zh_CN")
        {
            return string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}", access_token, openid, lang);
        }

        #endregion 网页授权

        /// <summary>
        /// 获取JSAPI_Ticket
        /// </summary>
        /// <param name="access_token">普通access_token</param>
        /// <param name="type">token类型（默认jsapi）</param>
        /// <returns></returns>
        public string JSAPI_GetJSAPI_TicketUrl(string access_token, string type = "jsapi")
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}", access_token, type);
        }

        #endregion 网页开发

        #region 永久素材管理
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Material_GetMaterialList(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", access_token);
        }

        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Material_GetMaterialCount(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}", access_token);
        }

        /// <summary>
        /// 删除素材
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Material_DeleteMaterial(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}", access_token);
        }

        /// <summary>
        /// 获取单个素材
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Material_GetMaterial(string access_token)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}", access_token);
        }

        /// <summary>
        /// 新增其他类型素材，除图文素材之外的所有素材
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Material_AddMaterial(string access_token, string type)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}&type={1}", access_token, type);
        }
        #endregion
    }
}
