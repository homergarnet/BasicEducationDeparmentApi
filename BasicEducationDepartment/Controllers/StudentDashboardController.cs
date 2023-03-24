using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.App_Utility.Data;
using BasicEducationDepartment.Authorization;
using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace BasicEducationDepartment.Controllers
{
    [StudentAuthorizations]
    [RoutePrefix("api")]
    public class StudentDashboardController : ApiController
    {

        private ErrorLogs _logger = new ErrorLogs();
        private StudentProfileUtils _studentProfileUtils = new StudentProfileUtils();
        private UsersUtils _userUtils = new UsersUtils();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();

        [HttpPost]
        [Route("add-or-update-student-profile")]
        public async Task<IHttpActionResult> CreateOrUpdateProfile()
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

            var createOrUpdateStr = "";
            var accountID = Convert.ToInt32(model["AccountID"]);
            var fileupload = HttpContext.Current.Request.Files["Imgpathsave"];
            var saveimages = "";

            if (fileupload != null)
            {

                saveimages = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), fileupload.FileName);
                fileupload.SaveAs(saveimages);

            }
            var isExistingProfile = db.AccountProfiles.Any(z => z.AccountID == accountID && z.Account.AccountIsAdmin == false);
            //update 
            if (isExistingProfile)
            {

                var accountProfiles = db.AccountProfiles.Where(z => z.AccountID == accountID && z.Account.AccountIsAdmin == false).FirstOrDefault();
                accountProfiles.AccountID = Convert.ToInt32(model["AccountID"]);
                accountProfiles.APName = model["APName"];
                accountProfiles.APEmailAddress = model["APEmailAddress"];
                accountProfiles.APAddress = model["APAddress"];
                accountProfiles.APContactNo = model["APContactNo"];
                accountProfiles.APMothersName = model["APMothersName"];
                accountProfiles.APFathersName = model["APFathersName"];
                accountProfiles.APImageName = fileupload != null ? fileupload.FileName : accountProfiles.APImageName;
                accountProfiles.APImagePath = saveimages != null ? saveimages.ToString() : accountProfiles.APImagePath;
                db.SaveChanges();
                createOrUpdateStr = "Updated successfully";

            }
            //insert data from db
            else
            {

                db.AccountProfiles.Add(new AccountProfile
                {

                    AccountID = Convert.ToInt32(model["AccountID"]),
                    APName = model["APName"],
                    APEmailAddress = model["APEmailAddress"],
                    APAddress = model["APAddress"],
                    APContactNo = model["APContactNo"],
                    APMothersName = model["APMothersName"],
                    APFathersName = model["APFathersName"],
                    APImageName = fileupload != null ? fileupload.FileName : "",
                    APImagePath = saveimages != null ? saveimages.ToString() : "",

                });

                db.SaveChanges();
                createOrUpdateStr = "Created successfully";

            }



            return Ok(createOrUpdateStr);

        }

        [Route("get-student-by-id")]
        [HttpGet]
        public IHttpActionResult GetStudentById()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _studentProfileUtils.GetStudentProfileById(user_id);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("create-student-referral-form")]
        [HttpPost]
        public IHttpActionResult StudentReferralForm([FromBody] StudentReferralFormDTO dto)
        {

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _studentProfileUtils.CreateStudentReferralForm(dto, user_id);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("update-student-password")]
        [HttpPut]
        public IHttpActionResult UpdatePassword([FromBody] UserChangePasswordDTO dto)
        {

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));

                var res = _userUtils.UpdatePassword(dto, user_id);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }


    }
}