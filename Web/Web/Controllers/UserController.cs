using Base.IService;
using Base.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utility;
using Utility.Components;
using Utility.ResultModel;
using Web.Attribute;
using Web.Utility;

namespace Web.Controllers
{
    public class UserController : AdminBaseController
    {
        //
        // GET: /User/
        //
        // GET: /Customer/
        public IUserService userService { get; set; }
        public IRoleService roleService { get; set; }
        public IUserAndRoleService userAndRoleService { get; set; }
        public IJobService jobService { get; set; }
        public IJobUsersService jobUsersService { get; set; }
        public UserController()
        {
            EntityName = "Sys_User";
        }

        [PageAuth]
        public ActionResult Form(int id = 0)
        {
            base.BaseForm(id);
            if (id == 0)
            {
                ViewBag.Data = ToJson(new Sys_User()
                {
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
                return View();
            }
            else
            {
                Sys_User entity = userService.Get(id).Data;
                ViewBag.Data = ToJson(entity);
                return View();
            }
        }

        #region 详细页

        [PageAuth]
        public ActionResult Detail(int id)
        {
            base.BaseDetail(id);
            return View();
        }
        #endregion

        [HttpPost]
        public JsonResult _Save(Sys_User entity, int UserID)
        {
            if (entity.ID == 0)
            {
                List<Sys_User> rtlist = userService.GetAll().Where(c => c.LoginAccount == entity.LoginAccount).ToList();
                if (rtlist.Count > 0)
                {
                    ItemResult<int> item = new ItemResult<int>();
                    item.Success = false;
                    item.Message = "登陆账号不能重复，请修改名称后，重新保存。";
                    return Json(item, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    ApplicationContext.Cache.Remove(EntityName + entity.ID);
                    entity.CreateTime = DateTime.Now;
                    entity.UpdateTime = DateTime.Now;
                    entity.OwnerID = UserID;
                    return Json(userService.Insert(entity), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                List<Sys_User> Irtlist = userService.GetAll().Where(c => c.LoginAccount == entity.LoginAccount && c.ID != entity.ID).ToList();
                if (Irtlist.Count > 0)
                {
                    ItemResult<int> item = new ItemResult<int>();
                    item.Success = false;
                    item.Message = "登陆账号不能重复，请修改名称后，重新保存。";
                    return Json(item, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    ApplicationContext.Cache.Remove(EntityName + entity.ID);
                    entity.UpdateUserID = UserID;
                    entity.UpdateTime = DateTime.Now;
                    return Json(userService.Update(entity), JsonRequestBehavior.DenyGet);
                }
            }
        }

        [HttpPost]
        public JsonResult _Delete(string ids)
        {
            return Json(userService.Delete(JsonConvert.DeserializeObject<List<int>>(ids)));
        }

        #region 数据停用

        [HttpPost]
        public JsonResult _Disable(string ids, int statecode = 1)
        {
            return Json(userService.Disable(EntityName, JsonConvert.DeserializeObject<List<int>>(ids), statecode), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult _IsUnDisable(int id)
        {
            return Json(userService.IsUnDisable(EntityName, id), JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 分配角色
        public ActionResult RoleList()
        {
            try
            {
                if (Request.QueryString["UID"] != null)
                {
                    List<Sys_User_Role> list = userAndRoleService.SelectAllRole(Convert.ToInt32(Request.QueryString["UID"]));

                    ViewBag.SelectValue = list;
                    ViewBag.UID = Request.QueryString["UID"].ToString();
                }

                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult RelationAjaxList(Pagination request, Pagination rq)
        {
            KendoFilterHelper.Single.SetPagination(Request.Form, rq);
            Pagination page = new Pagination();
            page.KeyWord = request.KeyWord;
            return new ListJsonResult
            {
                Data = JsonConvert.DeserializeObject(roleService.GetPagingList(page)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //return Json(fieldService.GetFKEntityfiledList(request, rq));
        }


        [HttpPost]
        /// <summary>
        /// 保存用户的角色
        /// </summary>
        /// <param name="roles">角色ID用，号隔开</param>
        /// <param name="UID">用户ID</param>
        /// <returns></returns>
        public JsonResult _SetRole(string roles, int UID, string rolesname)
        {
            ItemResult<int> result = userAndRoleService.SetRoles(UID, roles, rolesname);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 分配岗位
        public ActionResult JobList()
        {
            try
            {
                if (Request.QueryString["UID"] != null)
                {
                    List<Sys_Job_User> list = jobUsersService.SelectAllJob(Convert.ToInt32(Request.QueryString["UID"]));
                    ViewBag.SelectValue = list;
                    ViewBag.UID = Request.QueryString["UID"].ToString();
                }

                return View();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult _JobList(Pagination request, Pagination rq)
        {
            KendoFilterHelper.Single.SetPagination(Request.Form, rq);
            Pagination page = new Pagination();
            page.KeyWord = request.KeyWord;
            return new ListJsonResult
            {
                Data = JsonConvert.DeserializeObject(jobService.GetPagingList(page)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //return Json(fieldService.GetFKEntityfiledList(request, rq));
        }


        [HttpPost]
        /// <summary>
        /// 保存用户的岗位
        /// </summary>
        /// <param name="string">岗位ID用"，"号隔开</param>
        /// <param name="UID">岗位ID</param>
        /// <returns></returns>
        public JsonResult _SetJobs(string jobs, int UID, string jobsname)
        {
            ItemResult<int> result = jobUsersService.SetJobs(UID, jobs, jobsname);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 修改密码


        public ActionResult UpdatePassword()
        {
            return View();
        }

        public JsonResult _AjaxSubmitUpdatePassword(string oldpassword, string password, int UserID)
        {
            return Json(userService.UpdatePassword(oldpassword, password, UserID));
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult ResetPassword(int UserID)
        {
            return Json(userService.ResetPassword(UserID));
        }

        #endregion


    }
}
