

using BasicEducationDepartment.App_Utility;
using BasicEducationDepartment.Authorization;
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
    [AdminAuthorizations]
    [RoutePrefix("api")]
    public class EmailController : ApiController
    {
        private ErrorLogs _logger = new ErrorLogs();

        //[Route("send-email-with-attachment")]
        //[HttpPost]
        //public IHttpActionResult SendEmail(EmailDTO dto)
        //{

        //    try
        //    {
        //        var to = HttpContext.Current.Request.Params["To"];
        //        var subject = HttpContext.Current.Request.Params["Subject"];
        //        var body = HttpContext.Current.Request.Params["Body"];
        //        var file = HttpContext.Current.Request.Params["File"];

        //        //var res = EmailHelper.SendEmail(to: dto.To, dto.Subject, body: dto.Body, dto.File);
        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.createLogs(ex);
        //        return InternalServerError(ex);

        //    }

        //}

        [Route("send-email-with-attachment")]
        [HttpPost]

        public async Task<IHttpActionResult> SendEmail()
        {

            try
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

                var isEmailReturn = false;

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileupload = HttpContext.Current.Request.Files["File"];
                    var to = model["To"];
                    var subject = model["Subject"];
                    var body = model["Body"];
                    if (fileupload != null)
                    {

                        isEmailReturn = EmailHelper.SendEmail(to: to, subject, body: body, fileupload);
                        

                    }

                }

                return Ok(isEmailReturn);

            }
            catch (Exception ex)
            {

                _logger.createLogs(ex);
                return InternalServerError(ex);

            }

        }

    }
}