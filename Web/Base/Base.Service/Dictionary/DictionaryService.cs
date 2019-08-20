using Base.IService;
using Base.Model.Sys;
using Base.Model.Sys.Model;
using ORM;
using Utility;
using Utility.ResultModel;
using Newtonsoft.Json;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Model;
using System.Threading.Tasks;

namespace Base.Service
{
    public class DictionaryService : BaseService<DictionaryModel>, IDictionaryService
    {
        public ListResult<DictionaryModel> GetPagingList(DictionaryModel request, Pagination page)
        {
            ListResult<DictionaryModel> result = new ListResult<DictionaryModel>();
            var _sql = @"SELECT ID 'FieldID',Title 'FieldTitle',ShowName 'EntityName',LEFT(ValueList,LEN(ValueList)-1) AS ValueList
                        FROM 
                        (
	                        SELECT F.ID,F.Title,E.ShowName,
		                        (SELECT cast(D.Name as varchar(100)) + '、' FROM Sys_Dictionary D WHERE D.FieldID=F.ID AND D.IsSystem=0 AND StateCode=0  FOR XML PATH('')) AS ValueList 
	                        FROM Sys_Field F 
	                        INNER JOIN Sys_Entity E ON E.ID=F.EntityID
	                        WHERE F.FieldType IN('选项集','两个选项','选项集多选') AND IsCustomizeDictionary=1
                        )T ";
            var sql = new Sql(_sql);
            sql.Where("ValueList IS NOT NULL");
            if (!string.IsNullOrEmpty(request.EntityName))
            {
                sql.Where("ShowName LIKE @0", "%" + request.EntityName + "%");
            }
            if (!string.IsNullOrEmpty(request.FieldTitle))
            {
                sql.Where("Title LIKE @0", "%" + request.FieldTitle + "%");
            }
            if (!string.IsNullOrEmpty(request.ValueList))
            {
                sql.Where("ValueList LIKE @0", "%" + request.ValueList + "%");
            }
            return base.GetPagingList(sql, page);
        }

        public ItemResult<bool> Save(DictionaryModel entity)
        {
            ItemResult<bool> result = new ItemResult<bool>();
            if (!string.IsNullOrEmpty(entity.ValueList) && entity.ValueList != "[]")
            {
                var db = CreateDao();
                bool isKeepConnectionAlive = db.KeepConnectionAlive;
                try
                {
                    // 检查此前是否已经保持连接存活状态，如果没有，则进行设置
                    if (!isKeepConnectionAlive)
                        // 保持连接存活状态
                        db.KeepConnectionAlive = true;
                    // 开始事务
                    db.BeginTransaction();
                    //添加字段字典
                    List<Sys_Dictionary> dic = JsonConvert.DeserializeObject<List<Sys_Dictionary>>(entity.ValueList);
                    db.Execute(string.Format("delete from Sys_Dictionary WHERE FieldID={0}", entity.FieldID));
                    foreach (var d in dic)
                    {
                        d.FieldID = entity.FieldID;
                        db.Insert(d);
                    }
                    // 完成事务
                    db.CompleteTransaction();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Success = false;
                    // 中断事务
                    db.AbortTransaction();
                }
                finally
                {
                    // 检查此前是否已经保持连接存活状态，如果没有，则关闭数据库连接
                    if (!isKeepConnectionAlive)
                    {
                        // 关闭连接存活状态
                        db.KeepConnectionAlive = false;
                        // 关闭数据库连接
                        db.CloseSharedConnection();
                    }
                }
            }
            return result;
        }

       

        /// <summary>
        /// 返回所有数据字典
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public ListResult<Sys_Dictionary> GetAllList(int fieldId)
        {
            if (fieldId == 0) return new ListResult<Sys_Dictionary>() { Data = new List<Sys_Dictionary>() { } };
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_dictionary").Where("FieldID=@0", fieldId);
            return base.GetPagingList<Sys_Dictionary>(_sql, new Pagination() { Page = 1, PageSize = 9999 });
        }

        /// <summary>
        /// 返回所有数据字典
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public async Task<ListResult<Sys_Dictionary>> GetAllListAsync(int fieldId)
        {
            return await Task.Run(() =>
            {
                if (fieldId == 0) return new ListResult<Sys_Dictionary>() { Data = new List<Sys_Dictionary>() { } };
                Sql _sql = new Sql();
                _sql.Select("*").From("Sys_dictionary").Where("FieldID=@0", fieldId);
                return base.GetPagingList<Sys_Dictionary>(_sql, new Pagination() { Page = 1, PageSize = 9999 });
            });
        }

        /// <summary>
        /// 返回可以编辑的数据字典列表
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public ListResult<Sys_Dictionary> GetAllowEditList(int fieldId)
        {
            if (fieldId == 0) return new ListResult<Sys_Dictionary>() { Data = new List<Sys_Dictionary>() { } };
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_dictionary").Where("FieldID=@0 AND IsSystem=0", fieldId);
            return base.GetPagingList<Sys_Dictionary>(_sql, new Pagination() { Page = 1, PageSize = 9999 });
        }


    }
}
