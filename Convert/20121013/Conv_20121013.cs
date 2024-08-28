using Convert.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Convert._20121013
{
    public static class Conv_20121013
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

            // version.txt削除
            deleteVersionFile();

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
            if (Directory.Exists("../data"))
            {
                bkDirNames.Add(new string[] { "../data", "./bk_" + bkDirectoryName + "/data" });
            }
            if (Directory.Exists("../conf"))
            {
                bkDirNames.Add(new string[] { "../conf", "./bk_" + bkDirectoryName + "/conf" });
            }

            return bkDirNames;
        }

        /*
         * 対象フォルダのバックアップ処理＠Conv_20121013用
         */
        private static bool backupTargetFolder(List<string[]> bkDirNames)
        {
            foreach (string[] bkDirName in bkDirNames)
            {
                ConvertCommonUtil.DirectoryCopy(bkDirName[0], bkDirName[1]);

                // バックアップサイズチェック
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
         * version.txt削除
         */
        private static void deleteVersionFile()
        {
            string versionFilePath = "../conf/version.txt";

            if (File.Exists(versionFilePath))
            {
                File.Delete(versionFilePath);
            }
        }

        /*
         * コンバート
         */
        private static void convert(List<string[]> bkDirNames)
        {
            foreach (string[] bkDirName in bkDirNames)
            {
                // txtファイルをすべて取得
                string[] filePaths = System.IO.Directory.GetFiles(bkDirName[0], "*.txt", System.IO.SearchOption.AllDirectories);

                foreach (string filePath in filePaths)
                {
                    // Shift_JISで読み込む
                    string fileStr = readFile(filePath, Encoding.GetEncoding("Shift_JIS"));

                    // UTF-8で書き込む
                    writeFile(fileStr, filePath, Encoding.UTF8);
                }
            }
        }

        /*
         * ファイルを読み込む
         */
        private static string readFile(string path, Encoding encoding)
        {
            string ret = "";

            using (StreamReader sr = new StreamReader(
                path,
                encoding
            ))
            {
                // 文字列を設定
                ret = sr.ReadToEnd();
                sr.Close();
            }

            return ret;
        }

        /*
         * ファイルに書き込む
         */
        private static void writeFile(string str, string path, Encoding encoding)
        {
            using (StreamWriter writer = new StreamWriter(
                path,
                false,             // true：追記、false：上書き
                encoding
            ))
            {
                // 楽曲情報を書き込む
                writer.Write(str);
                writer.Close();
            }
        }
    }
}
