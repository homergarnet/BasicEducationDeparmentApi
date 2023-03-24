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
    [TeacherAuthorizations]
    [RoutePrefix("api")]
    public class TeacherController : ApiController
    {

        private ErrorLogs _logger = new ErrorLogs();
        private StudentProfileUtils _studentProfileUtils = new StudentProfileUtils();
        private UsersUtils _userUtils = new UsersUtils();
        private TeacherUtils _teacherUtils = new TeacherUtils();
        private BasicEducationDepartmentEntities db = new BasicEducationDepartmentEntities();

        [Route("create-teacher-referral-form")]
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

        [Route("update-teacher-password")]
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

        [Route("get-teacher-by-id")]
        [HttpGet]
        public IHttpActionResult GetTeacherById()
        {

            try
            {

                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userid"));
                var res = _teacherUtils.GetTeacherProfileById(user_id);
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