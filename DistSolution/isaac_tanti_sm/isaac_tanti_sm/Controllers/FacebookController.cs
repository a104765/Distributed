using isaac_tanti_sm.Client;
using isaac_tanti_sm.Endpoints;
using isaac_tanti_sm.JSONParser;
using isaac_tanti_sm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace isaac_tanti_sm.Controllers
{
    [RoutePrefix("api/facebook")]
    public class FacebookController : ApiController
    {
        /*public IEnumerable<ProfilePreference> Get()
        {
            using(distributedAssignmentEntities entities = new distributedAssignmentEntities())
            {
                return entities.ProfilePreferences.ToList();
            }
        }*/

        protected RestClient client;
        private facebookEndpoint facebookEndpoint;

        public FacebookController()
        {
            this.client = new RestClient();
        }

        [Route("getFullName")]
        public string getUserReading(string accesstoken)
        {
            this.facebookEndpoint = new facebookEndpoint(accesstoken);

            client.endpoint = facebookEndpoint.getByNameEndpoint();
            string response = client.makeRequest();

            JsonParser<FacebookModel> JsonParser = new JsonParser<FacebookModel>();

            FacebookModel deserialisedfacebookmodel = new FacebookModel();
            deserialisedfacebookmodel = JsonParser.parseJSON(response, JSONParser.Version.NETCore2);

            string name = deserialisedfacebookmodel.name;

            return name;
        }

        [Route("getFeed")]
        public List<FacebookFeedModel.FeedData> getUserFeed(string accesstoken)
        {
            this.facebookEndpoint = new facebookEndpoint(accesstoken);

            client.endpoint = facebookEndpoint.getByFeedEndpoint();
            string response = client.makeRequest();

            JsonParser<FacebookFeedModel> JsonParser = new JsonParser<FacebookFeedModel>();

            FacebookFeedModel deserialisedfacebookmodel = new FacebookFeedModel();
            deserialisedfacebookmodel = JsonParser.parseJSON(response, JSONParser.Version.NETCore2);

            List<FacebookFeedModel.FeedData> feeddata = new List<FacebookFeedModel.FeedData>();

            foreach(FacebookFeedModel.FeedData feed in deserialisedfacebookmodel.data)
            {
                feeddata.Add(feed);
            }

            return feeddata;
        }
    }
}
