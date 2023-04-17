//بسم الله الرحمن الرحیم
using FSRM.Models;
using FSRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using FSRM.Models.DataModel;
using System.Linq;

namespace FSRM.Services
{
    public class UserServices
    {
        #region Feild

        UserAccessData Obj_Access = new UserAccessData();
        AccessLogData Obj_AccLog = new AccessLogData();
        UserFolderData Obj_Folder = new UserFolderData();
        FolderLogData Obj_FolderLog = new FolderLogData();
        EmailServices Obj_Mail = new EmailServices();
        EmailData Obj_MailLog = new EmailData();
        
        db_FSRMEntities db = new db_FSRMEntities();
       

        #endregion

        #region User Permits

        public IEnumerable<UserPermitViewModel> ReadUserPermits(string OwnerName)
        {
            return Obj_Access.ReadUserPermits(OwnerName.ToLower());
        }


        public bool CreateNewUserPermit(UserPermitViewModel model, string OwnerMail, string OwnerOU)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();
            string eTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

            model.HDate = yr + mn + dy;
            model.MDate = dt;

            #endregion

            model.OwnerName = model.OwnerName.ToLower();
            model.PersonFName = model.PersonFName.ToLower();
            model.PersonLName=model.PersonLName.ToLower();

            int res = Obj_Access.CreateNewUserPermit(model, OwnerMail, OwnerOU, eTime);

            #region Send Email


            if (res != 0)
            {
                var AdminEmails = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);


                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست دسترسی جدید توسط کاربر " + model.OwnerName.ToString() + " ثبت گردید. </p> </div>";

                foreach (var Adr in AdminEmails)
                {
                    int MailStatus = Obj_Mail.SendMail(eBody, Adr.fld_AdminADName+ "@Domain.com") ? 1 : 0;

                    Obj_MailLog.SaveLog_Access(MailStatus, res, Adr.fld_AdminADName + "@Domain.com", eBody, model.HDate.ToString(), eTime, dt);
                }

                

            }

            #endregion

            return (res != 0) ? true : false;

        }


        public bool RemoveUserPermit(UserPermitViewModel model)
        {

            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

            int HDate = int.Parse(yr + mn + dy);


            #endregion

            string Res = "";

            model.OwnerName = model.OwnerName.ToLower();
            model.PersonFName = model.PersonFName.ToLower();
            model.PersonLName = model.PersonLName.ToLower();

            // REmove directly - Admin not checked
            if (model.EnableEdit)
            {
                Res = Obj_Access.RemoveUserPermit(model, HDate, HTime, dt);
            }
            else
            {
                // Admin has benn Checked before - 
                Res = Obj_Access.RemoveUserPermit_AfterAdminChecked(model, HDate, HTime, dt);
            }



            if (Res == "Email")
            {
                #region Send Email

                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست حذف دسترسی توسط کاربر " + model.OwnerName.ToString() + " ثبت گردید. </p> </div>";
                var AdminEmails = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);

                foreach (var Adr in AdminEmails)
                {
                    int MailStatus = Obj_Mail.SendMail(eBody, Adr.fld_AdminADName + "@Domain.com") ? 1 : 0;

                    Obj_MailLog.SaveLog_Access(MailStatus, model.AccessID, Adr.fld_AdminADName + "@Domain.com", eBody, HDate.ToString(), HTime, dt);
                }


                #endregion
            }

            bool rs = true;

            if (Res == "Failed")
            {
                rs = false;
            }
            if (Res == "Done")
            {
                rs = true;
            }


            return rs;
        }


        public bool UpdateUserPermit(UserPermitViewModel model, string OwnerMail, string OwnerOU)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

            int HDate = int.Parse(yr + mn + dy);

            model.OwnerName = model.OwnerName.ToLower();
            model.PersonFName = model.PersonFName.ToLower();
            model.PersonLName = model.PersonLName.ToLower();

            #endregion
            string Res = Obj_Access.UpdateUserPermit(model, OwnerMail, OwnerOU, HDate, HTime, dt);
            if (Res == "Email")
            {
                #region Send Email

                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست بروزرسانی دسترسی توسط کاربر " + model.OwnerName.ToString() + " ثبت گردید. </p> </div>";
                var AdminEmails = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);
                foreach (var Adr in AdminEmails)
                {
                    int MailStatus = Obj_Mail.SendMail(eBody, Adr.fld_AdminADName + "@Domain.com") ? 1 : 0;

                    Obj_MailLog.SaveLog_Access(MailStatus, model.AccessID, Adr.fld_AdminADName + "@Domain.com", eBody, HDate.ToString(), HTime, dt);
                }

                #endregion
            }

            bool rs = true;
            if (Res == "Failed")
            {
                rs = false;
            }
            if (Res == "Done")
            {
                rs = true;
            }

            return rs;
        }


        public IList<LogAccessViewModel> AccessLogByID(int AccID)
        {
            var AccessLog = Obj_AccLog.AccessLogByID(AccID);


            if (AccessLog.Count > 1)
            {
                string ch = "";
                for (int i = 1; i < AccessLog.Count; i++)
                {
                    string res = FindChanges(AccessLog[i - 1].Read, AccessLog[i].Read);

                    ch = (res != "") ? (res + "Read - ") : "";
                    res = FindChanges(AccessLog[i - 1].Write, AccessLog[i].Write);
                    ch += (res != "") ? (res + "Write - ") : "";
                    res = FindChanges(AccessLog[i - 1].Modify, AccessLog[i].Modify);
                    ch += (res != "") ? (res + "Modify ") : "";
                    AccessLog[i].Changes = ch;

                }
            }


            return AccessLog;
        }


        public string FindChanges(bool Pre, bool Curr)
        {
            string ch = "";
            if (Pre == Curr)
            {
            }
            else
            {
                if (Pre && !Curr)
                {
                    ch = "Remove ";
                }
                else
                {
                    ch = "Add ";
                }
            }


            return ch;
        }

        #endregion


        #region User Folder

        public IList<LogFolderViewModel> FolderLogByID(int FolderID)
        {
            var FolderLog = Obj_FolderLog.FolderLogByID(FolderID);



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
                                ch = ch + "  -  تغییر حجم پیشنهادی از " + FolderLog[i - 1].SugFolderSpace.ToString() + " به " + FolderLog[i].SugFolderSpace.ToString();

                            }
                            else
                            {
                                ch =  " تغییر حجم پیشنهادی از " + FolderLog[i - 1].SugFolderSpace.ToString() + " به " + FolderLog[i].SugFolderSpace.ToString();
                            }

                        }
                    }
                    FolderLog[i].Changes = ch;


                }

            }


            return FolderLog;
        }


        public IEnumerable<UserFolderViewModel> ReadUserFolders(string OwnerName)
        {
            return Obj_Folder.ReadUserFolders(OwnerName.ToLower());
        }


        public bool CreateNewUserFolder(UserFolderViewModel model)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();
            //string eTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

            model.ReqHDate = yr + mn + dy;
            model.ReqMDate = dt;

            #endregion

            model.OwnerName = model.OwnerName.ToLower();

            int res = Obj_Folder.CreateNewUserFolder(model);

            #region Send Email

            if (res != 0)
            {
                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست ایجاد پوشه جدید توسط کاربر " + model.OwnerName.ToString() + " ثبت گردید. </p> </div>";
                var AdminEmails = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);
                foreach (var Adr in AdminEmails)
                {
                    int MailStatus = Obj_Mail.SendMail(eBody, Adr.fld_AdminADName + "@Domain.com") ? 1 : 0;

                    Obj_MailLog.SaveLog_Folder(MailStatus, res, Adr.fld_AdminADName + "@Domain.com", eBody, model.ReqHDate.ToString(), dt);
                }
            }

            #endregion

            return (res != 0) ? true : false;

        }


        public bool RemoveUserFolder(UserFolderViewModel model)
        {

            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            model.ReqHDate = yr + mn + dy;
            model.ReqMDate = dt;

            #endregion
            model.OwnerName = model.OwnerName.ToLower();

            string Res = "";

            // REmove directly - Admin not checked
            if (model.EnableEdit)
            {
                Res = Obj_Folder.RemoveUserFolder(model);
            }
            else
            {
                // Admin has benn Checked before - 
                Res = Obj_Folder.RemoveUserFolder_AfterAdminChecked(model);
            }

            if (Res == "Email")
            {
                #region Send Email

                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست حذف پوشه توسط کاربر " + model.OwnerName.ToString() + " ثبت گردید. </p> </div>";
                var AdminEmails = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);
                foreach (var Adr in AdminEmails)
                {
                    int MailStatus = Obj_Mail.SendMail(eBody, Adr.fld_AdminADName + "@Domain.com") ? 1 : 0;

                    Obj_MailLog.SaveLog_Folder(MailStatus, model.FolderID, Adr.fld_AdminADName + "@Domain.com", eBody, model.ReqHDate.ToString(), dt);
                }
                #endregion
            }

            bool rs = true;

            if (Res == "Failed")
            {
                rs = false;
            }
            if (Res == "Done")
            {
                rs = true;
            }


            return rs;
        }

        public bool UpdateUserFolder(UserFolderViewModel model)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            model.ReqHDate = yr + mn + dy;
            model.ReqMDate = dt;

            model.OwnerName = model.OwnerName.ToLower();

            #endregion
            int Res = Obj_Folder.UpdateUserFolder(model);
            if (Res !=0)
            {
                #region Send Email

                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست بروزرسانی اطلاعات پوشه توسط کاربر " + model.OwnerName.ToString() + " ثبت گردید. </p> </div>";
                var AdminEmails = db.Tbl_Admins.Where(x => x.fld_AdminShow == true && x.fld_AdminStatus != 2);
                foreach (var Adr in AdminEmails)
                {
                    int MailStatus = Obj_Mail.SendMail(eBody, Adr.fld_AdminADName + "@Domain.com") ? 1 : 0;

                    Obj_MailLog.SaveLog_Folder(MailStatus, Res, Adr.fld_AdminADName + "@Domain.com", eBody, model.ReqHDate.ToString(), dt);

                }
                #endregion
            }


            return Res == 0 ? false : true;
        }


        #endregion



    }
}