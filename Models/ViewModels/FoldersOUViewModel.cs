//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSRM.Models.ViewModels
{
    public class FoldersOUViewModel
    {
        public string OuName { get; set; }

        public float OuAllFolders { get; set; }

        public bool Explode { get; set; }


        public int OpenRequests { get; set; }
        
        public int ClosedRequests { get; set; }

    }
}