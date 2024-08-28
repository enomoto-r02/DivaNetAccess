using DivaNetAccess.src;
using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess
{
    public class WebData
    {

#if DEBUG
        private readonly int WAIT_TIME = 30;
#else
        private readonly int WAIT_TIME = 100;
#endif
        private readonly int MAX_ERROR_CNT = 3;
        private readonly string TOKEN_KEY = "org.apache.struts.taglib.html.TOKEN";

        public CookieContainer cc { get; set; }

        public Dictionary<string, string> postDatas { get; private set; }
        public string postDataStr
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in postDatas.Keys)
                {
                    sb.Append(string.Format("{0}={1}", key, postDatas[key]));
                    sb.Append("&");     // 最後に付与しても大丈夫？
                }

                string ret = sb.ToString();

                return ret.Substring(0, ret.Length - 1);
            }
        }
        public byte[] postData { get; private set; }

        /*
         * コンストラクタ
         */
        public WebData()
        {
        }

        /*
         * コンストラクタ
         */
        public WebData(Player player)
        {
            cc = new CookieContainer();

            postDatas = new Dictionary<string, string>();

            postDatas.Add("accessCode", player.accessCode);
            postDatas.Add("password", player.password);

            postData = Encoding.ASCII.GetBytes(postDataStr);
        }

        /*
         *  GET通信を行う
         * 
         * 引数       url
         * 戻り値     String配列(0:POST後のURL、1:POST後のソース文字列) 
         */
        public string[] HttpGet(string url)
        {
            int errCnt = 0;

            string[] result = null;

            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(WAIT_TIME);

                    // リクエストの作成
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse res = req.GetResponse();

                    // レスポンスの読み取り
                    Stream resStream = res.GetResponseStream();
                    StreamReader sr = new StreamReader(resStream, SettingConst.WEB_ENCODING);

                    result = new string[] { res.ResponseUri.ToString(), sr.ReadToEnd() };

                    sr.Close();
                    resStream.Close();

                    // 正常終了
                    break;
                }
                catch (WebException e)
                {
                    errCnt++;

                    // リトライ回数を超えた
                    if (errCnt > MAX_ERROR_CNT)
                    {
                        throw e;
                    }

                    // DIVA.NET関連ページなら
                    if (url.Contains("project-diva-ac.net/divanet"))
                    {
                        // cookieの再取得を行う
                        updateCookie();
                    }
                }
            }

            return result;
        }

        /*
         *  POST通信を行う＠wiki用
         * 
         * 引数       url
         * 戻り値     String配列(0:POST後のURL、1:POST後のソース文字列) 
         */
        /*
        public string[] HttpPostWiki(string url)
        {
            System.Threading.Thread.Sleep(WAIT_TIME);
            
            int errCnt = 0;

            string[] result = null;

            while (true)
            {
                try
                {
                    // リクエストの作成
                    WebRequest req = WebRequest.Create(url);
                    WebResponse res = req.GetResponse();

                    // レスポンスの読み取り
                    Stream resStream = res.GetResponseStream();
                    StreamReader sr = new StreamReader(resStream, SettingConst.WEB_ENCODING);

                    // 結果を設定
                    result = new string[] { res.ResponseUri.ToString(), sr.ReadToEnd() };

                    sr.Close();
                    resStream.Close();

                    break;
                }
                catch (WebException e)
                {
                    errCnt++;

                    // リトライ回数を超えた
                    if (errCnt > MAX_ERROR_CNT)
                    {
                        throw e;
                    }

                    // リトライ
                }
            }

            return result;
        }
        */

        /*
         *  POST通信を行う
         * 
         * 引数       url
         * 戻り値     String配列(0:POST後のURL、1:POST後のソース文字列) 
         */
        public string[] HttpPost(string url)
        {
            System.Threading.Thread.Sleep(WAIT_TIME);

            // postDataのTOKENが無ければCookie無しとし、Cookieを取得する
            if (!postDatas.ContainsKey(TOKEN_KEY))
            {
                updateCookie();
            }

            int errCnt = 0;

            string[] result = null;

            while (true)
            {
                try
                {
                    // リクエストの作成
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = postData.Length;
                    req.CookieContainer = cc;

                    // ポスト・データの書き込み
                    Stream reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    reqStream.Close();

                    WebResponse res = req.GetResponse();

                    // レスポンスの読み取り
                    Stream resStream = res.GetResponseStream();
                    StreamReader sr = new StreamReader(resStream, SettingConst.WEB_ENCODING);

                    // 結果を設定
                    result = new string[] { res.ResponseUri.ToString(), sr.ReadToEnd() };

                    sr.Close();
                    resStream.Close();

                    if (result[1].Contains("DIVA.NETサーバへの接続を終了します。"))
                    {
                        errCnt++;

                        // リトライ回数を超えた
                        if (errCnt > MAX_ERROR_CNT)
                        {
                            throw new Exception("接続が確立できませんでした。");
                        }

                        // DIVA.NET関連ページなら
                        if (url.Contains("project-diva-ac.net/divanet"))
                        {
                            // cookieの再取得を行う
                            updateCookie();
                        }

                        // リトライ
                    }
                    else
                    {
                        // 終了
                        break;
                    }
                }
                catch (WebException e)
                {
                    errCnt++;

                    // リトライ回数を超えた
                    if (errCnt > MAX_ERROR_CNT)
                    {
                        throw e;
                    }

                    // DIVA.NET関連ページなら
                    if (url.Contains("project-diva-ac.net/divanet"))
                    {
                        // cookieの再取得を行う
                        updateCookie();
                    }

                    // リトライ
                }
            }

            return result;
        }

        // メンバのDIVA.NETのCookieを更新する
        private string[] updateCookie()
        {
            System.Threading.Thread.Sleep(WAIT_TIME);

            // アクセスコード、パスワード以外のパラメータを削除し、上書きする
            Dictionary<string, string> newParam = new Dictionary<string, string>();
            foreach (string key in postDatas.Keys)
            {
                if ("accessCode".Equals(key) || "password".Equals(key))
                {
                    newParam.Add(key, postDatas[key]);
                }
            }
            postDatas = newParam;

            string[] result = null;

            // TOKEN情報を除いたパラメータでログインする
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(UrlConst.URL_LOGIN);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = postData.Length;
            req.CookieContainer = cc;

            // ポスト・データの書き込み
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            WebResponse res = req.GetResponse();

            Stream resStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, SettingConst.WEB_ENCODING);

            // 結果を設定
            result = new string[] { res.ResponseUri.ToString(), sr.ReadToEnd() };

            sr.Close();
            resStream.Close();

            // hidden項目をパラメータに加える
            getHiddenParameter(postDatas, result[1], "loginActionForm");
            postData = Encoding.ASCII.GetBytes(postDataStr);

            System.Threading.Thread.Sleep(WAIT_TIME);

            // リクエストの作成
            req = (HttpWebRequest)WebRequest.Create(UrlConst.URL_LOGIN);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = postData.Length;
            req.CookieContainer = cc;

            reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            res = req.GetResponse();

            // レスポンスの読み取り
            resStream = res.GetResponseStream();
            sr = new StreamReader(resStream, SettingConst.WEB_ENCODING);

            // 結果を設定
            result = new string[] { res.ResponseUri.ToString(), sr.ReadToEnd() };

            return result;
        }

        // formタグ内のhiddenパラメータを追加する
        private void getHiddenParameter(Dictionary<string, string> param, string source, string formTagName)
        {
            HtmlDocument html = WebUtil.getHtmlDocument(source);

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElementCollection forms = html.GetElementsByTagName("form");

            // tbodyタグ全検索
            foreach (HtmlElement form in forms)
            {
                if (form.GetAttribute("name").Equals(formTagName))
                {
                    // tbodyタグ全検索
                    foreach (HtmlElement input in form.GetElementsByTagName("input"))
                    {
                        // tbodyタグ全検索
                        if (input.GetAttribute("type").Equals("hidden"))
                        {
                            string name = input.GetAttribute("name");
                            string value = input.GetAttribute("value");

                            param.Add(name, value);
                        }
                    }
                }
            }
        }
    }
}