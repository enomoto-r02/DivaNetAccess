using DivaNetAccess.src.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DivaNetAccess
{
    // 楽曲データグリッドのソート処理クラス
    //
    // 既存列の途中に割り込みで追加厳禁
    // (メモ欄更新時に保存の挙動がおかしくなるため)
    //
    public static class SongGridLogic
    {
        private const string SET_NAME = "songDataSet";
        private const string TABLE_NAME = "songDataTable";

        // SongDataGridView用列挙型
        public enum SongGridIndex
        {
            KOUKAI_ORDER = 0,
            NAME_ORDER,
            DIFF_INDEX,
            CLEAR_INDEX,
            TRIAL_INDEX,
            NO,
            DIFF,
            STAR,
            NAME,
            CLEAR,
            TRIAL,
            HISPEED,
            HIDDEN,
            SUDDEN,
            TASSEIRITU,
            RIRON,
            SA,
            SCORE,
            RENZOKU_MAX,
            RENZOKU_NOW,
            DATE,
            RANK,
            MEMO,
            VIEW_FLG,
        }

        /*
         * 初期設定
         */
        public static void initDataGridView(DataGridView view, ContextMenuStrip cms)
        {
            DataSet ds = new DataSet(SET_NAME);
            DataTable dt = new DataTable(TABLE_NAME);

            // テーブルをデータセットに設定
            ds.Tables.Add(dt);

            view.DataSource = dt;

            // 行ヘッダー追加
            initColumnAdd(dt);

            // 行全体の情報設定
            for (int i = 0; i < view.Columns.Count; i++)
            {
                view.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                view.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                view.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                view.Columns[i].ReadOnly = true;
                view.Columns[i].HeaderCell.ContextMenuStrip = cms;
            }

            // 行個別の情報設定
            initColumnWidth(view);

            /*
             * DisplayIndexを使用する場合はこの値をfalseにする必要がある
             * 引用元：http://jehupc.exblog.jp/9077976/
             */
            view.AutoGenerateColumns = false;
        }

        /*
         * 行ヘッダー追加
         */
        public static void initColumnAdd(DataTable dt)
        {
            // 公開順
            dt.Columns.Add("_koukaiOrder", typeof(int));

            // 曲名順
            dt.Columns.Add("_nameOrder", typeof(int));

            // 難易度インデックス
            dt.Columns.Add("_diffIndex", typeof(int));

            // クリアインデックス
            dt.Columns.Add("_clearIndex", typeof(int));

            // トライアルインデックス
            dt.Columns.Add("_trialIndex", typeof(int));

            // No
            dt.Columns.Add("no", typeof(int));

            // 難易度
            dt.Columns.Add("diff", Type.GetType("System.String"));

            // ★
            dt.Columns.Add("star", typeof(float));

            // 曲名
            dt.Columns.Add("name", Type.GetType("System.String"));

            // クリア
            dt.Columns.Add("clear", Type.GetType("System.String"));

            // トライアル
            dt.Columns.Add("trial", Type.GetType("System.String"));

            // HISPEED
            dt.Columns.Add("hispeed", Type.GetType("System.String"));

            // HIDDEN
            dt.Columns.Add("hidden", Type.GetType("System.String"));

            // SUDDEN
            dt.Columns.Add("sudden", Type.GetType("System.String"));

            // 達成率
            dt.Columns.Add("tasseiritu", typeof(float));

            // 理論値
            dt.Columns.Add("tasseirituRiron", typeof(float));

            // 差
            dt.Columns.Add("tasseirituRironSa", typeof(float));

            // スコア
            dt.Columns.Add("score", typeof(int));

            // 連続パーフェクトトライアル_現在
            dt.Columns.Add("trialNow", typeof(int));

            // 連続パーフェクトトライアル_最高
            dt.Columns.Add("trialMax", typeof(int));

            // 更新日
            dt.Columns.Add("date", typeof(DateTime));

            // 順位
            dt.Columns.Add("rank", typeof(int));

            // メモ
            dt.Columns.Add("memo", Type.GetType("System.String"));

            // 
            dt.Columns.Add("_viewFlg", typeof(bool));
        }

        /*
         * 行の情報設定
         */
        public static void initColumnWidth(DataGridView view)
        {
            // ここら辺はプロパティファイルでも作る？
            view.Columns[(int)SongGridIndex.KOUKAI_ORDER].Visible = false;
            view.Columns[(int)SongGridIndex.NAME_ORDER].Visible = false;
            view.Columns[(int)SongGridIndex.DIFF_INDEX].Visible = false;
            view.Columns[(int)SongGridIndex.CLEAR_INDEX].Visible = false;
            view.Columns[(int)SongGridIndex.TRIAL_INDEX].Visible = false;

            view.Columns[(int)SongGridIndex.NO].Width = 30;
            view.Columns[(int)SongGridIndex.NO].HeaderText = "No";

            view.Columns[(int)SongGridIndex.DIFF].Width = 80;
            view.Columns[(int)SongGridIndex.DIFF].HeaderText = "難易度";

            view.Columns[(int)SongGridIndex.STAR].HeaderText = "★";
            view.Columns[(int)SongGridIndex.STAR].Width = 30;

            view.Columns[(int)SongGridIndex.NAME].Width = 250;
            view.Columns[(int)SongGridIndex.NAME].HeaderText = "曲名";
            view.Columns[(int)SongGridIndex.NAME].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns[(int)SongGridIndex.CLEAR].Width = 50;
            view.Columns[(int)SongGridIndex.CLEAR].HeaderText = "CLEAR";

            view.Columns[(int)SongGridIndex.TRIAL].Width = 50;
            view.Columns[(int)SongGridIndex.TRIAL].HeaderText = "TRIAL";

            view.Columns[(int)SongGridIndex.HISPEED].Width = 30;
            view.Columns[(int)SongGridIndex.HISPEED].HeaderText = "HIS";

            view.Columns[(int)SongGridIndex.HIDDEN].Width = 30;
            view.Columns[(int)SongGridIndex.HIDDEN].HeaderText = "HID";

            view.Columns[(int)SongGridIndex.SUDDEN].Width = 30;
            view.Columns[(int)SongGridIndex.SUDDEN].HeaderText = "SUD";

            view.Columns[(int)SongGridIndex.TASSEIRITU].Width = 60;
            view.Columns[(int)SongGridIndex.TASSEIRITU].HeaderText = "達成率";
            view.Columns[(int)SongGridIndex.TASSEIRITU].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)SongGridIndex.RIRON].Width = 60;
            view.Columns[(int)SongGridIndex.RIRON].HeaderText = "理論値";
            view.Columns[(int)SongGridIndex.RIRON].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)SongGridIndex.SA].Width = 60;
            view.Columns[(int)SongGridIndex.SA].HeaderText = "差";
            view.Columns[(int)SongGridIndex.SA].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)SongGridIndex.SCORE].Width = 50;
            view.Columns[(int)SongGridIndex.SCORE].HeaderText = "スコア";

            view.Columns[(int)SongGridIndex.RENZOKU_NOW].Width = 60;
            view.Columns[(int)SongGridIndex.RENZOKU_NOW].HeaderText = "連P現在";

            view.Columns[(int)SongGridIndex.RENZOKU_MAX].Width = 60;
            view.Columns[(int)SongGridIndex.RENZOKU_MAX].HeaderText = "連P最高";

            view.Columns[(int)SongGridIndex.TASSEIRITU].Width = 60;
            view.Columns[(int)SongGridIndex.TASSEIRITU].HeaderText = "達成率";
            view.Columns[(int)SongGridIndex.TASSEIRITU].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)SongGridIndex.DATE].Width = 70;
            view.Columns[(int)SongGridIndex.DATE].HeaderText = "更新日";

            view.Columns[(int)SongGridIndex.RANK].Width = 50;
            view.Columns[(int)SongGridIndex.RANK].HeaderText = "順位";

            view.Columns[(int)SongGridIndex.MEMO].Width = 200;
            view.Columns[(int)SongGridIndex.MEMO].HeaderText = "メモ";
            view.Columns[(int)SongGridIndex.MEMO].ReadOnly = false;
            view.Columns[(int)SongGridIndex.MEMO].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns[(int)SongGridIndex.VIEW_FLG].Visible = true;
        }

        /*
         * 行にデータ追加
         */
        public static void addGrid(DataGridView view, List<SongGridData> viewDatas)
        {
            DataTable dt = (DataTable)view.DataSource;

            // 行のクリア
            dt.Rows.Clear();

            //今までの並び替えグリフを消す
            foreach (DataGridViewColumn col in view.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
                col.HeaderCell.Style.BackColor = view.ColumnHeadersDefaultCellStyle.BackColor;
            }

            // 行のデータ追加
            foreach (SongGridData data in viewDatas)
            {

                DataRow dr = dt.NewRow();
                dr[(int)SongGridIndex.KOUKAI_ORDER] = data._koukaiOrder;
                dr[(int)SongGridIndex.NAME_ORDER] = data._nameOrder;
                dr[(int)SongGridIndex.DIFF_INDEX] = data._diffIndex;
                dr[(int)SongGridIndex.CLEAR_INDEX] = data._clearIndex;
                dr[(int)SongGridIndex.TRIAL_INDEX] = data._trialIndex;
                dr[(int)SongGridIndex.NO] = dt.Rows.Count + 1;
                dr[(int)SongGridIndex.NAME] = data.name;
                dr[(int)SongGridIndex.STAR] = data.star;
                dr[(int)SongGridIndex.DIFF] = data.diff;
                dr[(int)SongGridIndex.CLEAR] = data.clear;
                dr[(int)SongGridIndex.TRIAL] = data.trial;
                dr[(int)SongGridIndex.TASSEIRITU] = data.tasseirituV();

                if (data.tasseirituRironV() != 0)
                {
                    dr[(int)SongGridIndex.RIRON] = data.tasseirituRironV();
                    dr[(int)SongGridIndex.SA] = data.tasseirituRironSaV();
                }
                dr[(int)SongGridIndex.SCORE] = data.score;
                // 連続パフェトラ解禁済
                if (data.trial_max != -1)
                {
                    dr[(int)SongGridIndex.RENZOKU_NOW] = data.trial_now;
                    dr[(int)SongGridIndex.RENZOKU_MAX] = data.trial_max;
                }

                dr[(int)SongGridIndex.HISPEED] = data.hispeed;
                dr[(int)SongGridIndex.HIDDEN] = data.hidden;
                dr[(int)SongGridIndex.SUDDEN] = data.sudden;

                if (data.date != null)
                {
                    dr[(int)SongGridIndex.DATE] = data.date;
                }
                if (data.rank != 0)
                {
                    dr[(int)SongGridIndex.RANK] = data.rank;
                }
                dr[(int)SongGridIndex.MEMO] = data.memo;

                dr[(int)SongGridIndex.VIEW_FLG] = data._viewFlg;

                dt.Rows.Add(dr);
            }

            /*
             * 不具合：上の"dt.Rows.Add(dr);"を行うと_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            view.Columns[(int)SongGridIndex.KOUKAI_ORDER].Visible = false;

        }


        public static void execSort(DataGridView view, DataGridViewCellMouseEventArgs e)
        {
            execSort2(view, e.ColumnIndex);
        }

        public static void execSort2(DataGridView view, int selectedColumnIndex)
        {
            // 現在の検索条件を取得する
            DataTable dt = (DataTable)view.DataSource;
            DataView dv = dt.DefaultView;
            string sortStr = dv.RowFilter;

            // 横スクロールの位置を退避
            int n = view.HorizontalScrollingOffset;
            int n2 = view.FirstDisplayedScrollingRowIndex;

            // ソート対象カラム取得
            int colIndex = selectedColumnIndex;

            // 曲名ソート→内部的に曲名順ソート
            if ((int)SongGridIndex.NAME == selectedColumnIndex)
            {
                colIndex = (int)SongGridIndex.NAME_ORDER;
            }
            // 難易度ソート→内部的に難易度インデックスソート
            else if ((int)SongGridIndex.DIFF == selectedColumnIndex)
            {
                colIndex = (int)SongGridIndex.DIFF_INDEX;
            }
            // クリアソート→内部的にクリアインデックスソート
            else if ((int)SongGridIndex.CLEAR == selectedColumnIndex)
            {
                colIndex = (int)SongGridIndex.CLEAR_INDEX;
            }
            // トライアルソート→内部的にトライアルインデックスソート
            else if ((int)SongGridIndex.TRIAL == selectedColumnIndex)
            {
                colIndex = (int)SongGridIndex.TRIAL_INDEX;
            }

            //並び替えの方向（昇順か降順か）を決める
            ListSortDirection sortDirection =
                    view.Columns[selectedColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending ||
                    view.Columns[selectedColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ?
                    ListSortDirection.Ascending : ListSortDirection.Descending;

            SortOrder sortOrder =
                sortDirection == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;

            // ソート対象列がNullでない行を取得
            DataRow[] notNullRows = dt.Select(
                view.Columns[selectedColumnIndex].Name + " Is Not Null",
                view.Columns[colIndex].Name + " " + getSortOrderStr(sortOrder)
            );

            // ソート対象列がNullの行を取得
            string twoSortStr = "_nameOrder ASC";
            DataRow[] nullRows = dt.Select(
                view.Columns[selectedColumnIndex].Name + " Is Null",
                twoSortStr       // 第２ソートは曲名の昇順
            );

            // テーブルのクリア
            dt = dt.Clone();

            // ソートされた行を追加
            foreach (DataRow notNullRow in notNullRows)
            {
                dt.ImportRow(notNullRow);
            }

            // Nullの行を追加
            foreach (DataRow nullRow in nullRows)
            {
                dt.ImportRow(nullRow);
            }

            // データソースに設定
            view.DataSource = dt;

            // ソート前の条件で検索する
            CommonGridSearchManager.searchGrid(view, sortStr);

            //今までの並び替えグリフを消す
            foreach (DataGridViewColumn col in view.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
                col.HeaderCell.Style.BackColor = view.ColumnHeadersDefaultCellStyle.BackColor;
            }

            //並び替えグリフを変更
            view.Columns[selectedColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
            view.Columns[selectedColumnIndex].HeaderCell.Style.BackColor = Color.LightYellow;

            // 横スクロールの位置を復帰
            view.HorizontalScrollingOffset = n;
            if (n2 >= 0)
            {
                view.FirstDisplayedScrollingRowIndex = n2;
            }

            /*
             * 不具合：上の"dt.Rows.Add(dr);"を行うと_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            view.Columns[(int)SongGridIndex.KOUKAI_ORDER].Visible = false;


            // 検索条件を保持する
            //view.SearchStr = searchStr;
        }

        /*
         * ソート処理
         */
        public static void execSort_org(DataGridView view, DataGridViewCellMouseEventArgs e)
        {
            // 現在の検索条件を取得する
            DataTable dt = (DataTable)view.DataSource;
            DataView dv = dt.DefaultView;
            string sortStr = dv.RowFilter;

            // 横スクロールの位置を退避
            int n = view.HorizontalScrollingOffset;
            int n2 = view.FirstDisplayedScrollingRowIndex;

            // ソート対象カラム取得
            int colIndex = e.ColumnIndex;

            // 曲名ソート→内部的に曲名順ソート
            if ((int)SongGridIndex.NAME == e.ColumnIndex)
            {
                colIndex = (int)SongGridIndex.NAME_ORDER;
            }
            // 難易度ソート→内部的に難易度インデックスソート
            else if ((int)SongGridIndex.DIFF == e.ColumnIndex)
            {
                colIndex = (int)SongGridIndex.DIFF_INDEX;
            }
            // クリアソート→内部的にクリアインデックスソート
            else if ((int)SongGridIndex.CLEAR == e.ColumnIndex)
            {
                colIndex = (int)SongGridIndex.CLEAR_INDEX;
            }
            // トライアルソート→内部的にトライアルインデックスソート
            else if ((int)SongGridIndex.TRIAL == e.ColumnIndex)
            {
                colIndex = (int)SongGridIndex.TRIAL_INDEX;
            }

            //並び替えの方向（昇順か降順か）を決める
            ListSortDirection sortDirection =
                    view.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending ||
                    view.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ?
                    ListSortDirection.Ascending : ListSortDirection.Descending;

            SortOrder sortOrder =
                sortDirection == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;

            // ソート対象列がNullでない行を取得
            DataRow[] notNullRows = dt.Select(
                view.Columns[e.ColumnIndex].Name + " Is Not Null",
                view.Columns[colIndex].Name + " " + getSortOrderStr(sortOrder)
            );

            // ソート対象列がNullの行を取得
            string twoSortStr = "_nameOrder ASC";
            DataRow[] nullRows = dt.Select(
                view.Columns[e.ColumnIndex].Name + " Is Null",
                twoSortStr       // 第２ソートは曲名の昇順
            );

            // テーブルのクリア
            dt = dt.Clone();

            // ソートされた行を追加
            foreach (DataRow notNullRow in notNullRows)
            {
                dt.ImportRow(notNullRow);
            }

            // Nullの行を追加
            foreach (DataRow nullRow in nullRows)
            {
                dt.ImportRow(nullRow);
            }

            // データソースに設定
            view.DataSource = dt;

            // ソート前の条件で検索する
            CommonGridSearchManager.searchGrid(view, sortStr);

            //今までの並び替えグリフを消す
            foreach (DataGridViewColumn col in view.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
                col.HeaderCell.Style.BackColor = view.ColumnHeadersDefaultCellStyle.BackColor;
            }

            //並び替えグリフを変更
            view.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
            view.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.LightYellow;

            // 横スクロールの位置を復帰
            view.HorizontalScrollingOffset = n;
            view.FirstDisplayedScrollingRowIndex = n2;

            //// 検索条件を保持する
            //view.SearchStr = searchStr;
        }

        /*
         * ソート情報→ソート文字列
         */
        private static string getSortOrderStr(SortOrder sortOrder)
        {
            return sortOrder == SortOrder.Ascending ? "ASC" : "DESC";
        }
    }
}
