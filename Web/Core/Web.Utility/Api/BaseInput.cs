using Newtonsoft.Json;

namespace Web.Utility.Api
{
    public class BaseInput<T>
    {
        /// <summary>
        /// 安全验证
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return true;
        }

        #region 带返回值的安全验证
        /// <summary>
        /// 安全验证
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid(ref ApiResult<T> result)
        {
            return true;
        }
        #endregion

        #region 转字符串
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToStr()
        {
            if (this == null) { return ""; }
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}