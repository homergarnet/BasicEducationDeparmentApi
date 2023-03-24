using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.App_Utility
{
    public class ErrorLogs
    {
        public void logs(int ex)
        {
            string basePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM"));
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM") + @"/ERROR_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date :" + DateTime.Now.ToString() + Environment.NewLine +
                    "Message :" + ex + Environment.NewLine +
                    "StackTrace :" + ex + "");
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
        public void createLogs(Exception ex)
        {
            string basePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM"));
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM") + @"/ERROR_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date :" + DateTime.Now.ToString() + Environment.NewLine +
                    "Message :" + ex.Message + Environment.NewLine +
                    "StackTrace :" + ex.StackTrace + "");
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
        public void createLogs(string ex)
        {
            string basePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM"));
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM") + @"/ERROR_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date :" + DateTime.Now.ToString() + Environment.NewLine +
                    "Message :" + ex + Environment.NewLine + "");
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public void createLogs(Exception ex, string body)
        {
            string basePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM"));
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Logs/Error/" + DateTime.Now.ToString("yyyyMM") + @"/ERROR_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Date :" + DateTime.Now.ToString() + Environment.NewLine +
                    "Body :" + body + Environment.NewLine +
                    "Message :" + ex.Message + Environment.NewLine +
                    "StackTrace :" + ex.StackTrace + "");
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}