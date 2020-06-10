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

        protected RestClient restClient;

        private TwitterEndpoint twitterEndpoint;

        public TwitterController() : base()
        {
            this.restClient = new RestClient();
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

        private string CreateHeader(string resourceUrl, Method method)
        {
            var oauthNonce = CreateOauthNonce();
            // Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var oauthTimestamp = CreateOAuthTimestamp();
            var oauthSignature = CreateOauthSignature(resourceUrl, method, oauthNonce, oauthTimestamp);
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

        public string CreateOauthSignature(string resourceUrl, Method method, string oauthNonce, string oauthTimestamp) 
        {
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                        "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

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

            this.twitterEndpoint = new TwitterEndpoint(AccessToken, accessSecret_token);


            string MyHeader = CreateHeader(twitterEndpoint.getHomeTweets(), Method.GET);

            

            restClient.endpoint = twitterEndpoint.getHomeTweets();
            twitterEndpoint.AuthorizationSignature(MyHeader);
            string response = restClient.makeRequest();
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
    }
}