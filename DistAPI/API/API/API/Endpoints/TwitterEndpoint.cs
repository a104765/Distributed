using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Endpoints
{
    public class TwitterEndpoint
    {
        private const string baseEndpoint = "https://api.twitter.com/1.1/";

        

        public string getHomeTweets()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("statuses/home_timeline.json");
            return stringBuilder.ToString();

        }


        public string getFavTweets()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("favorites/list.json");
            return stringBuilder.ToString();
        }
        public string getTwitterFriends()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("friends/list.json");
            return stringBuilder.ToString();
        }
        public string getTwitterFollowers()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("followers/list.json");
            return stringBuilder.ToString();
        }

        public string makeTweet()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("statuses/update.json");
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