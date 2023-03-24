using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.App_Utility.Data;
using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BasicEducationDepartment.Controllers
{
    [RoutePrefix("api")]
    public class AuthorizationController : ApiController
    {

        private ErrorLogs _logger = new ErrorLogs();
        private UsersUtils _userUtils = new UsersUtils();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();
        // GET: Authorization
        [Route("create")]
        [HttpPost]
        public IHttpActionResult CreateAccount([FromBody] Account dto)
        {

            try
            {

                var user = _userUtils.createAccount(dto);
                if (user == "User Already Exist")
                {
                    return BadRequest(user);
                }
                //create account
                else
                {
                    return Ok(user);
                }
              
            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);

            }
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]

        public IHttpActionResult Login([FromBody] Account dto)
        {

            try
            {

                var user = _userUtils.signin(dto);
                if (user == "Wrong user or password")
                {
                    return BadRequest(user);
                }
                //return the jwt token
                else
                {
                    return Ok(user);
                }

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);

            }

        }

        [Route("teacher-login")]
        [HttpPost]
        [AllowAnonymous]

        public IHttpActionResult TeacherLogin([FromBody] Account dto)
        {

            try
            {

                var user = _userUtils.signin(dto);
                if (user == "Wrong user or password")
                {
                    return BadRequest(user);
                }
                //return the jwt token
                else
                {
                    return Ok(user);
                }

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);

            }

        }

        [Route("admin-login")]
        [HttpPost]
        [AllowAnonymous]

        public IHttpActionResult AdminLogin([FromBody] Account dto)
        {

            try
            {

                var user = _userUtils.signin(dto, true);
                if (user == "Wrong user or password")
                {
                    return BadRequest(user);
                }
                //return the jwt token
                else
                {
                    return Ok(user);
                }

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);

            }

        }

        [HttpPost]
        [Route("create-new-profile")]
        public async Task<IHttpActionResult> CreateNewProfile()
        {

            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            string foldercreate = HttpContext.Current.Server.MapPath("~/Images");
            var provider = new MultipartFormDataStreamProvider(foldercreate);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var model = result.FormData;
            if (!Directory.Exists(foldercreate))
            {
                Directory.CreateDirectory(foldercreate);
            }

            //checker if you want to set all of the formdata file is not missing or null of any data
            //if (HttpContext.Current.Request.Files.AllKeys.Any())


            var fileupload = HttpContext.Current.Request.Files["Imgpathsave"];
            var saveimages = "";

            if (fileupload != null)
            {

                saveimages = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), fileupload.FileName);
                fileupload.SaveAs(saveimages);

            }

            Account accountObj = new Account();
            string accountType = model["AccountType"];
            accountObj.AccountUser = model["Username"];
            accountObj.AccountPassword = BCrypt.Net.BCrypt.HashPassword(model["Password"].Trim());
            accountObj.AccountType = accountType;
            accountObj.DateTimeCreated = DateTime.Now;
            db.Accounts.Add(accountObj);
            int accountId = accountObj.AccountID;
            AccountProfile accountProfileObj = new AccountProfile();
            accountProfileObj.AccountID = accountId;
            accountProfileObj.APName = model["FullName"];
            accountProfileObj.APEmailAddress = model["Email"];
            if(accountType == "teacher")
            {
                accountProfileObj.APJobDescription = model["APJobDescription"];
            }
            accountProfileObj.APImageName = fileupload != null ? fileupload.FileName : accountProfileObj.APImageName;
            accountProfileObj.APImagePath = saveimages != null ? saveimages.ToString() : accountProfileObj.APImagePath;
            accountProfileObj.APBirthMonth = model["Month"];
            accountProfileObj.APBirthDay = model["Day"];
            accountProfileObj.APBirthYear = model["Year"];
            accountProfileObj.DateTimeCreated = DateTime.Now;
            db.AccountProfiles.Add(accountProfileObj);
            db.SaveChanges();
            return Ok("Created Successfully");

        }

    }
}