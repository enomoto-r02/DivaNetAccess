using Convert.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Convert._20120711
{
    public static class Conv_20120711
    {
        /*
         * メイン処理
         */
        public static void convMain(string[] args)
        {
            // 対象ファイル取得
            Dictionary<string, string> files = ConvertCommonUtil.getDataFile(SettingConst.FILE_RECORD_DATA);

            // ファイルなし
            if (files == null)
            {
                Console.WriteLine("コンバート対象ファイルがありません。");
                Console.ReadKey();

                return;
            }

            // バックアップ
            ConvertCommonUtil.backupFiles(files);

            // コンバート
            List<SortedDictionary<string, newPlayRecordEntity>> newRecordsAllUser = convert(files);

            // 上書き
            writePlayRecordData(files, newRecordsAllUser);

            Console.WriteLine("コンバートが完了しました。");
            Console.ReadKey();

            return;
        }

        /*
         * コンバート
         */
        public static List<SortedDictionary<string, newPlayRecordEntity>> convert(Dictionary<string, string> files)
        {
            // 全プレイヤーの旧プレイ履歴リスト読み込み
            List<SortedDictionary<string, oldPlayRecordEntity>> oldRecordsAllUser = new List<SortedDictionary<string, oldPlayRecordEntity>>();

            foreach (string accessCode in files.Keys)
            {
                oldRecordsAllUser.Add(readOldPlayRecordData(files[accessCode]));
            }

            // 全プレイヤーの新プレイ履歴リストに変換
            List<SortedDictionary<string, newPlayRecordEntity>> newRecordsAllUser = new List<SortedDictionary<string, newPlayRecordEntity>>();

            foreach (SortedDictionary<string, oldPlayRecordEntity> oldRecords in oldRecordsAllUser)
            {
                SortedDictionary<string, newPlayRecordEntity> newRerocrds = new SortedDictionary<string, newPlayRecordEntity>();

                foreach (string key in oldRecords.Keys)
                {
                    oldPlayRecordEntity oldRecord = oldRecords[key];
                    newPlayRecordEntity newRecord = new newPlayRecordEntity();

                    // 項目セット
                    newRecord.date = DateTime.Parse(oldRecord.date);
                    newRecord.place = oldRecord.place;
                    newRecord.name = oldRecord.name;
                    newRecord.diff = oldRecord.diff.Split('　')[0];
                    newRecord.star = int.Parse(oldRecord.diff.Split('　')[1].Replace("★", ""));
                    newRecord.clear = oldRecord.clear.Trim();
                    newRecord.tasseiritu = convTasseiritu(oldRecord.tasseiritu);
                    newRecord.score = int.Parse(oldRecord.score);
                    newRecord.cool = int.Parse(oldRecord.cool.Split('/')[0]);
                    newRecord.coolP = convTasseiritu(oldRecord.cool.Split('/')[1]);
                    newRecord.fine = int.Parse(oldRecord.fine.Split('/')[0]);
                    newRecord.fineP = convTasseiritu(oldRecord.fine.Split('/')[1]);
                    newRecord.safe = int.Parse(oldRecord.safe.Split('/')[0]);
                    newRecord.safeP = convTasseiritu(oldRecord.safe.Split('/')[1]);
                    newRecord.sad = int.Parse(oldRecord.sad.Split('/')[0]);
                    newRecord.sadP = convTasseiritu(oldRecord.sad.Split('/')[1]);
                    newRecord.worst = int.Parse(oldRecord.worst.Split('/')[0]);
                    newRecord.worstP = convTasseiritu(oldRecord.worst.Split('/')[1]);
                    newRecord.combo = int.Parse(oldRecord.combo);
                    newRecord.challenge = int.Parse(oldRecord.challenge);
                    newRecord.hold = int.Parse(oldRecord.hold);
                    newRecord.trial = oldRecord.trial;
                    newRecord.module1 = oldRecord.module1;
                    newRecord.module2 = oldRecord.module2;
                    newRecord.button = oldRecord.button;
                    newRecord.skin = oldRecord.skin;
                    newRecord.memo = "";

                    // キー生成
                    newRecord.makeKey();
                    newRerocrds.Add(newRecord.key, newRecord);
                }

                newRecordsAllUser.Add(newRerocrds);
            }

            return newRecordsAllUser;
        }



        /*
         * プレイ履歴読み込み
         */
        public static SortedDictionary<string, oldPlayRecordEntity> readOldPlayRecordData(string path)
        {
            // プレイヤー情報生成
            SortedDictionary<string, oldPlayRecordEntity> ret = new SortedDictionary<string, oldPlayRecordEntity>();

            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                return null;
            }

            // ファイルを開く
            using (StreamReader sr = new StreamReader(
                path,
                SettingConst.FILE_ENCODING
            ))
            {
                string line;

                // ファイルの末尾まで
                while ((line = sr.ReadLine()) != null)
                {
                    oldPlayRecordEntity record = new oldPlayRecordEntity(line);

                    // リストに追加
                    ret.Add(record.key, record);
                }
            }

            return ret;
        }

        /*
         * プレイ履歴書き込み
         */
        public static void writePlayRecordData(Dictionary<string, string> files, List<SortedDictionary<string, newPlayRecordEntity>> newRecordsAllUser)
        {
            int i = 0;

            foreach (string accessCode in files.Keys)
            {
                string path = files[accessCode];

                // 1プレイヤー用バッファ
                StringBuilder buf = new StringBuilder();

                SortedDictionary<string, newPlayRecordEntity> newRecords = newRecordsAllUser[i];
                foreach (string key in newRecords.Keys)
                {
                    buf.Append(newRecords[key].ToString());
                }

                // プレイヤー情報書き込み
                FileUtil.writeFile(
                    buf.ToString(),
                    path,
                    false
                );

                i++;
            }
        }

        /*
         * 達成率変換(XX.XX%)→(XXXX)
         */
        public static int convTasseiritu(string tasseiritu)
        {
            string[] tmpArray = tasseiritu.Replace("％", "").Replace("%", "").Split('.');
            return int.Parse(int.Parse(tmpArray[0]) + tmpArray[1].PadRight(2, '0'));
        }
    }
}
