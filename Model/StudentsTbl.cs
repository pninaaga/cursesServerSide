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
    
    public partial class StudentsTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentsTbl()
        {
            this.Connect = new HashSet<Connect>();
            this.Registration = new HashSet<Registration>();
            this.StudentWaitToRegistration = new HashSet<StudentWaitToRegistration>();
        }
    
        public short IdStudent { get; set; }
        public string FirstNameS { get; set; }
        public string LastNameS { get; set; }
        public string AddressS { get; set; }
        public string PhoneS { get; set; }
        public string EmailS { get; set; }
        public string TzS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Connect> Connect { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Registration> Registration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentWaitToRegistration> StudentWaitToRegistration { get; set; }
    }
}
