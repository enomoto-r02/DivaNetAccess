using DivaNetAccess.src.CollectionCard;
using DivaNetAccess.src.Logic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DivaNetAccess
{
    // コレクションカードグリッドのソート処理クラス
    public static class CollectionCardGridLogic
    {
        private const string SET_NAME = "collectionCardDataSet";
        private const string TABLE_NAME = "collectionCardDataTable";

        private const string VIEW_DATE = "yyyy/MM/dd HH:mm";

        // CollectionCardGridView用列挙型
        public enum CollectionCardGridIndex
        {
            NO = 0,
            NAME,
            DATE,
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
            // NO
            dt.Columns.Add("no", typeof(int));

            // 未所持フラグ
            dt.Columns.Add("_noneFlg", typeof(int));
            // 種類
            dt.Columns.Add("_type", typeof(int));

            // 種類
            dt.Columns.Add("typeName", Type.GetType("System.String"));

            // 種類名
            dt.Columns.Add("typeNo", typeof(int));

            // カード名
            dt.Columns.Add("name", Type.GetType("System.String"));

            // 枚数
            dt.Columns.Add("num", typeof(int));

            // カード名
            dt.Columns.Add("date", typeof(DateTime));

            // NEW
            dt.Columns.Add("new", Type.GetType("System.String"));
        }

        /*
         * 行の情報設定
         */
        public static void initColumnWidth(DataGridView view)
        {
            view.Columns["no"].HeaderText = "No";
            view.Columns["no"].Width = 30;

            // ここら辺はプロパティファイルでも作る？
            view.Columns["_noneFlg"].HeaderText = "noneFlg";
            view.Columns["_noneFlg"].Visible = false;
            view.Columns["_type"].HeaderText = "type";
            view.Columns["_type"].Visible = false;

            view.Columns["typeName"].HeaderText = "種類";
            view.Columns["typeName"].Width = 60;

            view.Columns["typeNo"].HeaderText = "種類No";
            view.Columns["typeNo"].Width = 60;

            view.Columns["name"].HeaderText = "カード名";
            view.Columns["name"].Width = 250;
            view.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            view.Columns["num"].HeaderText = "獲得枚数";
            view.Columns["num"].Width = 60;

            view.Columns["date"].HeaderText = "初回獲得日時";
            view.Columns["date"].Width = 100;
            view.Columns["date"].DefaultCellStyle.Format = VIEW_DATE;

            view.Columns["new"].HeaderText = "NEW";
            view.Columns["new"].Width = 50;
        }

        /*
         * 行にデータ追加
         */
        public static void addGrid(DataGridView view, CollectionCardEntity collectionCardEntity)
        {
            DataTable dt = (DataTable)view.DataSource;

            // 行のクリア
            dt.Rows.Clear();

            int nowRow = 0;
            foreach (CollectionCard data in collectionCardEntity.collectionCards)
            {
                DataRow dr = dt.NewRow();

                dr["no"] = data.no;
                if (data.name == "？？？？")
                {
                    dr["_noneFlg"] = 1;
                }
                else
                {

                    dr["_noneFlg"] = 0;
                }
                dr["_type"] = data.type;
                dr["typeName"] = data.typeName;
                dr["typeNo"] = data.typeNo;
                dr["name"] = data.name;
                dr["num"] = data.num;
                if (data.getDate != DateTime.MinValue)
                {
                    dr["date"] = data.getDate;
                }
                if (data.newFlg == true)
                {
                    dr["new"] = "○";
                }

                dt.Rows.Add(dr);

                nowRow++;
            }

            execNoSort(view);
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
            string sortStr = view.Columns["no"].Name + " ASC";
            dv.Sort = sortStr;

            //先頭の行を選択する
            if (view.Rows.Count > 0)
            {
                // 行ではなくセルを選択しないとちゃんと選択されない？
                view.Rows[0].Cells["no"].Selected = true;
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

            // 横スクロールの位置を退避
            int n = view.HorizontalScrollingOffset;
            int n2 = view.FirstDisplayedScrollingRowIndex;

            //並び替えの方向（昇順か降順か）を決める
            ListSortDirection sortDirection =
                    view.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending ||
                    view.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ?
                    ListSortDirection.Ascending : ListSortDirection.Descending;

            SortOrder sortOrder =
                sortDirection == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;

            string fillter = "";
            string sort = "";
            string sortColumn = view.Columns[colIndex].Name;

            // 種類系のソートはNoソートとして扱う
            if (sortColumn == "typeNo" || sortColumn == "typeName")
            {
                sortColumn = "no";
            }
            sort = sortColumn + " " + getSortOrderStr(sortOrder);

            // Noソート以外は"？？？？"を末尾にする
            if (sortColumn != "no")
            {
                sort = "_noneFlg ASC, " + sort;
            }

            // 未取得カードは下に表示
            DataRow[] notNullRows = dt.Select(fillter, sort);

            // テーブルのクリア
            dt = dt.Clone();

            // ソートされた行を追加
            foreach (DataRow notNullRow in notNullRows)
            {
                dt.ImportRow(notNullRow);
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

        /*
         * 検索処理
         */
        public static void searchGrid(DataGridView view, string searchStr)
        {
            //バインドされているDataTableを取得
            DataTable dt = (DataTable)view.DataSource;

            //DataViewを取得
            DataView dv = dt.DefaultView;

            //並び替えを行う
            string sortStr = searchStr;
            dv.RowFilter = sortStr;
        }
    }
}
