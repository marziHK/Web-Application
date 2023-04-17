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
    public class AdminController : Controller
    {

        #region Feilds

        db_FSRMEntities db = new db_FSRMEntities();
        AllServices Obj_Access = new AllServices();

        //public static string User_OU;
        //public static string User_Name;
        //public static string User_Email;
        public int User_Role;

        string[] Admins;

        #endregion


        public void CheckLogin()
        {
            if (Session["User_Name"] == null || User_Role == 0)
            {
                Admins = db.Tbl_Admins.Where(x => x.fld_AdminStatus == 0).Select(x => x.fld_AdminADName).ToArray();
                User_Role = 0;

                // All User AD Groups
                string[] rolesArray = Roles.GetRolesForUser();
                if (User.IsInRole("Domain\\FSRM_Permit"))
                {
                    // string str = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    string Uname = User.Identity.Name;
                    //User_Name = Uname;
                    string Det = AD.GetOU(Uname);
                    Session["User_Name"] = Det.Split('-')[0];
                    Session["User_OU"] = Det.Split('-')[1];
                    Session["User_Email"] = Det.Split('-')[2];

                    //string tst = Uname.Remove(0, 15).ToLower();
                    string tst = Uname.Split('\\')[1].ToLower();
                    if (Admins.Contains(tst))
                    {
                        User_Role = 1; // admin
                    }
                    else
                    {
                        User_Role = 2; // user
                    }
                }
            }

        }

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
            ChartsController.CntAllAccess = db.Tbl_Access.Where(x => x.fld_AccessShow == true && x.fld_AdminChecked == true).Count();
            ChartsController.CntOpenAccess = db.Tbl_Access.Where(x => x.fld_AccessShow == true && x.fld_AdminChecked == false).Count();
            ChartsController.CntTodayAccess = db.Tbl_AccessLog.Where(x => x.fld_ShowLastLog == true && x.fld_AccessLogHDate == HDate).Count();
            ChartsController.CntAllFolderReq = db.Tbl_Folders.Where(x => x.fld_FolderShow == true && x.fld_AdminCheck != null).Count();
            ChartsController.CntOpenFolderReq = db.Tbl_Folders.Where(x => x.fld_FolderShow == true && x.fld_AdminCheck == false).Count();
            ChartsController.CntTodayFolderReq = db.Tbl_FoldersRequestLog.Where(x => x.fld_ShowLastLog == true && x.fld_FolderRequestLogHDate == HDate).Count();


            ViewBag.AllReads = ChartsController.AllReads;
            ViewBag.AllWrites = ChartsController.AllWrites;
            ViewBag.AllModifies = ChartsController.AllModifies;
            ViewBag.CntAllAccess = ChartsController.CntAllAccess;
            ViewBag.CntOpenAccess = ChartsController.CntOpenAccess;
            ViewBag.CntTodayAccess = ChartsController.CntTodayAccess;


            ViewBag.CntAllFolderReq = ChartsController.CntAllFolderReq;
            ViewBag.CntOpenFolderReq = ChartsController.CntOpenFolderReq;
            ViewBag.CntTodayFolderReq = ChartsController.CntTodayFolderReq;

        }

        public ActionResult Panel()
        {
            CheckLogin();

            if (User_Role == 1)
            {
                ReportCount();
                return View();
            }
            else
            {
                return RedirectToAction("Prevent", "User");
            }

        }

        public ActionResult Permits()
        {
            CheckLogin();

            if (User_Role == 1)
            {
                ReportCount();
                return View();
            }
            else
            {
                return RedirectToAction("Prevent", "User");
            }
        }

        public ActionResult Folders()
        {

            CheckLogin();

            if (User_Role == 1)
            {
                ReportCount();
                return View();
            }
            else
            {
                return RedirectToAction("Prevent", "User");
            }

        }

        public ActionResult Admin()
        {
            CheckLogin();
            string em = Session["User_Email"].ToString();
            if (User_Role == 1 && (em.Split('@')[0].ToLower() == "zahraei" || em.Split('@')[0].ToLower() == "hosseinkhani"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Prevent", "Admin");
            }
        }


        public ActionResult Prevent()
        {
            //ViewBag.User = User.Identity.Name;

            //User_Name = null;
            //User_OU = null;
            //User_Email = null;
            //User_Role = 0;
            return View();
        }



        #region Search

        public JsonResult Items_GetOUs()
        {
            var DB = new db_FSRMEntities();

            var UserOUs = DB.Tbl_Department.Select(x => new OUsViewModel { OUID = x.fld_DepartmentID, OUFaName = x.fld_DepartmentName });

            return Json(UserOUs, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Items_GetAdmins()
        {
            var DB = new db_FSRMEntities();

            var UserOUs = DB.Tbl_Admins.Where(x => x.fld_AdminShow == true).Select(x => new AdminsNameViewModel { AdminID = x.fld_AdminID, AdminName = x.fld_AdminName });
            return Json(UserOUs, JsonRequestBehavior.AllowGet);

        }


        #endregion

    }
}