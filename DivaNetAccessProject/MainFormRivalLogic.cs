using DivaNetAccess.src;
using DivaNetAccess.src.Logic;
using DivaNetAccess.src.searchSong;
using DivaNetAccess.src.Song;
using DivaNetAccess.src.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DivaNetAccess
{
    // MainForm_ライバル処理部分
    partial class MainForm : Form
    {
        #region 定数定義
        #endregion

        #region 変数定義

        // 全ライバル分の情報を保持
        List<string[]> rivalComboList;

        // ライバル分の情報を保持
        private Dictionary<string, SongData> rivalSongs;
        private Dictionary<string, RankingData> rivalRankings;
        private ToukeiData rivalToukei;

        // ライバル情報_検索条件ウインドウ
        private SearchSongWindow searchRivalSongWindow;
        // ライバル比較_検索条件ウインドウ
        private SearchRivalCompareWindow searchRivalCompareWindow;

        #endregion

        /*
         * ライバル情報読み込み処理
         */
        private void readRivalDatas()
        {
#warning 後で見直す
            if (string.IsNullOrEmpty(tboxRivalCode.Text))
            {
                return;
            }

            // ライバル情報読み込み
            playerControl.ActiveRival = RivalLogic.readRivalData(tboxRivalCode.Text);

            if (playerControl.ActiveRival == null)
            {
                return;
            }

            // 楽曲情報読み込み
            rivalSongs = RivalLogic.readSongData(playerControl.ActiveRival);

            // ランクイン情報読み込み
            rivalRankings = RivalLogic.readRankingData(playerControl.ActiveRival);

            // 楽曲が配信中か確認
            SongLogic.setViewFlg(playerUrls, ref rivalSongs);

            // 統計情報算出
            rivalToukei = ToukeiUtil.toukeiMain(playerControl.ActiveRival, rivalSongs, wikis, chkAllViewFlg.Checked);
            RivalJohoTabShukei();

            // ライバル情報表示
            //viewRivalData(playerControl.ActiveRival, rivalToukei);
            viewRivalData(rivalToukei);

            // 楽曲情報をGridに追加＠達成率理論値等読み込み後に行う
            addSongDataGridRival();

            // ライバル比較表示
            RivalCompareGridLogic.addGrid(songGridRivalCompare, playerSongs, rivalSongs, playerUrls);

            // ライバル比較タブ_楽曲数表示＠呼び出し位置をしっかり決めること
            viewGridCntLabel(labSongGridCntRivalCompare, songGridRivalCompare, "楽曲：{0}曲");

            // フォームのライバルコンボボックス設定
            initRivalComboBox(playerControl.ActiveRival.playerComboView);

            // 全曲検索するか判定＠内部的に検索条件ウインドウのロジックを使っているため、検索条件ウインドウ生成後に処理
            //SongLogic.setViewFlg(playerUrls, ref playerSongs);
            CommonGridSearchManager.searchGrid(songGridRival, searchRivalSongWindow.getSearchSongStr());
            viewGridCntLabel(labSongGridCntRival, songGridRival, "楽曲：{0}曲");
        }

        /*
         * フォームのライバルコンボボックス設定
         */
        private void initRivalComboBox(string name)
        {
            // ライバル名設定
            ComboBox[] cBoxs = { cBoxRival, cBoxRival2, cBoxRival3 };
            foreach (ComboBox cBox in cBoxs)
            {
                cBox.Text = name;
            }
        }

        /*
         * ライバルリストをコンボボックスに追加
         */
        private void addCboxRival()
        {
            // ライバルリストを取得
            rivalComboList = RivalLogic.readRivalList();

            ComboBox[] cBoxsRival = new ComboBox[] { cBoxRival, cBoxRival2, cBoxRival3 };
            foreach (ComboBox cBox in cBoxsRival)
            {
                cBox.Items.Clear();
                cBox.BeginUpdate();
                foreach (string[] dirInfo in rivalComboList)
                {
                    cBox.Items.Add(dirInfo[1]);
                }
                cBox.EndUpdate();
            }
        }

        #region ライバル情報タブ

        /*
         * ライバル情報表示
         */
        private void viewRivalData(ToukeiData toukei)
        {
            // ライバルコード
            tboxRivalCode.Text = playerControl.ActiveRival.rivalCode;

            // データ取得日
            labGetDateRival.Text = playerControl.ActiveRival.getDate.ToString();

            // LEVEL
            labLevelRival.Text = string.Format("{0}.{1}", (int)rivalToukei.level, rivalToukei.levelDankai);

            // ライバルに設定されている人数
            labSetRival.Text = playerControl.ActiveRival.setRival.ToString() + " 人";

            // 気になるプレイヤーに設定されている人数
            labSetInterested.Text = playerControl.ActiveRival.setInterested.ToString() + " 人";

            // 自己PR
            labPrRival.Text = DivaNetUtil.getNewLineStrByte(playerControl.ActiveRival.pr, 28);

            // タグ
            labTagRival.Text = playerControl.ActiveRival.getTag();

            // 称号
            labRankRival.Text = playerControl.ActiveRival.rank;

            // 実績称号＠統計より
            labJissekiRankRival.Text = toukei.nowRank.name;

            // Twitterアカウント
            /*
            if (string.IsNullOrEmpty(rival.twitterProfileUrl))
            {
                linkTwitterProfileUrlRival.Text = playerControl.ActiveRival.twitterConnect;

                // リンクを無効にする
                linkTwitterProfileUrlRival.Enabled = false;
            }
            else
            {
                linkTwitterProfileUrlRival.Text = "プロフィールを表示";

                // リンクを有効にする
                linkTwitterProfileUrlRival.Enabled = true;
            }
            */

            // 楽曲数
            labSongCntRival.Text = rivalToukei.getSongCntViewAll();

            // 理論値差
            if (wikis.Count != 0)
            {
                labRironSaCntRival.Text = rivalToukei.getRironSaViewAll();
            }

            // 合計達成率
            labSumTasseirituRival.Text = rivalToukei.getSumTasseirituViewAll();

            // 平均達成率
            labAvgTasseirituRival.Text = rivalToukei.getAvgTasseirituViewAll();

            // クリア状況
            labClearCntRival.Text = rivalToukei.getClearCntViewAll();

            // トライアル状況
            labTrialCntRival.Text = rivalToukei.getTrialCntViewAll();

            // 達成率ランキング
            labTasseirituRankingRival.Text = getTasseirituRankingStr(rivalRankings);
        }

        /*
         * ライバル情報_楽曲取得ボタン
         */
        private void btnGetRivalSongJoho_Click(object sender, EventArgs e)
        {
            // チェック処理
            if (string.IsNullOrEmpty(tboxAccessCode.Text)
             || string.IsNullOrEmpty(tboxPassword.Text)
            )
            {
                MessageBox.Show(MessageConst.E_MSG_0001, MessageConst.E_MSG_ERROR_T);
                return;
            }

            if (string.IsNullOrEmpty(tboxRivalCode.Text))
            {
                MessageBox.Show(MessageConst.E_MSG_0014, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // 楽曲情報存在チェック
            if (DivaNetUtil.isSongDataFile(playerControl.ActivePlayer) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0004, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // ライバル情報取得
            if (RivalLogic.getRivalDataMain(tboxAccessCode.Text, tboxPassword.Text, tboxRivalCode.Text, playerUrls, this, playerSongs, rivalSongs) == false)
            {
                // ボタン活性
                disableObjects(true);

                return;
            }

            // 選択ライバル読み込み
            changeRivalCommon();

            // ボタン活性
            disableObjects(true);
        }

        /*
         * Twitterプロフィールリンク_クリック
         */
        private void linkTwitterProfileUrlRival_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // ブラウザでログインURLを開く
            System.Diagnostics.Process.Start(playerControl.ActiveRival.twitterProfileUrl);
        }

        /*
         * songGridDataRivalに楽曲情報やランクイン情報をマージ
         */
        private void addSongDataGridRival()
        {
            List<SongGridData> gridList = new List<SongGridData>();

            // 曲数分ループ
            foreach (string songNameKey in rivalSongs.Keys)
            {
                SongData song = rivalSongs[songNameKey];

                // 難易度分ループ
                foreach (string songDiffKey in song.data.Keys)
                {
                    ResultData data = song.data[songDiffKey];

                    SongGridData grid = new SongGridData();
                    grid.setUrlData(playerUrls, songNameKey);
                    grid.setSongData(song);
                    grid.setResultData(data);

                    // ランクイン欄取得
                    string rankingKey = data.diff + song.name;
                    if (rivalRankings.ContainsKey(rankingKey))
                    {
                        RankingData ranking = rivalRankings[rankingKey];
                        grid.setRankingData(ranking);
                    }

                    // 理論値取得
                    if (wikis.ContainsKey(songNameKey))
                    {
                        SongData wiki = wikis[songNameKey];
                        if (wiki.data.ContainsKey(songDiffKey))
                        {
                            ResultData wikiData = wiki.data[songDiffKey];
                            grid.setWikiData(wikiData);
                        }
                    }

                    grid._viewFlg = song.viewFlg;

                    // ライバル楽曲をプレイヤーが未取得の場合、追加しないスキップ
                    if (!playerUrls.songUrl.ContainsKey(songNameKey))
                    {
                        grid._viewFlg = false;
                    }

                    // リストに追加
                    gridList.Add(grid);
                }
            }

            // 行にデータを追加する
            //RivalSongGridLogic.addGrid(songGridRival, gridList);
            RivalSongGridLogic.addGrid(songGridRival, gridList);
            RivalSongGridLogic.execSort2(songGrid, (int)RivalSongGridLogic.SongGridIndex.NO);

            //先頭の行を選択する
            if (songGridRival.Rows.Count > 0)
            {
                songGridRival.Rows[0].Selected = true;
            }

            // ライバル楽曲情報タブ_楽曲数表示
            viewGridCntLabel(labSongGridCntRival, songGridRival, "楽曲：{0}曲");
        }


        /*
         * ライバル削除ボタン
         */
        private void btnRivalDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region ライバル楽曲情報タブ

        /*
         * ライバル楽曲情報_検索ボタン 
         */
        private void btnSongSearchRival_Click(object sender, EventArgs e)
        {
            // 楽曲情報なし
            if (rivalSongs == null || rivalSongs.Count == 0)
            {
                return;
            }

            // 検索条件ウインドウ表示
            searchRivalSongWindow.ShowDialog();

            // 検索する
            CommonGridSearchManager.searchGrid(songGridRival, searchRivalSongWindow.getSearchSongStr());

            // ライバル楽曲情報タブ_楽曲数表示
            viewGridCntLabel(labSongGridCntRival, songGridRival, "楽曲：{0}曲");

            // 現在配信されていない楽曲を表示するチェックボックスの反映
            if (searchRivalSongWindow.GetChkAllViewFlg().Checked != chkAllViewFlg.Checked)
            {
                chkAllViewFlg_CheckedChangedCommon(searchRivalSongWindow.GetChkAllViewFlg().Checked);
            }
        }

        #endregion



        #region ライバル情報タブ_集計表示

        /*
         * 集計表示
         */
        private void RivalJohoTabShukei()
        {
            DataTable dt = (DataTable)songGridRival.DataSource;

            bool visible = !(playerSongs == null || playerSongs.Count == 0);

            ChangeVisibleGroupBoxControls(groupSongCntRival, visible);
            ChangeVisibleGroupBoxControls(groupRironCntRival, visible);
            ChangeVisibleGroupBoxControls(groupSumTasseirituRival, visible);
            ChangeVisibleGroupBoxControls(groupAvgTasseirituRival, visible);
            ChangeVisibleGroupBoxControls(groupClearRival, visible);
            ChangeVisibleGroupBoxControls(groupTrialRival, visible);
            ChangeVisibleGroupBoxControls(groupOptionRival, visible);

            // 統計情報表示
            if (visible)
            {
                // 楽曲数
                linkLabSongCntEaRival.Text = rivalToukei.songCnt[0].ToString();
                linkLabSongCntNoRival.Text = rivalToukei.songCnt[1].ToString();
                linkLabSongCntHaRival.Text = rivalToukei.songCnt[2].ToString();
                linkLabSongCntExRival.Text = rivalToukei.songCnt[3].ToString();
                linkLabSongCntExexRival.Text = rivalToukei.songCnt[4].ToString();
                linkLabSongCntKeiRival.Text = rivalToukei.songCnt[5].ToString();

                // 合計達成率
                linkLabSongSumEaRival.Text = TasseirituView(rivalToukei.sumTasseiritu[0]);
                linkLabSongSumNoRival.Text = TasseirituView(rivalToukei.sumTasseiritu[1]);
                linkLabSongSumHaRival.Text = TasseirituView(rivalToukei.sumTasseiritu[2]);
                linkLabSongSumExRival.Text = TasseirituView(rivalToukei.sumTasseiritu[3]);
                linkLabSongSumExexRival.Text = TasseirituView(rivalToukei.sumTasseiritu[4]);
                linkLabSongSumKeiRival.Text = TasseirituView(rivalToukei.sumTasseiritu[5]);

                // 平均達成率
                linkLabSongAvgEaRival.Text = TasseirituAvgView(rivalToukei.avgTasseiritu[0]);
                linkLabSongAvgNoRival.Text = TasseirituAvgView(rivalToukei.avgTasseiritu[1]);
                linkLabSongAvgHaRival.Text = TasseirituAvgView(rivalToukei.avgTasseiritu[2]);
                linkLabSongAvgExRival.Text = TasseirituAvgView(rivalToukei.avgTasseiritu[3]);
                linkLabSongAvgExexRival.Text = TasseirituAvgView(rivalToukei.avgTasseiritu[4]);
                linkLabSongAvgKeiRival.Text = TasseirituAvgView(rivalToukei.avgTasseiritu[5]);

                // 理論値数
                linkLabSongRironEaRival.Text = rivalToukei.rironSaCnt[0].ToString();
                linkLabSongRironNoRival.Text = rivalToukei.rironSaCnt[1].ToString();
                linkLabSongRironHaRival.Text = rivalToukei.rironSaCnt[2].ToString();
                linkLabSongRironExRival.Text = rivalToukei.rironSaCnt[3].ToString();
                linkLabSongRironExexRival.Text = rivalToukei.rironSaCnt[4].ToString();
                linkLabSongRironKeiRival.Text = rivalToukei.rironSaCnt[5].ToString();

                // クリア数_N
                linkLabSongClearNEaRival.Text = rivalToukei.clearCnt[0, 0].ToString();
                linkLabSongClearNNoRival.Text = rivalToukei.clearCnt[1, 0].ToString();
                linkLabSongClearNHaRival.Text = rivalToukei.clearCnt[2, 0].ToString();
                linkLabSongClearNExRival.Text = rivalToukei.clearCnt[3, 0].ToString();
                linkLabSongClearNExexRival.Text = rivalToukei.clearCnt[4, 0].ToString();
                linkLabSongClearNKeiRival.Text = rivalToukei.clearCnt[5, 0].ToString();

                // クリア数_C
                linkLabSongClearCEaRival.Text = rivalToukei.clearCnt[0, 1].ToString();
                linkLabSongClearCNoRival.Text = rivalToukei.clearCnt[1, 1].ToString();
                linkLabSongClearCHaRival.Text = rivalToukei.clearCnt[2, 1].ToString();
                linkLabSongClearCExRival.Text = rivalToukei.clearCnt[3, 1].ToString();
                linkLabSongClearCExexRival.Text = rivalToukei.clearCnt[4, 1].ToString();
                linkLabSongClearCKeiRival.Text = rivalToukei.clearCnt[5, 1].ToString();

                // クリア数_G
                linkLabSongClearGEaRival.Text = rivalToukei.clearCnt[0, 2].ToString();
                linkLabSongClearGNoRival.Text = rivalToukei.clearCnt[1, 2].ToString();
                linkLabSongClearGHaRival.Text = rivalToukei.clearCnt[2, 2].ToString();
                linkLabSongClearGExRival.Text = rivalToukei.clearCnt[3, 2].ToString();
                linkLabSongClearGExexRival.Text = rivalToukei.clearCnt[4, 2].ToString();
                linkLabSongClearGKeiRival.Text = rivalToukei.clearCnt[5, 2].ToString();

                // クリア数_E
                linkLabSongClearEEaRival.Text = rivalToukei.clearCnt[0, 3].ToString();
                linkLabSongClearENoRival.Text = rivalToukei.clearCnt[1, 3].ToString();
                linkLabSongClearEHaRival.Text = rivalToukei.clearCnt[2, 3].ToString();
                linkLabSongClearEExRival.Text = rivalToukei.clearCnt[3, 3].ToString();
                linkLabSongClearEExexRival.Text = rivalToukei.clearCnt[4, 3].ToString();
                linkLabSongClearEKeiRival.Text = rivalToukei.clearCnt[5, 3].ToString();

                // クリア数_P
                linkLabSongClearPEaRival.Text = rivalToukei.clearCnt[0, 4].ToString();
                linkLabSongClearPNoRival.Text = rivalToukei.clearCnt[1, 4].ToString();
                linkLabSongClearPHaRival.Text = rivalToukei.clearCnt[2, 4].ToString();
                linkLabSongClearPExRival.Text = rivalToukei.clearCnt[3, 4].ToString();
                linkLabSongClearPExexRival.Text = rivalToukei.clearCnt[4, 4].ToString();
                linkLabSongClearPKeiRival.Text = rivalToukei.clearCnt[5, 4].ToString();

                // トライアル数_N
                linkLabSongTrialNEaRival.Text = rivalToukei.trialCnt[0, 0].ToString();
                linkLabSongTrialNNoRival.Text = rivalToukei.trialCnt[1, 0].ToString();
                linkLabSongTrialNHaRival.Text = rivalToukei.trialCnt[2, 0].ToString();
                linkLabSongTrialNExRival.Text = rivalToukei.trialCnt[3, 0].ToString();
                linkLabSongTrialNExexRival.Text = rivalToukei.trialCnt[4, 0].ToString();
                linkLabSongTrialNKeiRival.Text = rivalToukei.trialCnt[5, 0].ToString();

                // トライアル数_C
                linkLabSongTrialCEaRival.Text = rivalToukei.trialCnt[0, 1].ToString();
                linkLabSongTrialCNoRival.Text = rivalToukei.trialCnt[1, 1].ToString();
                linkLabSongTrialCHaRival.Text = rivalToukei.trialCnt[2, 1].ToString();
                linkLabSongTrialCExRival.Text = rivalToukei.trialCnt[3, 1].ToString();
                linkLabSongTrialCExexRival.Text = rivalToukei.trialCnt[4, 1].ToString();
                linkLabSongTrialCKeiRival.Text = rivalToukei.trialCnt[5, 1].ToString();

                // トライアル数_G
                linkLabSongTrialGEaRival.Text = rivalToukei.trialCnt[0, 2].ToString();
                linkLabSongTrialGNoRival.Text = rivalToukei.trialCnt[1, 2].ToString();
                linkLabSongTrialGHaRival.Text = rivalToukei.trialCnt[2, 2].ToString();
                linkLabSongTrialGExRival.Text = rivalToukei.trialCnt[3, 2].ToString();
                linkLabSongTrialGExexRival.Text = rivalToukei.trialCnt[4, 2].ToString();
                linkLabSongTrialGKeiRival.Text = rivalToukei.trialCnt[5, 2].ToString();

                // トライアル数_E
                linkLabSongTrialEEaRival.Text = rivalToukei.trialCnt[0, 3].ToString();
                linkLabSongTrialENoRival.Text = rivalToukei.trialCnt[1, 3].ToString();
                linkLabSongTrialEHaRival.Text = rivalToukei.trialCnt[2, 3].ToString();
                linkLabSongTrialEExRival.Text = rivalToukei.trialCnt[3, 3].ToString();
                linkLabSongTrialEExexRival.Text = rivalToukei.trialCnt[4, 3].ToString();
                linkLabSongTrialEKeiRival.Text = rivalToukei.trialCnt[5, 3].ToString();

                // トライアル数_P
                linkLabSongTrialPEaRival.Text = rivalToukei.trialCnt[0, 4].ToString();
                linkLabSongTrialPNoRival.Text = rivalToukei.trialCnt[1, 4].ToString();
                linkLabSongTrialPHaRival.Text = rivalToukei.trialCnt[2, 4].ToString();
                linkLabSongTrialPExRival.Text = rivalToukei.trialCnt[3, 4].ToString();
                linkLabSongTrialPExexRival.Text = rivalToukei.trialCnt[4, 4].ToString();
                linkLabSongTrialPKeiRival.Text = rivalToukei.trialCnt[5, 4].ToString();
            }
        }

        #endregion ライバル情報タブ_集計

        #region ライバル比較タブ

        /*
         * ライバル比較_検索ボタン
         */
        private void btnSongSearchRivalCompare_Click(object sender, EventArgs e)
        {
            // 楽曲情報なし
            if (rivalSongs == null || rivalSongs.Count == 0)
            {
                return;
            }

            // 検索条件ウインドウ表示
            searchRivalCompareWindow.ShowDialog();

            // 検索する
            CommonGridSearchManager.searchGrid(songGridRivalCompare, searchRivalCompareWindow.getSearchSongStr());

            // ライバル比較タブ_楽曲数表示
            viewGridCntLabel(labSongGridCntRivalCompare, songGridRivalCompare, "楽曲：{0}曲");

            // 現在配信されていない楽曲を表示するチェックボックスの反映
            if (searchRivalCompareWindow.GetChkAllViewFlg().Checked != chkAllViewFlg.Checked)
            {
                chkAllViewFlg_CheckedChangedCommon(searchRivalCompareWindow.GetChkAllViewFlg().Checked);
            }
        }

        #endregion

    }
}
