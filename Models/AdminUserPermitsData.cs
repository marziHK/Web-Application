//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class AdminUserPermitsData
    {
        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion


        public string AdminChecked(int AccessID, string HDate, string HTime, DateTime MDate, string AdminName)
        {
            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 4).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                string Admin = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = Admin,
                    fld_AccessLogPreviousAccessID = al.fld_AccessLogPreviousAccessID,
                    fld_AccessLogHDate = Int32.Parse(HDate),
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = MDate,
                    fld_ShowLastLog = true
                };
                DB.Tbl_AccessLog.Add(Log);

                var q = DB.Tbl_Access.Where(x => x.fld_AccessID == AccessID).SingleOrDefault();
                q.fld_AdminChecked = true;
                q.fld_LastPersonModified = Admin;

                DB.SaveChanges();
                string OwnerEmail = DB.Tbl_FoldersOwner.Where(x => x.fld_FK_FolderOwnerOU == (DB.Tbl_Folders.Where(z => z.fld_FolderID == q.fld_FK_FolderID).Select(z => z.fld_FK_FolderOwner).FirstOrDefault())).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string AdminRefusePermit(int AccessID, string HDate, string HTime, DateTime MDate, string AdminName)
        {
            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 5).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                string Admin = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = Admin,
                    fld_AccessLogPreviousAccessID = al.fld_AccessLogPreviousAccessID,
                    fld_AccessLogHDate = Int32.Parse(HDate),
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = MDate,
                    fld_ShowLastLog = true
                };
                DB.Tbl_AccessLog.Add(Log);

                var q = DB.Tbl_Access.Where(x => x.fld_AccessID == AccessID).SingleOrDefault();
                q.fld_AdminChecked = true;
                q.fld_LastPersonModified = Admin;

                DB.SaveChanges();
                string OwnerEmail = DB.Tbl_FoldersOwner.Where(x => x.fld_FK_FolderOwnerOU == (DB.Tbl_Folders.Where(z => z.fld_FolderID == q.fld_FK_FolderID).Select(z => z.fld_FK_FolderOwner).FirstOrDefault())).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }


        public IEnumerable<AdminUserPermitsViewModel> ReadAllUserPermits()
        {
            try
            {
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
                         where
                            a.fld_AccessShow == true &&
                            aL.fld_ShowLastLog == true

                         //&& a.fld_AccessID == ((int)(from t in DB.Tbl_AccessLog
                         //                           orderby t.fld_AccessLogMDate descending
                         //                           select t.fld_FK_AccessID).FirstOrDefault())
                         orderby a.fld_AdminChecked ascending, aL.fld_AccessLogMDate descending
                         select new
                         {
                             a.fld_AccessID,
                             aL.fld_AccessLogID,
                             aL.fld_AccessLogPreviousAccessID,
                             aL.fld_FK_AccessStatus,
                             a.fld_AccessRead,
                             a.fld_AccessWrite,
                             a.fld_AccessModify,
                             a.fld_AdminChecked,
                             aS.fld_AccessStatusDesc,
                             aS.fld_AccessStatusCode,
                             a.fld_LastPersonModified,
                             a.fld_FK_FolderID,
                             f.fld_FolderAddress,
                             fO.fld_FolderOwnerName,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                         }
                         ).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();
                int StatusIDUp = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();
                // int StatusDelID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();
                int StatusDelID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in DB.Tbl_AccessLog
                                  join s in DB.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        Adm_Hd = Adm_Hd.Substring(0, 4) + "/" + Adm_Hd.Substring(4, 2) + "/" + Adm_Hd.Substring(6, 2);

                    }


                    AllAccess.Add(new AdminUserPermitsViewModel
                    {
                        AccessID = x.fld_AccessID,
                        AccessRead = (bool)x.fld_AccessRead,
                        AccessWrite = (bool)x.fld_AccessWrite,
                        AccessModify = (bool)x.fld_AccessModify,
                        AdminChecked = (bool)x.fld_AdminChecked,
                        FolderID = (int)x.fld_FK_FolderID,
                        FolderAddress = x.fld_FolderAddress,
                        PersonID = x.fld_PersonID,
                        PersonNO = x.fld_PersonNO,
                        PersonFName = x.fld_PersonFName,
                        PersonLName = x.fld_PersonLName,
                        PersonOUName = x.fld_DepartmentName,
                        PersonFullName = x.fld_PersonFName + " " + x.fld_PersonLName,
                        OwnerName = x.fld_FolderOwnerName,
                        LastPersonModified = x.fld_LastPersonModified,
                        AccessInserHDate = Acc_Hd,
                        AccessInserMDate = (DateTime)DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        AdminCheckMDate = (DateTime)DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,
                        //PersonAdded = x.fld_PersonAdded,
                        //UserOU = x.fld_DepartmentName
                        //UserOU = new OUsViewModel { OUID = (int)x.fld_FK_DepartmentID, OUFaName = x.fld_DepartmentName }

                    });
                }
                //}

                AllAccess.OrderByDescending(x => x.AccDel);
                AllAccess.OrderBy(x => x.AccDel);

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public string UpdateUserPermits_ByAdmin(AdminUserPermitsViewModel model, string AdminName, int HDate, string HTime, DateTime dt)
        {
            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 6).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var a = DB.Tbl_Access.Where(x => x.fld_AccessID == model.AccessID).SingleOrDefault();
                a.fld_AccessShow = false;

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var Perm = new Tbl_Access
                {
                    fld_AccessRead = model.AccessRead,
                    fld_AccessWrite = model.AccessWrite,
                    fld_AccessModify = model.AccessModify,
                    fld_LastPersonModified = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault(),
                    fld_FK_FolderID = model.FolderID,
                    fld_FK_PersonID = model.PersonID,
                    fld_AdminChecked = a.fld_AdminChecked,
                    fld_AccessShow = true
                };
                DB.Tbl_Access.Add(Perm);
                DB.SaveChanges();

                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = Perm.fld_AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault(),
                    fld_AccessLogPreviousAccessID = al.fld_AccessLogPreviousAccessID,
                    fld_AccessLogHDate = HDate,
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = dt,
                    fld_ShowLastLog = true
                };
                DB.Tbl_AccessLog.Add(Log);
                DB.SaveChanges();

                string OwnerEmail = DB.Tbl_FoldersOwner.Where(x => x.fld_FK_FolderOwnerOU == (DB.Tbl_Folders.Where(z => z.fld_FolderID == a.fld_FK_FolderID).Select(z => z.fld_FK_FolderOwner).FirstOrDefault())).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return Perm.fld_AccessID + "|" + OwnerEmail;
            }
            catch (Exception ex)
            {
                return "Faild";
            }
        }


        public bool DestroyPermit(AdminUserPermitsViewModel model)
        {
            DB = new db_FSRMEntities();
            var q = DB.Tbl_Access.Where(x => x.fld_AccessID == model.AccessID).SingleOrDefault();
            return (bool)q.fld_AdminChecked;

        }


       // سلب دسترسی بعد از اختصاص
        public string RemoveUserPermit_ByAdmin(int AccessID, string HDate, string HTime, DateTime MDate, string AdminName)
        {
            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 9).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                string Admin = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;
                

                var ac = DB.Tbl_Access.Where(x => x.fld_AccessID == AccessID).SingleOrDefault();
                ac.fld_AccessShow = false;
                ac.fld_AdminChecked = true;
                ac.fld_LastPersonModified = Admin;


                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = Admin,
                    fld_AccessLogPreviousAccessID = al.fld_AccessLogPreviousAccessID,
                    fld_AccessLogHDate = Int32.Parse(HDate),
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = MDate,
                    fld_ShowLastLog = false
                };
                DB.Tbl_AccessLog.Add(Log);


                DB.SaveChanges();
                string OwnerEmail = DB.Tbl_FoldersOwner.Where(x => x.fld_FK_FolderOwnerOU == (DB.Tbl_Folders.Where(z => z.fld_FolderID == ac.fld_FK_FolderID).Select(z => z.fld_FK_FolderOwner).FirstOrDefault())).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }

        // در ابتدا درخواست حذف میشه
        public string RemoveUserPermitsRequest_ByAdmin(AdminUserPermitsViewModel model, string AdminName, int HDate, string HTime, DateTime dt)
        {
            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 8).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var a = DB.Tbl_Access.Where(x => x.fld_AccessID == model.AccessID).SingleOrDefault();
                a.fld_AccessShow = false;
                a.fld_AdminChecked = true;
                a.fld_LastPersonModified = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = model.AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault(),
                    fld_AccessLogPreviousAccessID = al.fld_AccessLogPreviousAccessID,
                    fld_AccessLogHDate = HDate,
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = dt,
                    fld_ShowLastLog = false
                };
                DB.Tbl_AccessLog.Add(Log);
                DB.SaveChanges();

                string OwnerEmail = DB.Tbl_FoldersOwner.Where(x => x.fld_FK_FolderOwnerOU == (DB.Tbl_Folders.Where(z => z.fld_FolderID == model.FolderID).Select(z => z.fld_FK_FolderOwner).FirstOrDefault())).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception ex)
            {
                return "Faild";
            }
        }


        public string RemovePermits_UserRequst(int AccID, string AdminName, int HDate, string HTime, DateTime dt)
        {
            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 10).Select(x => x.fld_AccessStatusID).FirstOrDefault();
                string Admin = DB.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var a = DB.Tbl_Access.Where(x => x.fld_AccessID == AccID).SingleOrDefault();
                a.fld_AccessShow = false;
                a.fld_LastPersonModified = Admin;
                a.fld_AdminChecked = true;

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == AccID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;


                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = AccID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = Admin,
                    fld_AccessLogPreviousAccessID = al.fld_AccessLogPreviousAccessID,
                    fld_AccessLogHDate = HDate,
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = dt,
                    fld_ShowLastLog = false
                };
                DB.Tbl_AccessLog.Add(Log);
                DB.SaveChanges();

                string OwnerEmail = DB.Tbl_FoldersOwner.Where(x => x.fld_FK_FolderOwnerOU == (DB.Tbl_Folders.Where(z => z.fld_FolderID == (DB.Tbl_Access.Where(o=>o.fld_AccessID == AccID).Select(o=>o.fld_FK_FolderID).FirstOrDefault())
                                                            ).Select(z => z.fld_FK_FolderOwner).FirstOrDefault())).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception ex)
            {
                return "Faild";
            }
        }

    }
}