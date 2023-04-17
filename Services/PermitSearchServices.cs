//بسم الله الرحمن الرحیم
using FSRM.Models;
using FSRM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FSRM.Services
{
    public class PermitSearchServices
    {

        #region Feilds

        PermitSearchData Obj_Rep = new PermitSearchData();
        //AdminUserFolderData Obj_Folder = new AdminUserFolderData();

        #endregion


        public List<AdminUserPermitsViewModel> Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string[] ch_Types, string Owner, string OU_ID, string PersonelName, string PersonelNo)
        //public void Report(string ReqSData, string ReqEData, string AppSData, string AppEData, string[] ch_Access, string Owner, string OU_ID)
        {
            var ReportReqDate = new List<AdminUserPermitsViewModel>();
            var ReportsDate = new List<AdminUserPermitsViewModel>();
            var ReportsPerms = new List<AdminUserPermitsViewModel>();
            var ReportsRes = new List<AdminUserPermitsViewModel>();

            #region Dates - بررسی تاریخ درخواست

            DateTime dt = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string yr = pc.GetYear(dt).ToString();
            string mn = pc.GetMonth(dt).ToString().Length == 1 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString();
            string dy = pc.GetDayOfMonth(dt).ToString().Length == 1 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString();
            string Today = yr + mn + dy;
            ReqSData = ReqSData == null || ReqSData == "" ? "14001101" : ReqSData.Remove(4, 1).Remove(6, 1);
            ReqEData = ReqEData == null || ReqEData == "" ? Today : ReqEData.Remove(4, 1).Remove(6, 1);

            #endregion

            #region Check Request Type List

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


            #region فیلتر داده ها بر اساس تاریخ درخواست و وضعیت درخواست ها

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



            #region فیلتر بر اساس نوع دسترسی

            if (ch_Access == null)
            {
                ReportsPerms = ReportsDate;
            }
            else if (ch_Access.Length == 1)
            {
                if (Read)
                {
                    foreach (var item in ReportsDate)
                    {
                        if (item.AccessRead && !item.AccessWrite && !item.AccessModify)
                        {
                            ReportsPerms.Add(item);
                        }
                    }
                }
                if (Write)
                {
                    foreach (var item in ReportsDate)
                    {
                        if (!item.AccessRead && item.AccessWrite && !item.AccessModify)
                        {
                            ReportsPerms.Add(item);
                        }
                    }
                }
                if (Modify)
                {
                    foreach (var item in ReportsDate)
                    {
                        if (!item.AccessRead && !item.AccessWrite && item.AccessModify)
                        {
                            ReportsPerms.Add(item);
                        }
                    }
                }
            }
            else if (ch_Access.Length == 2)
            {
                if (Read && Write)
                {
                    foreach (var item in ReportsDate)
                    {
                        if (item.AccessRead || item.AccessWrite)
                        {
                            ReportsPerms.Add(item);
                        }
                    }
                }
                if (Read && Modify)
                {
                    foreach (var item in ReportsDate)
                    {
                        if (item.AccessRead || item.AccessModify)
                        {
                            ReportsPerms.Add(item);
                        }
                    }
                }
                if (Write && Modify)
                {
                    foreach (var item in ReportsDate)
                    {
                        if (item.AccessModify || item.AccessWrite)
                        {
                            ReportsPerms.Add(item);
                        }
                    }
                }
            }
            else if (ch_Access.Length == 3)
            {
                foreach (var item in ReportsDate)
                {
                    if (item.AccessRead || item.AccessWrite || item.AccessModify)
                    {
                        ReportsPerms.Add(item);
                    }
                }
            }

            #endregion


            bool Ch = false;


            if (Owner == "" && OU_ID == "" && PersonelName == "" && PersonelNo == "")
            {
                // Ca-0
                // تاریخ ها و وضعیت درخواست  + نوع دسترسی
                Ch = true;
                foreach (var item in ReportsPerms)
                {
                    ReportsRes.Add(item);
                }

            }
            else
            {
                #region معاونت

                if (OU_ID != "" && !Ch)
                {
                    if (Owner == "" && PersonelName == "" && PersonelNo == "")
                    {
                        // Ca-A
                        // معاونت
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName == "" && PersonelNo == "")
                    {
                        // Ca-E
                        // معاونت + مالک
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-F
                        // معاونت + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-G
                        // معاونت + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-K
                        // معاونت + مالک + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-L
                        // معاونت + مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-M
                        // معاونت + نام کابری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-O
                        // همه موارد
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (OU_ID == "" && !Ch)
                {
                    if (Owner != "" && PersonelName == "" && PersonelNo == "")
                    {
                        // Ca-B
                        //  مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-C
                        // نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-D
                        //  شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-H
                        // مالک + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-I
                        // مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner == "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-J
                        //  نام کاربری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (Owner != "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-N
                        // مالک + نام کابری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }


                #endregion

                #region مالک

                if (Owner != "" && !Ch)
                {
                    if (OU_ID == "" && PersonelName == "" && PersonelNo == "")
                    {
                        // Ca-B
                        // مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName == "" && PersonelNo == "")
                    {
                        // Ca-E
                        // معاونت + مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-H
                        // مالک + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-I
                        // مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner) && item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }

                    }
                    else if (OU_ID != "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-K
                        // معاونت + مالک + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-L
                        // معاونت + مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-N
                        // مالک + نام کابری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-O
                        // همه موارد
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (Owner == "" && !Ch)
                {
                    if (OU_ID != "" && PersonelName == "" && PersonelNo == "")
                    {
                        // Ca-A
                        //  معاونت
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-C
                        // نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-D
                        //  شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName != "" && PersonelNo == "")
                    {
                        // Ca-F
                        // معاونت + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName == "" && PersonelNo != "")
                    {
                        // Ca-G
                        // معاونت + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-J
                        //  نام کاربری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName != "" && PersonelNo != "")
                    {
                        // Ca-M
                        // معاونت + نام کابری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }


                #endregion

                #region نام کاربری

                if (PersonelName != "" && !Ch)
                {
                    if (OU_ID == "" && Owner == "" && PersonelNo == "")
                    {
                        // Ca-C
                        // نام کاربری
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner == "" && PersonelNo == "")
                    {
                        // Ca-F
                        // معاونت + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner != "" && PersonelNo == "")
                    {
                        // Ca-H
                        // مالک + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner == "" && PersonelNo != "")
                    {
                        // Ca-J
                        // نام کاربری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }

                    }
                    else if (OU_ID != "" && Owner != "" && PersonelNo == "")
                    {
                        // Ca-K
                        // معاونت + مالک + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner == "" && PersonelNo != "")
                    {
                        // Ca-M
                        // معاونت + نام کاربری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner != "" && PersonelNo != "")
                    {
                        // Ca-N
                        //  نام کاربری + شماره پرسنلی + مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner != "" && PersonelNo != "")
                    {
                        // Ca-O
                        // همه موارد
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (PersonelName == "" && !Ch)
                {
                    if (OU_ID != "" && Owner == "" && PersonelNo == "")
                    {
                        // Ca-A
                        //  معاونت
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner != "" && PersonelNo == "")
                    {
                        // Ca-B
                        // مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner == "" && PersonelNo != "")
                    {
                        // Ca-D
                        //  شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner != "" && PersonelNo == "")
                    {
                        // Ca-E
                        // معاونت + مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner == "" && PersonelNo != "")
                    {
                        // Ca-G
                        // معاونت + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && Owner != "" && PersonelNo != "")
                    {
                        // Ca-I
                        //  مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && Owner != "" && PersonelNo != "")
                    {
                        // Ca-L
                        // معاونت + مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }


                #endregion

                #region شماره پرسنلی

                if (PersonelNo != "" && !Ch)
                {
                    if (OU_ID == "" && PersonelName == "" && Owner == "")
                    {
                        // Ca-D
                        // شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName == "" && Owner == "")
                    {
                        // Ca-G
                        // معاونت + پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && Owner == "")
                    {
                        // Ca-J
                        // شماره پرسنلی + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName == "" && Owner != "")
                    {
                        // Ca-I
                        // مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }

                    }
                    else if (OU_ID != "" && PersonelName != "" && Owner == "")
                    {
                        // Ca-M
                        // معاونت + شماره پرسنلی + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName == "" && Owner != "")
                    {
                        // Ca-L
                        // معاونت + مالک + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && Owner != "")
                    {
                        // Ca-N
                        // مالک + نام کابری + شماره پرسنلی
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName != "" && Owner != "")
                    {
                        // Ca-O
                        // همه موارد
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonNO == PersonelNo && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }
                else if (PersonelNo == "" && !Ch)
                {
                    if (OU_ID != "" && PersonelName == "" && Owner == "")
                    {
                        // Ca-A
                        //  معاونت
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && Owner == "")
                    {
                        // Ca-C
                        // نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonNO == PersonelNo)
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName == "" && Owner != "")
                    {
                        // Ca-B
                        //  مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName != "" && Owner == "")
                    {
                        // Ca-F
                        // معاونت + نام کاربر
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonFullName.Contains(PersonelName))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName == "" && Owner != "")
                    {
                        // Ca-E
                        // معاونت + مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID == "" && PersonelName != "" && Owner != "")
                    {
                        // Ca-H
                        //  نام کابری + مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                    else if (OU_ID != "" && PersonelName != "" && Owner != "")
                    {
                        // Ca-K
                        // معاونت + نام کابری + مالک
                        Ch = true;
                        foreach (var item in ReportsPerms)
                        {
                            if (item.PersonOUID == int.Parse(OU_ID) && item.PersonFullName.Contains(PersonelName) && item.OwnerName.Contains(Owner))
                            {
                                ReportsRes.Add(item);
                            }
                        }
                    }
                }


                #endregion


            }


            //ReportsRes.Sort(o => o.AdminChecked);




            return ReportsRes;
        }

    }
}