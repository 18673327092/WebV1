namespace Web.Utility.Api
{
    /// <summary>
    /// 接口返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public int code { get; set; } = 0;

        /// <summary>
        /// 执行结果
        /// </summary>
        public bool state { get; set; } = false;

        /// <summary>
        /// 结果集
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string message { get; set; } = "";

    }
}