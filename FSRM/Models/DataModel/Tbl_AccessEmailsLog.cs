//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FSRM.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_AccessEmailsLog
    {
        public int fld_EmailID { get; set; }
        public int fld_EmailsStatus { get; set; }
        public int fld_FK_AccessID { get; set; }
        public string fld_EmailAddress { get; set; }
        public string fld_EmailBody { get; set; }
        public string fld_EmailSentHDate { get; set; }
        public string fld_EmailSentTime { get; set; }
        public System.DateTime fld_EmailSentMDateTime { get; set; }
    }
}
