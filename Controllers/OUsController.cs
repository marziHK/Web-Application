//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSRM.Models;
using FSRM.Models.ViewModels;
using FSRM.Models.DataModel;


namespace FSRM.Controllers
{
    public class OUsController : Controller
    {

        #region Feilds

        readonly db_FSRMEntities DB = new db_FSRMEntities();

        #endregion


        // GET: OUs
        public JsonResult Index()
        {
            var UserOUs = DB.Tbl_Department.Select(x => new OUsViewModel { OUID = x.fld_DepartmentID, OUFaName = x.fld_DepartmentName });

            return this.Json(UserOUs, JsonRequestBehavior.AllowGet);
        }
    }
}