//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class UserAccessData
    {

        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion




        public IList<UserPermitViewModel> ReadUserPermits(string OwnerName)
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
                         where a.fld_AccessShow == true && fO.fld_FolderOwnerName == OwnerName && aL.fld_ShowLastLog == true
                         orderby aL.fld_AccessLogMDate descending
                         select new
                         {
                             aL.fld_AccessLogID,
                             a.fld_AccessID,
                             aL.fld_AccessLogPreviousAccessID,
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
                             aS.fld_AccessStatusDesc,
                             aL.fld_FK_AccessStatus,
                             aS.fld_AccessStatusCode
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<UserPermitViewModel>();
                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                //int i = 1;
                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;
                    
                    string Hd = DB.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Hd = Hd.Substring(0, 4) + "/" + Hd.Substring(4, 2) + "/" + Hd.Substring(6, 2);

                    AllAccess.Add(new UserPermitViewModel
                    {
                        AccessID = x.fld_AccessID,
                        //AccessType = x.fld_AccessType,
                        //AdminChecked = (int)x.fld_AdminChecked,
                        FolderID = (int)x.fld_FK_FolderID,
                        FolderAddress = x.fld_FolderAddress,
                        PersonID = x.fld_PersonID,
                        PersonNO = x.fld_PersonNO,
                        PersonFName = x.fld_PersonFName,
                        PersonLName = x.fld_PersonLName,
                        OwnerName = x.fld_FolderOwnerName,
                        AccessRead = (bool)x.fld_AccessRead,
                        AccessWrite = (bool)x.fld_AccessWrite,
                        AccessModify = (bool)x.fld_AccessModify,
                        AccessStatus = x.fld_AccessStatusDesc,
                        EnableEdit = x.fld_AdminChecked == true ? false : true,
                        EnableDel = x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5? false : true,
                        HDate = Hd,
                        UserOU = new OUsViewModel { OUID = (int)x.fld_FK_DepartmentID, OUFaName = x.fld_DepartmentName }

                    });
                }

                AllAccess.OrderByDescending(x => x.EnableDel);
                AllAccess.OrderBy(x => x.EnableDel);
                

                return AllAccess;

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public int InsertPersonnal(UserPermitViewModel model)
        {
            int PID = 0;
            try
            {
                if (DB.Tbl_Personnel.Any(x => x.fld_PersonNO == model.PersonNO && x.fld_PersonFName == model.PersonFName && x.fld_PersonLName == model.PersonLName && x.fld_FK_DepartmentID == model.UserOU.OUID))
                {
                    // Return PersonnalID
                    PID = DB.Tbl_Personnel.Where(x => x.fld_PersonNO == model.PersonNO && x.fld_PersonFName == model.PersonFName && x.fld_PersonLName == model.PersonLName && x.fld_FK_DepartmentID == model.UserOU.OUID).Select(x => x.fld_PersonID).FirstOrDefault();
                }
                else
                {
                    var prs = new Tbl_Personnel
                    {
                        fld_PersonFName = model.PersonFName,
                        fld_PersonLName = model.PersonLName,
                        fld_PersonNO = model.PersonNO,
                        fld_FK_DepartmentID = model.UserOU.OUID

                    };
                    DB.Tbl_Personnel.Add(prs);
                    DB.SaveChanges();
                    PID = prs.fld_PersonID;
                }
                return PID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int InsertFolder(UserPermitViewModel model, string OwnerMail, string OwnerOU)
        {
            int FID = 0;
            try
            {
                var q = (from f in DB.Tbl_Folders
                         join o in DB.Tbl_FoldersOwner
                         on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                         where f.fld_FolderAddress == model.FolderAddress && o.fld_FolderOwnerName == model.OwnerName
                         select f.fld_FolderID).FirstOrDefault();

                if (q == 0)
                {
                    int OwnID = DB.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerEmail == OwnerMail && x.fld_FolderOwnerName == model.OwnerName).Select(x => x.fld_FolderOwnerID).FirstOrDefault();

                    if (OwnID == 0)
                    {
                        // Insert in DB
                        int OuID = DB.Tbl_Department.Where(x => x.fld_DepartmentADName == OwnerOU).Select(x => x.fld_DepartmentID).FirstOrDefault();

                        var Own = new Tbl_FoldersOwner
                        {
                            fld_FK_FolderOwnerOU = OuID,
                            fld_FolderOwnerName = model.OwnerName,
                            fld_FolderOwnerEmail = OwnerMail
                        };
                        DB.Tbl_FoldersOwner.Add(Own);
                        DB.SaveChanges();
                        OwnID = Own.fld_FolderOwnerID;
                    }

                    // Insert in DB
                    var Fld = new Tbl_Folders
                    {
                        fld_FK_FolderOwner = OwnID,
                        fld_FolderAddress = model.FolderAddress,
                        fld_FolderShow = false
                    };
                    DB.Tbl_Folders.Add(Fld);
                    DB.SaveChanges();
                    FID = Fld.fld_FolderID;
                }
                else
                {
                    FID = q;
                }
                return FID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int CreateNewUserPermit(UserPermitViewModel model, string OwnerMail, string OwnerOU, string eTime)
        {
            try
            {
                int PrsID = InsertPersonnal(model);
                int FldID = InsertFolder(model, OwnerMail, OwnerOU);
                int AccID = 0;
                int AccLogID = 0;
                if (FldID == 0 || PrsID == 0)
                {
                    // ???
                }
                else
                {
                    var Perm = new Tbl_Access
                    {
                        fld_AccessRead = model.AccessRead,
                        fld_AccessWrite = model.AccessWrite,
                        fld_AccessModify = model.AccessModify,
                        fld_LastPersonModified = model.OwnerName,
                        fld_FK_FolderID = FldID,
                        fld_FK_PersonID = PrsID,
                        fld_AdminChecked = false,
                        fld_AccessShow = true
                    };
                    DB.Tbl_Access.Add(Perm);
                    DB.SaveChanges();
                    AccID = Perm.fld_AccessID;


                    int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                    var Log = new Tbl_AccessLog
                    {
                        fld_FK_AccessID = AccID,
                        fld_FK_AccessStatus = StatusID,
                        fld_AccessLogPersonName = model.OwnerName,
                        fld_AccessLogPreviousAccessID = AccID,
                        fld_AccessLogHDate = Int32.Parse(model.HDate),
                        fld_AccessLogTime = eTime,
                        fld_AccessLogMDate = model.MDate,
                        fld_ShowLastLog = true
                    };
                    DB.Tbl_AccessLog.Add(Log);
                    DB.SaveChanges();
                    AccLogID = Log.fld_AccessLogID;
                }

                return AccLogID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public string RemoveUserPermit(UserPermitViewModel model, int HDate, string HTime, DateTime dt)
        {
            string Res = "";
            try
            {

                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 3).Select(x => x.fld_AccessStatusID).FirstOrDefault();
                //int OwnID = DB.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerName == model.OwnerName).Select(x => x.fld_FolderOwnerID).FirstOrDefault();
                //var q = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).SingleOrDefault();
                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = model.AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = model.OwnerName,
                    fld_AccessLogPreviousAccessID = DB.Tbl_AccessLog.Where(z => z.fld_FK_AccessID == model.AccessID).Select(z => z.fld_AccessLogPreviousAccessID).FirstOrDefault(),
                    fld_AccessLogHDate = HDate,
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = dt,
                    fld_ShowLastLog = false
                };
                DB.Tbl_AccessLog.Add(Log);

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var q = DB.Tbl_Access.Where(x => x.fld_AccessID == model.AccessID).SingleOrDefault();
                q.fld_AccessShow = false;
                Res = "Done";


                DB.SaveChanges();

                return Res;
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }


        public string RemoveUserPermit_AfterAdminChecked(UserPermitViewModel model, int HDate, string HTime, DateTime dt)
        {
            string Res = "";
            try
            {

                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var a = DB.Tbl_Access.Where(x => x.fld_AccessID == model.AccessID).SingleOrDefault();
                a.fld_AdminChecked = false;

                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = model.AccessID,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = model.OwnerName,
                    fld_AccessLogPreviousAccessID = DB.Tbl_AccessLog.Where(z => z.fld_FK_AccessID == model.AccessID).Select(z => z.fld_AccessLogPreviousAccessID).FirstOrDefault(),
                    fld_AccessLogHDate = HDate,
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = dt,
                    fld_ShowLastLog = true
                };
                DB.Tbl_AccessLog.Add(Log);

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;


                Res = "Email";


                DB.SaveChanges();

                return Res;
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }


        public string UpdateUserPermit(UserPermitViewModel model, string OwnerMail, string OwnerOU, int HDate, string HTime, DateTime dt)
        {
            string Res = "";

            try
            {
                DB = new db_FSRMEntities();

                int StatusID = DB.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                //int OwnID = DB.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerName == model.OwnerName).Select(x => x.fld_FolderOwnerID).FirstOrDefault();
                //var q = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).SingleOrDefault();

                var a = DB.Tbl_Access.Where(x => x.fld_AccessID == model.AccessID).SingleOrDefault();
                a.fld_AccessShow = false;

                var al = DB.Tbl_AccessLog.Where(x => x.fld_FK_AccessID == model.AccessID).OrderByDescending(x => x.fld_AccessLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;


                var Perm = new Tbl_Access
                {
                    fld_AccessRead = model.AccessRead,
                    fld_AccessWrite = model.AccessWrite,
                    fld_AccessModify = model.AccessModify,
                    fld_LastPersonModified = model.OwnerName,
                    fld_FK_FolderID = model.FolderID,
                    fld_FK_PersonID = model.PersonID,
                    fld_AdminChecked = false,
                    fld_AccessShow = true
                };
                DB.Tbl_Access.Add(Perm);
                DB.SaveChanges();
                int AccID_New = Perm.fld_AccessID;


                var Log = new Tbl_AccessLog
                {
                    fld_FK_AccessID = AccID_New,
                    fld_FK_AccessStatus = StatusID,
                    fld_AccessLogPersonName = model.OwnerName,
                    fld_AccessLogPreviousAccessID = DB.Tbl_AccessLog.Where(z => z.fld_FK_AccessID == model.AccessID).Select(z => z.fld_AccessLogPreviousAccessID).FirstOrDefault(),
                    fld_AccessLogHDate = HDate,
                    fld_AccessLogTime = HTime,
                    fld_AccessLogMDate = dt,
                    fld_ShowLastLog = true
                };
                DB.Tbl_AccessLog.Add(Log);
                DB.SaveChanges();


                Res = a.fld_AdminChecked == true ? "Email" : "Done";


                DB.SaveChanges();

                return Res;
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }


    }
}