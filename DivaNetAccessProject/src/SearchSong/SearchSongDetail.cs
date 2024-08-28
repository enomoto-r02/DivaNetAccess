using DivaNetAccess.src.myList;
using System.Text;

namespace DivaNetAccess.src.searchSong
{
    public class SearchSongDetail
    {
        // 難易度
        public string diffSearchStr;

        // ★
        public string starSearchStr;

        // 曲名
        public string nameSearchStr;

        // CLEAR
        public string clearSearchStr;

        // TRIAL
        public string trialSearchStr;

        // HISPEED
        public string hispeedSearchStr;

        // HIDDEN
        public string hiddenSearchStr;

        // SUDDEN
        public string suddenSearchStr;

        // 達成率
        public string tasserituSearchStr;

        // 理論値
        public string rironSearchStr;

        // 差
        public string saSearchStr;

        // スコア
        public string scoreSearchStr;

        // 連続パーフェクトトライアル_現在
        public string trialNowSearchStr;

        // 連続パーフェクトトライアル_現在
        public string trialMaxSearchStr;

        // 更新日
        public string dateSearchStr;

        // 順位
        public string rankSearchStr;

        // メモ
        public string memoSearchStr;

        // マイリスト
        public string myListSearchStr;

        // マイリスト
        private readonly MyListEntity myListEnt;

        // 検索フラグ
        public string viewFlgStr;

        /*
         * コンストラクタ
         */
        //public searchSongDetail()
        //{
        //}

        /*
         * コンストラクタ
         */
        public SearchSongDetail(MyListEntity myListEnt)
        {
            this.myListEnt = myListEnt;
        }

        /*
         * 全項目
         * 検索用文字列設定
         */
        public string getSearchStrAll()
        {
            StringBuilder ret = new StringBuilder();

            // オプション以外の検索項目文字列すべて
            string[] searchStrs = new string[]{
                diffSearchStr, starSearchStr, nameSearchStr, clearSearchStr, trialSearchStr,
                tasserituSearchStr, rironSearchStr, saSearchStr, scoreSearchStr,
                trialNowSearchStr, trialMaxSearchStr,
                dateSearchStr, rankSearchStr, memoSearchStr, myListSearchStr, viewFlgStr
            };

            foreach (string searchStr in searchStrs)
            {
                // 結合条件文字列を加える
                addSearchStrPlefix(ret, searchStr, "AND");
            }


            // オプションは3つの内容をOR結合する
            StringBuilder optionSearchStr = new StringBuilder();
            searchStrs = new string[] { hispeedSearchStr, hiddenSearchStr, suddenSearchStr };
            foreach (string searchStr in searchStrs)
            {
                // 結合条件文字列を加える
                addSearchStrPlefix(optionSearchStr, searchStr, "OR");
            }

            if (optionSearchStr.Length > 0)
            {
                if (ret.Length > 0)
                {
                    ret.Append(string.Format(" AND ( {0} )", optionSearchStr.ToString()));
                }
                else
                {
                    ret.Append(string.Format("( {0} )", optionSearchStr.ToString()));
                }
            }

            return ret.ToString();
        }

        /*
         * 結合条件を付与する
         */
        public string addSearchStrPlefix(StringBuilder ret, string searchStr, string plefix)
        {
            if (string.IsNullOrEmpty(searchStr) == false)
            {
                if (ret != null && ret.Length > 0)
                {
                    ret.Append(" " + plefix + " ");
                }
                ret.Append(searchStr);
            }

            return ret.ToString();
        }

        /*
         * マイリストに含まれている楽曲名を取得する
         */
        public string[] getMyListSongNameArray(int index)
        {
            return myListEnt.getMyListFromIndex(index).ToArray();
        }
    }
}
