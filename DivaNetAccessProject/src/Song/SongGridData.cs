using DivaNetAccess.src.util;
using System;

namespace DivaNetAccess
{
    // データグリッド表示用
    public class SongGridData
    {
        // 公開順
        public int _koukaiOrder { get; set; }

        // 曲名順
        public int _nameOrder { get; set; }

        // 難易度インデックス
        public int _diffIndex { get; set; }

        // クリアインデックス
        public int _clearIndex { get; set; }

        // トライアルインデックス
        public int _trialIndex { get; set; }

        // No
        public int no { get; set; }

        // 曲名
        public string name { get; set; }

        // 難易度
        public string diff { get; set; }

        // クリア
        public string clear { get; set; }

        // 達成率
        public int tasseiritu { get; set; }

        // 理論値
        public int tasseirituRiron { get; set; }

        // 差
        public int tasseirituRironSa { get; set; }

        // スコア
        public int score { get; set; }

        // トライアル
        public string trial { get; set; }

        // 連続パーフェクトトライアル(現在)
        public int trial_now { get; set; }

        // 連続パーフェクトトライアル(最高)
        public int trial_max { get; set; }

        // ★
        public string star { get; set; }

        // 更新日
        public DateTime? date { get; set; }

        // HISPEED
        public string hispeed { get; set; }

        // HIDDEN
        public string hidden { get; set; }

        // SuDDEN
        public string sudden { get; set; }

        // 順位
        public int rank { get; set; }

        // メモ
        public string memo { get; set; }

        // 表示フラグ
        public bool _viewFlg { get; set; }

        /*
         * コンストラクタ
         */
        public SongGridData()
        {

        }

        /*
         * 達成率取得(表示用)
         */
        public float tasseirituV()
        {
            return (float)(tasseiritu / 100f);
        }

        /*
         * 理論値取得(表示用)
         */
        public float tasseirituRironV()
        {
            return (float)(tasseirituRiron / 100f);
        }

        /*
         * 差取得(表示用)
         */
        public float tasseirituRironSaV()
        {
            return (float)(tasseirituRironSa / 100f);
        }

        /*
         * urlDataから取得
         */
        public void setUrlData(UrlData url, string songName)
        {
            if (!url.songUrl.ContainsKey(songName))
            {
                _koukaiOrder = 999;
                _nameOrder = 999;
            }
            else
            {
                _koukaiOrder = url.songUrl[songName]._koukaiOrde;
                _nameOrder = url.songUrl[songName]._nameOrder;
            }
        }

        /*
         * songDataから取得
         */
        public void setSongData(SongData song)
        {
            name = song.name;
        }

        /*
         * resultDataから取得
         */
        public void setResultData(ResultData result)
        {
            _diffIndex = WebUtil.getDiffIndex(result.diff);
            _clearIndex = WebUtil.getClearIndexChar(result.clear);
            _trialIndex = WebUtil.getClearIndexChar(result.trial);
            diff = result.diff;
            clear = result.clear;
            score = result.score;
            trial = result.trial;
            trial_now = result.trial_now;
            trial_max = result.trial_max;
            trial = result.trial;
            star = result.star;
            tasseiritu = result.tasseiritu;
            hispeed = result.hispeed;
            hidden = result.hidden;
            sudden = result.sudden;

            // 達成率理論値 - 達成率
            tasseirituRironSa = result.tasseiritu - tasseiritu;
        }

        /*
         * rankingDataから取得
         */
        public void setRankingData(RankingData ranking)
        {
            date = ranking.date;
            rank = ranking.rank;
        }

        /*
         * memoDataから取得
         */
        public void setMemoData(MemoData memo)
        {
            this.memo = memo.memo;
        }

        /*
         * wikiDataから取得
         */
        public void setWikiData(ResultData result)
        {
            tasseirituRiron = result.tasseiritu;

            // 達成率理論値 - 達成率(上書き)
            tasseirituRironSa = result.tasseiritu - tasseiritu;
        }
    }
}
