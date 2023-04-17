//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSRM.Models.ViewModels
{
    public class UserPermitViewModel
    {

        //[ScaffoldColumn(false)]
        public int AccessID { get; set; }

        public int PersonID { get; set; }


        [Required(ErrorMessage = "اجباری است")]
        public string PersonFName { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string PersonLName { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string PersonNO { get; set; }


        public string OwnerName { get; set; }


        //[Required(ErrorMessage = "اجباری است")]
        public string FolderAddress { get; set; }

        public int FolderID { get; set; }

        //public string AccessType { get; set; }
        [Editable(false)]
        public string HDate { get; set; }
        public DateTime MDate { get; set; }



        public bool AccessRead { get; set; }
        public bool AccessWrite { get; set; }
        public bool AccessModify { get; set; }



        #region For Editor Templates

        [UIHint("OUsEditor")]
        public OUsViewModel UserOU { get; set; }

        #endregion


        //وضعیت درخواست
        [Editable(false)]
        public string AccessStatus { get; set; }


        //اگر توسط ادمین بررسی شده نباید بتوان ادیت کرد

        public bool EnableEdit { get; set; }
        public bool EnableDel { get; set; }

    }
}