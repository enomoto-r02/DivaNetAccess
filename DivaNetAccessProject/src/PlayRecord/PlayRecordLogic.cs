using DivaNetAccess.src;
using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess
{
    public static class PlayRecordLogic
    {
        // プレイ履歴URL
        private static readonly string URL_PLAY_RECORD_BASE = "http://project-diva-ac.net/divanet/personal/playHistory/";

        // プレイ履歴詳細URL
        private static readonly string URL_PLAY_RECORD_DETAIL_BASE = "http://project-diva-ac.net/divanet/personal/playHistoryDetail/";

        // プレイ履歴用日付フォーマット
        public static readonly string DATE_FORMAT_PLAY_RECORD = "yyyy/MM/dd HH:mm";

        /* 
         * プレイ履歴取得メイン
         */
        public static Dictionary<string, PlayRecordEntity> getPlayRecordMain(Player player, UrlData playerUrls, MainForm form)
        {
            // ストップウォッチ生成
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // Web情報生成
            WebData web = new WebData(player);

            //// ログインチェック
            //if (DivaNetLogic.isLogin(web, form) == false)
            //{
            //    MessageBox.Show(MessageConst.E_MSG_0002, MessageConst.E_MSG_ERROR_T);

            //    DivaNetLogic.afterInit(form);
            //    return null;
            //}

            // プレイ履歴リスト生成
            Dictionary<string, PlayRecordEntity> records = new Dictionary<string, PlayRecordEntity>();

            // プレイ履歴リスト取得
            getPlayRecordList(web, 0, records, form);

            // プレイ履歴書き込み処理
            writePlayRecordData(player, records);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0004 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return records;
        }

        /*
         * プレイ履歴リスト取得
         */
        private static void getPlayRecordList(WebData web, int page, Dictionary<string, PlayRecordEntity> records, MainForm form)
        {
            string[] res = web.HttpPost(URL_PLAY_RECORD_BASE + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // フォーム表示用カウンタ
            int playRecordCnt = 1 + (page * 10);

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // 詳細ページを取得
                if (url.StartsWith(URL_PLAY_RECORD_DETAIL_BASE))
                {
                    form.Text = SettingConst.WINDOW_TITLE + " - プレイ履歴取得中...[" + playRecordCnt + "曲目]";

                    PlayRecordEntity record = new PlayRecordEntity();

                    // プレイ履歴詳細取得
                    record = getPlayRecordDetail(web, url);

                    // 取得していないプレイ履歴ならリストに追加
                    if (records.ContainsKey(record.key) == false)
                    {
                        records.Add(record.key, record);
                    }

                    playRecordCnt++;
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                getPlayRecordList(web, page + 1, records, form);
            }
        }

        /*
         * プレイ履歴詳細取得
         */
        public static PlayRecordEntity getPlayRecordDetail(WebData web, string url)
        {
            PlayRecordEntity ret = new PlayRecordEntity();

            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            HtmlElement body = html.GetElementsByTagName("body")[0];
            string[] innerTexts = WebUtil.getInnerTextReplace(body.InnerText);

            // 要素ごとに振り分け
            Dictionary<string, List<string>> innerElems = PlayRecordLogic.getDivaNetKomoku(innerTexts);

            foreach (string elem in innerElems.Keys)
            {
                List<string> elemValues = innerElems[elem];
                string elemValueStr = string.Join("", elemValues.ToArray());

                if (elem.Contains("[日時]"))
                {
                    ret.date = DateTime.Parse(elemValues[0].Replace("[日時] ", ""));
                }
                if (elem.Contains("[場所]"))
                {
                    if (elemValues != null && elemValues.Count > 0)
                    {
                        ret.place = elemValueStr.Replace("[場所] ", "");
                    }
                }
                if (elem.Contains("[曲名]"))
                {
                    ret.name += elemValueStr.Replace("[曲名]", "");
                }
                if (elem.Contains("[難易度]"))
                {
                    if (elemValues != null && elemValues.Count > 1)
                    {
                        string[] tmpArray = elemValues[1].Split('　');
                        ret.diff = tmpArray[0];
                        ret.star = float.Parse(tmpArray[1].Replace("★", ""));
                    }
                }
                if (elem.Contains("[CLEAR RANK]"))
                {
                    if (elemValues != null && elemValues.Count > 1)
                    {
                        ret.clear = elemValues[1];
                    }
                }
                if (elem.Contains("[達成率]"))
                {
                    ret.tasseiritu = WebUtil.convTasseiritu(elemValues[1]);
                    // 更新チェック
                    if (elemValueStr.Contains("NEW RECORD"))
                    {
                        ret.tasseirituNewRecord = true;
                    }
                }
                if (elem.Contains("[SCORE]"))
                {
                    ret.score = int.Parse(elemValues[1]);
                    // 更新チェック
                    if (elemValueStr.Contains("NEW RECORD"))
                    {
                        ret.scoreNewRecord = true;
                    }

                    for (int i = 0; i < elemValues.Count; i++)
                    {
                        if (elemValues[i].Contains("COOL："))
                        {
                            string[] tmpArray = elemValues[i].Split('：');
                            tmpArray = tmpArray[1].Replace("%", "").Split('/');
                            ret.cool = int.Parse(tmpArray[0]);
                            ret.coolP = WebUtil.convTasseiritu(tmpArray[1]);
                        }

                        if (elemValues[i].Contains("FINE："))
                        {
                            string[] tmpArray = elemValues[i].Split('：');
                            tmpArray = tmpArray[1].Replace("%", "").Split('/');
                            ret.fine = int.Parse(tmpArray[0]);
                            ret.fineP = WebUtil.convTasseiritu(tmpArray[1]);
                        }

                        if (elemValues[i].Contains("SAFE："))
                        {
                            string[] tmpArray = elemValues[i].Split('：');
                            tmpArray = tmpArray[1].Replace("%", "").Split('/');
                            ret.safe = int.Parse(tmpArray[0]);
                            ret.safeP = WebUtil.convTasseiritu(tmpArray[1]);
                        }

                        if (elemValues[i].Contains("SAD："))
                        {
                            string[] tmpArray = elemValues[i].Split('：');
                            tmpArray = tmpArray[1].Replace("%", "").Split('/');
                            ret.sad = int.Parse(tmpArray[0]);
                            ret.sadP = WebUtil.convTasseiritu(tmpArray[1]);
                        }

                        if (elemValues[i].Contains("WORST/WRONG："))
                        {
                            string[] tmpArray = elemValues[i + 1].Replace("%", "").Split('/');
                            ret.worst = int.Parse(tmpArray[0]);
                            ret.worstP = WebUtil.convTasseiritu(tmpArray[1]);
                        }

                        if (elemValues[i].Contains("COMBO："))
                        {
                            ret.combo = int.Parse(elemValues[i].Split('：')[1]);
                        }

                        if (elemValues[i].Contains("CHALLENGE TIME："))
                        {
                            ret.challenge = int.Parse(elemValues[i + 1]);
                        }

                        if (elemValues[i].Contains("同時押し/ホールド："))
                        {
                            ret.hold = int.Parse(elemValues[i + 1]);
                        }

                        if (elemValues[i].Contains("スライド："))
                        {
                            ret.slide = int.Parse(elemValues[i + 1]);
                        }
                    }
                }
                if (elem.Contains("[クリアトライアル]"))
                {
                    ret.trial = elemValues[1];

                    // 連続パーフェクトトライアル回数取得
                    if (elemValues[1].Contains("連続パーフェクトトライアル") && elemValues[1].Contains("成功"))
                    {
                        ret.trial += " " + elemValues[2];
                    }
                }

                if (elem.Contains("[リズムゲームオプション]"))
                {
                    ret.option = elemValues[1];
                }

                if (elem.Contains("[PV分岐]"))
                {
                    ret.pvjunc = elemValues[1];
                }

                if (elem.Contains("[モジュール]"))
                {
                    // モジュール有りと判断する文字列
                    string[] chkStr = { "ボーカル", "ゲスト", };

                    List<string> modules = new List<string>();

                    for (int i = 0; i < elemValues.Count; i++)
                    {
                        foreach (string key in chkStr)
                        {
                            if (elemValues[i].Contains(key))
                            {
                                string module = elemValues[i].Split('：')[1];

                                // ランダムか
                                if ((elemValues.Count > i + 1) && (elemValues[i + 1].Contains("ランダム")))
                                {
                                    module += elemValues[i + 1];
                                }
                                modules.Add(module);
                            }
                        }
                    }
                    // ダサい。。
                    if (modules.Count >= 1)
                    {
                        ret.module1 = modules[0];
                    }
                    if (modules.Count >= 2)
                    {
                        ret.module2 = modules[1];
                    }
                    if (modules.Count >= 3)
                    {
                        ret.module3 = modules[2];
                    }
                }
                if (elem.Contains("[ボタン音]"))
                {
                    ret.button = elemValues[1];
                }
                if (elem.Contains("[スライド音]"))
                {
                    ret.slideSE = elemValues[1];
                }
                if (elem.Contains("[チェーンスライド音]"))
                {
                    ret.chain = elemValues[1];
                }
                if (elem.Contains("[スキン]"))
                {
                    ret.skin = elemValues[1];
                }
            }

            ret.memo = "";

            // キー生成
            ret.makeKey();

            return ret;
        }

        /*
         * プレイ履歴書き込み
         */
        public static void writePlayRecordData(Player player, Dictionary<string, PlayRecordEntity> records)
        {
            StringBuilder buf = new StringBuilder();

            foreach (string key in records.Keys)
            {
                buf.Append(records[key].ToString());
            }

            // プレイ履歴書き込み
            FileUtil.writeFile(
                buf.ToString(),
            $"{player.DirPath}/{SettingConst.FILE_RECORD_DATA}",
            false
            );
        }

        private static Dictionary<string, PlayRecordEntity> _readPlayRecordData(string path, UrlData urlData)
        {
            // プレイヤー情報生成
            Dictionary<string, PlayRecordEntity> ret2 = new Dictionary<string, PlayRecordEntity>();
            Dictionary<string, PlayRecordEntity> ret = new Dictionary<string, PlayRecordEntity>();

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
                    PlayRecordEntity record = new PlayRecordEntity(line, urlData.songUrl);

                    // リストに追加
                    //ret2.Add(record.key, record);
                    ret2.Add(record.key + "_" + ret2.Count, record);  // デバッグ用にキー回避
                }
            }

            IOrderedEnumerable<KeyValuePair<string, PlayRecordEntity>> ioe = ret2.OrderByDescending(pair => pair.Key);
            foreach (KeyValuePair<string, PlayRecordEntity> item in ioe)
            {
                ret.Add(item.Key, item.Value);
            }

            return ret;
        }

        public static Dictionary<string, PlayRecordEntity> readPlayRecordData(Player player, UrlData urlData)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_RECORD_DATA}";
            return PlayRecordLogic._readPlayRecordData(path, urlData);
        }

        /*
         * DIVA.NET用の汎用メソッド
         * 　DIVA.NETのInnerTextから
         * 　"[xxx]"で始まる要素ごとに振り分ける
         */
        public static Dictionary<string, List<string>> getDivaNetKomoku(string[] innerTexts)
        {
            // 要素ごとに振り分け
            Dictionary<string, List<string>> innerElems = new Dictionary<string, List<string>>();

            bool bSearchEnd = false;
            List<string> elems = new List<string>();
            string title = "";

            for (int i = 0; i < innerTexts.Length; i++)
            {
                string titleElement = WebUtil.getMache(innerTexts[i].Trim(), "^\\[.*?\\].*");

                // "[(文字列)]"を含む行
                if (!string.IsNullOrEmpty(titleElement))
                {
                    if (bSearchEnd)
                    {
                        // 検索終了
                        bSearchEnd = false;

                        // Listに追加
                        innerElems.Add(title, elems);

                        // 初期化
                        elems = new List<string>();
                        title = "";
                    }
                    if (!bSearchEnd)
                    {
                        // 検索開始
                        bSearchEnd = true;

                        title = titleElement;
                        elems.Add(innerTexts[i]);
                    }
                }
                else
                {
                    if (bSearchEnd)
                    {
                        // 要素を追加
                        if (!string.IsNullOrEmpty(innerTexts[i].Trim()))
                        {
                            elems.Add(innerTexts[i]);
                        }

                    }
                }
            }

            return innerElems;
        }
    }
}
