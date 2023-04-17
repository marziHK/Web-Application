using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSRM.Models.ViewModels
{
    public class PermitsTypeChartViewModel
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public string Color { get; set; } // optional
        public bool Explode { get; set; } // optional
    }
}