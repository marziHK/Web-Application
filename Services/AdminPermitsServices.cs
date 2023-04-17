//بسم الله الرحمن الرحیم
using FSRM.Models;
using FSRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FSRM.Services
{
    public class AdminPermitsServices
    {

        #region Feilds

        AdminUserPermitsData Obj_Access = new AdminUserPermitsData();
        EmailServices Obj_Mail = new EmailServices();
        EmailData Obj_MailLog = new EmailData();
        AccessLogData Obj_Log = new AccessLogData();

        #endregion


        public bool AdminChecked(int AccessID, string AdminName)
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

                string OwnerMail = Obj_Access.AdminChecked(AccessID, HDate, HTime, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست تخصیص دسترسی شما توسط راهبر سیستم تایید و اعمال گردید. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Access(MailStatus, AccessID, OwnerMail, eBody, HDate, HTime, dt);
                }



                return res;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool AdminRefusePermit(int AccessID, string AdminName)
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

                string OwnerMail = Obj_Access.AdminRefusePermit(AccessID, HDate, HTime, dt, AdminName);
                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست تخصیص دسترسی شما توسط راهبر سیستم رد شده است. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Access(MailStatus, AccessID, OwnerMail, eBody, HDate, HTime, dt);
                }



                return res;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        //public bool RemovePermit(int AccessID, string AdminName)
        //{
        //    try
        //    {
        //        #region Data and Time

        //        DateTime dt = DateTime.Now;
        //        PersianCalendar pc = new PersianCalendar();
        //        string yr = pc.GetYear(dt).ToString();
        //        string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
        //        string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

        //        string HDate = yr + mn + dy;
        //        string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

        //        #endregion

        //        bool res = true;

        //        string OwnerMail = Obj_Access.RemovePermit(AccessID, HDate, HTime, dt, AdminName);
        //        if (OwnerMail != "")
        //        {
        //            string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست حذف دسترسی شما توسط راهبر سیستم تایید و اعمال گردید. </p> </div>";

        //            int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
        //            res = (MailStatus != 0) ? true : false;
        //            Obj_MailLog.SaveLog_Access(MailStatus, AccessID, OwnerMail, eBody, HDate, HTime, dt);
        //        }



        //        return res;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        public IList<LogAccessViewModel> AccessLogByID_ForAdmin(int AccID)
        {
            return Obj_Log.AccessLogByID_ForAdmin(AccID);
        }

        public IEnumerable<AdminUserPermitsViewModel> ReadAllUserPermits()
        {
            return Obj_Access.ReadAllUserPermits();
        }


        public bool UpdateUserPermits_ByAdmin(AdminUserPermitsViewModel model, string AdminName)
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
            string Res = Obj_Access.UpdateUserPermits_ByAdmin(model, AdminName, HDate, HTime, dt);

            bool rs = true;
            if (Res == "Failed")
            {
                rs = false;
            }
            else
            {
                #region Send Email
                string[] det = Res.Split('|');

                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست دسترسی شما توسط مدیر سیستم بازنگری و اصلاح شده است. </p> </div>";

                int MailStatus = Obj_Mail.SendMail(eBody, det[1]) ? 1 : 0;

                Obj_MailLog.SaveLog_Access(MailStatus, Int32.Parse(det[0]), det[1], eBody, HDate.ToString(), HTime, dt);


                #endregion
                rs = true;
            }
            return rs;
        }

        public bool DestroyPermit(AdminUserPermitsViewModel model, string AdminName)
        {
            bool Ch = Obj_Access.DestroyPermit(model);
            if (Ch)
            {
                return RemoveUserPermit_ByAdmin(model, AdminName);
            }
            else
            {
                return RemoveUserPermitsRequest_ByAdmin(model, AdminName);
            }
        }


        // در ابتدا
        public bool RemoveUserPermitsRequest_ByAdmin(AdminUserPermitsViewModel model, string AdminName)
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

            string Res = Obj_Access.RemoveUserPermitsRequest_ByAdmin(model, AdminName, HDate, HTime, dt);

            bool rs = true;
            if (Res == "Failed")
            {
                rs = false;
            }
            else
            {
                #region Send Email
                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست تخصیص دسترسی شما توسط مدیر سیستم حذف گردید. </p> </div>";

                int MailStatus = Obj_Mail.SendMail(eBody, Res) ? 1 : 0;

                Obj_MailLog.SaveLog_Access(MailStatus, model.AccessID, Res, eBody, HDate.ToString(), HTime, dt);


                #endregion
                rs = true;
            }
            return rs;
        }


        public bool RemoveUserPermit_ByAdmin(AdminUserPermitsViewModel model, string AdminName)
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

            string Res = Obj_Access.RemoveUserPermit_ByAdmin(model.AccessID, HDate.ToString(), HTime, dt, AdminName);

            bool rs = true;
            if (Res == "Failed")
            {
                rs = false;
            }
            else
            {
                #region Send Email
                string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p >  دسترسی های ایجاد شده توسط مدیر سیستم سلب گردید. </p> </div>";

                int MailStatus = Obj_Mail.SendMail(eBody, Res) ? 1 : 0;

                Obj_MailLog.SaveLog_Access(MailStatus, model.AccessID, Res, eBody, HDate.ToString(), HTime, dt);


                #endregion
                rs = true;
            }
            return rs;
        }


        public bool RemovePermits_UserRequst(int AccID, string AdminName)
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

                string OwnerMail = Obj_Access.RemovePermits_UserRequst(AccID, AdminName, int.Parse(HDate), HTime, dt);

                if (OwnerMail != "")
                {
                    string eBody = "<div style=\"direction: rtl; \"> <h4>با سلام </h4> <p > درخواست سلب دسترسی ها توسط راهبر سیستم تایید و اعمال گردید. </p> </div>";

                    int MailStatus = Obj_Mail.SendMail(eBody, OwnerMail) ? 1 : 0;
                    res = (MailStatus != 0) ? true : false;
                    Obj_MailLog.SaveLog_Access(MailStatus, AccID, OwnerMail, eBody, HDate.ToString(), HTime, dt);
                }


                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }

}
