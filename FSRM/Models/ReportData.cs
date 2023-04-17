//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;


namespace FSRM.Models.ViewModels
{
    public class ReportData
    {
        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion


        public List<AllAccessViewModel> ReportDates(int ReqSData, int ReqEData, int AppSData, int AppEData)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public List<AllAccessViewModel> ReportAll(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> ReportAll_1ch(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_AccessRead == Read
                         //      && a.fld_AccessModify == Modify
                         //      && a.fld_AccessWrite == Write
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && a.fld_PersonAdded.Contains(Owner)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> ReportAll_2Ch_RW(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      //&& a.fld_AccessModify == Modify
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessWrite == Write)

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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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
                       // PersonAdded = x.fld_PersonAdded,
                        UserOU = x.fld_DepartmentName
                        //UserOU = new OUsViewModel { OUID = (int)x.fld_FK_DepartmentID, OUFaName = x.fld_DepartmentName }

                    });
                }

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> ReportAll_2Ch_RM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //     // && a.fld_AccessWrite == Write
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify)

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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> ReportAll_2Ch_WM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      //&& a.fld_AccessRead == Read
                         //      && (a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public List<AllAccessViewModel> Report_Owner(int ReqSData, int ReqEData, int AppSData, int AppEData, string Owner)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_PersonAdded.Contains(Owner)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Owner_OU(int ReqSData, int ReqEData, int AppSData, int AppEData, string Owner, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && a.fld_PersonAdded.Contains(Owner)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<AllAccessViewModel> Report_Owner_Access(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Owner_Access_1ch(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      && a.fld_AccessRead == Read
                         //      && a.fld_AccessModify == Modify
                         //      && a.fld_AccessWrite == Write
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Owner_Access_2Ch_RW(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      //&& a.fld_AccessModify == Modify
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Owner_Access_2Ch_RM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      //&& a.fld_AccessWrite == Write
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify)

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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Owner_Access_2Ch_WM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, string Owner)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_PersonAdded.Contains(Owner)
                         //      //&& a.fld_AccessRead == Read
                         //      && (a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)

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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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
                       // PersonAdded = x.fld_PersonAdded,
                        UserOU = x.fld_DepartmentName
                        //UserOU = new OUsViewModel { OUID = (int)x.fld_FK_DepartmentID, OUFaName = x.fld_DepartmentName }

                    });
                }

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public List<AllAccessViewModel> Report_AccessAll(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_1ch(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_AccessRead == Read
                         //      && a.fld_AccessModify == Modify
                         //      && a.fld_AccessWrite == Write
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_2ch_RW(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      //&& a.fld_AccessModify == Modify
                         //      && (a.fld_AccessRead == Read
                         //          || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_2ch_RM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      //&& a.fld_AccessWrite == Write
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_2ch_WM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      //&& a.fld_AccessRead == Read
                         //      && (a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }







        public List<AllAccessViewModel> Report_Access_OU(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_OU_1ch(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && a.fld_AccessRead == Read
                         //      && a.fld_AccessModify == Modify
                         //      && a.fld_AccessWrite == Write
                         //      && p.fld_FK_DepartmentID == Ou_ID
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_OU_2ch_RW(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      //&& a.fld_AccessModify == Modify
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_OU_2ch_RM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      //&& a.fld_AccessWrite == Write
                         //      && (a.fld_AccessRead == Read
                         //      || a.fld_AccessModify == Modify)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<AllAccessViewModel> Report_Access_OU_2ch_WM(int ReqSData, int ReqEData, int AppSData, int AppEData, bool Read, bool Write, bool Modify, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
                         //      //&& a.fld_AccessRead == Read
                         //      && (a.fld_AccessModify == Modify
                         //      || a.fld_AccessWrite == Write)
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
                             //f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




        public List<AllAccessViewModel> Report_OU(int ReqSData, int ReqEData, int AppSData, int AppEData, int Ou_ID)
        {
            try
            {
                var q = (from a in DB.Tbl_Access
                         join p in DB.Tbl_Personnel
                         on a.fld_FK_PersonID equals p.fld_PersonID
                         join f in DB.Tbl_Folders
                         on a.fld_FK_FolderID equals f.fld_FolderID
                         join d in DB.Tbl_Department
                         on p.fld_FK_DepartmentID equals d.fld_DepartmentID
                         //where a.fld_state != 1
                         //      && a.fld_AdminChDate >= AppSData
                         //      && a.fld_AdminChDate <= AppEData
                         //      && a.fld_HDate >= ReqSData
                         //      && a.fld_HDate <= ReqEData
                         //      && p.fld_FK_DepartmentID == Ou_ID
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
                            // f.fld_FolderOwner,
                             p.fld_PersonFName,
                             p.fld_PersonLName,
                             p.fld_PersonNO,
                             p.fld_PersonID,
                             p.fld_FK_DepartmentID,
                             d.fld_DepartmentName
                             //,
                             //p.fld_PersonAdded
                         }).ToList();


                var AllAccess = new List<AllAccessViewModel>();

                //int i = 1;
                foreach (var x in q)
                {
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
                    //        AdminCh = "حذف دسترسی ها ";
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
                       // PersonAdded = x.fld_PersonAdded,
                        UserOU = x.fld_DepartmentName
                        //UserOU = new OUsViewModel { OUID = (int)x.fld_FK_DepartmentID, OUFaName = x.fld_DepartmentName }

                    });
                }

                return AllAccess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }






    }
}