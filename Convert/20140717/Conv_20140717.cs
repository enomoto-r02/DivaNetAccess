using Convert.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Convert._20140717
{
    class Conv_20140717
    {
        public static void convMain(string[] args)
        {
            Console.WriteLine("コンバートを開始します。");

            // 退避用フォルダ名
            string bkDirectoryName = DateTime.Now.ToString(SettingConst.DATE_FORMAT_YYYYMMDDHHMMSS);

            // バックアップフォルダ名リスト
            List<string[]> bkDirNames = createBackupTargetFolder(bkDirectoryName);
            if (1 > bkDirNames.Count)
            {
                // 完了メッセージ
                Console.WriteLine("バックアップ対象のフォルダがありません。コンバート処理は不要です。");
                Console.ReadKey();

                return;
            }

            // バックアップ
            if (!backupTargetFolder(bkDirNames))
            {
                Console.WriteLine("バックアップに失敗しました。コンバートを中断します。");
                Console.ReadKey();

                return;
            }

            // コンバート
            convert(bkDirNames);

            // 完了メッセージ
            Console.WriteLine("コンバートが終了しました。");
            Console.ReadKey();

            return;
        }

        /*
         * バックアップ対象フォルダのリスト生成
         */
        private static List<string[]> createBackupTargetFolder(string bkDirectoryName)
        {
            List<string[]> bkDirNames = new List<string[]>();

            // dataフォルダすべて
            if (Directory.Exists("../data"))
            {
                bkDirNames.Add(new string[] { "../data", "./bk_" + bkDirectoryName + "/data" });
            }

            //// confフォルダすべて
            //if (Directory.Exists("../conf"))
            //{
            //    bkDirNames.Add(new string[] { "../conf", "./bk_" + bkDirectoryName + "/conf" });
            //}

            return bkDirNames;
        }

        /*
         * 対象フォルダのバックアップ処理
         */
        private static bool backupTargetFolder(List<string[]> bkDirNames)
        {
            foreach (string[] bkDirName in bkDirNames)
            {
                ConvertCommonUtil.DirectoryCopy(bkDirName[0], bkDirName[1]);

                // バックアップフォルダのサイズチェック
                long originalDirSize_data = ConvertCommonUtil.GetDirectorySize(new DirectoryInfo(bkDirName[0]));
                long backupDirSize_data = ConvertCommonUtil.GetDirectorySize(new DirectoryInfo(bkDirName[1]));

                if ((backupDirSize_data == 0) || (originalDirSize_data != backupDirSize_data))
                {
                    return false;
                }
            }

            return true;
        }

        /*
         * コンバート
         */
        private static void convert(List<string[]> bkDirNames)
        {
            foreach (string[] bkDirName in bkDirNames)
            {

                #region playRecord.txtコンバート

                // playerData.txtをすべて取得
                string[] filePaths = System.IO.Directory.GetFiles(
                    bkDirName[0],
                    SettingConst.FILE_PLAYER_DATA,
                    System.IO.SearchOption.AllDirectories
                );

                foreach (string filePath in filePaths)
                {
                    // プレイヤーのplayerData.txtのみ
                    if (!filePath.Contains("rival"))
                    {
                        // ファイルをリスト化
                        List<List<string>> input = toListDataFile(filePath, '\n', '\t');

                        List<List<string>> output = new List<List<string>>();

                        foreach (List<string> inLine in input)
                        {
                            List<string> outLine = new List<string>();

                            for (int i = 0; i < inLine.Count; i++)
                            {
                                // そのまま格納
                                outLine.Add(inLine[i]);
                            }

                            // 入力行を格納
                            output.Add(outLine);
                        }

                        // ファイルの末尾にメモを追加
                        List<string> blankLine = new List<string>();
                        blankLine.Add("");
                        output.Add(blankLine);

                        // 書き込む
                        FileUtil.writeFile(Conv_20140717.toStringDataFile(output, '\n', '\t'), filePath, false);
                    }
                }

                #endregion

                #region songData.txtコンバート

                // songData.txtをすべて取得
                filePaths = System.IO.Directory.GetFiles(
                    bkDirName[0],
                    SettingConst.FILE_SONG_DATA,
                    System.IO.SearchOption.AllDirectories
                );

                foreach (string filePath in filePaths)
                {
                    // ファイルをリスト化
                    List<List<string>> input = toListDataFile(filePath, '\n', '\t');

                    // 1行をリスト化
                    List<List<string>> output = new List<List<string>>();

                    foreach (List<string> inLine in input)
                    {
                        List<string> outLine = new List<string>();

                        for (int i = 0; i < inLine.Count; i++)
                        {
                            if (i != 0 && i % 8 == 0)
                            {
                                // ★の前にHIS、HID、SUDを加える
                                outLine.Add("");
                                outLine.Add("");
                                outLine.Add("");
                            }

                            // そのまま格納
                            outLine.Add(inLine[i]);
                        }

                        // 入力行を格納
                        output.Add(outLine);
                    }

                    // 書き込む
                    FileUtil.writeFile(Conv_20140717.toStringDataFile(output, '\n', '\t'), filePath, false);
                }

                #endregion

                #region playRecord.txtコンバート

                // playRecord.txtをすべて取得
                filePaths = System.IO.Directory.GetFiles(
                    bkDirName[0],
                    SettingConst.FILE_RECORD_DATA,
                    System.IO.SearchOption.AllDirectories
                );

                foreach (string filePath in filePaths)
                {
                    // ファイルをリスト化
                    List<List<string>> input = toListDataFile(filePath, '\n', '\t');

                    List<List<string>> output = new List<List<string>>();

                    foreach (List<string> inLine in input)
                    {
                        List<string> outLine = new List<string>();

                        for (int i = 0; i < inLine.Count; i++)
                        {
                            if (i == 23)
                            {

                                // スライドの初期値
                                outLine.Add("0");
                            }
                            else if (i == 24)
                            {
                                // オプションの初期値
                                outLine.Add("");

                                // PV分岐の初期値
                                outLine.Add("");
                            }
                            else if (i == 26)
                            {
                                // モジュール３の初期値
                                outLine.Add("");
                            }
                            else if (i == 27)
                            {
                                // スライド音の初期値
                                outLine.Add("");

                                // チェーンスライド音の初期値
                                outLine.Add("");
                            }

                            // そのまま格納
                            outLine.Add(inLine[i]);
                        }

                        // 入力行を格納
                        output.Add(outLine);
                    }

                    // 書き込む
                    FileUtil.writeFile(Conv_20140717.toStringDataFile(output, '\n', '\t'), filePath, false);
                }

                #endregion

                #region memoData.txtコンバート

                // memoData.txtをすべて取得
                filePaths = System.IO.Directory.GetFiles(
                    bkDirName[0],
                    SettingConst.FILE_MEMO_DATA,
                    System.IO.SearchOption.AllDirectories
                );

                foreach (string filePath in filePaths)
                {
                    // ファイルをリスト化
                    List<List<string>> input = toListDataFile(filePath, '\n', '\t');

                    // 1行をリスト化
                    List<List<string>> output = new List<List<string>>();

                    foreach (List<string> inLine in input)
                    {
                        List<string> outLine = new List<string>();

                        for (int i = 0; i < inLine.Count; i++)
                        {
                            // そのまま格納
                            outLine.Add(inLine[i]);
                        }

                        // 行の末尾にFINE用メモを追加
                        outLine.Add("");

                        // 入力行を格納
                        output.Add(outLine);
                    }

                    // 書き込む
                    FileUtil.writeFile(Conv_20140717.toStringDataFile(output, '\n', '\t'), filePath, false);
                }

                #endregion
            }
        }

        /*
         * 汎用ファイル読み込みリスト生成
         */
        public static List<List<string>> toListDataFile(string path, char lineSeparator, char rowSeparator)
        {
            List<List<string>> allData = new List<List<string>>();

            // 1行ずつ分割
            string[] lines = FileUtil.readFile(path).Split(lineSeparator);

            foreach (string line in lines)
            {
                // 末尾の空行対策
                if (string.IsNullOrEmpty(line))
                {
                    // 念のためcontinue
                    continue;
                }

                // 1行をタブで分割
                string[] readStrArray = line.Split('\t');

                // 行リスト
                List<string> rowData = new List<string>();

                foreach (string readStr in readStrArray)
                {
                    rowData.Add(readStr);
                }

                // 1行分追加
                allData.Add(rowData);
            }

            return allData;
        }

        /*
         * 汎用ファイル書き込み用文字列生成
         */
        public static string toStringDataFile(List<List<string>> data, char lineSeparator, char rowSeparator)
        {
            StringBuilder sb = new StringBuilder();

            foreach (List<string> line in data)
            {
                for (int i = 0; i < line.Count; i++)
                {
                    sb.Append(line[i]);

                    if (line.Count - 1 > i)
                    {
                        sb.Append(rowSeparator);
                    }
                }

                sb.Append(lineSeparator);
            }

            return sb.ToString();
        }
    }
}
