using System.Text;

namespace DivaNetAccess
{
    /*
     * 楽曲統計情報クラス
     */
    public class ToukeiData
    {
        // 難易度
        public static string[] DIFF_STR = { "EASY", "NORMAL", "HARD", "EXTREME", "EX EXT", "総合" };

        // 評価
        public static readonly string[] CLEAR_RESULT = { "-", "C", "G", "E", "P" };

        // 曲数
        public int[] songCnt { get; set; }

        // クリア数
        public int[,] clearCnt { get; set; }

        // トライアル数
        public int[,] trialCnt { get; set; }

        // 累計ポイント
        public int point { get; set; }

        // 合計達成率
        public int[] sumTasseiritu { get; set; }

        // 平均達成率
        public int[] avgTasseiritu { get; set; }

        // 理論値差数
        public int[] rironSaCnt { get; set; }

        // 現在のレベル
        public float level { get; set; }

        // 次のレベルまで
        public float nextLevel { get; set; }

        // 次のゲージ変動まで
        public float levelDankai { get; set; }

        // 次のゲージ変動まで
        public float nextGageAction { get; set; }

        // 現在の称号
        public RankData nowRank { get; set; }

        // 次の称号
        public RankData nextRank { get; set; }

        /*
         * コンストラクタ
         */
        public ToukeiData()
        {
            songCnt = new int[DIFF_STR.Length];
            clearCnt = new int[DIFF_STR.Length, CLEAR_RESULT.Length];
            trialCnt = new int[DIFF_STR.Length, CLEAR_RESULT.Length];
            point = 0;
            sumTasseiritu = new int[DIFF_STR.Length];
            avgTasseiritu = new int[DIFF_STR.Length];
            rironSaCnt = new int[DIFF_STR.Length];
        }

        /*
         * 合計達成率表示(フォーム表示用)
         */
        public string getTasseirituView(string str)
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < sumTasseiritu.Length; i++)
            {
                buf.Append(DIFF_STR[i]);
                buf.Append("".PadRight(8 - Encoding.GetEncoding("Shift_JIS").GetByteCount(DIFF_STR[i])));
                buf.Append(((decimal)sumTasseiritu[i] / 100).ToString("0.00").PadLeft(10));
                buf.Append("%");
                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * 楽曲数表示(フォーム表示用)
         */
        public string getSongCntViewAll()
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < songCnt.Length; i++)
            {
                buf.Append(DIFF_STR[i]);
                buf.Append("".PadRight(8 - Encoding.GetEncoding("Shift_JIS").GetByteCount(DIFF_STR[i])));
                buf.Append(songCnt[i].ToString().PadLeft(4));
                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * 合計達成率表示(フォーム表示用)
         */
        public string getSumTasseirituViewAll()
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < sumTasseiritu.Length; i++)
            {
                buf.Append(DIFF_STR[i]);
                buf.Append("".PadRight(8 - Encoding.GetEncoding("Shift_JIS").GetByteCount(DIFF_STR[i])));
                buf.Append(((decimal)sumTasseiritu[i] / 100).ToString("0.00").PadLeft(10));
                buf.Append("%");
                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * 平均達成率表示(フォーム表示用)
         */
        public string getAvgTasseirituViewAll()
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < avgTasseiritu.Length; i++)
            {
                buf.Append(DIFF_STR[i]);
                buf.Append("".PadRight(8 - Encoding.GetEncoding("Shift_JIS").GetByteCount(DIFF_STR[i])));
                buf.Append(((decimal)avgTasseiritu[i] / 100000).ToString("0.00000").PadLeft(10));
                buf.Append("%");
                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * 理論値差数表示(フォーム表示用)
         */
        public string getRironSaViewAll()
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < rironSaCnt.Length; i++)
            {
                buf.Append(DIFF_STR[i]);
                buf.Append("".PadRight(8 - Encoding.GetEncoding("Shift_JIS").GetByteCount(DIFF_STR[i])));
                buf.Append(rironSaCnt[i].ToString().PadLeft(4));
                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * クリア状況表示(フォーム表示用)
         */
        public string getClearCntViewAll()
        {
            StringBuilder buf = new StringBuilder();

            // クリア状況_1行目(ヘッダ)
            buf.Append("".PadRight(8));
            for (int i = 0; i < CLEAR_RESULT.Length; i++)
            {
                buf.Append(" [" + CLEAR_RESULT[i] + "] ");
            }
            buf.Append("\n\n");

            // クリア状況_2行目以降
            for (int i = 0; i < DIFF_STR.Length - 1; i++)
            {
                buf.Append(DIFF_STR[i].PadRight(7) + "  ");

                for (int j = 0; j < CLEAR_RESULT.Length; j++)
                {
                    buf.Append(clearCnt[i, j].ToString().PadLeft(3) + "  ");
                }

                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * トライアル状況表示(フォーム表示用)
         */
        public string getTrialCntViewAll()
        {
            StringBuilder buf = new StringBuilder();

            // トライアル状況_1行目(ヘッダ)
            buf.Append("".PadRight(8));
            for (int i = 0; i < CLEAR_RESULT.Length; i++)
            {
                buf.Append(" [" + CLEAR_RESULT[i] + "] ");
            }
            buf.Append("\n\n");

            // トライアル状況_2行目以降
            for (int i = 0; i < DIFF_STR.Length - 1; i++)
            {
                buf.Append(DIFF_STR[i].PadRight(7) + "  ");

                for (int j = 0; j < CLEAR_RESULT.Length; j++)
                {
                    buf.Append(trialCnt[i, j].ToString().PadLeft(3) + "  ");
                }

                buf.Append("\n");
            }

            return buf.ToString();
        }

        /*
         * 理論値差数表示(Twitter用)
         */
        public string getRironSaTwitter()
        {
            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < rironSaCnt.Length; i++)
            {
                buf.Append(string.Format("{0}:{1}", DIFF_STR[i], rironSaCnt[i]));

                if (i < rironSaCnt.Length - 1)
                {
                    buf.Append(" ");
                }
            }

            buf.Append(" #DivaNetAccess");

            return buf.ToString();
        }
    }
}
