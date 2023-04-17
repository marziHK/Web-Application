//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSRM.Models.ViewModels
{
    public class FolderSearchViewModel
    {
        // تاریخ درخواست
        public string ReqSData { get; set; }
        public string ReqEData { get; set; }


        // تاریخ بررسی مدیر
        public string AppSData { get; set; }
        public string AppEData { get; set; }


        //نوع درخواست
        public int RequestType { get; set; }
        public bool RequestRemoved { get; set; }
        public bool RequestOpened { get; set; }
        public bool RequestClosed { get; set; }
        public bool RequestRefused { get; set; }


        // معاونت
        public int OUID { get; set; }
        public string OUFaName { get; set; }


        //حجم تصویب شده
        [UIHint("Integer")]
        public Int32 ApprovedFolderValue { get; set; }

        //درخواست کننده - مالک
        public string FolderOwner { get; set; }

        //مسیر پوشه
        public string FolderAddress { get; set; }



        public string AdminName { get; set; }
        public int AdminID { get; set; }

    }
}