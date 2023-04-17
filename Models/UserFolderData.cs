//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class UserFolderData
    {
        #region Feild

        db_FSRMEntities db = new db_FSRMEntities();

        #endregion


        public IList<UserFolderViewModel> ReadUserFolders(string OwnerName)
        {
            try
            {
                var q = (from l in db.Tbl_FoldersRequestLog
                         join f in db.Tbl_Folders
                         on l.fld_FK_FoldersID equals f.fld_FolderID
                         join o in db.Tbl_FoldersOwner
                         on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                         join s in db.Tbl_FoldersRequstStatus
                         on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                         orderby f.fld_AdminCheck ascending, l.fld_FolderRequestLogMDate descending
                         where f.fld_FolderShow == true && o.fld_FolderOwnerName == OwnerName && l.fld_ShowLastLog == true
                         select new
                         {
                             s.fld_FoldersRequestStatusCode,
                             s.fld_FoldersRequestStatusDescription,
                             f.fld_FolderID,
                             f.fld_FolderAddress,
                             f.fld_SuggestedAddress,
                             f.fld_SuggestedName,
                             f.fld_SuggestedSpace,
                             f.fld_ApprovedSpace,
                             f.fld_AdminCheck,
                             l.fld_FoldersRequstLogID,
                             l.fld_FolderRequestLog_PersonModified,
                             l.fld_FolderRequestLogPreviousFolderID,
                             l.fld_FK_FoldersRequestStatusID,
                             l.fld_FolderRequestLogHDate,
                             l.fld_FolderRequestLogMDate
                         }).ToList();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                // int StatusDelID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 3).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var UserFolders = new List<UserFolderViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
                    int FolderID = (int)x.fld_FolderRequestLogPreviousFolderID;

                    string fHd = db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == FolderID && z.fld_FK_FoldersRequestStatusID == StatusID).Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    fHd = fHd.Substring(0, 4) + "/" + fHd.Substring(4, 2) + "/" + fHd.Substring(6, 2);

                    string Adm_Hd = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == FolderID && (z.fld_FK_FoldersRequestStatusID == 4 || z.fld_FK_FoldersRequestStatusID == 5 || z.fld_FK_FoldersRequestStatusID == 6 || z.fld_FK_FoldersRequestStatusID == 7)).Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        Adm_Hd = Adm_Hd.Substring(0, 4) + "/" + Adm_Hd.Substring(4, 2) + "/" + Adm_Hd.Substring(6, 2);

                    }
                    bool Ed = true;
                    if (x.fld_AdminCheck == true || x.fld_FoldersRequestStatusCode == 7)
                    {
                        Ed = false;
                    }

                    UserFolders.Add(new UserFolderViewModel
                    {
                        FolderLogID = (int)x.fld_FoldersRequstLogID,
                        FolderID = (int)x.fld_FolderID,
                        FolderAddress = x.fld_FolderAddress,
                        SugFolderAddress = x.fld_SuggestedAddress,
                        SugFolderName = x.fld_SuggestedName,
                        SugFolderValue = (int)x.fld_SuggestedSpace,
                        ApprovedFolderValue = (int)x.fld_ApprovedSpace,
                        ReqHDate = fHd,
                        ReqMDate = (DateTime)x.fld_FolderRequestLogMDate,
                        RequestStatus = x.fld_FoldersRequestStatusDescription,
                        StatusCode = (int)x.fld_FoldersRequestStatusCode,
                        AdminCheckHDate = Adm_Hd,
                        EnableEdit = Ed,
                        EnableDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 ? false : true,

                    });
                }

                return UserFolders;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public int CheckOwner(string OwnerName, string OwnerEmail, string User_OU)
        {
            int OwnID = 0;
            try
            {
                int OUID = db.Tbl_Department.Where(x => x.fld_DepartmentADName == User_OU).Select(x => x.fld_DepartmentID).FirstOrDefault();

                if (db.Tbl_FoldersOwner.Any(x => x.fld_FolderOwnerName == OwnerName && x.fld_FolderOwnerEmail == OwnerEmail && x.fld_FK_FolderOwnerOU == OUID))
                {
                    // Return PersonnalID
                    OwnID = db.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerName == OwnerName && x.fld_FolderOwnerEmail == OwnerEmail && x.fld_FK_FolderOwnerOU == OUID).Select(x => x.fld_FolderOwnerID).FirstOrDefault();
                }
                else
                {
                    var own = new Tbl_FoldersOwner
                    {
                        fld_FK_FolderOwnerOU = OUID,
                        fld_FolderOwnerName = OwnerName,
                        fld_FolderOwnerEmail = OwnerEmail
                    };
                    db.Tbl_FoldersOwner.Add(own);
                    db.SaveChanges();
                    OwnID = own.fld_FolderOwnerID;
                }
                return OwnID;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int CreateNewUserFolder(UserFolderViewModel model)
        {
            try
            {
                int OwnerID = CheckOwner(model.OwnerName, model.OwnerEmail, model.OwnerOUName);

                int FolderID = 0;
                int FolderLogID = 0;
                if (OwnerID == 0)
                {
                    // ???
                }
                else
                {
                    var Fld = new Tbl_Folders
                    {
                        fld_SuggestedName = model.SugFolderName,
                        fld_SuggestedAddress = model.SugFolderAddress,
                        fld_SuggestedSpace = model.SugFolderValue,
                        fld_FK_FolderOwner = OwnerID,
                        fld_ApprovedSpace = 0,
                        fld_FolderAddress = null,
                        fld_AdminCheck = false,
                        fld_FolderShow = true
                    };
                    db.Tbl_Folders.Add(Fld);
                    db.SaveChanges();
                    FolderID = Fld.fld_FolderID;


                    int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                    var Log = new Tbl_FoldersRequestLog
                    {
                        fld_FolderRequestLog_PersonModified = model.OwnerName,
                        fld_FK_FoldersID = FolderID,
                        fld_FK_FoldersRequestStatusID = StatusID,
                        fld_FolderRequestLogPreviousFolderID = FolderID,
                        fld_FolderRequestLogHDate = Int32.Parse(model.ReqHDate),
                        fld_FolderRequestLogMDate = model.ReqMDate,
                        fld_ShowLastLog = true
                    };
                    db.Tbl_FoldersRequestLog.Add(Log);
                    db.SaveChanges();
                    FolderLogID = Log.fld_FoldersRequstLogID;
                }

                return FolderLogID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public string RemoveUserFolder(UserFolderViewModel model)
        {
            string Res = "";
            try
            {

                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 2).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var al = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == model.FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = model.OwnerName,
                    fld_FK_FoldersID = model.FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = model.FolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(model.ReqHDate),
                    fld_FolderRequestLogMDate = model.ReqMDate,
                    fld_ShowLastLog = false
                };
                db.Tbl_FoldersRequestLog.Add(Log);

                var q = db.Tbl_Folders.Where(x => x.fld_FolderID == model.FolderID).SingleOrDefault();
                q.fld_FolderShow = false;

                Res = "Done";

                db.SaveChanges();


                return Res;
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }

        public string RemoveUserFolder_AfterAdminChecked(UserFolderViewModel model)
        {
            string Res = "";
            try
            {

                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 3).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var f = db.Tbl_Folders.Where(x => x.fld_FolderID == model.FolderID).FirstOrDefault();
                f.fld_AdminCheck = false;

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == model.FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;


                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = model.OwnerName,
                    fld_FK_FoldersID = model.FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = db.Tbl_FoldersRequestLog.Where(z => z.fld_FK_FoldersID == model.FolderID).Select(z => z.fld_FolderRequestLogPreviousFolderID).FirstOrDefault(),
                    fld_FolderRequestLogHDate = Int32.Parse(model.ReqHDate),
                    fld_FolderRequestLogMDate = model.ReqMDate,
                    fld_ShowLastLog = true
                };
                db.Tbl_FoldersRequestLog.Add(Log);
                Res = "Email";

                db.SaveChanges();

                return Res;
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }

        public int UpdateUserFolder(UserFolderViewModel model)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 8).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var f = db.Tbl_Folders.Where(x => x.fld_FolderID == model.FolderID).SingleOrDefault();
                f.fld_FolderShow = false;

                var al = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == model.FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                al.fld_ShowLastLog = false;

                var Fld = new Tbl_Folders
                {
                    fld_FolderAddress = null,
                    fld_ApprovedSpace = 0,
                    fld_AdminCheck = false,
                    fld_FolderShow = true,
                    fld_SuggestedAddress = model.SugFolderAddress,
                    fld_SuggestedName = model.SugFolderName,
                    fld_SuggestedSpace = model.SugFolderValue,
                    fld_FK_FolderOwner = f.fld_FK_FolderOwner
                };
                db.Tbl_Folders.Add(Fld);
                db.SaveChanges();
                int FolderID_New = Fld.fld_FolderID;



                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = model.OwnerName,
                    fld_FK_FoldersID = FolderID_New,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = db.Tbl_FoldersRequestLog.Where(z => z.fld_FK_FoldersID == model.FolderID).Select(z => z.fld_FolderRequestLogPreviousFolderID).FirstOrDefault(),
                    fld_FolderRequestLogHDate = Int32.Parse(model.ReqHDate),
                    fld_FolderRequestLogMDate = model.ReqMDate,
                    fld_ShowLastLog = true
                };

                db.Tbl_FoldersRequestLog.Add(Log);
                db.SaveChanges();


                return FolderID_New;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}