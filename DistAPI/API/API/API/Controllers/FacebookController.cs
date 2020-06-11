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
    [Authorize]
    [RoutePrefix("api/facebook")]
    public class FacebookController : ApiController
    {
        protected RestClient client = new RestClient();
        private FacebookEndpoint facebookEndpoint;
        public FacebookController(): base()
        {
            this.facebookEndpoint = new FacebookEndpoint();
        }

        [Route("getProfile")]
        public FacebookProfileModel getUserReading(string accesstoken)
        {
            facebookEndpoint.accesstoken = accesstoken;
            client.endpoint = facebookEndpoint.getByNameEndpoint();
            
            string response = client.makeRequest(HttpVerb.GET,facebookEndpoint.getEndpoint());

            JSONParser<FacebookProfileModel> JSONParser = new JSONParser<FacebookProfileModel>();

            FacebookProfileModel deserialisedfacebookmodel = new FacebookProfileModel();
            deserialisedfacebookmodel = JSONParser.parseJson(response);

            FacebookProfileModel profile = deserialisedfacebookmodel;

            return profile;
        }


        [Route("getFeed")]
        public List<FacebookFeedModel.FeedData> getUserFeed(string accesstoken)
        {
            facebookEndpoint.accesstoken = accesstoken;
            client.endpoint = facebookEndpoint.getByFeedEndpoint();
            string response = client.makeRequest(HttpVerb.GET, facebookEndpoint.getEndpoint());

            JSONParser<FacebookFeedModel> JsonParser = new JSONParser<FacebookFeedModel>();

            FacebookFeedModel deserialisedfacebookmodel = new FacebookFeedModel();
            deserialisedfacebookmodel = JsonParser.parseJson(response);

            List<FacebookFeedModel.FeedData> feeddata = new List<FacebookFeedModel.FeedData>();

            foreach (FacebookFeedModel.FeedData feed in deserialisedfacebookmodel.data)
            {
                feeddata.Add(feed);
            }

            return feeddata;
        }

        [Route("getPictures")]
        public List<FacebookPictureModel.Picture> getPictures(string accesstoken)
        {
            facebookEndpoint.accesstoken = accesstoken;
            client.endpoint = facebookEndpoint.getPicturesEndpoint();
            string response = client.makeRequest(HttpVerb.GET, facebookEndpoint.getEndpoint());

            JSONParser<FacebookPictureModel> JsonParser = new JSONParser<FacebookPictureModel>();

            FacebookPictureModel deserialisedfacebookmodel = new FacebookPictureModel();
            deserialisedfacebookmodel = JsonParser.parseJson(response);

            List<FacebookPictureModel.Picture> picData = new List<FacebookPictureModel.Picture>();

            foreach (FacebookPictureModel.Picture pic in deserialisedfacebookmodel.data)
            {
                picData.Add(pic);
            }

            return picData;
        }



        [Route("getToken")]
        public FacebookTokenModel getToken(string accesstoken)
        {
            FacebookTokenModel fbToken = new FacebookTokenModel();

            facebookEndpoint.accesstoken = accesstoken;

            client.endpoint = facebookEndpoint.getAccountEndpoint();
            string response = client.makeRequest(HttpVerb.GET, facebookEndpoint.getEndpoint());

            JSONParser<FacebookTokenModel> JSONParser = new JSONParser<FacebookTokenModel>();

            FacebookTokenModel deserialisedfacebookmodel = new FacebookTokenModel();
            deserialisedfacebookmodel = JSONParser.parseJson(response);

            fbToken = deserialisedfacebookmodel;

            return fbToken;

        }

        [Route("getPagePosts")]
        public string getPagePosts(string accesstoken)
        {
            facebookEndpoint.accesstoken = accesstoken;
            client.endpoint = facebookEndpoint.getPostsEndpoint();
            string response = client.makeRequest(HttpVerb.GET, facebookEndpoint.getEndpoint());

            return response;
        }
        [Route("getUserLikes")]
        public List<FacebookLikesModel.LikeData> getUserLikes(string accesstoken)
        {
            facebookEndpoint.accesstoken = accesstoken;
            client.endpoint = facebookEndpoint.getByLikeEndpoint();
            string response = client.makeRequest(HttpVerb.GET, facebookEndpoint.getEndpoint());
            JSONParser<FacebookLikesModel> JsonParser = new JSONParser<FacebookLikesModel>();
            FacebookLikesModel deserialisedfacebookmodel = new FacebookLikesModel();
            deserialisedfacebookmodel = JsonParser.parseJson(response);
            List<FacebookLikesModel.LikeData> likedata = new List<FacebookLikesModel.LikeData>();
            foreach (FacebookLikesModel.LikeData likes in deserialisedfacebookmodel.data)
            {
                likedata.Add(likes);
            }
            return likedata;
        }

        [HttpPost]
        [Route("PostComment")]
        public IHttpActionResult postComment(string id, string message, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            facebookEndpoint.accesstoken = token;
            client.endpoint = facebookEndpoint.postComment(id, message);
            string response = client.makeRequest(HttpVerb.POST, facebookEndpoint.getEndpoint());

            return Ok();
        }

        public void SetPref(ProfilePreference fbPref)
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


        [Route("GetPref")]
        public ProfilePreference GetPref(string username)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProfilePreference pref = db.ProfilePrefereces.FirstOrDefault(x => x.Username == username);

            return pref;
        }


        [Route("SetPref")]
        public IHttpActionResult PostPref([FromBody] ProfilePreference pref)
        {
            SetPref(pref);

            return Ok();
        }
    }
}