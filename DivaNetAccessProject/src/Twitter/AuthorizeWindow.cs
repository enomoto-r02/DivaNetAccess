using DivaNetAccess.src.Twitter;
using System;
using System.Net;
using System.Windows.Forms;

namespace DivaNetAccess.src.twitter
{
    // Twitter認証ウインドウ＠初回のみ呼び出し
    public partial class AuthorizeWindow : Form
    {
        #region メンバ

        TwitterUtils auth;

        #endregion

        //public AuthorizeWindow()
        //{
        //    InitializeComponent();
        //}

        /*
         * TwitterWindowで生成した認証クラスを引き継ぐ
         */
        public AuthorizeWindow()
        {
            InitializeComponent();

            // 初回認証
            MessageBox.Show("Twitterとアプリの連動を行います。\n表示されたPINを入力してください。", "アプリ連動");

            authorize();
        }

        /*
         * 認証画面呼び出し
         */
        private void authorize()
        {
            auth = new TwitterUtils(TwitterConst.TWITTER_CONSUMER_KEY, TwitterConst.TWITTER_CONSUMER_SECRET);

            // リクエストトークンを取得する
            auth.GetRequestToken();

            // ブラウザでURLを開く
            System.Diagnostics.Process.Start(auth.GetAuthorizeUrl());
        }

        /*
         * 決定ボタン
         */
        private void btnAuthentication_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tboxPinCode.Text) || tboxPinCode.Text.Length != tboxPinCode.MaxLength)
            {
                MessageBox.Show("7桁のPINを入力してください。", "エラー");
                return;
            }

            // PIN不正対策
            try
            {
                // アクセストークンを取得する
                auth.GetAccessToken(tboxPinCode.Text);
            }
            catch (WebException we)
            {
                // ↓ステータスコードだけを見ればOKなのでこのif文はいらない？
                if (we.Status == System.Net.WebExceptionStatus.ProtocolError)
                {
                    //HttpWebResponseを取得
                    System.Net.HttpWebResponse errres =
                        (System.Net.HttpWebResponse)we.Response;

                    if (errres.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("PINが違います。\n再度PINを発行して認証処理を行ってください。", "エラー");

                        Close();
                        return;
                    }
                    else
                    {
                        throw we;
                    }
                }
                else
                {
                    throw we;
                }
            }

            TwitterLogic.writeTwitter(auth);

            MessageBox.Show("認証用ファイルを作成しました。\n再度つぶやくボタンを押してください。", "完了");

            Close();
        }

        /*
         * キャンセルボタン
         */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
