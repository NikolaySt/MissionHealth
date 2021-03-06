using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

using SocialNet.Backend.Configuration;

namespace SocialNet.WebSite
{
    public class TwitterHelper
    {
        private static readonly string _requesttokenUri = "https://api.twitter.com/oauth/request_token";
        private static readonly string _authenticateUri = "https://api.twitter.com/oauth/authenticate?oauth_token={0}";
        private static readonly string _accessTokenUri = "https://api.twitter.com/oauth/access_token";
        private static readonly string _userShowTokenUri = "https://api.twitter.com/1.1/users/show.json";
        protected static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private static readonly string _apiKey = SystemSettings.TwitterKey;
        private static readonly string _apiSecret = SystemSettings.TwitterSecret;

        public static async Task<string> GetConnectionUrl()
        {
            var request = WebRequest.Create(_requesttokenUri);
            request.Method = "POST";

            var oauth_timestamp = GetTimeSpan().ToString();
            var uniqueCode = GetUniqueCode();

            var requestParams = string.Format(
                "oauth_consumer_key={0}&" +
                "oauth_nonce={1}&" +
                "oauth_signature_method=HMAC-SHA1&" +
                "oauth_timestamp={2}&" +
                "oauth_version=1.0",
                _apiKey, uniqueCode, oauth_timestamp);

            var signatureFormat = "POST&{0}&{1}";

            var text = string.Format(signatureFormat, WebUtility.UrlEncode(_requesttokenUri), WebUtility.UrlEncode(requestParams));

            var oauth_signature = GetSignature(string.Format("{0}&{1}", _apiSecret, ""), text);

            var authorization = string.Format(
                "OAuth oauth_consumer_key=\"{0}\"," +
                "oauth_nonce=\"{1}\"," +
                "oauth_signature=\"{2}\"," +
                "oauth_signature_method=\"HMAC-SHA1\"," +
                "oauth_timestamp=\"{3}\"," +
                "oauth_version=\"1.0\""
                , _apiKey, uniqueCode, oauth_signature, oauth_timestamp);

            request.Headers.Add("Authorization", authorization);

            var tokens = new Dictionary<string, string>();

            using (var response = await request.GetResponseAsync())
            {
                var stream = new StreamReader(response.GetResponseStream());
                var values = await stream.ReadToEndAsync();
                var segments = values.Split('&');
                foreach (var pair in segments)
                {
                    var data = pair.Split('=');
                    if (data.Length != 2) continue;
                    tokens.Add(data[0], data[1]);
                }
            }

            var oauth_token = tokens["oauth_token"];
            var oauth_token_secret = tokens["oauth_token_secret"];

            return string.Format(_authenticateUri, oauth_token);
        }

        public static async Task<Dictionary<string, string>> GetAccessToken(string oauth_token, string oauth_verifier)
        {
            var oauth_timestamp = GetTimeSpan().ToString();
            var uniqueCode = GetUniqueCode();

            var requestParams = string.Format(
                "oauth_consumer_key={0}&" +
                "oauth_nonce={1}&" +
                "oauth_signature_method=HMAC-SHA1&" +
                "oauth_timestamp={2}&" +
                "oauth_token={3}&" +
                "oauth_version=1.0",
                _apiKey, uniqueCode, oauth_timestamp, oauth_token);

            var signatureFormat = "POST&{0}&{1}";

            var text = string.Format(signatureFormat, WebUtility.UrlEncode(_accessTokenUri), WebUtility.UrlEncode(requestParams));

            var oauth_signature = GetSignature(string.Format("{0}&{1}", _apiSecret, ""), text);

            var authorization = string.Format(
                "OAuth oauth_consumer_key=\"{0}\"," +
                "oauth_nonce=\"{1}\"," +
                "oauth_signature=\"{2}\"," +
                "oauth_signature_method=\"HMAC-SHA1\"," +
                "oauth_timestamp=\"{3}\"," +
                "oauth_token=\"{4}\"," +
                "oauth_version=\"1.0\""
                , _apiKey, uniqueCode, oauth_signature, oauth_timestamp, oauth_token);

            var request = WebRequest.Create(_accessTokenUri);
            request.Method = "POST";
            request.Headers.Add("Authorization", authorization);
            request.ContentType = "application/x-www-form-urlencoded";

            string postData = string.Format("oauth_verifier={0}", oauth_verifier);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            var tokens = new Dictionary<string, string>();
            using (var response = await request.GetResponseAsync())
            {
                var stream = new StreamReader(response.GetResponseStream());
                var values = await stream.ReadToEndAsync();
                var segments = values.Split('&');
                foreach (var pair in segments)
                {
                    var data = pair.Split('=');
                    if (data.Length != 2) continue;
                    tokens.Add(data[0], data[1]);
                }
            }

            //oauth_token = tokens["oauth_token"];
            //oauth_token_secret = tokens["oauth_token_secret"];
            //user_id = tokens["user_id"];
            //screen_name = tokens["screen_name"];

            return tokens;
        }

        public static async Task<JObject> GetUserShow(string oauth_token, string oauth_token_secret, string screen_name)
        {
            var oauth_timestamp = GetTimeSpan().ToString();
            var uniqueCode = GetUniqueCode();

            var requestParams = string.Format(
                "oauth_consumer_key={0}&" +
                "oauth_nonce={1}&" +
                "oauth_signature_method=HMAC-SHA1&" +
                "oauth_timestamp={2}&" +
                "oauth_token={3}&" +
                "oauth_version=1.0&" +
                "screen_name={4}",
                _apiKey, uniqueCode, oauth_timestamp, oauth_token, screen_name);

            var signatureFormat = "GET&{0}&{1}";

            var text = string.Format(signatureFormat, WebUtility.UrlEncode(_userShowTokenUri), WebUtility.UrlEncode(requestParams));

            var oauth_signature = GetSignature(string.Format("{0}&{1}", _apiSecret, oauth_token_secret), text);

            var authorization = string.Format(
                "OAuth oauth_consumer_key=\"{0}\"," +
                "oauth_nonce=\"{1}\"," +
                "oauth_signature=\"{2}\"," +
                "oauth_signature_method=\"HMAC-SHA1\"," +
                "oauth_timestamp=\"{3}\"," +
                "oauth_token=\"{4}\"," +
                "oauth_version=\"1.0\""
                , _apiKey, uniqueCode, oauth_signature, oauth_timestamp, oauth_token);

            var request = WebRequest.Create(string.Concat(_userShowTokenUri, "?screen_name=", screen_name));
            request.Method = "GET";
            request.Headers.Add("Authorization", authorization);

            var tokens = new Dictionary<string, string>();
            using (var response = await request.GetResponseAsync())
            {
                var stream = new StreamReader(response.GetResponseStream());
                var values = await stream.ReadToEndAsync();
                return JObject.Parse(values);
            }
        }

        private static string GetUniqueCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var code = new byte[32];
            var random = new Random();

            for (int i = 0; i < code.Length; i++)
            {
                code[i] = Convert.ToByte(chars[random.Next(chars.Length)]);
            }
            return Encoding.UTF8.GetString(code);
        }

        private static long GetTimeSpan()
        {
            var interval = DateTime.UtcNow - EpochStart;
            return (long)interval.TotalSeconds;
        }

        private static string GetSignature(string key, string data)
        {
            byte[] secretkey = Encoding.UTF8.GetBytes(key);
            var signatureResult = "";
            using (var hmac = new HMACSHA1(secretkey))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                signatureResult = Convert.ToBase64String(hashValue, 0, hashValue.Length);
            }
            return WebUtility.UrlEncode(signatureResult);
        }
    }
}