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
    
    public partial class Tbl_Access
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Access()
        {
            this.Tbl_AccessLog = new HashSet<Tbl_AccessLog>();
        }
    
        public int fld_AccessID { get; set; }
        public Nullable<int> fld_FK_PersonID { get; set; }
        public Nullable<int> fld_FK_FolderID { get; set; }
        public Nullable<bool> fld_AccessRead { get; set; }
        public Nullable<bool> fld_AccessWrite { get; set; }
        public Nullable<bool> fld_AccessModify { get; set; }
        public string fld_LastPersonModified { get; set; }
        public Nullable<bool> fld_AdminChecked { get; set; }
        public Nullable<bool> fld_AccessShow { get; set; }
    
        public virtual Tbl_Folders Tbl_Folders { get; set; }
        public virtual Tbl_Personnel Tbl_Personnel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_AccessLog> Tbl_AccessLog { get; set; }
    }
}