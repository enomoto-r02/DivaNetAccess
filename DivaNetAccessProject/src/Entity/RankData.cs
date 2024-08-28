using System.Text;

namespace DivaNetAccess
{
    // 称号クラス
    public class RankData
    {
        // 区切り文字
        private readonly string SEPALATOR = ",";

        // 称号名
        public string name { get; private set; }

        // 必須ポイント
        public int point { get; private set; }

        /*
         * コンストラクタ
         */
        public RankData(string line)
        {
            string[] array = line.Split(char.Parse(SEPALATOR));

            point = int.Parse(array[0]);
            name = array[1];
        }

        /*
         * 書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(point + SEPALATOR);
            ret.Append(name);
            ret.Append("\n");

            return ret.ToString();
        }
    }
}
