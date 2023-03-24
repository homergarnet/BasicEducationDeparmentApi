using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace BasicEducationDepartment.App_Utility.Data
{
    public class UsersUtils
    {
        private ErrorLogs _logger = new ErrorLogs();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();

        public string createAccount(Account dto)
        {
            Account result = new Account();

            bool userExist = db.Accounts.Any(z => z.AccountUser == dto.AccountUser);
            if (userExist)
            {
                return "User Already Exist";
            }

            Account accountObj = new Account();
            accountObj.AccountUser = dto.AccountUser;
            accountObj.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.AccountPassword.Trim());
            db.Accounts.Add(accountObj);
            db.SaveChanges();
            return "Successfully Created Account";

        }

        public string signin(Account dto, bool isAdmin = false)
        {

            //when student login
            if (dto.AccountType.Equals("student") && isAdmin == false)
            {

                var dtAccount = db.Accounts.Where(z => z.AccountUser == dto.AccountUser && z.AccountType.Equals("student") && z.AccountIsAdmin != true).FirstOrDefault();
                string password = dto.AccountPassword.Trim();
                bool verified = false;
                if (dtAccount != null)
                {
                    verified = BCrypt.Net.BCrypt.Verify(password, dtAccount.AccountPassword);
                }
                if (dtAccount != null && verified)
                {
                    return getToken(dtAccount.AccountID, "student");
                }
                else
                {
                    return "Wrong user or password";
                }

            }

            //when teacher login
            else if (dto.AccountType.Equals("teacher") && isAdmin == false)
            {

                var dtAccount = db.Accounts.Where(z => z.AccountUser == dto.AccountUser && z.AccountType.Equals("teacher") && z.AccountIsAdmin != true).FirstOrDefault();
                string password = dto.AccountPassword.Trim();
                bool verified = false;
                if (dtAccount != null)
                {
                    verified = BCrypt.Net.BCrypt.Verify(password, dtAccount.AccountPassword);
                }
                if (dtAccount != null && verified)
                {
                    return getToken(dtAccount.AccountID, "teacher");
                }
                else
                {
                    return "Wrong user or password";
                }

            }

            //when admin login
            else
            {

                var dtAccount = db.Accounts.Where(z => z.AccountUser == dto.AccountUser && z.AccountIsAdmin == true).FirstOrDefault();
                string password = dto.AccountPassword.Trim();
                bool verified = false;
                if (dtAccount != null)
                {
                    verified = BCrypt.Net.BCrypt.Verify(password, dtAccount.AccountPassword);
                }
                if (dtAccount != null && verified)
                {
                    return getToken(dtAccount.AccountID, "admin");
                }
                else
                {
                    return "Wrong user or password";
                }

            }

        }

        public string createFWToken(AccountProfileDTO dto)
        {

            //when student forgotpassword
            if (dto.AccountType.Equals("student"))
            {

                var dtAccount = db.AccountProfiles.Where(z => z.APEmailAddress == dto.APEmailAddress && z.Account.AccountType.Equals("student") && z.Account.AccountIsAdmin != true).FirstOrDefault();
                if (dtAccount != null)
                {
                    var tokenRes = getToken(dtAccount.AccountID, "student", 5);
                    dtAccount.Account.AccountToken = tokenRes;
                    db.SaveChanges();
                    return dtAccount.Account.AccountID.ToString();
                }
                else
                {
                    return "No Email Address Found";
                }

            }

            //when teacher forgotpassword
            else if (dto.AccountType.Equals("teacher"))
            {

                var dtAccount = db.AccountProfiles.Where(z => z.APEmailAddress == dto.APEmailAddress && z.Account.AccountType.Equals("teacher") && z.Account.AccountIsAdmin != true).FirstOrDefault();
                if (dtAccount != null)
                {
                    var tokenRes = getToken(dtAccount.AccountID, "teacher", 5);
                    dtAccount.Account.AccountToken = tokenRes;
                    db.SaveChanges();
                    return dtAccount.Account.AccountID.ToString();
                }
                else
                {
                    return "No Email Address Found";
                }

            }

            //when admin forgotpassword
            else if (dto.AccountType.Equals("admin"))
            {

                var dtAccount = db.AccountProfiles.Where(z => z.APEmailAddress == dto.APEmailAddress && z.Account.AccountType.Equals("admin") && z.Account.AccountIsAdmin == true).FirstOrDefault();
                if (dtAccount != null)
                {
                    var tokenRes = getToken(dtAccount.AccountID, "admin", 5);
                    dtAccount.Account.AccountToken = tokenRes;
                    db.SaveChanges();
                    return dtAccount.Account.AccountID.ToString();
                }
                else
                {
                    return "No Email Address Found";
                }

            }
            else
            {
                return "No Email Address Found";
            }

        }

        public string resetPassword(UserChangePasswordDTO dto)
        {

            //when student resetpassword
            if (dto.AccountType.Equals("student"))
            {

                var dtAccount = db.Accounts.Where(z => z.AccountID == dto.AccountId && z.AccountType.Equals("student") && z.AccountIsAdmin != true).FirstOrDefault();
                if (dtAccount != null)
                {

                    dtAccount.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword.Trim());
                    db.SaveChanges();
                    return "Successfully updated password";

                }
                else
                {
                    return "No Account Found";
                }

            }

            //when teacher resetpassword
            else if (dto.AccountType.Equals("teacher"))
            {

                var dtAccount = db.Accounts.Where(z => z.AccountID == dto.AccountId && z.AccountType.Equals("teacher") && z.AccountIsAdmin != true).FirstOrDefault();
                if (dtAccount != null)
                {

                    dtAccount.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword.Trim());
                    db.SaveChanges();
                    return "Successfully updated password";

                }
                else
                {
                    return "No Account Found";
                }

            }

            //when admin resetpassword
            else if (dto.AccountType.Equals("admin"))
            {

                var dtAccount = db.Accounts.Where(z => z.AccountID == dto.AccountId && z.AccountType.Equals("admin") && z.AccountIsAdmin == true).FirstOrDefault();
                if (dtAccount != null)
                {

                    dtAccount.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword.Trim());
                    db.SaveChanges();
                    return "Successfully updated password";

                }
                else
                {
                    return "No Account Found";
                }

            }

            else
            {
                return "No Email Address Found";
            }

        }



        public AccountProfileDTO getTokenById(int id, string accountType = "admin")
        {

            AccountProfileDTO accountRes = new AccountProfileDTO();
            var dtAccount = db.Accounts.Where(z => z.AccountID == id && z.AccountType.Equals(accountType)).FirstOrDefault();
            accountRes.AccountToken = dtAccount.AccountToken;
            return accountRes;

        }

        public string UpdatePassword(UserChangePasswordDTO dto, int userId)
        {

            bool userExist = db.Accounts.Any(z => z.AccountID == userId);
            string resultText = "";
            bool verified = false;

            if (userExist)
            {


                //When admin update password
                if (dto.AccountType.Equals("admin"))
                {

                    var dtAccount = db.Accounts.Where(z => z.AccountID == userId && z.AccountIsAdmin == true).FirstOrDefault();
                    verified = BCrypt.Net.BCrypt.Verify(dto.OldPassword, dtAccount.AccountPassword);

                    if (verified)
                    {
                        dtAccount.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword.Trim());
                        db.SaveChanges();
                        resultText = "Updated Admin Password";
                    }
                    else
                    {
                        resultText = "Wrong Admin Password";
                    }

                }
                //When student update password
                else if (dto.AccountType.Equals("student"))
                {

                    var dtAccount = db.Accounts.Where(z => z.AccountID == userId && z.AccountIsAdmin == false).FirstOrDefault();
                    verified = BCrypt.Net.BCrypt.Verify(dto.OldPassword, dtAccount.AccountPassword);

                    if (verified)
                    {
                        dtAccount.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword.Trim());
                        db.SaveChanges();
                        resultText = "Updated Student Password";
                    }
                    else
                    {
                        resultText = "Wrong Student Password";
                    }

                }

                //When teacher update password
                else if (dto.AccountType.Equals("teacher"))
                {

                    var dtAccount = db.Accounts.Where(z => z.AccountID == userId && z.AccountIsAdmin == false).FirstOrDefault();
                    verified = BCrypt.Net.BCrypt.Verify(dto.OldPassword, dtAccount.AccountPassword);

                    if (verified)
                    {
                        dtAccount.AccountPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword.Trim());
                        db.SaveChanges();
                        resultText = "Updated Teacher Password";
                    }
                    else
                    {
                        resultText = "Wrong Teacher Password";
                    }

                }
                else
                {
                    resultText = "No record has been updated";
                }

            }
            else
            {
                resultText = "Account did not exist";
            }

            return resultText;

        }

        public string getToken(int userid, string usertype, int expireMinutes = 300)
        {
            // Create payload
            var _payloads = new Dictionary<string, string>();
            _payloads.Add("api_key", "bed");
            _payloads.Add("api_secret", "bedsecret");
            _payloads.Add("userid", userid.ToString());
            _payloads.Add("usertype", usertype);

            string token = JwtManager.GenerateToken(_payloads, expireMinutes);

            return token;
        }

    }
}