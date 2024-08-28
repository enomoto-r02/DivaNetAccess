using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.Song
{
    public static class SongLogic
    {
        // 楽曲一覧(公開順)
        private static readonly string URL_SONG_KOUKAI_BASE = "http://project-diva-ac.net/divanet/pv/sort/0/true/";

        // 楽曲URL
        private static readonly string URL_SONG_DETAIL_BASE = "http://project-diva-ac.net/divanet/pv/info/";

        // 楽曲詳細URL
        private static readonly string URL_SONG_DETAIL_FORMAT = "http://project-diva-ac.net/divanet/pv/info/{0}/0/0";

        /* 
         * 楽曲取得メイン
         * ※ライバル情報取得のライバル楽曲取得メインとほぼ同じ
         */
        public static Dictionary<string, SongData> getSongMain(WebData web, UrlData urls, Dictionary<string, RankingData> rankings, MainForm form)
        {
            Dictionary<string, SongData> songs = new Dictionary<string, SongData>();

            // 楽曲数分
            foreach (string songName in urls.songUrl.Keys)
            {
                form.Text = SettingConst.WINDOW_TITLE + " - 楽曲取得中...[" + urls.songUrl[songName].name + "]";

                SongData song = new SongData();
                song.name = urls.songUrl[songName].name;
                songs.Add(song.name, song);

                // 楽曲詳細取得
                string url = string.Format(URL_SONG_DETAIL_FORMAT, urls.songUrl[songName].url);
                getSongDetail(web, songs[songName], rankings, url);
            }

            return songs;
        }

        /*
         * 楽曲リスト取得
         */
        private static void getSongList(WebData web, int page, Dictionary<string, SongData> songs)
        {
            // HTMLドキュメント取得
            string[] res = web.HttpPost(URL_SONG_KOUKAI_BASE + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // 楽曲URLか
                if (url.StartsWith(URL_SONG_DETAIL_BASE))
                {
                    // 曲名、公開順、URLを設定
                    SongData song = new SongData();

                    song.name = html.Links[i].InnerText;

                    songs.Add(song.name, song);
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getSongList(web, page + 1, songs);
            }
        }

        /*
         * 楽曲詳細取得
         * ※ライバル情報取得からも呼ばれる
         */
        public static void getSongDetail(WebData web, SongData song, Dictionary<string, RankingData> rankings, string url)
        {
            // HTMLドキュメント取得
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            #region 楽曲情報

            // ページ内のtbodyタグ(各難易度)を全て取得する
            HtmlElementCollection tables = html.GetElementsByTagName("table");

            // tbodyタグ全検索
            foreach (HtmlElement table in tables)
            {

                // テーブルから情報取得
                HtmlElementCollection tds = table.GetElementsByTagName("td");

                // tdタグなし
                if (tds.Count == 0)
                {
                    continue;
                }

                // tdタグの先頭が難易度のいずれでもない場合は終了＠tbodyの先頭が楽曲情報前提
                else if (tds[0].InnerText.StartsWith(WebUtil.DIFF_STR[0]) == false &&
                    tds[0].InnerText.StartsWith(WebUtil.DIFF_STR[1]) == false &&
                    tds[0].InnerText.StartsWith(WebUtil.DIFF_STR[2]) == false &&
                    tds[0].InnerText.StartsWith(WebUtil.DIFF_STR[3]) == false &&
                    tds[0].InnerText.StartsWith(WebUtil.DIFF_STR[4]) == false
                )
                {
                    continue;
                }

                // 汎用
                string tmp = "";
                string[] tmpArray;

                // 難易度クラス生成
                ResultData result = new ResultData();

                // 難易度インデックス
                tmp = tds[0].InnerText.Split('\n')[0];
                tmp = tmp.Replace("\r", "");
                result.diff = tmp;

                // ★
                tmp = tds[0].InnerText.Split('\n')[1];
                tmp = tmp.Replace("\r", "");
                result.star = tmp.Replace("★", "");

                // クリア状況
                int imgCnt = 0;
                for (int i = 3; i <= 6; i++)
                {
                    // イメージタグ数カウント
                    if (tds[i].GetElementsByTagName("img").Count > 0)
                    {
                        imgCnt++;
                    }
                }
                result.clear = WebUtil.CLEAR_RESULT_CHAR[imgCnt];

                // トライアル
                result.trial = WebUtil.getTrialStr(tds[7].InnerText);
                result.trial_max = WebUtil.getTrialRenzokuMax(tds[7].InnerText);
                result.trial_now = WebUtil.getTrialRenzokuNow(tds[7].InnerText);

                // 最高達成率
                tmp = tds[10].InnerText;
                tmpArray = tmp.Replace("%", "").Split('.');
                result.tasseiritu = int.Parse(tmpArray[0] + tmpArray[1].PadRight(2, '0'));

                // 自己ベスト
                tmp = tds[11].InnerText;
                tmp = tmp.Replace("pts", "");
                result.score = int.Parse(tmp);

                // スペシャルクエストクリア状況
                if (tds.Count > 12)
                {
                    tmp = tds[13].InnerHtml;

                    if (tmp.Contains("icon_op_1.jpg"))
                    {
                        result.hispeed = "○";
                        result.hidden = "-";
                        result.sudden = "-";
                    }
                    else if (tmp.Contains("icon_op_2.jpg"))
                    {
                        result.hispeed = "-";
                        result.hidden = "○";
                        result.sudden = "-";
                    }
                    else if (tmp.Contains("icon_op_3.jpg"))
                    {
                        result.hispeed = "-";
                        result.hidden = "-";
                        result.sudden = "○";
                    }
                    else if (tmp.Contains("icon_op_4.jpg"))
                    {
                        result.hispeed = "○";
                        result.hidden = "○";
                        result.sudden = "-";
                    }
                    else if (tmp.Contains("icon_op_5.jpg"))
                    {
                        result.hispeed = "○";
                        result.hidden = "-";
                        result.sudden = "○";
                    }
                    else if (tmp.Contains("icon_op_6.jpg"))
                    {
                        result.hispeed = "-";
                        result.hidden = "○";
                        result.sudden = "○";
                    }
                    else if (tmp.Contains("icon_op_7.jpg"))
                    {
                        result.hispeed = "○";
                        result.hidden = "○";
                        result.sudden = "○";
                    }
                    else
                    {
                        result.hispeed = "-";
                        result.hidden = "-";
                        result.sudden = "-";
                    }
                }

                // 追加
                song.data.Add(result.diff, result);
            }

            #endregion

            #region ランクインテーブル

            // ページ内のtbodyタグ(各難易度)を全て取得する
            tables = html.GetElementsByTagName("table");

            // tbodyタグ全検索
            foreach (HtmlElement table in tables)
            {
                // trタグ取得
                HtmlElementCollection trs = table.GetElementsByTagName("tr");

                if (trs.Count == 0)
                {
                    continue;
                }

                // tdタグ取得
                HtmlElementCollection tds = trs[0].GetElementsByTagName("td");

                if (tds.Count != 4)
                {
                    continue;
                }

                // テーブルのヘッダ確認
                if (
                        (tds[0].InnerText.Equals("難易度") == false)
                     || (tds[1].InnerText.Equals("スコア") == false)
                     || (tds[2].InnerText.Equals("記録日") == false)
                     || (tds[3].InnerText.Equals("順位") == false)
                    )
                {
                    continue;
                }

                for (int trCnt = 1; trs.Count > trCnt; trCnt++)
                {
                    tds = trs[trCnt].GetElementsByTagName("td");

                    // ランキング情報を設定
                    RankingData ranking = new RankingData();

                    ranking.name = song.name;
                    ranking.diff = tds[0].InnerText.Trim();
                    ranking.score = int.Parse(tds[1].InnerText.Trim());
                    ranking.date = DateTime.Parse(tds[2].InnerText.Trim());
                    ranking.rank = int.Parse(tds[3].InnerText.Trim().Replace("位", ""));

                    rankings.Add(ranking.diff + ranking.name, ranking);
                }
            }

            #endregion

            #region スコア暫定全国順位テーブル

            // ページ内のtbodyタグ(各難易度)を全て取得する
            tables = html.GetElementsByTagName("table");

            // tbodyタグ全検索
            foreach (HtmlElement table in tables)
            {
                // theadタグ取得＠theadタグあり=スコア暫定全国順位のテーブルとする
                HtmlElementCollection theads = table.GetElementsByTagName("thead");

                if (theads.Count == 0)
                {
                    continue;
                }

                // trタグ取得
                HtmlElementCollection trs = table.GetElementsByTagName("tbody")[0].GetElementsByTagName("tr");

                // tbodyタグ全検索
                foreach (HtmlElement tr in trs)
                {
                    HtmlElementCollection tds = tr.GetElementsByTagName("td");

                    // ランキング情報を設定
                    RankingData ranking = new RankingData();

                    ranking.name = song.name;
                    ranking.diff = tds[0].InnerText.Trim();
                    ranking.score = int.Parse(tds[1].InnerText.Trim());
                    ranking.rank = int.Parse(tds[2].InnerText.Trim().Replace("位", ""));

                    // ランクイン情報が入っていない＠ハイスコアランキングを優先する
                    string key = ranking.diff + ranking.name;

                    if (rankings.ContainsKey(key) == false)
                    {
                        rankings.Add(key, ranking);
                    }
                }
            }

            #endregion
        }

        /*
         * 楽曲情報書き込み
         */
        public static void writeSongData(Player player, Dictionary<string, SongData> songs)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + player.accessCode);

            StringBuilder buf = new StringBuilder();

            foreach (string key in songs.Keys)
            {
                buf.Append(songs[key].ToString());
            }

            // 楽曲情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_SONG_DATA,
                false
            );
        }

        /*
         * 楽曲情報読み込み
         */
        public static Dictionary<string, SongData> _readSongData(string path)
        {
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

        /*
         * 楽曲情報読み込み
         */
        public static Dictionary<string, SongData> readSongData(Player player)
        {
            Dictionary<string, SongData> ret = null;

            if (player != null)
            {

                string path = $"{player.DirPath}/{SettingConst.FILE_SONG_DATA}";
                ret = SongLogic._readSongData(path);
            }

            return ret;
        }

        /*
         * 現在配信曲か
         */
        public static void setViewFlg(UrlData url, ref Dictionary<string, SongData> songs)
        {
            foreach (string songName in songs.Keys)
            {
                songs[songName].viewFlg = url.songUrl.ContainsKey(songName);
            }
        }
    }
}
