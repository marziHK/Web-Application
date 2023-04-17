using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSRM.Models.ViewModels
{
    public class PermitsChartViewModel
    {
        public string OuName { get; set; }

        public float OuAllAccess { get; set; }

        public bool Explode { get; set; }

        public int ReadAccess { get; set; }

        public int WriteAccess { get; set; }

        public int ModifyAccess { get; set; }
    }
}