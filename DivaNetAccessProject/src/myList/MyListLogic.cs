using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DivaNetAccess.src.myList
{
    public static class MyListLogic
    {
        // マイリストTOPページURL
        private const string URL_MYLIST_PAGE = "http://project-diva-ac.net/divanet/myList/";

        // マイリスト管理ページURL
        private const string URL_MYLIST_KANRI = "http://project-diva-ac.net/divanet/myList/selectMyList/";

        /*
         * マイリストボタン
         */
        public static int getMyList(Player player, MainForm form)
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

                string[] res = web.HttpPost("http://project-diva-ac.net/divanet/logout/");
                DivaNetLogic.afterInit(form);
                return -1;
            }

            /*
            // 利用権チェック
            if (DivaNetLogic.isAuthorization(web, form) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0007, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return -1;
            }
            */

            // マイリスト情報生成
            MyListEntity myListEntity = new MyListEntity();

            // マイリスト取得メイン
            getMyListMain(web, myListEntity, form);

            // マイリスト書き込み処理
            writeMyListData(player, myListEntity);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0008 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return 0;
        }

        /*
         * マイリスト取得メイン
         */
        private static int getMyListMain(WebData web, MyListEntity myListEntity, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - マイリスト情報取得中...";

            // HTMLドキュメント取得
            string[] res = web.HttpPost(URL_MYLIST_PAGE);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            int myListIndex = 1;

            // リンクを全て取得する
            for (int i = 0; i < html.Links.Count; i++)
            {
                // URL形式変更
                string url = WebUtil.convUrlToHtml(html.Links[i].GetAttribute("href"));

                // マイリスト管理URLかつ登録されている楽曲がある(スラッシュを含む)か
                if (url.StartsWith(URL_MYLIST_KANRI) && html.Links[i].InnerText.Contains("/"))
                {
                    int useKyotaiNo = -1;
                    if (html.Links[i].InnerText.Contains("(*)"))
                    {
                        useKyotaiNo = 0;
                    }
                    MyListLogic.getMyListDetail(web, myListEntity, myListIndex, useKyotaiNo, url, form);
                    myListIndex++;
                }
            }

            return 0;
        }

        /*
         * マイリスト取得詳細
         */
        private static int getMyListDetail(WebData web, MyListEntity myListEntity, int myListIndex, int useKyotaiNo, string url, MainForm form)
        {
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            HtmlElement body = html.GetElementsByTagName("body")[0];
            string[] innerTexts = WebUtil.getInnerTextReplace(body.InnerText);

            // 要素ごとに振り分け
            Dictionary<string, List<string>> innerElems = PlayRecordLogic.getDivaNetKomoku(innerTexts);

            string listName = "";
            DateTime updateDate = new DateTime();

            // マイリスト名、最終更新時刻のみ先に取得
            foreach (string elem in innerElems.Keys)
            {
                List<string> elemValues = innerElems[elem];
                string elemValueStr = string.Join("", elemValues.ToArray());

                if (elem.Contains("[マイリスト名]"))
                {
                    listName = elemValues[1];
                }

                if (elem.Contains("[最終更新時刻]"))
                {
                    // yy/MM/dd HH:mm:ss形式
                    updateDate = DateTime.ParseExact(elemValues[1], "yy/MM/dd HH:mm", null);
                }
            }

            // 楽曲リストを取得
            foreach (string elem in innerElems.Keys)
            {
                List<string> elemValues = innerElems[elem];

                if (elem.Contains("[楽曲リスト]"))
                {
                    // [楽曲リスト](xx/xx)の現在の楽曲数を取得する
                    int songCnt = int.Parse(WebUtil.getMache(elem.Trim(), @"\d+"));

                    for (int i = 0; i < songCnt; i++)
                    {
                        myListData mList = new myListData();

                        mList.listNo = myListIndex;
                        mList.listName = listName;
                        mList.updateDate = updateDate;
                        mList.useKyotaiNo = useKyotaiNo;
                        mList.songNo = i + 1;
                        mList.songName = elemValues[i + 1];

                        myListEntity.myLists.Add(mList);
                    }
                }
            }

            return 0;
        }

        /*
         * マイリスト情報書き込み
         */
        public static void writeMyListData(Player player, MyListEntity mListEntity)
        {
            // 楽曲情報書き込み
            FileUtil.writeFile(
                mListEntity.ToString(),
                $"{player.DirPath}/{SettingConst.FILE_MYLIST_DATA}",
            false
            );
        }

        /*
         * マイリスト情報読み込み
         */
        public static MyListEntity readMyListData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_MYLIST_DATA}";
            return MyListLogic._readMyListData(path, player);
        }

        /*
         * マイリスト情報読み込み
         */
        private static MyListEntity _readMyListData(string path, Player player)
        {
            // マイリスト情報
            MyListEntity ret = new MyListEntity();

            // マイリスト情報ファイルが無い時はnullを返す
            if (File.Exists(path) == false)
            {
                //return null;
                return ret;
            }

            // ファイルを開く
            using (StreamReader sr = new StreamReader(
                path,
                SettingConst.FILE_ENCODING
            ))
            {
                // ファイルをすべて読み込む
                string fileStr = sr.ReadToEnd();

                ret = new MyListEntity(fileStr);
            }

            return ret;
        }
    }
}
