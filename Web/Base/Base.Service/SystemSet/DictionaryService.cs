using Base.Model;
using PetaPoco;
using System.Collections.Generic;
using Utility;
using Utility.ResultModel;

namespace Base.Service.SystemSet
{
    public class DictionaryService : BaseService<Sys_Dictionary>
    {
        private static DictionaryService dictionaryService = null;
        public static DictionaryService Single
        {
            get
            {
                if (dictionaryService == null)
                {
                    dictionaryService = new DictionaryService();
                }
                return dictionaryService;
            }
        }
        public List<Sys_Dictionary> GetList(Sys_Dictionary request)
        {
            Sql _sql = new Sql();
            _sql.Select("Sys_Dictionary.*,Sys_Field.Title").From("Sys_dictionary");
            _sql.Append(" INNER JOIN Sys_Field on Sys_Field.ID=Sys_Dictionary.FieldID");
            if (request.FieldID.HasValue)
                _sql.Where("Sys_Field.ID=@0", request.FieldID);
            _sql.Where("Sys_Dictionary.StateCode=0");
            return base.GetList(_sql);
        }

        public ListResult<Sys_Dictionary> GetDictionaryPageingList(Sys_Dictionary request, Pagination page)
        {
            return ApplicationContext.Cache.TryGet("GetDictionaryPageingList" + request.FieldID, 0, () =>
             {
                 Sql _sql = new Sql();
                 _sql.Select("Sys_dictionary.*,Sys_field.Title").From("Sys_dictionary");
                 _sql.Append(" inner join Sys_field on Sys_field.ID=Sys_dictionary.FieldID");
                 if (request.FieldID.HasValue)
                     _sql.Where("Sys_field.ID=@0", request.FieldID);
                 _sql.Where("Sys_dictionary.statecode=0");
                 return base.GetPagingList<Sys_Dictionary>(_sql, page);
             });
        }

        public ListResult<Sys_Dictionary> GetAllDictionaryList(int fieldId)
        {
            if (fieldId == 0) return new ListResult<Sys_Dictionary>() { Data = new List<Sys_Dictionary>() { } };
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_dictionary").Where("FieldID=@0", fieldId);
            return base.GetPagingList<Sys_Dictionary>(_sql, new Pagination() { Page = 1, PageSize = 9999 });
        }

        /// <summary>
        /// 获取非系统基础数据
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public ListResult<Sys_Dictionary> GetDictionaryList(int fieldId)
        {
            if (fieldId == 0) return new ListResult<Sys_Dictionary>() { Data = new List<Sys_Dictionary>() { } };
            Sql _sql = new Sql();
            _sql.Select("*").From("Sys_dictionary").Where("FieldID=@0 AND IsSystem=0", fieldId);
            return base.GetPagingList<Sys_Dictionary>(_sql, new Pagination() { Page = 1, PageSize = 9999 });
        }

    }
}
