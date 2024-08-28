using System.Text;

namespace DivaNetAccess.src.Const
{
    public static class SettingConst
    {
#if DEBUG
        // バージョン＠公開日
        public static string CREATE_DATE = "xxxxxxxx";

        // ウインドウ名
        public static string WINDOW_TITLE = "DivaNetにアクセスするなにか" + "_" + CREATE_DATE + "_DEBUG";
#else
        // バージョン＠公開日
        public static string CREATE_DATE = "20201209";

        // ウインドウ名
        public static string WINDOW_TITLE = "DivaNetにアクセスするなにか" + "_" + CREATE_DATE;
#endif

        // Webページ文字コード
        public static readonly Encoding WEB_ENCODING = Encoding.UTF8;

        // ファイル文字コード
        public static readonly Encoding FILE_ENCODING = Encoding.UTF8;

        // データディレクトリ名
        public static string DATA_DIR_NAME = "data";

        // コンフィグディレクトリ名
        public static string CONF_DIR_NAME = "conf";

        // データディレクトリ名(ライバル)
        public static string RIVAL_DIR_NAME = "rival";

        // バージョンファイル名
        public static string FILE_VERSION = "version.txt";

        // 楽曲情報ファイル名
        public static string FILE_SONG_DATA = "songData.txt";

        // メモ情報ファイル名
        public static string FILE_MEMO_DATA = "memoData.txt";

        // 称号ファイル名
        public static string FILE_RANK_DATA = "rank.txt";

        // ランクイン情報ファイル名
        public static string FILE_RANKING_DATA = "rankingData.txt";

        // 達成率理論値ファイル名
        public static string FILE_TASSEIRITU_RIRON_DATA = "wikiRironData.txt";

        // プレイ履歴ファイル名
        public static string FILE_RECORD_DATA = "playRecordData.txt";

        // マイリストファイル名
        public static string FILE_MYLIST_DATA = "myList.txt";

        // 楽曲別設定ファイル名
        public static string FILE_SONG_SETTING_DATA = "songSettingData.txt";

        // コレクションカードファイル名
        public static string FILE_COLLECTION_CARD_DATA = "collectionCard.txt";

        // URL情報ファイル名
        public static string FILE_URL_SONG_DATA = "urlSongData.txt";
        public static string FILE_URL_MODULE_DATA = "urlModuleData.txt";
        public static string FILE_URL_SKIN_DATA = "urlSkinData.txt";
        public static string FILE_URL_BUTTON_DATA = "urlButtonData.txt";
    }
}
