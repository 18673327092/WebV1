using Base.Model;
using Base.Model.Enum;
using Base.Model.Sys.Model;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.SystemSet
{
    public class SearchService : BaseService<Sys_Field>
    {
        private static SearchService searchService = null;
        public static SearchService Single
        {
            get
            {
                if (searchService == null)
                {
                    searchService = new SearchService();
                }
                return searchService;
            }
        }

        /// <summary>
        /// 搜索条件
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public List<SearchField> GetSearchFields(int eid)
        {
            List<SearchField> SearchFieldList = new List<SearchField>();
            List<Sys_Field> listfiled = base.GetList(new Sql("SELECT ID,Name,Field,FieldType,Title,EntityName,IsMultiple,SearchControlWidth,IsCustomizeSearchControl FROM Sys_Field WHERE entityid=" + eid + " AND IsAllowSearch=1 ORDER BY SearchControlSort DESC"));
            foreach (var field in listfiled)
            {
                var s = new SearchField()
                {
                    ID = field.ID,
                    Name = field.Name,
                    FieldType = field.FieldType,
                    Title = field.Title,
                    Field = field.Field,
                    IsMultiple = field.IsMultiple,
                    SearchControlWidth = field.SearchControlWidth,
                    IsCustomizeSearchControl = field.IsCustomizeSearchControl
                };
                if (field.FieldType == "选项集" || field.FieldType == "两个选项" || field.FieldType == "选项集多选")
                {
                    s.Field = field.EntityName + "." + field.Name;
                    s.DictionaryList = base.GetList<Sys_Dictionary>(new Sql("SELECT * FROM Sys_Dictionary WHERE FieldID=@0", field.ID) { });
                }
                else if (field.FieldType == "关联其他表")
                {
                    //  s.Field = field.EntityName + "." + field.Name;
                }
                SearchFieldList.Add(s);
            }
            return SearchFieldList;
        }

        /// <summary>
        /// 弹框视图搜索条件
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public List<SearchField> GetDialogSearchFields(int eid)
        {
            List<SearchField> SearchFieldList = new List<SearchField>();
            List<Sys_Field> listfiled = base.GetList(new Sql("SELECT ID,Name,Field,FieldType,Title,EntityName,IsMultiple FROM Sys_Field WHERE entityid=" + eid + " AND IsAllowDialogSearch=1"));
            foreach (var field in listfiled)
            {
                var s = new SearchField()
                {
                    ID = field.ID,
                    FieldType = field.FieldType,
                    Title = field.Title,
                    Field = field.Field,
                    IsMultiple = field.IsMultiple,
                    SearchControlWidth = field.SearchControlWidth,
                    IsCustomizeSearchControl = field.IsCustomizeSearchControl
                };
                if (field.FieldType == "选项集" || field.FieldType == "两个选项" || field.FieldType == EnumFieldType.选项集多选.ToString())
                {
                    s.Field = field.EntityName + "." + field.Name;
                    s.DictionaryList = base.GetList<Sys_Dictionary>(new Sql("SELECT * FROM Sys_Dictionary WHERE FieldID=@0", field.ID) { });
                }
                else if (field.FieldType == "关联其他表")
                {
                    s.Field = field.EntityName + "." + field.Name;
                }
                SearchFieldList.Add(s);
            }
            return SearchFieldList;
        }

        /// <summary>
        /// 获取某个视图下的搜索条件
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public List<SearchField> GetSearchFieldsByView(int v, int eid)
        {
            List<SearchField> SearchFieldList = new List<SearchField>();
            List<Sys_Field> listfiled = base.GetList(new Sql("SELECT ID,Name,Field,FieldType,Title,EntityName,RelationEntity,IsMultiple,SearchControlWidth,IsCustomizeSearchControl,charindex(','+rtrim(" + v + ")+',',','+SearchForView+',') AS SearchControIsForView FROM Sys_Field WHERE charindex(','+rtrim(" + v + ")+',',','+SearchForView+',')>0 OR (IsCustomizeSearchControl=1 AND entityid=" + eid + " )  ORDER BY SearchControlSort DESC"));
            foreach (var field in listfiled)
            {
                var s = new SearchField()
                {
                    ID = field.ID,
                    Name = field.Name,
                    FieldType = field.FieldType,
                    Title = field.Title,
                    Field = field.Field,
                    IsMultiple = field.IsMultiple,
                    RelationEntity=field.RelationEntity,
                    SearchControlWidth = field.SearchControlWidth,
                    IsCustomizeSearchControl = field.IsCustomizeSearchControl,
                    SearchControIsForView = field.SearchControIsForView > 0,
                };
                if (field.FieldType == "选项集" || field.FieldType == "两个选项" || field.FieldType == EnumFieldType.选项集多选.ToString())
                {
                    s.Field = field.EntityName + "." + field.Name;
                    s.DictionaryList = base.GetList<Sys_Dictionary>(new Sql("SELECT * FROM Sys_Dictionary WHERE FieldID=@0", field.ID) { });
                }
                else if (field.FieldType == "关联其他表")
                {
                    //s.Field = field.EntityName + "." + field.Name;
                }
                SearchFieldList.Add(s);
            }
            return SearchFieldList;
        }

        /// <summary>
        /// 获取搜索关键词
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public string GetSearchKeyValue(int eid)
        {
            List<string> fields = new List<string>();
            using (var db = CreateDao())
            {
                return db.ExecuteScalar<string>("SELECT LEFT(ValueList,LEN(ValueList)-1) KeySearch FROM (SELECT (SELECT cast(Title as varchar(100)) + '、' FROM Sys_Field WHERE IsKeySearch=1 AND EntityID=@0  FOR XML PATH('')) AS ValueList)Z", eid);
            }
        }
    }
}
