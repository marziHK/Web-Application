//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class FolderSearchData
    {
        #region Feild

        db_FSRMEntities db = new db_FSRMEntities();

        #endregion


        public List<AdminUserFolderViewModel> ReportByRequestDates(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = (from l in db.Tbl_FoldersRequestLog
                         join f in db.Tbl_Folders
                         on l.fld_FK_FoldersID equals f.fld_FolderID
                         join o in db.Tbl_FoldersOwner
                         on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                         join s in db.Tbl_FoldersRequstStatus
                         on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                         orderby f.fld_AdminCheck ascending, l.fld_FolderRequestLogMDate ascending
                         where
                                l.fld_ShowLastLog == true
                                && f.fld_FolderShow == true
                                && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                    && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                            .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                    && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                            .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                             l.fld_FolderRequestLogMDate,
                             o.fld_FK_FolderOwnerOU
                         })
                         //.GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         //)
                         //.Select(i => i.FirstOrDefault())
                         .ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
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



        public List<AdminUserFolderViewModel> ReportByRequestDates_1r_Open(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 l.fld_ShowLastLog == true
                                 && f.fld_FolderShow == true
                                 && f.fld_AdminCheck == false
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)

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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          })
                         .GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault())
                         .ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserFolderViewModel> ReportByRequestDates_1r_Closed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 7)
                                 && l.fld_ShowLastLog == true
                                 && f.fld_FolderShow == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)

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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU

                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserFolderViewModel> ReportByRequestDates_1r_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 s.fld_FoldersRequestStatusCode == 5
                                 && f.fld_AdminCheck == true
                                 && l.fld_ShowLastLog == true
                                 && f.fld_FolderShow == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)

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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserFolderViewModel> ReportByRequestDates_1r_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (s.fld_FoldersRequestStatusCode == 10
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 6)
                                 //l.fld_ShowLastLog == true
                                 //&& f.fld_FolderShow == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)

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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserFolderViewModel> ReportByRequestDates_2r_Open_Closed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (f.fld_AdminCheck == false
                                 || s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 7)
                                 && l.fld_ShowLastLog == true
                                 //&& f.fld_FolderShow == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserFolderViewModel> ReportByRequestDates_2r_Open_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (f.fld_AdminCheck == false
                                 || s.fld_FoldersRequestStatusCode == 5)
                                 && l.fld_ShowLastLog == true
                                 //&& f.fld_FolderShow == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserFolderViewModel> ReportByRequestDates_2r_Open_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (f.fld_AdminCheck == false
                                 || s.fld_FoldersRequestStatusCode == 6
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 10)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserFolderViewModel> ReportByRequestDates_2r_Closed_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 5
                                 || s.fld_FoldersRequestStatusCode == 7)
                                 && l.fld_ShowLastLog == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserFolderViewModel> ReportByRequestDates_2r_Closed_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 6
                                 || s.fld_FoldersRequestStatusCode == 7
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 10)
                                 && f.fld_AdminCheck == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)

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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          })
                         .GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         //.Select
                         )
                         .Select(i => i.FirstOrDefault())
                         .ToList();


                //var q = (from l in db.Tbl_FoldersRequestLog
                //         join f in db.Tbl_Folders
                //         on l.fld_FK_FoldersID equals f.fld_FolderID
                //         join o in db.Tbl_FoldersOwner
                //         on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                //         join s in db.Tbl_FoldersRequstStatus
                //         on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                //         orderby l.fld_FolderRequestLogMDate ascending
                //         where
                //                (s.fld_FoldersRequestStatusCode == 4
                //                || s.fld_FoldersRequestStatusCode == 6
                //                || s.fld_FoldersRequestStatusCode == 7
                //                || s.fld_FoldersRequestStatusCode == 9
                //                || s.fld_FoldersRequestStatusCode == 10)
                //                && f.fld_AdminCheck == true
                //                && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                //                                                                    && z.fld_FK_FoldersRequestStatusID == StatusID)
                //                                            .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                //                && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                //                                                                    && z.fld_FK_FoldersRequestStatusID == StatusID)
                //                                            .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
                //         select new
                //         {
                //             o.fld_FolderOwnerName,
                //             s.fld_FoldersRequestStatusCode,
                //             s.fld_FoldersRequestStatusDescription,
                //             f.fld_FolderID,
                //             f.fld_FolderAddress,
                //             f.fld_SuggestedAddress,
                //             f.fld_SuggestedName,
                //             f.fld_SuggestedSpace,
                //             f.fld_ApprovedSpace,
                //             f.fld_AdminCheck,
                //             l.fld_FoldersRequstLogID,
                //             l.fld_FolderRequestLog_PersonModified,
                //             l.fld_FolderRequestLogPreviousFolderID,
                //             l.fld_FK_FoldersRequestStatusID,
                //             l.fld_FolderRequestLogHDate,
                //             l.fld_FolderRequestLogMDate
                //         }).Distinct().ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserFolderViewModel> ReportByRequestDates_2r_Removed_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (s.fld_FoldersRequestStatusCode == 5
                                 || s.fld_FoldersRequestStatusCode == 6
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 10)
                                 && f.fld_AdminCheck == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public List<AdminUserFolderViewModel> ReportByRequestDates_3r_Open_Closed_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (f.fld_AdminCheck == false
                                 || s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 5
                                 || s.fld_FoldersRequestStatusCode == 7)
                                 && l.fld_ShowLastLog == true
                                 && f.fld_FolderShow == true
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserFolderViewModel> ReportByRequestDates_3r_Open_Closed_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (f.fld_AdminCheck == false
                                 || s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 6
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 10)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserFolderViewModel> ReportByRequestDates_3r_Open_Refused_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (f.fld_AdminCheck == false
                                 || s.fld_FoldersRequestStatusCode == 5
                                 || s.fld_FoldersRequestStatusCode == 6
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 10)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserFolderViewModel> ReportByRequestDates_3r_Closed_Refused_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where
                                 (s.fld_FoldersRequestStatusCode == 4
                                 || s.fld_FoldersRequestStatusCode == 5
                                 || s.fld_FoldersRequestStatusCode == 6
                                 || s.fld_FoldersRequestStatusCode == 7
                                 || s.fld_FoldersRequestStatusCode == 9
                                 || s.fld_FoldersRequestStatusCode == 10)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }





        public List<AdminUserFolderViewModel> ReportByRequestDates_4r_AllRequestType(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_FoldersRequstStatus.Where(x => x.fld_FoldersRequestStatusCode == 1).Select(x => x.fld_FoldersRequestStatusID).FirstOrDefault();

                var q = ((from l in db.Tbl_FoldersRequestLog
                          join f in db.Tbl_Folders
                          on l.fld_FK_FoldersID equals f.fld_FolderID
                          join o in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals o.fld_FolderOwnerID
                          join s in db.Tbl_FoldersRequstStatus
                          on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                          orderby l.fld_FolderRequestLogMDate ascending
                          where

                                 ((l.fld_ShowLastLog == true
                                     && f.fld_FolderShow == true)
                                     ||
                                     ((s.fld_FoldersRequestStatusCode == 6
                                         || s.fld_FoldersRequestStatusCode == 9
                                         || s.fld_FoldersRequestStatusCode == 10)
                                       && (l.fld_ShowLastLog == false
                                         && f.fld_FolderShow == false
                                         && f.fld_AdminCheck == true)
                                 ))
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_FoldersRequestLog.Where(z => z.fld_FolderRequestLogPreviousFolderID == l.fld_FolderRequestLogPreviousFolderID
                                                                                     && z.fld_FK_FoldersRequestStatusID == StatusID)
                                                             .Select(z => z.fld_FolderRequestLogHDate).FirstOrDefault()) <= ReqEData)
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
                              l.fld_FolderRequestLogMDate,
                              o.fld_FK_FolderOwnerOU
                          }).GroupBy(i => i.fld_FolderRequestLogPreviousFolderID)
                         )
                         .Select(i => i.FirstOrDefault()).ToList();



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

                    int ReportAdminDate = 0;
                    string Adm_Name = "";
                    if (x.fld_AdminCheck == true)
                    {
                        Adm_Hd = (from l in db.Tbl_FoldersRequestLog
                                  join s in db.Tbl_FoldersRequstStatus
                                  on l.fld_FK_FoldersRequestStatusID equals s.fld_FoldersRequestStatusID
                                  orderby l.fld_FolderRequestLogMDate ascending
                                  where l.fld_FolderRequestLogPreviousFolderID == FolderID
                                  && (s.fld_FoldersRequestStatusCode == 4 ||
                                     s.fld_FoldersRequestStatusCode == 5 ||
                                     s.fld_FoldersRequestStatusCode == 6 ||
                                     s.fld_FoldersRequestStatusCode == 7 ||
                                     s.fld_FoldersRequestStatusCode == 9 ||
                                     s.fld_FoldersRequestStatusCode == 10)
                                  select l.fld_FolderRequestLogHDate).FirstOrDefault().ToString();
                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccDel = x.fld_FoldersRequestStatusCode == 3 || x.fld_FoldersRequestStatusCode == 5 || x.fld_FoldersRequestStatusCode == 6 || x.fld_FoldersRequestStatusCode == 9 || x.fld_FoldersRequestStatusCode == 10 ? false : true,
                        AdminChecked = (bool)x.fld_AdminCheck,
                        ReportAdminDate = ReportAdminDate,
                        OwnerOUID = (int)x.fld_FK_FolderOwnerOU
                    });
                }

                return AllFolders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}