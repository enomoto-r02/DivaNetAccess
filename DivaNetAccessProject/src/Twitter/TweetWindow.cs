using DivaNetAccess.src.Common;
using DivaNetAccess.src.Twitter;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DivaNetAccess.src.twitter
{
    // ツイートウインドウ
    public partial class TweetWindow : BaseSearchWindow
    {
        #region メンバ

        readonly TwitterUtils auth;

        #endregion

        //public TweetWindow()
        //{
        //    InitializeComponent();
        //}

        /*
         * コンストラクタ
         */
        public TweetWindow(string viewStr)
        {
            InitializeComponent();

            // Twitter認証処理
            auth = TwitterLogic.readTwitter();

            tboxPostStr.Text = viewStr;
        }

        /*
         * つぶやくボタン
         */
        private void btnTweetPost_Click(object sender, EventArgs e)
        {
            // 文字列ブランクチェック
            if (string.IsNullOrEmpty(tboxPostStr.Text))
            {
                MessageBox.Show("つぶやく内容がありません。", "エラー");
                return;
            }

            // 文字数チェック
            if (tboxPostStr.Text.Length > TwitterConst.TWITTER_MAX_POST_LEN)
            {
                MessageBox.Show("つぶやく内容が長すぎます。", "エラー");
                return;
            }

            tweetPost();

            Close();
        }

        /*
         * つぶやき処理
         */
        public void tweetPost()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Clear();
            parameters.Add("status", auth.UrlEncode(tboxPostStr.Text));
            Console.WriteLine(auth.Post(TwitterConst.TWITTER_URL_WRITE, parameters));

            MessageBox.Show("ツイートしました。", "完了");
        }

        /*
         * 現在の文字数表示
         */
        private void tboxPostStr_TextChanged(object sender, EventArgs e)
        {
            labTextLength.Text = (TwitterConst.TWITTER_MAX_POST_LEN - tboxPostStr.Text.Length).ToString().PadLeft(5);
        }
    }
}
