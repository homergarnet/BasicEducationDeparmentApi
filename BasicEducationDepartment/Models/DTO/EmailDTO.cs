using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.Models.DTO
{
    public class EmailDTO
    {
        [Required]
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public HttpPostedFile File { get; set; }
    }
}