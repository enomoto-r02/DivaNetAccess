using System.Collections.Generic;

namespace DivaNetAccess.src.PlayRecordToukei
{
    // 値ソート順保持用Bean
    public class PlayRecordToukeiBean2
    {
        // モジュール使用回数
        public List<SortValue> moduleCnt { get; set; }

        // 店舗プレイ回数
        public List<SortValue> placeCnt { get; set; }

        // 楽曲プレイ回数
        public List<SortValue> songCnt { get; set; }

        // クリア回数
        public List<SortValue> clearCnt { get; set; }

        // 難易度回数
        public List<SortValue> diffCnt { get; set; }

        /*
         * コンストラクタ
         */
        public PlayRecordToukeiBean2()
        {
            moduleCnt = new List<SortValue>();
            placeCnt = new List<SortValue>();
            songCnt = new List<SortValue>();
            clearCnt = new List<SortValue>();
            diffCnt = new List<SortValue>();
        }
    }
}
