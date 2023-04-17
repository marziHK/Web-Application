//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSRM.Models.ViewModels
{
    public class UserFolderViewModel
    {


        #region Owner
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerOUName { get; set; }
        public int OwnerOUID { get; set; }

        #endregion

        #region Folder
        public int FolderID { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string SugFolderName { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string SugFolderAddress { get; set; }

        [Required(ErrorMessage = "اجباری است")]

        [UIHint("Integer")]
        public Int32 SugFolderValue { get; set; }


        [Editable(false)]
        public string FolderAddress { get; set; }

        [Editable(false)]
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
        public string AdminCheckHDate { get; set; }

        //اگر توسط ادمین بررسی شده نباید بتوان ادیت کرد
        public bool EnableEdit { get; set; }
        public bool EnableDel { get; set; }


    }
}