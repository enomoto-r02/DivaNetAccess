using DivaNetAccess.src.Const;
using DivaNetAccess.src.Song;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DivaNetAccess.src.util
{
    // DivaNetアクセスクラス
    public static class DivaNetLogic
    {
        // モジュール分類用charNo
        public static readonly string[] VOCALOID_NAME = { "初音ミク", "鏡音リン", "鏡音レン", "巡音ルカ", "MEIKO", "KAITO", "派生キャラ" };

        /*
         * 後処理
         */
        public static void afterInit(MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE;
        }

        /*
         * 楽曲取得ボタン
         */
        public static int getDataMain(Player player, UrlData urls, MainForm form, Dictionary<string, SongData> oldPlayerSongs)
        {
            // ストップウォッチ生成
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // Web情報生成
            WebData web = new WebData(player);

            // ログインチェック
            if (DivaNetLogic.isLogin(web, form) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0002, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return -1;
            }

            // プレイヤー情報取得
            getPlayerData(web, player, form);

            /*
            // 利用権チェック
            if(player.isAuthorization == false){
                MessageBox.Show(MessageConst.E_MSG_0007, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return -1;
            }
            */

            Dictionary<string, RankingData> rankings = new Dictionary<string, RankingData>();

            // 楽曲取得メイン
            Dictionary<string, SongData> songs = SongLogic.getSongMain(web, urls, rankings, form);

            // ランクイン情報取得＠達成率ランキングのみ
            getRankingData(web, 0, rankings, form, UrlConst.URL_RANKING_BASE, true);

            // ランクイン情報書き込み処理
            DivaNetUtil.writeRankingData(player, rankings);

            // 楽曲情報マージ処理
            DivaNetLogic.mergeSongs(oldPlayerSongs, ref songs);

            // 楽曲情報書き込み処理
            SongLogic.writeSongData(player, songs);

            // プレイヤー情報書き込み処理
            DivaNetUtil.writePlayerData(player);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0002 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return 0;
        }

        /*
         * ログインチェック
         */
        public static bool isLogin(WebData web, MainForm form, Player player)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - ログイン処理中...";

            Boolean ret = true;

            // ログインページへアクセスする
            string[] res = web.HttpPost(UrlConst.URL_LOGIN);

            // アクセス後のURLチェック
            if (res[0].Contains("/login/"))
            {
                ret = false;
            }

            return ret;
        }

        /*
         * ログインチェック
         */
        public static bool isLogin(WebData web, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - ログイン処理中...";

            Boolean ret = true;

            // ログインページへアクセスする
            string[] res = web.HttpPost(UrlConst.URL_LOGIN);

            // アクセス後のURLチェック
            if (res[0].Contains("/login/"))
            {
                ret = false;
            }

            return ret;
        }

        /*
         * 利用権チェック
         * 　True：利用権あり、False：利用権なし
         */
        /*
        public static bool isAuthorization(webData web, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - DIVA.NET利用権確認中...";

            bool ret = false;

            string[] res = web.HttpPost(UrlConst.URL_MENU);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // タグを全て取得する
            HtmlElementCollection bodys = html.GetElementsByTagName("body");

            string[] values = bodys[0].InnerText.Split('\n');
            for (int i = 0; i < values.Length; i++)
            {
                // 利用権
                if (values[i].StartsWith("[利用権"))
                {
                    return (values[i].IndexOf('◎') != -1);
                }
            }

            // この終わり方はないと思うけど…。
            return ret;
        }
        */

        /*
         * プレイヤー情報取得
         */
        private static void getPlayerData(WebData web, Player player, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - プレイヤー情報取得中...";

            string[] res = web.HttpPost(UrlConst.URL_MENU);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // タグを全て取得する
            HtmlElementCollection bodys = html.GetElementsByTagName("body");

            string[] values = WebUtil.getInnerTextReplace(bodys[0].InnerText);

            for (int i = 0; i < values.Length; i++)
            {
                // プレイヤー名
                if (values[i].StartsWith("[プレイヤー名]"))
                {
                    player.name = values[i + 1].TrimEnd();
                }

                // 称号
                if (values[i].StartsWith("[LEVEL/称号]"))
                {
                    string tmp = values[i + 1];
                    player.rank = tmp.Split(' ')[1];
                }

                // VP
                if (values[i].StartsWith("[VOCALOID POINT]"))
                {
                    string tmp = values[i + 1];
                    tmp = tmp.Replace("VP", "");
                    player.vp = int.Parse(tmp.TrimEnd());
                }

                // チケット
                if (values[i].StartsWith("[DIVAチケット]"))
                {
                    string tmp = values[i + 1];
                    tmp = tmp.Replace("枚", "");
                    player.ticket = int.Parse(tmp.TrimEnd());
                }

                // ライバルコード
                if (values[i].StartsWith("[ライバルコード"))
                {
                    player.rivalCode = values[i + 1].TrimEnd();
                }

                /*
                // 利用期限
                if (values[i].StartsWith("利用期限："))
                {
                    string tmp = values[i].Replace("利用期限：", "");
                    player.limit = DateTime.Parse(tmp.TrimEnd());
                }

                // 利用権
                if (values[i].StartsWith("[利用権"))
                {
                    player.isAuthorization = (values[i].IndexOf('◎') != -1);
                }
                */
            }

            // 最終更新日
            player.getDate = DateTime.Now;
        }

        /*
         * ランクイン情報取得＠すべて取得
         */
        public static void getRankingData(WebData web, int page, Dictionary<string, RankingData> rankings,
            MainForm form, string url, bool isTasseirituOnly)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - ランクイン情報取得中...[" + (page + 1) + "ページ]";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(url + "/" + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElementCollection tbodys = html.GetElementsByTagName("tbody");

            // tbodyタグ全検索
            foreach (HtmlElement tbody in tbodys)
            {
                #region ランキングの種類判定

                bool isTasseirituRanking = false;
                bool isHighScoreRanking = false;
                HtmlElementCollection trs;


                // テーブルから情報取得
                trs = tbody.GetElementsByTagName("tr");

                // ランキング情報あり
                if (trs.Count > 0)
                {
                    // ヘッダの数から判断
                    if ((trs[0].GetElementsByTagName("td").Count == 3) && (page == 0))
                    {
                        isTasseirituRanking = true;
                    }
                    else if (trs[0].GetElementsByTagName("td").Count == 1)
                    {
                        isHighScoreRanking = true;
                    }
                }

                #endregion

                #region 達成率ランキング

                if (isTasseirituRanking)
                {
                    // ヘッダ以外を処理
                    for (int trCount = 1; trCount < trs.Count; trCount++)
                    {
                        // tdタグ取得
                        HtmlElementCollection tds = trs[trCount].GetElementsByTagName("td");

                        RankingData ranking = new RankingData();
                        ranking.name = "達成率ランキング";
                        ranking.diff = tds[0].InnerText.Trim();     // 空白が入る？のでTrim
                        ranking.score = int.Parse(tds[1].InnerText.Replace("%", "").Replace(".", ""));
                        ranking.rank = int.Parse(tds[2].InnerText.Replace("位", ""));

                        rankings.Add(ranking.diff + ranking.name, ranking);
                    }
                }

                // 達成率ランキングのみなら1ページで終了
                if (isTasseirituOnly)
                {
                    return;
                }

                #endregion

                #region ハイスコアランキング

                if (isHighScoreRanking)
                {
                    // テーブルから情報取得
                    trs = tbody.GetElementsByTagName("tr");

                    // テーブルヘッダをスキップ
                    for (int i = 2; i < trs.Count; i++)
                    {
                        // 曲名判定
                        //if (trs[i].GetElementsByTagName("a").Count > 0)
                        if (trs[i].GetElementsByTagName("td").Count == 1)
                        {
                            // 曲名保持
                            string songName = trs[i].InnerText.Trim();

                            // 難易度タグ用カウンタ
                            int j = i + 1;
                            while (trs.Count > j)    // trsの配列外対策
                            {
                                // 難易度のtrタグ取得
                                HtmlElement trDiff = trs[j];

                                // 難易度がHARDorEXTREME以外(=次の曲名)チェック
                                if (trDiff.InnerText.StartsWith(WebUtil.DIFF_STR[2]) == false &&
                                    trDiff.InnerText.StartsWith(WebUtil.DIFF_STR[3]) == false
                                )
                                {
                                    // 難易度タグのループ終了
                                    break;
                                }

                                // 難易度タグの時
                                string[] trDiffStr = trDiff.InnerText.Split(' ');

                                RankingData ranking = new RankingData();
                                ranking.name = songName;
                                ranking.diff = trDiffStr[0];
                                ranking.score = int.Parse(trDiffStr[1]);
                                ranking.date = DateTime.Parse(trDiffStr[2]);
                                ranking.rank = int.Parse(trDiffStr[3].Replace("位", ""));

                                rankings.Add(ranking.diff + ranking.name, ranking);

                                j++;
                            }
                        }
                    }
                }

                #endregion
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getRankingData(web, page + 1, rankings, form, url, isTasseirituOnly);
            }
        }

        /*
         * ブラウザでログイン処理
         */
        public static int viewBrowserDivaNet(string accessCode, string password, MainForm form)
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
                return -1;
            }

            DivaNetLogic.afterInit(form);

            // パラメータを加えてブラウザでメニューURLを開く
            System.Diagnostics.Process.Start("http://project-diva-ac.net/divanet/menu?" + web.postDataStr);

            return 0;
        }

        /*
         * URL取得ボタンメイン
         */
        public static bool getUrlMain(Player player, MainForm form)
        {
            // ストップウォッチ生成
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // Web情報生成
            WebData web = new WebData(player);

            // ログインチェック
            if (DivaNetLogic.isLogin(web, form, player) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0002, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return false;
            }

            // プレイヤー情報取得
            getPlayerData(web, player, form);

            // 利用権チェック
            /*
            if (player.isAuthorization == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0007, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return false;
            }
            */

            // URL情報生成
            UrlData url = new UrlData();

            //// 楽曲URL取得
            getSongUrlData(web, 0, url.songUrl, form);
            getSongOrderByName2(web, 0, url.songUrl, 1);

            //// モジュールURL取得
            //for (int i = 0; i < charNoArray.Length; i++)      // キャラ数分
            //{
            //    getModuleUrlData(web, i, 0, url.moduleUrl, form);
            //}
            //getModuleUrlDataRandom(web, url.moduleUrl, form);   // ランダムセレクト

            //// 購入済みスキンURL取得
            //getBoughtSkinUrlDataMain(web, 0, url.skinUrl, form);

            //// 未購入スキンURL取得
            //getNotBoughtSkinUrlDataMain(web, 0, url.skinUrl, form);

            //// ボタン音URL取得
            //getButtonUrlDataMain(web, 0, url.buttonUrl, form);

            // プレイヤー情報書き込み処理
            DivaNetUtil.writePlayerData(player);

            // URL情報書き込み処理
            DivaNetUtil.writeUrlData(player, url);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0005 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return true;
        }

        /*
         * 楽曲URL取得
         */
        private static void getSongUrlData(WebData web, int page, Dictionary<string, SongUrlData> songUrls, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - 楽曲取得中...[" + (page + 1) + "ページ]";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(UrlConst.URL_SONG_KOUKAI_BASE + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // 楽曲URLか
                if (url.StartsWith(UrlConst.URL_SONG_DETAIL_BASE))
                {
                    SongUrlData songUrl = new SongUrlData();
                    songUrl.name = html.Links[i].InnerText;
                    songUrl._koukaiOrde = songUrls.Count + 1;
                    songUrl.soloNumber = getSoloNumber(web, url);
                    songUrl.url = DivaNetUtil.convSongUrl(url);

                    songUrls.Add(songUrl.name, songUrl);

#if DEBUG
                    if (songUrls.Count >= 2)
                    {
                        break;
                    }
#else
#endif
                }
            }

            // 次のページを取得
#if DEBUG
#else
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getSongUrlData(web, page + 1, songUrls, form);
            }
#endif
        }

        /*
         * 楽曲種類取得
         */
        private static int getSoloNumber(WebData web, string url)
        {
            int soloNumber;

            // HTMLドキュメント取得
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            HtmlElement body = html.GetElementsByTagName("body")[0];

            string[] innerTexts = body.InnerText.Split('\n');

            foreach (string innerText in innerTexts)
            {

                // 楽曲種類取得＠これだと危ない？
                if (innerText.StartsWith("["))
                {

                    if (innerText.StartsWith("[ソロ]"))
                    {
                        return 0;

                    }
                    else if (innerText.StartsWith("[衣装チェンジ]"))
                    {
                        return 1;
                    }
                    else if (innerText.StartsWith("[デュエット]"))
                    {
                        return 2;
                    }
                }
            }

            // ありえない終わり方
            soloNumber = -1;

            return soloNumber;
        }

        /*
         * 曲名順取得
         */
        private static void getSongOrderByName2(WebData web, int page, Dictionary<string, SongUrlData> songUrls, int songNameCnt)
        {
            // HTMLドキュメント取得
            string[] res = web.HttpPost(UrlConst.URL_SONG_KYOKUMEI_BASE + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // 楽曲URLか
                if (url.StartsWith(UrlConst.URL_SONG_DETAIL_BASE))
                {
                    // 曲名から楽曲情報取得
                    if (songUrls.ContainsKey(html.Links[i].InnerText))
                    {
                        SongUrlData songUrl = songUrls[html.Links[i].InnerText];

                        // 曲名順を設定
                        songUrl._nameOrder = songNameCnt;

                        // 上書きする
                        songUrls[songUrl.name] = songUrl;

                        // インクリメント
                        songNameCnt++;
                    }
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getSongOrderByName2(web, page + 1, songUrls, songNameCnt);
            }
        }

        /*
         * モジュールURL取得(ランダム以外)
         */
        public static void getModuleUrlData(WebData web, int charNo, int page, Dictionary<string, ModuleUrlData> modules, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - モジュール取得中...[" + VOCALOID_NAME[charNo] + "]";

            string[] res = web.HttpPost(UrlConst.URL_MODULE_BASE + charNo + "/" + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            HtmlElementCollection body = html.GetElementsByTagName("a");

            // bodyタグの1行ごとを読み込む
            for (int i = 0; i < body.Count; i++)
            {
                string urlDetail = WebUtil.convUrlToHtml(body[i].GetAttribute("href"));

                if (urlDetail.StartsWith(UrlConst.URL_MODULE_DETAIL_BASE))
                {
                    ModuleUrlData module = new ModuleUrlData();
                    module.name = body[i].InnerText;
                    module.url = DivaNetUtil.convModuleUrl(urlDetail);
                    module.charNo = charNo;                                 // キャラクター番号
                    module.isBought = isModuleBought(web, urlDetail);       // 所持

                    modules.Add(module.name, module);
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getModuleUrlData(web, charNo, page + 1, modules, form);
            }
        }

        /*
         * モジュールが購入済みかチェック
         */
        public static bool isModuleBought(WebData web, string url)
        {
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // submit取得
            HtmlElementCollection input = html.GetElementsByTagName("input");

            return !(input.Count > 0);
        }

        /*
         * モジュールURL取得(ランダム)
         */
        public static void getModuleUrlDataRandom(WebData web, Dictionary<string, ModuleUrlData> modules, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - モジュール取得中...[ランダムセレクト]";

            string[] res = web.HttpPost(UrlConst.URL_MODULE_SET_COMMON_VOCAL1);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // すべてのリンクを読み込む
            for (int i = 0; i < html.Links.Count; i++)
            {
                if (html.Links[i].InnerText.StartsWith("ランダムセレクト"))
                {
                    int charNo = int.Parse(html.Links[i].GetAttribute("href").Split('/')[6]);    // キャラクター番号

                    // URL形式変更
                    string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                    // モジュール設定ページ取得
                    string[] res2 = web.HttpPost(url);
                    HtmlDocument html2 = WebUtil.getHtmlDocument(res2[1]);

                    for (int j = 0; j < html2.Links.Count; j++)
                    {
                        // URL形式変更
                        string url2 = WebUtil.convUrlToHtml(html2.Links[j].GetAttribute("href"));

                        if (url2.StartsWith("http://project-diva-ac.net/divanet/module/confirm/COMMON/vocal1/"))
                        {
                            ModuleUrlData module = new ModuleUrlData();
                            module.name = html2.Links[j].InnerText;
                            module.url = DivaNetUtil.convModuleUrlRandom(url2);
                            module.charNo = charNo;
                            module.isBought = true;         // 所持

                            modules.Add(module.name, module);
                        }
                    }

                    break;
                }
            }
        }

        /*
         * 購入済みスキンリスト取得
         */
        public static void getBoughtSkinUrlDataMain(WebData web, int page, Dictionary<string, SkinUrlData> skins, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - 購入済みスキン取得中...[" + (page + 1) + "ページ]";

            string[] res = web.HttpPost(UrlConst.URL_SKIN_LIST_BASE + page + "/0/");
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // すべてのリンクを読み込む
            for (int i = 0; i < html.Links.Count; i++)
            {
                string link = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                if (link.StartsWith(UrlConst.URL_SKIN_DETAIL_BASE))
                {
                    getSkinUrlDataDetail(web, link, skins, UrlConst.URL_SKIN_BASE, true);
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getBoughtSkinUrlDataMain(web, page + 1, skins, form);
            }
        }

        /*
         * 未購入スキンリスト取得
         */
        public static void getNotBoughtSkinUrlDataMain(WebData web, int page, Dictionary<string, SkinUrlData> skins, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - 未購入スキン取得中...[" + (page + 1) + "ページ]";

            string[] res = web.HttpPost(UrlConst.URL_SKIN_SHOP_LIST_BASE + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // すべてのリンクを読み込む
            for (int i = 0; i < html.Links.Count; i++)
            {
                string link = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                if (link.StartsWith(UrlConst.URL_SKIN_SHOP_DETAIL_BASE))
                {
                    getSkinUrlDataDetail(web, link, skins, UrlConst.URL_SKIN_SHOP_BASE, false);
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getNotBoughtSkinUrlDataMain(web, page + 1, skins, form);
            }
        }

        /*
         * スキンURL取得
         */
        public static void getSkinUrlDataDetail(WebData web, string url, Dictionary<string, SkinUrlData> skins, string SKIN_BASE_URL, bool isBought)
        {
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // すべてのリンクを読み込む
            for (int i = 0; i < html.Links.Count; i++)
            {
                string link = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                if (link.StartsWith(SKIN_BASE_URL))
                {
                    SkinUrlData skin = new SkinUrlData();
                    skin.name = html.Links[i].InnerText;
                    skin.skinGroupUrl = DivaNetUtil.convSkinGroupUrl(link, isBought);
                    skin.skinUrl = DivaNetUtil.convSkinUrl(link, isBought);
                    skin.isBought = isBought;

                    skins.Add(skin.name, skin);
                }
            }
        }

        /*
         * ボタン音URL取得
         */
        public static void getButtonUrlDataMain(WebData web, int page, Dictionary<string, string> buttons, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - ボタン音取得中...[" + (page + 1) + "ページ]";

            string[] res = web.HttpPost(UrlConst.URL_BUTTON_LIST_BASE + page + "/0");
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // すべてのリンクを読み込む
            for (int i = 0; i < html.Links.Count; i++)
            {
                string link = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                if (link.StartsWith(UrlConst.URL_BUTTON_BASE))
                {
                    link = DivaNetUtil.convButtonUrl(link);
                    buttons.Add(html.Links[i].InnerText, link);
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getButtonUrlDataMain(web, page + 1, buttons, form);
            }
        }

        /*
         * マージ処理＠楽曲削除対応
         * oldSongs側にしか無い楽曲を、newSongsにコピーする
         */
        public static void mergeSongs(Dictionary<string, SongData> oldSongs, ref Dictionary<string, SongData> newSongs)
        {
            // 楽曲が読み込めていなかったらスルー＠初回起動回避
            // プレイヤー情報は存在するけど読み込んでいない場合は上書きされてしまうが、それはありえないはず
            if (oldSongs == null)
            {
                return;
            }

            foreach (string songName in oldSongs.Keys)
            {
                if (!newSongs.ContainsKey(songName))
                {
                    newSongs.Add(songName, oldSongs[songName]);
                }
            }
        }
    }
}
