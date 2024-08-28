namespace DivaNetAccess.src.Twitter
{
    public static class TwitterConst
    {
        // Twitter情報ファイル名
        public static string FILE_TWITTER_NAME = "twitter.txt";

        // Twitter連動キー
        public static string TWITTER_CONSUMER_KEY = "bD4VSzpYuaQiciSeeINw";
        public static string TWITTER_CONSUMER_SECRET = "78rUcKRQA6WMblnujZJjJGnLqXMseUXOOx7huQGiTL4";

        // TwitterつぶやきURL(API 1.1)
        public const string TWITTER_URL_WRITE = "https://api.twitter.com/1.1/statuses/update.json";

        public static int TWITTER_MAX_POST_LEN = 140;
    }
}
