//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class AdminUserFolderData
    {

        #region Feild

        db_FSRMEntities db = new db_FSRMEntities();

        #endregion

        public string AdminChecked(int FolderID, string HDate, DateTime MDate, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 4).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                string Admin = db.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = Admin,
                    fld_FK_FoldersID = FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = fl.fld_FolderRequestLogPreviousFolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(HDate),
                    fld_FolderRequestLogMDate = MDate,
                    fld_ShowLastLog = true
                };
                db.Tbl_FoldersRequestLog.Add(Log);


                var q = db.Tbl_Folders.Where(x => x.fld_FolderID == FolderID).SingleOrDefault();
                q.fld_FolderAddress = q.fld_SuggestedAddress + "\\" + q.fld_SuggestedName;
                q.fld_ApprovedSpace = q.fld_SuggestedSpace;
                q.fld_AdminCheck = true;
                q.fld_FolderShow = true;

                db.SaveChanges();
                string OwnerEmail = db.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerID == q.fld_FK_FolderOwner).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string AdminRefuseFolder(int FolderID, string HDate, DateTime MDate, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 5).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                string Admin = db.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = Admin,
                    fld_FK_FoldersID = FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = fl.fld_FolderRequestLogPreviousFolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(HDate),
                    fld_FolderRequestLogMDate = MDate,
                    fld_ShowLastLog = true
                };
                db.Tbl_FoldersRequestLog.Add(Log);


                var q = db.Tbl_Folders.Where(x => x.fld_FolderID == FolderID).SingleOrDefault();
                //q.fld_FolderAddress = q.fld_SuggestedAddress + "\\" + q.fld_SuggestedName;
                //q.fld_ApprovedSpace = q.fld_SuggestedSpace;
                q.fld_AdminCheck = true;
                q.fld_FolderShow = true;

                db.SaveChanges();
                string OwnerEmail = db.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerID == q.fld_FK_FolderOwner).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }



        public IList<AdminUserFolderViewModel> ReadAllUserFolders()
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
                         orderby f.fld_AdminCheck ascending, l.fld_FolderRequestLogMDate ascending
                         where l.fld_ShowLastLog == true && f.fld_FolderShow == true
                         select new
                         {
                             o.fld_FolderOwnerName,
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
                int StatusDelID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 3).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var AllFolders = new List<AdminUserFolderViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
                    int FolderID = (int)x.fld_FolderRequestLogPreviousFolderID;

                    string fHd = db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == FolderID && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                            .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    fHd = fHd.Substring(0, 4) + "/" + fHd.Substring(4, 2) + "/" + fHd.Substring(6, 2);

                    string Adm_Hd = "";
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate descending
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
                        Adm_Name = x.fld_FolderRequestLog_PersonModified;
                    }

                    AllFolders.Add(new AdminUserFolderViewModel
                    {
                        OwnerName = x.fld_FolderOwnerName,
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
                        AdminCheckedName = Adm_Name,
                        AccDel = x.fld_FoldersRequestStatusCode == 3  || x.fld_FoldersRequestStatusCode == 5 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                    });
                }


                AllFolders.OrderBy(x => x.AccDel);



                return AllFolders;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public bool UpdateFolderInfo_ByAdmin(AdminUserFolderViewModel model, string HDate, DateTime MDate, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 7).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                string Admin = db.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == model.FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;

                var f = db.Tbl_Folders.Where(x => x.fld_FolderID == model.FolderID).SingleOrDefault();
                f.fld_FolderShow = false;

                var Fo = new Tbl_Folders
                {
                    fld_FK_FolderOwner = f.fld_FK_FolderOwner,
                    fld_SuggestedAddress = f.fld_SuggestedAddress,
                    fld_SuggestedName = f.fld_SuggestedName,
                    fld_SuggestedSpace = f.fld_SuggestedSpace,
                    fld_AdminCheck = f.fld_AdminCheck,
                    fld_FolderShow = true,
                    fld_FolderAddress = model.FolderAddress,
                    fld_ApprovedSpace = model.ApprovedFolderValue
                };
                db.Tbl_Folders.Add(Fo);
                db.SaveChanges();

                int FolderID_Insert = Fo.fld_FolderID;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = Admin,
                    fld_FK_FoldersID = FolderID_Insert,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = fl.fld_FolderRequestLogPreviousFolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(HDate),
                    fld_FolderRequestLogMDate = MDate,
                    fld_ShowLastLog = true
                };
                db.Tbl_FoldersRequestLog.Add(Log);
                db.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string RemoveFolder_UserRequst(int FolderID, string HDate, DateTime MDate, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 9).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                string Admin = db.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;

                var f = db.Tbl_Folders.Where(x => x.fld_FolderID == FolderID).SingleOrDefault();
                f.fld_FolderShow = false;
                f.fld_AdminCheck = true;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = Admin,
                    fld_FK_FoldersID = FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = fl.fld_FolderRequestLogPreviousFolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(HDate),
                    fld_FolderRequestLogMDate = MDate,
                    fld_ShowLastLog = false
                };
                db.Tbl_FoldersRequestLog.Add(Log);

                db.SaveChanges();
                string OwnerEmail = db.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerID == f.fld_FK_FolderOwner).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public bool DestroyFolder(AdminUserFolderViewModel model)
        {
            db = new db_FSRMEntities();
            var q = db.Tbl_Folders.Where(x => x.fld_FolderID == model.FolderID).SingleOrDefault();
            return (bool)q.fld_AdminCheck;

        }

        // بعد ایجاد
        public string RemoveFolder_ByAdmin(int FolderID, string HDate, DateTime MDate, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 10).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                string Admin = db.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;

                var f = db.Tbl_Folders.Where(x => x.fld_FolderID == FolderID).SingleOrDefault();
                f.fld_FolderShow = false;
                f.fld_AdminCheck = true;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = Admin,
                    fld_FK_FoldersID = FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = fl.fld_FolderRequestLogPreviousFolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(HDate),
                    fld_FolderRequestLogMDate = MDate,
                    fld_ShowLastLog = false
                };
                db.Tbl_FoldersRequestLog.Add(Log);

                db.SaveChanges();
                string OwnerEmail = db.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerID == f.fld_FK_FolderOwner).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }


        public string RemoveFolderRequst_ByAdmin(int FolderID, string HDate, DateTime MDate, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();

                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 6).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                string Admin = db.Tbl_Admins.Where(x => x.fld_AdminADName == AdminName).Select(x => x.fld_AdminName).FirstOrDefault();

                var fl = db.Tbl_FoldersRequestLog.Where(x => x.fld_FK_FoldersID == FolderID).OrderByDescending(x => x.fld_FolderRequestLogMDate).ToList().FirstOrDefault();
                fl.fld_ShowLastLog = false;

                var f = db.Tbl_Folders.Where(x => x.fld_FolderID == FolderID).SingleOrDefault();
                f.fld_FolderShow = false;
                f.fld_AdminCheck = true;

                var Log = new Tbl_FoldersRequestLog
                {
                    fld_FolderRequestLog_PersonModified = Admin,
                    fld_FK_FoldersID = FolderID,
                    fld_FK_FoldersRequestStatusID = StatusID,
                    fld_FolderRequestLogPreviousFolderID = fl.fld_FolderRequestLogPreviousFolderID,
                    fld_FolderRequestLogHDate = Int32.Parse(HDate),
                    fld_FolderRequestLogMDate = MDate,
                    fld_ShowLastLog = false
                };
                db.Tbl_FoldersRequestLog.Add(Log);

                db.SaveChanges();
                string OwnerEmail = db.Tbl_FoldersOwner.Where(x => x.fld_FolderOwnerID == f.fld_FK_FolderOwner).Select(x => x.fld_FolderOwnerEmail).FirstOrDefault().ToString();

                return OwnerEmail;
            }
            catch (Exception)
            {
                return "";
            }
        }



    }
}