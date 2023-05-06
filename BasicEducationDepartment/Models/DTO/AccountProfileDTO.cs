using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.Models.DTO
{

    public class AccountProfileDTO
    {

        public long AccountID { get; set; }
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
        public string DateTimeCreated { get; set; }
        public string DateTimeUpdated { get; set; }
        public string AccountToken { get; set; }
        public string AccountType { get; set; }
        public bool IsATokenExpire { get; set; }

    }

}