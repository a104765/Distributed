using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FacebookTokenModel
    {
        public List<Data> data { get; set; }
        
        public class Data {
            public string access_token { get; set; }
        }
    }
}