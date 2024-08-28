using System;

namespace DivaNetAccess.src.VersionConfig
{
    public class VersionData
    {
        // 日付書式
        private const string DATE_FORMAT = "yyyyMMdd";

        // version.txt
        public DateTime version { get; set; }

        /*
         * コンストラクタ
         */
        public VersionData()
        {

        }

        /*
         * コンストラクタ(ファイル読み込み用)
         */
        public VersionData(string line)
        {
            version = DateTime.ParseExact(line, DATE_FORMAT, null);
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            return version.ToString(DATE_FORMAT);
        }
    }
}
