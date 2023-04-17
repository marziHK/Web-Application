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
        //AdminUserFolder Obj_Folder = new AdminUserFolderData();
        #endregion


        public void Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Types, string Owner, string OU_ID, string ApprovedValue)
        //public void Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        {
            var ReportReqDate = new List<AdminUserFolderViewModel>();
            var ReportsDate = new List<AdminUserFolderViewModel>();
            var ReportsRes = new List<AdminUserFolderViewModel>();

            //if (ReqSData == "" && ReqEData == "" && AppSData == "" && AppEData == "" && Owner == "" && OU_ID == null && ch_Types == null)
            //{
            // بدون فیلتر - تمام داده ها نشان د اده شود
            //ReportsRes = Obj_Folder.ReadAllUserFolders();
            //}
            //else
            //{
            #region Dates - بررسی تاریخ ها

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

            //  نتایج با اعمال تاریخ ها
            ReportReqDate = Obj_Rep.ReportByRequestDates(int.Parse(ReqSData), int.Parse(ReqEData), int.Parse(AppSData), int.Parse(AppEData));

            if (AppSData == "" && AppEData == "")
            {
                ReportsDate = ReportReqDate;
            }
            else if (AppSData != "" && AppEData == "")
            {
                foreach (var item in ReportReqDate)
                {
                    if (item.AdminChecked && item.ReportAdminDate !=0 && item.ReportAdminDate >= int.Parse(AppSData))
                    {
                        ReportsDate.Add(item);
                    }
                }
            }
            else if (AppSData == "" && AppEData != "")
            {
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
                foreach (var item in ReportReqDate)
                {
                    if (item.AdminChecked && item.ReportAdminDate != 0 && item.ReportAdminDate >= int.Parse(AppSData) && item.ReportAdminDate <= int.Parse(AppEData))
                    {
                        ReportsDate.Add(item);
                    }
                }
            }

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


            if (ch_Types.Length == 1)
            {
                if (RequestOpened)
                {
                    //درخواست باز
                    //admincheck = false 

                }
                if (RequestClosed)
                {
                    //تایید شده
                    //codes : 4/7

                }
                if (RequestRefused)
                {
                    //تایید نشده
                    //codes : 5

                }
                if (RequestRemoved)
                {
                    //حذف شده
                    //codes : 10/9/6
                    //again
                }
            }
            else if (ch_Types.Length == 2)
            {
                if (RequestOpened && RequestClosed)
                {
                    // باز و تایید شده
                    //admincheck = false  & codes : 4/7
                }
                if (RequestOpened && RequestRefused)
                {
                    // باز و تایید نشده
                    //admincheck = false  & codes : 5

                }
                if (RequestOpened && RequestRemoved)
                {
                    // باز و حذف شده
                    //admincheck = false  & codes : 10/9/6
                    //again

                }
                if (RequestClosed && RequestRefused)
                {
                    // تایید شده و تایید نشده
                    //codes : 4/7/5
                }
                if (RequestClosed && RequestRemoved)
                {
                    // تایید شده و حذف شده
                    //codes : 10/9/6/4/7
                    //again

                }
                if (RequestRemoved && RequestRefused)
                {
                    // تایید نشده و حذف نشده
                    //codes : 10/9/6/5
                    //again

                }
            }
            else if (ch_Types.Length == 3)
            { // admincheck = false 
                if (RequestOpened && RequestClosed && RequestRefused)
                {
                    // باز و تایید شده و تایید نشده
                    //admincheck = false  & codes : 4/7/5
                }
                if (RequestOpened && RequestClosed && RequestRemoved)
                {
                    // باز و تایید شده و حذف شده
                    //admincheck = false  & codes : 10/9/6/4/7
                    //again

                }
                if (RequestOpened && RequestRefused && RequestRemoved)
                {
                    // باز و تایید نشده و حذف شده
                    //admincheck = false  & codes : 10/9/6/5
                    //again

                }
                if (RequestClosed && RequestRefused && RequestRemoved)
                {
                    // تایید شده و تایید نشده و حذف شده
                    //codes : 10/9/6/4/7/5
                    //again

                }
            }
            else if (ch_Types.Length == 4)
            {
                // همه درخواست ها 
            }



            

            if (Owner == "" && OU_ID == null && ch_Types == null && ApprovedValue == "")
            {
                // فقط تاریخ   -- Ca-0
                // درخواست های حذف شده نباید نشان داده شود
                foreach (var item in ReportsDate)
                {
                    if (item.StatusCode != 10 || item.StatusCode != 9)
                    {
                        ReportsRes.Add(item);
                    }
                }

            }
            else
            {
                //  *******************************************************************

                if (OU_ID != null && ReportsDate.Count == 0)//&& Owner == "" && ApprovedValue == "" && ch_Types == null)
                {
                    if (Owner == "" && ApprovedValue == "" && ch_Types == null)
                    {
                        // Ca-A
                        // تاریخ و معاونت    
                        foreach (var item in ReportsDate)
                        {
                            if (item.OwnerOUID == int.Parse(OU_ID))
                            {
                                ReportsDate.Add(item);
                            }
                        }

                    }
                    else if (Owner != "")
                    {
                        if (ApprovedValue == "" && ch_Types == null)
                        {
                            // Ca-B
                            // تاریخ + معاونت + مالک 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-C
                            // تاریخ + معاونت + مالک + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-D
                            // تاریخ + معاونت + مالک + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            // Ca-E
                            // تاریخ + معاونت + مالک + وضعیت + حجم  == همه شروط
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    //ReportsRes.Add(item);
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (Owner == "")
                    {
                        if (ApprovedValue == "" && ch_Types == null)
                        {
                            // Ca-A
                            // تاریخ + معاونت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-F
                            // تاریخ + معاونت + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-G
                            // تاریخ + معاونت + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types != null)
                        {
                            // Ca-H
                            // تاریخ + معاونت + وضعیت + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if ((item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue)))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                else if (OU_ID == null && ReportsDate.Count == 0)
                {
                    if (Owner != "")
                    {
                        if (ApprovedValue == "" && ch_Types == null)
                        {
                            // Ca-I
                            // تاریخ + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-J
                            // تاریخ + مالک + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-K
                            // تاریخ + مالک + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types != null)
                        {
                            // Ca-L
                            // تاریخ + مالک + وضعیت + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (Owner == "")
                    {
                        if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-M
                            // تاریخ + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-N
                            // تاریخ + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                #region وضعیت درخواست

                                if (ch_Types.Length == 1)
                                {
                                    if (RequestOpened)
                                    {
                                        //درخواست باز
                                        //admincheck = false 
                                        if (item.AdminChecked == false)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed)
                                    {
                                        //تایید شده
                                        //codes : 4/7
                                        if (item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }

                                    }
                                    if (RequestRefused)
                                    {
                                        //تایید نشده
                                        //codes : 5
                                        if (item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestRemoved)
                                    {
                                        //حذف شده
                                        //codes : 10/9/6
                                        if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 2)
                                {
                                    if (RequestOpened && RequestClosed)
                                    {
                                        // باز و تایید شده
                                        //admincheck = false  & codes : 4/7
                                        if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRefused)
                                    {
                                        // باز و تایید نشده
                                        //admincheck = false  & codes : 5
                                        if (item.AdminChecked == false || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRemoved)
                                    {
                                        // باز و حذف شده
                                        //admincheck = false  & codes : 10/9/6
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRefused)
                                    {
                                        // تایید شده و تایید نشده
                                        //codes : 4/7/5
                                        if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRemoved)
                                    {
                                        // تایید شده و حذف شده
                                        //codes : 10/9/6/4/7
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestRemoved && RequestRefused)
                                    {
                                        // تایید نشده و حذف نشده
                                        //codes : 10/9/6/5
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 3)
                                { // admincheck = false 
                                    if (RequestOpened && RequestClosed && RequestRefused)
                                    {
                                        // باز و تایید شده و تایید نشده
                                        //admincheck = false  & codes : 4/7/5
                                        if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestClosed && RequestRemoved)
                                    {
                                        // باز و تایید شده و حذف شده
                                        //admincheck = false  & codes : 10/9/6/4/7
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRefused && RequestRemoved)
                                    {
                                        // باز و تایید نشده و حذف شده
                                        //admincheck = false  & codes : 10/9/6/5
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRefused && RequestRemoved)
                                    {
                                        // تایید شده و تایید نشده و حذف شده
                                        //codes : 10/9/6/4/7/5
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 4)
                                {
                                    // همه درخواست ها 
                                    ReportsRes.Add(item);
                                }

                                #endregion


                            }
                        }
                        else if (ApprovedValue != "" && ch_Types != null)
                        {
                            // Ca-O
                            // تاریخ + وضعیت + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }


                //  *******************************************************************


                if (Owner != "" && ReportsDate.Count == 0)
                {
                    if (OU_ID == null && ApprovedValue == "" && ch_Types == null)
                    {
                        // Ca-I
                        // تاریخ + مالک 
                        foreach (var item in ReportsDate)
                        {
                            if (item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != null)
                    {
                        if (ApprovedValue == "" && ch_Types == null)
                        {
                            // Ca-B
                            // تاریخ + مالک + معاونت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-C
                            // تاریخ + مالک + معاونت + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-D                      
                            // تاریخ + مالک + معاونت + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            // Ca-E
                            // تاریخ + مالک + معاونت + وضعیت + حجم   = همه شروط
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    //ReportsRes.Add(item);
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (OU_ID == null)
                    {
                        //if (ApprovedValue == "" && ch_Types == null)
                        //{
                        //    // Ca-I
                        //    // تاریخ + مالک  
                        //    foreach (var item in ReportsDate)
                        //    {
                        //        if (item.OwnerName.Contains(Owner))
                        //        {
                        //            ReportsRes.Add(item);
                        //        }
                        //    }
                        //}
                        //else
                        if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-J
                            // تاریخ + مالک + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-K
                            // تاریخ + مالک + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            // Ca-L
                            // تاریخ + مالک + وضعیت + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                else if (Owner == "" && ReportsDate.Count == 0)
                {
                    if (OU_ID != null)
                    {
                        if (ApprovedValue == "" && ch_Types == null)
                        {
                            // Ca-A
                            // تاریخ + معاونت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-F
                            // تاریخ + معاونت + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-G
                            // تاریخ + معاونت + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (ApprovedValue != "" && ch_Types != null)
                        {
                            // Ca-H
                            // تاریخ + معاونت + وضعیت + حجم  
                            foreach (var item in ReportsDate)
                            {
                                if ((item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue)))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (OU_ID == null)
                    {
                        if (ApprovedValue != "" && ch_Types == null)
                        {
                            // Ca-M
                            // تاریخ + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (ApprovedValue == "" && ch_Types != null)
                        {
                            // Ca-N
                            // تاریخ + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                #region وضعیت درخواست

                                if (ch_Types.Length == 1)
                                {
                                    if (RequestOpened)
                                    {
                                        //درخواست باز
                                        //admincheck = false 
                                        if (item.AdminChecked == false)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed)
                                    {
                                        //تایید شده
                                        //codes : 4/7
                                        if (item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }

                                    }
                                    if (RequestRefused)
                                    {
                                        //تایید نشده
                                        //codes : 5
                                        if (item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestRemoved)
                                    {
                                        //حذف شده
                                        //codes : 10/9/6
                                        if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 2)
                                {
                                    if (RequestOpened && RequestClosed)
                                    {
                                        // باز و تایید شده
                                        //admincheck = false  & codes : 4/7
                                        if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRefused)
                                    {
                                        // باز و تایید نشده
                                        //admincheck = false  & codes : 5
                                        if (item.AdminChecked == false || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRemoved)
                                    {
                                        // باز و حذف شده
                                        //admincheck = false  & codes : 10/9/6
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRefused)
                                    {
                                        // تایید شده و تایید نشده
                                        //codes : 4/7/5
                                        if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRemoved)
                                    {
                                        // تایید شده و حذف شده
                                        //codes : 10/9/6/4/7
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestRemoved && RequestRefused)
                                    {
                                        // تایید نشده و حذف نشده
                                        //codes : 10/9/6/5
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 3)
                                { // admincheck = false 
                                    if (RequestOpened && RequestClosed && RequestRefused)
                                    {
                                        // باز و تایید شده و تایید نشده
                                        //admincheck = false  & codes : 4/7/5
                                        if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestClosed && RequestRemoved)
                                    {
                                        // باز و تایید شده و حذف شده
                                        //admincheck = false  & codes : 10/9/6/4/7
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRefused && RequestRemoved)
                                    {
                                        // باز و تایید نشده و حذف شده
                                        //admincheck = false  & codes : 10/9/6/5
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRefused && RequestRemoved)
                                    {
                                        // تایید شده و تایید نشده و حذف شده
                                        //codes : 10/9/6/4/7/5
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 4)
                                {
                                    // همه درخواست ها 
                                    ReportsRes.Add(item);
                                }

                                #endregion


                            }
                        }
                        else if (ApprovedValue != "" && ch_Types != null)
                        {
                            // Ca-O
                            // تاریخ + وضعیت + حجم  
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }


                //  *******************************************************************

                if (ApprovedValue != "" && ReportsDate.Count == 0)
                {
                    if (OU_ID == null && Owner == "" && ch_Types == null)
                    {
                        // Ca-M
                        //تاریخ و حجم
                        foreach (var item in ReportsDate)
                        {
                            if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != null)
                    {
                        if (Owner == "" && ch_Types == null)
                        {
                            // Ca-F
                            // تاریخ + حجم + معاونت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (Owner != "" && ch_Types == null)
                        {
                            // Ca-C
                            // تاریخ + حجم + معاونت + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner == "" && ch_Types != null)
                        {
                            // Ca-H
                            // تاریخ + حجم + معاونت + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if ((item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue)))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner != "" && ch_Types != null)
                        {
                            // Ca-E
                            // تاریخ + حجم + معاونت + مالک + وضعیت = همه شروط
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    //ReportsRes.Add(item);
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (OU_ID == null)
                    {
                        if (Owner != "" && ch_Types == null)
                        {
                            // Ca-J
                            // تاریخ + حجم + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner == "" && ch_Types != null)
                        {
                            // Ca-O
                            // تاریخ + حجم + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner != "" && ch_Types != null)
                        {
                            // Ca-L
                            // تاریخ + حجم + مالک + وضعیت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                else if (ApprovedValue == "" && ReportsDate.Count == 0)
                {
                    if (OU_ID != null)
                    {
                        if (Owner == "" && ch_Types == null)
                        {
                            // Ca-A
                            // تاریخ + معاونت  
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (Owner != "" && ch_Types == null)
                        {
                            // Ca-B
                            // تاریخ + معاونت + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner == "" && ch_Types != null)
                        {
                            // Ca-G
                            // تاریخ + معاونت + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner != "" && ch_Types != null)
                        {
                            // Ca-D
                            // تاریخ + معاونت + مالک + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (OU_ID == null)
                    {
                        if (Owner != "" && ch_Types == null)
                        {
                            // Ca-I
                            // تاریخ + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner == "" && ch_Types != null)
                        {
                            // Ca-N
                            // تاریخ + وضعیت
                            foreach (var item in ReportsDate)
                            {
                                #region وضعیت درخواست

                                if (ch_Types.Length == 1)
                                {
                                    if (RequestOpened)
                                    {
                                        //درخواست باز
                                        //admincheck = false 
                                        if (item.AdminChecked == false)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed)
                                    {
                                        //تایید شده
                                        //codes : 4/7
                                        if (item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }

                                    }
                                    if (RequestRefused)
                                    {
                                        //تایید نشده
                                        //codes : 5
                                        if (item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestRemoved)
                                    {
                                        //حذف شده
                                        //codes : 10/9/6
                                        if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 2)
                                {
                                    if (RequestOpened && RequestClosed)
                                    {
                                        // باز و تایید شده
                                        //admincheck = false  & codes : 4/7
                                        if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRefused)
                                    {
                                        // باز و تایید نشده
                                        //admincheck = false  & codes : 5
                                        if (item.AdminChecked == false || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRemoved)
                                    {
                                        // باز و حذف شده
                                        //admincheck = false  & codes : 10/9/6
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRefused)
                                    {
                                        // تایید شده و تایید نشده
                                        //codes : 4/7/5
                                        if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRemoved)
                                    {
                                        // تایید شده و حذف شده
                                        //codes : 10/9/6/4/7
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestRemoved && RequestRefused)
                                    {
                                        // تایید نشده و حذف نشده
                                        //codes : 10/9/6/5
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 3)
                                { // admincheck = false 
                                    if (RequestOpened && RequestClosed && RequestRefused)
                                    {
                                        // باز و تایید شده و تایید نشده
                                        //admincheck = false  & codes : 4/7/5
                                        if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestClosed && RequestRemoved)
                                    {
                                        // باز و تایید شده و حذف شده
                                        //admincheck = false  & codes : 10/9/6/4/7
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestOpened && RequestRefused && RequestRemoved)
                                    {
                                        // باز و تایید نشده و حذف شده
                                        //admincheck = false  & codes : 10/9/6/5
                                        if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                    if (RequestClosed && RequestRefused && RequestRemoved)
                                    {
                                        // تایید شده و تایید نشده و حذف شده
                                        //codes : 10/9/6/4/7/5
                                        if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                        {
                                            ReportsRes.Add(item);
                                        }
                                    }
                                }
                                else if (ch_Types.Length == 4)
                                {
                                    // همه درخواست ها 
                                    ReportsRes.Add(item);
                                }

                                #endregion


                            }
                        }
                        else if (Owner != "" && ch_Types != null)
                        {
                            // Ca-K
                            // تاریخ + مالک + وضعیت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }



                //  *******************************************************************

                if (ch_Types != null && ReportsDate.Count == 0)
                {
                    if (OU_ID == null && Owner == "" && ApprovedValue == "")
                    {
                        // Ca-N
                        //تاریخ و وضعیت
                        foreach (var item in ReportsDate)
                        {
                            #region وضعیت درخواست

                            if (ch_Types.Length == 1)
                            {
                                if (RequestOpened)
                                {
                                    //درخواست باز
                                    //admincheck = false 
                                    if (item.AdminChecked == false)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestClosed)
                                {
                                    //تایید شده
                                    //codes : 4/7
                                    if (item.StatusCode == 4 || item.StatusCode == 7)
                                    {
                                        ReportsRes.Add(item);
                                    }

                                }
                                if (RequestRefused)
                                {
                                    //تایید نشده
                                    //codes : 5
                                    if (item.StatusCode == 5)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestRemoved)
                                {
                                    //حذف شده
                                    //codes : 10/9/6
                                    if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                            }
                            else if (ch_Types.Length == 2)
                            {
                                if (RequestOpened && RequestClosed)
                                {
                                    // باز و تایید شده
                                    //admincheck = false  & codes : 4/7
                                    if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestOpened && RequestRefused)
                                {
                                    // باز و تایید نشده
                                    //admincheck = false  & codes : 5
                                    if (item.AdminChecked == false || item.StatusCode == 5)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestOpened && RequestRemoved)
                                {
                                    // باز و حذف شده
                                    //admincheck = false  & codes : 10/9/6
                                    if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestClosed && RequestRefused)
                                {
                                    // تایید شده و تایید نشده
                                    //codes : 4/7/5
                                    if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestClosed && RequestRemoved)
                                {
                                    // تایید شده و حذف شده
                                    //codes : 10/9/6/4/7
                                    if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestRemoved && RequestRefused)
                                {
                                    // تایید نشده و حذف نشده
                                    //codes : 10/9/6/5
                                    if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                            }
                            else if (ch_Types.Length == 3)
                            { // admincheck = false 
                                if (RequestOpened && RequestClosed && RequestRefused)
                                {
                                    // باز و تایید شده و تایید نشده
                                    //admincheck = false  & codes : 4/7/5
                                    if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestOpened && RequestClosed && RequestRemoved)
                                {
                                    // باز و تایید شده و حذف شده
                                    //admincheck = false  & codes : 10/9/6/4/7
                                    if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestOpened && RequestRefused && RequestRemoved)
                                {
                                    // باز و تایید نشده و حذف شده
                                    //admincheck = false  & codes : 10/9/6/5
                                    if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                                if (RequestClosed && RequestRefused && RequestRemoved)
                                {
                                    // تایید شده و تایید نشده و حذف شده
                                    //codes : 10/9/6/4/7/5
                                    if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                    {
                                        ReportsRes.Add(item);
                                    }
                                }
                            }
                            else if (ch_Types.Length == 4)
                            {
                                // همه درخواست ها 
                                ReportsRes.Add(item);
                            }

                            #endregion


                        }
                    }
                    else if (OU_ID != null)
                    {
                        if (Owner == "" && ApprovedValue == "")
                        {
                            // Ca-G
                            // تاریخ + وضعیت + معاونت 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner != "" && ApprovedValue == "")
                        {
                            // Ca-D
                            // تاریخ + وضعیت + معاونت + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner == "" && ApprovedValue != "")
                        {
                            // Ca-H
                            // تاریخ+ وضعیت  + معاونت + حجم
                            foreach (var item in ReportsDate)
                            {
                                if ((item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue)))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner != "" && ApprovedValue != "")
                        {
                            // Ca-E
                            // تاریخ + وضعیت + معاونت + مالک + حجم = همه شروط
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    //ReportsRes.Add(item);
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                    else if (OU_ID == null)
                    {
                        if (Owner != "" && ApprovedValue == "")
                        {
                            // Ca-K
                            // تاریخ + وضعیت + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner == "" && ApprovedValue != "")
                        {
                            // Ca-O
                            // تاریخ + وضعیت + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                        else if (Owner != "" && ApprovedValue != "")
                        {
                            // Ca-L
                            // تاریخ + وضعیت + حجم + مالک 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    #region وضعیت درخواست

                                    if (ch_Types.Length == 1)
                                    {
                                        if (RequestOpened)
                                        {
                                            //درخواست باز
                                            //admincheck = false 
                                            if (item.AdminChecked == false)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed)
                                        {
                                            //تایید شده
                                            //codes : 4/7
                                            if (item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }

                                        }
                                        if (RequestRefused)
                                        {
                                            //تایید نشده
                                            //codes : 5
                                            if (item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved)
                                        {
                                            //حذف شده
                                            //codes : 10/9/6
                                            if (item.StatusCode == 6 || item.StatusCode == 9 || item.StatusCode == 10)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 2)
                                    {
                                        if (RequestOpened && RequestClosed)
                                        {
                                            // باز و تایید شده
                                            //admincheck = false  & codes : 4/7
                                            if (item.AdminChecked == false || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused)
                                        {
                                            // باز و تایید نشده
                                            //admincheck = false  & codes : 5
                                            if (item.AdminChecked == false || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRemoved)
                                        {
                                            // باز و حذف شده
                                            //admincheck = false  & codes : 10/9/6
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused)
                                        {
                                            // تایید شده و تایید نشده
                                            //codes : 4/7/5
                                            if (item.StatusCode == 4 || item.StatusCode == 5 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRemoved)
                                        {
                                            // تایید شده و حذف شده
                                            //codes : 10/9/6/4/7
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestRemoved && RequestRefused)
                                        {
                                            // تایید نشده و حذف نشده
                                            //codes : 10/9/6/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 3)
                                    { // admincheck = false 
                                        if (RequestOpened && RequestClosed && RequestRefused)
                                        {
                                            // باز و تایید شده و تایید نشده
                                            //admincheck = false  & codes : 4/7/5
                                            if (item.AdminChecked == false || item.StatusCode == 5 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestClosed && RequestRemoved)
                                        {
                                            // باز و تایید شده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/4/7
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestOpened && RequestRefused && RequestRemoved)
                                        {
                                            // باز و تایید نشده و حذف شده
                                            //admincheck = false  & codes : 10/9/6/5
                                            if (item.AdminChecked == false || item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                        if (RequestClosed && RequestRefused && RequestRemoved)
                                        {
                                            // تایید شده و تایید نشده و حذف شده
                                            //codes : 10/9/6/4/7/5
                                            if (item.StatusCode == 10 || item.StatusCode == 9 || item.StatusCode == 6 || item.StatusCode == 4 || item.StatusCode == 7 || item.StatusCode == 5)
                                            {
                                                ReportsRes.Add(item);
                                            }
                                        }
                                    }
                                    else if (ch_Types.Length == 4)
                                    {
                                        // همه درخواست ها 
                                        ReportsRes.Add(item);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                else if (ch_Types == null && ReportsDate.Count == 0)
                {
                    if (OU_ID != null)
                    {
                        if (Owner == "" && ApprovedValue == "")
                        {
                            // Ca-A
                            // تاریخ + معاونت  
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (Owner != "" && ApprovedValue == "")
                        {
                            // Ca-B
                            // تاریخ + معاونت + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner == "" && ApprovedValue != "")
                        {
                            // Ca-F
                            // تاریخ + معاونت + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsDate.Add(item);
                                }
                            }
                        }
                        else if (Owner != "" && ApprovedValue != "")
                        {
                            // Ca-C
                            // تاریخ + معاونت + مالک + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                    }
                    else if (OU_ID == null)
                    {
                        if (Owner != "" && ApprovedValue == "")
                        {
                            // Ca-I
                            // تاریخ + مالک
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner == "" && ApprovedValue != "")
                        {
                            // Ca-M
                            // تاریخ + حجم
                            foreach (var item in ReportsDate)
                            {
                                if (item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                        else if (Owner != "" && ApprovedValue != "")
                        {
                            // Ca-J
                            // تاریخ + مالک + حجم 
                            foreach (var item in ReportsDate)
                            {
                                if (item.OwnerName.Contains(Owner) && item.ApprovedFolderValue == int.Parse(ApprovedValue))
                                {
                                    ReportsRes.Add(item);
                                }
                            }
                        }
                    }
                }


            }





        }
    }
}