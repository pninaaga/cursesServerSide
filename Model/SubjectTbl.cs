//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Serverr.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubjectTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubjectTbl()
        {
            this.CoursesTbl = new HashSet<CoursesTbl>();
            this.SubjectOfTeacherTbl = new HashSet<SubjectOfTeacherTbl>();
            this.SubjectTbl1 = new HashSet<SubjectTbl>();
        }
    
        public short IdSubject { get; set; }
        public string SubjectName { get; set; }
        public Nullable<short> IdFatherSubject { get; set; }
        public string Picture { get; set; }
        public string CourseExplainText { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoursesTbl> CoursesTbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubjectOfTeacherTbl> SubjectOfTeacherTbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubjectTbl> SubjectTbl1 { get; set; }
        public virtual SubjectTbl SubjectTbl2 { get; set; }
    }
}
