using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class TwitterFollowerModel
    {
        public List<Follower> users { get; set; }
        public class Follower
        {
            public string name { get; set; }
        }
    }
}