//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BasicEducationDepartment.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountProfile
    {
        public int AccountProfileID { get; set; }
        public int AccountID { get; set; }
        public string APName { get; set; }
        public string APEmailAddress { get; set; }
        public string APAddress { get; set; }
        public string APJobDescription { get; set; }
        public string APContactNo { get; set; }
        public string APMothersName { get; set; }
        public string APFathersName { get; set; }
        public string APImageName { get; set; }
        public string APImagePath { get; set; }
        public string APBirthMonth { get; set; }
        public string APBirthDay { get; set; }
        public string APBirthYear { get; set; }
        public Nullable<System.DateTime> DateTimeCreated { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
