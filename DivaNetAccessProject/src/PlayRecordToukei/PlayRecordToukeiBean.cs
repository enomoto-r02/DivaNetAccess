using System.Collections.Generic;

namespace DivaNetAccess.src.PlayRecordToukei
{
    public class PlayRecordToukeiBean
    {
        // モジュール使用回数
        public Dictionary<string, int> moduleCnt { get; set; }

        // 店舗プレイ回数
        public Dictionary<string, int> placeCnt { get; set; }

        // 楽曲プレイ回数
        public Dictionary<string, int> songCnt { get; set; }

        // 難易度回数
        public Dictionary<string, int> diffCnt { get; set; }

        /*
         * コンストラクタ
         */
        public PlayRecordToukeiBean()
        {
            moduleCnt = new Dictionary<string, int>();
            placeCnt = new Dictionary<string, int>();
            songCnt = new Dictionary<string, int>();
            diffCnt = new Dictionary<string, int>();
        }
    }
}
