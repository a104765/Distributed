using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Endpoints
{
    class facebookEndpoint : Endpoint
    {
        public string accesstoken;
        public facebookEndpoint(string accesstoken) : base(
            "https://graph.facebook.com/")
        {
            this.accesstoken = accesstoken;
        }

        public string getByNameEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("v7.0/me?fields=");
            stringBuilder.Append("name,email,birthday&access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }

        public string getByFeedEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("v7.0/me/feed?");
            stringBuilder.Append("access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }

        public string getByLikeEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("v7.0/me/likes?");
            stringBuilder.Append("access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }

        public string getAccountEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("v7.0/me/accounts?");
            stringBuilder.Append("access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }

        public string getPostsEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("104390687968731/posts?");
            stringBuilder.Append("access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }

        public string postComment(string id = "", string message = "", string access_token = "")
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append($"{id}/");
            stringBuilder.Append("comments?");
            stringBuilder.Append($"message={message}&");
            stringBuilder.Append($"access_token={access_token}");
            return stringBuilder.ToString();
        }

    }
}