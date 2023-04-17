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


namespace FSRM.Controllers
{
    public class UserController : Controller
    {

        #region Feilds

        db_FSRMEntities DB = new db_FSRMEntities();
        UserServices Obj_User = new UserServices();

        public string User_OU;
        public string User_Name;
        public string User_Email;
        public int User_Role;

        string[] Admins;



        #endregion

        public void CheckLogin()
        {
            if (Session["User_Name"] == null || User_Role == 0)
            {
                Admins = DB.Tbl_Admins.Where(x => x.fld_AdminStatus == 0).Select(x => x.fld_AdminADName).ToArray();

                //Session["User_Name"] = null;
                //Session["User_Email"] = null;
                //Session["User_OU"] = null;
                User_Role = 0;

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
                    User_Name = Session["User_Name"].ToString();
                    User_OU = Session["User_OU"].ToString();
                    User_Email = Session["User_Email"].ToString();

                    //string tst = Uname.Remove(0, 15).ToLower();
                    string tst = Uname.Split('\\')[1].ToLower();
                    if (Admins.Contains(tst))
                    //if (Admins.Contains(Session["User_Name"].Split(',')[0]))
                    {
                        User_Role = 1;
                    }
                    else
                    {
                        User_Role = 2;
                    }
                }
            }
        }


        public ActionResult Prevent()
        {
            //Session["User_Name"] = User.Identity.Name;

            //Session["User_Name"] = null;
            //Session["User_OU"] = null;
            //Session["User_Email"] = null;
            //User_Role = 0;
            return View();
        }



        #region Users Permits 

        #region Get Folders Name From share

        public List<string> GetRootDir()
        {
            List<string> list = new List<string>();

            string root = @"\\fsrm\" + "root";
            list.Add(root.Replace("\\", ">"));

            root = @"\\fsrm\" + "WIND TURBINE";
            list.Add(root.Replace("\\", ">"));
            return list;
        }
        public string GetDir(string dir)
        {
            List<string> Dirlist = new List<string>();
            if (string.IsNullOrWhiteSpace(dir))
            {
                Dirlist = GetRootDir();
            }
            else
            {
                try
                {
                    dir = @"" + dir.Replace(">", "\\").Replace("&gt;", "\\");
                    string[] subdirectoryEntries = Directory.GetDirectories(dir);
                    foreach (var item in subdirectoryEntries)
                    {
                        Dirlist.Add(item.Replace("\\", ">"));
                    }
                }
                catch (Exception ex)
                {
                    Dirlist.Add(ex.Message);
                    Dirlist.Add("Access Denied.");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(Dirlist);
                    //throw;
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(Dirlist);


        }

        #endregion

        public ActionResult Panel()
        {
            //AD.GetICTUsers();
            CheckLogin();
            ViewData["defaultOU"] = DB.Tbl_Department.Select(o => new OUsViewModel { OUID = o.fld_DepartmentID, OUFaName = o.fld_DepartmentName }).First();
            //if (ViewBag.User == null)
            //{
            //    if (Session["User_Name"] == null )
            //    {
            //        CheckLogin();
            //    }
            //    else
            //    {
            //        ViewBag.User = Session["User_Name"];
            //    }
            //}
            if (User_Role == 2 || User_Role == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Prevent", "User");
            }
        }


        public ActionResult UserPermits()
        {
            ViewData["defaultOU"] = DB.Tbl_Department.Select(o => new OUsViewModel { OUID = o.fld_DepartmentID, OUFaName = o.fld_DepartmentName }).First();

            return PartialView("UserPermits");
        }

        public ActionResult UserFolders()
        {
            return PartialView("UserFolders");
        }


        public JsonResult Read_AccessLog([DataSourceRequest] DataSourceRequest request, int AccId)
        {
            //int AccId = 1;
            var dd = Json(Obj_User.AccessLogByID(AccId).ToDataSourceResult(request));
            return dd;
        }


        public JsonResult Read_FolderLog([DataSourceRequest] DataSourceRequest request, int FolderID)
        {
            //int AccId = 1;
            return Json(Obj_User.FolderLogByID(FolderID).ToDataSourceResult(request));
        }


        #endregion



    }
}