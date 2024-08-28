using DivaNetAccess.src;
using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess
{
    // 楽曲別設定用DIVANETアクセスクラス
    public static class DivaNetSongSettingLogic
    {
        // 楽曲別設定URL
        private static readonly string URL_SONG_SETTING_BASE = "http://project-diva-ac.net/divanet/setting/individual/0/true/";

        // モジュール設定URL
        private static readonly string URL_MODULE_BASE = "http://project-diva-ac.net/divanet/module/{0}/{1}/{2}/{3}/0";

        // モジュール未設定URL＠楽曲別
        private static readonly string URL_MODULE_INIT = "http://project-diva-ac.net/divanet/module/resetIndividual/{0}/0";

        // モジュール設定URL確認画面＠楽曲URL、vocal1or2、モジュールURL、キャラNo
        private static readonly string URL_MODULE_CONFIRM = "http://project-diva-ac.net/divanet/module/confirm/{0}/{1}/{2}/{3}/0/0";

        // モジュール設定URL楽曲選択＠これをしないとエラーになる
        private static readonly string URL_MODULE_INFO = "http://project-diva-ac.net/divanet/module/selectPv/{0}/0";

        // モジュール未設定URL＠楽曲共通
        private static readonly string URL_MODULE_INIT_COMMON = "http://project-diva-ac.net/divanet/module/resetCommon/";

        // スキン設定URL
        private static readonly string URL_SKIN_BASE = "http://project-diva-ac.net/divanet/skin/{0}/{1}/{2}/0";

        // ボタン音設定URL
        private static readonly string URL_BUTTON_BASE = "http://project-diva-ac.net/divanet/buttonSE/{0}/{1}/{2}/0";

        // 楽曲共通設定URL
        private static readonly string URL_SONG_SETTING_COMMON_BASE = "http://project-diva-ac.net/divanet/setting/common/";

        /*
         * 楽曲別設定ボタンメイン
         */
        public static Dictionary<string, SongSettingData> getSongSettingMain(string accessCode, string password, MainForm form)
        {
            // ストップウォッチ生成
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // プレイヤー情報生成
            Player player = new Player(accessCode, password);

            // Web情報生成
            WebData web = new WebData(player);

            // ログインチェック
            if (DivaNetLogic.isLogin(web, form) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0002, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return null;
            }

            // プレイ履歴リスト生成
            Dictionary<string, SongSettingData> settings = new Dictionary<string, SongSettingData>();

            // 楽曲共通設定取得
            getSongSettingCommonDataMain(web, settings, form);

            // 楽曲別設定リスト取得
            getSongSettingData(web, 0, settings, form);

            // 楽曲別設定書き込み処理
            DivaNetSongSettingLogic.writeSongSettingData(player, settings);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(
                MessageConst.N_MSG_0006 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒",
                MessageConst.N_MSG_FINISH_T
            );

            return settings;
        }

        /*
         * 楽曲別設定取得
         */
        private static void getSongSettingData(WebData web, int page, Dictionary<string, SongSettingData> settings, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - 楽曲別設定取得中...[" + (page + 1) + "ページ]";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(URL_SONG_SETTING_BASE + "/" + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElementCollection tbodys = html.GetElementsByTagName("tbody");

            // tbodyタグ全検索
            foreach (HtmlElement tbody in tbodys)
            {
                //string[] innerTexts = tbody.InnerText.Replace("\r\n", "\n").Trim().Split('\n');
                string[] innerTexts = WebUtil.getInnerTextReplace(tbody.InnerText);

                SongSettingData setting = new SongSettingData();

                // カウンタ＠モジュールor衣装1、2の行数ずれ対策
                int innerTextsCnt = 0;

                // 曲名
                setting.name = innerTexts[innerTextsCnt];
                innerTextsCnt++;

                // モジュール1
                setting.module1 = innerTexts[innerTextsCnt].Split('1')[1];
                innerTextsCnt++;

                // "2"があればモジュール2に設定
                if (innerTexts[innerTextsCnt].Split('2').Length > 1)
                {
                    setting.module2 = innerTexts[innerTextsCnt].Split('2')[1];
                    innerTextsCnt++;
                }
                else
                {
                    setting.module2 = "-";
                }

                // スキン＠3文字目(スキン)以降を切り取る
                setting.skin = innerTexts[innerTextsCnt].Substring(3);
                innerTextsCnt++;

                // ボタン音＠4文字目(ボタン音)以降を切り取る
                setting.button = innerTexts[innerTextsCnt].Substring(4);
                innerTextsCnt++;

                settings.Add(setting.name, setting);
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getSongSettingData(web, page + 1, settings, form);
            }
        }

        /*
         * 楽曲共通設定取得
         */
        public static void getSongSettingCommonDataMain(WebData web, Dictionary<string, SongSettingData> settings, MainForm form)
        {
            // HTMLドキュメント取得
            string[] res = web.HttpPost(URL_SONG_SETTING_COMMON_BASE);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElement body = html.GetElementsByTagName("body")[0];

            //string[] innerTexts = body.InnerText.Replace("\r\n", "\n").Trim().Split('\n');
            string[] innerTexts = WebUtil.getInnerTextReplace(body.InnerText);

            SongSettingData setting = new SongSettingData();

            // bodyタグの1行ごとを読み込む
            for (int i = 0; i < innerTexts.Length; i++)
            {
                string innerText = innerTexts[i];

                if (innerText.StartsWith("[ボーカル1]"))
                {
                    i = i + 1;
                    setting.module1 = innerTexts[i];
                }

                if (innerText.StartsWith("[ボーカル2]"))
                {
                    i = i + 1;
                    setting.module2 = innerTexts[i];
                }

                if (innerText.StartsWith("▼共通スキン設定"))
                {
                    i = i + 2;
                    setting.skin = innerTexts[i];
                }

                if (innerText.StartsWith("[ボタン音]"))
                {
                    i = i + 1;
                    setting.button = innerTexts[i];
                }
            }

            setting.name = "楽曲共通";

            // 楽曲共通追加
            settings.Add(setting.name, setting);
        }

        /*
         * 楽曲別設定_設定
         */
        public static void songSettingSet(string accessCode, string password, string songName, ComboBox[] cBoxs, UrlData urls, MainForm form)
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

            // モジュール設定
            songSettingSetModule(web, songName, cBoxs[0].Text, cBoxs[1].Text, urls);

            // ボタン音設定
            songSettingSetButton(web, songName, cBoxs[2].Text, urls);

            // スキン設定
            songSettingSetSkin(web, songName, cBoxs[3].Text, urls);

            DivaNetLogic.afterInit(form);

            MessageBox.Show(MessageConst.N_MSG_0007 + MessageConst.N_MSG_FINISH_AFTER, MessageConst.N_MSG_FINISH_T);
        }

        /*
         * モジュール設定
         */
        public static void songSettingSetModule(WebData web, string songName, string module1Name, string module2Name, UrlData urls)
        {
            int soloNumber;

            // 楽曲共通チェック
            string songUrl;
            if (songName.Equals("楽曲共通"))
            {
                songUrl = "COMMON";
                soloNumber = -1;        // 楽曲共通＠仮
            }
            else
            {
                songUrl = urls.songUrl[songName].url;
                soloNumber = urls.songUrl[songName].soloNumber;
            }

            string url;
            string updateType;

            // 未設定にする
            if (module1Name.Equals("未設定") || module2Name.Equals("未設定"))
            {
                // 楽曲共通
                if (soloNumber == -1)
                {
                    web.HttpPost(URL_MODULE_INIT_COMMON);
                }
                // 楽曲別
                else
                {
                    url = string.Format(URL_MODULE_INIT, songUrl);
                    web.HttpPost(url);
                }
            }
            // 設定する
            else
            {
                string module1Url = urls.moduleUrl[module1Name].url;
                string module2Url;

                // ソロ
                if (soloNumber == 0)
                {
                    updateType = "update";
                    url = string.Format(URL_MODULE_BASE, updateType, songUrl, "vocal1", module1Url);

                    // POST通信
                    web.HttpPost(url);
                }
                // 衣装チェンジ
                else if (soloNumber == 1)
                {
                    module2Url = urls.moduleUrl[module2Name].url;
                    int module1CharNo = urls.moduleUrl[module1Name].charNo;
                    int module2CharNo = urls.moduleUrl[module2Name].charNo;
                    updateType = "updateDuet";

                    // 楽曲選択URLにアクセス＠これをしないとエラーになる。
                    url = string.Format(URL_MODULE_INFO, songUrl);
                    web.HttpGet(url);

                    url = string.Format(URL_MODULE_CONFIRM, songUrl, "vocal1", module1Url, module1CharNo);
                    web.HttpGet(url);

                    url = string.Format(URL_MODULE_CONFIRM, songUrl, "vocal2", module2Url, module2CharNo);
                    string source = web.HttpGet(url)[0];

                    // 送信できる＠ボタンの有無をチェック
                    if (isSongSettingButton(web, source))
                    {
                        url = string.Format(URL_MODULE_BASE, updateType, songUrl, module1Url, module2Url);
                        web.HttpPost(url);
                    }
                    else
                    {
                        MessageBox.Show(MessageConst.E_MSG_0013, MessageConst.E_MSG_ERROR_T);
                    }
                }
                // デュエット
                else if (soloNumber == 2)
                {
                    module2Url = urls.moduleUrl[module2Name].url;
                    updateType = "updateDuet";

                    // 楽曲選択URLにアクセス＠これをしないとエラーになる。
                    url = string.Format(URL_MODULE_INFO, songUrl);
                    web.HttpGet(url);
                    url = string.Format(URL_MODULE_BASE, updateType, songUrl, module1Url, module2Url);
                    web.HttpPost(url);
                }
                // 楽曲共通＠ただ2回設定するだけ
                else
                {
                    module2Url = urls.moduleUrl[module2Name].url;

                    updateType = "update";

                    // モジュール１設定
                    url = string.Format(URL_MODULE_BASE, updateType, songUrl, "vocal1", module1Url);

                    // POST通信
                    web.HttpPost(url);

                    // モジュール２設定
                    url = string.Format(URL_MODULE_BASE, updateType, songUrl, "vocal2", module2Url);

                    // POST通信
                    web.HttpPost(url);
                }
            }
        }

        /*
         * ボタン音設定
         */
        public static void songSettingSetButton(WebData web, string songName, string buttonName, UrlData urls)
        {
            string setUrl;
            string songUrl;
            string buttonUrl;

            if (songName.Equals("楽曲共通"))
            {
                songUrl = "COMMON";
            }
            else
            {
                songUrl = urls.songUrl[songName].url;
            }


            if (buttonName.Equals("未設定"))
            {
                setUrl = "unset";
                buttonUrl = "0";
            }
            else if (buttonName.Equals("共通ボタン音設定無効"))
            {
                setUrl = "invalidateCommonSetting";
                buttonUrl = "0";
            }
            else
            {
                setUrl = "update";

                // ボタンURL+"/0"
                StringBuilder sb = new StringBuilder();
                sb.Append(urls.buttonUrl[buttonName]);
                sb.Append("/");
                sb.Append("0");

                buttonUrl = sb.ToString();

            }

            string url = string.Format(URL_BUTTON_BASE, setUrl, songUrl, buttonUrl);

            // POST通信
            web.HttpPost(url);
        }

        /*
         * スキン設定
         */
        public static void songSettingSetSkin(WebData web, string songName, string skinName, UrlData urls)
        {
            string setUrl;
            string songUrl;
            string skinUrl;

            if (songName.Equals("楽曲共通"))
            {
                songUrl = "COMMON";
            }
            else
            {
                songUrl = urls.songUrl[songName].url;
            }


            if (skinName.Equals("未設定"))
            {
                setUrl = "unset";
                skinUrl = "0";
            }
            else if (skinName.Equals("使用しない"))
            {
                setUrl = "noUse";
                skinUrl = "0";
            }
            else
            {
                setUrl = "update";

                // スキンURL+"/"+スキングループURL
                StringBuilder sb = new StringBuilder();
                sb.Append(urls.skinUrl[skinName].skinUrl);
                sb.Append("/");
                sb.Append(urls.skinUrl[skinName].skinGroupUrl);

                skinUrl = sb.ToString();

            }

            string url = string.Format(URL_SKIN_BASE, setUrl, songUrl, skinUrl);

            // POST通信
            web.HttpPost(url);
        }

        /*
         * 指定したURLにSubmitボタンがあるか返す
         */
        public static bool isSongSettingButton(WebData web, string url)
        {
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // submit取得
            HtmlElementCollection input = html.GetElementsByTagName("input");

            return input.Count > 0;
        }

        /*
         * 楽曲別設定書き込み
         */
        public static void writeSongSettingData(Player player, Dictionary<string, SongSettingData> settings)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + player.accessCode);

            StringBuilder buf = new StringBuilder();

            foreach (string name in settings.Keys)
            {
                buf.Append(settings[name].ToString());
            }

            // プレイヤー情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_SONG_SETTING_DATA,
                false
            );
        }

        /*
         * 楽曲別設定読み込み
         */
        public static Dictionary<string, SongSettingData> readSongSettingData(Player player)
        {
            // プレイヤー情報生成
            Dictionary<string, SongSettingData> ret = new Dictionary<string, SongSettingData>();

            string path = "./" + SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_SONG_SETTING_DATA;

            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                // 生成したインスタンスを返す
                return ret;
            }

            // ファイルを開く
            using (StreamReader sr = new StreamReader(
                path,
                SettingConst.FILE_ENCODING
            ))
            {

                string line;

                // ファイルの末尾まで
                while ((line = sr.ReadLine()) != null)
                {
                    SongSettingData setting = new SongSettingData(line);

                    // リストに追加
                    ret.Add(setting.name, setting);
                }
            }

            return ret;
        }
    }
}
