using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

    }
    public class BaseResponse <T>
    {
        public bool Success { get; set; }

        public string ErrorMessage{get; set;}

        public List<T> List { get; set; }

    }
}
