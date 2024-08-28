using DivaNetAccess.src.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DivaNetAccess
{
    // プレイ履歴データグリッドのソート処理クラス
    public static class PlayRecordGridLogic
    {
        private const string SET_NAME = "playRecordDataSet";
        private const string TABLE_NAME = "playRecordDataTable";

        private const string VIEW_DATE = "yyyy/MM/dd HH:mm";

        // playRecordGridView用列挙型
        public enum PlayRecordGridIndex
        {
            KOUKAI_ORDER = 0,
            NAME_ORDER,
            DIFF_INDEX,
            CLEAR_INDEX,
            TRIAL_INDEX,
            TASSEIRITU_NEW_RECORD,
            SCORE_NEW_RECORD,
            DEL_FLG,
            NO,
            DATE,
            PLACE,
            NAME,
            DIFF,
            STAR,
            CLEAR,
            TASSEIRITU,
            SCORE,
            COOL,
            COOLP,
            FINE,
            FINEP,
            SAFE,
            SAFEP,
            SAD,
            SADP,
            WORST,
            WORSTP,
            COMBO,
            CHALLENGE,
            HOLD,
            SLIDE,
            TRIAL,
            OPTION,
            PVJUNC,
            MODULE1,
            MODULE2,
            MODULE3,
            BUTTON,
            SLIDE_SE,
            CHAIN,
            SKIN,
            MEMO
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

            // 行ヘッダー追加
            initColumnAdd(dt);

            view.DataSource = dt;

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

            // 達成率_更新
            dt.Columns.Add("_tasseirituNewRecord", typeof(bool));

            // スコア_更新
            dt.Columns.Add("_scoreNewRecord", typeof(bool));

            // 削除
            dt.Columns.Add("del", typeof(bool));

            // NO
            dt.Columns.Add("no", typeof(int));

            // 日付
            dt.Columns.Add("date", typeof(DateTime));

            // 場所
            dt.Columns.Add("place", Type.GetType("System.String"));

            // 曲名
            dt.Columns.Add("name", Type.GetType("System.String"));

            // 難易度
            dt.Columns.Add("diff", Type.GetType("System.String"));

            // ★
            dt.Columns.Add("star", typeof(float));

            // CLEAR
            dt.Columns.Add("clear", Type.GetType("System.String"));

            // 達成率
            dt.Columns.Add("tasseiritu", typeof(float));

            // スコア
            dt.Columns.Add("score", typeof(int));

            // COOL
            dt.Columns.Add("cool", typeof(int));

            // COOL率
            dt.Columns.Add("coolP", typeof(float));

            // FINE
            dt.Columns.Add("fine", typeof(int));

            // FINE率
            dt.Columns.Add("fineP", typeof(float));

            // SAFE
            dt.Columns.Add("safe", typeof(int));

            // SAFE率
            dt.Columns.Add("safeP", typeof(float));

            // SAD
            dt.Columns.Add("sad", typeof(int));

            // SAD率
            dt.Columns.Add("sadP", typeof(float));

            // WORST/WRONG
            dt.Columns.Add("worst", typeof(int));

            // WORST/WRONG率
            dt.Columns.Add("worstP", typeof(float));

            // COMBO
            dt.Columns.Add("combo", typeof(int));

            // チャレンジタイム
            dt.Columns.Add("challenge", typeof(int));

            // HOLD
            dt.Columns.Add("hold", typeof(int));

            // スライド
            dt.Columns.Add("slide", typeof(int));

            // トライアル
            dt.Columns.Add("trial", Type.GetType("System.String"));

            // オプション
            dt.Columns.Add("option", Type.GetType("System.String"));

            // PV分岐
            dt.Columns.Add("pvjunc", Type.GetType("System.String"));

            // モジュール１
            dt.Columns.Add("module1", Type.GetType("System.String"));

            // モジュール２
            dt.Columns.Add("module2", Type.GetType("System.String"));

            // モジュール３
            dt.Columns.Add("module3", Type.GetType("System.String"));

            // ボタン音
            dt.Columns.Add("button", Type.GetType("System.String"));

            // スライド音
            dt.Columns.Add("slideSE", Type.GetType("System.String"));

            // チェーンスライド音
            dt.Columns.Add("chain", Type.GetType("System.String"));

            // スキン
            dt.Columns.Add("skin", Type.GetType("System.String"));

            // メモ
            dt.Columns.Add("memo", Type.GetType("System.String"));
        }

        /*
         * 行の情報設定
         */
        public static void initColumnWidth(DataGridView view)
        {
            // ここら辺はプロパティファイルでも作る？
            view.Columns[(int)PlayRecordGridIndex.KOUKAI_ORDER].Visible = false;
            view.Columns[(int)PlayRecordGridIndex.NAME_ORDER].Visible = false;
            view.Columns[(int)PlayRecordGridIndex.DIFF_INDEX].Visible = false;
            view.Columns[(int)PlayRecordGridIndex.CLEAR_INDEX].Visible = false;
            view.Columns[(int)PlayRecordGridIndex.TRIAL_INDEX].Visible = false;
            view.Columns[(int)PlayRecordGridIndex.TASSEIRITU_NEW_RECORD].Visible = false;
            view.Columns[(int)PlayRecordGridIndex.SCORE_NEW_RECORD].Visible = false;

            view.Columns[(int)PlayRecordGridIndex.DEL_FLG].HeaderText = "削除";
            view.Columns[(int)PlayRecordGridIndex.DEL_FLG].Width = 40;
            view.Columns[(int)PlayRecordGridIndex.DEL_FLG].ReadOnly = false;

            view.Columns[(int)PlayRecordGridIndex.NO].HeaderText = "No";
            view.Columns[(int)PlayRecordGridIndex.NO].Width = 30;

            view.Columns[(int)PlayRecordGridIndex.DATE].HeaderText = "日時";
            view.Columns[(int)PlayRecordGridIndex.DATE].Width = 100;
            view.Columns[(int)PlayRecordGridIndex.DATE].DefaultCellStyle.Format = VIEW_DATE;

            view.Columns[(int)PlayRecordGridIndex.PLACE].HeaderText = "場所";
            view.Columns[(int)PlayRecordGridIndex.PLACE].Width = 180;
            view.Columns[(int)PlayRecordGridIndex.PLACE].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns[(int)PlayRecordGridIndex.NAME].HeaderText = "曲名";
            view.Columns[(int)PlayRecordGridIndex.NAME].Width = 250;
            view.Columns[(int)PlayRecordGridIndex.NAME].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns[(int)PlayRecordGridIndex.DIFF].HeaderText = "難易度";
            view.Columns[(int)PlayRecordGridIndex.DIFF].Width = 80;

            view.Columns[(int)PlayRecordGridIndex.STAR].HeaderText = "★";
            view.Columns[(int)PlayRecordGridIndex.STAR].Width = 30;

            view.Columns[(int)PlayRecordGridIndex.CLEAR].HeaderText = "CLEAR";
            view.Columns[(int)PlayRecordGridIndex.CLEAR].Width = 80;

            view.Columns[(int)PlayRecordGridIndex.TASSEIRITU].HeaderText = "達成率";
            view.Columns[(int)PlayRecordGridIndex.TASSEIRITU].Width = 60;
            view.Columns[(int)PlayRecordGridIndex.TASSEIRITU].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)PlayRecordGridIndex.SCORE].HeaderText = "スコア";
            view.Columns[(int)PlayRecordGridIndex.SCORE].Width = 60;

            view.Columns[(int)PlayRecordGridIndex.COOL].HeaderText = "COOL";
            view.Columns[(int)PlayRecordGridIndex.COOL].Width = 50;

            view.Columns[(int)PlayRecordGridIndex.COOLP].HeaderText = "COOL\n率";
            view.Columns[(int)PlayRecordGridIndex.COOLP].Width = 50;
            view.Columns[(int)PlayRecordGridIndex.COOLP].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)PlayRecordGridIndex.FINE].HeaderText = "FINE";
            view.Columns[(int)PlayRecordGridIndex.FINE].Width = 50;

            view.Columns[(int)PlayRecordGridIndex.FINEP].HeaderText = "FINE\n率";
            view.Columns[(int)PlayRecordGridIndex.FINEP].Width = 50;
            view.Columns[(int)PlayRecordGridIndex.FINEP].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)PlayRecordGridIndex.SAFE].HeaderText = "SAFE";
            view.Columns[(int)PlayRecordGridIndex.SAFE].Width = 50;

            view.Columns[(int)PlayRecordGridIndex.SAFEP].HeaderText = "SAFE\n率";
            view.Columns[(int)PlayRecordGridIndex.SAFEP].Width = 50;
            view.Columns[(int)PlayRecordGridIndex.SAFEP].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)PlayRecordGridIndex.SAD].HeaderText = "SAD";
            view.Columns[(int)PlayRecordGridIndex.SAD].Width = 50;

            view.Columns[(int)PlayRecordGridIndex.SADP].HeaderText = "SAD\n率";
            view.Columns[(int)PlayRecordGridIndex.SADP].Width = 50;
            view.Columns[(int)PlayRecordGridIndex.SADP].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)PlayRecordGridIndex.WORST].HeaderText = "WORST\nWRONG";
            view.Columns[(int)PlayRecordGridIndex.WORST].Width = 50;

            view.Columns[(int)PlayRecordGridIndex.WORSTP].HeaderText = "WORST\nWRONG率";
            view.Columns[(int)PlayRecordGridIndex.WORSTP].Width = 70;
            view.Columns[(int)PlayRecordGridIndex.WORSTP].DefaultCellStyle.Format = "##0.00";

            view.Columns[(int)PlayRecordGridIndex.COMBO].HeaderText = "COMBO";
            view.Columns[(int)PlayRecordGridIndex.COMBO].Width = 50;

            view.Columns[(int)PlayRecordGridIndex.CHALLENGE].HeaderText = "CHALLENGE\nTIME";
            view.Columns[(int)PlayRecordGridIndex.CHALLENGE].Width = 80;

            view.Columns[(int)PlayRecordGridIndex.HOLD].HeaderText = "同時押し\nホールド";
            view.Columns[(int)PlayRecordGridIndex.HOLD].Width = 60;

            view.Columns[(int)PlayRecordGridIndex.SLIDE].HeaderText = "スライド";
            view.Columns[(int)PlayRecordGridIndex.SLIDE].Width = 70;

            view.Columns[(int)PlayRecordGridIndex.TRIAL].HeaderText = "TRIAL";
            view.Columns[(int)PlayRecordGridIndex.TRIAL].Width = 170;

            view.Columns[(int)PlayRecordGridIndex.OPTION].HeaderText = "オプション";
            view.Columns[(int)PlayRecordGridIndex.OPTION].Width = 80;

            view.Columns[(int)PlayRecordGridIndex.PVJUNC].HeaderText = "PV分岐";
            view.Columns[(int)PlayRecordGridIndex.PVJUNC].Width = 60;

            view.Columns[(int)PlayRecordGridIndex.MODULE1].HeaderText = "モジュール1";
            view.Columns[(int)PlayRecordGridIndex.MODULE1].Width = 200;

            view.Columns[(int)PlayRecordGridIndex.MODULE2].HeaderText = "モジュール2";
            view.Columns[(int)PlayRecordGridIndex.MODULE2].Width = 200;

            view.Columns[(int)PlayRecordGridIndex.MODULE3].HeaderText = "モジュール3";
            view.Columns[(int)PlayRecordGridIndex.MODULE3].Width = 200;

            view.Columns[(int)PlayRecordGridIndex.BUTTON].HeaderText = "ボタン音";
            view.Columns[(int)PlayRecordGridIndex.BUTTON].Width = 70;
            view.Columns[(int)PlayRecordGridIndex.BUTTON].Visible = false;

            view.Columns[(int)PlayRecordGridIndex.SLIDE_SE].HeaderText = "スライド音";
            view.Columns[(int)PlayRecordGridIndex.SLIDE_SE].Width = 70;
            view.Columns[(int)PlayRecordGridIndex.SLIDE_SE].Visible = false;

            view.Columns[(int)PlayRecordGridIndex.CHAIN].HeaderText = "チェーン\nスライド音";
            view.Columns[(int)PlayRecordGridIndex.CHAIN].Width = 70;
            view.Columns[(int)PlayRecordGridIndex.CHAIN].Visible = false;

            view.Columns[(int)PlayRecordGridIndex.SKIN].HeaderText = "スキン";
            view.Columns[(int)PlayRecordGridIndex.SKIN].Width = 120;
            view.Columns[(int)PlayRecordGridIndex.SKIN].Visible = false;

            view.Columns[(int)PlayRecordGridIndex.MEMO].HeaderText = "メモ";
            view.Columns[(int)PlayRecordGridIndex.MEMO].Width = 350;
            view.Columns[(int)PlayRecordGridIndex.MEMO].ReadOnly = false;
            view.Columns[(int)PlayRecordGridIndex.MEMO].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
        }

        /*
         * 行にデータ追加
         */
        public static void addGrid(DataGridView view, Dictionary<string, PlayRecordEntity> datas)
        {
            view.SuspendLayout();

            DataTable dt = (DataTable)view.DataSource;

            // 行のクリア
            dt.Rows.Clear();

            int nowRow = 0;

            foreach (string key in datas.Keys)
            {
                PlayRecordEntity data = datas[key];

                DataRow dr = dt.NewRow();

                dr[(int)PlayRecordGridIndex.KOUKAI_ORDER] = data._koukaiOrde;
                dr[(int)PlayRecordGridIndex.NAME_ORDER] = data._nameOrder;
                dr[(int)PlayRecordGridIndex.DIFF_INDEX] = data._diffIndex;
                dr[(int)PlayRecordGridIndex.CLEAR_INDEX] = data._clearIndex;
                dr[(int)PlayRecordGridIndex.TRIAL_INDEX] = data._trialIndex;
                dr[(int)PlayRecordGridIndex.TASSEIRITU_NEW_RECORD] = data.tasseirituNewRecord;
                dr[(int)PlayRecordGridIndex.SCORE_NEW_RECORD] = data.scoreNewRecord;
                dr[(int)PlayRecordGridIndex.NO] = nowRow + 1;
                dr[(int)PlayRecordGridIndex.DATE] = data.date.ToString(VIEW_DATE);
                dr[(int)PlayRecordGridIndex.PLACE] = data.place;
                dr[(int)PlayRecordGridIndex.NAME] = data.name;
                dr[(int)PlayRecordGridIndex.DIFF] = data.diff;
                dr[(int)PlayRecordGridIndex.STAR] = data.star;
                dr[(int)PlayRecordGridIndex.CLEAR] = data.clear;
                dr[(int)PlayRecordGridIndex.TASSEIRITU] = (float.Parse(data.tasseiritu.ToString()) / 100.0f);
                dr[(int)PlayRecordGridIndex.SCORE] = data.score;
                dr[(int)PlayRecordGridIndex.COOL] = data.cool;
                dr[(int)PlayRecordGridIndex.COOLP] = (float.Parse(data.coolP.ToString()) / 100.0f);
                dr[(int)PlayRecordGridIndex.FINE] = data.fine;
                dr[(int)PlayRecordGridIndex.FINEP] = (float.Parse(data.fineP.ToString()) / 100.0f);
                dr[(int)PlayRecordGridIndex.SAFE] = data.safe;
                dr[(int)PlayRecordGridIndex.SAFEP] = (float.Parse(data.safeP.ToString()) / 100.0f);
                dr[(int)PlayRecordGridIndex.SAD] = data.sad;
                dr[(int)PlayRecordGridIndex.SADP] = (float.Parse(data.sadP.ToString()) / 100.0f);
                dr[(int)PlayRecordGridIndex.WORST] = data.worst;
                dr[(int)PlayRecordGridIndex.WORSTP] = (float.Parse(data.worstP.ToString()) / 100.0f);
                dr[(int)PlayRecordGridIndex.COMBO] = data.combo;
                dr[(int)PlayRecordGridIndex.CHALLENGE] = data.challenge;
                dr[(int)PlayRecordGridIndex.HOLD] = data.hold;
                dr[(int)PlayRecordGridIndex.SLIDE] = data.slide;
                dr[(int)PlayRecordGridIndex.TRIAL] = data.trial;
                dr[(int)PlayRecordGridIndex.OPTION] = data.option;
                dr[(int)PlayRecordGridIndex.PVJUNC] = data.pvjunc;
                dr[(int)PlayRecordGridIndex.MODULE1] = data.module1;
                dr[(int)PlayRecordGridIndex.MODULE2] = data.module2;
                dr[(int)PlayRecordGridIndex.MODULE3] = data.module3;
                dr[(int)PlayRecordGridIndex.BUTTON] = data.button;
                dr[(int)PlayRecordGridIndex.SLIDE_SE] = data.slideSE;
                dr[(int)PlayRecordGridIndex.CHAIN] = data.chain;
                dr[(int)PlayRecordGridIndex.BUTTON] = data.button;
                dr[(int)PlayRecordGridIndex.SKIN] = data.skin;
                dr[(int)PlayRecordGridIndex.MEMO] = data.memo;

                dt.Rows.Add(dr);

                nowRow++;
            }

            view.DataSource = dt;

            // できてはないけどかなり早い。実装する？？
            // 達成率等をモデル側で持つ必要がある(そんなに時間かからんと思うけど)
            // また検索やソートにも影響(列の数だけソート実装する必要ある？ 実装例が欲しい。ひとつできれば後はコピペになると思うが)
            // 参考：http://nam-it.blog.jp/archives/7439293.html
            /*
            List<playRecordEntity> hoge = new List<playRecordEntity>();

            foreach (string key in datas.Keys)
            {
                hoge.Add(datas[key]);
            }
            view.DataSource = hoge;
            */

            execNoSort(view);

            view.ResumeLayout();
        }

        /*
         * 行クリア
         */
        public static void clearGrid(DataGridView view)
        {
            DataTable dt = (DataTable)view.DataSource;

            // 行のクリア
            dt.Rows.Clear();
        }

        /*
         * Noソート処理
         */
        public static void execNoSort(DataGridView view)
        {
            //バインドされているDataTableを取得
            DataTable dt = (DataTable)view.DataSource;

            //DataViewを取得
            DataView dv = dt.DefaultView;

            //並び替えを行う
            string sortStr = view.Columns[(int)PlayRecordGridIndex.NO].Name + " ASC";
            dv.Sort = sortStr;

            //先頭の行を選択する
            if (view.Rows.Count > 0)
            {
                // 行ではなくセルを選択しないとちゃんと選択されない？
                view.Rows[0].Cells[(int)PlayRecordGridIndex.NO].Selected = true;
            }
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

            // ソート対象カラム取得
            int colIndex = e.ColumnIndex;

            // 削除列は無効
            if ((int)PlayRecordGridIndex.DEL_FLG == colIndex)
            {
                return;
            }

            // 横スクロールの位置を退避
            int n = view.HorizontalScrollingOffset;
            int n2 = view.FirstDisplayedScrollingRowIndex;

            // 曲名ソート→内部的に曲名順ソート
            if ((int)PlayRecordGridIndex.NAME == e.ColumnIndex)
            {
                colIndex = (int)PlayRecordGridIndex.NAME_ORDER;
            }
            // 難易度ソート→内部的に難易度インデックスソート
            else if ((int)PlayRecordGridIndex.DIFF == e.ColumnIndex)
            {
                colIndex = (int)PlayRecordGridIndex.DIFF_INDEX;
            }
            // クリアソート→内部的にクリアインデックスソート
            else if ((int)PlayRecordGridIndex.CLEAR == e.ColumnIndex)
            {
                colIndex = (int)PlayRecordGridIndex.CLEAR_INDEX;
            }
            // トライアルソート→内部的にトライアルインデックスソート
            else if ((int)PlayRecordGridIndex.TRIAL == e.ColumnIndex)
            {
                colIndex = (int)PlayRecordGridIndex.TRIAL_INDEX;
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
                "No ASC"       // 第２ソートはNOの降順
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

            // 横スクロールの位置を復帰
            view.HorizontalScrollingOffset = n;
            view.FirstDisplayedScrollingRowIndex = n2;

            //今までの並び替えグリフを消す
            foreach (DataGridViewColumn col in view.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
                col.HeaderCell.Style.BackColor = view.ColumnHeadersDefaultCellStyle.BackColor;
            }

            //並び替えグリフを変更
            view.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
            view.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.LightYellow;
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
