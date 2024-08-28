using DivaNetAccess.src.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DivaNetAccess.src
{
    public static class RivalSongGridLogic
    {
        private const string SET_NAME = "songDataSetRival";
        private const string TABLE_NAME = "songDataTableRival";

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

            // 
            dt.Columns.Add("_viewFlg", typeof(bool));
        }

        /*
         * 行の情報設定
         */
        public static void initColumnWidth(DataGridView view)
        {
            // ここら辺はプロパティファイルでも作る？
            view.Columns["_koukaiOrder"].Visible = false;
            view.Columns["_nameOrder"].Visible = false;
            view.Columns["_diffIndex"].Visible = false;
            view.Columns["_clearIndex"].Visible = false;
            view.Columns["_trialIndex"].Visible = false;

            view.Columns["no"].Width = 30;
            view.Columns["no"].HeaderText = "No";

            view.Columns["diff"].Width = 80;
            view.Columns["diff"].HeaderText = "難易度";

            view.Columns["star"].HeaderText = "★";
            view.Columns["star"].Width = 30;

            view.Columns["name"].Width = 250;
            view.Columns["name"].HeaderText = "曲名";
            view.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns["clear"].Width = 50;
            view.Columns["clear"].HeaderText = "CLEAR";

            view.Columns["trial"].Width = 50;
            view.Columns["trial"].HeaderText = "TRIAL";

            view.Columns["hispeed"].Width = 30;
            view.Columns["hispeed"].HeaderText = "HIS";

            view.Columns["hidden"].Width = 30;
            view.Columns["hidden"].HeaderText = "HID";

            view.Columns["sudden"].Width = 30;
            view.Columns["sudden"].HeaderText = "SUD";

            view.Columns["tasseiritu"].Width = 60;
            view.Columns["tasseiritu"].HeaderText = "達成率";
            view.Columns["tasseiritu"].DefaultCellStyle.Format = "##0.00";

            view.Columns["tasseirituRiron"].Width = 60;
            view.Columns["tasseirituRiron"].HeaderText = "理論値";
            view.Columns["tasseirituRiron"].DefaultCellStyle.Format = "##0.00";

            view.Columns["tasseirituRironSa"].Width = 60;
            view.Columns["tasseirituRironSa"].HeaderText = "差";
            view.Columns["tasseirituRironSa"].DefaultCellStyle.Format = "##0.00";

            view.Columns["score"].Width = 50;
            view.Columns["score"].HeaderText = "スコア";

            view.Columns["trialNow"].Width = 60;
            view.Columns["trialNow"].HeaderText = "連P現在";

            view.Columns["trialMax"].Width = 60;
            view.Columns["trialMax"].HeaderText = "連P最高";

            view.Columns["date"].Width = 70;
            view.Columns["date"].HeaderText = "更新日";

            view.Columns["rank"].Width = 50;
            view.Columns["rank"].HeaderText = "順位";

            view.Columns["_viewFlg"].Visible = false;
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

                dr["_koukaiOrder"] = data._koukaiOrder;
                dr["_nameOrder"] = data._nameOrder;
                dr["_diffIndex"] = data._diffIndex;
                dr["_clearIndex"] = data._clearIndex;
                dr["_trialIndex"] = data._trialIndex;
                dr["no"] = dt.Rows.Count + 1;
                dr["name"] = data.name;
                dr["star"] = data.star;
                dr["diff"] = data.diff;
                dr["clear"] = data.clear;
                dr["trial"] = data.trial;
                dr["tasseiritu"] = data.tasseirituV();

                if (data.tasseirituRironV() != 0)
                {
                    dr["tasseirituRiron"] = data.tasseirituRironV();
                    dr["tasseirituRironSa"] = data.tasseirituRironSaV();
                }
                dr["score"] = data.score;
                // 連続パフェトラ解禁済
                if (data.trial_max != -1)
                {
                    dr["trialNow"] = data.trial_now;
                    dr["trialMax"] = data.trial_max;
                }

                dr["hispeed"] = data.hispeed;
                dr["hidden"] = data.hidden;
                dr["sudden"] = data.sudden;

                if (data.date != null)
                {
                    dr["date"] = data.date;
                }
                if (data.rank != 0)
                {
                    dr["rank"] = data.rank;
                }

                dr["_viewFlg"] = data._viewFlg;

                dt.Rows.Add(dr);
            }


            /*
             * 不具合：上の"dt.Rows.Add(dr);"を行うと_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            view.Columns["_koukaiOrder"].Visible = false;
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
            if ((int)RivalSongGridLogic.SongGridIndex.NAME == selectedColumnIndex)
            {
                colIndex = (int)RivalSongGridLogic.SongGridIndex.NAME_ORDER;
            }
            // 難易度ソート→内部的に難易度インデックスソート
            else if ((int)RivalSongGridLogic.SongGridIndex.DIFF == selectedColumnIndex)
            {
                colIndex = (int)RivalSongGridLogic.SongGridIndex.DIFF_INDEX;
            }
            // クリアソート→内部的にクリアインデックスソート
            else if ((int)RivalSongGridLogic.SongGridIndex.CLEAR == selectedColumnIndex)
            {
                colIndex = (int)RivalSongGridLogic.SongGridIndex.CLEAR_INDEX;
            }
            // トライアルソート→内部的にトライアルインデックスソート
            else if ((int)RivalSongGridLogic.SongGridIndex.TRIAL == selectedColumnIndex)
            {
                colIndex = (int)RivalSongGridLogic.SongGridIndex.TRIAL_INDEX;
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
            view.Columns[(int)RivalSongGridLogic.SongGridIndex.KOUKAI_ORDER].Visible = false;
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
