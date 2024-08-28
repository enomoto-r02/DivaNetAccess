using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DivaNetAccess.src.CollectionCard
{
    public static class CollectionCardLogic
    {
        private enum CardTypeUrl
        {
            MODULE = 0,
            SONG = 1,
        }

        // モジュールカードTOPページURL
        // {モジュールカードor楽曲カード}/{ソート順}/{ページ数}
        private const string URL_CARD_PAGE = "http://project-diva-ac.net/divanet/collectionCard/album/{0}/{1}";
        private const string URL_MODULE_CARD_PAGE_LIST = "http://project-diva-ac.net/divanet/collectionCard/changeDisplay/0/2/0";

        private const string URL_MODULE_CARD_PAGE_DETAIL = "http://project-diva-ac.net/divanet/collectionCard/card/";


        private const string TEST = "http://project-diva-ac.net";
        /*
         * コレクションカードボタン
         */
        public static int getCollectionCard(string accessCode, string password, MainForm form, CollectionCardEntity cardEntity)
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

                string[] res = web.HttpPost("http://project-diva-ac.net/divanet/logout/");
                DivaNetLogic.afterInit(form);
                return -1;
            }

            // 利用権チェック
            /*
            if (DivaNetLogic.isAuthorization(web, form) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0007, MessageConst.E_MSG_ERROR_T);

                DivaNetLogic.afterInit(form);
                return -1;
            }
            */

            // コレクションカード生成
            CollectionCardEntity collectionCardEntity = new CollectionCardEntity();

            // コレクションカード取得メイン
            getCollectionCardMain(web, collectionCardEntity, form);

            // コレクションカードマージ処理
            //mergeCollectionCard(cardEntity, ref collectionCardEntity);

            // コレクションカード書き込み処理
            writeCollectionCard(player, collectionCardEntity);

            // 後処理
            DivaNetLogic.afterInit(form);

            // ストップウォッチ停止
            sw.Stop();

            MessageBox.Show(MessageConst.N_MSG_0009 + "\r\n" + (double)sw.ElapsedMilliseconds / 1000.0f + " 秒", MessageConst.N_MSG_FINISH_T);

            return 0;
        }

        /*
         * コレクションカード取得メイン
         */
        private static int getCollectionCardMain(WebData web, CollectionCardEntity collectionCardEntity, MainForm form)
        {
            form.Text = SettingConst.WINDOW_TITLE + " - コレクションカード情報取得中...";

            // カードアルバムページを開く
            string[] res = web.HttpPost(string.Format(URL_CARD_PAGE + "/0", "0", (int)CollectionCard.CardType.MIKU));
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // リスト表示に切り替える
            res = web.HttpPost(URL_MODULE_CARD_PAGE_LIST);
            html = WebUtil.getHtmlDocument(res[1]);

            // 各キャラクターのモジュールを取得
            foreach (CollectionCard.CardType m in Enum.GetValues(typeof(CollectionCard.CardType)))
            {
                // 設定用
                int typeNo = 1;

                string url;
                string vocaloid_name;
                if (m != CollectionCard.CardType.SONG)
                {
                    url = string.Format(URL_CARD_PAGE, (int)CardTypeUrl.MODULE, (int)m);
                    vocaloid_name = (DivaNetLogic.VOCALOID_NAME[(int)m - (int)CollectionCard.CardType.MIKU]);

                    //return 0;
                }
                else
                {
                    url = string.Format(URL_CARD_PAGE, (int)CardTypeUrl.SONG, "0");
                    vocaloid_name = "楽曲";

                    //return 0;
                }
                form.Text = SettingConst.WINDOW_TITLE + " - コレクションカード情報取得中...[" + vocaloid_name + "]";

                getCardAlbumPage(web, url, 0, m, typeNo, collectionCardEntity);
            }

            return 0;
        }

        /*
         * カードアルバム(一覧)ページ取得処理
         */
        private static int getCardAlbumPage(WebData web, string url, int page, CollectionCard.CardType cardType, int typeNo, CollectionCardEntity collectionCardEntity)
        {
            string[] res = web.HttpPost(url + "/" + page);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // 所持しているカード名リスト生成
            Dictionary<string, string> detailCardNameLinks = new Dictionary<string, string>();

            // 空白行を削除
            string[] source = WebUtil.delBlankLine(res[1]).Split('\n');

            for (int i = 0; i < source.Length; i++)
            {
                string line = source[i];

                // URL詳細あり
                if (line.Contains("/divanet/collectionCard/card/"))
                {
                    CollectionCard c = new CollectionCard();

                    // URL
                    string href = WebUtil.getMache(line, "href=\".*?\"").Replace("href=", "").Replace("\"", "");
                    // カード名
                    string name = line.Replace(WebUtil.getMache(line, "<a .*?>"), "").Replace(WebUtil.getMache(line, "</a.*?>"), "");

                    // カードアルバム(詳細)ページ取得
                    getCardAlbumPageDetail(web, TEST + href, cardType, c);

                    // 設定
                    c.no = collectionCardEntity.collectionCards.Count + 1;
                    c.name = name.Replace("〜", "～");    // UTF-8→SJISの文字化け対策＠暫定
                    c.type = cardType;
                    c.typeNo = typeNo;

                    // 次の行にspanタグで囲まれた"NEW"があれば新規獲得
                    if (string.IsNullOrEmpty(WebUtil.getMache(source[i + 1], "<span .*?>NEW</span.*?>")) == false)
                    {
                        c.newFlg = true;
                    }

                    // 追加
                    collectionCardEntity.collectionCards.Add(c);

                    // カウントアップ
                    typeNo++;
                }
                else if (line.Contains("？？？？"))
                {
                    CollectionCard c = new CollectionCard();

                    // カード名
                    string name = line.Replace(WebUtil.getMache(line, "<br.*?>"), "");

                    // 設定
                    c.no = collectionCardEntity.collectionCards.Count + 1;
                    c.name = name;
                    c.type = cardType;
                    c.typeNo = typeNo;

                    // 追加
                    collectionCardEntity.collectionCards.Add(c);

                    // カウントアップ
                    typeNo++;
                }
            }

            // 次のページを取得
            if (WebUtil.checkFoundNextPage(html.Links))
            {
                int nextPage = page + 1;

                getCardAlbumPage(web, url, nextPage, cardType, typeNo, collectionCardEntity);
            }

            return 0;
        }

        /*
         * カードアルバム(詳細)ページ取得処理
         */
        private static int getCardAlbumPageDetail(WebData web, string url, CollectionCard.CardType moduleType, CollectionCard card)
        {
            string[] res = web.HttpPost(url);
            HtmlDocument html = WebUtil.getHtmlDocument(res[1]);

            // innerTextからs
            string innerText = html.Body.InnerText;
            string[] texts = innerText.Split('\n');

            // 要素ごとに格納
            Dictionary<string, string> elems = getDivaNetKomokuCollectionCard(texts);

            // innerTextのモジュール表示範囲を取得
            foreach (string key in elems.Keys)
            {
                if (key.Equals("獲得枚数"))
                {
                    card.num = int.Parse(elems[key].Replace("枚", ""));
                }

                if (key.Equals("初回獲得日時"))
                {
                    card.getDate = DateTime.Parse(elems[key]);
                }
            }

            card.url = url.Split('/')[6];

            return 0;
        }

        /*
         * コレクションカード書き込み
         */
        public static void writeCollectionCard(Player player, CollectionCardEntity collectionCardEntity)
        {
            // 楽曲情報書き込み
            FileUtil.writeFile(
                collectionCardEntity.ToString(),
                $"{player.DirPath}/{SettingConst.FILE_COLLECTION_CARD_DATA}",
                false
            );
        }

        /*
         * コレクションカード読み込み
         */
        private static CollectionCardEntity _readCollectionCard(string path)
        {
            // マイリスト情報
            CollectionCardEntity ret = new CollectionCardEntity();

            // コレクションカードファイルが無ければ生成したインスタンスを返す
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
                // ファイルをすべて読み込む
                string fileStr = sr.ReadToEnd();

                ret = new CollectionCardEntity(fileStr);
            }

            return ret;
        }

        /*
         * コレクションカード読み込み
         */
        public static CollectionCardEntity readCollectionCard(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_COLLECTION_CARD_DATA}";
            return CollectionCardLogic._readCollectionCard(path);
        }

        /*
         * DIVA.NET用の汎用メソッド
         * 　DIVA.NETのInnerTextから
         * 　"(文字列)：(文字列)"で始まる要素ごとに振り分ける
         */
        public static Dictionary<string, string> getDivaNetKomokuCollectionCard(string[] innerTexts)
        {
            // 要素ごとに振り分け
            Dictionary<string, string> innerElems = new Dictionary<string, string>();

            for (int i = 0; i < innerTexts.Length; i++)
            {
                string sepalator = "：";
                string titleElement = WebUtil.getMache(innerTexts[i].Trim(), "^.*" + sepalator + ".*");

                // "(文字列)：(文字列)"を含む行
                if (!string.IsNullOrEmpty(titleElement))
                {
                    string[] elem = titleElement.Split(char.Parse(sepalator));

                    if (elem.Length >= 2)
                    {
                        innerElems.Add(elem[0], elem[1]);
                    }
                }
            }

            return innerElems;
        }

        /*
         * マージ処理＠楽曲削除対応
         * URLをキーにチェックしているので、アクセスコード変更に対応できなくなってしまったため今回は非対応
         */
        private static void mergeCollectionCard(CollectionCardEntity oldCollection, ref CollectionCardEntity newCollection)
        {
            foreach (CollectionCard oldCard in oldCollection.collectionCards)
            {
                bool findFlg = false;

                foreach (CollectionCard newCard in newCollection.collectionCards)
                {
                    if (oldCard.key() == newCard.key())
                    {
                        // 一致あったらループ抜ける＠無駄なループさせない
                        findFlg = true;
                        break;
                    }
                }

                // 一致があったら上のループへ戻る
                if (findFlg)
                {
                    continue;
                }

                // カードの増減差異があったら残す＠ここに入るのは差異のあったカードのみ
                newCollection.collectionCards.Add(oldCard);
            }
        }
    }
}
