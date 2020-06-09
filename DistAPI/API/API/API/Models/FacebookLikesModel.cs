using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FacebookLikesModel
    {
        public List<LikeData> data { get; set; }

        public class LikeData
        {
            public string name { get; set; }
            public string created_time { get; set; }
            public string id { get; set; }
        }
    }
}