using DivaNetAccess.src.Logic;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DivaNetAccess.src
{
    public static class RivalCompareGridLogic
    {
        private const string SET_NAME = "songDataSetRivalCompare";
        private const string TABLE_NAME = "songDataTableRivalCompare";

        // DataGridView用列挙型
        public enum Index
        {
            KOUKAI_ORDER = 0,
            NAME_ORDER,
            DIFF_INDEX,
            NO,
            DIFF,
            STAR,
            NAME,
            TASSEIRITU,
            TASSEIRITU_RIVAL,
            TASSEIRITU_SA,
            SCORE,
            SCORE_RIVAL,
            SCORE_SA,
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

            // No
            dt.Columns.Add("no", typeof(int));

            // 難易度
            dt.Columns.Add("diff", Type.GetType("System.String"));

            // ★
            dt.Columns.Add("star", typeof(float));

            // 曲名
            dt.Columns.Add("name", Type.GetType("System.String"));

            // 達成率
            dt.Columns.Add("tasseiritu", typeof(float));

            // 達成率_ライバル
            dt.Columns.Add("tasseiritu_rival", typeof(float));

            // 達成率_差
            dt.Columns.Add("tasseiritu_sa", typeof(float));

            // スコア
            dt.Columns.Add("score", typeof(int));

            // スコア_ライバル
            dt.Columns.Add("score_rival", typeof(int));

            // スコア_差
            dt.Columns.Add("score_sa", typeof(int));

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

            view.Columns["no"].Width = 30;
            view.Columns["no"].HeaderText = "No";

            view.Columns["diff"].Width = 80;
            view.Columns["diff"].HeaderText = "難易度";

            view.Columns["star"].HeaderText = "★";
            view.Columns["star"].Width = 30;

            view.Columns["name"].Width = 250;
            view.Columns["name"].HeaderText = "曲名";
            view.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns["tasseiritu"].Width = 60;
            view.Columns["tasseiritu"].HeaderText = "達成率";
            view.Columns["tasseiritu"].DefaultCellStyle.Format = "##0.00";

            view.Columns["tasseiritu_rival"].Width = 60;
            view.Columns["tasseiritu_rival"].HeaderText = "達成率\n(ライバル)";
            view.Columns["tasseiritu_rival"].DefaultCellStyle.Format = "##0.00";

            view.Columns["tasseiritu_sa"].Width = 60;
            view.Columns["tasseiritu_sa"].HeaderText = "達成率\n(差)";
            view.Columns["tasseiritu_sa"].DefaultCellStyle.Format = "##0.00";

            view.Columns["score"].Width = 60;
            view.Columns["score"].HeaderText = "スコア";

            view.Columns["score_rival"].Width = 60;
            view.Columns["score_rival"].HeaderText = "スコア\n(ライバル)";

            view.Columns["score_sa"].Width = 60;
            view.Columns["score_sa"].HeaderText = "スコア\n(差)";

            view.Columns["_viewFlg"].Visible = false;
        }

        /*
         * 行にデータ追加
         */
        public static void addGrid(DataGridView view, Dictionary<string, SongData> playerSongs, Dictionary<string, SongData> rivalSongs, UrlData urls)
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
            foreach (string songName in playerSongs.Keys)
            {
                SongData playerSong = playerSongs[songName];

                // 削除曲対応
                if (!rivalSongs.ContainsKey(songName))
                {
                    continue;
                }
                SongData rivalSong = rivalSongs[songName];

                // 行のデータ追加
                foreach (string diff in playerSong.data.Keys)
                {
                    DataRow dr = dt.NewRow();


                    if (playerSong.data.ContainsKey(diff)       // ←ありえないパターン
                        && rivalSong.data.ContainsKey(diff)     // ←たぶんある
                                                                //&& urls.songUrl.ContainsKey(songName)   // 
                    )
                    {
                        ResultData playerResult = playerSong.data[diff];
                        ResultData rivalResult = rivalSong.data[diff];
                        //songUrlData songUrl = urls.songUrl[songName];

                        //dr["_koukaiOrder"] = songUrl._koukaiOrde;
                        dr["_koukaiOrder"] = urls.songUrl.ContainsKey(songName) ? urls.songUrl[songName]._koukaiOrde : 9999;
                        //dr["_nameOrder"] = songUrl._nameOrder;
                        dr["_nameOrder"] = urls.songUrl.ContainsKey(songName) ? urls.songUrl[songName]._nameOrder : 9999;
                        dr["_diffIndex"] = WebUtil.getDiffIndex(playerResult.diff);
                        dr["no"] = dt.Rows.Count + 1;
                        dr["name"] = songName;
                        dr["star"] = playerResult.star;
                        dr["diff"] = diff;
                        dr["tasseiritu"] = playerResult.tasseirituV();
                        dr["tasseiritu_rival"] = rivalResult.tasseirituV();
                        dr["tasseiritu_sa"] = playerResult.tasseirituV() - rivalResult.tasseirituV();
                        dr["score"] = playerResult.score;
                        dr["score_rival"] = rivalResult.score;
                        dr["score_sa"] = playerResult.score - rivalResult.score;

                        dr["_viewFlg"] = rivalSong.viewFlg;

                        // 未取得の楽曲
                        if (!rivalSongs.ContainsKey(songName))
                        {
                            dr["_viewFlg"] = false;
                        }

                        dt.Rows.Add(dr);
                    }
                }
            }


            /*
             * 不具合：上の"dt.Rows.Add(dr);"を行うと_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            view.Columns["_koukaiOrder"].Visible = false;
        }

        /*
         * ソート処理
         */
        public static void execSort(DataGridView view, object sender, DataGridViewCellMouseEventArgs e)
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
            if ((int)Index.NAME == e.ColumnIndex)
            {
                colIndex = (int)Index.NAME_ORDER;
            }
            // 難易度ソート→内部的に難易度インデックスソート
            else if ((int)Index.DIFF == e.ColumnIndex)
            {
                colIndex = (int)Index.DIFF_INDEX;
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
            DataRow[] nullRows = dt.Select(
                view.Columns[e.ColumnIndex].Name + " Is Null",
                "_nameOrder ASC"       // 第２ソートは曲名の昇順
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
