//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FSRM.Models.ViewModels
{
    public class AccessSearchViewModel
    {
        public string ReqSData { get; set; }
        public string ReqEData { get; set; }

        public string AppSData { get; set; }
        public string AppEData { get; set; }


        public int AccessType { get; set; }
        public bool AccessRead { get; set; }
        public bool AccessWrite { get; set; }
        public bool AccessModify { get; set; }

        public int OUID { get; set; }
        public string OUFaName { get; set; }


        public string FolderOwner { get; set; }


        public string PersonName { get; set; }

        public string PersonalNo { get; set; }


        // درخواست های حذف شده یا موجود
        public int RequestType { get; set; }

        public bool RequestRemoved { get; set; }
        public bool RequestOpened { get; set; }
        public bool RequestClosed { get; set; }
        public bool RequestRefused { get; set; }
    }
}