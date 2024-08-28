using DivaNetAccess.src.Const;
using DivaNetAccess.src.PlayRecordToukei;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DivaNetAccess.src.util
{
    public static class DivaNetUtil
    {
        /*
         * プレイヤー情報書き込み
         */
        public static void writePlayerData(Player player)
        {
#warning なおす
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + player.accessCode);

            // プレイヤー情報書き込み
            FileUtil.writeFile(
                player.Write(),
                SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + Player.FILE_NAME,
                false
            );
        }

        /*
         * メモファイル書き込み
         */
        private static void writeMemoData(Player player, Dictionary<string, MemoData> memos, string path)
        {
            StringBuilder buf = new StringBuilder();

            foreach (string key in memos.Keys)
            {
                buf.Append(memos[key].ToString() + "\n");
            }

            // メモ情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                path,
                false
            );
        }

        /*
         * メモファイル書き込み＠プレイヤー読み込み用
         */
        public static void writeMemoDataPlayer(Player player, Dictionary<string, MemoData> memos)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_MEMO_DATA}";
            DivaNetUtil.writeMemoData(player, memos, path);
        }

        /*
         * メモファイル読み込み
         */
        public static Dictionary<string, MemoData> _readMemoData(string path)
        {
            Dictionary<string, MemoData> ret = new Dictionary<string, MemoData>();

            // メモファイルが無い時はインスタンスを生成して終了
            if (File.Exists(path) == false)
            {
                return ret;
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
                    // メモ情報生成
                    MemoData memo = new MemoData(line);

                    // 曲名＋難易度をキーとしてリストに追加
                    //ret.Add(memo.diff + memo.name, memo);
                    ret.Add(memo.Key, memo);
                }
            }

            return ret;
        }

        /*
         * メモファイル読み込み
         */
        public static Dictionary<string, MemoData> readMemoData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_MEMO_DATA}";
            return DivaNetUtil._readMemoData(path);
        }

        /*
         * 称号ファイル読み込み
         */
        public static List<RankData> readRankData()
        {
            string path = SettingConst.CONF_DIR_NAME + "/" + SettingConst.FILE_RANK_DATA;

            List<RankData> ret = new List<RankData>();

            // 称号ファイルが無い時はインスタンスを生成して終了
            if (File.Exists(path) == false)
            {
                ret = createRankData();
                writeRankData(ret);

                return ret;
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
                    RankData rank = new RankData(line);

                    // リストに追加
                    ret.Add(rank);
                }
            }

            return ret;
        }

        /*
         * 称号リスト作成
         */
        public static List<RankData> createRankData()
        {
            List<RankData> ret = createRankList();

            writeRankData(ret);

            return ret;
        }

        /*
         * 称号リスト作成
         */
        private static List<RankData> createRankList()
        {
            List<RankData> ret = new List<RankData>();

            ret.Add(new RankData("0,エチュード"));
            ret.Add(new RankData("200,プレリュード"));
            ret.Add(new RankData("400,バラード"));
            ret.Add(new RankData("600,カンタータ"));
            ret.Add(new RankData("750,ノクターン"));
            ret.Add(new RankData("900,セレナーデ"));
            ret.Add(new RankData("1050,ロンド"));
            ret.Add(new RankData("1200,ワルツ"));
            ret.Add(new RankData("1350,カンツォーナ"));
            ret.Add(new RankData("1500,メヌエット"));
            ret.Add(new RankData("1650,マーチ"));
            ret.Add(new RankData("1800,ララバイ"));
            ret.Add(new RankData("1950,カッサシオン"));
            ret.Add(new RankData("2100,フーガ"));
            ret.Add(new RankData("2250,ボレロ"));
            ret.Add(new RankData("2400,カプリス"));
            ret.Add(new RankData("2550,スケルツォ"));
            ret.Add(new RankData("2700,ソナタ"));
            ret.Add(new RankData("2850,バルカロール"));
            ret.Add(new RankData("3000,ガヴォット"));
            ret.Add(new RankData("3150,マドリガーレ"));
            ret.Add(new RankData("3300,ジグ"));
            ret.Add(new RankData("3450,アリア"));
            ret.Add(new RankData("3600,マズルカ"));
            ret.Add(new RankData("3750,パヴァーヌ"));
            ret.Add(new RankData("3900,ガイヤルド"));
            ret.Add(new RankData("4050,インテルメッツォ"));
            ret.Add(new RankData("4200,ルフラン"));
            ret.Add(new RankData("4350,シャコンヌ"));
            ret.Add(new RankData("4500,パッサカリア"));
            ret.Add(new RankData("4650,アルマンド"));
            ret.Add(new RankData("4800,クーラント"));
            ret.Add(new RankData("4950,サラバンド"));
            ret.Add(new RankData("5100,カヴァティーナ"));
            ret.Add(new RankData("5250,ロマンス"));
            ret.Add(new RankData("5400,インベンション"));
            ret.Add(new RankData("5550,シンフォニア"));
            ret.Add(new RankData("5700,パルティータ"));
            ret.Add(new RankData("5850,コンチェルト"));
            ret.Add(new RankData("6000,リゴドン"));
            ret.Add(new RankData("6150,ディヴェルティメント"));
            ret.Add(new RankData("6300,オーバーチュア"));
            ret.Add(new RankData("6450,フリアント"));
            ret.Add(new RankData("6600,ノエル"));
            ret.Add(new RankData("6750,レチタティーヴォ"));
            ret.Add(new RankData("6900,ファンタジア"));
            ret.Add(new RankData("7050,フォルラーヌ"));
            ret.Add(new RankData("7200,サルタレロ"));
            ret.Add(new RankData("7350,リチェルカーレ"));
            ret.Add(new RankData("7500,トラッド"));
            ret.Add(new RankData("7650,オラトリオ"));
            ret.Add(new RankData("7800,トレパーク"));
            ret.Add(new RankData("7950,ファランドール"));
            ret.Add(new RankData("8100,ラプソディ"));
            ret.Add(new RankData("8250,エコセーズ"));
            ret.Add(new RankData("8400,コロラトゥーラ"));
            ret.Add(new RankData("8550,コントルダンス"));
            ret.Add(new RankData("8700,アルペジオ"));
            ret.Add(new RankData("8850,カンパネラ"));
            ret.Add(new RankData("9000,デクラメーション"));
            ret.Add(new RankData("9150,エスタンピー"));
            ret.Add(new RankData("9300,エレジー"));
            ret.Add(new RankData("9450,ラメント"));
            ret.Add(new RankData("9600,タランテラ"));
            ret.Add(new RankData("9750,アンサンブル"));
            ret.Add(new RankData("9850,アンティフォナ"));
            ret.Add(new RankData("9950,カデンツァ"));
            ret.Add(new RankData("10050,カノン"));
            ret.Add(new RankData("10080,ディーヴァ"));

            return ret;
        }

        /*
         * 称号書き込み＠初期化用
         */
        private static void writeRankData(List<RankData> ranks)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.CONF_DIR_NAME);
            string path = SettingConst.CONF_DIR_NAME + "/" + SettingConst.FILE_RANK_DATA;

            StringBuilder buf = new StringBuilder();

            foreach (RankData rank in ranks)
            {
                buf.Append(rank.ToString());
            }

            // 楽曲情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                path,
                false
            );
        }

        /*
         * ランクイン情報書き込み
         */
        public static void writeRankingData(Player player, Dictionary<string, RankingData> rankings)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + player.accessCode);

            StringBuilder buf = new StringBuilder();

            foreach (string key in rankings.Keys)
            {
                buf.Append(rankings[key].ToString());
            }

            // 楽曲情報書き込み
            FileUtil.writeFile(
                buf.ToString(),
                SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_RANKING_DATA,
                false
            );
        }

        /*
         * ランクイン情報読み込み
         */
        public static Dictionary<string, RankingData> _readRankingData(string path)
        {
            Dictionary<string, RankingData> ret = new Dictionary<string, RankingData>();

            // ランキング情報が無い時はインスタンスを生成して終了
            if (File.Exists(path) == false)
            {
                return ret;
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
                    RankingData ranking = new RankingData(line);

                    // リストに追加
                    ret.Add(ranking.diff + ranking.name, ranking);
                }
            }

            return ret;
        }

        /*
         * ランクイン情報読み込み
         */
        public static Dictionary<string, RankingData> readRankingData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_RANKING_DATA}";
            return DivaNetUtil._readRankingData(path);
        }

        /*
         * 楽曲ファイルが存在するかチェック＠プレイ履歴等のチェック用
         */
        public static bool isSongDataFile(Player player)
        {
            string path = $"{player.DirPath}{SettingConst.FILE_SONG_DATA}";
            // 楽曲ファイル存在確認
            return File.Exists(path);
        }

        /*
         * URL情報書き込み
         */
        public static void writeUrlData(Player player, UrlData url)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME);
            FileUtil.createFolder(SettingConst.DATA_DIR_NAME + "/" + player.accessCode);

            // 楽曲URL書き込み
            FileUtil.writeFile(
                url.ToStringSongUrl(),
                SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_URL_SONG_DATA,
                false
            );

            //// モジュールURL書き込み
            //FileUtil.writeFile(
            //    url.ToStringModuleUrl(),
            //    SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_URL_MODULE_DATA,
            //    false
            //);

            //// スキンURL書き込み
            //FileUtil.writeFile(
            //    url.ToStringSkinUrl(),
            //    SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_URL_SKIN_DATA,
            //    false
            //);

            //// ボタン音URL書き込み
            //FileUtil.writeFile(
            //    url.ToStringButtonUrl(),
            //    SettingConst.DATA_DIR_NAME + "/" + player.accessCode + "/" + SettingConst.FILE_URL_BUTTON_DATA,
            //    false
            //);
        }

        /*
         * URL情報読み込み
         */
        public static UrlData readUrlData(Player player)
        {
            // プレイヤー情報生成
            UrlData ret = new UrlData();

            if (player != null)
            {
                ret.songUrl = readSongUrlData(player);
                ret.moduleUrl = readModuleUrlData(player);
                ret.skinUrl = readSkinUrlData(player);
                ret.buttonUrl = readButtonUrlData(player);
            }

            return ret;
        }

        /*
         * 楽曲URL情報読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, SongUrlData> _readSongUrlData(string path)
        {
            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                // nullを返す
                return null;
            }

            Dictionary<string, SongUrlData> ret = new Dictionary<string, SongUrlData>();

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
                    SongUrlData songUrl = new SongUrlData(line);

                    // リストに追加
                    ret.Add(songUrl.name, songUrl);
                }
            }

            return ret;
        }

        /*
         * 楽曲URL情報読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, SongUrlData> readSongUrlData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_URL_SONG_DATA}";
            return DivaNetUtil._readSongUrlData(path);
        }

        /*
         * モジュールURL読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, ModuleUrlData> _readModuleUrlData(string path)
        {
            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                // nullを返す
                return null;
            }

            Dictionary<string, ModuleUrlData> ret = new Dictionary<string, ModuleUrlData>();

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
                    ModuleUrlData module = new ModuleUrlData(line);

                    // リストに追加
                    ret.Add(module.name, module);
                }
            }

            return ret;
        }

        /*
         * モジュールURL読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, ModuleUrlData> readModuleUrlData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_URL_MODULE_DATA}";
            return DivaNetUtil._readModuleUrlData(path);
        }

        /*
         * スキンURL読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, SkinUrlData> _readSkinUrlData(string path)
        {
            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                // nullを返す
                return null;
            }

            Dictionary<string, SkinUrlData> ret = new Dictionary<string, SkinUrlData>();

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
                    SkinUrlData skin = new SkinUrlData(line);

                    // リストに追加
                    ret.Add(skin.name, skin);
                }
            }

            return ret;
        }

        private static Dictionary<string, SkinUrlData> readSkinUrlData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_URL_SKIN_DATA}";
            return DivaNetUtil._readSkinUrlData(path);
        }

        /*
         * スキンURL読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, string> _readButtonUrlData(string path)
        {
            // プレイヤー情報ファイル確認
            if (File.Exists(path) == false)
            {
                // nullを返す
                return null;
            }

            Dictionary<string, string> ret = new Dictionary<string, string>();

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
                    string[] data = line.Split('\t');

                    // リストに追加
                    ret.Add(data[0], data[1]);
                }
            }

            return ret;
        }

        /*
         * スキンURL読み込み＠URL読み込みから呼び出される
         */
        private static Dictionary<string, string> readButtonUrlData(Player player)
        {
            string path = $"{player.DirPath}/{SettingConst.FILE_URL_SKIN_DATA}";
            return DivaNetUtil._readButtonUrlData(path);
        }

        /*
         * 楽曲URL取得
         */
        public static string convSongUrl(string url)
        {
            // 危険？
            return url.Split('/')[6];
        }

        /*
         * モジュールURL取得
         */
        public static string convModuleUrl(string url)
        {
            // 危険？
            return url.Split('/')[6];
        }

        /*
         * モジュールURL取得(ランダム)
         */
        public static string convModuleUrlRandom(string url)
        {
            // 危険？
            return url.Split('/')[8];
        }

        /*
         * スキングループURL取得
         */
        public static string convSkinGroupUrl(string url, bool isBought)
        {
            if (isBought)
            {
                // 購入済み
                return url.Split('/')[8];
            }
            else
            {
                // 未購入
                return url.Split('/')[7];
            }

        }

        /*
         * スキンURL取得
         */
        public static string convSkinUrl(string url, bool isBought)
        {
            if (isBought)
            {
                // 購入済み
                return url.Split('/')[7];
            }
            else
            {
                // 未購入
                return url.Split('/')[6];
            }

        }

        /*
         * ボタン音URL取得
         */
        public static string convButtonUrl(string url)
        {
            // 危険？
            return url.Split('/')[7];
        }


        /*
         * クリア統計詳細情報のリンクURLからクリア状況を取得
         */
        public static string getDiffFromClearToukeiDetails(string url)
        {
            string diffStr = url.Split('/')[7];
            if (diffStr.Equals("EXTRA_EXTREME"))
            {
                diffStr = "EX EXTREME";
            }

            return diffStr;
        }

        /*
         * クリア統計詳細情報のリンクURLから難易度状況を取得
         */
        public static string getClearFromClearToukeiDetails(string url)
        {
            string key = url.Split('/')[8];

            Dictionary<string, string> mappings = new Dictionary<string, string>();

            mappings.Add("-1", "-");
            mappings.Add("2", "C");
            mappings.Add("3", "G");
            mappings.Add("4", "E");
            mappings.Add("5", "P");

            if (mappings.ContainsKey(key))
            {
                return mappings[key];
            }

            return "";
        }

        /*
         * クリア統計詳細情報のリンクURLからトライアル状況を取得
         */
        public static string getTrialFromClearToukeiDetails(string url)
        {
            string key = url.Split('/')[8];

            Dictionary<string, string> mappings = new Dictionary<string, string>();

            mappings.Add("-1", "-");
            mappings.Add("0", "C");
            mappings.Add("1", "G");
            mappings.Add("2", "E");
            mappings.Add("3", "P");

            if (mappings.ContainsKey(key))
            {
                return mappings[key];
            }

            return "";
        }

        /*
         * ライバル情報のPR表示用
         */
        public static string getNewLineStrByte(string input, int maxByteCount)
        {
            Encoding encoding = Encoding.UTF8;

            char[] chars = input.ToCharArray();

            // 改行バイト数まで満たないならそのまま表示
            if (chars.Length <= maxByteCount)
            {
                return input;
            }

            StringBuilder sb = new StringBuilder();

            int lineCnt = 1;
            foreach (char c in chars)
            {
                sb.Append(c);

                if (lineCnt >= maxByteCount)
                {
                    sb.AppendLine();
                    lineCnt = 1;
                }

                lineCnt++;
            }

            return sb.ToString();
        }

        /*
         * Dictionary→ソートされたリスト変換
         */
        public static List<SortValue> getSortValue(Dictionary<string, int> dict)
        {

            List<SortValue> tests = new List<SortValue>();

            foreach (string key in dict.Keys)
            {
                SortValue test = new SortValue(key, dict[key]);
                tests.Add(test);
            }
            tests.Sort();

            return tests;
        }

        public static void getSortValue2(IDictionary<string, Player> itemTable)
        {
            // キーで昇順
            IOrderedEnumerable<KeyValuePair<string, Player>> _table_1 =
                itemTable.OrderBy(selector =>
                {
                    return selector.Key;
                });

            // キーで降順
            IOrderedEnumerable<KeyValuePair<string, Player>> _table_2 =
                itemTable.OrderByDescending(selector =>
                {
                    return selector.Key;
                });
        }
    }
}
