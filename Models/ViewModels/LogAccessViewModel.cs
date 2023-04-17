//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FSRM.Models.ViewModels
{
    public class LogAccessViewModel
    {
        public int AccessID { get; set; }

        public int AccessLogID { get; set; }

        public string AccessStatusDesc { get; set; }

        public string PersonName { get; set; }

        public string LogHDate { get; set; }

        public DateTime? LogMDate { get; set; }
    }
}