using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace BasicEducationDepartment.App_Utility
{
    public class EmailHelper
    {

        public static bool SendEmail(string to, string subject, string body, HttpPostedFile attachment)
        {
            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("wherearetheyare@gmail.com");
                //12384261238426Qq
                mm.To.Add(to);
                mm.Subject = subject;
                mm.Body = body;
                if (mm.Body != null)
                {
                    mm.IsBodyHtml = mm.Body.Contains("</");
                }
                if (attachment != null)
                {
                    mm.Attachments.Add(new System.Net.Mail.Attachment(attachment.InputStream, attachment.FileName, attachment.ContentType));
                }
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.UseDefaultCredentials = true;
                smtp.Port = 587;
                smtp.EnableSsl = true;
                //gmail acc and app password(NOT REAL PASSWORD)
                //https://stackoverflow.com/questions/18503333/the-smtp-server-requires-a-secure-connection-or-the-client-was-not-authenticated/66169647#66169647
                smtp.Credentials = new System.Net.NetworkCredential("wherearetheyare@gmail.com", "esxvaxbjikgqexro");
                smtp.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}