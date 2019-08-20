using Base.IService;
using Base.Model;

using Base.Model.Sys;
using ORM;
using Utility;
using Utility.ResultModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    /// <summary>
    /// 附件业务类
    /// </summary>
    public class AttachmentService : BaseService<Base_Attachment>, IAttachmentService
    {
        public ListResult<Base_Attachment> GetPagingList(Base_Attachment request, Pagination page)
        {
            return base.GetPagingList(page);
        }

        /// <summary>
        ///启用附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemResult<bool> Enable(int id)
        {
            using (var db = CreateDao())
            {
                ItemResult<bool> result = new ItemResult<bool>();
                result.Data = db.Execute("UPDATE Base_Attachment SET StateCode=0 WHERE ID=@0", id) > 0;
                result.Success = true;
                return result;
            }
        }

        /// <summary>
        ///启用附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemResult<bool> Enable(List<Base_Attachment> list)
        {
            using (var db = CreateDao())
            {
                foreach (Base_Attachment model in list)
                {
                    db.Execute("UPDATE Base_Attachment SET StateCode=0 WHERE ID=@0", model.ID);
                }
                ItemResult<bool> result = new ItemResult<bool>();
                result.Success = true;
                return result;
            }
        }
    }
}
