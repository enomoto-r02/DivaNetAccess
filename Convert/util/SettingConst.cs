using System.Text;

namespace Convert.util
{
    public static class SettingConst
    {
        // 日付フォーマット
        public static string DATE_FORMAT_YYYYMMDDHHMMSS = "yyyyMMddHHmmss";
        public static string DATE_FORMAT_YYYYMMDDHHMMSS_SEPALATOR = "yyyy/MM/dd HH:mm:ss";


        // データディレクトリ名
        public static string DATA_DIR_NAME = "data";

        // コンフィグディレクトリ名
        public static string CONF_DIR_NAME = "conf";

        // Webページ文字コード
        public static readonly Encoding WEB_ENCODING = Encoding.UTF8;

        // ファイル文字コード
        public static readonly Encoding FILE_ENCODING = Encoding.UTF8;

        // バージョンファイル名
        public static string FILE_VERSION = "version.txt";

        // 楽曲情報ファイル名
        public static string FILE_SONG_DATA = "songData.txt";

        // プレイヤー情報ファイル名
        public static string FILE_PLAYER_DATA = "playerData.txt";

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

        // 楽曲別設定ファイル名
        public static string FILE_SONG_SETTING_DATA = "songSettingData.txt";

        // URL情報ファイル名
        public static string FILE_URL_SONG_DATA = "urlSongData.txt";
        public static string FILE_URL_MODULE_DATA = "urlModuleData.txt";
        public static string FILE_URL_SKIN_DATA = "urlSkinData.txt";
        public static string FILE_URL_BUTTON_DATA = "urlButtonData.txt";
    }
}
