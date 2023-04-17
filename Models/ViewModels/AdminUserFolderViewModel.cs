//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FSRM.Models.ViewModels
{
    public class AdminUserFolderViewModel
    {

        #region Owner
        [Editable(false)]
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerOUName { get; set; }
        public int OwnerOUID { get; set; }

        #endregion

        #region Folder
        public int FolderID { get; set; }
        [Editable(false)]
        public string SugFolderName { get; set; }
        [Editable(false)]
        public string SugFolderAddress { get; set; }


        [UIHint("Integer")]
        [Editable(false)]
        public Int32 SugFolderValue { get; set; }


        public string FolderAddress { get; set; }

        [UIHint("Integer")]
        public int ApprovedFolderValue { get; set; }

        [Editable(false)]
        public string ReqHDate { get; set; }
        public DateTime ReqMDate { get; set; }

        #endregion


        [Editable(false)]
        public string RequestStatus { get; set; }

        public int StatusCode { get; set; }

        public int FolderLogID { get; set; }

        [Editable(false)]
        public bool AdminChecked { get; set; }
        [Editable(false)]
        public string AdminCheckedName { get; set; }

        [Editable(false)]
        public string AdminCheckHDate { get; set; }
        public DateTime AdminCheckMDate { get; set; }

        public bool AccDel { get; set; }



        // for report
        public int ReportAdminDate { get; set; }
    }
}