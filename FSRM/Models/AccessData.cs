//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class AccessData
    {
        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion


        #region For Admin

        public List<AllAccessViewModel> ReadAllAccess()
        {
            try
            {
                DB = new db_FSRMEntities();

                var q = (from a in DB.Tbl_Access
                         join aL in DB.Tbl_AccessLog
                         on a.fld_AccessID equals aL.fld_FK_AccessID
                         join aS in DB.Tbl_AccessStatus
                         on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join fO in DB.Tbl_FoldersOwner
                         on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                         where a.fld_AccessShow == true
                         orderby a.fld_AdminChecked ascending, aL.fld_AccessLogMDate ascending
                         select new
                         {
                             //a.fld_state,
                             //a.fld_PersonAdded,
                             a.fld_AccessID,
                             a.fld_AccessRead,
                             a.fld_AccessWrite,
                             a.fld_AccessModify,
                             a.fld_AdminChecked,
                             a.fld_FK_FolderID,
                             f.fld_FolderAddress,
                             fO.fld_FolderOwnerName,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName,
                             aS.fld_AccessStatusDesc
                             //,
                             //p.fld_PersonAdded
                             //}).ToList();
                         }).AsEnumerable();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
                    //if (x.fld_AdminChecked == true && x.fld_state == 2)
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                        string AdminCh = "";
                        //if (x.fld_AdminChecked == 1)
                        //{
                        //    AdminCh = "اعمال تغییرات توسط مدیر سیستم";
                        //}
                        //else
                        //{
                        //    if (x.fld_state == 0)
                        //    {
                        //        AdminCh = "ثبت توسط کاربر";
                        //    }
                        //    else if (x.fld_state == 2)
                        //    {
                        //        AdminCh = "درخواست حذف دسترسی ها";
                        //    }
                        //}
                        AllAccess.Add(new AllAccessViewModel
                        {
                            AccessID = x.fld_AccessID,
                            AccessRead = (bool)x.fld_AccessRead,
                            AccessWrite = (bool)x.fld_AccessWrite,
                            AccessModify = (bool)x.fld_AccessModify,
                            AdminChecked = AdminCh,
                            FolderID = (int)x.fld_FK_FolderID,
                            FolderAddress = x.fld_FolderAddress,
                            PersonID = x.fld_PersonID,
                            PersonNO = x.fld_PersonNO,
                            PersonFName = x.fld_PersonFName,
                            PersonLName = x.fld_PersonLName,
                            //PersonAdded = x.fld_PersonAdded,
                            UserOU = x.fld_DepartmentName
                            //UserOU = new OUsViewModel { OUID = (int)x.fld_FK_DepartmentID, OUFaName = x.fld_DepartmentName }

                        });
                    }
                //}

                return AllAccess;

            }
            catch (Exception)
            {

                throw;
            }

        }


        public bool AdminChecked(int AccessID, string HDate, string HTime)
        {
            try
            {
                DB = new db_FSRMEntities();
                var q = DB.Tbl_Access.Where(x => x.fld_AccessID == AccessID).SingleOrDefault();
                q.fld_AdminChecked = true;
                //q.fld_AdminChDate = int.Parse(HDate);
                //q.fld_AdminChTime = HTime;

                DB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion




        #region For Users


       

        

        

        #endregion

    }
}