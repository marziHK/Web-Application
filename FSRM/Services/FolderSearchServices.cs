//بسم الله الرحمن الرحیم
using FSRM.Models;
using FSRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FSRM.Services
{
    public class FolderSearchServices
    {

        #region Feilds

        FolderSearchData Obj_Rep = new FolderSearchData();
        //AdminUserFolderData Obj_Folder = new AdminUserFolderData();

        #endregion


        public List<AdminUserFolderViewModel> Report(string ReqSData, string ReqEData, string AppSData, string AppEData,
                            string[] ch_Types, string Owner, string OU_ID, string ApprovedValue, string FolderAddress, string AdminName)
        //public void Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        {
            var ReportReqDate = new List<AdminUserFolderViewModel>();
            var ReportsDate = new List<AdminUserFolderViewModel>();
            var ReportsFolder = new List<AdminUserFolderViewModel>();
            var ReportsRes = new List<AdminUserFolderViewModel>();

            //if (ReqSData == "" && ReqEData == "" && AppSData == "" && AppEData == "" && Owner == "" && OU_ID == "" && ch_Types == null && ApprovedValue == "")
            //{
            //    //بدون فیلتر -تمام داده ها نشان د اده شود
            //    ReportsRes = Obj_Folder.ReadAllUserFolders();
            //}
            //else
            //{
            #region Dates - بررسی تاریخ درخواست

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();
            string Today = yr + mn + dy;
            ReqSData = ReqSData == null || ReqSData == "" ? "14001101" : ReqSData.Remove(4, 1).Remove(6, 1);
            ReqEData = ReqEData == null || ReqEData == "" ? Today : ReqEData.Remove(4, 1).Remove(6, 1);
            //AppSData = AppSData == null || AppSData == "" ? "14001101" : AppSData.Remove(4, 1).Remove(6, 1);
            //AppEData = AppEData == null || AppEData == "" ? Today : AppEData.Remove(4, 1).Remove(6, 1);


            #endregion


            #region Check Type List

            bool RequestOpened = false;
            bool RequestClosed = false;
            bool RequestRefused = false;
            bool RequestRemoved = false;

            if (ch_Types != null)
            {
                foreach (var x in ch_Types)
                {
                    switch (x)
                    {
                        case "1":
                            RequestOpened = true;
                            break;
                        case "2":
                            RequestClosed = true;
                            break;
                        case "3":
                            RequestRefused = true;
                            break;
                        case "4":
                            RequestRemoved = true;
                            break;
                    }
                }
            }

            #endregion

            //  نتایج با اعمال تاریخ درخواست ها

            #region نام مدیر سیستم  - تاریخ درخواست و وضعیت درخواست ها  -- ReportReqDate

            if (AdminName == null || AdminName == "")
            {
                #region فیلتر داده ها بر اساس تاریخ درخواست و وضعیت درخواست ها  -- ReportReqDate

                if (ch_Types == null)
                {
                    // فقط بر اساس تاریخ درخواست 
                    ReportReqDate = Obj_Rep.ReportByRequestDates(int.Parse(ReqSData), int.Parse(ReqEData));
                }
                else if (ch_Types.Length == 1)
                {
                    if (RequestOpened)
                    {
                        //درخواست باز
                        //admincheck = false 
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Open(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestClosed)
                    {
                        //تایید شده
                        //codes : 4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Closed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestRefused)
                    {
                        //تایید نشده
                        //codes : 5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Refused(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestRemoved)
                    {
                        //حذف شده
                        //codes : 10/9/8
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Removed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                }
                else if (ch_Types.Length == 2)
                {
                    if (RequestOpened && RequestClosed)
                    {
                        // باز و تایید شده
                        //admincheck = false  & codes : 4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Open_Closed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestOpened && RequestRefused)
                    {
                        // باز و تایید نشده
                        //admincheck = false  & codes : 5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Open_Refused(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestOpened && RequestRemoved)
                    {
                        // باز و حذف شده
                        //admincheck = false  & codes : 10/9/8
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Open_Removed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestClosed && RequestRefused)
                    {
                        // تایید شده و تایید نشده
                        //codes : 4/6/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Closed_Refused(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestClosed && RequestRemoved)
                    {
                        // تایید شده و حذف شده
                        //codes : 10/9/8/4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Closed_Removed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestRemoved && RequestRefused)
                    {
                        // تایید نشده و حذف نشده
                        //codes : 10/9/8/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Removed_Refused(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                }
                else if (ch_Types.Length == 3)
                { // admincheck = false 
                    if (RequestOpened && RequestClosed && RequestRefused)
                    {
                        // باز و تایید شده و تایید نشده
                        //admincheck = false  & codes : 4/6/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Open_Closed_Refused(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestOpened && RequestClosed && RequestRemoved)
                    {
                        // باز و تایید شده و حذف شده
                        //admincheck = false  & codes : 10/9/8/4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Open_Closed_Removed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestOpened && RequestRefused && RequestRemoved)
                    {
                        // باز و تایید نشده و حذف شده
                        //admincheck = false  & codes : 10/9/8/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Open_Refused_Removed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                    if (RequestClosed && RequestRefused && RequestRemoved)
                    {
                        // تایید شده و تایید نشده و حذف شده
                        //codes : 10/9/8/4/6/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Closed_Refused_Removed(int.Parse(ReqSData), int.Parse(ReqEData));
                    }
                }
                else if (ch_Types.Length == 4)
                {
                    // همه درخواست ها 
                    ReportReqDate = Obj_Rep.ReportByRequestDates_4r_AllRequestType(int.Parse(ReqSData), int.Parse(ReqEData));
                }

                #endregion

            }
            else
            {

                #region فیلتر داده ها بر اساس تاریخ درخواست و وضعیت درخواست ها  -- ReportReqDate

                if (ch_Types == null)
                {
                    // فقط بر اساس تاریخ درخواست 
                    ReportReqDate = Obj_Rep.ReportByRequestDates_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                }
                else if (ch_Types.Length == 1)
                {
                    if (RequestOpened)
                    {
                        //درخواست باز
                        //admincheck = false 
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Open_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestClosed)
                    {
                        //تایید شده
                        //codes : 4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Closed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestRefused)
                    {
                        //تایید نشده
                        //codes : 5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Refused_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestRemoved)
                    {
                        //حذف شده
                        //codes : 10/9/8
                        ReportReqDate = Obj_Rep.ReportByRequestDates_1r_Removed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                }
                else if (ch_Types.Length == 2)
                {
                    if (RequestOpened && RequestClosed)
                    {
                        // باز و تایید شده
                        //admincheck = false  & codes : 4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Open_Closed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestOpened && RequestRefused)
                    {
                        // باز و تایید نشده
                        //admincheck = false  & codes : 5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Open_Refused_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestOpened && RequestRemoved)
                    {
                        // باز و حذف شده
                        //admincheck = false  & codes : 10/9/8
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Open_Removed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestClosed && RequestRefused)
                    {
                        // تایید شده و تایید نشده
                        //codes : 4/6/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Closed_Refused_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestClosed && RequestRemoved)
                    {
                        // تایید شده و حذف شده
                        //codes : 10/9/8/4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Closed_Removed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestRemoved && RequestRefused)
                    {
                        // تایید نشده و حذف نشده
                        //codes : 10/9/8/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_2r_Removed_Refused_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                }
                else if (ch_Types.Length == 3)
                { // admincheck = false 
                    if (RequestOpened && RequestClosed && RequestRefused)
                    {
                        // باز و تایید شده و تایید نشده
                        //admincheck = false  & codes : 4/6/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Open_Closed_Refused_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestOpened && RequestClosed && RequestRemoved)
                    {
                        // باز و تایید شده و حذف شده
                        //admincheck = false  & codes : 10/9/8/4/6
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Open_Closed_Removed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestOpened && RequestRefused && RequestRemoved)
                    {
                        // باز و تایید نشده و حذف شده
                        //admincheck = false  & codes : 10/9/8/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Open_Refused_Removed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                    if (RequestClosed && RequestRefused && RequestRemoved)
                    {
                        // تایید شده و تایید نشده و حذف شده
                        //codes : 10/9/8/4/6/5
                        ReportReqDate = Obj_Rep.ReportByRequestDates_3r_Closed_Refused_Removed_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                    }
                }
                else if (ch_Types.Length == 4)
                {
                    // همه درخواست ها 
                    ReportReqDate = Obj_Rep.ReportByRequestDates_4r_AllRequestType_AdminName(int.Parse(ReqSData), int.Parse(ReqEData), AdminName);
                }

                #endregion
            }


            #endregion


            #region فیلتر بر اساس تاریخ بررسی مدیر

            if (AppSData == "" && AppEData == "")
            {
                ReportsDate = ReportReqDate;
            }
            else if (AppSData != "" && AppEData == "")
            {
                AppSData = AppSData.Remove(4, 1).Remove(6, 1);
                foreach (var item in ReportReqDate)
                {
                    if (item.AdminChecked && item.ReportAdminDate != 0 && item.ReportAdminDate >= int.Parse(AppSData))
                    {
                        ReportsDate.Add(item);
                    }
                }
            }
            else if (AppSData == "" && AppEData != "")
            {
                AppEData = AppEData.Remove(4, 1).Remove(6, 1);
                foreach (var item in ReportReqDate)
                {
                    if (item.AdminChecked && item.ReportAdminDate != 0 && item.ReportAdminDate <= int.Parse(AppEData))
                    {
                        ReportsDate.Add(item);
                    }
                }
            }
            else if (AppSData != "" && AppEData != "")
            {
                AppSData = AppSData.Remove(4, 1).Remove(6, 1);
                AppEData = AppEData.Remove(4, 1).Remove(6, 1);

                foreach (var item in ReportReqDate)
                {
                    if (item.AdminChecked && item.ReportAdminDate != 0 && item.ReportAdminDate >= int.Parse(AppSData) && item.ReportAdminDate <= int.Parse(AppEData))
                    {
                        ReportsDate.Add(item);
                    }
                }
            }

            #endregion


            #region فیلتر بر اساس نام مسیر پوشه --  ReportsFolder

            if (FolderAddress == "")
            {
                foreach (var item in ReportsDate)
                {
                    ReportsFolder.Add(item);
                }
            }
            else
            {
                foreach(var item in ReportsDate)
                {
                    if (item.FolderAddress!= null && item.FolderAddress.ToLower().Contains(FolderAddress.ToLower()))
                    {
                        ReportsFolder.Add(item);
                    }
                }
            }

            #endregion

            bool Ch = false;

            if (Owner == "" && OU_ID == "" && ApprovedValue == "" )
            {
                // Ca-0
                // تاریخ ها و وضعیت درخواست
                foreach (var item in ReportsFolder)
                {
                    ReportsRes.Add(item);
                }
                Ch = true;

            }
            else
            {
                if (Owner != "" && !Ch)
                {
                    if (OU_ID == "" && ApprovedValue == "")
                    {
                        // Ca-A
                        // مالک
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                       // ReportsRes.Sort();
                    }
                    else if (OU_ID != "" && ApprovedValue == "")
                    {
                        // Ca-D
                        // مالک + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && ApprovedValue != "")
                    {
                        // Ca-E
                        // مالک + حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && ApprovedValue != "")
                    {
                        // Ca-F
                        // مالک + حجم + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (Owner == "" && !Ch)
                {
                    if (OU_ID != "" && ApprovedValue == "")
                    {
                        // Ca-B
                        // معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && ApprovedValue != "")
                    {
                        // Ca-C
                        // حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && ApprovedValue != "")
                    {
                        // Ca-G
                        // حجم + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }



                if (OU_ID != "" && !Ch)
                {
                    if (Owner == "" && ApprovedValue == "")
                    {
                        // Ca-B
                        // معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && ApprovedValue == "")
                    {
                        // Ca-D
                        // مالک + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && ApprovedValue != "")
                    {
                        // Ca-G
                        // معاونت + حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && ApprovedValue != "")
                    {
                        // Ca-F
                        // مالک + حجم + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (OU_ID == "" && !Ch)
                {
                    if (Owner != "" && ApprovedValue == "")
                    {
                        // Ca-A
                        // مالک 
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && ApprovedValue != "")
                    {
                        // Ca-C
                        // حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && ApprovedValue != "")
                    {
                        // Ca-E
                        // مالک + حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }


                if (ApprovedValue != "" && !Ch)
                {
                    if (OU_ID == "" && Owner == "")
                    {
                        // Ca-C
                        // حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner == "")
                    {
                        // Ca-G
                        // حجم + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner != "")
                    {
                        // Ca-E
                        // مالک + حجم
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner != "")
                    {
                        // Ca-F
                        // مالک + حجم + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (ApprovedValue == "" && !Ch)
                {
                    if (OU_ID != "" && Owner == "")
                    {
                        // Ca-B
                        // معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner != "")
                    {
                        // Ca-A
                        // مالک
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner != "")
                    {
                        // Ca-D
                        // مالک + معاونت
                        Ch = true;
                        foreach (var item in ReportsFolder)
                        {
                            if (item.OwnerName == Owner && item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }

            }



            //}
            return ReportsRes;
        }
    }
}