//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class FolderLogData
    {
        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion


        public IList<LogFolderViewModel> FolderLogByID(int FolderID)
        {
            try
            {
                var FolderLog = new List<LogFolderViewModel>();

                int LogID = (int)DB.Tbl_FoldersRequestLog.OrderByDescending(x => x.fld_FolderRequestLogMDate).Where(x => x.fld_FK_FoldersID == FolderID).Select(x => x.fld_FolderRequestLogPreviousFolderID).FirstOrDefault();

                var q = (from l in DB.Tbl_FoldersRequestLog
                         join s in DB.Tbl_FoldersRequstStatus
                         on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                         join f in DB.Tbl_Folders
                         on l.fld_FK_FoldersID equals f.fld_FolderID

                         where l.fld_FolderRequestLogPreviousFolderID == LogID
                         orderby l.fld_FolderRequestLogMDate
                         select new
                         {
                             l.fld_FK_FoldersID,
                             l.fld_FoldersRequstLogID,
                             l.fld_FolderRequestLogHDate,
                             l.fld_FolderRequestLogMDate,
                             l.fld_FolderRequestLog_PersonModified,
                             s.fld_FoldersRequestStatusDescription,
                             s.fld_FoldersRequestStatusCode,
                             f.fld_FolderAddress,
                             f.fld_ApprovedSpace,
                             f.fld_SuggestedAddress,
                             f.fld_SuggestedName,
                             f.fld_SuggestedSpace,
                         }).ToList();

                foreach (var x in q)
                {
                    string Hd = x.fld_FolderRequestLogHDate.ToString();
                    Hd = Hd.Substring(0, 4) + "/" + Hd.Substring(4, 2) + "/" + Hd.Substring(6, 2);

                    FolderLog.Add(new LogFolderViewModel
                    {
                        FolderID = (int)x.fld_FK_FoldersID,
                        FolderLogID = x.fld_FoldersRequstLogID,
                        FolderStatusDesc = x.fld_FoldersRequestStatusDescription,
                        LogHDate = Hd,
                        LogMDate = x.fld_FolderRequestLogMDate,
                        FolderAddress = x.fld_FolderAddress,
                        FolderSpace = (int)x.fld_ApprovedSpace,
                        FolderStatusCode = (int)x.fld_FoldersRequestStatusCode,
                        SugFolderAddress = x.fld_SuggestedAddress,
                        SugFolderName = x.fld_SuggestedName,
                        SugFolderSpace = (int)x.fld_SuggestedSpace,
                        Changes = ""
                    });
                }


                return FolderLog;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public IList<LogFolderViewModel> FolderLogByID_ForAdmin(int FolderID)
        {
            try
            {
                var FolderLog = new List<LogFolderViewModel>();

                int LogID = (int)DB.Tbl_FoldersRequestLog.OrderByDescending(x => x.fld_FolderRequestLogHDate).Where(x => x.fld_FK_FoldersID == FolderID)
                                    .Select(x => x.fld_FolderRequestLogPreviousFolderID).FirstOrDefault();

                var q = (from l in DB.Tbl_FoldersRequestLog
                         join s in DB.Tbl_FoldersRequstStatus
                         on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                         join f in DB.Tbl_Folders
                         on l.fld_FK_FoldersID equals f.fld_FolderID


                         where l.fld_FolderRequestLogPreviousFolderID == LogID
                         orderby l.fld_FolderRequestLogMDate
                         select new
                         {
                             l.fld_FK_FoldersID,
                             l.fld_FoldersRequstLogID,
                             l.fld_FolderRequestLogHDate,
                             l.fld_FolderRequestLogMDate,
                             l.fld_FolderRequestLog_PersonModified,
                             s.fld_FoldersRequestStatusDescription,
                             s.fld_FoldersRequestStatusCode,
                             f.fld_FolderAddress,
                             f.fld_ApprovedSpace,
                             f.fld_SuggestedAddress,
                             f.fld_SuggestedName,
                             f.fld_SuggestedSpace,

                         }).ToList();


                foreach (var x in q)
                {
                    string Hd = x.fld_FolderRequestLogHDate.ToString();
                    Hd = Hd.Substring(0, 4) + "/" + Hd.Substring(4, 2) + "/" + Hd.Substring(6, 2);

                    FolderLog.Add(new LogFolderViewModel
                    {
                        FolderID = (int)x.fld_FK_FoldersID,
                        FolderLogID = x.fld_FoldersRequstLogID,
                        FolderStatusDesc = x.fld_FoldersRequestStatusDescription,
                        PersonName = x.fld_FolderRequestLog_PersonModified,
                        LogHDate = Hd,
                        LogMDate = x.fld_FolderRequestLogMDate,
                        FolderAddress = x.fld_FolderAddress,
                        FolderSpace = (int)x.fld_ApprovedSpace,
                        FolderStatusCode = (int)x.fld_FoldersRequestStatusCode,
                        SugFolderAddress = x.fld_SuggestedAddress,
                        SugFolderName = x.fld_SuggestedName,
                        SugFolderSpace = (int)x.fld_SuggestedSpace,
                        Changes = ""
                    });
                }


                return FolderLog;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}