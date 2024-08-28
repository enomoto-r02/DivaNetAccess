using DivaNetAccess.src.util;

namespace DivaNetAccess.src.Util
{
    public static class LogUtil
    {
        // ログディレクトリ名
        public static string LOG_DIR_NAME = "log";

        // ログファイル名
        public static string LOG_FILE_NAME = "error.log";

        /*
         * ログファイル書き込み
         */
        public static void writeLog(string message)
        {
            // ディレクトリ生成
            FileUtil.createFolder(LOG_DIR_NAME);

            // ログファイル書き込み
            FileUtil.writeFile(
                message,
                LOG_DIR_NAME + "/" + LOG_FILE_NAME,
                false       // true：追記、false：上書き
            );
        }
    }
}
