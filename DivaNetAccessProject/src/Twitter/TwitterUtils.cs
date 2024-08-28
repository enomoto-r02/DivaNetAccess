using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DivaNetAccess.src.twitter
{
    public class TwitterUtils
    {
        #region Webより引用

        //const string REQUEST_TOKEN_URL = "http://twitter.com/oauth/request_token";
        //const string ACCESS_TOKEN_URL = "http://twitter.com/oauth/access_token";
        //const string AUTHORIZE_URL = "http://twitter.com/oauth/authorize";

        const string REQUEST_TOKEN_URL = "https://api.twitter.com/oauth/request_token";
        const string ACCESS_TOKEN_URL = "https://api.twitter.com/oauth/access_token";
        const string AUTHORIZE_URL = "https://api.twitter.com/oauth/authorize";

        private readonly Random random = new Random();

        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string RequestToken { get; private set; }
        public string RequestTokenSecret { get; private set; }
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }
        public string UserId { get; private set; }
        public string ScreenName { get; private set; }

        public TwitterUtils(string consumerKey, string consumerSecret)
        {
            ServicePointManager.Expect100Continue = false;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public TwitterUtils(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string userId, string screenName)
        {
            ServicePointManager.Expect100Continue = false;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
            UserId = userId;
            ScreenName = screenName;
        }

        public void GetRequestToken()
        {
            Dictionary<string, string> parameters = GenerateParameters("");
            string signature = GenerateSignature("", "GET", REQUEST_TOKEN_URL, parameters);
            parameters.Add("oauth_signature", UrlEncode(signature));
            string response = HttpGet(REQUEST_TOKEN_URL, parameters);
            Dictionary<string, string> dic = ParseResponse(response);
            RequestToken = dic["oauth_token"];
            RequestTokenSecret = dic["oauth_token_secret"];
        }

        public string GetAuthorizeUrl()
        {
            return AUTHORIZE_URL + "?oauth_token=" + RequestToken;
        }

        public void GetAccessToken(string pin)
        {
            Dictionary<string, string> parameters = GenerateParameters(RequestToken);
            parameters.Add("oauth_verifier", pin);
            string signature = GenerateSignature(RequestTokenSecret, "GET", ACCESS_TOKEN_URL, parameters);
            parameters.Add("oauth_signature", UrlEncode(signature));
            string response = HttpGet(ACCESS_TOKEN_URL, parameters);
            Dictionary<string, string> dic = ParseResponse(response);
            AccessToken = dic["oauth_token"];
            AccessTokenSecret = dic["oauth_token_secret"];
            UserId = dic["user_id"];
            ScreenName = dic["screen_name"];
        }

        public string Get(string url, IDictionary<string, string> parameters)
        {
            Dictionary<string, string> parameters2 = GenerateParameters(AccessToken);
            foreach (var p in parameters)
                parameters2.Add(p.Key, p.Value);
            string signature = GenerateSignature(AccessTokenSecret, "GET", url, parameters2);
            parameters2.Add("oauth_signature", UrlEncode(signature));
            return HttpGet(url, parameters2);
        }

        public string Post(string url, IDictionary<string, string> parameters)
        {
            Dictionary<string, string> parameters2 = GenerateParameters(AccessToken);
            foreach (var p in parameters)
                parameters2.Add(p.Key, p.Value);
            string signature = GenerateSignature(AccessTokenSecret, "POST", url, parameters2);
            parameters2.Add("oauth_signature", UrlEncode(signature));
            return HttpPost(url, parameters2);
        }

        private string HttpGet(string url, IDictionary<string, string> parameters)
        {
            WebRequest req = WebRequest.Create(url + '?' + JoinParameters(parameters));
            WebResponse res = req.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return result;
        }

        string HttpPost(string url, IDictionary<string, string> parameters)
        {
            byte[] data = Encoding.ASCII.GetBytes(JoinParameters(parameters));
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            WebResponse res = req.GetResponse();
            Stream resStream = res.GetResponseStream();
            StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            resStream.Close();
            return result;

        }

        private Dictionary<string, string> ParseResponse(string response)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string s in response.Split('&'))
            {
                int index = s.IndexOf('=');
                if (index == -1)
                    result.Add(s, "");
                else
                    result.Add(s.Substring(0, index), s.Substring(index + 1));
            }
            return result;
        }

        private string JoinParameters(IDictionary<string, string> parameters)
        {
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (var parameter in parameters)
            {
                if (first)
                    first = false;
                else
                    result.Append('&');
                result.Append(parameter.Key);
                result.Append('=');
                result.Append(parameter.Value);
            }
            return result.ToString();
        }

        private string GenerateSignature(string tokenSecret, string httpMethod, string url, Dictionary<string, string> parameters)
        {
            string signatureBase = GenerateSignatureBase(httpMethod, url, parameters);
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(UrlEncode(ConsumerSecret) + '&' + UrlEncode(tokenSecret));
            byte[] data = System.Text.Encoding.ASCII.GetBytes(signatureBase);
            byte[] hash = hmacsha1.ComputeHash(data);
            return Convert.ToBase64String(hash);
        }

        private string GenerateSignatureBase(string httpMethod, string url, Dictionary<string, string> parameters)
        {
            StringBuilder result = new StringBuilder();
            result.Append(httpMethod);
            result.Append('&');
            result.Append(UrlEncode(url));
            result.Append('&');
            result.Append(UrlEncode(JoinParameters(parameters)));
            return result.ToString();
        }

        private Dictionary<string, string> GenerateParameters(string token)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("oauth_consumer_key", ConsumerKey);
            result.Add("oauth_signature_method", "HMAC-SHA1");
            result.Add("oauth_timestamp", GenerateTimestamp());
            result.Add("oauth_nonce", GenerateNonce());
            result.Add("oauth_version", "1.0");
            if (!string.IsNullOrEmpty(token))
                result.Add("oauth_token", token);
            return result;
        }

        public string UrlEncode(string value)
        {
            string unreserved = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            StringBuilder result = new StringBuilder();
            byte[] data = Encoding.UTF8.GetBytes(value);
            foreach (byte b in data)
            {
                if (b < 0x80 && unreserved.IndexOf((char)b) != -1)
                    result.Append((char)b);
                else
                    result.Append('%' + String.Format("{0:X2}", (int)b));
            }
            return result.ToString();
        }

        private string GenerateNonce()
        {
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(8);
            for (int i = 0; i < 8; ++i)
                result.Append(letters[random.Next(letters.Length)]);
            return result.ToString();
        }

        private string GenerateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #endregion

        #region メンバ

        private static readonly string LINE_SEPARATOR = "\n";

        #endregion

        #region ファイル書き込み用

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(AccessToken + LINE_SEPARATOR);
            sb.Append(AccessTokenSecret + LINE_SEPARATOR);
            sb.Append(UserId + LINE_SEPARATOR);
            sb.Append(ScreenName + LINE_SEPARATOR);

            return sb.ToString();
        }

        #endregion
    }
}
