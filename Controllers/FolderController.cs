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
using System.Management;
using System.IO;
using System.Globalization;

namespace FSRM.Controllers
{
    public class FolderController : Controller
    {
        #region Feilds

        db_FSRMEntities DB = new db_FSRMEntities();
        UserServices Obj_User = new UserServices();
        AdminFolderServices Obj_Folder = new AdminFolderServices();
        db_FSRMEntities db = new db_FSRMEntities();
        FolderSearchServices Obj_Rep = new FolderSearchServices();
        //public static string User_OU;
        //public static string User_Name;
        //public static string User_Email;
        //public static int User_Role;

        //string[] Admins;

        #endregion
       
        
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

            ChartsController.CntAllFolderReq = db.Tbl_Folders.Where(x => x.fld_FolderShow == true && x.fld_AdminCheck != null).Count();
            ChartsController.CntOpenFolderReq = db.Tbl_Folders.Where(x => x.fld_FolderShow == true && x.fld_AdminCheck == false).Count();
            ChartsController.CntTodayFolderReq = db.Tbl_FoldersRequestLog.Where(x => x.fld_ShowLastLog == true && x.fld_FolderRequestLogHDate == HDate).Count();


            ViewBag.CntAllFolderReq = ChartsController.CntAllFolderReq;
            ViewBag.CntOpenFolderReq = ChartsController.CntOpenFolderReq;
            ViewBag.CntTodayFolderReq = ChartsController.CntTodayFolderReq;

        }


        #region Grid User Folders

        public ActionResult ReadUserFolders_ByUser([DataSourceRequest] DataSourceRequest request)
        {
            //DataSourceResult q = Obj_Access.ReadAllAccess();

            return Json(Obj_User.ReadUserFolders(Session["User_Name"].ToString()).ToDataSourceResult(request));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateUserFolders_ByUser([DataSourceRequest] DataSourceRequest request, UserFolderViewModel Folder)
        {
            if (Folder != null && ModelState.IsValid)
            {
                char c92 = new char();
                c92 = Convert.ToChar(92);

                string x = Folder.SugFolderAddress;
                string cc = x.Replace('>', c92);
                Folder.SugFolderAddress = cc;

                Folder.OwnerName = Session["User_Name"].ToString();
                Folder.OwnerEmail = Session["User_Email"].ToString();
                Folder.OwnerOUName = Session["User_OU"].ToString();

                Obj_User.CreateNewUserFolder(Folder);
            }

            return Json(Obj_User.ReadUserFolders(Session["User_Name"].ToString()).ToDataSourceResult(request, ModelState));

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveUserFolders_ByUser([DataSourceRequest] DataSourceRequest request, UserFolderViewModel Folder)
        {
            if (Folder != null)
            {
                Folder.OwnerName = Session["User_Name"].ToString();
                Folder.OwnerEmail = Session["User_Email"].ToString();
                Folder.OwnerOUName = Session["User_OU"].ToString();

                Obj_User.RemoveUserFolder(Folder);
            }

            return Json(Obj_User.ReadUserFolders(Session["User_Name"].ToString()).ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateUserFolders_ByUser([DataSourceRequest] DataSourceRequest request, UserFolderViewModel Folder)
        {
            if (Folder != null && ModelState.IsValid)
            {
                Folder.OwnerName = Session["User_Name"].ToString();
                Folder.OwnerEmail = Session["User_Email"].ToString();
                Folder.OwnerOUName = Session["User_OU"].ToString();

                Obj_User.UpdateUserFolder(Folder);
            }

            //return Json(new[] { Obj_User.ReadUserFolders(Session["User_Name"].ToString()) }.ToDataSourceResult(request, ModelState));
            return Json(Obj_User.ReadUserFolders(Session["User_Name"].ToString()).ToDataSourceResult(request, ModelState));
        }


        #endregion


        #region Grid Admin

        [HttpPost]
        public ActionResult AdminCheckedFolder([DataSourceRequest] DataSourceRequest request, int FolderID, int StatusCode)
        {
            string AdminName = User.Identity.Name.Split('\\')[1].ToLower();

            if (StatusCode == 3)
            {// حذف پوشه 
                Obj_Folder.RemoveFolder_UserRequst(FolderID, AdminName);
            }
            else
            {
                Obj_Folder.AdminChecked(FolderID, AdminName);

            }
            ReportCount();
            //return RedirectToAction("Panel", "Admin");
            return Json(Obj_Folder.ReadAllUserFolders().ToDataSourceResult(request));

        }

        [HttpPost]
        public ActionResult AdminRefuseFolder([DataSourceRequest] DataSourceRequest request, int FolderID, int StatusCode)
        {
            string AdminName = User.Identity.Name.Split('\\')[1].ToLower();

            //if (StatusCode == 3)
            //{// حذف پوشه 
            //    Obj_Folder.RemoveFolder_UserRequst(FolderID, AdminName);
            //}
            //else
            //{
            Obj_Folder.AdminRefuseFolder(FolderID, AdminName);

            //}
            ReportCount();
            //return RedirectToAction("Panel", "Admin");
            return Json(Obj_Folder.ReadAllUserFolders().ToDataSourceResult(request));

        }


        public JsonResult Load_FolderLog([DataSourceRequest] DataSourceRequest request, int FolderID)
        {
            //int AccId = 1;
            return Json(Obj_Folder.FolderLogByID_ForAdmin(FolderID).ToDataSourceResult(request));
        }

        public ActionResult ReadUserFolders_ByAdmin([DataSourceRequest] DataSourceRequest request, string Typ,
            string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Request, string Owner,string FolderVal, string OU_ID)
        {
            //DataSourceResult q = Obj_Access.ReadAllAccess();
            //var q;
            if (ReqSData == "" && ReqEData == "" && AppSData == "" && AppEData == "" && Owner == "" && OU_ID == "" && ch_Request == null && FolderVal == "")
            {
                //بدون فیلتر -تمام داده ها نشان د اده شود
                 var q = Obj_Folder.ReadAllUserFolders() ;
                return Json(q.ToDataSourceResult(request));
            }
            else
            {
                var q = Obj_Rep.Report(ReqSData, ReqEData, AppSData, AppEData, ch_Request, Owner, OU_ID, FolderVal);
                return Json(q.ToDataSourceResult(request));
            }
           
        }


        [HttpPost]
        public ActionResult RemoveUserFolders_ByAdmin([DataSourceRequest] DataSourceRequest request, AdminUserFolderViewModel Folder)
        {
            string AdminName = User.Identity.Name.Split('\\')[1].ToLower();

            Obj_Folder.DestroyFolder(Folder, AdminName);
            //return RedirectToAction("Panel", "Admin");
            ReportCount();
            return Json(Obj_Folder.ReadAllUserFolders().ToDataSourceResult(request));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateUserFolders_ByAdmin([DataSourceRequest] DataSourceRequest request, AdminUserFolderViewModel Folder)
        {
            string AdminName = User.Identity.Name.Split('\\')[1].ToLower();
            if (Folder != null && ModelState.IsValid)
            {
                Obj_Folder.UpdateFolderInfo_ByAdmin(Folder, AdminName);
                // Obj_User.UpdateUserFolder(Folder);
            }
            //ReportCount();
            return Json(Obj_Folder.ReadAllUserFolders().ToDataSourceResult(request, ModelState));
            //return Json(new[] { Folder }.ToDataSourceResult(request, ModelState));
        }

        #endregion




    }
}