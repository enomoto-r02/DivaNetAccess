using DivaNetAccess.src.Const;
using DivaNetAccess.src.Twitter;
using DivaNetAccess.src.util;
using System.IO;

namespace DivaNetAccess.src.twitter
{
    public static class TwitterLogic
    {

        #region 楽曲書き込み

        /*
         * 楽曲書き込み
         */
        public static void writeTwitter(TwitterUtils twitter)
        {
            // 出力
            FileUtil.createFolder(SettingConst.CONF_DIR_NAME);
            FileUtil.writeFile(twitter.ToString(), SettingConst.CONF_DIR_NAME + "/" + TwitterConst.FILE_TWITTER_NAME, false);
        }

        #endregion

        #region 楽曲読み込み

        /*
         * 楽曲読み込み
         */
        public static TwitterUtils readTwitter()
        {
            string filePath = string.Format("{0}/{1}", SettingConst.CONF_DIR_NAME, TwitterConst.FILE_TWITTER_NAME);

            // 楽曲ファイル存在チェック
            if (File.Exists(filePath) == false)
            {
                return null;
            }

            // ファイルを開く
            using (StreamReader sr = new StreamReader(
                filePath,
                SettingConst.FILE_ENCODING
            ))
            {
                string[] lines = sr.ReadToEnd().Split('\n');

                TwitterUtils twitter = new TwitterUtils(
                    TwitterConst.TWITTER_CONSUMER_KEY,
                    TwitterConst.TWITTER_CONSUMER_SECRET,
                    lines[0],       // accessToken
                    lines[1],       // accessTokenSecret
                    lines[2],       // userId
                    lines[3]        // screenName
                );

                return twitter;
            }
        }

        #endregion
    }
}
