using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.DivaRecord
{
    public static class DivaRecordLogic
    {
        // DIVAレコードチェックURL
        private static readonly string URL_DIVA_RECORD_CHECK_BASE = "http://project-diva-ac.net/divanet/record/check/";

        /*
         * DIVAレコードチェックメイン
         */
        public static void checkDivaRecordMain(string accessCode, string password, MainForm form)
        {
            // プレイヤー情報生成
            Player player = new Player(accessCode, password);

            // Web情報生成
            WebData web = new WebData(player);

            // ログインチェック
            if (DivaNetLogic.isLogin(web, form) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0002, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return;
            }

            // DIVAレコードチェックの結果を取得
            string checkResult = checkDivaRecord(web, form);

            // 後処理
            DivaNetLogic.afterInit(form);

            MessageBox.Show(checkResult, MessageConst.N_MSG_FINISH_T);
        }

        /*
         * DIVAレコードチェック
         */
        public static string checkDivaRecord(WebData web, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - DIVAレコード確認中...";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(URL_DIVA_RECORD_CHECK_BASE);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElement body = html.GetElementsByTagName("center")[1];

            string[] innerTexts = WebUtil.getInnerTextReplace(body.InnerText);

            StringBuilder buf = new StringBuilder();

            foreach (string innerText in innerTexts)
            {
                buf.Append(innerText);
            }

            buf.Append(MessageConst.N_MSG_FINISH_AFTER);

            return buf.ToString();
        }
    }
}
