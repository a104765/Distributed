using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FacebookProfileModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
    }
}