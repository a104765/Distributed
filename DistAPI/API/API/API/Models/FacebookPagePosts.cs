using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FacebookPagePosts
    {
        public List<Post> data { get; set; }

        public class Post
        {
            public string Created_time { get; set; }
            public string Message { get; set; }
            public string Id { get; set; }
        }
    }

    
}