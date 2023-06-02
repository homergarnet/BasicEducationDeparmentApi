using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.App_Utility.Data;
using BasicEducationDepartment.Authorization;
using BasicEducationDepartment.Models;
using BasicEducationDepartment.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BasicEducationDepartment.Controllers
{
    [AdminAuthorizations]
    [RoutePrefix("api")]
    public class AdminDashboardController : ApiController
    {

        private ErrorLogs _logger = new ErrorLogs();
        private AdminProfileUtils _adminProfileUtils = new AdminProfileUtils();
        private UsersUtils _userUtils = new UsersUtils();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();


        [HttpPost]
        [Route("add-or-update-admin-profile")]
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

            var isExistingProfile = db.AccountProfiles.Any(z => z.AccountID == accountID && z.Account.AccountIsAdmin == true);
            //update 
            if (isExistingProfile)
            {

                var accountProfiles = db.AccountProfiles.Where(z => z.AccountID == accountID && z.Account.AccountIsAdmin == true).FirstOrDefault();
                accountProfiles.AccountID = Convert.ToInt32(model["AccountID"]);
                accountProfiles.APEmailAddress = model["APEmailAddress"];
                accountProfiles.APAddress = model["APAddress"];
                accountProfiles.APJobDescription = model["APJobDescription"];
                accountProfiles.APName = model["APName"];
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
                    APEmailAddress = model["APEmailAddress"],
                    APAddress = model["APAddress"],
                    APJobDescription = model["APJobDescription"],
                    APName = model["APName"],
                    APImageName = fileupload != null ? fileupload.FileName : "",
                    APImagePath = saveimages != null ? saveimages.ToString() : "",

                });
                db.SaveChanges();
                createOrUpdateStr = "Created successfully";

            }

            return Ok(createOrUpdateStr);

        }

        [Route("get-admin-by-id")]
        [HttpGet]
        public IHttpActionResult GetAdminById()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetAdminProfileById(user_id);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-status-list")]
        [HttpPost]
        public IHttpActionResult GetStatusList(StudentReferralFormDTO dto)
        {
            try
            {

                List<StudentReferralFormDTO> result = _adminProfileUtils.GetStatusList(dto);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }

        [Route("get-student-referral-form-by-id")]
        [HttpPost]
        public IHttpActionResult GetStudentReferralFormById(StudentReferralFormDTO dto)
        {

            try
            {

                var res = _adminProfileUtils.GetStudentReferralFormById(dto.StudentReferralID);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("update-queing-status")]
        [HttpPut]
        public IHttpActionResult UpdateQueingStatus([FromBody] StudentReferralFormDTO dto)
        {

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                dto.StudentAcknowledgeById = user_id;
                var res = _adminProfileUtils.UpdateQueingStatus(dto);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-total-referral-form-count")]
        [HttpGet]
        public IHttpActionResult GetTotalReferralFormCount()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetTotalReferralCount();
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("update-admin-password")]
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

        [Route("get-analytics-for-category")]
        [HttpGet]
        public IHttpActionResult GetAnalyticsForCategory()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetAnalyticsForCategory();
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-academic-concern-graph")]
        [HttpGet]
        public IHttpActionResult GetAcademicConcernGraph()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetAcademicConcernGraph();
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-mood-behavior-graph")]
        [HttpGet]
        public IHttpActionResult GetMoodBehaviorGraph()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetMoodBehaviorGraph();
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-relationship-graph")]
        [HttpGet]
        public IHttpActionResult GetRelationshipGraph()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetRelationshipGraph();
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-home-concern-graph")]
        [HttpGet]
        public IHttpActionResult GetHomeConcernGraph()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetHomeConcernGraph();
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-referral-form-graph")]
        [HttpGet]
        public IHttpActionResult GetReferralFormGraph()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetReferralFormGraph();

                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [Route("get-most-pick-reasons")]
        [HttpGet]
        public IHttpActionResult GetMostPickReasons()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _adminProfileUtils.GetMostPickReasons();
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