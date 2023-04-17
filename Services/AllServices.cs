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
        ReportData Obj_Rep = new ReportData();
        EmailServices Obj_Mail = new EmailServices();
        EmailData Obj_MailLog = new EmailData();


        #endregion

        public List<AllAccessViewModel> ReadAllAccess()
        {
            return Obj_Access.ReadAllAccess();

        }

        public List<AllAccessViewModel> Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        //public void Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        {
            if (OU_ID == "")
            {
                OU_ID = null;
            }
            //if (Owner == "")
            //{
            //    Owner = null;

            //}
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
                string yr = pc.GetYear(dt).ToString();
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
                    ReportsRes = Obj_Rep.ReportDates(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData));
                }
                else if (Owner != "" && OU_ID != null && ch_Access != null)
                {
                    // همه شروط
                    if (ch_Access.Length == 1)
                    {
                        ReportsRes = Obj_Rep.ReportAll_1ch(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner, int.Parse(OU_ID));
                    }
                    else if (ch_Access.Length == 2)
                    {
                        if (Read && Write)
                        {
                            ReportsRes = Obj_Rep.ReportAll_2Ch_RW(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner, int.Parse(OU_ID));
                        }
                        else if (Read && Modify)
                        {
                            ReportsRes = Obj_Rep.ReportAll_2Ch_RM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner, int.Parse(OU_ID));
                        }
                        else if (Write && Modify)
                        {
                            ReportsRes = Obj_Rep.ReportAll_2Ch_WM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner, int.Parse(OU_ID));
                        }
                    }
                    else if (ch_Access.Length == 3)
                    {
                        ReportsRes = Obj_Rep.ReportAll(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner, int.Parse(OU_ID));
                    }

                }
                else if (Owner != "")
                {
                    if (OU_ID == null && ch_Access == null)
                    {
                        // فقط مالک
                        ReportsRes = Obj_Rep.Report_Owner(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Owner);
                    }
                    else if (OU_ID != null && ch_Access == null)
                    {
                        // مالک و معاونت
                        ReportsRes = Obj_Rep.Report_Owner_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Owner, int.Parse(OU_ID));
                    }
                    else if (OU_ID == null && ch_Access != null)
                    {
                        // مالک و دسترسی
                        if (ch_Access.Length == 1)
                        {
                            ReportsRes = Obj_Rep.Report_Owner_Access_1ch(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                        }
                        else if (ch_Access.Length == 2)
                        {
                            if (Read && Write)
                            {
                                ReportsRes = Obj_Rep.Report_Owner_Access_2Ch_RW(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);

                            }
                            else if (Read && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Owner_Access_2Ch_RM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                            }
                            else if (Write && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Owner_Access_2Ch_WM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                            }
                        }
                        else if (ch_Access.Length == 3)
                        {
                            ReportsRes = Obj_Rep.Report_Owner_Access(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                        }
                    }
                }

                else if (OU_ID != null)
                {
                    if (Owner == "" && ch_Access == null)
                    {
                        // فقط معاونت
                        ReportsRes = Obj_Rep.Report_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), int.Parse(OU_ID));
                    }
                    else if (Owner != "" && ch_Access == null)
                    {
                        // معاونت و مالک 
                        ReportsRes = Obj_Rep.Report_Owner_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Owner, int.Parse(OU_ID));
                    }
                    else if (Owner == "" && ch_Access != null)
                    {
                        // معاونت و دسترسی
                        if (ch_Access.Length == 1)
                        {
                            ReportsRes = Obj_Rep.Report_Access_OU_1ch(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                        }
                        else if (ch_Access.Length == 2)
                        {
                            if (Read && Write)
                            {
                                ReportsRes = Obj_Rep.Report_Access_OU_2ch_RW(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                            }
                            else if (Read && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Access_OU_2ch_RM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                            }
                            else if (Write && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Access_OU_2ch_WM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                            }
                        }
                        else if (ch_Access.Length == 3)
                        {
                            ReportsRes = Obj_Rep.Report_Access_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                        }
                    }
                }

                else if (ch_Access != null)
                {
                    if (Owner == "" && OU_ID == null)
                    {
                        // فقط دسترسی ها
                        if (ch_Access.Length == 1)
                        {
                            ReportsRes = Obj_Rep.Report_Access_1ch(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify);
                        }
                        else if (ch_Access.Length == 2)
                        {
                            if (Read && Write)
                            {
                                ReportsRes = Obj_Rep.Report_Access_2ch_RW(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify);
                            }
                            else if (Read && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Access_2ch_RM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify);
                            }
                            else if (Write && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Access_2ch_WM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify);
                            }
                        }
                        else if (ch_Access.Length == 3)
                        {
                            ReportsRes = Obj_Rep.Report_AccessAll(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify);
                        }
                    }
                    else if (Owner != "" && OU_ID == null)
                    {
                        // دسترسی و مالک 
                        if (ch_Access.Length == 1)
                        {
                            ReportsRes = Obj_Rep.Report_Owner_Access_1ch(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                        }
                        else if (ch_Access.Length == 2)
                        {
                            if (Read && Write)
                            {
                                ReportsRes = Obj_Rep.Report_Owner_Access_2Ch_RW(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);

                            }
                            else if (Read && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Owner_Access_2Ch_RM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                            }
                            else if (Write && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Owner_Access_2Ch_WM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                            }
                        }
                        else if (ch_Access.Length == 3)
                        {
                            ReportsRes = Obj_Rep.Report_Owner_Access(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, Owner);
                        }
                    }
                    else if (Owner == "" && OU_ID != null)
                    {
                        // دسترسی و معاونت
                        if (ch_Access.Length == 1)
                        {
                            ReportsRes = Obj_Rep.Report_Access_OU_1ch(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                        }
                        else if (ch_Access.Length == 2)
                        {
                            if (Read && Write)
                            {
                                ReportsRes = Obj_Rep.Report_Access_OU_2ch_RW(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                            }
                            else if (Read && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Access_OU_2ch_RM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                            }
                            else if (Write && Modify)
                            {
                                ReportsRes = Obj_Rep.Report_Access_OU_2ch_WM(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                            }
                        }
                        else if (ch_Access.Length == 3)
                        {
                            ReportsRes = Obj_Rep.Report_Access_OU(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData), Read, Write, Modify, int.Parse(OU_ID));
                        }
                    }
                }
                else
                {
                    ReportsRes = ReadAllAccess();
                }
            }




            return ReportsRes;

        }



       
    }
}