using System.Text;

namespace DivaNetAccess
{
    public class SongSettingData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";

        private enum textIndex
        {
            NAME = 0,
            MODULE1,
            MODULE2,
            BUTTON,
            SKIN
        }

        // 曲名
        public string name { get; set; }

        // モジュール1
        public string module1 { get; set; }

        // モジュール2
        public string module2 { get; set; }

        // ボタン音
        public string button { get; set; }

        // スキン
        public string skin { get; set; }

        /*
         * コンストラクタ
         */
        public SongSettingData()
        {

        }

        /*
         * ファイル読み込み用
         */
        public SongSettingData(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            name = data[(int)textIndex.NAME];
            module1 = data[(int)textIndex.MODULE1];
            module2 = data[(int)textIndex.MODULE2];
            button = data[(int)textIndex.BUTTON];
            skin = data[(int)textIndex.SKIN];
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(name + SEPALATOR);
            ret.Append(module1 + SEPALATOR);
            ret.Append(module2 + SEPALATOR);
            ret.Append(button + SEPALATOR);
            ret.Append(skin);

            ret.Append("\n");

            return ret.ToString();
        }
    }
}
