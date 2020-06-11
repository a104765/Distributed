using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class TwitterFriendModel
    {
        public List<FriendsUsers> users { get; set; }
        public class FriendsUsers
        {
            public string name { get; set; }
        }
    }
}