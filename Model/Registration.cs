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
    
    public partial class Registration
    {
        public short IdRegistration { get; set; }
        public short IdCourse { get; set; }
        public short IdStudent { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
    
        public virtual CoursesTbl CoursesTbl { get; set; }
        public virtual StudentsTbl StudentsTbl { get; set; }
    }
}
