using Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.ResultModel;

namespace Base.IService
{
    /// <summary>
    ///附件接口
    /// </summary>
    public interface IAttachmentService : IBaseService<Base_Attachment>
    {
        ItemResult<bool> Enable(int id);
        ItemResult<bool> Enable(List<Base_Attachment> list);
    }
}
