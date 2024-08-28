using DivaNetAccessLib;
using System;
using System.Text;

namespace DivaNetAccess
{
    public class ResultData : BaseEntity
    {
        public override string GetKey(params string[] data)
        {
            throw new NotImplementedException();
        }

        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // 難易度
        public string diff { get; set; }

        // クリア
        public string clear { get; set; }

        // 達成率
        public int tasseiritu { get; set; }

        // スコア
        public int score { get; set; }

        // トライアル
        public string trial { get; set; }

        // 連続パーフェクトトライアル現在数
        public int trial_now { get; set; }

        // 連続パーフェクトトライアル最高記録
        public int trial_max { get; set; }

        // HISPEED
        public string hispeed { get; set; }

        // HIDDEN
        public string hidden { get; set; }

        // SUDDEN
        public string sudden { get; set; }

        // ★
        public string star { get; set; }

        public override string Key { get { return diff; } }

        /*
         * 楽曲情報書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(diff + SEPALATOR);
            ret.Append(clear + SEPALATOR);
            ret.Append(tasseiritu + SEPALATOR);
            ret.Append(score + SEPALATOR);
            ret.Append(trial + SEPALATOR);
            ret.Append(trial_now + SEPALATOR);
            ret.Append(trial_max + SEPALATOR);
            ret.Append(hispeed + SEPALATOR);
            ret.Append(hidden + SEPALATOR);
            ret.Append(sudden + SEPALATOR);
            ret.Append(star);

            return ret.ToString();
        }

        /*
         * 達成率理論値書き込み用
         */
        public string ToStringTasseirituRiron()
        {
            StringBuilder ret = new StringBuilder();

            ret.Append(diff + SEPALATOR);
            ret.Append(tasseiritu);

            return ret.ToString();
        }

        /*
         * コンストラクタ
         */
        public ResultData()
        {
            // クリア統計情報から取得時のnull対策＠ライバル情報設定時に実装、影響範囲不明
            clear = DivaNetAccess.src.util.WebUtil.CLEAR_RESULT_CHAR[0];
            trial = DivaNetAccess.src.util.WebUtil.CLEAR_RESULT_CHAR[0];
        }

        // コンストラクタ(ファイル読み込み)
        public ResultData(string[] songRows)
        {
            diff = songRows[0];
            clear = songRows[1];
            tasseiritu = int.Parse(songRows[2]);
            score = int.Parse(songRows[3]);
            trial = songRows[4];
            trial_now = int.Parse(songRows[5]);
            trial_max = int.Parse(songRows[6]);
            hispeed = songRows[7];
            hidden = songRows[8];
            sudden = songRows[9];
            star = songRows[10];
        }

        /*
         * 達成率理論値用(ファイル読み込み)
         */
        public void resultDataRiron(string[] songRows)
        {
            diff = songRows[0];
            tasseiritu = int.Parse(songRows[1]);
        }

        /*
         * 達成率取得(表示用)
         */
        public float tasseirituV()
        {
            return (float)(tasseiritu / 100f);
        }

        public override void Load()
        {
            ;
        }
    }
}
