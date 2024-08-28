using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess.src.Logic
{
    public class SettingCommon
    {
        public DataTable gridSettingTable;

        readonly string TEXT_NAME;
        readonly string TEXT_PATH;

        public SettingCommon()
        {
            gridSettingTable = new DataTable();

            // まともな定数化を誰か。。
            TEXT_NAME = "setting.txt";
            TEXT_PATH = SettingConst.CONF_DIR_NAME + "/" + TEXT_NAME;

            init();

            read();
        }

        /*
         * 初期設定
         */
        public void init()
        {
            // キー
            gridSettingTable.Columns.Add("name", Type.GetType("System.String"));

            // カラム名
            gridSettingTable.Columns.Add("column", Type.GetType("System.String"));

            // プロパティ名＠Width、Visibleなど
            gridSettingTable.Columns.Add("property", Type.GetType("System.String"));

            // 設定値
            gridSettingTable.Columns.Add("value", typeof(string));
        }

        /*
         * 読み込み処理
         */
        public void read()
        {
            // ファイル存在チェック
            if (File.Exists(TEXT_PATH) == false)
            {
                return;
            }

            string[] lines = FileUtil.readFile(TEXT_PATH).Split('\n');

            foreach (string line in lines)
            {
                // 空行スキップ＠最後の改行など
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] gridInfo = line.Split('=')[0].Split('.');
                string valueStr = line.Split('=')[1];

                // 行情報セット
                DataRow row = gridSettingTable.NewRow();

                row["name"] = gridInfo[0];
                row["column"] = gridInfo[1];
                row["property"] = gridInfo[2];
                row["value"] = valueStr;

                gridSettingTable.Rows.Add(row);
            }
        }

        /*
         * 書き込み処理＠共通でも良い？
         */
        public void write(Dictionary<string, DataGridView> dgvs)
        {
            StringBuilder sb = new StringBuilder();
            string format = "{0}.{1}.{2}={3}";

            foreach (string key in dgvs.Keys)
            {
                DataGridView dgv = dgvs[key];

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    // "_"から始まる列は対象外とする
                    if (column.Name.StartsWith("_"))
                    {
                        continue;
                    }

                    StringBuilder colData = new StringBuilder();

                    // Visible
                    colData.Append(string.Format(format, key, column.Name, "Visible", column.Visible));
                    colData.Append("\n");

                    // Width
                    colData.Append(string.Format(format, key, column.Name, "Width", column.Width));
                    colData.Append("\n");

                    // DisplayIndex
                    colData.Append(string.Format(format, key, column.Name, "DisplayIndex", column.DisplayIndex));
                    colData.Append("\n");

                    sb.Append(colData.ToString());
                }
            }

            // ファイル出力
            FileUtil.createFolder(SettingConst.CONF_DIR_NAME);
            FileUtil.writeFile(sb.ToString(), TEXT_PATH, false);
        }

        /*
         * 情報設定＠refは必要？
         */
        public void set(Dictionary<string, DataGridView> dgvs)
        {
            foreach (string key in dgvs.Keys)
            {
                DataGridView dgv = dgvs[key];

                #region DisplayIndexの設定＠最初にDisplayIndexの設定(昇順で処理)

                // 対象の設定値を取得＠DisplayIndex
                DataRow[] rows = gridSettingTable.Select(string.Format("name='{0}' AND property='{1}'", key, "DisplayIndex"));

                Dictionary<int, string> dispDictionary = new Dictionary<int, string>();
                foreach (DataRow row in rows)
                {
                    // 設定値を昇順で保存
                    dispDictionary.Add(int.Parse(row["value"].ToString()), row["column"].ToString());
                }

                foreach (int displayIndex in dispDictionary.Keys)
                {
                    string columnName = dispDictionary[displayIndex];
                    dgv.Columns[columnName].DisplayIndex = displayIndex;
                }

                #endregion

                #region DisplayIndex以外の設定

                // 対象の設定値を取得＠DisplayIndex以外
                rows = gridSettingTable.Select(string.Format("name='{0}' AND property<>'{1}'", key, "DisplayIndex"));

                foreach (DataRow row in rows)
                {
                    string column = row["column"].ToString();
                    string property = row["property"].ToString();
                    string value = row["value"].ToString();

                    // 設定
                    switch (property)
                    {
                        case "Visible":
                            dgv.Columns[column].Visible = bool.Parse(value);
                            break;
                        case "Width":
                            dgv.Columns[column].Width = int.Parse(value);
                            break;
                    }
                }

                #endregion

                //foreach (DataRow row in rows)
                //{
                //    string column = row["column"].ToString();
                //    int value = int.Parse(row["value"].ToString());
                //    dgv.Columns[column].DisplayIndex = value;
                //}

                //// 対象の設定値を取得
                //DataRow[] rows = gridSettingTable.Select(string.Format("name = '{0}'", key));

                //foreach(DataRow row in rows)
                //{
                //    string column = (string)row["column"];
                //    string property = (string)row["property"];
                //    string value = (string)row["value"];

                //    // 設定
                //    switch(property)
                //    {
                //        case "Visible":
                //            dgv.Columns[column].Visible = bool.Parse(value);
                //            break;
                //        case "Width":
                //            dgv.Columns[column].Width = int.Parse(value);
                //            break;
                //        case "DisplayIndex":
                //            dgv.Columns[column].DisplayIndex = int.Parse(value);
                //            break;
                //    }
                //}
            }
        }
    }
}
