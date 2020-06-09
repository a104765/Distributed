using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class FacebookPagePostModel
    {
        public List<Data> data { get; set; }

        public class Data
        {
            public string Created_time { get; set; }
            public string Message { get; set; }
            public string Id { get; set; }
        }
    }
}