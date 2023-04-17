//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace FSRM.Models.ViewModels
{
    public class AllAccessViewModel
    {
        public int AccessID { get; set; }


        public int PersonID { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string PersonFName { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string PersonLName { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string PersonNO { get; set; }



        public string PersonAdded { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string FolderAddress { get; set; }

        public int FolderID { get; set; }

        public bool AccessRead { get; set; }
        public bool AccessWrite { get; set; }
        public bool AccessModify { get; set; }


        public string AdminChecked { get; set; }

        public int HDate { get; set; }
        public DateTime MDate { get; set; }




        #region For Editor Templates

        public string UserOU { get; set; }
        //public OUsViewModel UserOU { get; set; }


        #endregion

    }
}