using System.Collections.Generic;
using System.Text;

namespace DivaNetAccess
{
    public class UrlData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";
        private readonly string SEPALATOR_LINE = "\n";

        // モジュール
        public Dictionary<string, ModuleUrlData> moduleUrl { get; set; }

        // 楽曲
        public Dictionary<string, SongUrlData> songUrl { get; set; }

        // スキン
        public Dictionary<string, SkinUrlData> skinUrl { get; set; }

        // ボタン音
        public Dictionary<string, string> buttonUrl { get; set; }

        /*
         * コンストラクタ
         */
        public UrlData()
        {
            moduleUrl = new Dictionary<string, ModuleUrlData>();
            songUrl = new Dictionary<string, SongUrlData>();
            skinUrl = new Dictionary<string, SkinUrlData>();
            buttonUrl = new Dictionary<string, string>();
        }

        /*
         * モジュールURL書き込み用
         */
        public string ToStringModuleUrl()
        {
            StringBuilder ret = new StringBuilder();

            foreach (string name in moduleUrl.Keys)
            {
                ret.Append(moduleUrl[name].ToString());

                ret.Append(SEPALATOR_LINE);
            }

            return ret.ToString();
        }

        /*
         * 楽曲URL書き込み用
         */
        public string ToStringSongUrl()
        {
            StringBuilder ret = new StringBuilder();

            foreach (string name in songUrl.Keys)
            {
                ret.Append(songUrl[name].ToString());

                ret.Append(SEPALATOR_LINE);
            }

            return ret.ToString();
        }

        /*
         * スキンURL書き込み用
         */
        public string ToStringSkinUrl()
        {
            StringBuilder ret = new StringBuilder();

            foreach (string name in skinUrl.Keys)
            {
                ret.Append(skinUrl[name].ToString());

                ret.Append(SEPALATOR_LINE);
            }

            return ret.ToString();
        }

        /*
         * ボタン音URL書き込み用
         */
        public string ToStringButtonUrl()
        {
            StringBuilder ret = new StringBuilder();

            foreach (string name in buttonUrl.Keys)
            {
                // スキン名
                ret.Append(name + SEPALATOR);

                // URL
                ret.Append(buttonUrl[name]);

                ret.Append(SEPALATOR_LINE);
            }

            return ret.ToString();
        }
    }
}
