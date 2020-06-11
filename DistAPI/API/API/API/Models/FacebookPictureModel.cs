using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FacebookPictureModel
    {
        public List<Picture> data { get; set; }

        public class Picture
        {
            public string name { get; set; }
            public string picture { get; set; }
            public string id { get; set; }
        }
    }
}