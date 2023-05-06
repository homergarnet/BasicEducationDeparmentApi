using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.App_Utility.Data;
using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        private readonly string _api = ConfigurationManager.AppSettings["api"];
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

        //Working but not needed for now
        //[Route("login")]
        //[HttpPost]
        //[AllowAnonymous]

        //public IHttpActionResult Login([FromBody] Account dto)
        //{

        //    try
        //    {

        //        var user = _userUtils.signin(dto);
        //        if (user == "Wrong user or password")
        //        {
        //            return BadRequest(user);
        //        }
        //        //return the jwt token
        //        else
        //        {
        //            return Ok(user);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
        //        return InternalServerError(ex);

        //    }

        //}

        [HttpPost]
        [Route("login")]
        public async Task<dynamic> StudentLogin([FromBody] Account dto)
        {

            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://stdominiccollege.edu.ph/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //this code allows you to accept body -> x-www-form-urlencoded 
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("api_key", "sdca_api_2023"));
                postData.Add(new KeyValuePair<string, string>("student_number", dto.AccountUser));
                HttpContent content = new FormUrlEncodedContent(postData);

                //this code allows you to accept body -> (raw, json)
                //string jsonData = "{\"name\":\"John\", \"age\":30}";
                //HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://stdominiccollege.edu.ph/SDCALMSv2/index.php/API/StudentTicketingAPI", content);
                UsersUtils userUtils = new UsersUtils();
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(data);

                    var studentNumber = Convert.ToInt32(result.Student_number);
                    var newObj = new
                    {
                        StudentDetails = result,
                        token = userUtils.getToken(studentNumber, "student"),
                        apiLink = _api
                    };
                    if (result.Student_number == dto.AccountUser && result.Student_number == dto.AccountPassword)
                    {
                        dto.AccountType = "student";
                        var user = _userUtils.createAccount(dto);
                        return newObj;
                    }
                    else
                    {
                        return BadRequest("Wrong user or password");
                    }

                } else
                {
                    return BadRequest("Not success fetch api");
                }

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return BadRequest("Exception in creating account");

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
            long accountId = accountObj.AccountID;
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