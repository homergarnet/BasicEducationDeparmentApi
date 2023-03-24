using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.App_Utility.Data;
using BasicEducationDepartment.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BasicEducationDepartment.Controllers
{

    [RoutePrefix("api")]
    public class ResetPasswordController : ApiController
    {

        private ErrorLogs _logger = new ErrorLogs();
        private UsersUtils _userUtils = new UsersUtils();

        [Route("reset-password")]
        [HttpPost]
        [AllowAnonymous]

        public IHttpActionResult ResetPassword([FromBody] UserChangePasswordDTO dto)
        {

            try
            {

                var res = _userUtils.resetPassword(dto);
                return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);

            }

        }

    }
}