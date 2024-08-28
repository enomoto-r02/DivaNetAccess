using DivaNetAccessLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace DivaNetAccess
{
    public class SongData : BaseEntity
    {
        public override string GetKey(params string[] data)
        {
            throw new NotImplementedException();
        }

        // 区切り文字
        private readonly string SEPALATOR = "\t";

        // 難易度毎の情報までの要素数(楽曲情報用)
        private readonly int DATA_LENGTH = 1;

        // 難易度毎の情報までの要素数(達成率理論値用)
        private readonly int DATA_LENGTH_RIRON = 1;

        // 難易度毎の項目数
        private readonly int DATA_LENGTH_RESULT_DATA = 11;

        // 曲名
        public string name { get; set; }

        // 難易度・resultScore
        public Dictionary<string, ResultData> data { get; set; }

        // 表示・計算の対象フラグ
        public bool viewFlg { get; set; } = true;

        public override string Key { get { return name; } }

        /*
         * 楽曲情報書き込み用
         */
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(name);

            foreach (string key in data.Keys)
            {
                ret.Append(SEPALATOR);
                ret.Append(data[key].ToString());
            }

            ret.Append("\n");

            return ret.ToString();
        }

        /*
         * 達成率理論値書き込み用
         */
        public string ToStringTasseirituRiron()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(name);

            foreach (string key in data.Keys)
            {
                ret.Append(SEPALATOR);
                ret.Append(data[key].ToStringTasseirituRiron());
            }

            ret.Append("\n");

            return ret.ToString();
        }

        /*
         * コンストラクタ
         */
        public SongData()
        {
            data = new Dictionary<string, ResultData>();
        }

        /*
         * コンストラクタ(ファイル読み込み用)
         */
        public SongData(string songRow) : base(songRow)
        {
            // Javaのsuper()みたいなのは無い？
            data = new Dictionary<string, ResultData>();

            string[] songRows = songRow.Split(char.Parse(SEPALATOR));
            name = songRows[0];

            // 難易度の数を算出
            int resultCnt = (songRows.Length - 1) / DATA_LENGTH_RESULT_DATA;

            // 読み込み
            for (int i = 0; i < resultCnt; i++)
            {
                int startIndex = DATA_LENGTH + i * DATA_LENGTH_RESULT_DATA;

                // 詰め直すのは効率悪い…。
                string[] resultParam = new string[]{
                    songRows[startIndex],
                    songRows[startIndex + 1],
                    songRows[startIndex + 2],
                    songRows[startIndex + 3],
                    songRows[startIndex + 4],
                    songRows[startIndex + 5],
                    songRows[startIndex + 6],
                    songRows[startIndex + 7],
                    songRows[startIndex + 8],
                    songRows[startIndex + 9],
                    songRows[startIndex + 10],
                };

                // 難易度クラス生成
                ResultData result = new ResultData(resultParam);

                data.Add(result.diff, result);
            }
        }

        /*
         * 達成率理論値の読み込み用
         */
        public void songDataRiron(string songRow)
        {
            // Javaのsuper()みたいなのは無い？
            data = new Dictionary<string, ResultData>(); ;

            string[] songRows = songRow.Split(char.Parse(SEPALATOR));
            name = songRows[0];

            // 難易度の数を算出
            int resultCnt = (songRows.Length - 1) / 2;

            // 読み込み
            for (int i = 0; i < resultCnt; i++)
            {
                int startIndex = DATA_LENGTH_RIRON + i * 2;

                // 詰め直すのは効率悪い…。
                string[] resultParam = new string[]{
                    songRows[startIndex],
                    songRows[startIndex + 1],
                };

                // 難易度クラス生成
                ResultData result = new ResultData();
                result.resultDataRiron(resultParam);

                data.Add(result.diff, result);
            }
        }

        public override void Load()
        {
            ;
        }
    }
}
