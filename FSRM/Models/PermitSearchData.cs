//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class PermitSearchData
    {
        #region Feild

        db_FSRMEntities db = new db_FSRMEntities();

        #endregion


        #region با اعمال نام مدیر سیستم 

        public List<AdminUserPermitsViewModel> ReportByRequestDates_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = (from a in db.Tbl_Access
                         join aL in db.Tbl_AccessLog
                         on a.fld_AccessID equals aL.fld_FK_AccessID
                         join aS in db.Tbl_AccessStatus
                         on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID
                         join p in db.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join d in db.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         join f in db.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join fO in db.Tbl_FoldersOwner
                         on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                         where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                            a.fld_AccessShow == true &&
                            aL.fld_ShowLastLog == true
                            && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                    && z.fld_FK_AccessStatus == StatusID)
                                                            .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                    && z.fld_FK_AccessStatus == StatusID)
                                                            .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
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


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Open_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join aL in db.Tbl_AccessLog
                          on a.fld_AccessID equals aL.fld_FK_AccessID
                          join aS in db.Tbl_AccessStatus
                          on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             a.fld_AccessShow == true &&
                             aL.fld_ShowLastLog == true &&
                             a.fld_AdminChecked == false &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Closed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                          (s.fld_AccessStatusCode == 4
                                  || s.fld_AccessStatusCode == 6) &&
                             a.fld_AccessShow == true &&
                             l.fld_ShowLastLog == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Refused_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             s.fld_AccessStatusCode == 5 &&
                             a.fld_AccessShow == true &&
                             l.fld_ShowLastLog == true &&
                             a.fld_AdminChecked == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Removed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Open_Closed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (f.fld_AdminCheck == false
                             || s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 6) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Open_Refused_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (f.fld_AdminCheck == false
                             || s.fld_AccessStatusCode == 5) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Open_Removed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (f.fld_AdminCheck == false
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Closed_Refused_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 6) &&
                             l.fld_ShowLastLog == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Closed_Removed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 6
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Removed_Refused_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Open_Closed_Refused_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (a.fld_AdminChecked == false
                             || s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 6) &&
                             l.fld_ShowLastLog == true &&
                             a.fld_AccessShow == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Open_Closed_Removed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (a.fld_AdminChecked == false
                             || s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 6
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Open_Refused_Removed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (a.fld_AdminChecked == false
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Closed_Refused_Removed_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                             (s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 6
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserPermitsViewModel> ReportByRequestDates_4r_AllRequestType_AdminName(int ReqSData, int ReqEData, string AdminName)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                           (

                           db.Tbl_AccessLog.Where(e => e.fld_FK_AccessID == a.fld_AccessID)
                                         .Where(e => e.fld_AccessLogPersonName.Contains(AdminName))
                                            .Count()


                           ) > 0
                           &&
                            ((a.fld_AccessShow == true && l.fld_ShowLastLog == true)
                                ||
                             ((s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10)
                             && a.fld_AccessShow == false
                             && l.fld_ShowLastLog == false)) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    int ReportAdminDate = 0;
                    string Adm_Hd = "";
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion

        #region بدون اعمال نام مدیر سیستم 



        public List<AdminUserPermitsViewModel> ReportByRequestDates(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();
                
                var q = (from a in db.Tbl_Access
                         join aL in db.Tbl_AccessLog
                         on a.fld_AccessID equals aL.fld_FK_AccessID
                         join aS in db.Tbl_AccessStatus
                         on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID
                         join p in db.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join d in db.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         join f in db.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join fO in db.Tbl_FoldersOwner
                         on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                         where
                          
                            a.fld_AccessShow == true &&
                            aL.fld_ShowLastLog == true
                            && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                    && z.fld_FK_AccessStatus == StatusID)
                                                            .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                    && z.fld_FK_AccessStatus == StatusID)
                                                            .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
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


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Open(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join aL in db.Tbl_AccessLog
                          on a.fld_AccessID equals aL.fld_FK_AccessID
                          join aS in db.Tbl_AccessStatus
                          on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             a.fld_AccessShow == true &&
                             aL.fld_ShowLastLog == true &&
                             a.fld_AdminChecked == false &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == aL.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Closed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                          (s.fld_AccessStatusCode == 4
                                  || s.fld_AccessStatusCode == 6) &&
                             a.fld_AccessShow == true &&
                             l.fld_ShowLastLog == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,
                        PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             s.fld_AccessStatusCode == 5 &&
                             a.fld_AccessShow == true &&
                             l.fld_ShowLastLog == true &&
                             a.fld_AdminChecked == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_1r_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Open_Closed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (f.fld_AdminCheck == false
                             || s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 6) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Open_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (f.fld_AdminCheck == false
                             || s.fld_AccessStatusCode == 5) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Open_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (f.fld_AdminCheck == false
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Closed_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 6) &&
                             l.fld_ShowLastLog == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Closed_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 6
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_2r_Removed_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData)
                                 && ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Open_Closed_Refused(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (a.fld_AdminChecked == false
                             || s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 6) &&
                             l.fld_ShowLastLog == true &&
                             a.fld_AccessShow == true &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Open_Closed_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (a.fld_AdminChecked == false
                             || s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 6
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Open_Refused_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (a.fld_AdminChecked == false
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AdminUserPermitsViewModel> ReportByRequestDates_3r_Closed_Refused_Removed(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                             (s.fld_AccessStatusCode == 4
                             || s.fld_AccessStatusCode == 5
                             || s.fld_AccessStatusCode == 6
                             || s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    string Adm_Hd = "";
                    int ReportAdminDate = 0;
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AdminUserPermitsViewModel> ReportByRequestDates_4r_AllRequestType(int ReqSData, int ReqEData)
        {
            try
            {
                db = new db_FSRMEntities();
                int StatusID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 1).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                var q = ((from a in db.Tbl_Access
                          join l in db.Tbl_AccessLog
                          on a.fld_AccessID equals l.fld_FK_AccessID
                          join s in db.Tbl_AccessStatus
                          on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                          join p in db.Tbl_Personnel
                          on a.fld_FK_PersonID equals p.fld_PersonID
                          join d in db.Tbl_Department
                          on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                          join f in db.Tbl_Folders
                          on a.fld_FK_FolderID equals f.fld_FolderID
                          join fO in db.Tbl_FoldersOwner
                          on f.fld_FK_FolderOwner equals fO.fld_FolderOwnerID
                          where
                            ((a.fld_AccessShow == true && l.fld_ShowLastLog == true)
                                ||
                             ((s.fld_AccessStatusCode == 8
                             || s.fld_AccessStatusCode == 9
                             || s.fld_AccessStatusCode == 10)
                             && a.fld_AccessShow == false
                             && l.fld_ShowLastLog == false)) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                                     && z.fld_FK_AccessStatus == StatusID)
                                                             .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) >= ReqSData) &&
                             ((db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == l.fld_AccessLogPreviousAccessID
                                                                            && z.fld_FK_AccessStatus == StatusID)
                                                    .Select(z => z.fld_AccessLogHDate).FirstOrDefault()) <= ReqEData)
                          orderby a.fld_AdminChecked ascending, l.fld_AccessLogMDate descending
                          select new
                          {
                              a.fld_AccessID,
                              l.fld_AccessLogID,
                              l.fld_AccessLogPreviousAccessID,
                              l.fld_FK_AccessStatus,
                              a.fld_AccessRead,
                              a.fld_AccessWrite,
                              a.fld_AccessModify,
                              a.fld_AdminChecked,
                              s.fld_AccessStatusDesc,
                              s.fld_AccessStatusCode,
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
                         ).GroupBy(i => i.fld_AccessLogPreviousAccessID))
                         .Select(i => i.FirstOrDefault()).ToList();

                var AllAccess = new List<AdminUserPermitsViewModel>();


                int StatusIDUp = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 2).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                int StatusDelID = db.Tbl_AccessStatus.Where(x => x.fld_AccessStatusCode == 7).Select(x => x.fld_AccessStatusID).FirstOrDefault();

                foreach (var x in q)
                {
                    int AccID = (int)x.fld_AccessLogPreviousAccessID;

                    ///    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == StatusID || z.fld_FK_AccessStatus == StatusIDUp)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    string Acc_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    Acc_Hd = Acc_Hd.Substring(0, 4) + "/" + Acc_Hd.Substring(4, 2) + "/" + Acc_Hd.Substring(6, 2);

                    int ReportAdminDate = 0;
                    string Adm_Hd = "";
                    if (x.fld_AccessStatusCode == 7)
                    {
                        Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusDelID).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();
                    }
                    else
                    {
                        Adm_Hd = (from l in db.Tbl_AccessLog
                                  join s in db.Tbl_AccessStatus
                                  on l.fld_FK_AccessStatus equals s.fld_AccessStatusID
                                  where l.fld_AccessLogPreviousAccessID == AccID
                                  && (s.fld_AccessStatusCode == 4 ||
                                        s.fld_AccessStatusCode == 5 ||
                                        s.fld_AccessStatusCode == 6)
                                  select l.fld_AccessLogHDate).FirstOrDefault().ToString();
                        //Adm_Hd = db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogHDate).FirstOrDefault().ToString();

                    }
                    if (Adm_Hd != "")
                    {
                        ReportAdminDate = int.Parse(Adm_Hd);
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
                        AccessInserMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && z.fld_FK_AccessStatus == StatusID).Select(z => z.fld_AccessLogMDate).FirstOrDefault(),
                        AccessStatusCode = x.fld_AccessStatusCode,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        AdminCheckHDate = Adm_Hd,
                        ReportAdminDate = ReportAdminDate,PersonOUID = (int)x.fld_FK_DepartmentID,
                        AdminCheckMDate = (DateTime)db.Tbl_AccessLog.Where(z => z.fld_AccessLogPreviousAccessID == AccID && (z.fld_FK_AccessStatus == 4 || z.fld_FK_AccessStatus == 5 || z.fld_FK_AccessStatus == 6)).Select(z => z.fld_AccessLogMDate).FirstOrDefault().GetValueOrDefault(),
                        AccDel = x.fld_AccessStatusCode == 8 || x.fld_AccessStatusCode == 9 || x.fld_AccessStatusCode == 10 || x.fld_AccessStatusCode == 7 || x.fld_AccessStatusCode == 5 ? false : true,

                    });
                }


                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        #endregion


    }

}