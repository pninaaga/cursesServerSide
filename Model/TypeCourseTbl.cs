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
    
    public partial class TypeCourseTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TypeCourseTbl()
        {
            this.CoursesTbl = new HashSet<CoursesTbl>();
        }
    
        public short IdTypeCourse { get; set; }
        public short TypeCourse { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoursesTbl> CoursesTbl { get; set; }
    }
}
