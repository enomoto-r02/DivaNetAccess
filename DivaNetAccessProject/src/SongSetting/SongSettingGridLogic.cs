using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;


namespace DivaNetAccess
{
    public class SongSettingGridLogic
    {
        private const string SET_NAME = "playRecordDataSet";
        private const string TABLE_NAME = "playRecordDataTable";

        public enum songSettingGridIndex
        {
            NAME = 1,
            MODULE1,
            MODULE2,
            BUTTON,
            SKIN
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
                view.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
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
            // NO
            dt.Columns.Add("no", Type.GetType("System.String"));

            // 曲名
            dt.Columns.Add("name", Type.GetType("System.String"));

            // モジュール１
            dt.Columns.Add("module1", Type.GetType("System.String"));

            // モジュール２
            dt.Columns.Add("module2", Type.GetType("System.String"));

            // ボタン音
            dt.Columns.Add("button", Type.GetType("System.String"));

            // スキン
            dt.Columns.Add("skin", Type.GetType("System.String"));
        }

        /*
         * 行の情報設定
         */
        public static void initColumnWidth(DataGridView view)
        {
            view.Columns["no"].HeaderText = "No";
            view.Columns["no"].Width = 30;

            view.Columns["name"].HeaderText = "曲名";
            view.Columns["name"].Width = 240;
            view.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns["module1"].HeaderText = "モジュール1";
            view.Columns["module1"].Width = 120;

            view.Columns["module2"].HeaderText = "モジュール2";
            view.Columns["module2"].Width = 120;

            view.Columns["button"].HeaderText = "ボタン音";
            view.Columns["button"].Width = 120;

            view.Columns["skin"].HeaderText = "スキン";
            view.Columns["skin"].Width = 150;
        }

        /*
         * 行にデータ追加
         */
        public static void addGrid(DataGridView view, Dictionary<string, SongSettingData> datas, UrlData urls)
        {
            // コンボボックス列追加
            //test(view, datas, urls);

            DataTable dt = (DataTable)view.DataSource;

            // 行のクリア
            dt.Rows.Clear();

            foreach (string name in datas.Keys)
            {
                DataRow dr = dt.NewRow();
                SongSettingData data = datas[name];

                // Sort後の列番号用
                if (dt.Rows.Count > 0)      // 楽曲共通設定以外
                {
                    dr["no"] = dt.Rows.Count;
                }
                dr["name"] = data.name;
                dr["module1"] = data.module1;
                dr["module2"] = data.module2;
                dr["button"] = data.button;
                dr["skin"] = data.skin;

                dt.Rows.Add(dr);
            }
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
    }
}
