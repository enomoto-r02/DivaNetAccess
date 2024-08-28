using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DivaNetAccess.src.util
{
    // Webユーティリティクラス
    public static class WebUtil
    {
        // 難易度
        public static readonly string[] DIFF_STR = { "EASY", "NORMAL", "HARD", "EXTREME", "EX EXTREME", "ALL" };

        // 評価
        public static readonly string[] CLEAR_RESULT_CHAR = { "-", "C", "G", "E", "P" };

        // 評価＠プレイ履歴用
        public static readonly string[] CLEAR_RESULT_STRING = { "NOT CLEAR", "STANDARD", "GREAT", "EXCELLENT", "PERFECT" };

        // トライアル文字列＠DIVA.NET用
        public static readonly string[] TRIAL_STR = { "C-TRIAL ○", "G-TRIAL ○", "E-TRIAL ○", "COMPLETE" };

        // トライアル文字列＠プレイ履歴トライアル用
        public static readonly string[] TRIAL_STR_PLAYRECORD = {
            "クリアトライアル", "グレートクリアトライアル", "エクセレントクリアトライアル", "パーフェクトクリアトライアル"
        };

        // 置換用URL
        public static string AFTER_URL = "http://project-diva-ac.net";


        /*
         * ソース文字列からHTMLDocumentを取得する
         */
        public static HtmlDocument getHtmlDocument(string source)
        {
            HtmlDocument ret;

            // WebBrowserコントロールを作成
            WebBrowser browser = new WebBrowser();
            browser.Dock = DockStyle.Fill;
            browser.Name = "webBrowser1";
            browser.ScriptErrorsSuppressed = true;     // スクリプトエラーの警告を無効にする＠Wiki対策

            browser.Navigate("about:blank");    // 空白ページを開く
            browser.Document.OpenNew(true);     // クリア
            browser.Document.Write(source);     // 書き込み

            try
            {
                // サーバーへアクセスするわけではないので、ウェイトは控えめ
                int waitTime = 20;

                // 初回強制ウェイト
                System.Threading.Thread.Sleep(waitTime);

                // 読み込みが完了していない
                while (browser.IsBusy)
                {
                    // 再度ウェイト
                    System.Threading.Thread.Sleep(waitTime);

                    browser.Refresh();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("HTMLの読み込みに失敗しました。", MessageConst.E_MSG_ERROR_T);

                throw;
            }

            ret = browser.Document;

            // 解放
            browser.Dispose();

            return ret;
        }

        /*
         * HtmlDocumentで読み込んだURLを修正する
         */
        public static string convUrlToHtml(string url)
        {
            if (url.StartsWith("about:blank"))
            {
                return url.Replace("about:blank", AFTER_URL);
            }
            else if (url.StartsWith("about:"))
            {
                return url.Replace("about:", AFTER_URL);
            }

            // 通常はありえないパターン
            return "";
        }

        /*
         * 次ページ遷移の有無を調べる
         */
        public static Boolean checkFoundNextPage(HtmlElementCollection links)
        {
            // リンクを取得
            foreach (HtmlElement ele in links)
            {
                // 
                if ("次へ[#]".Equals(ele.InnerText))
                {
                    return true;
                }
            }

            return false;
        }

        /*
         * 難易度文字列→インデックス
         */
        public static int getDiffIndex(string diff)
        {
            for (int i = 0; i < DIFF_STR.Length - 1; i++)
            {
                if (DIFF_STR[i].Equals(diff))
                {
                    return i;
                }
            }
            return -1;
        }

        /*
         * クリア文字(例："C")→インデックス
         */
        public static int getClearIndexChar(string clear)
        {
            for (int i = 0; i < CLEAR_RESULT_CHAR.Length; i++)
            {
                if (CLEAR_RESULT_CHAR[i].Equals(clear))
                {
                    return i;
                }
            }
            return -1;
        }

        /*
         * クリア文字列(例："STANDARD")→インデックス
         */
        public static int getClearIndexString(string clear)
        {
            for (int i = 0; i < CLEAR_RESULT_STRING.Length; i++)
            {
                if (CLEAR_RESULT_STRING[i].Equals(clear))
                {
                    return i;
                }
            }
            return -1;
        }

        /*
         * トライアル状況→文字列
         */
        public static string getTrialStr(string trial)
        {
            for (int i = 0; i < TRIAL_STR.Length; i++)
            {
                if (TRIAL_STR[i].Equals(trial))
                {
                    // CLEAR_RESULTは"-"から始まるので+1
                    return CLEAR_RESULT_CHAR[i + 1];
                }
                // 連続パフェトラOFF
                if (trial.StartsWith("COMPLETE"))
                {
                    return CLEAR_RESULT_CHAR[4];
                }
                // 連続パフェトラON
                if (trial.StartsWith("連続"))
                {
                    return CLEAR_RESULT_CHAR[4];
                }
            }
            // "-"
            return CLEAR_RESULT_CHAR[0];
        }

        /*
         * 連続パーフェクトトライアル現在数取得
         */
        public static int getTrialRenzokuNow(string trial)
        {
            string tmp = WebUtil.getMache(trial, "連続\\d+回");

            // 連続パフェトラ未解禁
            if (string.IsNullOrEmpty(tmp))
            {
                return -1;
            }

            // 回数だけ抽出
            return int.Parse(WebUtil.getMache(tmp, "\\d+"));
        }

        /*
         * 連続パーフェクトトライアル最高数取得
         */
        public static int getTrialRenzokuMax(string trial)
        {
            string tmp = WebUtil.getMache(trial, "最高記録\\d+");

            // 連続パフェトラ未解禁
            if (string.IsNullOrEmpty(tmp))
            {
                return -1;
            }

            // 回数だけ抽出
            return int.Parse(WebUtil.getMache(tmp, "\\d+"));
        }

        /*
         * トライアル状況("連続パーフェクトトライアル 失敗")→文字列＠プレイ履歴用
         */
        public static int getTrialIndexPlayRecord(string trial)
        {
            // トライアルなし
            if (string.IsNullOrEmpty(trial))
            {
                return 0;
            }

            // 連続パフェトラ加算用
            bool renzokuFlg = false;

            if (trial.Contains("連続"))
            {
                renzokuFlg = true;
            }

            // 前方一致検索用に除外
            string trialTmp = trial.Replace("連続", "");

            for (int i = 0; i < TRIAL_STR_PLAYRECORD.Length; i++)
            {
                if (trialTmp.StartsWith(TRIAL_STR_PLAYRECORD[i]))
                {
                    if (renzokuFlg)
                    {
                        // クリアトライアルを1とするので添字+1かつ、連続パフェトラで+1
                        return i + 1 + 1;
                    }
                    else
                    {
                        // クリアトライアルを1とするので添字+1
                        return i + 1;
                    }
                }
            }

            return 0;
        }

        /*
         * 正規表現に最初にマッチする文字列を取得する
         */
        public static string getMache(string target, string pattern)
        {
            string ret = "";

            // null回避
            if (target == null)
            {
                return ret;
            }

            MatchCollection mc = Regex.Matches(target, pattern);

            // ヒットした
            if (mc.Count > 0)
            {
                ret = mc[0].Value;
            }

            return ret;
        }

        /*
         * タブ、スペースのみを含む、空白行を削除する
         */
        public static string delBlankLine(string target)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string line in target.Split('\n'))
            {
                string s = line.Replace("\t", "").Trim();

                if (string.IsNullOrEmpty(s) == false)
                {
                    sb.Append(s);
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }

        /*
         * 正規表現にマッチする文字列の配列を取得する
         */
        public static string[] getMaches(string target, string pattern)
        {
            string[] ret = null;

            // null回避
            if (target == null)
            {
                return ret;
            }

            MatchCollection mc = Regex.Matches(target, pattern);

            // ヒットした
            if (mc.Count > 0)
            {
                ret = new string[mc.Count];

                // ヒットした文字列を格納
                for (int i = 0; i < mc.Count; i++)
                {
                    ret[i] = mc[i].Value;
                }
            }

            return ret;
        }

        /*
         * 達成率変換(XX.XX%)→(XXXX)
         */
        public static int convTasseiritu(string tasseiritu)
        {
            string[] tmpArray = tasseiritu.Replace("％", "").Replace("%", "").Split('.');
            return int.Parse(int.Parse(tmpArray[0]) + tmpArray[1].PadRight(2, '0'));
        }

        /*
         * innterTextの改行、トリム処理を行う
         */
        public static string[] getInnerTextReplace(string innerText)
        {
            string[] tmpArray = innerText.Replace("\r\n", "\n").Split('\n');

            string[] retArray = new string[tmpArray.Length];

            for (int i = 0; i < tmpArray.Length; i++)
            {
                retArray[i] = tmpArray[i].Trim();
            }

            return retArray;
        }
    }
}
