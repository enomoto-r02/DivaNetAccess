using DivaNetAccess.src.Const;
using DivaNetAccess.src.Song;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DivaNetAccess.src
{
    public static class RivalLogic
    {
        // ライバル情報ベース
        //private static readonly string URL_RIVAL_BASE = "http://project-diva-ac.net/divanet/rival/mainRival/";

        // ライバルコード検索ベース
        private static readonly string URL_RIVAL_CODE_SEARCH = "http://project-diva-ac.net/divanet/rival/searchRivalCode?rivalCode={0}";

        // ライバル_クリア統計情報ベース
        private static readonly string URL_RIVAL_CLEAR_TOUKEI_BASE = "http://project-diva-ac.net/divanet/statistics/indexother/";

        // ライバル_楽曲リスト(プレイ情報確認)ベース
        private static readonly string URL_RIVAL_SONG_LIST_BASE = "http://project-diva-ac.net/divanet/pv/sortOther/";

        // ライバル_クリア統計詳細情報ページ
        private static readonly string URL_RIVAL_SONG_LIST_TOUKEI = "http://project-diva-ac.net/divanet/statistics/detailother/";

        // ライバル_クリア統計詳細情報_クリア状況ベース
        private static readonly string URL_RIVAL_SONG_LIST_TOUKEI_CLEAR_BASE = "http://project-diva-ac.net/divanet/statistics/selectByClearKindOther/";

        // ライバル_クリア統計詳細情報_トライアル状況ベース
        private static readonly string URL_RIVAL_SONG_LIST_TOUKEI_TRIAL_BASE = "http://project-diva-ac.net/divanet/statistics/selectByTrialClearKindOther/";

        // ライバル_楽曲詳細URL
        private static readonly string URL_RIVAL_SONG_DETAIL_BASE = "http://project-diva-ac.net/divanet/pv/infoOther/{0}/{1}/0/0";

        // ライバル_プレイデータ比較ベース
        //private static readonly string URL_RIVAL_SONG_COMPARE_BASE = "http://project-diva-ac.net/divanet/statistics/compare/";

        // ライバル_ランクインリストベース
        private static readonly string URL_RIVAL_RAINKING_BASE = "http://project-diva-ac.net/divanet/ranking/listOther/";

        // ライバル_タグベース
        private static readonly string URL_RIVAL_TAG_BASE = "http://project-diva-ac.net/divanet/rival/searchByTagName/";

        // ライバル_プロフィール表示
        //private static readonly string URL_RIVAL_TWITTER_PROFILE = "http://project-diva-ac.net/divanet/twitter/profile/";

        #region ライバル情報取得メイン

        /*
         * ライバル情報取得メイン
         */
        public static bool getRivalDataMain(string accessCode, string password, string rivalCode, UrlData urls, MainForm form, Dictionary<string, SongData> playerSongs, Dictionary<string, SongData> oldRivalSongs)
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
                return false;
            }

            // ライバル楽曲情報作成
            Dictionary<string, SongData> rivalSongs = new Dictionary<string, SongData>();

            // ランクイン情報生成
            Dictionary<string, RankingData> rankings = new Dictionary<string, RankingData>();

            // ライバル情報クラス生成
            Rival rival = new Rival();
            rival.rivalCode = rivalCode;

            // ライバルURL取得
            getRivalData(web, form, rival);

            // 公開設定されていない
            if (string.IsNullOrEmpty(rival.url))
            {
                MessageBox.Show("相手のライバル情報が公開されていません。\nまたは自分のライバル情報を設定していません。", "失敗");

                DivaNetLogic.afterInit(form);
                return false;
            }

            // ランクイン情報取得
            if (rival.koukaiDetails[2])
            {
                DivaNetLogic.getRankingData(web, 0, rankings, form, URL_RIVAL_RAINKING_BASE + rival.url, false);
            }

            // ライバル楽曲情報取得
            if (rival.koukaiDetails[1])
            {
                // 楽曲リスト(プレイ情報確認)から取得＠暫定順位も同時に取れる
                rivalSongs = getRivalSong(web, urls, form, rival, rankings);
            }
            else if (rival.koukaiDetails[0])
            {
                // クリア統計情報から取得
                rivalSongs = getRivalSongToukei(web, urls, form, rival, playerSongs);

                // クリア統計情報の暫定順位を取得
                getZanteiJuniMainToukei(web, form, rival, rankings);
            }
            else
            {
                MessageBox.Show("楽曲情報が公開されていません。", "失敗");

                DivaNetLogic.afterInit(form);
                return false;
            }

            // ライバル情報書き込み処理
            writeRivalData(rival);

            // 削除前楽曲があればマージ
            DivaNetLogic.mergeSongs(oldRivalSongs, ref rivalSongs);

            // 楽曲情報書き込み処理
            writeSongData(rival, rivalSongs);

            // ランクイン情報書き込み処理
            writeRivalRankingData(rival, rankings);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0002 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return true;
        }

        #endregion

        #region ライバル楽曲情報取得

        /*
         * ライバル楽曲情報取得＠楽曲リストから取得
         */
        private static Dictionary<string, SongData> getRivalSong(WebData web, UrlData urls, MainForm form, Rival rival, Dictionary<string, RankingData> rankings)
        {
            Dictionary<string, SongData> songs = new Dictionary<string, SongData>();

            // 楽曲数分
            foreach (string songName in urls.songUrl.Keys)
            {
                form.Text = SettingConst.WINDOW_TITLE + " - 楽曲取得中...[" + urls.songUrl[songName].name + "]";

                SongData song = new SongData();
                song.name = urls.songUrl[songName].name;
                songs.Add(song.name, song);

                // 楽曲詳細取得＠SongLogicのメソッドを呼び出す
                string url = string.Format(URL_RIVAL_SONG_DETAIL_BASE, rival.url, urls.songUrl[songName].url);
                SongLogic.getSongDetail(web, songs[songName], rankings, url);
            }

            return songs;
        }

        #endregion

        #region ライバル楽曲情報取得＠クリア統計情報から取得

        /*
         * ライバル楽曲情報取得＠クリア統計情報から取得
         */
        private static Dictionary<string, SongData> getRivalSongToukei(WebData web, UrlData urls, MainForm form, Rival rival, Dictionary<string, SongData> playerSongs)
        {
            Dictionary<string, SongData> rivalSongs = new Dictionary<string, SongData>();

            // ★マージ
            margeStar(playerSongs, rivalSongs);

            // クリア統計情報ページのリンク取得
            List<string> linkUrls = new List<string>();
            getRivalSongToukeiLinks(web, form, rival, linkUrls);

            // クリアURLが最初に来るようにソートする
            linkUrls.Sort();

            foreach (string linkUrl in linkUrls)
            {
                string diff = DivaNetUtil.getDiffFromClearToukeiDetails(linkUrl);
                string clear = "";
                string trial = "";

                bool isClearOrTrialPage = false;

                // クリア
                if (linkUrl.StartsWith(URL_RIVAL_SONG_LIST_TOUKEI_CLEAR_BASE))
                {
                    clear = DivaNetUtil.getClearFromClearToukeiDetails(linkUrl);
                    isClearOrTrialPage = true;

                    form.Text = SettingConst.WINDOW_TITLE + " - 楽曲取得中...[クリア状況][" + diff + "]";
                }
                else if (linkUrl.StartsWith(URL_RIVAL_SONG_LIST_TOUKEI_TRIAL_BASE))
                {
                    trial = DivaNetUtil.getTrialFromClearToukeiDetails(linkUrl);
                    isClearOrTrialPage = true;

                    form.Text = SettingConst.WINDOW_TITLE + " - 楽曲取得中...[トライアル状況][" + diff + "]";
                }

                if (isClearOrTrialPage)
                {
                    getSongDetailToukei(web, form, rivalSongs, linkUrl, diff, clear, trial, 0);
                }
            }

            return rivalSongs;
        }

        #endregion

        #region プレイヤーの楽曲情報から情報(★)をマージする＠暫定対応

        /*
         * プレイヤーの楽曲情報から情報(★)をマージする＠暫定対応
         */
        private static void margeStar(Dictionary<string, SongData> playerSongs, Dictionary<string, SongData> rivalSongs)
        {
            foreach (string songName in playerSongs.Keys)
            {
                SongData rivalSong = new SongData();
                rivalSong.name = songName;

                foreach (string diff in playerSongs[songName].data.Keys)
                {
                    ResultData rivalResult = new ResultData();
                    ResultData playerResult = playerSongs[songName].data[diff];

                    // プレイヤー楽曲情報から難易度を取得する
                    rivalResult.star = playerResult.star;
                    rivalResult.diff = diff;

                    rivalSong.data.Add(rivalResult.diff, rivalResult);
                }

                // 楽曲を追加
                rivalSongs.Add(songName, rivalSong);
            }
        }

        #endregion

        #region クリア統計情報ページのリンク取得

        /*
         * クリア統計情報ページのリンク取得
         */
        private static List<string> getRivalSongToukeiLinks(WebData web, MainForm form, Rival rival, List<string> linkUrls)
        {

            // HTMLドキュメント取得
            string[] res = web.HttpPost(URL_RIVAL_SONG_LIST_TOUKEI + rival.url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // クリア状況URLまたはトライアル状況URLか
                if (url.StartsWith(URL_RIVAL_SONG_LIST_TOUKEI_CLEAR_BASE)
                 || url.StartsWith(URL_RIVAL_SONG_LIST_TOUKEI_TRIAL_BASE))
                {
                    // ページ情報を切る＠再帰用にするため
                    linkUrls.Add(url.Substring(0, url.Length - 1));
                    //linkUrls.Add(url);
                }
            }

            return linkUrls;
        }

        #endregion

        #region クリア統計詳細情報ページ_楽曲一覧ページ取得

        /*
         * クリア統計詳細情報ページ_楽曲一覧ページ取得
         */
        private static void getSongDetailToukei(WebData web, MainForm form, Dictionary<string, SongData> songs,
            string url, string diff, string clear, string trial, int page)
        {
            // HTMLドキュメント取得
            string[] res = web.HttpPost(url + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElementCollection tbodys = html.GetElementsByTagName("tbody");

            // テーブルから情報取得
            HtmlElementCollection trs = tbodys[0].GetElementsByTagName("tr");

            // trタグ全検索
            for (int i = 1; i < trs.Count; i++)
            {
                // 楽曲名
                HtmlElement font = trs[i].GetElementsByTagName("FONT")[0];
                string name = font.InnerText;

                // プレイヤーが取得していない楽曲の時
                if (!songs.ContainsKey(name))
                {
                    continue;
                }

                // 汎用
                string tmp = "";
                string[] tmpArray;

                // tdタグ取得
                HtmlElementCollection tds = trs[i].GetElementsByTagName("TD");

                // 達成率
                tmp = tds[1].InnerText;
                tmpArray = tmp.Replace("%", "").Split('.');
                songs[name].data[diff].tasseiritu = int.Parse(tmpArray[0] + tmpArray[1].PadRight(2, '0'));

                // スコア
                tmp = tds[2].InnerText;
                tmp = tmp.Replace("pts", "");
                songs[name].data[diff].score = int.Parse(tmp);

                // クリア
                if (!string.IsNullOrEmpty(clear))
                {
                    songs[name].data[diff].clear = clear;
                }

                // トライアル
                if (!string.IsNullOrEmpty(trial))
                {
                    songs[name].data[diff].trial = trial;
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getSongDetailToukei(web, form, songs, url, diff, clear, trial, page + 1);
            }
        }

        #endregion

        #region ライバル情報取得

        /*
         * ライバル情報取得
         */
        private static void getRivalData(WebData web, MainForm form, Rival rival)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - ライバル情報取得中...";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(string.Format(URL_RIVAL_CODE_SEARCH, rival.rivalCode));
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            HtmlElement body = html.GetElementsByTagName("body")[0];

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // クリア統計情報
                if (url.StartsWith(URL_RIVAL_CLEAR_TOUKEI_BASE))
                {
                    rival.url = url.Split('/')[6];
                    rival.koukaiDetails[0] = true;
                }

                // 楽曲リスト(プレイ情報確認)
                else if (url.StartsWith(URL_RIVAL_SONG_LIST_BASE))
                {
                    if (string.IsNullOrEmpty(rival.url)) { rival.url = url.Split('/')[6]; }
                    rival.koukaiDetails[1] = true;
                }

                // ランクインリスト
                else if (url.StartsWith("http://project-diva-ac.net/divanet/ranking/listOther/"))
                {
                    rival.koukaiDetails[2] = true;
                }

                // タグ
                else if (url.StartsWith(URL_RIVAL_TAG_BASE))
                {
                    rival.tags.Add(html.Links[i].InnerText);
                }

                // Twitter_プロフィール表示
                /*
                else if (url.StartsWith(URL_RIVAL_TWITTER_PROFILE))
                {
                    // TwitterのURL取得
                    string[] twitterHtml = web.HttpPost(url);
                    rival.twitterProfileUrl = twitterHtml[0];
                }
                */
            }

            // 文字列を全て取得する
            string[] innerTexts = WebUtil.getInnerTextReplace(body.InnerText);

            for (int i = 0; i < innerTexts.Length; i++)
            {
                // プレイヤー名
                if (innerTexts[i].StartsWith("[プレイヤー名]"))
                {
                    rival.name = innerTexts[i + 1];
                }

                // 称号
                else if (innerTexts[i].StartsWith("[LEVEL/称号]"))
                {
                    string tmp = innerTexts[i + 1];
                    rival.rank = tmp.Split(' ')[1];
                }

                // ライバルに設定されている人数
                else if (innerTexts[i].StartsWith("[ライバルに設定されている人数]"))
                {
                    string tmp = innerTexts[i + 1].Replace("人", "");
                    rival.setRival = int.Parse(tmp);
                }

                // 気になるプレイヤーに登録されている人数
                else if (innerTexts[i].StartsWith("[気になるプレイヤーに登録されている人数]"))
                {
                    string tmp = innerTexts[i + 1].Replace("人", "");
                    rival.setInterested = int.Parse(tmp);
                }

                // 自己PR
                else if (innerTexts[i].StartsWith("[自己PR]"))
                {
                    // 未設定か設定ありで添字の位置が違う
                    if (!string.IsNullOrEmpty(innerTexts[i + 1]))
                    {
                        rival.pr = innerTexts[i + 1];
                    }
                    else
                    {
                        rival.pr = innerTexts[i + 2];
                    }
                }

                // タグ
                else if (innerTexts[i].StartsWith("[タグ]"))
                {
                    // URLから取っていなければ"未設定"を設定
                    if (rival.tags.Count == 0)
                    {
                        rival.tags.Add(innerTexts[i + 1]);
                    }
                }

                // Twitter連動公開設定
                else if (innerTexts[i].StartsWith("[Twitter連動設定]") || innerTexts[i].StartsWith("[Twitter連動公開設定]"))
                {
                    rival.twitterConnect = innerTexts[i + 1];
                }

                // 他プレイヤーからの勝利宣言受付設定
                else if (innerTexts[i].StartsWith("[他プレイヤーからの勝利宣言受付設定]"))
                {
                    rival.winAnnounce = innerTexts[i + 1];
                }
            }

            // 最終更新日
            rival.getDate = DateTime.Now;
        }

        #endregion

        #region クリア統計詳細情報ページ_暫定順位ページ取得

        /*
         * クリア統計詳細情報ページ_暫定順位ページ取得
         */
        private static void getZanteiJuniMainToukei(WebData web, MainForm form, Rival rival, Dictionary<string, RankingData> rankings)
        {
            // めんどいしURLはハードコーディングでいいや。。
            string ZANTEI_JUNI_BASE_URL = "http://project-diva-ac.net/divanet/statistics/interimRankOther/{0}/{1}/";

            // 暫定順位ページがあるのはHARD、EXTREME、EX EXTREMEのみ
            List<int> diffIndexList = new List<int>();
            diffIndexList.Add(2);
            diffIndexList.Add(3);
            diffIndexList.Add(4);

            foreach (int diffIndex in diffIndexList)
            {
                string diffStr = ToukeiData.DIFF_STR[diffIndex];
                if (diffIndex == 4)  // EX EXTは文字が違うので暫定
                {
                    diffStr = "EXTRA_EXTREME";
                }

                string url = string.Format(ZANTEI_JUNI_BASE_URL, rival.url, diffStr);

                getZanteiJuniDetailToukei(web, form, rankings, url, diffStr, 0);
            }
        }

        #endregion

        #region クリア統計詳細情報ページ_暫定順位ページ取得

        /*
         * クリア統計詳細情報ページ_暫定順位ページ取得
         */
        private static void getZanteiJuniDetailToukei(WebData web, MainForm form, Dictionary<string, RankingData> rankings,
            string url, string diff, int page)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - スコア暫定順位一覧取得中...[" + diff + "]";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(url + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // ページ内のtbodyタグを全て取得する
            HtmlElementCollection tbodys = html.GetElementsByTagName("tbody");

            // テーブルから情報取得
            HtmlElementCollection trs = tbodys[0].GetElementsByTagName("tr");

            // trタグ全検索
            for (int i = 1; i < trs.Count; i++)
            {
                HtmlElementCollection tds = trs[i].GetElementsByTagName("td");

                // 未クリアの暫定順位は取得しない
                int rank;
                if (int.TryParse(tds[1].InnerText.Trim().Replace("位", ""), out rank) == false)
                {
                    continue;
                }

                // ランキング情報を設定
                RankingData ranking = new RankingData();

                ranking.name = tds[0].InnerText.Trim();
                ranking.diff = diff;
                ranking.rank = rank;    // TryParseで変換した順位をそのままセット
                ranking.score = int.Parse(tds[2].InnerText.Trim());

                // 順位が入っていなければ暫定順位を設定
                string key = ranking.diff + ranking.name;

                if (rankings.ContainsKey(key) == false)
                {
                    rankings.Add(key, ranking);
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getZanteiJuniDetailToukei(web, form, rankings, url, diff, page + 1);
            }
        }

        #endregion

        #region 楽曲情報書き込み(ライバル)

        /*
         * 楽曲情報書き込み(ライバル)
         */
        public static void writeSongData(Rival rival, Dictionary<string, SongData> songs)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode);

            StringBuilder buf = new StringBuilder();

            foreach (string key in songs.Keys)
            {
                buf.Append(songs[key].ToString());
            }

            // 楽曲情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode + "/" + SettingConst.FILE_SONG_DATA,
                false
            );
        }

        #endregion

        #region 楽曲情報読み込み

        /*
         * 楽曲情報読み込み
         */
        public static Dictionary<string, SongData> readSongData(Rival rival)
        {
            string path = SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode + "/" + SettingConst.FILE_SONG_DATA;

            // 楽曲情報ファイルが無い時はnullを返す
            if (File.Exists(path) == false)
            {
                return null;
            }

            // 楽曲リスト
            Dictionary<string, SongData> ret = new Dictionary<string, SongData>();

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
                    // 楽曲情報生成
                    SongData song = new SongData(line);

                    // リストに追加
                    ret.Add(song.name, song);
                }
            }

            return ret;
        }

        #endregion

        /*
         * ライバル情報書き込み
         */
        public static void writeRivalData(Rival rival)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode);

            // 楽曲情報書き込み
            FileUtil.writeFile(
                rival.ToString(),
                SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode + "/" + Player.FILE_NAME,
                false
            );
        }

        /*
         * ライバルリスト読み込み
         */
        public static List<string[]> readRivalList()
        {
            List<string[]> ret = new List<string[]>();

            // フォルダが無い
            if (Directory.Exists("./" + SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME) == false)
            {
                return ret;
            }

            // フォルダ一覧取得
            string[] dirNames = Directory.GetDirectories("./" + SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME);

            // フォルダ分ループ
            foreach (string dirName in dirNames)
            {
                // フォルダ名の形式チェック
                var reg = new Regex(@"\.\/data\/rival\\[0-9a-zA-Z]{10}$");
                if (!reg.IsMatch(dirName))
                {
                    continue;
                }

                string path = dirName + "/" + Player.FILE_NAME;

                // プレイヤー情報ファイル確認
                if (File.Exists(path) == false)
                {
                    // ファイルが見つからなければスキップ
                    continue;
                }

                // ファイルを開く
                using (StreamReader sr = new StreamReader(
                    path,
                    SettingConst.FILE_ENCODING
                ))
                {
                    string[] rivals = new string[2];

                    // ライバルコードを設定
                    rivals[0] = sr.ReadLine();

                    // ライバル名を設定
                    rivals[1] = sr.ReadLine();

                    ret.Add(rivals);
                }
            }

            // ライバル名順にソート
            ret.Sort((a, b) =>
            {
                int result = a[1].CompareTo(b[1]);
                return result;
            });

            return ret;
        }

        /*
         * ライバル情報読み込み
         */
        public static Rival readRivalData(string rivalCode)
        {
            string path = "./" + SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rivalCode + "/" + Player.FILE_NAME;

            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                return null;
            }

            // ライバル情報生成
            Rival rival;

            // ファイルを開く
            using (StreamReader sr = new StreamReader(
                path,
                SettingConst.FILE_ENCODING
            ))
            {
                // ファイルをすべて読み込む
                string fileStr = sr.ReadToEnd();

                // プレイヤー情報設定
                rival = new Rival(fileStr.Split('\n'));
            }

            return rival;
        }

        /*
         * ランクイン情報書き込み
         */
        public static void writeRivalRankingData(Rival rival, Dictionary<string, RankingData> rankings)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode);

            string path = SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode + "/" + SettingConst.FILE_RANKING_DATA;

            StringBuilder buf = new StringBuilder();

            foreach (string key in rankings.Keys)
            {
                buf.Append(rankings[key].ToString());
            }

            // 楽曲情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                path,
                false
            );
        }

        /*
         * ランクイン情報読み込み
         */
        public static Dictionary<string, RankingData> readRankingData(Rival rival)
        {
            string path = SettingConst.DATA_DIR_NAME + "/" + SettingConst.RIVAL_DIR_NAME + "/" + rival.rivalCode + "/" + SettingConst.FILE_RANKING_DATA;

            Dictionary<string, RankingData> ret = new Dictionary<string, RankingData>();

            // ランキング情報が無い時はインスタンスを生成して終了
            if (File.Exists(path) == false)
            {
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
                    RankingData ranking = new RankingData(line);

                    // リストに追加
                    ret.Add(ranking.diff + ranking.name, ranking);
                }
            }

            return ret;
        }
    }
}
