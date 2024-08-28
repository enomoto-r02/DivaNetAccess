using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.Logic
{
    public class SearchPlayRecordDetail
    {
        // 難易度
        public string diffSearchStr;

        // ★
        public string starSearchStr;

        // CLEAR
        public string clearSearchStr;

        // 曲名
        public string songNameSearchStr;

        // 日時   
        public string dateSearchStr;

        // 場所
        public string placeSearchStr;

        // 達成率
        public string tasserituSearchStr;

        // 達成率_更新
        public string tasserituNewRecordSearchStr;

        // スコア
        public string scoreSearchStr;

        // スコア_更新
        public string scoreNewRecordSearchStr;

        // COOL率
        public string coolPSearchStr;

        // FINE率
        public string finePSearchStr;

        // SAFE率
        public string safePSearchStr;

        // SAD率
        public string sadPSearchStr;

        // WORST/WRONG率
        public string worstPSearchStr;

        // 同時押し/HOLD率
        public string holdSearchStr;

        // TRIAL
        public string trialSearchStr;

        // オプション
        public string optionSearchStr;

        // メモ
        public string memoSearchStr;

        // 
        public string viewFlgSearchStr;

        /*
         * コンストラクタ
         */
        public SearchPlayRecordDetail()
        {
            diffSearchStr = "";
            starSearchStr = "";
            clearSearchStr = "";
            songNameSearchStr = "";
            dateSearchStr = "";
            placeSearchStr = "";
            tasserituSearchStr = "";
            tasserituNewRecordSearchStr = "";
            scoreSearchStr = "";
            scoreNewRecordSearchStr = "";
            coolPSearchStr = "";
            finePSearchStr = "";
            safePSearchStr = "";
            sadPSearchStr = "";
            worstPSearchStr = "";
            holdSearchStr = "";
            trialSearchStr = "";
            optionSearchStr = "";
            memoSearchStr = "";
            viewFlgSearchStr = "";
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
                diffSearchStr, starSearchStr, clearSearchStr,
                songNameSearchStr, dateSearchStr, placeSearchStr,
                tasserituSearchStr, tasserituNewRecordSearchStr,
                scoreSearchStr, scoreNewRecordSearchStr,
                coolPSearchStr, finePSearchStr, safePSearchStr,
                sadPSearchStr, worstPSearchStr, holdSearchStr,
                trialSearchStr, optionSearchStr, memoSearchStr, viewFlgSearchStr
            };

            foreach (string searchStr in searchStrs)
            {
                // 結合条件文字列を加える
                CommonGridSearchManager.addSearchStrPlefix(ret, searchStr);
            }

            return ret.ToString();
        }

        #region 検索用文字列設定

        /*
         * 難易度
         * 検索用文字列設定
         */
        public void setDiffSearchStr(CheckBox[] diffs)
        {
            diffSearchStr = CommonGridSearchManager.getChkboxSearchDiff("diff", diffs);
        }

        /*
         * ★
         * 検索用文字列設定
         */
        public void setStarSearchStr(TextBox star)
        {
            starSearchStr = CommonGridSearchManager.getTextBoxSearchStar("star", star);
        }

        /*
         * CLEAR
         * 検索用文字列設定
         */
        public void setClearSearchStr(CheckBox[] clears)
        {
            // インデックス列を検索
            clearSearchStr = CommonGridSearchManager.getChkboxSearchStrForClear("_clearIndex", clears);
        }

        /*
         * TRIAL
         * 検索用文字列設定
         */
        public void setTrialSearchStr(CheckBox[] trials)
        {
            // インデックス列を検索
            trialSearchStr = CommonGridSearchManager.getChkboxSearchStrForClear("_trialIndex", trials);
        }

        /*
         * 曲名
         * 検索用文字列設定
         */
        public void setSongNameSearchStr(TextBox songName)
        {
            songNameSearchStr = CommonGridSearchManager.getNameSearchStrLike("name", songName);
        }

        /*
         * 日時
         * 検索用文字列設定
         */
        public void setDateSearchStr(TextBox dateOver, TextBox dateUnder)
        {
            dateSearchStr = CommonGridSearchManager.getDateSearchStr("date", dateOver, dateUnder);
        }

        /*
         * 場所
         * 検索用文字列設定
         */
        public void setPlaceSearchStr(TextBox textBox)
        {
            placeSearchStr = CommonGridSearchManager.getNameSearchStrLike("place", textBox);
        }

        /*
         * 達成率
         * 検索用文字列設定
         */
        public void setTasseirituSearchStr(TextBox tasseirituOver, TextBox tasseirituUnder)
        {
            tasserituSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseiritu", tasseirituOver, tasseirituUnder);
        }

        /*
         * 達成率_更新
         * 検索用文字列設定
         */
        public void setTasseirituNewRecordSearchStr(CheckBox tasseirituNewRecord)
        {
            tasserituNewRecordSearchStr = CommonGridSearchManager.getChkboxSearchStrBool("_tasseirituNewRecord", tasseirituNewRecord);
        }

        /*
         * スコア
         * 検索用文字列設定
         */
        public void setScoreSearchStr(TextBox scoreOver, TextBox scoreUnder)
        {
            scoreSearchStr = CommonGridSearchManager.getNumberSearchStr("score", scoreOver, scoreUnder);
        }

        /*
         * スコア_更新
         * 検索用文字列設定
         */
        public void setScoreNewRecordSearchStr(CheckBox scoreNewRecord)
        {
            scoreNewRecordSearchStr = CommonGridSearchManager.getChkboxSearchStrBool("_scoreNewRecord", scoreNewRecord);
        }

        /*
         * COOL率
         * 検索用文字列設定
         */
        public void setCoolPSearchStr(TextBox coolPOver, TextBox coolPUnder)
        {
            coolPSearchStr = CommonGridSearchManager.getNumberSearchStr("coolP", coolPOver, coolPUnder);
        }

        /*
         * FINE率
         * 検索用文字列設定
         */
        public void setFinePSearchStr(TextBox finePOver, TextBox finePUnder)
        {
            finePSearchStr = CommonGridSearchManager.getNumberSearchStr("fineP", finePOver, finePUnder);
        }

        /*
         * SAFE率
         * 検索用文字列設定
         */
        public void setSafePSearchStr(TextBox safePOver, TextBox safePUnder)
        {
            safePSearchStr = CommonGridSearchManager.getNumberSearchStr("safeP", safePOver, safePUnder);
        }

        /*
         * SAD率
         * 検索用文字列設定
         */
        public void setSadPSearchStr(TextBox sadPOver, TextBox sadPUnder)
        {
            sadPSearchStr = CommonGridSearchManager.getNumberSearchStr("sadP", sadPOver, sadPUnder);
        }

        /*
         * WORST/WRONG率
         * 検索用文字列設定
         */
        public void setWorstPSearchStr(TextBox worstPOver, TextBox worstPUnder)
        {
            worstPSearchStr = CommonGridSearchManager.getNumberSearchStr("worstP", worstPOver, worstPUnder);
        }

        /*
         * 同時押し/HOLD
         * 検索用文字列設定
         */
        public void setHoldSearchStr(TextBox holdOver, TextBox holdUnder)
        {
            holdSearchStr = CommonGridSearchManager.getNumberSearchStr("hold", holdOver, holdUnder);
        }

        /*
         * オプション
         * 検索用文字列設定
         */
        public void setOptionSearchStr(Panel optionPanel)
        {
            optionSearchStr = CommonGridSearchManager.getPanelSearchStr("option", optionPanel);
        }

        /*
         * メモ
         * 検索用文字列設定
         */
        public void setMemoSearchStr(TextBox textBox)
        {
            memoSearchStr = CommonGridSearchManager.getNameSearchStrLike("memo", textBox);
        }

        #endregion
    }
}
