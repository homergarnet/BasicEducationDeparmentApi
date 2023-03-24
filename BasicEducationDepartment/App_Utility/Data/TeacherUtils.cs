using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.App_Utility.Data
{
    public class TeacherUtils
    {

        private ErrorLogs _logger = new ErrorLogs();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();

        public AccountProfileDTO GetTeacherProfileById(int user_id)
        {

            var isExistingAccount = db.Accounts.Any(z => z.AccountID == user_id && !z.AccountType.Equals("student") && z.AccountIsAdmin == false);
            var returnObj = new AccountProfileDTO();
            if (isExistingAccount)
            {
                var isExistingAccountProfile = db.AccountProfiles.Any(z => z.AccountID == user_id);
                if (isExistingAccountProfile)
                {
                    var accountProfile = db.AccountProfiles.Where(z => z.AccountID == user_id && z.Account.AccountIsAdmin == false).FirstOrDefault();

                    returnObj.AccountID = user_id;
                    returnObj.APName = accountProfile.APName;
                    returnObj.APEmailAddress = accountProfile.APEmailAddress;
                    returnObj.APAddress = accountProfile.APAddress;
                    returnObj.APJobDescription = accountProfile.APJobDescription;
                    returnObj.APContactNo = accountProfile.APContactNo;
                    returnObj.APMothersName = accountProfile.APMothersName;
                    returnObj.APFathersName = accountProfile.APFathersName;
                    returnObj.APImageName = accountProfile.APImageName;
                    returnObj.APImagePath = accountProfile.APImagePath;

                }
                else
                {
                    returnObj.AccountID = user_id;
                }

            }

            return returnObj;

        }

    }
}