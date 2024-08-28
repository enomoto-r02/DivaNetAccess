using DivaNetAccess.src.myList;
using System.Text;

namespace DivaNetAccess.src
{
    public class SearchRivalCompare
    {
        // 難易度
        public string diffSearchStr;

        // ★
        public string starSearchStr;

        // 曲名
        public string nameSearchStr;

        // 達成率
        public string tasserituSearchStr;

        // 達成率(ライバル)
        public string tasserituRivalSearchStr;

        // 達成率(差)
        public string tasserituSaSearchStr;

        // スコア
        public string scoreSearchStr;

        // スコア(ライバル)
        public string scoreRivalSearchStr;

        // スコア(差)
        public string scoreSaSearchStr;

        // 自分が未プレイの楽曲は表示しない
        public string playerNotPlaySearchStr;

        // ライバルが未プレイの楽曲は表示しない
        public string rivalNotPlaySearchStr;

        // マイリスト
        public string myListSearchStr;

        // マイリスト
        private readonly MyListEntity myListEnt;

        // 現在配信されていない楽曲を表示する
        public string viewFlgStr;

        /*
         * コンストラクタ
         */
        //public searchRivalCompare()
        //{
        //}

        /*
         * コンストラクタ
         */
        public SearchRivalCompare(MyListEntity myListEnt)
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

            // 検索項目文字列すべて
            string[] searchStrs = {
                diffSearchStr, starSearchStr, nameSearchStr,
                tasserituSearchStr, tasserituRivalSearchStr, tasserituSaSearchStr,
                scoreSearchStr, scoreRivalSearchStr, scoreSaSearchStr,
                myListSearchStr, playerNotPlaySearchStr, rivalNotPlaySearchStr, viewFlgStr,
            };

            foreach (string searchStr in searchStrs)
            {
                // 結合条件文字列を加える
                addSearchStrPlefix(ret, searchStr);
            }

            return ret.ToString();
        }

        /*
         * 結合条件(AND)を付与する
         */
        public string addSearchStrPlefix(StringBuilder ret, string searchStr)
        {
            // " AND "を先頭に加える
            if (string.IsNullOrEmpty(searchStr) == false)
            {
                if (ret != null && ret.Length > 0)
                {
                    ret.Append(" AND ");
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
