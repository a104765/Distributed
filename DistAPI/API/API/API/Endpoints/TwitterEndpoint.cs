using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Endpoints
{
    class TwitterEndpoint : Endpoint
    {
        public string accesstoken;
        public string secretKey;

        public TwitterEndpoint(string accesstoken, string secretKey) : base(
            "https://api.twitter.com/1.1/"
            )
        {
            this.accesstoken = accesstoken;
            this.secretKey = secretKey;
        }

        public string getHomeTweets()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("statuses/home_timeline.json");
            return stringBuilder.ToString();


        }

        string a = "";
        public void AuthorizationSignature(string auth)
        {
            a = auth;
        }
        public Dictionary<string, string> getTwitterEndpoint()
        {
            return new Dictionary<string, string>
            {
                {"Authorization",a}
            };
        }
    }
}