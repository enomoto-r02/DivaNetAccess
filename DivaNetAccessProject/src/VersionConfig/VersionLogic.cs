using DivaNetAccess.src.Const;
using DivaNetAccess.src.util;
using System.IO;

namespace DivaNetAccess.src.VersionConfig
{
    public static class VersionLogic
    {
        private static readonly string PATH = SettingConst.CONF_DIR_NAME + "/" + SettingConst.FILE_VERSION;

        /*
         * バージョン情報読み込み
         */
        public static VersionData readVersionData()
        {

            VersionData version;

            // version.txtがあるか
            if (File.Exists(PATH) == false)
            {
                version = new VersionData(SettingConst.CREATE_DATE);

                // 無ければ作る
                writeVersionData(version);

                return version;
            }
            else
            {
                // ファイルを開く
                using (StreamReader sr = new StreamReader(
                    PATH,
                    SettingConst.FILE_ENCODING
                ))
                {
                    // バージョン取得
                    version = new VersionData(sr.ReadToEnd());
                }

                return version;
            }
        }

        /*
         * バージョン情報書き込み
         */
        private static void writeVersionData(VersionData version)
        {
            // ディレクトリ生成
            FileUtil.createFolder(SettingConst.CONF_DIR_NAME);

            // バージョン情報書き込み
            FileUtil.writeFile(
                version.ToString(),
                SettingConst.CONF_DIR_NAME + "/" + SettingConst.FILE_VERSION,
                false
            );
        }


    }
}
