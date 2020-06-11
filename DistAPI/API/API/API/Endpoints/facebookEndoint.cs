using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Endpoints
{
    public class FacebookEndpoint 
    {
        public string accesstoken;

        public string baseEndpoint = "https://graph.facebook.com/";
        

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

        public string getPicturesEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("v7.0/me/photos?fields=name,picture&type=uploaded&");
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

        public string postComment(string id = "", string message = "")
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append($"{id}/");
            stringBuilder.Append("comments?");
            stringBuilder.Append($"message={message}&");
            stringBuilder.Append($"access_token={accesstoken}");
            return stringBuilder.ToString();
        }


        public Dictionary<string, string> getEndpoint()
        {
            return new Dictionary<string, string>
            {
                {"Authorization",accesstoken}
            };
        }

    }
}