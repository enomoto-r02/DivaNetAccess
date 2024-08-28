using System.Text;

namespace DivaNetAccess
{
    public class ModuleUrlData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // モジュール名
        public string name { get; set; }

        // URL
        public string url { get; set; }

        // キャラクターNo
        public int charNo { get; set; }

        // 購入フラグ
        public bool isBought { get; set; }

        // 必須VP
        public int vp { get; set; }

        // 必須チケット＠今後の仕様変更用
        public int ticket { get; set; }

        /*
         * コンストラクタ
         */
        public ModuleUrlData()
        {
        }

        /*
         * コンストラクタ(ファイル読み込み用)
         */
        public ModuleUrlData(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            name = data[0];
            url = data[1];
            charNo = int.Parse(data[2]);
            isBought = bool.Parse(data[3]);
            vp = int.Parse(data[4]);
            ticket = int.Parse(data[5]);
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(name + SEPALATOR);
            ret.Append(url + SEPALATOR);
            ret.Append(charNo + SEPALATOR);
            ret.Append(isBought + SEPALATOR);
            ret.Append(vp + SEPALATOR);
            ret.Append(ticket);

            return ret.ToString();
        }
    }
}
