using Base.IService;
using Base.Model.Sys.Model;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Utility;
using Web.Utility;

namespace Web.Controllers
{
    public class ReportController : AdminBaseController
    {
        public IReportService service { get; set; }
        public ReportController()
        {
            EntityName = "Sys_Report";
        }

        public ActionResult Index(int id)
        {
            ViewBag.GridFields = service.GetReportColumns(id);
            ViewBag.SearchField = service.GetReportParameters(id);
            return View(service.Get(id).Data);
        }

        [HttpPost]
        public JsonResult _ReportList(Pagination rq, AdminCredential User)
        {
            ApplicationContext.KendoFilter.SetPagination(Request.Form, rq);
            Session[rq.vid.ToString() + "Filter"] = rq.Filter;
            return new ListJsonResult
            {
                Data = JsonConvert.DeserializeObject(service.GetReportData(rq, User)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public FileResult Export(Pagination rq, AdminCredential User)
        {
            #region 报表导出
            var report = service.Get(rq.vid).Data;
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = (short)24;// NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            IFont font = workbook.CreateFont(); //创建一个字体样式对象
            font.FontHeightInPoints = 10;//字体大小
            font.FontName = "宋体"; //和excel里面的字体对应
            font.Boldweight = short.MaxValue;//字体加粗
            font.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            style.SetFont(font); //将字体样式赋给样式对象

            NPOI.SS.UserModel.ISheet sheet1 = workbook.CreateSheet(report.Name);
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.Height = 430;

            var FieldList = JsonConvert.DeserializeObject<List<ViewFieldModel>>(service.GetReportColumns(rq.vid.Value));
            int i = 0;
            foreach (var item in FieldList)
            {
                ICell cell = row1.CreateCell(i);
                cell.SetCellValue(item.Title);
                cell.CellStyle = style;
                i++;
            }
            rq.Filter = Session[rq.vid.ToString() + "Filter"].ToString();
            DataTable dt = service.GetExportReportData(rq, User);
            //将数据逐步写入sheet1各个行
            for (int z = 0; z < dt.Rows.Count; z++)
            {
                var row = dt.Rows[z];
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(z + 1);
                int c = 0;
                foreach (var item in FieldList)
                {
                    rowtemp.CreateCell(c++).SetCellValue(row[item.Field].ToString());
                }
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", report.Name + ".xls");
            #endregion
        }
    }
}
