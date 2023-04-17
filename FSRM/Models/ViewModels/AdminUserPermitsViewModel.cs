//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FSRM.Models.ViewModels
{
    public class AdminUserPermitsViewModel
    {
        public int AccessID { get; set; }


        public int PersonID { get; set; }
        public string PersonFName { get; set; }
        public string PersonLName { get; set; }
        [Editable(false)]
        public string PersonFullName { get; set; }
        [Editable(false)]
        public string PersonNO { get; set; }
        [Editable(false)]
        public string PersonOUName { get; set; }
        [Editable(false)]
        public string OwnerName { get; set; }
        [Editable(false)]
        public string FolderAddress { get; set; }
        public int FolderID { get; set; }

        

        public bool AccessRead { get; set; }
        public bool AccessWrite { get; set; }
        public bool AccessModify { get; set; }
        
        [Editable(false)]
        public string AccessInserHDate { get; set; }
        public DateTime AccessInserMDate { get; set; }


        //وضعیت درخواست
        [Editable(false)]
        public string AccessStatusDesc { get; set; }
        [Editable(false)]
        public int AccessStatusCode { get; set; }

        [Editable(false)]
        public string AdminCheckHDate { get; set; }
        [Editable(false)]
        public DateTime AdminCheckMDate { get; set; }


        //نام ادمین که تغییرات را انجام داده
        [Editable(false)]
        public string LastPersonModified { get; set; }

        public bool AdminChecked { get; set; }

        public bool AccDel { get; set; }


        public int ReportAdminDate { get; set; }

        public int PersonOUID { get; set; }
    }
}