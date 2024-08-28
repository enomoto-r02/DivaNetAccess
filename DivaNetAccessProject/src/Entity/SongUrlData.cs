using System.Text;

namespace DivaNetAccess
{
    public class SongUrlData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // 公開順
        public int _koukaiOrde { get; set; }

        // 曲名順
        public int _nameOrder { get; set; }

        // 種類(0：ソロ、1；衣装チェンジ、2：デュエット)
        public int soloNumber { get; set; }

        // 曲名
        public string name { get; set; }

        // URL
        public string url { get; set; }

        /*
         * コンストラクタ
         */
        public SongUrlData()
        {
        }

        /*
         * コンストラクタ
         */
        public SongUrlData(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            _koukaiOrde = int.Parse(data[0]);
            _nameOrder = int.Parse(data[1]);
            soloNumber = int.Parse(data[2]);
            name = data[3];
            url = data[4];
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();

            buf.Append(_koukaiOrde + SEPALATOR);
            buf.Append(_nameOrder + SEPALATOR);
            buf.Append(soloNumber + SEPALATOR);
            buf.Append(name + SEPALATOR);
            buf.Append(url);

            return buf.ToString();
        }
    }
}
