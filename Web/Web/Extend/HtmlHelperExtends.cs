using Base.Model.Sys.Model;
using Web.Utility.Extend;
using Web.Utility.Extend.Model;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtends
    {
        public static HtmlString FormControl(this HtmlHelper helper, FormFieldModel formfield, int ID = 0, string defaultValue = "")
        {
            return HtmlControllHelper.InitMetadataHtml(new FormControllerModel()
            {
                FieldID = formfield.FieldID,
                ID = ID,
                DefaultValue = defaultValue,
                FormField = formfield,
            });
        }

        public static HtmlString FormControl(this HtmlHelper helper, FormControllerModel model)
        {
            return HtmlControllHelper.InitMetadataHtml(model);
        }

        public static HtmlString FormControl(this HtmlHelper helper, int fieldid, int ID = 0, string defaultValue = "")
        {
            return HtmlControllHelper.InitMetadataHtml(new FormControllerModel()
            {
                FieldID = fieldid,
                ID = ID,
                DefaultValue = defaultValue
            });
        }

        /// <summary>
        /// 自定义控件
        /// </summary>
        public static HtmlString FormCustomizeControl(this HtmlHelper helper, int fieldid, int ID = 0, string className = "",
            string customizeFieldName = "", string defaultValue = "", string attr = "")
        {
            return HtmlControllHelper.InitMetadataHtml(new FormControllerModel()
            {
                FieldID = fieldid,
                ID = ID,
                DefaultValue = defaultValue,
                IsCustomize = true,
                CustomizeFieldName = customizeFieldName,
                Attr = attr,
                ClassName = " CustomizeController " + className
            });
        }
        public static HtmlString FormCustomizeControl(this HtmlHelper helper, FormControllerModel model)
        {
            model.IsCustomize = true;
            model.ClassName = " CustomizeController " + model.ClassName;
            return HtmlControllHelper.InitMetadataHtml(model);
        }


        public static HtmlString FormHiddenControl(this HtmlHelper helper, int fieldid, int ID = 0, string defaultvalue = "")
        {
            return HtmlControllHelper.FormHiddenControl(fieldid, ID, defaultvalue);
        }

        public static HtmlString FormHiddenControl(this HtmlHelper helper, string fieldname, int ID = 0, string EntityName = "", string defaultvalue = "")
        {
            return HtmlControllHelper.FormHiddenControl(fieldname, ID, EntityName, defaultvalue);
        }

        public static HtmlString FormSpanControl(this HtmlHelper helper, int fieldid, int ID = 0)
        {
            return HtmlControllHelper.FormSpanControl(fieldid, ID);
        }


        public static HtmlString GetText(this HtmlHelper helper, int fieldid, int ID, string defaultvalue = "")
        {
            return HtmlControllHelper.GetText(fieldid, ID, defaultvalue);
        }

        public static HtmlString GetValue(this HtmlHelper helper, int fieldid, int ID)
        {
            return HtmlControllHelper.GetValue(fieldid, ID);
        }
        public static HtmlString InitSearchControler(this HtmlHelper helper, SearchField field)
        {
            field.ClassName = "inputText";
            return HtmlControllHelper.InitSearchControler(field);
        }
        public static HtmlString InitRelationFieldSearchControler(this HtmlHelper helper, SearchField field)
        {
            return HtmlControllHelper.InitRelationFieldSearchControler(field);
        }


    }
}