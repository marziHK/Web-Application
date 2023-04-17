//بسم الله الرحمن الرحیم
using FSRM.Models;
using FSRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using FSRM.Models.DataModel;

namespace FSRM.Services
{
    public class AdminFolderServices
    {

        #region Feilds

        AdminUserFolderData Obj_Folder = new AdminUserFolderData();
        EmailServices Obj_Mail = new EmailServices();
        EmailData Obj_MailLog = new EmailData();
        FolderLogData Obj_Log = new FolderLogData();

        #endregion


        public bool AdminChecked(int FolderID, string AdminName)
        {
            try
            {
                #region Data and Time

                DateTime dt = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string yr = pc.GetYear(dt).ToString();
                string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
                string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

                string HDate = yr + mn + dy;
                string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

                #endregion

                bool res = true;

                string OwnerMail = Obj_Folder.AdminChecked(FolderID, HDate, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست ایجاد پوشه شما توسط راهبر سیستم تایید و ایجاد گردید. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Folder(MailStatus, FolderID, OwnerMail, eBody, HDate, dt);
                }



                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AdminRefuseFolder(int FolderID, string AdminName)
        {
            try
            {
                #region Data and Time

                DateTime dt = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string yr = pc.GetYear(dt).ToString();
                string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
                string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

                string HDate = yr + mn + dy;
                string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

                #endregion

                bool res = true;

                string OwnerMail = Obj_Folder.AdminRefuseFolder(FolderID, HDate, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست ایجاد پوشه شما توسط راهبر سیستم رد شده است. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Folder(MailStatus, FolderID, OwnerMail, eBody, HDate, dt);
                }



                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IList<LogFolderViewModel> FolderLogByID_ForAdmin(int FolderID)
        {
            var FolderLog = Obj_Log.FolderLogByID_ForAdmin(FolderID);



            if (FolderLog.Count > 1)
            {
                string ch = "";
                for (int i = 1; i < FolderLog.Count; i++)
                {
                    // تغییرات توسط مدیر سیستم
                    if (FolderLog[i].FolderStatusCode == 7)
                    {
                        if (FolderLog[i].FolderAddress != FolderLog[i - 1].FolderAddress)
                        {
                            ch = "تغییر آدرس پیشنهادی از " + FolderLog[i - 1].FolderAddress + " به " + FolderLog[i].FolderAddress;
                        }
                        if (FolderLog[i].FolderSpace != FolderLog[i - 1].FolderSpace)
                        {
                            if (ch != "")
                            {
                                ch = ch + "  -  تغییر حجم پوشه از " + FolderLog[i - 1].FolderSpace.ToString() + " به " + FolderLog[i].FolderSpace.ToString();

                            }
                            else
                            {
                                ch = " تغییر حجم پوشه از " + FolderLog[i - 1].FolderSpace.ToString() + " به " + FolderLog[i].FolderSpace.ToString();
                            }
                        }
                    }

                    //تغییرات توسط کاربر
                    if (FolderLog[i].FolderStatusCode == 8)
                    {

                        if (FolderLog[i].SugFolderAddress != FolderLog[i - 1].SugFolderAddress)
                        {
                            ch = "تغییر آدرس پوشه از " + FolderLog[i - 1].SugFolderAddress + " به " + FolderLog[i].SugFolderAddress;
                        }
                        if (FolderLog[i].SugFolderName != FolderLog[i - 1].SugFolderName)
                        {
                            if (ch != "")
                            {
                                ch = ch + "  -  تغییر نام پیشنهادی از " + FolderLog[i - 1].SugFolderName + " به " + FolderLog[i].SugFolderName;

                            }
                            else
                            {
                                ch = " تغییر نام پیشنهادی از " + FolderLog[i - 1].SugFolderName + " به " + FolderLog[i].SugFolderName;
                            }
                        }
                        if (FolderLog[i].SugFolderSpace != FolderLog[i - 1].SugFolderSpace)
                        {
                            if (ch != "")
                            {
                                ch = ch + "  -  تغییر حجم پیشنهادی از " + FolderLog[i - 1].SugFolderSpace.ToString() + " به " + FolderLog[i].FolderSpace.ToString();

                            }
                            else
                            {
                                ch = " تغییر حجم پیشنهادی از " + FolderLog[i - 1].SugFolderSpace.ToString() + " به " + FolderLog[i].SugFolderSpace.ToString();
                            }

                        }
                    }
                    FolderLog[i].Changes = ch;


                }

            }
            return FolderLog;


        }

        public IEnumerable<AdminUserFolderViewModel> ReadAllUserFolders()
        {
            var q = Obj_Folder.ReadAllUserFolders();


            return q;
        }


        public bool UpdateFolderInfo_ByAdmin(AdminUserFolderViewModel model, string AdminName)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            string HDate = yr + mn + dy;

            #endregion
            return Obj_Folder.UpdateFolderInfo_ByAdmin(model, HDate, dt, AdminName);
        }

        public bool RemoveFolder_UserRequst(int FolderID, string AdminName)
        {
            try
            {
                #region Data and Time

                DateTime dt = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string yr = pc.GetYear(dt).ToString();
                string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
                string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

                string HDate = yr + mn + dy;
                string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

                #endregion

                bool res = true;

                string OwnerMail = Obj_Folder.RemoveFolder_UserRequst(FolderID, HDate, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست حذف پوشه شما توسط راهبر سیستم تایید و اعمال گردید. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Folder(MailStatus, FolderID, OwnerMail, eBody, HDate, dt);
                }



                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool DestroyFolder(AdminUserFolderViewModel model, string AdminName)
        {
            bool Ch = Obj_Folder.DestroyFolder(model);
            if (Ch)
            {
                return RemoveFolder_ByAdmin(model.FolderID, AdminName);
            }
            else
            {
                return RemoveFolderRequst_ByAdmin(model.FolderID, AdminName);
            }
        }

        public bool RemoveFolder_ByAdmin(int FolderID, string AdminName)
        {
            try
            {
                #region Data and Time

                DateTime dt = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string yr = pc.GetYear(dt).ToString();
                string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
                string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

                string HDate = yr + mn + dy;
                string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

                #endregion

                bool res = true;

                string OwnerMail = Obj_Folder.RemoveFolder_ByAdmin(FolderID, HDate, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > پوشه توسط راهبر سیستم حذف گردید. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Folder(MailStatus, FolderID, OwnerMail, eBody, HDate, dt);
                }



                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveFolderRequst_ByAdmin(int FolderID, string AdminName)
        {
            try
            {
                #region Data and Time

                DateTime dt = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string yr = pc.GetYear(dt).ToString();
                string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
                string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

                string HDate = yr + mn + dy;
                string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

                #endregion
                bool res = true;
                string OwnerMail = Obj_Folder.RemoveFolderRequst_ByAdmin(FolderID, HDate, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست ایجاد پوشه در FSRM مورد تایید نمی باشد و حذف گردید. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Folder(MailStatus, FolderID, OwnerMail, eBody, HDate, dt);
                }

                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }
}