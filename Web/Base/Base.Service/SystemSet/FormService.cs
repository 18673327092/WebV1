using Base.Model;
using Base.Model.Sys.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.ResultModel;

namespace Base.Service.SystemSet
{
    public class FormService : BaseService<Sys_Form>
    {
        private static FormService formService = null;
        public static FormService Single
        {
            get
            {
                if (formService == null)
                {
                    formService = new FormService();
                }
                return formService;
            }
        }

        public Sys_Form GetForm(int entityid, int formid = 0)
        {
            return ApplicationContext.Cache.TryGet(string.Format("GetForm{0}{1}", entityid, formid), 0, () =>
             {
                 BaseResult result = new BaseResult();
                 using (var db = CreateDao())
                 {
                     Sys_Form model = new Sys_Form();
                     if (formid == 0)
                     {
                         model = db.First<Sys_Form>("SELECT top 1 * FROM Sys_Form WHERE EntityID=@0 AND IsMain=1", entityid);
                     }
                     else
                     {
                         model = db.First<Sys_Form>("SELECT * FROM Sys_Form WHERE ID=@0", formid);
                     }
                     try
                     {
                         model.FormShowFieldsDB = JsonConvert.DeserializeObject<List<FormSectionModel>>(model.FormShowFields);
                         model.FormHideFieldsDB = JsonConvert.DeserializeObject<List<int>>(model.FormHideFields);

                     }
                     catch (Exception)
                     {

                     }
                     return model;
                 }
             });

        }
    }
}
