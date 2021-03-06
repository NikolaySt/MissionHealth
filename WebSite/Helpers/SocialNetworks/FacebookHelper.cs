using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;

using SocialNet.Backend.Configuration;

namespace SocialNet.WebSite
{
    public class FacebookHelper
    {
        private static readonly string _authorizeUri = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}";
        private static readonly string _accessTokenUri = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}";
        private static readonly string _scope = "public_profile,email";

        private static readonly string _apiKey = SystemSettings.FacebookKey;
        private static readonly string _apiSecret = SystemSettings.FacebookSecret;

        public static string GetConnectUrl(string redirectUri)
        {
            return string.Format(_authorizeUri, _apiKey, redirectUri, _scope);
        }

        public static async Task<string> GetAccessToken(string code, string redirectUri)
        {
            var tokens = new Dictionary<string, string>();

            var url = string.Format(_accessTokenUri, _apiKey, redirectUri, _scope, code, _apiSecret);

            var request = WebRequest.Create(url);

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
            var access_token = tokens["access_token"];

            if (string.IsNullOrWhiteSpace(access_token)) throw new UnauthorizedAccessException();

            return access_token;
        }
    }
}