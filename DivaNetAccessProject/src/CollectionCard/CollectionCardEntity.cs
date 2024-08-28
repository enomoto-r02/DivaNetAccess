using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace DivaNetAccess.src.CollectionCard
{
    // コレクションカードリストクラス
    public class CollectionCardEntity
    {
        // 区切り文字
        private readonly string SEPALATOR = "\n";

        // コレクションカード情報
        public List<CollectionCard> collectionCards = new List<CollectionCard>();

        /*
         * コンストラクタ
         */
        public CollectionCardEntity()
        {
        }

        /*
         * コンストラクタ(ファイル読み込み用)
         */
        public CollectionCardEntity(string fileStr)
        {
            // 空白行は読み込まない
            string[] lines = fileStr.Split(new string[] { SEPALATOR }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                CollectionCard collectionCard = new CollectionCard(line);

                collectionCards.Add(collectionCard);
            }
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            foreach (CollectionCard cc in collectionCards)
            {
                ret.Append(cc.ToString() + SEPALATOR);
            }

            return ret.ToString();
        }
    }


    // コレクションカードクラス
    public class CollectionCard
    {
        public enum Index
        {
            NO = 0,
            TYPE,
            TYPE_NO,
            NAME,
            NUM,
            GET_DATE,
            NEW_FLG,
            URL,
        }

        // カード種類
        public enum CardType
        {
            // 以下、DIVA.NETのURLに合わせて１からスタート
            SONG = 1,       // 楽曲
            MIKU,           // 初音ミク
            RIN,            // 鏡音リン
            LEN,            // 鏡音レン
            LUKA,           // 巡音ルカ
            MEIKO,          // MEIKO
            KAITO,          // KAITO
            HASEI,          // 派生キャラ
            //EXTRA,          // エキストラ
        }

        // カード種類
        private readonly Dictionary<CardType, string> CardTypeStr = new Dictionary<CardType, string>();

        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // 初回獲得日付_書式
        private readonly string DATE_FORMAT = "yyyy/MM/dd HH:mm";

        // No
        public int no { get; set; }

        // カード種類No
        public int typeNo { get; set; }

        // カード種類
        public CardType type { get; set; }

        // カード名
        public string typeName
        {
            get
            {
                return getTypeName();
            }
        }

        // カード名
        public string name { get; set; }

        // 枚数
        public int num { get; set; }

        // 初回獲得日付
        public DateTime getDate { get; set; }

        // NEWフラグ
        public bool newFlg { get; set; }

        // URL
        public string url { get; set; }

        // Dictionary用キー＠難易度別に２枚ある楽曲カードで重複するため
        public string key()
        {
            return name + "_" + url;
        }

        /*
         * コンストラクタ
         */
        public CollectionCard()
        {
            newFlg = false;
        }

        /*
         * ファイル読み込み用
         */
        public CollectionCard(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            // ファイルから
            no = int.Parse(data[(int)Index.NO]);
            typeNo = int.Parse(data[(int)Index.TYPE]);
            type = (CardType)Enum.Parse(typeof(CardType), data[(int)Index.TYPE_NO]);
            name = data[(int)Index.NAME];
            num = int.Parse(data[(int)Index.NUM]);
            getDate = DateTime.Parse(data[(int)Index.GET_DATE]);
            newFlg = bool.Parse(data[(int)Index.NEW_FLG]);
            url = data[(int)Index.URL];
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(no.ToString() + SEPALATOR);
            sb.Append(typeNo + SEPALATOR);
            sb.Append(type + SEPALATOR);
            sb.Append(name + SEPALATOR);
            sb.Append(num.ToString() + SEPALATOR);
            sb.Append(getDate.ToString(DATE_FORMAT) + SEPALATOR);
            sb.Append(newFlg.ToString() + SEPALATOR);
            sb.Append(url);

            return sb.ToString();
        }


        private string getTypeName()
        {
            string tn;
            switch (type)
            {
                case CardType.SONG:
                    tn = "楽曲";
                    break;

                // 楽曲分、インデックスを差し引く
                default:
                    tn = DivaNetLogic.VOCALOID_NAME[(int)type - (int)CardType.MIKU];
                    break;
            }

            return tn;
        }
    }
}
