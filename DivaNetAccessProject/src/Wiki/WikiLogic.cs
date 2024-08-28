﻿using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.Wiki
{
    // PDA Wikiアクセスクラス
    public static class WikiLogic
    {
        // 達成率理論値ページ
        private static readonly string URL_TASSEI_RIRON = "https://w.atwiki.jp/projectdiva_ac/pages/222.html";

        /*
         * 達成率理論値取得メイン
         */
        public static int tasseirituRironMain(MainForm form)
        {
            // ストップウォッチ生成
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            form.Text = SettingConst.WINDOW_TITLE + " - 達成率理論値取得中...";

            WebData web = new WebData();

            Dictionary<string, SongData> songs = new Dictionary<string, SongData>();

            // 達成率理論値取得
            getTasseirituRiron(web, songs);

            // 達成率理論値書き込み
            writeTasseirituRiron(songs);

            form.Text = SettingConst.WINDOW_TITLE;

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0003 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return 0;
        }

        /*
         * 達成率理論値取得メイン
         */
        public static void getTasseirituRiron(WebData web, Dictionary<string, SongData> songs)
        {
            string[] res = web.HttpGet(URL_TASSEI_RIRON);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // TABLEタグを取得
            foreach (HtmlElement table in html.GetElementsByTagName("TABLE"))
            {
                // TRタグを取得
                HtmlElementCollection trs = table.GetElementsByTagName("tr");

                // ヘッダに"曲名"を含むテーブル以外はスキップ
                if (trs.Count == 0 || !trs[0].InnerText.Contains("曲名"))
                {
                    continue;
                }

                for (int trCnt = 0; trCnt < trs.Count; trCnt++)
                {
                    // "曲名"または"難易度別"を含む行はスキップ
                    if (trs[trCnt].InnerText.Contains("曲名") | trs[trCnt].InnerText.Contains("難易度別"))
                    {
                        continue;
                    }

                    SongData song = new SongData();

                    Dictionary<string, ResultData> datas = new Dictionary<string, ResultData>();

                    HtmlElementCollection tds = trs[trCnt].GetElementsByTagName("td");

                    // 曲名を設定
                    song.name = tds[0].InnerText;

                    // 曲名不一致＠暫定対応
                    if (song.name == "SING & SMILE")
                    {
                        song.name = "SING＆SMILE";
                    }
                    if (song.name == "shake it!")
                    {
                        song.name = "shake it！";
                    }

                    // 難易度ごとの情報を設定
                    for (int tdCnt = 1; tdCnt < tds.Count; tdCnt++)
                    {
                        ResultData data = new ResultData();

                        // 難易度
                        data.diff = WebUtil.DIFF_STR[tdCnt - 1];

                        // 達成率
                        string tasseiritu = WebUtil.getMache(tds[tdCnt].InnerText, @"\d+[.]\d+");

                        // 楽曲なしチェック
                        if ("".Equals(tasseiritu))
                        {
                            data.tasseiritu = 0;
                        }
                        else
                        {
                            bool isdo = false;
                            double d;
                            isdo = double.TryParse(tasseiritu, out d);
                            if (isdo)
                            {
                                tasseiritu = d.ToString("000.00");
                                data.tasseiritu = int.Parse(tasseiritu.Replace(".", ""));
                            }
                        }

                        datas.Add(data.diff, data);
                    }

                    // 楽曲に難易度情報を設定
                    song.data = datas;

                    // 楽曲を追加
                    songs.Add(song.name, song);
                }
            }
        }

        /*
         * 達成率理論値書き込み
         */
        public static void writeTasseirituRiron(Dictionary<string, SongData> songs)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.CONF_DIR_NAME);

            StringBuilder buf = new StringBuilder();

            foreach (string key in songs.Keys)
            {
                buf.Append(songs[key].ToStringTasseirituRiron());
            }

            // 楽曲情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                SettingConst.CONF_DIR_NAME + "/" + SettingConst.FILE_TASSEIRITU_RIRON_DATA,
                false
            );
        }

        /*
         * 達成率理論値読み込み
         */
        private static Dictionary<string, SongData> _readTasseirituRiron(string path)
        {
            // 楽曲リスト
            Dictionary<string, SongData> songs = new Dictionary<string, SongData>();

            // 達成率理論値ファイル存在チェック
            if (File.Exists(path) == false)
            {
                return songs;
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
                    // 楽曲情報生成
                    SongData song = new SongData();
                    song.songDataRiron(line);

                    // リストに追加
                    songs.Add(song.name, song);
                }
            }

            return songs;
        }

        /*
         * 達成率理論値読み込み
         */
        public static Dictionary<string, SongData> readTasseirituRiron(Player player)
        {
            string path;
            if (player.IsBase)
            {
                path = SettingConst.CONF_DIR_NAME + "/" + SettingConst.FILE_TASSEIRITU_RIRON_DATA;
            }
            else
            {
                path = $"{player.DirPath}/{SettingConst.FILE_TASSEIRITU_RIRON_DATA}";
            }
            return WikiLogic._readTasseirituRiron(path);
        }

        /*
         * 達成率理論値読み込み
         */
        public static Dictionary<string, SongData> readTasseirituRironBackup(Player player)
        {
            string path = SettingConst.CONF_DIR_NAME + "/" + player.accessCode + "/" + player.backupDateDir + SettingConst.FILE_TASSEIRITU_RIRON_DATA;
            return WikiLogic._readTasseirituRiron(path);
        }
    }
}
