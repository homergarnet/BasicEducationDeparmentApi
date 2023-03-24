using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.Models.DTO
{
    public class AdminDTO
    {
    }  
    
    public class DashboardDTO
    {

        public int TSRForm { get; set; }
        public int TSRFormPending { get; set; }
        public int TSRFormCompleted { get; set; }

    }
}