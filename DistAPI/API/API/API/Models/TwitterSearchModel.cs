using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class TwitterSearchModel
    {
        public List<Search> statuses { get; set; }

        public class Search
        {
            public string text { get; set; }
            public string created_at { get; set; }
        }
    }
}