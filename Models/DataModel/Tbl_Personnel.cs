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
    
    public partial class Tbl_Personnel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Personnel()
        {
            this.Tbl_Access = new HashSet<Tbl_Access>();
        }
    
        public int fld_PersonID { get; set; }
        public string fld_PersonFName { get; set; }
        public string fld_PersonLName { get; set; }
        public string fld_PersonNO { get; set; }
        public Nullable<int> fld_FK_DepartmentID { get; set; }
    
        public virtual Tbl_Department Tbl_Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Access> Tbl_Access { get; set; }
    }
}
