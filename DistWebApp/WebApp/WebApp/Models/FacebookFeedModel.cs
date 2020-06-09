using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class FacebookFeedModel
    {
        public List<FeedData> data { get; set; }

        public class FeedData
        {
            public string message { get; set; }
            public string created_time { get; set; }
            public string id { get; set; }
        }
    }
}