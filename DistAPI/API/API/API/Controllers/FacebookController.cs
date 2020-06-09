using API.Client;
using API.Endpoints;
using API.JSONParser;
using API.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/facebook")]
    public class FacebookController : ApiController
    {
        protected RestClient client;
        private facebookEndpoint facebookEndpoint;

        public FacebookController()
        {
            this.client = new RestClient();
        }

        [Route("getProfile")]
        public FacebookProfileModel getUserReading(string accesstoken)
        {
            this.facebookEndpoint = new facebookEndpoint(accesstoken);

            client.endpoint = facebookEndpoint.getByNameEndpoint();
            string response = client.makeRequest();

            JsonParser<FacebookProfileModel> JsonParser = new JsonParser<FacebookProfileModel>();

            FacebookProfileModel deserialisedfacebookmodel = new FacebookProfileModel();
            deserialisedfacebookmodel = JsonParser.parseJSON(response, JSONParser.Version.NETCore2);

            FacebookProfileModel profile = deserialisedfacebookmodel;

            return profile;
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

            foreach (FacebookFeedModel.FeedData feed in deserialisedfacebookmodel.data)
            {
                feeddata.Add(feed);
            }

            return feeddata;
        }
        [Route("getToken")]
        public List<FacebookTokenModel> getToken(string accesstoken)
        {
            List<FacebookTokenModel> fbPost = new List<FacebookTokenModel>();
            this.facebookEndpoint = new facebookEndpoint(accesstoken);

            client.endpoint = facebookEndpoint.getAccountEndpoint();
            string response = client.makeRequest();

            JsonParser<FacebookTokenModel> JsonParser = new JsonParser<FacebookTokenModel>();

            FacebookTokenModel deserialisedfacebookmodel = new FacebookTokenModel();
            deserialisedfacebookmodel = JsonParser.parseJSON(response, JSONParser.Version.NETCore2);

            fbPost.Add(deserialisedfacebookmodel);

            return fbPost;

        }

        [Route("getPagePosts")]
        public string getPagePosts(string accesstoken)
        {
            this.facebookEndpoint = new facebookEndpoint(accesstoken);
            client.endpoint = facebookEndpoint.getPostsEndpoint();
            string response = client.makeRequest();
            
            return response;
        }
        [Route("getUserLikes")]
        public List<FacebookLikesModel.LikeData> getUserLikes(string accesstoken)
        {
            this.facebookEndpoint = new facebookEndpoint(accesstoken);
            client.endpoint = facebookEndpoint.getByLikeEndpoint();
            string response = client.makeRequest();
            JsonParser<FacebookLikesModel> JsonParser = new JsonParser<FacebookLikesModel>();
            FacebookLikesModel deserialisedfacebookmodel = new FacebookLikesModel();
            deserialisedfacebookmodel = JsonParser.parseJSON(response, JSONParser.Version.NETCore2);
            List<FacebookLikesModel.LikeData> likedata = new List<FacebookLikesModel.LikeData>();
            foreach (FacebookLikesModel.LikeData likes in deserialisedfacebookmodel.data)
            {
                likedata.Add(likes);
            }
            return likedata;
        }

        [HttpPost]
        [Route("PostComment")]
        public IHttpActionResult postComment(string id, string msg, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            client.endpoint = facebookEndpoint.postComment(id, msg, token);
            string response = client.makeRequest();

            return Ok();
        }

        public void Pref(ProfilePreference fbPref)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(x => x.UserName == fbPref.Username);
            var getFromDb = db.ProfilePrefereces.FirstOrDefault(x => x.Owner.Id == user.Id);
            fbPref.Owner = user;
            if (getFromDb == null)
            {
                
                db.ProfilePrefereces.Add(fbPref);
            }
            else
            {
                db.ProfilePrefereces.Remove(getFromDb);
                db.ProfilePrefereces.Add(fbPref);
            }

            db.SaveChanges();
        }


        [Route("SetPref")]
        public IHttpActionResult PostPref([FromBody] ProfilePreference pref)
        {
            Pref(pref);

            return Ok();
        }
    }
}