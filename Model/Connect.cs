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
    
    public partial class Connect
    {
        public int IdConnect { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Tz { get; set; }
        public string MailToConnect { get; set; }
        public string PasswordC { get; set; }
        public string Phone { get; set; }
        public Nullable<short> IdStudent { get; set; }
    
        public virtual StudentsTbl StudentsTbl { get; set; }
    }
}
