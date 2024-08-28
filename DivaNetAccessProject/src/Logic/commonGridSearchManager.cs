using DivaNetAccess.src.util;
using DivaNetAccess.src.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.Logic
{
    public static class CommonGridSearchManager
    {
        #region Grid操作

        /*
         * 検索処理
         */
        public static void searchGrid(DataGridView view, string searchStr)
        {
            bool reViewFlg = view.Columns[0].Visible;

            //バインドされているDataTableを取得
            DataTable dt = (DataTable)view.DataSource;

            //DataViewを取得
            DataView dv = dt.DefaultView;

            //並び替えを行う
            string sortStr = searchStr;
            dv.RowFilter = sortStr;

            /*
             * 不具合：上の"dt.Rows.Add(dr);"を行うと_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            view.Columns[0].Visible = reViewFlg;
        }

        #endregion

        #region 検索文字列生成

        /*
         * 結合条件(AND)を付与する
         */
        public static string addSearchStrPlefix(StringBuilder ret, string searchStr)
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
         * チェックボックスの状態を検索するSQLを生成＠チェックありの場合のみ
         * 楽曲検索_NewRecordなど
         */
        public static string getChkboxSearchStrBool(string columnName, CheckBox chk)
        {
            StringBuilder ret = new StringBuilder();
            List<CheckBox> chkTrues = new List<CheckBox>();

            // チェックが入っている時のみ検索条件に追加する
            if (chk.Checked)
            {
                ret.Append(columnName + " = " + chk.Checked.ToString());
                return "(" + ret.ToString() + ")";
            }
            else
            {
                return ret.ToString();
            }
        }



        /*
         * Text値を検索するSQLを生成
         * 　画面のラベル文字列→検索する文字列のマッピングを行う
         */
        public static string getChkboxSearchMapping(string columnName, CheckBox[] chks, Dictionary<string, string> mappings)
        {
            StringBuilder ret = new StringBuilder();
            List<CheckBox> chkTrues = new List<CheckBox>();

            // チェック数カウント
            foreach (CheckBox chk in chks)
            {
                // チェックされているか
                if (chk.Checked)
                {
                    chkTrues.Add(chk);
                }
            }

            // チェックなし
            if (1 > chkTrues.Count)
            {
                return "";
            }

            ret.Append(columnName + " = ");

            // チェックされたチェックボックスのText値
            for (int i = 0; i < chkTrues.Count; i++)
            {
                if (mappings.ContainsKey(chkTrues[i].Text))
                {
                    ret.Append("'" + mappings[chkTrues[i].Text] + "'");

                    if (i != chkTrues.Count - 1)
                    {
                        ret.Append(" or " + columnName + " = ");
                    }
                }
            }

            return "(" + ret.ToString() + ")";
        }

        /*
         * 難易度のText値を検索するSQLを生成
         * 　画面の難易度ラベル→検索する難易度文字列のマッピングを行う
         */
        public static string getChkboxSearchDiff(string columnName, CheckBox[] chks)
        {
            // マッピング用コレクション
            Dictionary<string, string> diffMap = new Dictionary<string, string>();
            diffMap.Add("E", "EASY");
            diffMap.Add("N", "NORMAL");
            diffMap.Add("H", "HARD");
            diffMap.Add("EX", "EXTREME");
            diffMap.Add("EX EXT", "EX EXTREME");

            return getChkboxSearchMapping(columnName, chks, diffMap);
        }

        /*
         * ★のText値を検索するSQLを生成
         */
        public static string getTextBoxSearchStar(string columnName, TextBox textBox)
        {
            StringBuilder ret = new StringBuilder();
            string target = textBox.Text.Trim();

            if (string.IsNullOrEmpty(target))
            {
                return "";
            }

            char splitAnd = ',';
            char splitBetween = '-';

            // 区切り文字変換、全角→半角、空白除去
            target = ToolUtil.convNum(target);
            target = ToolUtil.convKigo(target);
            target = target.Replace(" ", "");

            string[] stars = target.Split(splitAnd);

            for (int i = 0; i < stars.Length; i++)
            {
                // 範囲指定文字で分割
                string[] starBetweens = stars[i].Split(splitBetween);
                if (starBetweens.Length > 2)
                {
                    return "";
                }

                string res = WebUtil.getMache(stars[0], "\\d+(\\.\\d+)?");

                // 整数 or 整数＋小数チェック
                if (string.IsNullOrEmpty(res))
                {
                    return "";
                }
                else
                {
                    if (starBetweens.Length > 1)
                    {
                        // 範囲指定あり
                        ret.Append("(" + columnName + " >= " + starBetweens[0] + " AND " + columnName + "<=" + starBetweens[1] + ")");
                    }
                    else
                    {
                        // 範囲指定なし
                        ret.Append("(" + columnName + " = " + starBetweens[0] + ")");
                    }
                }

                if (i != stars.Length - 1)
                {
                    ret.Append(" OR ");
                }
            }

            return ret.ToString();
        }

        /*
         * チェックボックスのText値を検索するSQLを生成
         * CLEAR、HISPEEDなど
         */
        public static string getChkboxSearchStr(string columnName, CheckBox[] chks)
        {
            StringBuilder ret = new StringBuilder();
            List<CheckBox> chkTrues = new List<CheckBox>();

            // チェック数カウント
            foreach (CheckBox chk in chks)
            {
                // チェックされているか
                if (chk.Checked)
                {
                    chkTrues.Add(chk);
                }
            }

            // チェックなし
            if (1 > chkTrues.Count)
            {
                return "";
            }

            ret.Append(columnName + " = ");

            // チェックされたチェックボックスのText値
            for (int i = 0; i < chkTrues.Count; i++)
            {
                ret.Append("'" + chkTrues[i].Text + "'");

                if (i != chkTrues.Count - 1)
                {
                    ret.Append(" or " + columnName + " = ");
                }
            }

            return "(" + ret.ToString() + ")";
        }

        /*
         * チェックボックスのText値からコードを取得し検索するSQLを生成
         * クリア("STANDARD")等
         */
        public static string getChkboxSearchStrForClear(string columnName, CheckBox[] chks)
        {
            StringBuilder ret = new StringBuilder();
            List<CheckBox> chkTrues = new List<CheckBox>();

            // チェック数カウント
            foreach (CheckBox chk in chks)
            {
                // チェックされているか
                if (chk.Checked)
                {
                    chkTrues.Add(chk);
                }
            }

            // チェックなし
            if (1 > chkTrues.Count)
            {
                return "";
            }

            ret.Append(columnName + " = ");

            // チェックされたチェックボックスのインデックス値
            for (int i = 0; i < chkTrues.Count; i++)
            {
                ret.Append(WebUtil.getClearIndexChar(chkTrues[i].Text));

                if (i != chkTrues.Count - 1)
                {
                    ret.Append(" or " + columnName + " = ");
                }
            }

            return "(" + ret.ToString() + ")";
        }

        /*
         * ViewFlgを検索するSQLを生成
         */
        public static string getChkboxSearchStrForViewFlg(string columnName, CheckBox chk)
        {
            StringBuilder ret = new StringBuilder();

            // チェックが付いていなかったら、ViewFlgがTrueの楽曲を表示
            if (!chk.Checked)
            {
                ret.Append("(" + columnName + " = true)");
            }

            return ret.ToString();
        }

        /*
         * テキストボックスのText値で部分一致検索をするSQLを生成
         * 検索用文字列設定
         */
        public static string getNameSearchStrLike(string columnName, TextBox textBox)
        {
            StringBuilder ret = new StringBuilder();

            // テキストボックスのText値を設定
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                ret.Append(columnName + " like ");
                ret.Append(" '%");
                ret.Append(textBox.Text);
                ret.Append("%' ");
            }

            // ()を加える
            if (ret != null && ret.Length == 0)
            {
                return "";
            }
            else
            {
                return "(" + ret.ToString() + ")";
            }
        }

        /*
         * テキストボックスのText値の範囲で検索するSQLを生成
         * 検索用文字列設定
         * 達成率など
         */
        public static string getNumberSearchStr(string columnName, TextBox textBoxOver, TextBox textBoxUnder)
        {
            StringBuilder ret = new StringBuilder();
            float target;

            // 達成率_以上
            if (float.TryParse(textBoxOver.Text, out target))
            {
                ret.Append(columnName + " >= ");
                ret.Append(target.ToString());
            }
            else
            {
                textBoxOver.Text = "";
            }

            // 達成率_ 以下
            if (float.TryParse(textBoxUnder.Text, out target))
            {
                // 結合条件文字列を加える
                addSearchStrPlefix(ret, columnName + " <= " + target.ToString());
            }
            else
            {
                textBoxUnder.Text = "";
            }

            // ()を加える
            if (ret != null && ret.Length == 0)
            {
                return "";
            }
            else
            {
                return "(" + ret.ToString() + ")";
            }
        }

        /*
         * テキストボックスのText値の範囲で検索するSQLを生成(yyyyMMdd型)
         * 検索用文字列設定
         * 日付など
         */
        public static string getDateSearchStr(string columnName, TextBox textBoxBefore, TextBox textBoxAfter)
        {
            string inputFormat = "yyyyMMdd";
            StringBuilder ret = new StringBuilder();

            DateTime target;

            // 更新日_以前
            if (DateTime.TryParseExact(
                textBoxBefore.Text, inputFormat, null, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out target))
            {
                ret.Append(columnName + " <= ");
                DateTime _dateBefore = DateTime.ParseExact(textBoxBefore.Text, inputFormat, DateTimeFormatInfo.InvariantInfo);
                ret.Append("'" + _dateBefore.ToString() + "'");
            }
            else
            {
                textBoxBefore.Text = "";
            }

            // 更新日_以前
            if (DateTime.TryParseExact(
                textBoxAfter.Text, inputFormat, null, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out target))
            {
                // 結合条件文字列を加える
                //addSearchStrPlefix(ret, colName + " >= " + "'" + target.ToString() + "'");
                DateTime _dateAfter = DateTime.ParseExact(textBoxAfter.Text, inputFormat, DateTimeFormatInfo.InvariantInfo);
                CommonGridSearchManager.addSearchStrPlefix(ret, columnName + " >= " + "'" + _dateAfter.ToString() + "'");
            }
            else
            {
                textBoxAfter.Text = "";
            }

            // ()を加える
            if (ret != null && ret.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Format("( {0} )", ret.ToString());
            }
        }

        /*
         * パネルに含まれるラジオボタンのText値を検索するSQLを生成
         * 検索用文字列設定
         * プレイ履歴のオプションなど
         */
        public static string getPanelSearchStr(string columnName, Panel p)
        {
            StringBuilder ret = new StringBuilder();

            foreach (RadioButton r in p.Controls)
            {
                if (r.Checked)
                {
                    if (r.Text != "なし")
                    {
                        ret.Append(r.Text);
                        break;
                    }
                }
            }

            if (ret.Length > 0)
            {
                return string.Format("({0} = '{1}')", columnName, ret.ToString());
            }
            else
            {
                return "";
            }
        }

        /*
         * 選択されたマイリストに含まれる楽曲名で完全一致検索をするSQLを生成
         */
        public static string getNameSearchStrIn(string columnName, string[] datas)
        {
            StringBuilder ret = new StringBuilder();

            for (int i = 0; i < datas.Length; i++)
            {
                // 曲名はエスケープする
                ret.Append(string.Format("'{0}'", datas[i].Replace("'", "''")));

                if (i < datas.Length - 1)
                {
                    ret.Append(", ");
                }
            }

            // ()を加える
            if (ret.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Format("{0} in ( {1} )", columnName, ret.ToString());
            }
        }

        /*
         * 未プレイの楽曲は表示しない
         * 検索用文字列設定
         */
        public static string getNotPlaySearchStr(string colTasseiritsu, CheckBox chkIsNotPlay)
        {
            StringBuilder ret = new StringBuilder();

            if (chkIsNotPlay.Checked)
            {
                ret.Append(string.Format("{0} > 0 ", colTasseiritsu));
            }

            // ()を加える
            if (ret != null && ret.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Format("( {0} )", ret.ToString());
            }
        }

        #endregion
    }
}
