using DivaNetAccess.src.util;
using System.Collections.Generic;

namespace DivaNetAccess.src.PlayRecordToukei
{
    public static class PlayRecordToukeiLogic
    {
        /*
         * プレイ履歴統計メイン処理
         */
        public static PlayRecordToukeiBean2 playRecordToukeiMain(Player player, Dictionary<string, PlayRecordEntity> records)
        {
            /*
            PlayRecordToukeiBean2 toukei2 = new PlayRecordToukeiBean2();
            toukei2 = calcCnt(records);

            return toukei2;
            */
            return calcCnt(records);
        }

        /*
         * 回数カウント処理
         */
        private static PlayRecordToukeiBean2 calcCnt(Dictionary<string, PlayRecordEntity> records)
        {
            Dictionary<string, int> moduleCnt = new Dictionary<string, int>();
            Dictionary<string, int> placeCnt = new Dictionary<string, int>();
            Dictionary<string, int> songCnt = new Dictionary<string, int>();
            Dictionary<string, int> clearCnt = new Dictionary<string, int>();
            Dictionary<string, int> diffCnt = new Dictionary<string, int>();

            foreach (string key in records.Keys)
            {
                PlayRecordEntity record = records[key];

                // モジュール使用回数
                if (moduleCnt.ContainsKey(record.module1))
                {
                    moduleCnt[record.module1] += 1;
                }
                else
                {
                    moduleCnt.Add(record.module1, 1);
                }

                // プレイ店舗数
                if (placeCnt.ContainsKey(record.place))
                {
                    placeCnt[record.place] += 1;
                }
                else
                {
                    placeCnt.Add(record.place, 1);
                }

                // 楽曲回数
                if (songCnt.ContainsKey(record.name))
                {
                    songCnt[record.name] += 1;
                }
                else
                {
                    songCnt.Add(record.name, 1);
                }

                // クリア回数
                if (clearCnt.ContainsKey(record.clear))
                {
                    clearCnt[record.clear] += 1;
                }
                else
                {
                    clearCnt.Add(record.clear, 1);
                }

                // 難易度回数
                if (diffCnt.ContainsKey(record.diff))
                {
                    diffCnt[record.diff] += 1;
                }
                else
                {
                    diffCnt.Add(record.diff, 1);
                }
            }

            // 詰め直す
            PlayRecordToukeiBean2 ret = new PlayRecordToukeiBean2();
            ret.moduleCnt = DivaNetUtil.getSortValue(moduleCnt);
            ret.placeCnt = DivaNetUtil.getSortValue(placeCnt);
            ret.songCnt = DivaNetUtil.getSortValue(songCnt);
            ret.clearCnt = DivaNetUtil.getSortValue(clearCnt);
            ret.diffCnt = DivaNetUtil.getSortValue(diffCnt);

            return ret;
        }
    }
}
