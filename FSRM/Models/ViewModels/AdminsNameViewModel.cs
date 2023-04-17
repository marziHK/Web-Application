//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FSRM.Models.ViewModels
{
    public class AdminsNameViewModel
    {
        public int AdminID { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "اجباری است")]
        public string AdminADName { get; set; }
        public DateTime MDate { get; set; }
    }
}