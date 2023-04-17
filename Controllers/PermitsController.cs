//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FSRM.Services;
using FSRM.Models;
using FSRM.Models.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using FSRM.Models.DataModel;
using System.Globalization;

namespace FSRM.Controllers
{
    public class PermitsController : Controller
    {


        #region Feilds

        db_FSRMEntities DB = new db_FSRMEntities();
        AdminPermitsServices Obj_Permit = new AdminPermitsServices();
        PermitSearchServices Obj_Rep = new PermitSearchServices();
        UserServices Obj_UserAccess = new UserServices();
        db_FSRMEntities db = new db_FSRMEntities();


        string[] Admins;


        #endregion

        //test
        public void ReportCount()
        {
            #region date
            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();
            int HDate = int.Parse(yr + mn + dy);
            #endregion
            ChartsController.AllReads = db.Tbl_Access.Where(x => x.fld_AccessShow == true && x.fld_AdminChecked == true && x.fld_AccessRead == true).Count();
            ChartsController.AllWrites = db.Tbl_Access.Where(x => x.fld_AccessShow == true && x.fld_AdminChecked == true && x.fld_AccessWrite == true).Count();
            ChartsController.AllModifies = db.Tbl_Access.Where(x => x.fld_AccessShow == true && x.fld_AdminChecked == true && x.fld_AccessModify == true).Count();
            ChartsController.CntAllFolderReq = db.Tbl_Folders.Where(x => x.fld_FolderShow == true && x.fld_AdminCheck != null).Count();
            ChartsController.CntOpenAccess = db.Tbl_Access.Where(x => x.fld_AccessShow == true && x.fld_AdminChecked == false).Count();
            ChartsController.CntTodayAccess = db.Tbl_AccessLog.Where(x => x.fld_ShowLastLog == true && x.fld_AccessLogHDate == HDate).Count();


            ViewBag.AllReads = ChartsController.AllReads;
            ViewBag.AllWrites = ChartsController.AllWrites;
            ViewBag.AllModifies = ChartsController.AllModifies;
            ViewBag.CntAllAccess = ChartsController.CntAllAccess;
            ViewBag.CntOpenAccess = ChartsController.CntOpenAccess;
            ViewBag.CntTodayAccess = ChartsController.CntTodayAccess;

        }

        public void CheckLogin()
        {
            if (Session["User_Name"] == null)
            {
                Admins = DB.Tbl_Admins.Where(x => x.fld_AdminStatus == 0).Select(x => x.fld_AdminADName).ToArray();

                // All User AD Groups
                string[] rolesArray = Roles.GetRolesForUser();
                //string[] usersInRole = Roles.GetUsersInRole("MAPNAGENERATOR\\ICT Unit"); ;

                if (User.IsInRole("MAPNAGENERATOR\\FSRM_Permit"))
                //if (User.IsInRole("MAPNAGENERATOR\\Internet Free")) 
                {
                    // string str = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    string Uname = User.Identity.Name;
                    //Session["User_Name"] = Uname;
                    string Det = AD.GetOU(Uname);
                    Session["User_Name"] = Det.Split('-')[0];
                    Session["User_OU"] = Det.Split('-')[1];
                    Session["User_Email"] = Det.Split('-')[2];

                }
            }
        }
       
        
        #region Grid User Permits

        public ActionResult ReadUserPermits_ByUser([DataSourceRequest] DataSourceRequest request)
        {
            //DataSourceResult q = Obj_Access.ReadAllAccess();

            return Json(Obj_UserAccess.ReadUserPermits(Session["User_Name"].ToString()).ToDataSourceResult(request));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateUserPermits_ByUser([DataSourceRequest] DataSourceRequest request, UserPermitViewModel Permit)
        {
            if (Permit != null && ModelState.IsValid)
            {
                CheckLogin();


                char c92 = new char();
                c92 = Convert.ToChar(92);

                string x = Permit.FolderAddress;
                string cc = x.Replace('>', c92);
                Permit.FolderAddress = cc;
                
                Permit.OwnerName = Session["User_Name"].ToString();
                Obj_UserAccess.CreateNewUserPermit(Permit, Session["User_Email"].ToString(), Session["User_OU"].ToString());
            }

            return Json(new[] { Obj_UserAccess.ReadUserPermits(Session["User_Name"].ToString()) }.ToDataSourceResult(request, ModelState));

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveUserPermits_ByUser([DataSourceRequest] DataSourceRequest request, UserPermitViewModel Permit)
        {
            if (Permit != null)
            {
                CheckLogin();
                Obj_UserAccess.RemoveUserPermit(Permit);
            }

            return Json(new[] { Obj_UserAccess.ReadUserPermits(Session["User_Name"].ToString()) }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateUserPermits_ByUser([DataSourceRequest] DataSourceRequest request, UserPermitViewModel Permit)
        {
            if (Permit != null)
            {
                CheckLogin();
                char c92 = new char();
                c92 = Convert.ToChar(92);

                string x = Permit.FolderAddress;
                string cc = x.Replace('>', c92);
                Permit.FolderAddress = cc;

                Obj_UserAccess.UpdateUserPermit(Permit, Session["User_Email"].ToString(), Session["User_OU"].ToString());
            }

            return Json(new[] { Obj_UserAccess.ReadUserPermits(Session["User_Name"].ToString()) }.ToDataSourceResult(request, ModelState));
        }


        #endregion



        #region Grid Admin -- Permits

        [HttpPost]
        public ActionResult AdminCheckedPermit([DataSourceRequest] DataSourceRequest request, int AccId, int StatusCode)
        {
            string AdminName = User.Identity.Name.Split('\\')[1].ToLower();

            if (StatusCode == 7)
            {// سلب دسترسی
                Obj_Permit.RemovePermits_UserRequst(AccId, AdminName);
            }
            else
            {
                Obj_Permit.AdminChecked(AccId, AdminName);

            }
            ReportCount();
            //return RedirectToAction("Panel", "Admin");
            return Json(Obj_Permit.ReadAllUserPermits().ToDataSourceResult(request));

        }

        [HttpPost]
        public ActionResult AdminRefusePermit([DataSourceRequest] DataSourceRequest request, int AccId, int StatusCode)
        {
            string AdminName = User.Identity.Name.Split('\\')[1].ToLower();

            Obj_Permit.AdminRefusePermit(AccId, AdminName);

            ReportCount();

            return Json(Obj_Permit.ReadAllUserPermits().ToDataSourceResult(request));

        }
        
        public JsonResult Load_AccessLog([DataSourceRequest] DataSourceRequest request, int AccId)
        {
            //int AccId = 1;
            return Json(Obj_Permit.AccessLogByID_ForAdmin(AccId).ToDataSourceResult(request));
        }

        public ActionResult ReadAllUserPermits_ByAdmin([DataSourceRequest] DataSourceRequest request, string Typ,
            string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string[] ch_Request, string Owner, string PersonNo, string OU_ID, string PersonName)
        {
            //if (ReqSData == "" && ReqEData == "" && AppSData == "" && AppEData == "" && Owner == "" && OU_ID == null && ch_Access == null)
            //{
            // Load all Permits

            //return Json(.ToDataSourceResult(request));
            //}
            //else
            //{
            //Report

            //}

            //var q = Obj_Access.Report(ReqSData, ReqEData, AppSData, AppEData, ch_Access, Owner, OU_ID);
            //return Json(q.ToDataSourceResult(request));
            if (ReqSData == "" && ReqEData == "" && AppSData == "" && AppEData == "" && Owner == "" && OU_ID == "" && ch_Access == null && ch_Request == null && PersonNo == "" && PersonName == "")
            {
                //بدون فیلتر -تمام داده ها نشان د اده شود
                
                var q = Obj_Permit.ReadAllUserPermits();
                return Json(q.ToDataSourceResult(request));
            }
            else
            {
                //var q = Obj_Rep.Report(ReqSData, ReqEData, AppSData, AppEData, ch_Request, Owner, OU_ID, FolderVal);
                var q = Obj_Rep.Report(ReqSData, ReqEData, AppSData, AppEData, ch_Access, ch_Request, Owner, OU_ID, PersonName, PersonNo);
                return Json(q.ToDataSourceResult(request));
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveUserPermits_ByAdmin([DataSourceRequest] DataSourceRequest request, AdminUserPermitsViewModel Permit)
        {
            if (Permit != null)
            {
                string AdminName = User.Identity.Name.Split('\\')[1].ToLower();
                Obj_Permit.DestroyPermit(Permit, AdminName);
            }
            ReportCount();
            return Json(Obj_Permit.ReadAllUserPermits().ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdateUserPermits_ByAdmin([DataSourceRequest] DataSourceRequest request, AdminUserPermitsViewModel Permit)
        {
            if (Permit != null)
            {
                string AdminName = User.Identity.Name.Split('\\')[1].ToLower();

                Obj_Permit.UpdateUserPermits_ByAdmin(Permit, AdminName);
            }
            ReportCount();
            return Json(Obj_Permit.ReadAllUserPermits().ToDataSourceResult(request));
        }

        #endregion


    }
}