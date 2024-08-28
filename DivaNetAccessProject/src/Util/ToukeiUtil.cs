using System;
using System.Collections.Generic;

namespace DivaNetAccess.src.util
{
    /*
     * 統計処理クラス
     */
    public static class ToukeiUtil
    {
        // 次のLEVELまでの達成率(139.79%)
        private const int CONST_NEXT_LEVEL = 13979;

        // 次のゲージ変動までの達成率(1.3979%)
        private const float CONST_NEXT_GAGE_ACTION = 139.79f;

        /*
         * 統計処理メイン
         */
        public static ToukeiData toukeiMain(Player player, Dictionary<string, SongData> songs, Dictionary<string, SongData> wikis, bool allViewFlg)
        {
            ToukeiData ret = new ToukeiData();

            // 曲数、クリア数、トライアル数カウント
            calcSongCnt(songs, wikis, ret, allViewFlg);

            // 称号情報計算処理
            calcRankPoint(player, ret);

            return ret;
        }

        /*
         * 楽曲情報カウント処理
         */
        private static void calcSongCnt(Dictionary<string, SongData> songs, Dictionary<string, SongData> wikis, ToukeiData toukei, bool allViewFlg)
        {
            // 1曲ずつ
            foreach (string songName in songs.Keys)
            {
                SongData song = songs[songName];

                if (!allViewFlg && !song.viewFlg)
                {
                    continue;
                }

                SongData wiki = null;
                if (wikis.ContainsKey(songName))
                {
                    wiki = wikis[songName];
                }

                int songPoint = 0;

                // 難易度ずつ
                foreach (string diffName in song.data.Keys)
                {
                    ResultData diffData = song.data[diffName];
                    ResultData rironData = null;

                    // 難易度添字取得
                    int diffIndex = WebUtil.getDiffIndex(diffName);

                    // 楽曲数
                    toukei.songCnt[diffIndex]++;

                    // クリア数
                    int clearIndex = WebUtil.getClearIndexChar(diffData.clear);
                    toukei.clearCnt[diffIndex, clearIndex]++;
                    toukei.clearCnt[ToukeiData.DIFF_STR.Length - 1, clearIndex]++;

                    // トライアル数
                    int trialIndex = WebUtil.getClearIndexChar(diffData.trial);
                    toukei.trialCnt[diffIndex, trialIndex]++;
                    toukei.trialCnt[ToukeiData.DIFF_STR.Length - 1, trialIndex]++;

                    // 理論値差数
                    if (wiki != null)
                    {
                        if (wiki.data.ContainsKey(diffName))
                        {
                            rironData = wiki.data[diffName];
                        }
                    }
                    if (rironData != null)
                    {
                        // 理論値差が正常値(Wikiから情報を取得できている)
                        if (rironData.tasseiritu != 0)
                        {
                            int sa = diffData.tasseiritu - rironData.tasseiritu;
                            if (sa >= 0)
                            {
                                toukei.rironSaCnt[diffIndex]++;
                            }
                        }
                    }

                    // ポイント
                    int point = calcSongPoint(diffIndex, clearIndex);
                    if (point > songPoint)
                    {
                        songPoint = point;
                    }

                    // 合計達成率
                    toukei.sumTasseiritu[diffIndex] += diffData.tasseiritu;
                }

                // 累計ポイントを加算
                toukei.point += songPoint;
            }

            int sumTasseirituAll = 0;
            int songCntAll = 0;
            int rironSaAll = 0;

            for (int i = 0; i < ToukeiData.DIFF_STR.Length - 1; i++)
            {
                // 曲数
                toukei.songCnt[ToukeiData.DIFF_STR.Length - 1] += toukei.songCnt[i];

                // 合計達成率
                toukei.sumTasseiritu[ToukeiData.DIFF_STR.Length - 1] += toukei.sumTasseiritu[i];


                // 平均達成率
                if (toukei.sumTasseiritu[i] != 0)       // 0除算対応
                {
                    // 表示部分を全て整数値にする
                    decimal tmp = (toukei.sumTasseiritu[i] / (decimal)toukei.songCnt[i]) * 1000;

                    // 小数部を切り捨てて設定
                    toukei.avgTasseiritu[i] = (int)tmp;
                }

                // 理論値数
                toukei.rironSaCnt[ToukeiData.DIFF_STR.Length - 1] += toukei.rironSaCnt[i];

                // 総合用に集計(総合はEX EXTを除いたものとする)
                if (i < ToukeiData.DIFF_STR.Length - 2)
                {
                    rironSaAll += toukei.rironSaCnt[i];


                    sumTasseirituAll += toukei.sumTasseiritu[i];
                    songCntAll += toukei.songCnt[i];
                    rironSaAll += toukei.rironSaCnt[i];
                }
            }

            // 合計達成率(総合)
            toukei.sumTasseiritu[ToukeiData.DIFF_STR.Length - 1] = sumTasseirituAll;

            // 平均達成率(総合)
            if (sumTasseirituAll != 0)       // 0除算対応
            {
                decimal tmp2 = ((decimal)sumTasseirituAll / songCntAll) * 1000;
                toukei.avgTasseiritu[ToukeiData.DIFF_STR.Length - 1] = (int)tmp2;
            }

            // 現在のレベル取得
            toukei.level = (toukei.sumTasseiritu[ToukeiData.DIFF_STR.Length - 1] / (float)CONST_NEXT_LEVEL) + 1;

            // 次のレベルまで取得
            toukei.nextLevel = ((int)toukei.level * CONST_NEXT_LEVEL) - toukei.sumTasseiritu[ToukeiData.DIFF_STR.Length - 1];

            // 次のゲージ変動まで取得
            int[] gageInfo = getNextGageActionTasseiritu(toukei.sumTasseiritu[ToukeiData.DIFF_STR.Length - 1]);
            toukei.levelDankai = gageInfo[0];
            toukei.nextGageAction = gageInfo[1];
        }

        /*
         * 楽曲ポイント計算処理
         */
        private static int calcSongPoint(int diffIndex, int clearIndex)
        {
            int[][] POINT_BASE = {
                new int[]{  0,  1,  4,  5,  6 },    // EASY
                new int[]{  0,  2,  8,  9, 12 },    // NORMAL
                new int[]{  0,  5, 20, 23, 30 },    // HARD
                new int[]{  0,  7, 28, 32, 42 },    // EXTREME
                new int[]{  0,  0,  0,  0,  0 }     // EX EXTREME
            };

            return POINT_BASE[diffIndex][clearIndex];
        }

        /*
         * 称号情報計算処理
         */
        private static void calcRankPoint(Player player, ToukeiData toukei)
        {
            // 称号ファイル読み込み
            List<RankData> ranks = DivaNetUtil.readRankData();

            // 称号ファイルが無ければ作成
            if (ranks == null)
            {

                ranks = DivaNetUtil.createRankData();
            }

            // 現在の称号取得
            toukei.nowRank = getNowRank(ranks, toukei.point);

            // 最上位ランクチェック
            if (toukei.nowRank == null)
            {
                //最上位称号をセット
                toukei.nowRank = ranks[ranks.Count - 1];

                // 次の称号はnullをセット
                toukei.nextRank = null;
            }
            else
            {
                // 次の称号取得
                toukei.nextRank = getNextRank(ranks, toukei.point);
            }
        }


        /*
         * 現在の称号取得処理
         */
        private static RankData getNowRank(List<RankData> ranks, int point)
        {
            for (int i = 0; i < ranks.Count; i++)
            {
                if (ranks[i].point > point)
                {
                    return ranks[i - 1];
                }
            }

            // どの称号の必須ポイントよりも高い
            return null;
        }

        /*
         * 次の称号取得処理
         */
        private static RankData getNextRank(List<RankData> ranks, int point)
        {
            for (int i = 0; i < ranks.Count; i++)
            {
                if (ranks[i].point > point)
                {
                    return ranks[i];
                }
            }

            return null;
        }

        /*
         * 次のゲージ変動取得処理
         */
        public static int[] getNextGageActionTasseiritu(int sumTasseiritu)
        {
            // 現在のレベル取得
            int level = sumTasseiritu / CONST_NEXT_LEVEL + 1;

            // 達成率超過分取得
            int overTasseiritu = sumTasseiritu - ((level - 1) * CONST_NEXT_LEVEL);

            // 現在の変動段階取得
            int dankai = (int)(overTasseiritu / CONST_NEXT_GAGE_ACTION);

            // 次の変動の必要達成率取得
            int overHendoTasseiritu = (int)Math.Ceiling((double)(dankai + 1) * CONST_NEXT_GAGE_ACTION);

            // 次の変動までの差分取得
            int nextGageActionTasseiritu = overHendoTasseiritu - overTasseiritu;

            //return nextGageActionTasseiritu;

            return new int[] { dankai, nextGageActionTasseiritu };
        }
    }
}
