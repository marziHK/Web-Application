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


namespace FSRM.Controllers
{
    public class ChartsController : Controller
    {
        #region Feilds

        db_FSRMEntities DB = new db_FSRMEntities();

        public static float AllReads;
        public static float AllWrites;
        public static float AllModifies;
        public static float CntAllAccess;
        public static float CntOpenAccess;
        public static float CntTodayAccess;

        public static float CntAllFolderReq;
        public static float CntOpenFolderReq;
        public static float CntTodayFolderReq;

        #endregion



        #region Folders Chart

        [HttpPost]
        public ActionResult AllFoldersByOU()
        {
            var q = (from f in DB.Tbl_Folders
                     join o in DB.Tbl_FoldersOwner
                     on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                     join d in DB.Tbl_Department
                     on o.fld_FK_FolderOwnerOU equals d.fld_DepartmentID
                     where f.fld_FolderShow == true && f.fld_AdminCheck == true
                     group f by new { d.fld_DepartmentName } into g
                     select new
                     {
                         OuName = g.Key.fld_DepartmentName,
                         AllFld = g.Count(x => x != null)
                     });

            List<FoldersOUViewModel> model = new List<FoldersOUViewModel>();

            foreach (var item in q)
            {
                model.Add(new FoldersOUViewModel
                {
                    OuAllFolders = (float)Math.Round((item.AllFld / (CntAllFolderReq- CntOpenFolderReq)) * 100, 2),
                    OuName = item.OuName.ToString(),
                    Explode = true
                });
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult FolderRequestByOU()
        {

            var q = (from f in DB.Tbl_Folders
                     join o in DB.Tbl_FoldersOwner
                     on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                     join d in DB.Tbl_Department
                     on o.fld_FK_FolderOwnerOU equals d.fld_DepartmentID
                     where f.fld_FolderShow == true //&& f.fld_AdminCheck == true
                     group f by new { d.fld_DepartmentName } into g
                     select new
                     {
                         OuName = g.Key.fld_DepartmentName,
                         //rd = g.Where(x=>x.a.fld_AccessRead)
                         Open = g.Where(x => x.fld_AdminCheck == false).Count(),
                         Closed = g.Where(x => x.fld_AdminCheck == true).Count(),
                     }).ToList();

            List<FoldersOUViewModel> model = new List<FoldersOUViewModel>();

            foreach (var item in q)
            {
                model.Add(new FoldersOUViewModel
                {
                    //OuAllAccess = (float)Math.Round((item.AllPerm / CntAllAccess) * 100, 2),
                    OuName = item.OuName.ToString(),
                    OpenRequests = item.Open,
                    ClosedRequests = item.Closed
                });
            }

            return Json(model);
        }

        #endregion


        #region Permits Chart

        [HttpPost]
        public ActionResult AllPermitsByType()
        {

            List<PermitsTypeChartViewModel> model = new List<PermitsTypeChartViewModel>()
            {
                new PermitsTypeChartViewModel() {Name = "Read", Value= AllReads, Color="#E9D5DA" },
                new PermitsTypeChartViewModel() {Name = "Write", Value= AllWrites, Color="#827397" },
                new PermitsTypeChartViewModel() {Name = "Modify", Value= AllModifies, Color="#4D4C7D" },
        };

            return Json(model);
        }


        [HttpPost]
        public ActionResult AllPermitsByOU()
        {
            var q = (from a in DB.Tbl_Access
                     join p in DB.Tbl_Personnel
                     on a.fld_FK_PersonID equals p.fld_PersonID
                     join d in DB.Tbl_Department
                     on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                     where a.fld_AdminChecked == true
                     group a by new { d.fld_DepartmentName } into g
                     select new
                     {
                         OuName = g.Key.fld_DepartmentName,
                         AllPerm = g.Count(x => x != null)
                     }).ToList();

            List<PermitsChartViewModel> model = new List<PermitsChartViewModel>();
            foreach (var item in q)
            {
                model.Add(new PermitsChartViewModel
                {
                    OuAllAccess = (float)Math.Round((item.AllPerm / CntAllAccess) * 100, 2),
                    OuName = item.OuName.ToString(),
                    Explode = true
                });
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult OUPermitsDetails()
        {

            var q = (from l in DB.Tbl_AccessLog
                     join a in DB.Tbl_Access
                     on l.fld_FK_AccessID equals a.fld_AccessID
                     join s in DB.Tbl_AccessStatus
                     on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                     join p in DB.Tbl_Personnel
                     on a.fld_FK_PersonID equals p.fld_PersonID
                     join d in DB.Tbl_Department
                     on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                     where a.fld_AdminChecked == true &&  a.fld_AccessShow ==true && (s.fld_AccessStatusCode == 4 || s.fld_AccessStatusCode == 6 || s.fld_AccessStatusCode == 7)
                     group new { a, p, d } by new { d.fld_DepartmentName } into g
                     select new
                     {
                         OuName = g.Key.fld_DepartmentName,
                         ReadAccess = g.Where(x => x.a.fld_AccessRead == true && x.a.fld_AdminChecked == true).Count(),
                         WriteAccess = g.Where(x => x.a.fld_AccessWrite == true && x.a.fld_AdminChecked == true).Count(),
                         ModifyAccess = g.Where(x => x.a.fld_AccessModify == true && x.a.fld_AdminChecked == true).Count()
                     }).ToList();

            //float CntAllAccess = DB.Tbl_Access.Where(x => x.fld_state == 0).Count();

            List<PermitsChartViewModel> model = new List<PermitsChartViewModel>();

            foreach (var item in q)
            {
                model.Add(new PermitsChartViewModel
                {
                    OuName = item.OuName.ToString(),
                    ReadAccess = item.ReadAccess,
                    WriteAccess = item.WriteAccess,
                    ModifyAccess = item.ModifyAccess
                });
            }

            return Json(model);
        }

        #endregion
    }

}
