using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Optimization;
using Utility;
namespace Utility.Components
{

    public class BundleConfigHelper
    {
        //资源路径
        private static string PATH = "~/Content/base";
        /// <summary>
        /// 公共js
        /// </summary>
        public static string common_js = PATH + "/js/common.js";
        /// <summary>
        /// 弹框JS
        /// </summary>
        public static string dialog_js = PATH + "/js/dialog.js";
        /// <summary>
        ///cookie JS
        /// </summary>
        public static string cookie_js = "~/Content/plugin/jquery/jquery.cookie.js";
        /// <summary>
        /// 列表基础js
        /// </summary>
        public static string gridbase_js = "~/Content/plugin/page/js/gridbase.js";
        /// <summary>
        ///列表样式
        /// </summary>
        public static string grid_css = PATH + "/css/grid.css";
        /// <summary>
        /// pagegrid.js
        /// </summary>
        public static string pagegrid_js = PATH + "/js/pagegrid.js";

        public static void Init(BundleCollection bundles)
        {
            //Home 主页
            bundles.Add(new ScriptBundle("~/home/js").Include(common_js, PATH + "/js/home.js"));

            //登录页面
            bundles.Add(new ScriptBundle("~/login/js").Include(common_js, dialog_js, "~/Content/plugin/Validform_v5.3.2.js", cookie_js, PATH + "/js/login.js"));

            //模版页面
            bundles.Add(new StyleBundle("~/_layout/css").Include("~/Content/plugin/font-awesome/css/font-awesome.css",
                   "~/Content/plugin/layui/build/css/layui.css", PATH + "/css/layout.css", PATH + "/css/toobar.css"));
            bundles.Add(new ScriptBundle("~/_layout/js").Include("~/Content/plugin/page/js/layout.js", common_js, dialog_js));


            bundles.Add(new StyleBundle("~/_layoutform/css").Include(PATH + "/css/form.css"));

            //表单模版页面
            bundles.Add(new ScriptBundle("~/_layoutform/js").Include(
                  "~/Content/plugin/bootstrap-fileinput/js/jquery-form.js",
                  "~/Content/plugin/jquery.validate/jquery.validate.addMethod.js",
                  "~/Content/plugin/jquery.validate/jquery.metadata.js",
                  "~/Content/plugin/jquery.validate/messages_zh.js",
                  cookie_js,
                  "~/Content/plugin/laydate/laydate.js",
                  PATH + "/js/pageform.js",
                  PATH + "/js/layoutform.js"));

            //列表模版页面
            bundles.Add(new StyleBundle("~/_layoutlist/css").Include(PATH + "/css/layoutlist.css", grid_css));
            bundles.Add(new ScriptBundle("~/_layoutlist/js").Include(common_js, dialog_js, PATH + "/js/pagegrid.js", PATH + "/js/layoutlist.js"));

            //表单页面
            bundles.Add(new StyleBundle("~/form/css").Include(PATH + "/css/form.css"));

            //普通列表页面
            bundles.Add(new StyleBundle("~/comm_grid/css").Include(PATH + "/css/grid.css"));
            bundles.Add(new ScriptBundle("~/comm_grid/js").Include(gridbase_js, PATH + "/js/pagegrid.js"));


            //普通表单验证
            bundles.Add(new ScriptBundle("~/formvalid/js").Include(
               "~/Content/plugin/bootstrap-fileinput/js/jquery-form.js",
               "~/Content/plugin/jquery.validate/jquery.validate.addMethod.js",
               "~/Content/plugin/jquery.validate/jquery.metadata.js",
               "~/Content/plugin/jquery.validate/messages_zh.js", PATH + "/js/formvalid.js"));


            //报表页面
            bundles.Add(new StyleBundle("~/report/css").Include("~/Content/base/css/report.css"));
            bundles.Add(new ScriptBundle("~/report/js").Include(gridbase_js, "~/Content/plugin/laydate/laydate.js",
              pagegrid_js, PATH + "/js/report.js"));

            #region 视图
            bundles.Add(new StyleBundle("~/view/css").Include(PATH + "/modules/view/css/view.css"));

            bundles.Add(new ScriptBundle("~/view/js").Include(gridbase_js, pagegrid_js, PATH + "/modules/view/js/view.js"));
            //编辑列
            bundles.Add(new StyleBundle("~/editcolumns/css").Include(PATH + "/modules/view/css/editcolumns.css"));
            bundles.Add(new ScriptBundle("~/editcolumns/js").Include(common_js, dialog_js, pagegrid_js, PATH + "/modules/view/js/editcolumns.js"));
            //添加列
            bundles.Add(new StyleBundle("~/addcolumns/css").Include(grid_css, PATH + "/modules/view/css/addtcolumns.css"));
            bundles.Add(new ScriptBundle("~/addcolumns/js").Include(gridbase_js, common_js, pagegrid_js, PATH + "/modules/view/js/addcolumns.js"));

            bundles.Add(new StyleBundle("~/relationfieldselectpanel/css").Include(PATH + "/css/layoutlist.css", grid_css, PATH + "/css/relationfieldselectpanel.css"));
            bundles.Add(new ScriptBundle("~/relationfieldselectpanel/js").Include(common_js, gridbase_js, pagegrid_js, PATH + "/js/relationfieldselectpanel.js"));
            bundles.Add(new ScriptBundle("~/relationfieldselectpanelmulti/js").Include(common_js, dialog_js, gridbase_js, pagegrid_js, PATH + "/js/relationfieldselectpanelmulti.js"));
            #endregion

            #region 权限相关
            //页面按钮权限配置
            bundles.Add(new ScriptBundle("~/pageconfig/js").Include(PATH + "/js/pageconfig.js"));
            //数据权限配置
            bundles.Add(new ScriptBundle("~/dataconfig/js").Include(gridbase_js, PATH + "/js/dataconfig.js"));

            //分派
            bundles.Add(new StyleBundle("~/assign/css").Include(grid_css, "~/Content/base/css/assign.css"));
            bundles.Add(new ScriptBundle("~/assign/js").Include(gridbase_js, common_js, pagegrid_js, dialog_js, PATH + "/js/assign.js"));

            //共享
            bundles.Add(new StyleBundle("~/share/css").Include(grid_css, PATH + "/css/share.css"));
            bundles.Add(new ScriptBundle("~/share/js").Include(gridbase_js, common_js, pagegrid_js, dialog_js, PATH + "/js/share.js"));
            #endregion
        }

    }
}
