using System;
using System.Text;

namespace DivaNetAccess
{
    // ランクイン情報
    public class RankingData
    {
        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // 曲名
        public string name { get; set; }

        // 難易度
        public string diff { get; set; }

        // スコア
        public int score { get; set; }

        // 更新日
        public DateTime date { get; set; }

        // 順位
        public int rank { get; set; }


        /*
         * コンストラクタ
         */
        public RankingData()
        {
        }

        /*
         * コンストラクタ
         */
        public RankingData(string line)
        {
            string[] data = line.Split(char.Parse(SEPALATOR));

            name = data[0];
            diff = data[1];
            score = int.Parse(data[2]);
            date = DateTime.Parse(data[3]);
            rank = int.Parse(data[4]);
        }

        /*
         * ファイル書き込み用
         */
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();

            buf.Append(name + SEPALATOR);
            buf.Append(diff + SEPALATOR);
            buf.Append(score + SEPALATOR);
            buf.Append(date + SEPALATOR);
            buf.Append(rank);

            buf.Append("\n");

            return buf.ToString();
        }
    }
}
