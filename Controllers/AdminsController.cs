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
    public class AdminsController : Controller
    {
        #region Feilds

        db_FSRMEntities db = new db_FSRMEntities();


        #endregion


        public ActionResult ReadAllAdmins([DataSourceRequest] DataSourceRequest request)
        {
            #region Load Data

            db = new db_FSRMEntities();
            var q = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);

            var AllAdmin = new List<AdminsNameViewModel>();

            foreach (var x in q)
            {
                AllAdmin.Add(new AdminsNameViewModel
                {
                    AdminID = x.fld_AdminID,
                    AdminADName = x.fld_AdminADName,
                    AdminName = x.fld_AdminName
                });
            }

            #endregion

            return Json(AllAdmin.ToDataSourceResult(request));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateNewAdmin([DataSourceRequest] DataSourceRequest request, AdminsNameViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                db = new db_FSRMEntities();

                var ad = new Tbl_Admins
                {
                    fld_AdminADName = model.AdminADName.ToLower(),
                    fld_AdminName = model.AdminName.ToLower(),
                    fld_AdminShow = true,
                    fld_AdminStatus = 0,
                    fld_AdminMDateA = DateTime.Now
                };
                db.Tbl_Admins.Add(ad);
                db.SaveChanges();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveAdmin([DataSourceRequest] DataSourceRequest request, AdminsNameViewModel model)
        {
            if (model != null)
            {
                db = new db_FSRMEntities();
                var q = db.Tbl_Admins.Where(x=>x.fld_AdminID == model.AdminID).SingleOrDefault();
                q.fld_AdminShow = false;
                q.fld_AdminStatus = 2;
                q.fld_AdminMDateR = DateTime.Now;
                db.SaveChanges();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateAdmin([DataSourceRequest] DataSourceRequest request, AdminsNameViewModel model)
        {
            if (model != null)
            {
                db = new db_FSRMEntities();
                var q = db.Tbl_Admins.Where(x => x.fld_AdminID == model.AdminID).SingleOrDefault();
                q.fld_AdminShow = false;
                q.fld_AdminStatus = 1;
                q.fld_AdminMDateR = DateTime.Now;


                var ad = new Tbl_Admins
                {
                    fld_AdminADName = model.AdminADName,
                    fld_AdminName = model.AdminName,
                    fld_AdminShow = true,
                    fld_AdminStatus = 1,
                    fld_AdminMDateA = DateTime.Now
                };
                db.Tbl_Admins.Add(ad);

                db.SaveChanges();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

    }
}