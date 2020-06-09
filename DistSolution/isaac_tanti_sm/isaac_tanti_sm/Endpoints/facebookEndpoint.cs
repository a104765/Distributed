using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace isaac_tanti_sm.Endpoints
{
    class facebookEndpoint : Endpoint
    {
        public string accesstoken;
        public facebookEndpoint(string accesstoken) : base(
            "https://graph.facebook.com/v7.0/me")
        {
            this.accesstoken = accesstoken;
        }

        public string getByNameEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("?fields=");
            stringBuilder.Append("name&access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }

        public string getByFeedEndpoint()
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append("/feed?");
            stringBuilder.Append("access_token=");
            stringBuilder.Append(accesstoken);
            return stringBuilder.ToString();
        }
    }
}