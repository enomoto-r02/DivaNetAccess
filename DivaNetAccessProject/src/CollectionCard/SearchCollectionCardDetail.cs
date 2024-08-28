using DivaNetAccess.src.Logic;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.searchSong
{
    public class SearchCollectionCardDetail
    {
        // カード名
        public string cardNameSearchStr;

        // 種類
        public string cardTypeSearchStr;

        // 枚数
        public string maisuSearchStr;

        // 初回獲得日時
        public string getDateSearchStr;

        /*
         * コンストラクタ
         */
        public SearchCollectionCardDetail()
        {
        }

        /*
         * 全項目
         * 検索用文字列設定
         */
        public string getSearchStrAll()
        {
            StringBuilder ret = new StringBuilder();


            // 
            string[] searchStrs = new string[]{
                cardNameSearchStr, cardTypeSearchStr, maisuSearchStr, getDateSearchStr
            };

            foreach (string searchStr in searchStrs)
            {
                // 結合条件文字列を加える
                addSearchStrPlefix(ret, searchStr, "AND");
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

        #region 検索用文字列設定

        /*
         * カード名
         * 検索用文字列設定
         */
        public void setCardNameSearchStr(TextBox cardNameText)
        {
            cardNameSearchStr = CommonGridSearchManager.getNameSearchStrLike("name", cardNameText);
        }

        /*
         * 種類
         * 検索用文字列設定
         */
        public void setTypeSearchStr(CheckBox[] types)
        {
            cardTypeSearchStr = CommonGridSearchManager.getChkboxSearchStr("typeName", types);
        }

        /*
         * 枚数
         * 検索用文字列設定
         */
        public void setMaisuSearchStr(TextBox maisuOver, TextBox maisuUnder)
        {
            maisuSearchStr = CommonGridSearchManager.getNumberSearchStr("num", maisuOver, maisuUnder);
        }

        /*
         * 初回獲得日時
         * 検索用文字列設定
         */
        public void setGetDateSearchStr(TextBox beforeText, TextBox afterText)
        {
            getDateSearchStr = CommonGridSearchManager.getDateSearchStr("date", beforeText, afterText);
        }

        #endregion
    }
}
