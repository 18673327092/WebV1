using Utility.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Model;
using Base.Model.Base.Model;

namespace Base.IService
{
    public interface IImagesService : IBaseService<Base_Images>
    {
        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <returns></returns>
        Task<ItemResult<Base_Images>> GetAsync(int id);
        Task<ItemResult<string>> GetSrc(int id);
        Task<ItemResult<string>> GetOriginalSrc(int id);
        Task<ItemResult<string>> GetThumbnailSrc(int id);

        /// <summary>
        /// 图片存储到OSS
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        ItemResult<string> ImageToOSS(string filename, byte[] data, string localFilename = "");

        /// <summary>
        /// 删除存储到OSS的图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        ItemResult<string> OSSImageDelete(string filename);
    }
}