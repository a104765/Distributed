using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Runtime.CompilerServices;
using API.Endpoints;
using API.Client;
using API.JSONParser;
using API.Models;
using static API.Models.TwitterFriendModel;
using static API.Models.TwitterFollowerModel;

namespace API.Controllers
{

    public enum Method
    {
        POST,
        GET
    }


    [Authorize]
    [RoutePrefix("api/twitter")]

    public class TwitterController : ApiController
    {

        protected RestClient restClient = new RestClient();

        protected TwitterEndpoint twitterEndpoint;

        public TwitterController() : base()
        {
            this.twitterEndpoint = new TwitterEndpoint();
        }

        public const string OauthVersion = "1.0";
        public const string OauthSignatureMethod = "HMAC-SHA1";
        public const string ConsumerApiKey = "8lFqMx2R3kV0GJbSdYWDFWg8L";
        public const string ConsumerApiSecretKey = "He6acN3MdDNMFW5CglpwDMPBPPTP03XAydpCZpUDXmXx8NRtY6";
        public string AccessToken = "";
        public string AccessTokenSecret = "";

        private string CreateOauthNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private static string CreateOAuthTimestamp()
        {

            var nowUtc = DateTime.UtcNow;
            var timeSpan = nowUtc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            return timestamp;
        }

        private string CreateHeader(string resourceUrl, Method method, string tweet = null, string search = null)
        {
            var oauthNonce = CreateOauthNonce();
            // Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var oauthTimestamp = CreateOAuthTimestamp();
            var oauthSignature = CreateOauthSignature(resourceUrl, method, oauthNonce, oauthTimestamp, tweet, search);
            StringBuilder requestParameters = new StringBuilder();
            requestParameters.Append($"OAuth oauth_consumer_key={ConsumerApiKey},");
            requestParameters.Append($"oauth_nonce={oauthNonce},");
            requestParameters.Append($"oauth_signature_method={OauthSignatureMethod},");
            requestParameters.Append($"oauth_timestamp={oauthTimestamp},");
            requestParameters.Append($"oauth_token={AccessToken},");
            requestParameters.Append($"oauth_version={OauthVersion},");
            requestParameters.Append($"oauth_signature={oauthSignature}");
            var sigBaseString = requestParameters.ToString();

            return sigBaseString;

           
        }

        public string CreateOauthSignature(string resourceUrl, Method method, string oauthNonce, string oauthTimestamp, string tweet = null, string search = null) 
        {
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                        "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";


            if (tweet != null)
            {
                baseFormat += "&status=" + Uri.EscapeDataString(tweet);
            }
            if (search != null)
            {
                baseFormat += "&q=" + search;
            }


            var baseString = string.Format(baseFormat,
                                        ConsumerApiKey,
                                        oauthNonce,
                                        OauthSignatureMethod,
                                        oauthTimestamp,
                                        AccessToken,
                                        OauthVersion
                                        );

            baseString = string.Concat(method + "&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(ConsumerApiSecretKey),
                                    "&", Uri.EscapeDataString(AccessTokenSecret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }
            string respOS = Uri.EscapeDataString(oauth_signature);
            return respOS;
        }


        [HttpGet]
        [Route("HomeTweets")]
        public List<string> GetTwitterHomeTweets(string access_token = "", string accessSecret_token = "")
        {
            AccessToken = access_token;
            AccessTokenSecret = accessSecret_token;
            string myHeader = CreateHeader(twitterEndpoint.getHomeTweets(), Method.GET);

            restClient.endpoint = twitterEndpoint.getHomeTweets();

            twitterEndpoint.AuthorizationSignature(myHeader);
            string response = restClient.makeRequest(HttpVerb.GET, twitterEndpoint.getTwitterEndpoint());
            List<string> twitterHomeTimeline = new List<string>();

            using (JSONParser<List<TwitterHomeModel>> jsonParser = new JSONParser<List<TwitterHomeModel>>())
            {
                List<TwitterHomeModel> deserializedTwitterPostsAPIModel = jsonParser.parseJson(response);
                for (int i = 0; i < deserializedTwitterPostsAPIModel.Count; i++)
                {
                    twitterHomeTimeline.Add(deserializedTwitterPostsAPIModel[i].text);
                }
            }
            return twitterHomeTimeline;
        }

        [HttpGet]
        [Route("FavTweets")]
        public List<string> GetFavTweets(string access_token = "", string accessSecret_token = "")
        {
            AccessToken = access_token;
            AccessTokenSecret = accessSecret_token;
            string myHeader = CreateHeader(twitterEndpoint.getFavTweets(), Method.GET);

            restClient.endpoint = twitterEndpoint.getFavTweets();

            twitterEndpoint.AuthorizationSignature(myHeader);
            string response = restClient.makeRequest(HttpVerb.GET, twitterEndpoint.getTwitterEndpoint());
            List<string> favTweets = new List<string>();

            using (JSONParser<List<FavTweetModel>> jsonParser = new JSONParser<List<FavTweetModel>>())
            {
                List<FavTweetModel> deserializedTwitterPostsAPIModel = jsonParser.parseJson(response);
                for (int i = 0; i < deserializedTwitterPostsAPIModel.Count; i++)
                {
                    favTweets.Add(deserializedTwitterPostsAPIModel[i].text);
                }
            }
            return favTweets;
        }


        [HttpGet]
        [Route("TwitterFriends")]
        public List<FriendsUsers> GetTwitterFriendList(string access_token = "", string accessSecret_token = "")
        {
            List<FriendsUsers> Friends = new List<FriendsUsers>();
            AccessToken = access_token;
            AccessTokenSecret = accessSecret_token;


            string MyHeader = CreateHeader(twitterEndpoint.getTwitterFriends(), Method.GET);

            restClient.endpoint = twitterEndpoint.getTwitterFriends();
            twitterEndpoint.AuthorizationSignature(MyHeader);
            string response = restClient.makeRequest(HttpVerb.GET, twitterEndpoint.getTwitterEndpoint());
            JSONParser<TwitterFriendModel> jSONParser = new JSONParser<TwitterFriendModel>();
            TwitterFriendModel deserializedFacebookLikeAPIModel = new TwitterFriendModel();
            deserializedFacebookLikeAPIModel = jSONParser.parseJson(response);
            foreach (FriendsUsers posts in deserializedFacebookLikeAPIModel.users)
            {
                Friends.Add(posts);
            }
            return Friends;
        }


        [HttpGet]
        [Route("TwitterFollowers")]
        public List<Follower> GetTwitterFollowers(string access_token = "", string accessSecret_token = "")
        {
            List<Follower> Friends = new List<Follower>();
            AccessToken = access_token;
            AccessTokenSecret = accessSecret_token;


            string MyHeader = CreateHeader(twitterEndpoint.getTwitterFollowers(), Method.GET);

            restClient.endpoint = twitterEndpoint.getTwitterFollowers();
            twitterEndpoint.AuthorizationSignature(MyHeader);
            string response = restClient.makeRequest(HttpVerb.GET, twitterEndpoint.getTwitterEndpoint());
            JSONParser<TwitterFollowerModel> jSONParser = new JSONParser<TwitterFollowerModel>();
            TwitterFollowerModel deserializedFacebookLikeAPIModel = new TwitterFollowerModel();
            deserializedFacebookLikeAPIModel = jSONParser.parseJson(response);
            foreach (Follower posts in deserializedFacebookLikeAPIModel.users)
            {
                Friends.Add(posts);
            }
            return Friends;
        }

        [HttpPost]
        [Route("PostTweet")]
        public IHttpActionResult PostATweet(string access_token = "", string accessSecret_token = "", string tweet = "")
        {
            AccessToken = access_token;
            AccessTokenSecret = accessSecret_token;
            string search = null;

            string MyHeader = CreateHeader(twitterEndpoint.makeTweet(), Method.POST, tweet, search);

            restClient.endpoint = twitterEndpoint.makeTweet();
            twitterEndpoint.AuthorizationSignature(MyHeader);
            restClient.makeRequest(HttpVerb.POST, twitterEndpoint.getTwitterEndpoint(), tweet);

            return Ok();
        }

        public void addSearch(SearchHistory searchHistory, string username)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(x => x.UserName == username);
            searchHistory.Owner = user;
            db.SearchHistory.Add(searchHistory);
            db.SaveChanges();
        }

        [HttpGet]
        [Route("Search")]
        public List<TwitterSearchModel.Search> Search(string username, string access_token, string accessSecret_token, string query)
        {
            List<TwitterSearchModel.Search> statuses = new List<TwitterSearchModel.Search>();
            AccessToken = access_token;
            AccessTokenSecret = accessSecret_token;
            string tweet = null;
            string myHeader = CreateHeader(twitterEndpoint.searchList(), Method.GET, tweet, query);

            restClient.endpoint = twitterEndpoint.searchList();
            twitterEndpoint.AuthorizationSignature(myHeader);

            ServicePointManager.Expect100Continue = false;

            restClient.endpoint += "?q=" + query;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(restClient.endpoint);


            restClient.makeRequest(HttpVerb.GET, twitterEndpoint.getTwitterEndpoint());

            request.Headers.Add("Authorization", myHeader);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();

            string data = new StreamReader(response.GetResponseStream()).ReadToEnd();
            JSONParser<TwitterSearchModel> jSONParser = new JSONParser<TwitterSearchModel>();
            TwitterSearchModel deserializedModel = new TwitterSearchModel();
            deserializedModel = jSONParser.parseJson(data);
            foreach (TwitterSearchModel.Search res in deserializedModel.statuses)
            {
                statuses.Add(res);
            }
            return statuses;

        }
    }
}