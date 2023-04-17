//بسم الله الرحمن الرحیم
using FSRM.Models;
using FSRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FSRM.Services
{
    public class AllServices
    {
        #region Feild

        AccessData Obj_Access = new AccessData();



        #endregion

        public List<AllAccessViewModel> ReadAllAccess()
        {
            return Obj_Access.ReadAllAccess();

        }


        public bool AdminChecked(int AccessID)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString().Length == 1 ? "0" + pc.GetYear(dt).ToString() : pc.GetYear(dt).ToString();
            string mn = pc.GetYear(dt).ToString().Length == 1 ? "0" + pc.GetYear(dt).ToString() : pc.GetYear(dt).ToString();
            string dy = pc.GetYear(dt).ToString().Length == 1 ? "0" + pc.GetYear(dt).ToString() : pc.GetYear(dt).ToString();

            string HDate = yr + mn + dy;
            string HTime = string.Format("{0}:{1}:{2}", pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

            #endregion
            return Obj_Access.AdminChecked(AccessID, HDate, HTime);
        }



        public List<AllAccessViewModel> Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        //public void Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        {
            // return Obj_Access.Report("r");
            var ReportsRes = new List<AllAccessViewModel>();
            if (ReqSData == "" && ReqEData == "" && AppSData == "" && AppEData == "" && Owner == "" && OU_ID == null && ch_Access == null)
            {
                ReportsRes = Obj_Access.ReadAllAccess();
            }
            else
            {
                DateTime dt = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                string yr = pc.GetYear(dt).ToString().Length == 1 ? "0" + pc.GetYear(dt).ToString() : pc.GetYear(dt).ToString();
                string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
                string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();
                string Today = yr + mn + dy;
                ReqSData = ReqSData == null || ReqSData == "" ? "14001001" : ReqSData.Remove(4, 1).Remove(6, 1);
                ReqEData = ReqEData == null || ReqEData == "" ? Today : ReqEData.Remove(4, 1).Remove(6, 1);
                AppSData = AppSData == null || AppSData == "" ? "14001001" : AppSData.Remove(4, 1).Remove(6, 1);
                AppEData = AppEData == null || AppEData == "" ? Today : AppEData.Remove(4, 1).Remove(6, 1);


                #region Check Access List

                bool Read = false;
                bool Write = false;
                bool Modify = false;

                if (ch_Access != null)
                {
                    foreach (var x in ch_Access)
                    {
                        switch (x)
                        {
                            case "1":
                                Read = true;
                                break;
                            case "2":
                                Write = true;
                                break;
                            case "3":
                                Modify = true;
                                break;
                        }
                    }
                }



                #endregion




                if (Owner == "" && OU_ID == null && ch_Access == null)
                {
                    // فقط تاریخ
                    ReportsRes = Obj_Access.ReportDates(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData));
                }
                else if (Owner != "" && OU_ID != null && ch_Access != null)
                {
                    // همه شروط
                    ReportsRes = Obj_Access.ReportAll(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner, int.Parse(OU_ID));
                }
                else if (Owner != "")
                {
                    if (OU_ID == null && ch_Access == null)
                    {
                        // فقط مالک
                        ReportsRes = Obj_Access.Report_Owner(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Owner);
                    }
                    else if (OU_ID != null && ch_Access == null)
                    {
                        // مالک و معاونت
                        ReportsRes = Obj_Access.Report_Owner_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Owner, int.Parse(OU_ID));
                    }
                    else if (OU_ID == null && ch_Access != null)
                    {
                        // مالک و دسترسی
                        ReportsRes = Obj_Access.Report_Owner_Access(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                    }
                }

                else if (OU_ID != null)
                {
                    if (Owner == "" && ch_Access == null)
                    {
                        // فقط معاونت
                        ReportsRes = Obj_Access.Report_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), int.Parse(OU_ID));
                    }
                    else if (Owner != "" && ch_Access == null)
                    {
                        // معاونت و مالک 
                        ReportsRes = Obj_Access.Report_Owner_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Owner, int.Parse(OU_ID));
                    }
                    else if (Owner == "" && ch_Access != null)
                    {
                        // معاونت و دسترسی
                        ReportsRes = Obj_Access.Report_Access_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                    }
                }

                else if (ch_Access != null)
                {
                    if (Owner == "" && OU_ID == null)
                    {
                        // فقط دسترسی ها
                        ReportsRes = Obj_Access.Report_Access(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify);
                    }
                    else if (Owner != "" && OU_ID == null)
                    {
                        // دسترسی و مالک 
                        ReportsRes = Obj_Access.Report_Owner_Access(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                    }
                    else if (Owner == "" && OU_ID != null)
                    {
                        // دسترسی و معاونت
                        ReportsRes = Obj_Access.Report_Access_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                    }
                }
                else
                {
                    ReportsRes = ReadAllAccess();
                }
            }

            var Res = new List<AllAccessViewModel>();
            var m = ReportsRes.Find(x => x.PersonAdded.Contains("te"));
            var w = ReportsRes.FindAll(x => x.HDate >= int.Parse(ReqSData));
            foreach (var item in ReportsRes)
            {
                if (item.PersonAdded.Contains("te"))
                {
                    Res.Add(item);
                }

            }


            return ReportsRes;

        }



        #region User Permits

        public IEnumerable<UserPermitViewModel> ReadUserPermits(string OwnerName)
        {
            return Obj_Access.ReadUserPermits(OwnerName);

        }


        public bool CreateNewUserPermit(UserPermitViewModel model)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString().Length == 1 ? "0" + pc.GetYear(dt).ToString() : pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            model.HDate = yr + mn + dy;
            model.MDate = dt;

            #endregion

            int DepID = model.UserOU.OUID;

            return Obj_Access.CreateNewUserPermit(model, DepID);

        }


        public bool RemoveUserPermit(UserPermitViewModel model, int State)
        {
            #region Data and Time

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString().Length == 1 ? "0" + pc.GetYear(dt).ToString() : pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();

            model.HDate = yr + mn + dy;
            model.MDate = dt;

            #endregion

            return Obj_Access.RemoveUserPermit(model, State);
        }


        public bool UpdateUserPermit(UserPermitViewModel model)
        {
            bool Res = RemoveUserPermit(model, 1);
            if (Res)
            {
                Res = CreateNewUserPermit(model);
            }
            return Res;
        }

        #endregion
    }
}