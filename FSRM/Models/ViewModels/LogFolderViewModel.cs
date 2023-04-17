//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FSRM.Models.ViewModels
{
    public class LogFolderViewModel
    {
        public int FolderID { get; set; }

        public int FolderLogID { get; set; }

        public string FolderStatusDesc { get; set; }

        public string PersonName { get; set; }

        public string LogHDate { get; set; }

        public DateTime? LogMDate { get; set; }


        public string Changes { get; set; }
        public string FolderAddress { get; set; }
        public int FolderSpace { get; set; }
        public string SugFolderAddress { get; set; }
        public string SugFolderName { get; set; }
        public int SugFolderSpace { get; set; }
        public int FolderStatusCode { get; set; }
    }
}