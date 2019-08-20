using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Model.Enum
{
    public enum ReportEnum
    {
        查询语句 = 1,
        存储过程 = 2
    }

    /// <summary>
    /// 参数类型
    /// </summary>
    public enum ParameterTypeEnum
    {
        文本 = 1,
        选项集 = 2,
        关联其他表 = 3,
        数字 = 4,
        日期 = 5,
        时间 = 6,
    }
}
