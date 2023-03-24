using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.App_Utility.Data;
using BasicEducationDepartment.Authorization;
using BasicEducationDepartment.Models;
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
    public class ForgotPasswordController : ApiController
    {

        private ErrorLogs _logger = new ErrorLogs();
        private StudentProfileUtils _studentProfileUtils = new StudentProfileUtils();
        private UsersUtils _userUtils = new UsersUtils();
        private readonly string _webLink = ConfigurationManager.AppSettings["webLink"];

        [Route("create-f-p-token")]
        [HttpPost]
        [AllowAnonymous]

        public IHttpActionResult CReateFWToken([FromBody] AccountProfileDTO dto)
        {

            try
            {

                var res = _userUtils.createFWToken(dto);

                var isEmailReturn = false;

                var subject = "Basic Education Department Forgot Password";
                var body = "<html><body>";

                if(dto.AccountType.Equals("student"))
                {
                    body += "Please click on the link below to forgot password:<br><br><a href='" + _webLink + "home/reset-password/" + res + "'> Click Here </a>";
                }
                else if(dto.AccountType.Equals("teacher"))
                {
                    body += "Please click on the link below to forgot password:<br><br><a href='" + _webLink + "teacher/reset-password/" + res + "'> Click Here </a>";
                }

                else if(dto.AccountType.Equals("admin"))
                {
                    body += "Please click on the link below to forgot password:<br><br><a href='" + _webLink + "home/home/reset-password/" + res + "'> Click Here </a>";
                }


                body += "</body></html>";


                if (res == "No Email Address Found")
                {
                    return BadRequest(res);
                }
                //return the jwt token
                else
                {
                    isEmailReturn = EmailHelper.SendEmail(to: dto.APEmailAddress, subject, body: body, null);
                    return Ok(res);
                }

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);

            }

        }

        [Route("get-token-by-id-and-type/{id}/{type}")]
        [HttpGet]
        public IHttpActionResult GetTokenById(int id, string type)
        {

            try
            {

                var res = _userUtils.getTokenById(id, type);
                var jwthandler = new JwtSecurityTokenHandler();
                var jwttoken = jwthandler.ReadToken(res.AccountToken);
                var expDate = jwttoken.ValidTo;
                //when jwt token already expired
                if (expDate < DateTime.UtcNow.AddMinutes(1))
                {
                    res.IsATokenExpire = true;
                }
                //when jwt token is not yet expired
                else
                {
                    res.IsATokenExpire = false;
                }
                    return Ok(res);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

        [StudentAuthorizations]
        [Route("student-fp-checker")]
        [HttpGet]
        public IHttpActionResult StudentFPChecker()
        {

            try
            {



                return Ok(true);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }


    }

}