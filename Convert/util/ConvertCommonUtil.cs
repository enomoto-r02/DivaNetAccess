using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Convert.util
{
    public static class ConvertCommonUtil
    {
        /*
         * コンバート実行ファイルのパスをチェックする
         */
        public static bool isConvertDirectoryCheck()
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string exePath = myAssembly.Location;

            DirectoryInfo dirInfoNow = new DirectoryInfo(Path.GetDirectoryName(exePath));
            DirectoryInfo dirInfoParent = dirInfoNow.Parent;

            // カレントフォルダ名チェック
            if (!dirInfoParent.Name.Contains("DivaNetにアクセスするなにか"))
            {
                Console.WriteLine("\"Convert.exe\"は\"DivaNetにアクセスするなにか\"フォルダに配置してください");
                Console.ReadKey();

                return false;
            }

            // コンバート実行フォルダ名チェック
            if (!"Convert".Equals(dirInfoNow.Name))
            {
                Console.WriteLine("\"Convert.exe\"は\"Convert\"フォルダに配置してください");
                Console.ReadKey();

                return false;
            }

            return true;
        }

        /*
         * CONVフォルダからdataフォルダ内をファイルを検索し、対象ファイルのコレクションを返す
         */
        public static Dictionary<string, string> getDataFile(string targetFileName)
        {
            string dirPath = "..\\" + SettingConst.DATA_DIR_NAME + "\\";
            string[] folders;

            try
            {
                folders = Directory.GetDirectories(dirPath);
            }
            catch (Exception)
            {
                // dataフォルダなし
                return null;
            }

            // バックアップ用コレクション生成
            Dictionary<string, string> files = new Dictionary<string, string>();

            // アクセスコードフォルダ全検索
            foreach (string folder in folders)
            {
                string targetFilePath = Path.GetFullPath(Path.Combine(folder, SettingConst.FILE_RECORD_DATA));

                if (File.Exists(targetFilePath))
                {
                    FileInfo fileInfo = new FileInfo(targetFilePath);

                    // アクセスコードをキーにしてフルパスを格納
                    files.Add(fileInfo.Directory.Name, targetFilePath);
                }
            }

            return files;
        }

        /*
         * 対象ファイルを実行ファイルと同じ階層にフォルダを作成し、バックアップをする
         */
        public static void backupFiles(Dictionary<string, string> files)
        {
            string bkFolderName = "bk_" + DateTime.Now.ToString(SettingConst.DATE_FORMAT_YYYYMMDDHHMMSS);

            // 時分秒のフォルダを生成
            FileUtil.createFolder(bkFolderName);

            foreach (string accessCode in files.Keys)
            {
                string soutaiFolderPath = @".\" + bkFolderName + @"\" + accessCode;

                // アクセスコードのフォルダを生成
                FileUtil.createFolder(soutaiFolderPath);

                // ファイルのコピー
                File.Copy(files[accessCode], soutaiFolderPath + @"\" + SettingConst.FILE_RECORD_DATA);
            }
        }

        /*
         * ディレクトリのコピー
         * 引用元：http://www.woodensoldier.info/computer/csharptips/73.htm
         */
        public static void DirectoryCopy(string sourcePath, string destinationPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
            DirectoryInfo destinationDirectory = new DirectoryInfo(destinationPath);

            //コピー先のディレクトリがなければ作成する
            if (destinationDirectory.Exists == false)
            {
                destinationDirectory.Create();
                destinationDirectory.Attributes = sourceDirectory.Attributes;
            }

            //ファイルのコピー
            foreach (FileInfo fileInfo in sourceDirectory.GetFiles())
            {
                //同じファイルが存在していたら、常に上書きする
                fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);
            }

            //ディレクトリのコピー（再帰を使用）
            foreach (System.IO.DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
            {
                DirectoryCopy(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name);
            }
        }

        /*
         * ディレクトリのサイズ取得
         * 引用元：http://dobon.net/vb/dotnet/file/foldersize.html
         */
        public static long GetDirectorySize(DirectoryInfo dirInfo)
        {
            long size = 0;

            //フォルダ内の全ファイルの合計サイズを計算する
            foreach (FileInfo fi in dirInfo.GetFiles())
                size += fi.Length;

            //サブフォルダのサイズを合計していく
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
                size += GetDirectorySize(di);

            //結果を返す
            return size;
        }
    }
}
