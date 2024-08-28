using System.Text;

namespace DivaNetAccess
{
    public class SkinUrlData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // スキン名
        public string name { get; set; }

        // スキングループURL
        public string skinGroupUrl { get; set; }

        // スキンURL
        public string skinUrl { get; set; }

        // 購入フラグ
        public bool isBought { get; set; }

        // 必須VP
        public int vp { get; set; }

        // 必須チケット＠今後の仕様変更用
        public int ticket { get; set; }

        /*
         * コンストラクタ
         */
        public SkinUrlData()
        {
        }

        /*
         * コンストラクタ
         */
        public SkinUrlData(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            name = data[0];
            skinGroupUrl = data[1];
            skinUrl = data[2];
            isBought = bool.Parse(data[3]);
            vp = int.Parse(data[4]);
            ticket = int.Parse(data[5]);
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();

            buf.Append(name + SEPALATOR);
            buf.Append(skinGroupUrl + SEPALATOR);
            buf.Append(skinUrl + SEPALATOR);
            buf.Append(isBought + SEPALATOR);
            buf.Append(vp + SEPALATOR);
            buf.Append(ticket);

            return buf.ToString();
        }
    }
}
