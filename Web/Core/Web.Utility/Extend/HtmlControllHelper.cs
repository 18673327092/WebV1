using Base.Model;
using Base.Model.Enum;
using Base.Model.Sys.Model;
using Base.Service.SystemSet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using Utility.Components;

namespace System.Web.Mvc
{
    public class HtmlControllHelper
    {
        #region 表单控件

        public static HtmlString InitMetadataHtml(FormControllerModel model)
        {
            StringBuilder strb = new StringBuilder();
            var _field = SystemSetService.Field.Get(model.FieldID).Data;
            if (_field == null) { return new HtmlString(""); }

            Dictionary<string, string> PageData;
            string value = _field.DefaultValue;
            if (!string.IsNullOrEmpty(model.DefaultValue)) value = model.DefaultValue;
            if (model.ID > 0)
            {
                string CacheKey = _field.EntityName + model.ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(_field.EntityName, model.ID);
                });
                if (PageData.ContainsKey(_field.Name))
                {
                    value = PageData[_field.Name];
                }
                if (_field.FieldType == EnumFieldType.时间.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = ConvertHelper.Single.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else if (_field.FieldType == EnumFieldType.日期.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                    }
                }
            }

            if (_field.MaxLength > 0)
            {
                model.Attr += string.Format(" maxlength='{0}'", _field.MaxLength);
            }

            var fieldName = _field.Name;
            var placeholder = _field.Placeholder;
            var valid = _field.Valid;
            if (model.FormField != null)
            {
                valid = model.FormField.Valid;
            }
            //自定义控件
            if (model.IsCustomize.HasValue && model.IsCustomize.Value)
            {
                fieldName = string.IsNullOrEmpty(model.CustomizeFieldName) ? "Cus-" + fieldName : model.CustomizeFieldName;
            }

            if (!string.IsNullOrEmpty(valid))
                valid = valid.Replace("[field]", _field.Title);
            int _value = -1;
            List<Sys_Dictionary> list = new List<Sys_Dictionary>();
            switch (_field.FieldType)
            {
                case "两个选项":
                   // #region MyRegion
                   // list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _field.ID }, new Pagination() { Page = 1, PageSize = 20 }).Data;
                   // if (!string.IsNullOrEmpty(value)) { _value = Convert.ToInt32(value); } else { value = "1"; }
                   // strb.Append("<div data-isreadonly='" + ((model.IsReadOnly.HasValue && model.IsReadOnly.Value) ? 1 : 0) + "' class='fl radio-switch' " + (1 == _value ? "checked" : "") + ">");
                   // strb.Append("<span></span>");
                   // var z = 0;
                   // foreach (var item in list)
                   // {
                   //     z++;
                   //     strb.AppendFormat("<input data-name='{0}' name='radio_{0}' id='{0}_{1}'  value='{2}'  type='radio' value='1' data-title='{3}' style='display:none;' />", fieldName, z, item.Value, item.Name);
                   // }
                   // strb.Append(" </div>");
                   // strb.AppendFormat("<input name='{0}' class='radiovalue {1}' style='position: absolute; top: -1429px;' type='hidden' value='{2}'  />"
                   //, fieldName, valid, value);
                   // #endregion
                   // break;
                case "选项集":
                    #region MyRegion
                    if (!string.IsNullOrEmpty(value)) { _value = Convert.ToInt32(value); }
                    list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _field.ID },
                          new Pagination() { Page = 1, PageSize = 20 }).Data;
                    //只读
                    if (model.IsReadOnly.HasValue && model.IsReadOnly.Value)
                    {
                        var _title = string.Empty;
                        if (list.Any(e => e.Value == _value))
                        {
                            _title = list.Where(e => e.Value == _value).FirstOrDefault().Name;
                        }
                        strb.AppendFormat("<span  id='span{0}' name='{0}' class='' style='' id='span{0}'>{1}</span><input type='hidden' type='text' value='{2}' />",
                            fieldName, _title, value);
                    }
                    else
                    {
                        strb.AppendFormat(" <select  id='{0}' name='{0}' class='form-tag field fl valid {1} {2}' {3}  >", fieldName, valid, model.ClassName, model.Attr);
                        if (string.IsNullOrEmpty(model.DefaultValue))
                            strb.Append("<option value=''>请选择</option>");
                        foreach (var item in list)
                        {
                            strb.AppendFormat("<option value='{0}' " + (item.Value == _value ? "selected='selected'" : "") + " >{1}</option>", item.Value, item.Name);
                        }
                        strb.Append(" </select>");
                    }
                    #endregion
                    break;
                case "选项集多选":
                    #region MyRegion
                    list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _field.ID },
                                    new Pagination() { Page = 1, PageSize = 20 }).Data;
                    strb.AppendFormat("<input name='{0}'  class='checkboxvalue {1}' value='{2}' style='position: absolute; top: -1429px;' type='hidden'  /><div class='fl'>", fieldName, valid, value);
                    var i = 0;
                    foreach (var item in list)
                    {
                        var vs = new string[] { };
                        if (!string.IsNullOrEmpty(value))
                            vs = value.Split(new char[] { ',' });
                        i++;
                        strb.AppendFormat("<div class='fl'><input name='chk_{0}' id='{0}_{1}' value='{2}' type='checkbox' " + (vs.Any(e => e == item.Value.ToString()) ? "checked" : "") + " ><label for='{0}_{1}'>&nbsp;{3} &nbsp;&nbsp;</label> </div>"
                            , fieldName, i, item.Value, item.Name);
                    }
                    strb.Append(" </div>");

                    #endregion
                    break;
                case "单行文本":
                    strb.AppendFormat("<input id='{0}' name='{0}' class='inputText fl valid {1} {5} ' type='text' value='{2}' {3} placeholder='{4}' >"
                        , fieldName, valid, value, model.Attr, placeholder, model.ClassName);
                    break;
                case "多行文本":
                    strb.AppendFormat("<textarea id='{0}' name='{0}' class='form-tag field inputText fl  {1} {5} ' style='height:60px;' {3} placeholder='{4}' >{2}</textarea>"
                        , fieldName, valid, value, model.Attr, placeholder, model.ClassName);
                    break;
                case "整数":
                    strb.AppendFormat("<input id='{0}' name='{0}' class='form-tag inputText fl  numberbox valid  {1} {5}'  type='text' value='{2}' {3}  placeholder='{4}' >"
                           , fieldName, valid, value, model.Attr, placeholder, model.ClassName);
                    break;
                case "浮点数":
                    strb.AppendFormat("<input id='{0}' name='{0}' class='form-tag inputText fl floatbox  valid  {1} {5} '  type='text' value='{2}' {3}  placeholder='{4}' >"
                           , fieldName, valid, value, model.Attr, placeholder, model.ClassName);
                    break;
                case "货币":
                    strb.AppendFormat("<input id='{0}' name='{0}' class='form-tag inputText fl  moneybox valid  {1} {5}'  type='text' value='{2}' {3}  placeholder='{4}' >"
                           , fieldName, valid, value, model.Attr, placeholder, model.ClassName);
                    break;
                case "日期":
                    strb.AppendFormat("<input id='{0}' name='{0}' class='form-tag field inputText fl laydate-icon date  valid  {1} {5}' value='{2}' {3} placeholder='{4}' onclick=\"{6}\"  >"
                           , fieldName, valid, value, model.Attr, placeholder, model.ClassName, "laydate({ format: 'YYYY-MM-DD'})");
                    break;
                case "时间":
                    strb.AppendFormat("<input id='{0}' name='{0}' class='form-tag field inputText fl laydate-icon time  valid  {1} {5}' value='{2}' {3} placeholder='{4}' onclick=\"{6} \" >"
                           , fieldName, valid, value, model.Attr, placeholder, model.ClassName, "laydate({istime: true, format: 'YYYY-MM-DD hh:mm:ss'})");
                    break;
                case "关联其他表":
                    var _rfieldName = string.Empty;
                    int _rvalue = -1;
                    if (!string.IsNullOrEmpty(value)) { _rvalue = Convert.ToInt32(value); }
                    if (_field.IsDropDownSource.HasValue && _field.IsDropDownSource.Value)
                    {
                        Dictionary<int, string> _datalist = SystemSetService.Entity.GetFKDropDownSourceData(_field.RelationEntity);
                        strb.AppendFormat(" <select id='{0}' name='{0}' class='form-tag field valid fl {1} {2}' {3} >", fieldName, valid, model.ClassName, model.Attr);
                        if (!(_field.IsMust.HasValue && _field.IsMust.Value))
                        {
                            strb.AppendFormat("<option value=''>请选择</option>");
                        }

                        foreach (var item in _datalist)
                        {
                            strb.AppendFormat("<option value='{0}' " + (item.Key == _rvalue ? "selected='selected'" : "") + " >{1}</option>", item.Key, item.Value);
                        }
                        strb.Append(" </select>");
                    }
                    else
                    {
                        if (_rvalue != -1)
                        {
                            _rfieldName = SystemSetService.Common.GetFKField_Name(_field.RelationEntity, _rvalue);
                        }
                        if (string.IsNullOrEmpty(model.Attr))
                        {
                           // model.Attr = "style='width:216px;float: left;padding: 7px 6px;padding-right: 35px;'";
                        }
                        strb.AppendFormat("<span id='span_{0}{1}' {2} class='lookupSpan form-tag inputText fl text-box single-line {3}'>{4}</span>"
                            , fieldName, model.ID, model.Attr, model.ClassName, _rfieldName);
                        strb.AppendFormat("<input  id='input_{0}{1}'  name='{0}' value='{2}'  class='form-tag field {3} ' type='text' style='position: absolute; top: -1429px;border: 0;height: 0px;  color: white;' >"
                            , fieldName, model.ID, value, _field.Valid);
                        if (!model.IsReadOnly.HasValue || !model.IsReadOnly.Value)
                        {
                            strb.AppendFormat("<button id='btn_{0}{1}' data-id='{1}' data-fieldid='{2}'  data-value='{3}' onclick='open_field_dialog(this)' title='选择或更改此字段的值' type='button'  class='select-btn' ><img src='/Content/base/images/view.png' style=' width: 15px;margin-top: -4px;margin-left: 2px; '></button>"
                           , fieldName, model.ID, _field.ID, value);
                        }

                    }
                    break;
                case "关联其他表多选":
                    string rand2 = (new DateTime().Ticks).ToString();
                    var _rfieldName2 = string.Empty;
                    if (!string.IsNullOrEmpty(value))
                    {
                        _rfieldName2 = SystemSetService.Common.GetFKField_Names(_field.RelationEntity, value);
                    }
                    if (string.IsNullOrEmpty(model.Attr))
                    {
                        //model.Attr = "style='width:216px;float: left;padding: 7px 6px;padding-right: 35px;'";
                    }
                    strb.AppendFormat("<span id='span_{0}{1}' {2} class='lookupSpan form-tag inputText fl text-box single-line {3}'>{4}</span>"
                        , fieldName, model.ID, model.Attr, model.ClassName, _rfieldName2);
                    strb.AppendFormat("<input id='input_{0}{1}' name='{0}' value='{3}' class='form-tag field {4} ' type='text' style='position: absolute; top: -1429px;border: 0;height: 0px;  color: white;'  >"
                        , fieldName, model.ID, _rfieldName2, value, valid);
                    strb.AppendFormat("<button id='btn_{0}{1}' data-id='{1}' data-fieldid='{2}'  data-value='{3}' onclick='open_field_dialogmulti(this)' title='选择或更改此字段的值' type='button'  class='select-btn' ><img src='/Content/base/images/view.png' style='width: 15px;margin-top: -4px;margin-left: 2px;'></button>"
                        , fieldName, model.ID, _field.ID, value);
                    break;
                case "图片":
                    strb.AppendFormat("<div class='filepanel '  data-readonly='" + (model.IsReadOnly.HasValue && model.IsReadOnly.Value) + "' data-multi='false' data-name='{0}' data-value='{1}' data-auto='true' data-width='{2}' data-height='{3}' data-valid='{4}'></div>", _field.Name, value, _field.ImageWidth, _field.ImageHeight, valid);
                    //StringBuilder ss = new StringBuilder();
                    //if (!string.IsNullOrEmpty(value))
                    //{
                    //    string[] srclist = value.Split(new char[] { ',' });
                    //    foreach (string src in srclist)
                    //    {
                    //        if (!(model.IsReadOnly.HasValue && model.IsReadOnly.Value))
                    //        {
                    //            ss.AppendFormat(@"<img class='prewImagePanel' id='preview_{0}' src='{2}' style='display:block;'/><div class='prewImagePanelTool deleteImage' data-value='{1}'>删除</div>",
                    //                fieldName, src, Utility.ApplicationContext.AppSetting.OSS_Domain + "/" + src);
                    //        }
                    //        else
                    //        {
                    //            ss.AppendFormat(@"<img class='prewImagePanel' id='preview_{0}' src='{2}' />",
                    //                fieldName, src, Utility.ApplicationContext.AppSetting.OSS_Domain + "/" + src);
                    //        }
                    //    }
                    //}
                    //string filedHtmlImag = string.Empty;
                    //if (!(model.IsReadOnly.HasValue && model.IsReadOnly.Value))
                    //{
                    //    filedHtmlImag = string.Format(@"<input name='file_{0}' data-id='{0}' data-multi='false' id='file_{0}' type='file' class='imageupload file-loading'>
                    //                     <input type='hidden' name='{0}' id='{0}' value='{1}' />", fieldName, value);
                    //}
                    //strb.Append("<table style='float: left;'><tr><td> " + ss.ToString() + "</td> </tr><tr> <td>" + filedHtmlImag + "</td></tr></table>");
                    break;
                case "图片集":
                    strb.AppendFormat("<div class='filepanel'  data-readonly='" + (model.IsReadOnly.HasValue && model.IsReadOnly.Value) + "' data-multi='true' data-name='{0}' data-value='{1}' data-auto='true' data-width='{2}' data-height='{3}' data-valid='{4}'></div>", _field.Name, value, _field.ImageWidth, _field.ImageHeight, valid);
                    //StringBuilder s = new StringBuilder();
                    //s.Append(@"<div data-id='" + fieldName + "' id='gridly_" + fieldName + "' class='gridly' '>");
                    //if (!string.IsNullOrEmpty(value))
                    //{
                    //    string[] srclist = value.Split(new char[] { ',' });
                    //    foreach (string src in srclist)
                    //    {
                    //        if (!(model.IsReadOnly.HasValue && model.IsReadOnly.Value))
                    //        {
                    //            s.AppendFormat(@"<div class='brick small'>
                    //                            <div class='fileinput-new thumbnail' style=''>
                    //                                <a href='{1}' class='fancybox-button'  >
                    //                                    <img data-value='{0}' src='{1}' style='width: 100px;height:100px;'>
                    //                                </a>
                    //                            </div>
                    //                            <div class='gridlytoobar'><a href='javascript:;' data-value='{0}' class='deleteImage' >删除</a></div>
                    //                        </div>", src, Utility.ApplicationContext.AppSetting.OSS_Domain + "/" + src);
                    //        }
                    //        else
                    //        {
                    //            s.AppendFormat(@"<div class='thumbnail' style='float:left;'>
                    //                                <a href='{1}' class='fancybox-button'  >
                    //                                    <img id='img_Image' src='{1}' style='max-width: 100px;'>
                    //                                </a>
                    //                        </div>", src, Utility.ApplicationContext.AppSetting.OSS_Domain + "/" + src);
                    //        }
                    //    }
                    //}
                    //s.Append("</div>");
                    //string filedHtml = string.Empty;
                    //if (!(model.IsReadOnly.HasValue && model.IsReadOnly.Value))
                    //{
                    //    filedHtml = string.Format(@"<input name='file_{0}' data-id='{0}' data-multi='true' id='file_{0}' type='file' class='imageupload file-loading'>
                    //                     <input type='hidden' name='{0}' id='{0}' value='{1}' />", fieldName, value);
                    //}
                    //strb.Append("<table style='float: left;'><tr><td> " + s.ToString() + "</td> </tr><tr> <td>" + filedHtml + "</td></tr></table>");
                    break;
                case "附件":
                    if (!(model.IsReadOnly.HasValue && model.IsReadOnly.Value))
                    {
                        strb.AppendFormat("<input name='{0}'  class='form-tag field {2} ' id='{0}' value='{1}' type='text' style='position: absolute; top: -1429px;border: 0;height: 0px;  color: white;' >", fieldName, value, valid);
                        strb.AppendFormat("<input name='file_{0}' data-id='{0}' id='file_{0}' type='file' class='filesupload file-loading'>", fieldName);
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        List<Base_Attachment> attachmentlist = new List<Base_Attachment>();
                        try
                        {
                            attachmentlist = JsonHelper.Single.JsonToObject<List<Base_Attachment>>(value);
                        }
                        catch (Exception)
                        {

                        }
                        string strs = string.Empty;
                        foreach (var attachment in attachmentlist)
                        {
                            strs += string.Format(@"<div data-field='{0}' data-id='{1}' data-name='{2}'  data-size='{3}' data-attachment='{4}' id=""file_{0}{1}"" class=""uploadifyQueueItem"">
                                        <div class=""cancel"">
                                            <a href=""javascript:jQuery('#file_{0}').uploadifyCancel('{1}')"">
                                                <img src=""/Content/plugin/uploadify/cancel.png"" border=""0"">
                                            </a>
                                        </div>
                                           <div style=""float:right;""><a title=""下载"" target=""_balnk"" href=""{4}""><img style=""height: 15px;margin-right: 5px;"" src=""/Content/plugin/uploadify/down.png"" /></a></div>                                        
                                        <span class=""fileName"">{2} ({3})</span><span class=""percentage""></span>
                                    </div>", fieldName, attachment.ID, attachment.Name, attachment.Size, attachment.Attachment);
                        }
                        strb.AppendFormat(@"<div id=""file_{0}Queue"" class=""uploadifyQueue uploadifyQueueDB"">" + strs + "</div>", fieldName);
                    }

                    break;
                default:
                    strb.AppendFormat("<input id='{0}' name='{0}' class='inputText fl valid  {1} {5}' type='text' value='{2}' {3}  placeholder='{4}' >"
                       , fieldName, valid, value, model.Attr, placeholder, model.ClassName);
                    break;
            }
            return new HtmlString(strb.ToString());
        }

        //表单Hidden控件
        public static HtmlString FormHiddenControl(int fieldid, int ID = 0, string defaultvalue = "")
        {
            var _filed = SystemSetService.Field.Get(fieldid).Data;
            if (_filed == null) { return new HtmlString(""); }
            Dictionary<string, string> PageData;
            string value = defaultvalue;
            if (ID > 0)
            {
                string CacheKey = _filed.EntityName + ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(_filed.EntityName, ID);
                });
                if (PageData.ContainsKey(_filed.Name))
                    value = PageData[_filed.Name];
                var _value = 0;
                switch (_filed.FieldType)
                {
                    case "两个选项":
                    case "选项集":
                    case "关联其他表":
                        if (!string.IsNullOrEmpty(value)) { _value = Convert.ToInt32(value); }
                        if (_value == 0)
                        {
                            value = defaultvalue;
                        }
                        break;
                    default:
                        if (string.IsNullOrEmpty(value)) value = defaultvalue;
                        break;
                }
            }
            return new HtmlString(string.Format("<input id='{0}'  name='{0}' value='{1}' type='hidden' >", _filed.Name, value));
        }

        //表单Hidden控件
        public static HtmlString FormHiddenControl(string fieldname, int ID = 0, string EntityName = "", string defaultvalue = "")
        {
            Dictionary<string, string> PageData;
            string value = defaultvalue;
            if (ID > 0)
            {
                string CacheKey = EntityName + ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(EntityName, ID);
                });
                if (PageData.ContainsKey(fieldname))
                    value = PageData[fieldname];
                if (string.IsNullOrEmpty(value)) value = defaultvalue;
            }
            return new HtmlString(string.Format("<input id='{0}'  name='{0}' value='{1}' type='hidden' >", fieldname, value));
        }

        //表单Span控件
        public static HtmlString FormSpanControl(int fieldid, int ID = 0)
        {
            var _filed = SystemSetService.Field.Get(fieldid).Data;
            if (_filed == null) { return new HtmlString(""); }
            Dictionary<string, string> PageData;
            string value = string.Empty;
            if (ID > 0)
            {
                string CacheKey = _filed.EntityName + ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(_filed.EntityName, ID);
                });
                value = PageData[_filed.Name];
            }
            return new HtmlString(string.Format("<span id='{0}' style='padding-top: 4px !important;cursor: default;' class='form-tag form-control text-box single-line'>{1}</span>", _filed.Name, value));
        }

        public static HtmlString GetText(int fieldid, int ID, string defaultvalue = "")
        {
            var _filed = SystemSetService.Field.Get(fieldid).Data;
            if (_filed == null) { return new HtmlString(""); ; }
            Dictionary<string, string> PageData;
            string value = "";
            if (ID > 0)
            {
                string CacheKey = _filed.EntityName + ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(_filed.EntityName, ID);
                });
                if (PageData.ContainsKey(_filed.Name))
                {
                    value = PageData[_filed.Name];
                }
                if (_filed.FieldType == EnumFieldType.时间.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = ConvertHelper.Single.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else if (_filed.FieldType == EnumFieldType.日期.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                    }
                }
            }
            int _value = -1;
            switch (_filed.FieldType)
            {
                case "两个选项":
                case "选项集":
                    if (!string.IsNullOrEmpty(value)) { _value = Convert.ToInt32(value); }
                    List<Sys_Dictionary> list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _filed.ID },
                        new Pagination() { Page = 1, PageSize = 20 }).Data;
                    if (list.Any(e => e.Value == _value))
                    {
                        value = list.Where(e => e.Value == _value).FirstOrDefault().Name;
                    }
                    break;
                case "选项集多选":
                    list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _filed.ID },
                         new Pagination() { Page = 1, PageSize = 20 }).Data;
                    value = SystemSetService.FormDataSource.GetDictionaryNames(value, fieldid);
                    break;
                case "关联其他表":
                    string rand = (new DateTime().Ticks).ToString();
                    var fieldname = string.Empty;
                    int _rvalue = -1;
                    if (!string.IsNullOrEmpty(value)) { _rvalue = Convert.ToInt32(value); }
                    if (_rvalue != -1)
                    {
                        fieldname = SystemSetService.Entity.GetFKField_Name(_filed.RelationEntity, _rvalue);
                    }
                    value = fieldname;
                    if (string.IsNullOrEmpty(value)) { value = defaultvalue; }
                    if (value == "0") value = "";
                    break;
                case "关联其他表多选":
                    string rand2 = (new DateTime().Ticks).ToString();
                    var fieldname2 = string.Empty;
                    if (!string.IsNullOrEmpty(value))
                    {
                        fieldname2 = SystemSetService.Entity.GetFKField_Names(_filed.RelationEntity, value);
                    }
                    value = fieldname2;
                    if (value == "0") value = "";
                    break;
                case "图片":
                    //string imgsrc = string.Empty;
                    //if (!string.IsNullOrEmpty(value))
                    //{
                    //    imgsrc = SystemSetService.Common.GetImgSrcByID(Convert.ToInt32(value));
                    //    value = string.Format(@"<div  class='fileinput-new thumbnail' style='width: 156px;height: 156px;text-align: center;margin-bottom: 8px;'>
                    //                            <a href='{0}' class='fancybox-button'>
                    //                                <img src='{0}' style=' max-width: 156px;' />
                    //                            </a>
                    //                        </div>", imgsrc);
                    //}

                    value = string.Format("<div class='filepanel' data-readonly='true' data-multi='false' data-name='{0}' data-value='{1}' data-auto='true' data-width='{2}' data-height='{3}'></div>", _filed.Name, value, _filed.ImageWidth, _filed.ImageHeight);
                    break;
                case "图片集":
                    //List<string> idarr = new List<string>();
                    //StringBuilder strb = new StringBuilder();
                    //if (!string.IsNullOrEmpty(value))
                    //{
                    //    string[] ids = value.Split(new char[] { ',' });
                    //    foreach (string id in ids)
                    //    {
                    //        imgsrc = SystemSetService.Common.GetImgSrcByID(Convert.ToInt32(id));
                    //        if (string.IsNullOrEmpty(imgsrc)) continue;
                    //        strb.AppendFormat(@"<div class='thumbnail' style='float:left;'>
                    //                                <a href='{0}' class='fancybox-button'  >
                    //                                    <img id='img_Image' src='{0}' style='max-width: 156px;'>
                    //                                </a>
                    //                        </div>", imgsrc);
                    //    }
                    //}
                    //value = strb.ToString();
                    value = string.Format("<div class='filepanel' data-readonly='true' data-multi='true' data-name='{0}' data-value='{1}' data-auto='true' data-width='{2}' data-height='{3}'></div>", _filed.Name, value, _filed.ImageWidth, _filed.ImageHeight);
                    break;
                case "附件":
                    if (!string.IsNullOrEmpty(value))
                    {
                        List<Base_Attachment> attachmentlist = new List<Base_Attachment>();
                        try
                        {
                            attachmentlist = JsonHelper.Single.JsonToObject<List<Base_Attachment>>(value);
                        }
                        catch (Exception)
                        {

                        }
                        string strs = string.Empty;
                        foreach (var attachment in attachmentlist)
                        {
                            strs += string.Format(@"<div data-field='{0}' data-id='{1}' data-name='{2}'  data-size='{3}' data-attachment='{4}' id=""file_{0}{1}"" class=""uploadifyQueueItem"">
                                        <div class=""cancel"">
                                            <a href=""javascript:jQuery('#file_{0}').uploadifyCancel('{1}')"">
                                                <img src=""/Content/plugin/uploadify/cancel.png"" border=""0"">
                                            </a>
                                        </div>
                                           <div style=""float:right;""><a title=""下载"" target=""_balnk"" href=""{4}""><img style=""height: 15px;margin-right: 5px;"" src=""/Content/plugin/uploadify/down.png"" /></a></div>                                        
                                        <span class=""fileName"">{2} ({3})</span><span class=""percentage""></span>
                                    </div>", _filed.Name, attachment.ID, attachment.Name, attachment.Size, attachment.Attachment);
                        }
                        value = string.Format(@"<div id=""file_{0}Queue"" class=""uploadifyQueue uploadifyQueueDB"">" + strs + "</div>", _filed.Name);
                    }
                    break;
            }


            return new HtmlString(!string.IsNullOrEmpty(value) ? value : defaultvalue);
        }
        public static HtmlString GetValue(int fieldid, int ID)
        {
            var _filed = SystemSetService.Field.Get(fieldid).Data;
            if (_filed == null) { return new HtmlString(""); ; }
            Dictionary<string, string> PageData;
            string value = "";
            if (ID > 0)
            {
                string CacheKey = _filed.EntityName + ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(_filed.EntityName, ID);
                });
                if (PageData.ContainsKey(_filed.Name))
                {
                    value = PageData[_filed.Name];
                }
                if (_filed.FieldType == EnumFieldType.时间.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = ConvertHelper.Single.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else if (_filed.FieldType == EnumFieldType.日期.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                    }
                }
            }
            return new HtmlString(value);
        }
        #endregion

        #region 查询控件
        public static HtmlString InitSearchControler(SearchField field)
        {
            StringBuilder strb = new StringBuilder();
            var _style = string.Empty;
            if (field.SearchControlWidth == 0) field.SearchControlWidth = 117;
            if (field.SearchControlWidth > 0) { _style = "width:" + field.SearchControlWidth + "px"; }
            switch (field.FieldType)
            {
                case "两个选项":
                case "选项集":
                case "选项集多选":
                    strb.Append("<select style='width:" + (field.SearchControlWidth + 8) + "px' data-actions-box='true' title='--选择--' data-width='" + (field.SearchControlWidth + 8) + "px' data-sort='" + field.Sort + "' id='" + field.Field + "' class='SearchField selectpicker ' " + (field.IsMultiple.HasValue && field.IsMultiple.Value ? "multiple" : "") + " data-field='" + field.Field + "' data-type='" + field.FieldType + "'>");
                    if (!field.IsMultiple.HasValue || !field.IsMultiple.Value)
                        strb.AppendFormat("<option value=''>全部</option>");
                    foreach (var item in field.DictionaryList)
                    {
                        strb.AppendFormat("<option value='{0}'>{1}</option>", item.Value, item.Name);
                    }
                    strb.Append(" </select>");
                    break;
                case "整数":
                case "浮点数":
                case "货币":
                    strb.Append(@"<div class='float_SearchInput'>
                                    <input  data-field='" + field.Field + @"' data-type='" + field.FieldType + @"' data-opera='>='  class='SearchField search-control  '>
                                    <span>至</span>
                                    <input  data-field='" + field.Field + @"' data-type= '" + field.FieldType + @"' data-opera='<=' class='SearchField search-control  '>
                                 </div>");
                    break;
                case "单行文本":
                case "多行文本":
                    strb.Append("<input style='width:" + field.SearchControlWidth + "px' class='search-control SearchField' data-field='" + field.Field + "' data-type='" + field.FieldType + "' >");
                    break;
                case "日期":
                    strb.Append(@"<div style='  width: 345px;'>
                                    <input style='width:80px;' data-field='" + field.Field + @"' data-type='日期' data-opera='>=' onclick=""laydate({ istime: false, format: 'YYYY-MM-DD' })"" class='SearchField search-control  laydate-icon date'>
                                    <span style='float: left;margin-top: 2px; width: 15px;'>至</span>
                                    <input style='width:80px;'data-field='" + field.Field + @"' data-type='日期' data-opera='<=' onclick=""laydate({ istime: false, format: 'YYYY-MM-DD' })""  class='SearchField search-control  laydate-icon date'>
                                 </div>");
                    break;
                case "时间":
                    strb.Append(@"<div style='  width: 350px;'>
                                    <input  style='width:135px;' data-field='" + field.Field + @"' data-type='时间' data-opera='>=' onclick=""laydate({istime: true, format: 'YYYY-MM-DD hh:mm'})"" class='SearchField search-control  laydate-icon date' >
                                    <span style='float: left;margin-top: 2px; width: 15px;'>至</span>
                                    <input  style='width:135px;' data-field='" + field.Field + @"' data-type='时间' data-opera='<=' onclick=""laydate({istime: true, format: 'YYYY-MM-DD hh:mm'})""  class='SearchField search-control  laydate-icon date'>
                                 </div>");
                    break;
                case "关联其他表":
                    strb.Append("<input style='width:" + field.SearchControlWidth + "px' class='search-control  SearchField' data-field='" + field.Field + "' data-type='单行文本' >");
                    break;
                case "关联其他表多选"://Sys_Department_ForDepartment$Name
                    strb.Append("<input style='width:" + field.SearchControlWidth + "px' class='search-control  SearchField' data-field='dbo.Get" + field.RelationEntity + "Names(" + field.Field + ")' data-type='单行文本' >");
                    break;
            }
            return new HtmlString(strb.ToString());
        }
        public static HtmlString InitRelationFieldSearchControler(SearchField field)
        {
            StringBuilder strb = new StringBuilder();
            var _style = string.Empty;
            if (field.SearchControlWidth == 0) field.SearchControlWidth = 100;
            if (field.SearchControlWidth > 0) { _style = "width:" + field.SearchControlWidth + "px"; }
            switch (field.FieldType)
            {
                case "两个选项":
                case "选项集":
                case "选项集多选":
                    strb.Append("<select style='" + _style + "' data-actions-box='true' title='" + field.Title + "' data-width='" + field.SearchControlWidth + "px' data-sort='" + field.Sort + "' id='" + field.Field + "' class='SearchField selectpicker selectpicker' " + (field.IsMultiple.HasValue && field.IsMultiple.Value ? "multiple" : "") + " data-field='" + field.Field + "' data-type='" + field.FieldType + "'>");
                    if (!field.IsMultiple.HasValue || !field.IsMultiple.Value)
                        strb.Append("<option value=''>全部</option>");
                    foreach (var item in field.DictionaryList)
                    {
                        strb.AppendFormat("<option value='{0}'>{1}</option>", item.Value, item.Name);
                    }
                    strb.Append(" </select>");
                    break;
                case "整数":
                case "单行文本":
                case "多行文本":
                    strb.Append("<input style='" + _style + "' class='inputText search-control SearchField' data-field='" + field.Field + "' data-type='" + field.FieldType + "' placeholder='" + field.Title + "'>");
                    break;
                case "日期":
                    strb.Append(@"<div>
                                    <input " + _style + @" data-field='" + field.Field + "' data-type='日期' data-opera='>=' onclick=\"laydate({ istime: false, format: 'YYYY-MM-DD' })\" placeholder='" + field.Title + @"[起始]' class='inputText SearchField search-control laydate-icon date'>
                                    <span style=' float: left; margin-top: 2px; width: 15px;'>至</span>
                                    <input " + _style + @" data-field='" + field.Field + "' data-type='日期' data-opera='<=' onclick=\"laydate({ istime: false, format: 'YYYY-MM-DD' })\" placeholder='" + field.Title + @"[结束]' class='inputText SearchField search-control laydate-icon date'>
                                 </div>");
                    break;
                case "时间":
                    strb.Append(@"<div>
                                    <input  style='" + _style + @"' data-field='" + field.Field + "' data-type='时间' data-opera='>=' onclick=\"laydate({istime: true, format: 'YYYY-MM-DD hh:mm:ss'})\" placeholder='" + field.Title + @"[起始]' class='inputText SearchField search-control laydate-icon date'>
                                    <span style=' float: left; margin-top: 2px; width: 15px;'>至</span>
                                    <input  style='" + _style + @"' data-field='" + field.Field + "' data-type='时间' data-opera='<=' onclick=\"laydate({istime: true, format: 'YYYY-MM-DD hh:mm:ss'})\" placeholder='" + field.Title + @"[结束]' class='inputText SearchField search-control laydate-icon date'>
                                 </div>");
                    break;
                case "关联其他表":
                case "关联其他表多选":
                    strb.AppendFormat(@"<div class='search-control'>
                                            <span class='inputText search-span' id='span_" + field.ID + @"0' data-fieldid='" + field.ID + @"' data-id='0' data-value='' onclick='layoutlist.open_field_dialog(this)' style='float:left;' data-title='选择" + field.Title + @"'>选择" + field.Title + @"</span>
                                            <input data-id='" + field.ID + @"' id='input_" + field.ID + @"0' class='SearchField' type='hidden' data-field='" + field.Field + @"' data-type='" + field.FieldType + @"'>
                                            <i id='icon_" + field.ID + @"0' class='search-icon icon-search nav-search-icon' data-fieldid='" + field.ID + @"' data-id='0' data-value='' onclick='layoutlist.open_field_dialog(this)' style='float: left; margin-top: 9px; margin-left: -20px; '></i>
                                        </div>");
                    break;
            }
            return new HtmlString(strb.ToString());
        }

        #endregion

        #region 自定义控件
        public static HtmlString InitCustomizeMetadataHtml(int fieldid, int ID, string attr, string className = "", string defaultValue = "", string cusfieldname = "")
        {
            StringBuilder strb = new StringBuilder();
            var _filed = SystemSetService.Field.Get(fieldid).Data;
            if (_filed == null) { return new HtmlString(""); }
            string EntityName = _filed.EntityName;
            if (string.IsNullOrEmpty(_filed.FieldType)) _filed.FieldType = "text";
            Dictionary<string, string> PageData;
            string value = _filed.DefaultValue;
            if (!string.IsNullOrEmpty(defaultValue)) value = defaultValue;
            if (ID > 0)
            {
                string CacheKey = EntityName + ID;
                PageData = CacheHelper.Single.TryGet<Dictionary<string, string>>(CacheKey, 0, () =>
                {
                    return SystemSetService.Common.GetPageFormData(EntityName, ID);
                });
                if (PageData.ContainsKey(_filed.Name))
                {
                    value = PageData[_filed.Name];
                }
                if (_filed.FieldType == EnumFieldType.时间.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = ConvertHelper.Single.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else if (_filed.FieldType == EnumFieldType.日期.ToString())
                {
                    if (ConvertHelper.Single.IsDateTime(value))
                    {
                        value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
                    }
                }
            }

            if (_filed.MaxLength > 0)
            {
                attr += string.Format(" maxlength='{0}'", _filed.MaxLength);
            }
            if (!string.IsNullOrEmpty(_filed.Valid))
                _filed.Valid = _filed.Valid.Replace("[field]", _filed.Title);

            var placeholder = _filed.Placeholder;
            int _value = -1;
            List<Sys_Dictionary> list = new List<Sys_Dictionary>();
            if (string.IsNullOrEmpty(cusfieldname))
            {
                cusfieldname = "Cus-" + _filed.Name;
            }
            switch (_filed.FieldType)
            {
                case "两个选项":
                    list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _filed.ID },
                          new Pagination() { Page = 1, PageSize = 20 }).Data;
                    if (!string.IsNullOrEmpty(value)) { _value = Convert.ToInt32(value); }
                    strb.Append("<input id='" + cusfieldname + "' name='" + cusfieldname + "'  class='CustomizeController radiovalue " + _filed.Valid + "' style='position: absolute; top: -1429px;' type='hidden' value='" + value + "' /><div class='fl'>");
                    var z = 0;
                    foreach (var item in list)
                    {
                        z++;
                        strb.Append("<div class='fl'><input " + (item.Value == _value ? "checked" : "") + " value='" + item.Value + "' data-name='" + cusfieldname + "' name='radio_" + cusfieldname + "' id='" + cusfieldname + "_" + z + "' type='radio' ><label for='" + cusfieldname + "_" + z + "'>&nbsp;" + item.Name + " &nbsp;&nbsp;</label> </div>");
                    }
                    strb.Append(" </div>");
                    break;
                case "选项集":
                    if (!string.IsNullOrEmpty(value)) { _value = Convert.ToInt32(value); }
                    list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _filed.ID },
                          new Pagination() { Page = 1, PageSize = 20 }).Data;
                    strb.Append(" <select class='CustomizeController form-tag field valid " + className + " " + _filed.Valid + " ' id='" + cusfieldname + "' name='" + cusfieldname + "' " + attr + ">");
                    if (string.IsNullOrEmpty(defaultValue))
                        strb.AppendFormat("<option value=''>请选择</option>");
                    foreach (var item in list)
                    {
                        strb.AppendFormat("<option value='{0}' " + (item.Value == _value ? "selected='selected'" : "") + " >{1}</option>", item.Value, item.Name);
                    }
                    strb.Append(" </select>");
                    break;
                case "选项集多选":
                    list = SystemSetService.Dictionary.GetDictionaryPageingList(new Sys_Dictionary() { FieldID = _filed.ID },
                         new Pagination() { Page = 1, PageSize = 20 }).Data;
                    strb.Append("<input class='CustomizeController checkboxvalue  " + _filed.Valid + "' style='position: absolute; top: -1429px;' type='hidden' value='" + value + "' id='" + cusfieldname + "' name='" + cusfieldname + "' /><div class='fl'>");
                    var i = 0;
                    foreach (var item in list)
                    {
                        var vs = value.Split(new char[] { ',' });
                        i++;
                        strb.Append("<div class='fl'><input name='chk_" + cusfieldname + "' id='" + cusfieldname + "_" + i + "' value='" + item.Value + "' type='checkbox' " + (vs.Any(e => e == item.Value.ToString()) ? "checked" : "") + " ><label for='" + cusfieldname + "_" + i + "'>&nbsp;" + item.Name + " &nbsp;&nbsp;</label> </div>");
                    }
                    strb.Append(" </div>");
                    break;
                case "单行文本":
                    strb.Append("<input id='" + cusfieldname + "' name='" + cusfieldname + "' type='text' class='CustomizeController inputText " + className + "  valid  " + _filed.Valid + "' value='" + value + "'  " + attr + "  placeholder='" + placeholder + "' >");
                    break;
                case "多行文本":
                    strb.Append("<textarea class='CustomizeController form-tag field inputText " + className + "   " + _filed.Valid + " ' style='height:60px;'  id='" + cusfieldname + "' name='" + cusfieldname + "'  " + attr + "  placeholder='" + placeholder + "' >" + value + "</textarea>");
                    break;
                case "整数":
                    strb.Append("<input class='CustomizeController form-tag inputText  " + className + "  numberbox " + _filed.Valid + " ' id='" + cusfieldname + "' name='" + cusfieldname + "' type='text' value='" + value + "' " + attr + "  placeholder='" + placeholder + "' >");
                    break;
                case "浮点数":
                case "货币":
                    strb.Append("<input id='" + cusfieldname + "' name='" + cusfieldname + "'  type='text' class='CustomizeController inputText " + className + "  valid  " + _filed.Valid + "' value='" + value + "'  " + attr + "  placeholder='" + placeholder + "' >");
                    break;
                case "日期":
                    strb.Append("<input  onclick=\"laydate({ format: 'YYYY-MM-DD'})\" class='CustomizeController form-tag field inputText laydate-icon date  " + _filed.Valid + " ' id='" + cusfieldname + "' name='" + cusfieldname + "'  value='" + value + "' " + attr + "  placeholder='" + placeholder + "' >");
                    break;
                case "时间":
                    strb.Append("<input  onclick=\"laydate({istime: true, format: 'YYYY-MM-DD hh:mm:ss'}) \" class='CustomizeController form-tag field inputText laydate-icon time  " + _filed.Valid + " ' id='" + cusfieldname + "' name='" + cusfieldname + "'   value='" + value + "' " + attr + "  placeholder='" + placeholder + "' >");
                    break;
                case "关联其他表":
                    string rand = (new DateTime().Ticks).ToString();
                    var fieldname = string.Empty;
                    int _rvalue = -1;
                    if (!string.IsNullOrEmpty(value)) { _rvalue = Convert.ToInt32(value); }
                    if (_filed.IsDropDownSource.HasValue && _filed.IsDropDownSource.Value)
                    {
                        Dictionary<int, string> _datalist = SystemSetService.Entity.GetFKDropDownSourceData(_filed.RelationEntity);
                        strb.Append(" <select class='CustomizeController form-tag field valid " + className + " " + _filed.Valid + " ' id='" + cusfieldname + "' " + attr + ">");
                        strb.AppendFormat("<option value=''>请选择</option>");
                        foreach (var item in _datalist)
                        {
                            strb.AppendFormat("<option value='{0}' " + (item.Key == _rvalue ? "selected='selected'" : "") + " >{1}</option>", item.Key, item.Value);
                        }
                        strb.Append(" </select>");
                    }
                    else
                    {
                        if (_rvalue != -1)
                        {
                            fieldname = SystemSetService.Entity.GetFKField_Name(_filed.RelationEntity, _rvalue);
                        }
                        if (!string.IsNullOrEmpty(_filed.OpenLink) && !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(fieldname))
                        {
                            strb.AppendFormat(@"<span id=""span_{0}{1}"" ondblclick=""OpenValue('{3}${4}${5}')"" style=""    width: 245px;  float: left;  padding: 7px 6px;"" class=""form-tag  text-box  inputText single-line"">{2}</span>", _filed.ID, rand, fieldname, _filed.OpenLink, value, fieldname);
                        }
                        else
                        {
                            strb.AppendFormat(@"<span id=""span_{0}{1}""  style="" width: 245px; float: left;  padding: 7px 6px;"" class=""form-tag inputText text-box single-line"">{2}</span>", _filed.ID, rand, fieldname);

                        }
                        strb.AppendFormat(@"<input data-id='{0}' id=""input_{0}{1}""  class='CustomizeController form-tag field {3} '  type=""text"" style=""position: absolute; top: -1429px;border: 0;height: 0px;  color: white;"" value=""{2}"" >", _filed.ID, rand, value, _filed.Valid);
                        //strb.AppendFormat(@"<i id=""icon_{0}{1}"" class=""icon-search nav-search-icon select_filedvalue"" data-fieldid=""{0}"" data-id=""{1}"" data-value=""{2}"" onclick=""open_field_dialog(this)""></i>", _filed.ID, rand, value);
                        strb.AppendFormat(@"<button id=""icon_{0}{1}"" data-fieldid=""{0}"" data-id=""{1}"" data-value=""{2}"" onclick=""open_field_dialog(this)"" title=""选择或更改此字段的值"" type=""button""  class=""select-btn"" >...</button>", _filed.ID, rand, value);
                    }
                    break;
                case "关联其他表多选":
                    string rand2 = (new DateTime().Ticks).ToString();
                    var fieldname2 = string.Empty;
                    if (!string.IsNullOrEmpty(value))
                    {
                        fieldname2 = SystemSetService.Entity.GetFKField_Names(_filed.RelationEntity, value);
                    }
                    strb.AppendFormat(@"<span id=""span_{0}{1}""  style=""    width: 245px;  float: left;  padding: 7px 6px;"" class=""form-tag inputText text-box single-line"">{2}</span>", _filed.ID, rand2, fieldname2);
                    strb.AppendFormat(@"<input data-id='{0}' id=""input_{0}{1}"" class='CustomizeController form-tag field {3} '   type=""text"" style=""position: absolute; top: -1429px;border: 0;height: 0px;  color: white;"" value=""{2}"" >", _filed.ID, rand2, value, _filed.Valid);
                    //strb.AppendFormat(@"<i id=""icon_{0}{1}"" class=""icon-search nav-search-icon select_filedvalue"" data-fieldid=""{0}"" data-id=""{1}"" data-value=""{2}"" onclick=""open_field_dialogmulti(this)""></i>", _filed.ID, rand2, value);
                    strb.AppendFormat(@"<button id=""icon_{0}{1}"" data-fieldid=""{0}"" data-id=""{1}"" data-value=""{2}"" onclick=""open_field_dialogmulti(this)"" title=""选择或更改此字段的值"" type=""button""  class=""select-btn"" >...</button>", _filed.ID, rand2, value);

                    break;
                case "图像":
                    strb.AppendFormat(@"<div  data-id='{0}' class='fileinput fileinput-new' data-provides='fileinput' style='text-align:center;margin-left: 5px;'>
                                            <div class='fileinput-new thumbnail' style=' height: 112px; width: 131px;'>
                                               <a href='{4}' class='fancybox-button'><img style='height: 100px; width: 120px;' src='{3}' /></a>
                                            </div>
                                            <div class='fileinput-preview fileinput-exists thumbnail' style=' height: 112px; width: 130px;  '></div>
                                            <div>
                                                <span class='btn default btn-file'>
                                                    <span class='fileinput-new'>选择</span>
                                                    <span class='fileinput-exists'>重选</span>
                                                    <input type='file' class='UploadFile' id='file_{1}' name='file_{1}' accept='image/jpeg,image/png,image/gif'>
                                                    <input type='hidden' class='UploadTag' id='{1}' value='{2}' />
                                                </span>
                                            </div>
                                        </div>", _filed.ID, _filed.Name, value, (string.IsNullOrEmpty(value) ? "/Content/plugin/bootstrap-fileinput/image/backgroup.png" : value), value.Replace("x" + ApplicationContext.AppSetting.DefaultThumbnail_Size, "images"));
                    break;
                case "附件":
                    strb.AppendFormat("<input name='{0}'  class='CustomizeController form-tag field {2} ' id='{0}' value='{1}' type='text' style='position: absolute; top: -1429px;border: 0;height: 0px;  color: white;' >", _filed.Name, value, _filed.Valid);
                    strb.AppendFormat("<input name='file_{0}' data-id='{0}' id='file_{0}' type='file' class='filesupload file-loading'>", _filed.Name);
                    if (!string.IsNullOrEmpty(value))
                    {
                        List<Base_Attachment> attachmentlist = new List<Base_Attachment>();
                        try
                        {
                            attachmentlist = JsonHelper.Single.JsonToObject<List<Base_Attachment>>(value);
                        }
                        catch (Exception)
                        {

                        }
                        string strs = string.Empty;
                        foreach (var attachment in attachmentlist)
                        {
                            strs += string.Format(@"<div data-field='{0}' data-id='{1}' data-name='{2}'  data-size='{3}' data-attachment='{4}' id=""file_{0}{1}"" class=""uploadifyQueueItem"">
                                        <div class=""cancel"">
                                            <a href=""javascript:jQuery('#file_{0}').uploadifyCancel('{1}')"">
                                                <img src=""/Content/plugin/uploadify/cancel.png"" border=""0"">
                                            </a>
                                        </div>
                                           <div style=""float:right;""><a title=""下载"" target=""_balnk"" href=""{4}""><img style=""height: 15px;margin-right: 5px;"" src=""/Content/plugin/uploadify/down.png"" /></a></div>                                        
                                        <span class=""fileName"">{2} ({3})</span><span class=""percentage""></span>
                                    </div>", _filed.Name, attachment.ID, attachment.Name, attachment.Size, attachment.Attachment);
                        }
                        strb.AppendFormat(@"<div id=""file_{0}Queue"" class=""uploadifyQueue uploadifyQueueDB"">" + strs + "</div>", _filed.Name);
                    }

                    break;
                default:
                    strb.Append("<input class='CustomizeController form-tag field  " + className + " " + _filed.Valid + " ' id='" + cusfieldname + "' name='" + cusfieldname + "' type='text' value='" + value + "'  >");
                    break;
            }

            return new HtmlString(strb.ToString());
        }
        #endregion
    }

    public class FormControllerModel
    {
        /// <summary>
        ///字段ID
        /// </summary>
        public int FieldID { get; set; }
        /// <summary>
        /// 当前数据ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 控件属性
        /// </summary>
        public string Attr { get; set; }
        /// <summary>
        /// 控件Class
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        public FormFieldModel FormField { get; set; }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool? IsCustomize { get; set; }
        /// <summary>
        /// 自定义控件名称
        /// </summary>
        public string CustomizeFieldName { get; set; }
        public bool? IsReadOnly { get; set; }
    }
}

