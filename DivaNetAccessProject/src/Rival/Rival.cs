using System;
using System.Collections.Generic;
using System.Text;

namespace DivaNetAccess.src
{
    // playerDataの派生クラス
    public class Rival : Player
    {
        public readonly static new string DIR_NAME = "rival";

        public enum Index
        {
            RIVALCODE = 0,
            NAME,
            RANK,
            SETRIVAL,
            SETINTERESTED,
            PR,
            TAGS,
            TWITTERCONNECT,
            WINANNOUNCE,
            KOUKAIDETAILS,
            TWITTERURL,
            GETDATE,
            MEMO,
        }

        // URL＠アクセスするたびに変わるのでファイルに出力はしない
        public string url;

        // ライバルに設定されている人数
        public int setRival;

        // 気になるプレイヤーに登録されている人数
        public int setInterested;

        // 自己PR
        public string pr;

        // タグ
        public List<string> tags;

        // Twitter連動公開設定
        public string twitterConnect;

        // 他プレイヤーからの勝利宣言受付設定
        public string winAnnounce;

        // TwitterプロフィールURL
        public string twitterProfileUrl;

        /*
         * 詳細情報
         *   [0]：クリア統計情報
         *   [1]：[楽曲リスト(プレイ情報確認)]
         *   [2]：[ランクインリスト]
         *   [3]：[プレイデータ比較]
         */
        public bool[] koukaiDetails { get; private set; }

        /*
         * コンストラクタ
         */
        public Rival() : base()
        {
            tags = new List<string>();
            koukaiDetails = new bool[3];
        }

        /*
         * ライバル情報書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(rivalCode + SEPALATOR);
            ret.Append(name + SEPALATOR);
            ret.Append(rank + SEPALATOR);
            ret.Append(setRival + SEPALATOR);
            ret.Append(setInterested + SEPALATOR);
            ret.Append(pr + SEPALATOR);
            foreach (string tag in tags) { ret.Append(tag + SEPALATOR); }
            if (tags.Count > 0) { ret.Remove(ret.Length - 1, 1); }          // 末尾(つまりは余計に書いたタブ)削除＠ださい
            ret.Append(SEPALATOR);
            ret.Append(twitterConnect + SEPALATOR);
            ret.Append(winAnnounce + SEPALATOR);
            //foreach (bool koukaiDetail in koukaiDetails) { ret.Append(koukaiDetail + SEPALATOR); }
            for (int i = 0; i < 1; i++) { ret.Append(koukaiDetails[i] + SEPALATOR); }      // コンバート作るのだるいから2つだけ
            if (koukaiDetails.Length > 0) { ret.Remove(ret.Length - 1, 1); } // 末尾(つまりは余計に書いたタブ)削除＠ださい
            ret.Append(SEPALATOR);
            ret.Append(twitterProfileUrl + SEPALATOR);
            ret.Append(getDate + SEPALATOR);
            ret.Append(memo + SEPALATOR);

            return ret.ToString();
        }

        /*
         * コンストラクタ＠ファイル読み込み時
         */
        public Rival(string[] lines) : this()
        {
            string[] tmpStrArray;
            string[] tmpBoolArray;

            rivalCode = lines[(int)Index.RIVALCODE];
            name = lines[(int)Index.NAME];
            rank = lines[(int)Index.RANK];
            setRival = int.Parse(lines[(int)Index.SETRIVAL]);
            setInterested = int.Parse(lines[(int)Index.SETINTERESTED]);
            pr = lines[(int)Index.PR];
            tmpStrArray = lines[(int)Index.TAGS].Split(char.Parse(SEPALATOR));
            foreach (string tmp in tmpStrArray) { tags.Add(tmp); }
            twitterConnect = lines[(int)Index.TWITTERCONNECT];
            winAnnounce = lines[(int)Index.WINANNOUNCE];
            tmpBoolArray = lines[(int)Index.KOUKAIDETAILS].Split(char.Parse(SEPALATOR));
            for (int i = 0; i < tmpBoolArray.Length; i++) { koukaiDetails[i] = bool.Parse(tmpBoolArray[i]); }
            twitterProfileUrl = lines[(int)Index.TWITTERURL];
            getDate = DateTime.Parse(lines[(int)Index.GETDATE]);
            memo = lines[(int)Index.MEMO];
        }

        /*
         * タグ取得(ウインドウ表示用)
         */
        public string getTag()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string tag in tags)
            {
                sb.Append(tag + "\n");
            }

            // 最後の1文字削除
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}
