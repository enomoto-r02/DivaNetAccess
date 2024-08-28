using DivaNetAccess.src;
using DivaNetAccess.src.CollectionCard;
using DivaNetAccess.src.Common;
using DivaNetAccess.src.Const;
using DivaNetAccess.src.Debug;
using DivaNetAccess.src.DivaRecord;
using DivaNetAccess.src.Logic;
using DivaNetAccess.src.myList;
using DivaNetAccess.src.PlayRecordToukei;
using DivaNetAccess.src.PlayRecordToukeiCntView;
using DivaNetAccess.src.searchSong;
using DivaNetAccess.src.Song;
using DivaNetAccess.src.twitter;
using DivaNetAccess.src.util;
using DivaNetAccess.src.Util;
using DivaNetAccess.src.Wiki;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DivaNetAccess
{
    public partial class MainForm : Form, IBaseControl
    {
        #region 定数定義

        // 難易度プルダウン用列挙型
        private enum cBoxDiffIndex
        {
            ALL = 0,
            EASY,
            NORMAL,
            HARD,
            EXTREME
        }

        #endregion

        #region 変数定義

        PlayerControl playerControl;

        // １プレイヤー分の情報を保持
        private Dictionary<string, SongData> playerSongs;
        private Dictionary<string, MemoData> playerMemos;
        private Dictionary<string, RankingData> playerRankings;
        private Dictionary<string, SongData> wikis;
        private ToukeiData playerToukei;
        private Dictionary<string, SongSettingData> playerSettings;
        private UrlData playerUrls;
        private PlayRecordToukeiBean2 playerRecordsToukei;
        private Dictionary<string, PlayRecordEntity> playerRecords;
        private MyListEntity myListEnt;
        private CollectionCardEntity collectionCard;

        // 楽曲情報_検索条件ウインドウ
        private SearchSongWindow searchSongWindow;

        // プレイ履歴_検索条件ウインドウ
        private SearchPlayRecordWindow searchPlayRecordWindow;

        // プレイ履歴統計_全回数表示ウインドウ
        private PlayRecordToukeiCntViewWindow recordsToukeiCntViewWindow;

        // コレクションカード_検索条件ウインドウ
        private SearchCollectionCardWindow searchCollectionCardWindow;

        // DataGridView用DataTable
        readonly SettingCommon dgvc;

        // 全DataGridView
        readonly Dictionary<string, DataGridView> dgvs = new Dictionary<string, DataGridView>();

        #endregion

        #region 全般・共通処理

        /*
         * コンストラクタ
         */
        public MainForm()
        {
            try
            {
                InitializeComponent();

                InitControls();

                // プレイヤーコンボボックス
                List<PlayerCombo> playerCombos = new List<PlayerCombo>()
                {
                    cBoxPlayer, cBoxPlayer2, cBoxPlayer3, cBoxPlayer4, cBoxPlayer5, cBoxPlayer6, cBoxPlayer7, cBoxPlayer8
                };
                List<PlayerCombo> backupCombos = new List<PlayerCombo>()
                {
                    backupCombo,
                };
                List<PlayerCombo> rivalCombos = new List<PlayerCombo>()
                {
                    cBoxRival, cBoxRival2, cBoxRival3,
                };
                playerControl = new PlayerControl(playerCombos, backupCombos, rivalCombos);

                // version.txt読み込み
                //version = VersionLogic.readVersionData();

                // 各DataGridViewの初期設定
                SongGridLogic.initDataGridView(songGrid, cMenuStripSongGrid);
                PlayRecordGridLogic.initDataGridView(playRecordGrid, cMenuStripSongGrid);
                SongSettingGridLogic.initDataGridView(songSettingGrid, cMenuStripSongGrid);
                RivalSongGridLogic.initDataGridView(songGridRival, cMenuStripSongGrid);
                RivalCompareGridLogic.initDataGridView(songGridRivalCompare, cMenuStripSongGrid);
                CollectionCardGridLogic.initDataGridView(collectionCardGrid, cMenuStripSongGrid);

                dgvc = new SettingCommon();

                // 楽曲別設定タブ非表示
                tabMain.TabPages.Remove(tabSongSettingPage);

                // DataGridView設定反映
                dgvs.Add("songGrid", songGrid);
                dgvs.Add("playRecordGrid", playRecordGrid);
                dgvs.Add("songSettingGrid", songSettingGrid);
                dgvs.Add("songGridRival", songGridRival);
                dgvs.Add("songGridRivalCompare", songGridRivalCompare);
                dgvs.Add("collectionCardGrid", collectionCardGrid);
                dgvc.set(dgvs);

                // 初期表示
                changePlayerCommon();

                disableObjects(true);


                // 遊び心メソッド
                enjoy();
            }
            catch (Exception e)
            {
                MessageBox.Show(MessageConst.E_MSG_9000, MessageConst.E_MSG_ERROR_T);
                LogUtil.writeLog(DateTime.Now.ToString() + " " + e.Message + "\r\n" + e.StackTrace);
            }
        }

        /*
         * フォーム初期化
         */
        private void initMainForm()
        {
            // 楽曲別設定コンボボックスクリア
            ComboBox[] cBoxs = { cBoxSetModule1, cBoxSetModule2, cBoxSetButton, cBoxSetSkin };
            //FormUtil.ClearControl(cBoxs);

            // 初期表示ラベルに空白を設定
            Label[] blankLabel = {

                // プレイヤー情報タブ_プレイヤー情報
                labRivalCode, labGetDate, labLevel, labDankai, labLimit, labNextLevel, labNextGageAction,
                labNextPoint, labPoint, labRank, labJissekiRank, labTicket, labVp,

                // プレイヤー情報タブ_他
                //labSongCnt, labRironSaCnt, labClearCnt, labTrialCnt, 
                //labSumTasseiritu, labAvgTasseiritu, 
                labTasseirituRanking,

                // 楽曲情報タブ
                labSongGridCnt,

                // プレイ履歴タブ
                labPlayRecordGridCnt,

                // プレイ履歴統計タブ
                labSongRecordCnt, labModuleRecordCnt, labPlaceRecordCnt, labClearRecordCnt,
                labPlayRecordCnt, labDiffRecordCnt,

                // ライバル情報タブ_ライバル情報
                //labGetDateRival, labLevelRival, labSetRival, labSetInterested,
                //labPrRival, labTagRival, labRankRival,
                labJissekiRankRival,

                // ライバル系はMainFormRivalLogic.csで行っています

                // ライバル情報タブ_他
                //labSongCntRival, labClearCntRival, labTrialCntRival, labSumTasseirituRival, labAvgTasseirituRival, labTasseirituRankingRival,

                // ライバル楽曲情報タブ
                //labSongGridCntRival,

                // ライバル比較タブ
                //labSongGridCntRivalCompare,

                // コレクションカード情報タブ
                labCollectionCardGridCnt,

                // ライバル情報タブ_ライバル情報
                labGetDateRival, labLevelRival, labSetRival, labSetInterested,
                labPrRival, labTagRival, labRankRival, labJissekiRankRival, 

                // ライバル情報タブ_他
                //labSongCntRival, labRironSaCntRival, labClearCntRival, labTrialCntRival, 
                //labSumTasseirituRival, labAvgTasseirituRival, 
                labTasseirituRankingRival,

                // ライバル楽曲情報タブ
                labSongGridCntRival,

                // ライバル比較タブ
                labSongGridCntRivalCompare
            };

            //FormUtil.ClearControl(blankLabel);

            // リンクラベル
            LinkLabel[] linkLabels = {

                // プレイ履歴統計タブ
                linkSongRecordCnt, linkModuleRecordCnt, linkPlaceRecordCnt,

                //// ライバル情報タブ
                //linkTwitterProfileUrlRival

                // コレクションカード情報タブ

                // ライバル情報タブ
                linkTwitterProfileUrlRival
            };

            //FormUtil.ClearControl(linkLabels);

            // グループボックス内全オブジェクト
            GroupBox[] groupBoxs = {
                // プレイヤー情報統計タブ
                groupSongCnt, groupRironCnt, groupClear, groupTrial, groupSumTasseiritu, groupAvgTasseiritu, groupOption,
                // コレクションカード情報タブ
                groupBox17,
                // ライバル系タブ
                groupSongCntRival, groupRironCntRival, groupClearRival, groupTrialRival,
                groupSumTasseirituRival, groupAvgTasseirituRival, groupOptionRival,
            };

            //FormUtil.ClearControl(groupBoxs);

            /*
            foreach (GroupBox groupBox in groupBoxs)
            {
                foreach (Control c in groupBox.Controls)
                {
                    c.Visible = false;
                }
            }
            */
        }


        /*
        * プレイヤー読み込み処理
        * ActivePlayerToに設定されているプレイヤーの各ファイルを読み込む
        * isPlayerChanged：バックアップ選択時はfalse
        */
        private void readPlayer()
        {
            // 初期化
            playerUrls = null;
            playerSongs = null;
            playerRankings = null;
            playerMemos = null;
            wikis = null;
            myListEnt = null;
            //playerSettings = null;
            playerRecords = null;
            playerToukei = null;
            playerRecordsToukei = null;
            collectionCard = null;

            // 通常時
            playerControl.LoadPlayerByActivePlayerToOrRefreshActivePlayer(false);

            // URL情報読み込み
            playerUrls = DivaNetUtil.readUrlData(playerControl.ActivePlayer);

            // 楽曲情報読み込み
            playerSongs = SongLogic.readSongData(playerControl.ActivePlayer);

            // 初回取得時対策
            if (playerSongs == null)
            {
                return;
            }

            // 楽曲が配信中か確認
            SongLogic.setViewFlg(playerUrls, ref playerSongs);

            // ランクイン情報読み込み
            playerRankings = DivaNetUtil.readRankingData(playerControl.ActivePlayer);

            // メモ情報読み込み
            playerMemos = DivaNetUtil.readMemoData(playerControl.ActivePlayer);

            // 達成率理論値読み込み
            wikis = WikiLogic.readTasseirituRiron(playerControl.ActivePlayer);

            // マイリスト読み込み
            myListEnt = MyListLogic.readMyListData(playerControl.ActivePlayer);

            // 楽曲別設定読み込み
            //playerSettings = DivaNetSongSettingLogic.readSongSettingData(playerControl.ActivePlayer);

            // プレイ履歴読み込み
            playerRecords = PlayRecordLogic.readPlayRecordData(playerControl.ActivePlayer, playerUrls);

            // 統計情報読み込み
            playerToukei = ToukeiUtil.toukeiMain(playerControl.ActivePlayer, playerSongs, wikis, chkAllViewFlg.Checked);

            // プレイ履歴統計算出
            playerRecordsToukei = PlayRecordToukeiLogic.playRecordToukeiMain(playerControl.ActivePlayer, playerRecords);

            // コレクションカード読み込み
            collectionCard = CollectionCardLogic.readCollectionCard(playerControl.ActivePlayer);
        }

        /*
        * プレイヤー読み込み処理
        * ActivePlayerToに設定されているプレイヤーの各ファイルを読み込む
        * isPlayerChanged：バックアップ選択時はfalse
        */
        private void readPlayer_bk(bool isActivePlayerRead = true)
        {
            // 初期化
            playerUrls = null;
            playerSongs = null;
            playerRankings = null;
            playerMemos = null;
            wikis = null;
            myListEnt = null;
            //playerSettings = null;
            playerRecords = null;
            playerToukei = null;
            playerRecordsToukei = null;
            collectionCard = null;

            // 通常時
            if (isActivePlayerRead)
            {
                playerControl.LoadPlayerByActivePlayerToOrRefreshActivePlayer(false);
            }
            // URL取得直後
            else
            {
                playerControl.LoadPlayerByAccessCode(tboxAccessCode.Text);
            }

            // URL情報読み込み
            playerUrls = DivaNetUtil.readUrlData(playerControl.ActivePlayer);

            // 楽曲情報読み込み
            playerSongs = SongLogic.readSongData(playerControl.ActivePlayer);

            // 初回取得時対策
            if (playerSongs == null)
            {
                return;
            }

            // 楽曲が配信中か確認
            SongLogic.setViewFlg(playerUrls, ref playerSongs);

            // ランクイン情報読み込み
            playerRankings = DivaNetUtil.readRankingData(playerControl.ActivePlayer);

            // メモ情報読み込み
            playerMemos = DivaNetUtil.readMemoData(playerControl.ActivePlayer);

            // 達成率理論値読み込み
            wikis = WikiLogic.readTasseirituRiron(playerControl.ActivePlayer);

            // マイリスト読み込み
            myListEnt = MyListLogic.readMyListData(playerControl.ActivePlayer);

            // 楽曲別設定読み込み
            //playerSettings = DivaNetSongSettingLogic.readSongSettingData(playerControl.ActivePlayer);

            // プレイ履歴読み込み
            playerRecords = PlayRecordLogic.readPlayRecordData(playerControl.ActivePlayer, playerUrls);

            // 統計情報読み込み
            playerToukei = ToukeiUtil.toukeiMain(playerControl.ActivePlayer, playerSongs, wikis, chkAllViewFlg.Checked);

            // プレイ履歴統計算出
            playerRecordsToukei = PlayRecordToukeiLogic.playRecordToukeiMain(playerControl.ActivePlayer, playerRecords);

            // コレクションカード読み込み
            collectionCard = CollectionCardLogic.readCollectionCard(playerControl.ActivePlayer);
        }

        /*
         * フォーム終了時の処理
         */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Application.Exit以外の終了＠Application.Exitは現状でエラー発生時のみの終了とする
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                saveCommentFiles();

                // 各Gridの情報を保持
                Dictionary<string, DataGridView> dgvs = new Dictionary<string, DataGridView>();
                dgvs.Add("songGrid", songGrid);
                dgvs.Add("songSettingGrid", songSettingGrid);
                dgvs.Add("playRecordGrid", playRecordGrid);
                dgvs.Add("songGridRival", songGridRival);
                dgvs.Add("songGridRivalCompare", songGridRivalCompare);
                dgvs.Add("collectionCardGrid", collectionCardGrid);
                dgvc.write(dgvs);
            }
        }

        /*
         * コメント系ファイル保存
         * memoData.txt、playRecordData.txt
         */
        private void saveCommentFiles()
        {
            // 既にプレイヤーを選択している
            if (playerControl.ActivePlayer != null && playerControl.ActivePlayer.IsBase)
            {
                if (playerSongs != null)
                {
                    // メモファイル書き込み
                    DivaNetUtil.writeMemoDataPlayer(playerControl.ActivePlayer, playerMemos);
                }

                if (playerRecords != null)
                {
                    // プレイ履歴書き込み処理
                    PlayRecordLogic.writePlayRecordData(playerControl.ActivePlayer, playerRecords);
                }
            }
        }

        /*
         * プレイヤー切り替え
         */
        private void cBoxPlayer_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox target = (ComboBox)sender;

            // 項目を選択していない
            if (target.SelectedIndex == -1)
            {
                return;
            }

            playerControl.ActivePlayerTo = (Player)target.SelectedItem;

            // 選択中のプレイヤーを選択した
            if (playerControl.ActivePlayer != null)
            {
                if (playerControl.ActivePlayer.Key == playerControl.ActivePlayerTo.Key)
                {
                    playerControl.ActivePlayerTo = null;
                    return;
                }
            }

            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // プレイヤー切り替え共通処理
            changePlayerCommon();

            // ボタン活性
            disableObjects(true);
        }

        /*
         * ライバル切り替え
         */
        private void cBoxRival_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox target = (ComboBox)sender;







            // 項目を選択していない
            if (target.SelectedIndex == -1)
            {
                return;
            }

            playerControl.ActivePlayerTo = (Player)target.SelectedItem;

            // 選択中のプレイヤーを選択した
            if (playerControl.ActivePlayer != null)
            {
                if (playerControl.ActivePlayer.Key == playerControl.ActivePlayerTo.Key)
                {
                    playerControl.ActivePlayerTo = null;
                    return;
                }
            }

            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // ライバル切り替え共通処理
            changeRivalCommon();

            // ボタン活性
            disableObjects(true);
        }

        /*
         * プレイヤー切り替え共通処理
         */
        private void changePlayerCommon()
        {
            //initMainForm();
            ClearControls();

            // 起動時のみプレイヤーリストを読み込む
            if (playerControl.getPlayersCount == 0)
            {
                playerControl.LoadPlayers();
            }

            // 選択プレイヤー読み込み
            readPlayer();

            // 楽曲情報まで読み込めていたら
            // (プレイヤーを読み込めていたらにすると、初回起動時にも後続処理が走るため)
            if (playerSongs == null)
            {
                return;
            }

            // 検索条件ウインドウ生成＠マイリスト検索があるため、プレイヤー情報読み込み後に処理
            searchSongWindow = new SearchSongWindow(true, myListEnt);
            searchSongWindow.StartPosition = FormStartPosition.CenterParent;

            searchPlayRecordWindow = new SearchPlayRecordWindow(playerSongs);
            searchPlayRecordWindow.StartPosition = FormStartPosition.CenterParent;

            searchRivalSongWindow = new SearchSongWindow(false, myListEnt);
            searchRivalSongWindow.StartPosition = FormStartPosition.CenterParent;

            searchRivalCompareWindow = new SearchRivalCompareWindow(myListEnt);
            searchRivalCompareWindow.StartPosition = FormStartPosition.CenterParent;

            recordsToukeiCntViewWindow = new PlayRecordToukeiCntViewWindow();
            recordsToukeiCntViewWindow.StartPosition = FormStartPosition.CenterParent;

            searchCollectionCardWindow = new SearchCollectionCardWindow();
            searchCollectionCardWindow.StartPosition = FormStartPosition.CenterParent;

            // 全曲検索するか判定＠内部的に検索条件ウインドウのロジックを使っているため、検索条件ウインドウ生成後に処理
            CommonGridSearchManager.searchGrid(songGrid, searchSongWindow.getSearchSongStr());
            viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");

            // 楽曲情報反映
            addSongDataGrid();

#if DEBUG
            DebugCommon debug = new DebugCommon();
            debug.StopWatchStart();
#endif
            // プレイ履歴反映
            addPlayRecordDataGrid();
#if DEBUG
            debug.StopWatchEndView("addPlayRecordDataGrid");
#endif

            //// 楽曲別設定反映
            //addSongSetttingDataGrid();

            // プレイヤー情報反映
            viewPlayerData(playerControl.ActivePlayer);

            // 統計情報反映
            PlayerJohoTabShukei();

            //// 楽曲別設定コンボボックス設定
            //setSongSettingComboBox(playerUrls);

            // プレイ履歴統計表示
            viewPlayRecordToukei(playerRecordsToukei);

            // コレクションカードをGridに追加
            addCollectionCardGrid();

            // コレクションカード統計を表示する
            viewCollectionCardToukei();

            // プレイヤーコンボボックスに反映
            //initPlayerComboBox(playerControl.ActivePlayer.ComboView);

            // バックアップコンボボックス初期化
            if (playerControl.ActivePlayer.IsBase)
            {
                playerControl.LoadBackups();
            }

            // ライバル情報読み込み
            changeRivalCommon();

            if (string.IsNullOrEmpty(playerControl.getPlayerNameLabel))
            {
                Text = $"{SettingConst.WINDOW_TITLE}";
            }
            else
            {
                Text = $"{SettingConst.WINDOW_TITLE} - {playerControl.getPlayerNameLabel}";
            }
        }

        public void changeRivalCommon()
        {
            if (playerControl.ActivePlayer == null)
            {
                return;
            }

#warning initMainFormView
            //initMainFormRival();

            // 起動時のみプレイヤーリストを読み込む
            if (playerControl.getPlayersCount == 0)
            {
                playerControl.LoadPlayers();
            }



            // ライバルリストをコンボボックスに追加
            readRivalDatas();
        }

        /*
         * オブジェクト活性/非活性
         * 切替処理(データ取得時)
         */
        private void disableObjects(bool state, bool isPlayerChanged = true)
        {
            this.SuspendLayout();
            foreach (Control c in tabMain.Controls)
            {
                bool setState = state;
                c.Enabled = setState;
            }
            this.ResumeLayout();
        }

        /*
         * 楽曲数表示
         */
        private void viewGridCntLabel(Label lab, DataGridView gridView, string format)
        {
            lab.Text = string.Format(format, gridView.Rows.Count.ToString().PadLeft(4));
            lab.Visible = true;
        }

        /*
         * 達成率ランキング用文字列取得
         */
        private string getTasseirituRankingStr(Dictionary<string, RankingData> rankingData)
        {
            string viewLabelStr = "";

            // 達成率ランキング情報取得＠定数化とかどうする？
            if (rankingData.ContainsKey("HARD達成率ランキング"))
            {
                viewLabelStr += "HARD".PadRight(12) + rankingData["HARD達成率ランキング"].rank.ToString().PadLeft(5) + "位\n";
            }

            if (rankingData.ContainsKey("EXTREME達成率ランキング"))
            {
                viewLabelStr += "EXTREME".PadRight(12) + rankingData["EXTREME達成率ランキング"].rank.ToString().PadLeft(5) + "位\n";
            }

            if (rankingData.ContainsKey("全難易度達成率ランキング"))
            {
                viewLabelStr += "総合".PadRight(10) + rankingData["全難易度達成率ランキング"].rank.ToString().PadLeft(5) + "位\n";
            }

            if (rankingData.ContainsKey("EX EXTREME達成率ランキング"))
            {
                viewLabelStr += "EX EXTREME".PadRight(12) + rankingData["EX EXTREME達成率ランキング"].rank.ToString().PadLeft(5) + "位\n";
            }

            // ランクインなし
            if (string.IsNullOrEmpty(viewLabelStr))
            {
                viewLabelStr = "ランクインしていません";
            }

            return viewLabelStr;
        }

        #region DataGridView_列ヘッダ右クリック処理

        // 一時保持オブジェクト
        int tmpGridViewSelectedColumnIndex;
        DataGridView tmpGridView;

        /*
         * DataGridViewの列ヘッダ右クリック_押下直後
         */
        private void Grid_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            // 選択したDataGridViewと列番号保持
            tmpGridView = (DataGridView)sender;
            tmpGridViewSelectedColumnIndex = e.ColumnIndex;
        }

        /*
         * DataGridViewの列ヘッダ右クリック_非表示
         */
        private void ToolStripMenuItemVisibleNot_Click(object sender, EventArgs e)
        {
            // 表示している列数をカウント
            int viewColumnCnt = 0;

            foreach (DataGridViewColumn column in tmpGridView.Columns)
            {
                // 名前が"_"から始まる列以外
                if ((column.Name.StartsWith("_") == false) && (column.Visible == true))
                {
                    viewColumnCnt++;
                }
            }

            // 表示している列が2列以上
            if (1 > viewColumnCnt)
            {
                MessageBox.Show("すべての列を非表示にすることはできません。", "エラー");
            }
            // No列の非表示を禁止する
            else if (tmpGridView.Columns[tmpGridViewSelectedColumnIndex].HeaderText == "No")
            {
                MessageBox.Show("No列を非表示にすることはできません。", "エラー");
            }
            else
            {
                // 対象の列を非表示にする
                tmpGridView.Columns[tmpGridViewSelectedColumnIndex].Visible = false;
            }
        }

        /*
         * DataGridViewの列ヘッダ右クリック_すべて表示
         */
        private void ToolStripMenuItemVisibleAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in tmpGridView.Columns)
            {
                // 名前が"_"から始まる列以外
                if (column.Name.StartsWith("_") == false)
                {
                    column.Visible = true;
                }
            }
        }

        #endregion

        /*
         * あそびごころメソッド
         */
        private void enjoy()
        {
        }

        #endregion

        #region プレイヤー情報タブ

        /*
         * プレイヤー情報_URL取得ボタン
         */
        private void btnGetUrl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tboxAccessCode.Text)
             || string.IsNullOrEmpty(tboxPassword.Text)
            )
            {
                MessageBox.Show(MessageConst.E_MSG_0001, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // 取得確認メッセージ
            DialogResult result = MessageBox.Show(MessageConst.I_MSG_0003, MessageConst.I_MSG_INFO_T, MessageBoxButtons.OKCancel);
            if (result != DialogResult.OK)
            {
                return;
            }

            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            Player player = new Player(tboxAccessCode.Text, tboxPassword.Text);

            // データ取得ボタン処理呼び出し
            if (DivaNetLogic.getUrlMain(player, this) == false)
            {
                // ボタン活性
                disableObjects(true);
                return;
            }

            // 初回取得時は楽曲情報まで取得する
            if (DivaNetUtil.isSongDataFile(player) == false)
            {
                MessageBox.Show(MessageConst.I_MSG_0002, MessageConst.I_MSG_INFO_T);

                playerControl.ActivePlayerTo = player;

                // プレイヤーリスト再読み込み
                playerControl.initPlayers();

                // 取得したプレイヤーの情報を読み込む準備
                playerControl.ActivePlayerTo = player;

                changePlayerCommon();

                btnGetData_Click(sender, e);
            }
            else
            {
                changePlayerCommon();
            }

            disableObjects(true);
        }

        /*
         * 楽曲取得ボタン
         */
        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (playerControl.ActivePlayer == null)
            {
                MessageBox.Show(MessageConst.E_MSG_0015, MessageConst.E_MSG_ERROR_T);
                return;
            }

            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // データ取得ボタン処理呼び出し
            int ret = DivaNetLogic.getDataMain(playerControl.ActivePlayer, playerUrls, this, playerSongs);

            // 問題があった場合は活性にする
            if (ret == -1)
            {
                disableObjects(true);
                return;
            }

            changePlayerCommon();

            disableObjects(true);
        }

        /*
         * プレイ履歴ボタン
         */
        private void btnPlayRecord_Click(object sender, EventArgs e)
        {
            // チェック処理
            if (playerControl.ActivePlayer == null)
            {
                MessageBox.Show(MessageConst.E_MSG_0015, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // プレイ履歴取得処理呼び出し
            PlayRecordLogic.getPlayRecordMain(playerControl.ActivePlayer, playerUrls, this);

            // プレイヤー切り替え共通処理
            changePlayerCommon();

            // ボタン活性
            disableObjects(true);
        }

        /*
         * 達成率理論値ボタン
         */
        private void btnGetTasseiritu_Click(object sender, EventArgs e)
        {
            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            WikiLogic.tasseirituRironMain(this);

            // プレイヤー切り替え共通処理
            changePlayerCommon();

            // ボタン活性
            disableObjects(true);
        }

        /*
         * ブラウザでログインリンク
         */
        private void linkLoginDivaNet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // チェック処理
            if (string.IsNullOrEmpty(tboxAccessCode.Text)
             || string.IsNullOrEmpty(tboxPassword.Text)
            )
            {
                MessageBox.Show(MessageConst.E_MSG_0001, MessageConst.E_MSG_ERROR_T);
                return;
            }

            DivaNetLogic.viewBrowserDivaNet(tboxAccessCode.Text, tboxPassword.Text, this);
        }

        /*
         * DIVAレコードボタン
         */
        private void btnCheckDivaRecord_Click(object sender, EventArgs e)
        {
            // ボタン非活性
            disableObjects(false);

            // チェック処理
            if (string.IsNullOrEmpty(tboxAccessCode.Text)
             || string.IsNullOrEmpty(tboxPassword.Text)
            )
            {
                MessageBox.Show(MessageConst.E_MSG_0001, MessageConst.E_MSG_ERROR_T);

                // ボタン活性
                disableObjects(true);

                return;
            }

            // 楽曲情報存在チェック
            if (DivaNetUtil.isSongDataFile(playerControl.ActivePlayer) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0004, MessageConst.E_MSG_ERROR_T);

                // ボタン活性
                disableObjects(true);

                return;
            }

            // DIVAレコードチェック処理を呼び出す
            DivaRecordLogic.checkDivaRecordMain(tboxAccessCode.Text, tboxPassword.Text, this);

            // ボタン活性
            disableObjects(true);
        }

        /*
         * マイリストボタン押下
         */
        private void btnGetMyList_Click(object sender, EventArgs e)
        {
            // チェック処理
            if (playerControl.ActivePlayer == null)
            {
                MessageBox.Show(MessageConst.E_MSG_0015, MessageConst.E_MSG_ERROR_T);

                // ボタン活性
                disableObjects(true);

                return;
            }

            disableObjects(false);

            saveCommentFiles();

            // マイリスト取得処理を呼び出す
            MyListLogic.getMyList(playerControl.ActivePlayer, this);

            changePlayerCommon();

            // ボタン活性
            disableObjects(true);
        }

        /*
         * プレイヤー情報表示
         */
        private void viewPlayerData(Player player)
        {
            tboxAccessCode.Text = playerControl.ActivePlayer.accessCode;
            tboxPassword.Text = playerControl.ActivePlayer.password;

            // VP
            labVp.Text = playerControl.ActivePlayer.vp.ToString();

            // チケット
            labTicket.Text = playerControl.ActivePlayer.ticket.ToString() + " 枚";

            // 称号
            labRank.Text = playerControl.ActivePlayer.rank.ToString();

            // ライバルコード
            labRivalCode.Text = playerControl.ActivePlayer.rivalCode;

            // 利用期限
            labLimit.Text = playerControl.ActivePlayer.limit.ToString();

            // データ取得日
            labGetDate.Text = playerControl.ActivePlayer.getDate.ToString();

            // ポイント
            labPoint.Text = playerToukei.point.ToString();

            // LEVEL
            labLevel.Text = string.Format("{0}.{1}", (int)playerToukei.level, playerToukei.levelDankai);

            // 次のLEVELまで
            float nextLevel = (float)playerToukei.nextLevel / 100.0f;
            labNextLevel.Text = nextLevel.ToString("##0.00") + " %";

            // 次のゲージ変動まで
            float nextGageAction = (float)playerToukei.nextGageAction / 100.0f;
            labNextGageAction.Text = nextGageAction.ToString("##0.00") + " %";

            // 実績称号
            labJissekiRank.Text = playerToukei.nowRank.name;

            // 次の称号まで
            if (playerToukei.nextRank == null)
            {
                // 最上位称号
                labNextPointTitle.Text = "[次の称号まで]";
                labNextPoint.Text = "最上位の称号です";
            }
            else
            {
                // 最上位称号以外
                labNextPointTitle.Text = "[" + playerToukei.nextRank.name + "まで]";
                labNextPoint.Text = (playerToukei.nextRank.point - playerToukei.point).ToString();
            }

            // 楽曲数
            labSongCnt.Text = playerToukei.getSongCntViewAll();

            // 理論値差
            if (wikis.Count != 0)
            {
                labRironSaCnt.Text = playerToukei.getRironSaViewAll();
            }

            // 合計達成率
            labSumTasseiritu.Text = playerToukei.getSumTasseirituViewAll();

            // 平均達成率
            labAvgTasseiritu.Text = playerToukei.getAvgTasseirituViewAll();

            // クリア状況
            labClearCnt.Text = playerToukei.getClearCntViewAll();

            // トライアル状況
            labTrialCnt.Text = playerToukei.getTrialCntViewAll();

            // 達成率ランキング
            labTasseirituRanking.Text = getTasseirituRankingStr(playerRankings);
        }

        /*
         * コレクションカードボタン
         */
        private void btnGetCollectionCard_Click(object sender, EventArgs e)
        {
            if (playerControl.ActivePlayer == null)
            {
                MessageBox.Show(MessageConst.E_MSG_0015, MessageConst.E_MSG_ERROR_T);

                return;
            }

            disableObjects(false);

            saveCommentFiles();

            // コレクションカード取得処理を呼び出す
            CollectionCardLogic.getCollectionCard(tboxAccessCode.Text, tboxPassword.Text, this, collectionCard);

            changePlayerCommon();

            disableObjects(true);
        }

        /*
         * つぶやくボタン
         */
        private void btnTweetRiron_Click(object sender, EventArgs e)
        {
            if (playerControl.ActivePlayer == null)
            {
                MessageBox.Show("プレイヤーを選択してください。", "エラー");

                return;
            }

            // Twitter認証ファイルなし
            if (TwitterLogic.readTwitter() == null)
            {
                using (AuthorizeWindow authorizeWindow = new AuthorizeWindow())
                {
                    authorizeWindow.ShowDialog();
                }
            }
            else
            {
                string postMsg = playerControl.ActivePlayer.playerComboView + "の理論値数 ";
                postMsg += playerToukei.getRironSaTwitter();

                using (TweetWindow tweetWindow = new TweetWindow(postMsg))
                {
                    tweetWindow.ShowDialog();
                }
            }
        }

        #endregion

        #region プレイヤー情報タブ_集計表示

        /*
         * 集計表示
         */
        private void PlayerJohoTabShukei()
        {
            bool visible = !(playerSongs == null || playerSongs.Count == 0);

            ChangeVisibleGroupBoxControls(groupSongCnt, visible);
            ChangeVisibleGroupBoxControls(groupRironCnt, visible);
            ChangeVisibleGroupBoxControls(groupSumTasseiritu, visible);
            ChangeVisibleGroupBoxControls(groupAvgTasseiritu, visible);
            ChangeVisibleGroupBoxControls(groupClear, visible);
            ChangeVisibleGroupBoxControls(groupTrial, visible);
            ChangeVisibleGroupBoxControls(groupOption, visible);

            // 統計情報表示
            if (visible)
            {
                // 楽曲数
                linkLabSongCntEa.Text = playerToukei.songCnt[0].ToString();
                linkLabSongCntNo.Text = playerToukei.songCnt[1].ToString();
                linkLabSongCntHa.Text = playerToukei.songCnt[2].ToString();
                linkLabSongCntEx.Text = playerToukei.songCnt[3].ToString();
                linkLabSongCntExex.Text = playerToukei.songCnt[4].ToString();
                linkLabSongCntKei.Text = playerToukei.songCnt[5].ToString();

                // 合計達成率
                linkLabSongSumEa.Text = TasseirituView(playerToukei.sumTasseiritu[0]);
                linkLabSongSumNo.Text = TasseirituView(playerToukei.sumTasseiritu[1]);
                linkLabSongSumHa.Text = TasseirituView(playerToukei.sumTasseiritu[2]);
                linkLabSongSumEx.Text = TasseirituView(playerToukei.sumTasseiritu[3]);
                linkLabSongSumExex.Text = TasseirituView(playerToukei.sumTasseiritu[4]);
                linkLabSongSumKei.Text = TasseirituView(playerToukei.sumTasseiritu[5]);

                // 平均達成率
                linkLabSongAvgEa.Text = TasseirituAvgView(playerToukei.avgTasseiritu[0]);
                linkLabSongAvgNo.Text = TasseirituAvgView(playerToukei.avgTasseiritu[1]);
                linkLabSongAvgHa.Text = TasseirituAvgView(playerToukei.avgTasseiritu[2]);
                linkLabSongAvgEx.Text = TasseirituAvgView(playerToukei.avgTasseiritu[3]);
                linkLabSongAvgExex.Text = TasseirituAvgView(playerToukei.avgTasseiritu[4]);
                linkLabSongAvgKei.Text = TasseirituAvgView(playerToukei.avgTasseiritu[5]);

                // 理論値数
                linkLabSongRironEa.Text = playerToukei.rironSaCnt[0].ToString();
                linkLabSongRironNo.Text = playerToukei.rironSaCnt[1].ToString();
                linkLabSongRironHa.Text = playerToukei.rironSaCnt[2].ToString();
                linkLabSongRironEx.Text = playerToukei.rironSaCnt[3].ToString();
                linkLabSongRironExex.Text = playerToukei.rironSaCnt[4].ToString();
                linkLabSongRironKei.Text = playerToukei.rironSaCnt[5].ToString();

                // クリア数_N
                linkLabSongClearNEa.Text = playerToukei.clearCnt[0, 0].ToString();
                linkLabSongClearNNo.Text = playerToukei.clearCnt[1, 0].ToString();
                linkLabSongClearNHa.Text = playerToukei.clearCnt[2, 0].ToString();
                linkLabSongClearNEx.Text = playerToukei.clearCnt[3, 0].ToString();
                linkLabSongClearNExex.Text = playerToukei.clearCnt[4, 0].ToString();
                linkLabSongClearNKei.Text = playerToukei.clearCnt[5, 0].ToString();

                // クリア数_C
                linkLabSongClearCEa.Text = playerToukei.clearCnt[0, 1].ToString();
                linkLabSongClearCNo.Text = playerToukei.clearCnt[1, 1].ToString();
                linkLabSongClearCHa.Text = playerToukei.clearCnt[2, 1].ToString();
                linkLabSongClearCEx.Text = playerToukei.clearCnt[3, 1].ToString();
                linkLabSongClearCExex.Text = playerToukei.clearCnt[4, 1].ToString();
                linkLabSongClearCKei.Text = playerToukei.clearCnt[5, 1].ToString();

                // クリア数_G
                linkLabSongClearGEa.Text = playerToukei.clearCnt[0, 2].ToString();
                linkLabSongClearGNo.Text = playerToukei.clearCnt[1, 2].ToString();
                linkLabSongClearGHa.Text = playerToukei.clearCnt[2, 2].ToString();
                linkLabSongClearGEx.Text = playerToukei.clearCnt[3, 2].ToString();
                linkLabSongClearGExex.Text = playerToukei.clearCnt[4, 2].ToString();
                linkLabSongClearGKei.Text = playerToukei.clearCnt[5, 2].ToString();

                // クリア数_E
                linkLabSongClearEEa.Text = playerToukei.clearCnt[0, 3].ToString();
                linkLabSongClearENo.Text = playerToukei.clearCnt[1, 3].ToString();
                linkLabSongClearEHa.Text = playerToukei.clearCnt[2, 3].ToString();
                linkLabSongClearEEx.Text = playerToukei.clearCnt[3, 3].ToString();
                linkLabSongClearEExex.Text = playerToukei.clearCnt[4, 3].ToString();
                linkLabSongClearEKei.Text = playerToukei.clearCnt[5, 3].ToString();

                // クリア数_P
                linkLabSongClearPEa.Text = playerToukei.clearCnt[0, 4].ToString();
                linkLabSongClearPNo.Text = playerToukei.clearCnt[1, 4].ToString();
                linkLabSongClearPHa.Text = playerToukei.clearCnt[2, 4].ToString();
                linkLabSongClearPEx.Text = playerToukei.clearCnt[3, 4].ToString();
                linkLabSongClearPExex.Text = playerToukei.clearCnt[4, 4].ToString();
                linkLabSongClearPKei.Text = playerToukei.clearCnt[5, 4].ToString();

                // トライアル数_N
                linkLabSongTrialNEa.Text = playerToukei.trialCnt[0, 0].ToString();
                linkLabSongTrialNNo.Text = playerToukei.trialCnt[1, 0].ToString();
                linkLabSongTrialNHa.Text = playerToukei.trialCnt[2, 0].ToString();
                linkLabSongTrialNEx.Text = playerToukei.trialCnt[3, 0].ToString();
                linkLabSongTrialNExex.Text = playerToukei.trialCnt[4, 0].ToString();
                linkLabSongTrialNKei.Text = playerToukei.trialCnt[5, 0].ToString();

                // トライアル数_C
                linkLabSongTrialCEa.Text = playerToukei.trialCnt[0, 1].ToString();
                linkLabSongTrialCNo.Text = playerToukei.trialCnt[1, 1].ToString();
                linkLabSongTrialCHa.Text = playerToukei.trialCnt[2, 1].ToString();
                linkLabSongTrialCEx.Text = playerToukei.trialCnt[3, 1].ToString();
                linkLabSongTrialCExex.Text = playerToukei.trialCnt[4, 1].ToString();
                linkLabSongTrialCKei.Text = playerToukei.trialCnt[5, 1].ToString();

                // トライアル数_G
                linkLabSongTrialGEa.Text = playerToukei.trialCnt[0, 2].ToString();
                linkLabSongTrialGNo.Text = playerToukei.trialCnt[1, 2].ToString();
                linkLabSongTrialGHa.Text = playerToukei.trialCnt[2, 2].ToString();
                linkLabSongTrialGEx.Text = playerToukei.trialCnt[3, 2].ToString();
                linkLabSongTrialGExex.Text = playerToukei.trialCnt[4, 2].ToString();
                linkLabSongTrialGKei.Text = playerToukei.trialCnt[5, 2].ToString();

                // トライアル数_E
                linkLabSongTrialEEa.Text = playerToukei.trialCnt[0, 3].ToString();
                linkLabSongTrialENo.Text = playerToukei.trialCnt[1, 3].ToString();
                linkLabSongTrialEHa.Text = playerToukei.trialCnt[2, 3].ToString();
                linkLabSongTrialEEx.Text = playerToukei.trialCnt[3, 3].ToString();
                linkLabSongTrialEExex.Text = playerToukei.trialCnt[4, 3].ToString();
                linkLabSongTrialEKei.Text = playerToukei.trialCnt[5, 3].ToString();

                // トライアル数_P
                linkLabSongTrialPEa.Text = playerToukei.trialCnt[0, 4].ToString();
                linkLabSongTrialPNo.Text = playerToukei.trialCnt[1, 4].ToString();
                linkLabSongTrialPHa.Text = playerToukei.trialCnt[2, 4].ToString();
                linkLabSongTrialPEx.Text = playerToukei.trialCnt[3, 4].ToString();
                linkLabSongTrialPExex.Text = playerToukei.trialCnt[4, 4].ToString();
                linkLabSongTrialPKei.Text = playerToukei.trialCnt[5, 4].ToString();
            }
        }

        private void linkLabSong_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelSearchHeader llsh = (LinkLabelSearchHeader)sender;

            // 楽曲情報なし
            if (playerSongs == null || playerSongs.Count == 0)
            {
                return;
            }

            // 検索する
            string appendViewFlgStr = searchSongWindow.getViewFlgStr();
            if (string.IsNullOrEmpty(appendViewFlgStr))
            {
                CommonGridSearchManager.searchGrid(songGrid, llsh.SearchStr);
            }
            else
            {
                if (string.IsNullOrEmpty(llsh.SearchStr))
                {
                    CommonGridSearchManager.searchGrid(songGrid, appendViewFlgStr);
                }
                else
                {
                    CommonGridSearchManager.searchGrid(songGrid, string.Format("({0}) and {1}", llsh.SearchStr, appendViewFlgStr));
                }
            }

            // 楽曲情報タブの楽曲数を表示
            viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");

            // 楽曲情報タブをアクティブにする
            tabMain.SelectedTab = tabSongJohoPage;
        }

        #endregion プレイヤー情報タブ_集計

        #region 楽曲情報タブ

        /*
         * 楽曲情報_メモ欄変更
         */
        private void songDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // プレイヤー名が空白以外＆メモ欄以外が変更された＠起動時呼び出し回避
            if (e.ColumnIndex == (int)SongGridLogic.SongGridIndex.MEMO && playerControl.ActivePlayer != null && cBoxPlayer.Text.Equals("") == false)
            {
                // 対象の情報取得
                string diff = (string)songGrid[(int)SongGridLogic.SongGridIndex.DIFF, e.RowIndex].Value;
                string name = (string)songGrid[(int)SongGridLogic.SongGridIndex.NAME, e.RowIndex].Value;
                string key = name + "." + diff;

                if (DBNull.Value != songGrid[e.ColumnIndex, e.RowIndex].Value)
                {
                    string value = (string)songGrid[e.ColumnIndex, e.RowIndex].Value;

                    // メモにキーが存在するか
                    if (playerMemos.ContainsKey(key))
                    {
                        // メモ欄を上書き
                        playerMemos[key].memo = value;
                    }
                    else
                    {
                        // メモを新規追加
                        MemoData memo = new MemoData(name, diff, value);
                        playerMemos.Add(key, memo);
                    }
                }
                else
                {
                    // メモから削除
                    playerMemos.Remove(key);
                }
            }
        }

        /*
         * 楽曲情報_列ヘッダクリック
         */
        private void songDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView target = (DataGridView)sender;

            if (target.Rows.Count > 0)
            {
                if (target == songGrid)
                {
                    SongGridLogic.execSort(target, e);
                    viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");
                }
                else if (target == songGridRival)
                {
                    SongGridLogic.execSort(target, e);
                    viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");
                }
            }
        }

        /*
         * songGridDataに楽曲情報やランクイン情報をマージ
         */
        private void addSongDataGrid()
        {
            List<SongGridData> gridList = new List<SongGridData>();

            // 曲数分ループ
            foreach (string songNameKey in playerSongs.Keys)
            {
                if (!playerSongs.ContainsKey(songNameKey))
                {
                    continue;
                }

                SongData song = playerSongs[songNameKey];

                // 難易度分ループ
                foreach (string songDiffKey in song.data.Keys)
                {
                    // 計算対象外は表示しない
                    ResultData data = song.data[songDiffKey];

                    SongGridData grid = new SongGridData();
                    grid.setUrlData(playerUrls, songNameKey);
                    grid.setSongData(song);
                    grid.setResultData(data);

                    // ランクイン欄取得
                    string rankingKey = data.diff + song.name;
                    if (playerRankings.ContainsKey(rankingKey))
                    {
                        RankingData ranking = playerRankings[rankingKey];
                        grid.setRankingData(ranking);
                    }

                    // メモ欄取得
                    string memoKey = song.name + "." + data.diff;

                    if (playerMemos.ContainsKey(memoKey))
                    {
                        MemoData memo = playerMemos[memoKey];
                        grid.setMemoData(memo);
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

                    // 表示・計算の対象フラグ
                    grid._viewFlg = song.viewFlg;

                    // リストに追加
                    gridList.Add(grid);
                }
            }

            // 行にデータを追加する
            SongGridLogic.addGrid(songGrid, gridList);
            SongGridLogic.execSort2(songGrid, (int)SongGridLogic.SongGridIndex.NO);


            //先頭の行を選択する
            if (songGrid.Rows.Count > 0)
            {
                songGrid.Rows[0].Selected = true;
            }

            // 楽曲情報タブ_楽曲数表示
            viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");
        }

        /*
         * 楽曲情報タブ_検索ボタン 
         */
        private void btnSongSearch_Click(object sender, EventArgs e)
        {
            // 楽曲情報なし
            if (playerSongs == null || playerSongs.Count == 0)
            {
                return;
            }

            // 検索条件ウインドウ表示
            searchSongWindow.ShowDialog();

            // 検索する
            CommonGridSearchManager.searchGrid(songGrid, searchSongWindow.getSearchSongStr());

            // 楽曲情報タブ_楽曲数表示
            viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");

            // 現在配信されていない楽曲を表示するチェックボックスの反映
            if (searchSongWindow.GetChkAllViewFlg().Checked != chkAllViewFlg.Checked)
            {
                chkAllViewFlg_CheckedChangedCommon(searchSongWindow.GetChkAllViewFlg().Checked);
            }
        }

        #endregion

        #region 楽曲別設定タブ

        /*
         * 楽曲別設定ボタン
         */
        private void btnGetSongSetting_Click(object sender, EventArgs e)
        {
            // チェック処理
            if (string.IsNullOrEmpty(tboxAccessCode.Text)
             || string.IsNullOrEmpty(tboxPassword.Text)
            )
            {
                MessageBox.Show(MessageConst.E_MSG_0001, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // プレイヤー情報存在チェック
            if (DivaNetUtil.isSongDataFile(playerControl.ActivePlayer) == false)
            {
                MessageBox.Show(MessageConst.E_MSG_0004, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // データ取得ボタン処理呼び出し
            playerSettings = DivaNetSongSettingLogic.getSongSettingMain(tboxAccessCode.Text, tboxPassword.Text, this);

            // データ取得ボタン処理エラー
            if (playerSettings == null)
            {
                // ボタン活性
                disableObjects(true);
                return;
            }

            // プレイヤー切り替え共通処理
            changePlayerCommon();

            // ボタン活性
            disableObjects(true);
        }

        // 楽曲別設定をGridに追加
        private void addSongSetttingDataGrid()
        {
            if (playerSettings == null || playerSettings.Count == 0)
            {
                // 行クリア
                SongSettingGridLogic.clearGrid(songSettingGrid);

                return;
            }

            // 行にデータを追加する
            SongSettingGridLogic.addGrid(songSettingGrid, playerSettings, playerUrls);

            //先頭の行を選択する
            if (songSettingGrid.Rows.Count > 0)
            {
                songSettingGrid.Rows[0].Selected = true;

                // 選択行の内容をチェックボックスに反映する
                songSettingGrid_CellClick(null, null);
            }
        }

        /*
         * 楽曲別設定コンボボックス設定
         */
        private void setSongSettingComboBox(UrlData urls)
        {
            // 汎用変数
            ComboBox[] cBoxs;

            // モジュール１、２
            cBoxs = new ComboBox[] { cBoxSetModule1, cBoxSetModule2 };
            foreach (ComboBox cBox in cBoxs)
            {
                cBox.Items.Clear();
                cBox.BeginUpdate();
                cBox.Items.Add("未設定");
                foreach (string moduleName in urls.moduleUrl.Keys)
                {
                    ModuleUrlData module = urls.moduleUrl[moduleName];

                    // 購入済みモジュールのみ
                    if (module.isBought)
                    {
                        cBox.Items.Add(module.name);
                    }
                }
                cBox.EndUpdate();
            }

            // スキン
            ComboBox comBox;
            comBox = cBoxSetSkin;
            comBox.Items.Clear();
            comBox.BeginUpdate();

            comBox.Items.Add("未設定");
            comBox.Items.Add("使用しない");
            foreach (string skinName in urls.skinUrl.Keys)
            {
                SkinUrlData skin = urls.skinUrl[skinName];

                // 購入済みモジュールのみ
                if (skin.isBought)
                {
                    comBox.Items.Add(skin.name);
                }
            }
            comBox.EndUpdate();

            // ボタン音
            comBox = cBoxSetButton;
            comBox.Items.Clear();
            comBox.BeginUpdate();

            comBox.Items.Add("未設定");
            foreach (string buttonName in urls.buttonUrl.Keys)
            {
                comBox.Items.Add(buttonName);
            }
            comBox.EndUpdate();
        }

        /*
         * 楽曲別設定_設定ボタン
         */
        private void btnSongSettingSet_Click(object sender, EventArgs e)
        {
            int rowIndex = -1;

            // デザイン上、必ず１行しか選択されないはずなのでこれでOK
            foreach (DataGridViewRow row in songSettingGrid.SelectedRows)
            {
                rowIndex = row.Index;
            }

            // 行が選択されていない＠楽曲別設定読み込みチェックも兼ねる
            if (rowIndex < 0)
            {
                MessageBox.Show(MessageConst.E_MSG_0012, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // チェック処理
            if (string.IsNullOrEmpty(tboxAccessCode.Text)
             || string.IsNullOrEmpty(tboxPassword.Text)
            )
            {
                MessageBox.Show(MessageConst.E_MSG_0001, MessageConst.E_MSG_ERROR_T);

                // ボタン活性
                disableObjects(true);

                return;
            }

            // 設定値
            ComboBox[] cBoxs = { cBoxSetModule1, cBoxSetModule2, cBoxSetButton, cBoxSetSkin };

            // 楽曲別設定処理を呼び出す
            DivaNetSongSettingLogic.songSettingSet(tboxAccessCode.Text, tboxPassword.Text, songSettingGrid[1, rowIndex].Value.ToString(), cBoxs, playerUrls, this);

            // 楽曲別設定書き込み処理
            DivaNetSongSettingLogic.writeSongSettingData(playerControl.ActivePlayer, playerSettings);

            // ボタン活性
            disableObjects(true);
        }

        /*
         * 楽曲別設定_セルクリック時
         */
        private void songSettingGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = -1;

            foreach (DataGridViewRow row in songSettingGrid.SelectedRows)
            {
                // 必ず１行しか選択されないはずなのでこれでOK
                rowIndex = row.Index;
            }

            // ヘッダクリック
            if (rowIndex < 0)
            {
                return;
            }

            // 行の内容をコンボボックスに反映
            ComboBox[] cBoxs = new ComboBox[] { cBoxSetModule1, cBoxSetModule2, cBoxSetButton, cBoxSetSkin };
            for (int colIndex = 0; colIndex < cBoxs.Length; colIndex++)
            {
                cBoxs[colIndex].Text = songSettingGrid[colIndex + 2, rowIndex].Value.ToString();

                // 設定不可の項目("-")は非活性
                if ("-".Equals(cBoxs[colIndex].Text))
                {
                    cBoxs[colIndex].Enabled = false;
                }
                else
                {
                    cBoxs[colIndex].Enabled = true;
                }
            }

            // ボタン音＠楽曲共通時に共通ボタン音設定無効を追加するため、毎回クリア
            ComboBox comBox;
            comBox = cBoxSetButton;
            comBox.Items.Clear();
            comBox.BeginUpdate();

            comBox.Items.Add("未設定");

            // 楽曲共通のみ
            if ("楽曲共通".Equals(songSettingGrid[1, rowIndex].Value.ToString()) == false)
            {
                comBox.Items.Add("共通ボタン音設定無効");
            }

            // ボタン音追加
            foreach (string buttonName in playerUrls.buttonUrl.Keys)
            {
                comBox.Items.Add(buttonName);
            }
            comBox.EndUpdate();
        }

        #endregion

        #region プレイ履歴タブ

        /*
         * playRecordGridにプレイ履歴情報を設定
         */
        private void addPlayRecordDataGrid()
        {
            // プレイ履歴チェックボックスの初期化
            chkPlayRecordSelect.Checked = false;

            // プレイ履歴なし
            if (playerRecords == null || playerRecords.Count == 0)
            {
                // 行クリア
                PlayRecordGridLogic.clearGrid(playRecordGrid);

                return;
            }

            // 行にデータを追加する
            PlayRecordGridLogic.addGrid(playRecordGrid, playerRecords);

            // 楽曲数を表示する
            viewGridCntLabel(labPlayRecordGridCnt, playRecordGrid, "楽曲：{0}曲");
        }

        /*
         * プレイ履歴_列ヘッダクリック
         */
        private void playRecordGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (playRecordGrid.Rows.Count > 0)
            {
                PlayRecordGridLogic.execSort(playRecordGrid, sender, e);
            }
        }

        /*
         * プレイ履歴_検索ボタン
         */
        private void btnPlayRecordSearch_Click(object sender, EventArgs e)
        {
            // 楽曲情報なし
            if (playerRecords == null || playerRecords.Count == 0)
            {
                return;
            }

            // 検索条件ウインドウ表示
            searchPlayRecordWindow.ShowDialog();

            // 検索する＠削除チェックボックスを初期化する
            CommonGridSearchManager.searchGrid(playRecordGrid, searchPlayRecordWindow.getSearchStr());

            // チェックボックスの状態を設定
            chkPlayRecordSelect.Checked = false;
            foreach (DataGridViewRow row in playRecordGrid.Rows)
            {
                row.Cells["del"].Value = false;
            }

            // 楽曲数を表示する
            viewGridCntLabel(labPlayRecordGridCnt, playRecordGrid, "楽曲：{0}曲");
        }

        /*
         * プレイ履歴全選択チェックボックス
         */
        private void chkPlayRecordSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (1 > playRecordGrid.Rows.Count)
            {
                return;
            }

            // チェックボックスオブジェクト
            CheckBox chkSelect = (CheckBox)sender;

            // チェックボックスの状態を設定
            foreach (DataGridViewRow row in playRecordGrid.Rows)
            {
                row.Cells["del"].Value = chkSelect.Checked;
            }
        }

        /*
         * プレイ履歴タブ_メモ欄変更
         */
        private void playRecordGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // プレイヤー名が空白以外＆メモ欄以外が変更された＠起動時呼び出し回避
            if (e.ColumnIndex == (int)PlayRecordGridLogic.PlayRecordGridIndex.MEMO && playerControl.ActivePlayer != null && cBoxPlayer.Text.Equals("") == false)
            {
                string memo = "";

                // 対象の情報取得
                if (DBNull.Value != playRecordGrid[e.ColumnIndex, e.RowIndex].Value)
                {
                    memo = (string)playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.MEMO, e.RowIndex].Value;
                }

                // キー生成
                string date = playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.DATE, e.RowIndex].Value.ToString();
                string name = playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.NAME, e.RowIndex].Value.ToString();
                string diff = playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.DIFF, e.RowIndex].Value.ToString();
                string score = playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.SCORE, e.RowIndex].Value.ToString();

                string key = date + name + diff + score;

                // メモ欄を設定
                playerRecords[key].memo = memo;
            }
        }
        /*
         * プレイ履歴削除ボタン
         */
        private void btnPlayRecordDel_Click(object sender, EventArgs e)
        {
            if (1 > playRecordGrid.Rows.Count)
            {
                MessageBox.Show(MessageConst.E_MSG_0008, MessageConst.E_MSG_ERROR_T);
                return;
            }

            List<string> delKeys = new List<string>();

            foreach (DataGridViewRow row in playRecordGrid.Rows)
            {
                // Null回避＠初期値はDBNull
                if (DBNull.Value != row.Cells[(int)PlayRecordGridLogic.PlayRecordGridIndex.DEL_FLG].Value)
                {
                    // チェックが付いているか
                    if ((bool)(row.Cells[(int)PlayRecordGridLogic.PlayRecordGridIndex.DEL_FLG].Value))
                    {
                        // 削除用のプレイ履歴キー生成
                        string delKey =
                            row.Cells[(int)PlayRecordGridLogic.PlayRecordGridIndex.DATE].Value.ToString() +
                            row.Cells[(int)PlayRecordGridLogic.PlayRecordGridIndex.NAME].Value +
                            row.Cells[(int)PlayRecordGridLogic.PlayRecordGridIndex.DIFF].Value +
                            row.Cells[(int)PlayRecordGridLogic.PlayRecordGridIndex.SCORE].Value;
                        delKeys.Add(delKey.ToString());
                    }
                }
            }

            // メッセージ表示
            if (delKeys.Count > 0)
            {
                DialogResult dr = MessageBox.Show(MessageConst.I_MSG_0001, MessageConst.I_MSG_INFO_T, MessageBoxButtons.OKCancel);

                if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show(MessageConst.E_MSG_0009, MessageConst.E_MSG_ERROR_T);
                return;
            }

            // プレイ履歴リストから削除
            foreach (string delKey in delKeys)
            {
                if (playerRecords.ContainsKey(delKey))
                {
                    playerRecords.Remove(delKey);
                }
            }

            // プレイ履歴書き込み処理
            PlayRecordLogic.writePlayRecordData(playerControl.ActivePlayer, playerRecords);

            // プレイ履歴読み込み
            playerRecords = PlayRecordLogic.readPlayRecordData(playerControl.ActivePlayer, playerUrls);

            // プレイ履歴をGridに追加
            addPlayRecordDataGrid();
        }

        /*
         * プレイ履歴_ダブルクリック
         */
        private void playRecordGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 列ヘッダのダブルクリック対策
            if (e.RowIndex == -1)
            {
                return;
            }

            // メモ欄のダブルクリックは処理なし＠誤操作対策
            if (e.ColumnIndex == (int)PlayRecordGridLogic.PlayRecordGridIndex.MEMO)
            {
                return;
            }

            // キー生成
            string diffIndex = playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.DIFF_INDEX, e.RowIndex].Value.ToString();
            string name = playRecordGrid[(int)PlayRecordGridLogic.PlayRecordGridIndex.NAME, e.RowIndex].Value.ToString();

            // 楽曲名のエスケープ対応＠Gothicなど
            name = name.Replace("'", "''");

            // 楽曲名のランダムセレクト暫定対応
            name = name.Replace("（ランダムセレクト）", "");

            // 検索
            string searchStr = string.Format("name = '{0}' and _diffIndex = {1}", name, diffIndex);
            CommonGridSearchManager.searchGrid(songGrid, searchStr);

            // 楽曲情報タブをアクティブにする
            tabMain.SelectedTab = tabSongJohoPage;

            /*
             * 不具合：_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            songGrid.Columns["_koukaiOrder"].Visible = false;
        }

        #endregion

        #region プレイ履歴統計タブ

        /*
         * プレイ履歴統計表示
         */
        private void viewPlayRecordToukei(PlayRecordToukeiBean2 toukei)
        {
            if (toukei.songCnt.Count > 0)
            {
                // 楽曲数を表示する
                viewGridCntLabel(labPlayRecordCnt, playRecordGrid, "楽曲：{0}曲");

                // プレイ履歴数
                viewGridCntLabel(labPlayRecordGridCnt, playRecordGrid, "楽曲：{0}曲");

                // 楽曲回数
                labSongRecordCnt.Text = getPlayRecordCountString(5, toukei.songCnt);
                linkSongRecordCnt.Visible = true;

                // プレイ店舗数
                labPlaceRecordCnt.Text = getPlayRecordCountString(5, toukei.placeCnt);
                linkPlaceRecordCnt.Visible = true;

                // モジュール使用回数
                labModuleRecordCnt.Text = getPlayRecordCountString(5, toukei.moduleCnt);
                linkModuleRecordCnt.Visible = true;

                // クリア回数
                labClearRecordCnt.Text = getPlayRecordCountString(toukei.clearCnt.Count, toukei.clearCnt);

                // 難易度回数
                labDiffRecordCnt.Text = getPlayRecordCountString(toukei.diffCnt.Count, toukei.diffCnt);
            }
        }

        /*
         * プレイ履歴統計表示用文字列を取得
         */
        private string getPlayRecordCountString(int viewLength, List<SortValue> countList)
        {
            StringBuilder buf = new StringBuilder();

            int max = countList.Count > viewLength ? viewLength : countList.Count;

            for (int i = 0; i < max; i++)
            {
                string name = countList[i].name;
                int cnt = countList[i].value;

                buf.Append(string.Format("{0}回  {1}", cnt.ToString().PadLeft(4), name));
                buf.AppendLine();
            }

            return buf.ToString();
        }

        /*
         * プレイ履歴統計タブ_楽曲_すべて表示リンク
         */
        private void linkLabSongRecordCntAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            recordsToukeiCntViewWindow.windowView("楽曲", getPlayRecordCountString(playerRecordsToukei.songCnt.Count, playerRecordsToukei.songCnt));
            recordsToukeiCntViewWindow.ShowDialog();
        }

        /*
         * プレイ履歴統計タブ_モジュール_すべて表示リンク
         */
        private void linkModuleRecordCnt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            recordsToukeiCntViewWindow.windowView("モジュール", getPlayRecordCountString(playerRecordsToukei.moduleCnt.Count, playerRecordsToukei.moduleCnt));
            recordsToukeiCntViewWindow.ShowDialog();
        }

        /*
         * プレイ履歴統計タブ_プレイ店舗_すべて表示リンク
         */
        private void linkPlaceRecordCnt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            recordsToukeiCntViewWindow.windowView("プレイ店舗", getPlayRecordCountString(playerRecordsToukei.placeCnt.Count, playerRecordsToukei.placeCnt));
            recordsToukeiCntViewWindow.ShowDialog();
        }

        #endregion

        #region コレクションカードタブ

        /*
         * collectionCardGridにプレイ履歴情報を設定
         */
        private void addCollectionCardGrid()
        {
            // コレクションカードなし
            if (collectionCard == null || collectionCard.collectionCards.Count == 0)
            {
                // 行クリア
                CollectionCardGridLogic.clearGrid(collectionCardGrid);

                return;
            }

            // 行にデータを追加する
            CollectionCardGridLogic.addGrid(collectionCardGrid, collectionCard);
        }

        /*
         * コレクションカード_列ヘッダクリック
         */
        private void collectionCardGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (collectionCardGrid.Rows.Count > 0)
            {
                CollectionCardGridLogic.execSort(collectionCardGrid, sender, e);
            }
        }

        /*
         * コレクションカード_検索ボタン
         * 
         */
        private void btnCollectionCardSearch_Click(object sender, EventArgs e)
        {
            // コレクションカード情報なし
            if (collectionCard == null || collectionCard.collectionCards.Count == 0)
            {
                return;
            }

            // 検索条件ウインドウ表示
            searchCollectionCardWindow.ShowDialog();

            // 検索する
            CollectionCardGridLogic.searchGrid(collectionCardGrid, searchCollectionCardWindow.getSearchStr());

            // コレクションカード情報タブ_コレクションカード数表示
            viewGridCntLabel(labCollectionCardGridCnt, collectionCardGrid, "カード：{0}種類");
        }

        #endregion

        #region コレクションカード統計タブ

        /*
         * コレクションカード統計初期化
         */
        private void initCollectionCardToukei()
        {
            List<LinkLabelSearchHeader> cList = new List<LinkLabelSearchHeader>() {
                linkLabelCollectionCardSong,    linkLabelCollectionCardSongAll,
                linkLabelCollectionCardModule,  linkLabelCollectionCardModuleAll,
                linkLabelCollectionCardMiku,    linkLabelCollectionCardMikuAll,
                linkLabelCollectionCardRin,     linkLabelCollectionCardRinAll,
                linkLabelCollectionCardLen,     linkLabelCollectionCardLenAll,
                linkLabelCollectionCardLuka,    linkLabelCollectionCardLukaAll,
                linkLabelCollectionCardMeiko,   linkLabelCollectionCardMeikoAll,
                linkLabelCollectionCardKaito,   linkLabelCollectionCardKaitoAll,
                linkLabelCollectionCardHasei,   linkLabelCollectionCardHaseiAll,
                linkLabelCollectionCardKei,     linkLabelCollectionCardKeiAll,
            };

            foreach (LinkLabelSearchHeader c in cList)
            {
                c.Init();
            }
        }

        /*
         * コレクションカード統計表示
         */
        private void viewCollectionCardToukei()
        {
            initCollectionCardToukei();

            // コレクションカード情報タブ_コレクションカード数表示
            viewGridCntLabel(labCollectionCardGridCnt, collectionCardGrid, "カード：{0}種類");

            DataTable dt = (DataTable)collectionCardGrid.DataSource;

            bool visible = !(collectionCard == null || collectionCard.collectionCards.Count == 0);

            // グループ全体を一括で管理しているため、linkLabelCollectionCardSongなどで集計結果0の時、表示がおかしくなる(前に選択していたプレイヤーの情報が表示される)
            ChangeVisibleGroupBoxControls(groupBox17, visible);

            // 統計情報表示
            if (visible)
            {
                // 枚数合計と総数と収集率をタイプ別に集計
                DataView dv = new DataView(dt);

                DataTable dtShukei = dv.ToTable("dtShukeiCollection", true, new string[] { "_type" });
                dtShukei.Columns.Add("SumAll");
                dtShukei.Columns.Add("Sum");
                dtShukei.Columns.Add("Percent");

                foreach (DataRow row in dtShukei.Rows)
                {
                    row["SumAll"] = dt.Compute("count(num)", string.Format("_type = '{0}'", row["_type"]));
                    row["Sum"] = dt.Compute("count(num)", string.Format("_type = '{0}' and num > 0", row["_type"]));

                    float SumAll = float.Parse(row["SumAll"].ToString());
                    float Sum = float.Parse(row["Sum"].ToString());
                    float percent = 0.0f;
                    if (Sum > 0 && SumAll > 0)
                    {
                        percent = float.Parse(row["Sum"].ToString()) / float.Parse(row["SumAll"].ToString());
                    }
                    row["Percent"] = string.Format("{0} %", Math.Round(percent * 100));
                }

                // 集計結果を表示
                foreach (DataRow row in dtShukei.Rows)
                {
                    CollectionCard.CardType type =
                        (CollectionCard.CardType)Enum.Parse(typeof(CollectionCard.CardType), row["_type"].ToString());

                    switch (type)
                    {
                        case CollectionCard.CardType.SONG:
                            linkLabelCollectionCardSong.Text = row["Sum"].ToString();
                            linkLabelCollectionCardSongAll.Text = row["SumAll"].ToString();
                            labCollectionCardSongP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.MIKU:
                            linkLabelCollectionCardMiku.Text = row["Sum"].ToString();
                            linkLabelCollectionCardMikuAll.Text = row["SumAll"].ToString();
                            labCollectionCardMikuP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.RIN:
                            linkLabelCollectionCardRin.Text = row["Sum"].ToString();
                            linkLabelCollectionCardRinAll.Text = row["SumAll"].ToString();
                            labCollectionCardRinP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.LEN:
                            linkLabelCollectionCardLen.Text = row["Sum"].ToString();
                            linkLabelCollectionCardLenAll.Text = row["SumAll"].ToString();
                            labCollectionCardLenP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.LUKA:
                            linkLabelCollectionCardLuka.Text = row["Sum"].ToString();
                            linkLabelCollectionCardLukaAll.Text = row["SumAll"].ToString();
                            labCollectionCardLukaP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.MEIKO:
                            linkLabelCollectionCardMeiko.Text = row["Sum"].ToString();
                            linkLabelCollectionCardMeikoAll.Text = row["SumAll"].ToString();
                            labCollectionCardMeikoP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.KAITO:
                            linkLabelCollectionCardKaito.Text = row["Sum"].ToString();
                            linkLabelCollectionCardKaitoAll.Text = row["SumAll"].ToString();
                            labCollectionCardKaitoP.Text = row["Percent"].ToString();
                            break;
                        case CollectionCard.CardType.HASEI:
                            linkLabelCollectionCardHasei.Text = row["Sum"].ToString();
                            linkLabelCollectionCardHaseiAll.Text = row["SumAll"].ToString();
                            labCollectionCardHaseiP.Text = row["Percent"].ToString();
                            break;
                        default:
                            break;
                    }
                }

                // モジュールと計は力技で別途カウント
                linkLabelCollectionCardModule.Text = dt.Compute("count(num)", string.Format("_type <> '{0}' and num > 0", (int)CollectionCard.CardType.SONG)).ToString();
                linkLabelCollectionCardModuleAll.Text = dt.Compute("count(num)", string.Format("_type <> '{0}'", (int)CollectionCard.CardType.SONG)).ToString();
                float moduleP = float.Parse(linkLabelCollectionCardModule.Text) / float.Parse(linkLabelCollectionCardModuleAll.Text);
                labCollectionCardModuleP.Text = string.Format("{0} %", Math.Round(moduleP * 100));

                linkLabelCollectionCardKei.Text = dt.Compute("count(num)", string.Format("num > 0", (int)CollectionCard.CardType.SONG)).ToString();
                linkLabelCollectionCardKeiAll.Text = dt.Rows.Count.ToString();
                float keiP = float.Parse(linkLabelCollectionCardKei.Text) / float.Parse(linkLabelCollectionCardKeiAll.Text);
                labCollectionCardKeiP.Text = string.Format("{0} %", Math.Round(keiP * 100));
            }
        }

        // リンク押下イベント
        private void LinkLabelCollectionCard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelSearchHeader llsh = (LinkLabelSearchHeader)sender;

            // コレクションカード情報なし
            if (collectionCard == null || collectionCard.collectionCards.Count == 0)
            {
                return;
            }

            // 検索する
            CollectionCardGridLogic.searchGrid(collectionCardGrid, llsh.SearchStr);

            // コレクションカード情報タブ_コレクションカード数表示
            viewGridCntLabel(labCollectionCardGridCnt, collectionCardGrid, "カード：{0}種類");

            // コレクションカードタブをアクティブにする
            tabMain.SelectedTab = tabCollectionCardPage;
            collectionCardGrid.CurrentCell = collectionCardGrid[0, 0];
        }

        #endregion

        #region 共通＠後ほど、Util化します。

        public static void ChangeVisibleGroupBoxControls(GroupBox target, bool visible)
        {
            foreach (Control c in target.Controls)
            {
                c.Visible = visible;
            }
        }

        public static string TasseirituView(int target)
        {
            return ((decimal)target / 100).ToString("0.00") + "%";
        }

        public static string TasseirituAvgView(int target)
        {
            return ((decimal)target / 100000).ToString("0.00000") + "%";
        }

        #endregion

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // 再描画
            Refresh();
        }

        /*
         * 全ての楽曲を表示するチェックボックス
         */
        private void chkAllViewFlg_CheckedChanged(object sender, EventArgs e)
        {
            bool chked = ((CheckBox)sender).Checked;
            chkAllViewFlg_CheckedChangedCommon(chked);
        }

        /*
         * 全ての楽曲を表示するチェックボックス_ライバル
         */
        private void chkAllViewFlgRival_CheckedChanged(object sender, EventArgs e)
        {
            bool chked = ((CheckBox)sender).Checked;
            chkAllViewFlg_CheckedChangedCommon(chked);
        }

        // 全てのチェック状態の反映と検索処理、統計更新
        private void chkAllViewFlg_CheckedChangedCommon(bool chked)
        {
            // 楽曲検索ウインドウ
            searchSongWindow.setChkViewFlgSong(chked);
            // ライバル楽曲検索ウインドウ
            searchRivalSongWindow.setChkViewFlgSong(chked);
            // ライバル楽曲比較検索ウインドウ
            searchRivalCompareWindow.setChkViewFlgSong(chked);

            // プレイヤー情報タブ
            chkAllViewFlg.CheckedChanged -= new System.EventHandler(chkAllViewFlg_CheckedChanged);
            chkAllViewFlg.Checked = chked;
            chkAllViewFlg.CheckedChanged += new System.EventHandler(chkAllViewFlg_CheckedChanged);

            // ライバル情報
            chkAllViewFlgRival.CheckedChanged -= new System.EventHandler(chkAllViewFlgRival_CheckedChanged);
            chkAllViewFlgRival.Checked = chked;
            chkAllViewFlgRival.CheckedChanged += new System.EventHandler(chkAllViewFlgRival_CheckedChanged);

            // 検索・更新する

            // 楽曲情報タブ
            CommonGridSearchManager.searchGrid(songGrid, searchSongWindow.getSearchSongStr());
            viewGridCntLabel(labSongGridCnt, songGrid, "楽曲：{0}曲");

            // プレイヤー情報タブ
            playerToukei = ToukeiUtil.toukeiMain(playerControl.ActivePlayer, playerSongs, wikis, chked);
            PlayerJohoTabShukei();

            // プレイ履歴統計タブ
            playerRecordsToukei = PlayRecordToukeiLogic.playRecordToukeiMain(playerControl.ActivePlayer, playerRecords);
            viewPlayRecordToukei(playerRecordsToukei);

            if (playerControl.ActiveRival != null)
            {
                // ライバル楽曲タブ
                CommonGridSearchManager.searchGrid(songGridRival, searchRivalSongWindow.getSearchSongStr());
                viewGridCntLabel(labSongGridCntRival, songGridRival, "楽曲：{0}曲");

                // ライバル情報タブ
                rivalToukei = ToukeiUtil.toukeiMain(playerControl.ActiveRival, rivalSongs, wikis, chked);
                RivalJohoTabShukei();

                // ライバル比較タブ
                CommonGridSearchManager.searchGrid(songGridRivalCompare, searchRivalCompareWindow.getSearchSongStr());
                viewGridCntLabel(labSongGridCntRivalCompare, songGridRivalCompare, "楽曲：{0}曲");
            }
        }

        /*
         * バックアップを取るボタン
         */
        private void btnCreateBackup_Click(object sender, EventArgs e)
        {
            if (playerControl.ActivePlayer == null)
            {
                MessageBox.Show(MessageConst.E_MSG_0015, MessageConst.E_MSG_ERROR_T);
                return;
            }

            DialogResult result = MessageBox.Show(MessageConst.I_MSG_0004, MessageConst.I_MSG_INFO_T, MessageBoxButtons.OKCancel);
            if (result != DialogResult.OK)
            {
                return;
            }

            // ボタン非活性
            disableObjects(false);

            playerControl.CopyBackupFile(true);

            // 完了メッセージ
            MessageBox.Show(MessageConst.N_MSG_0010 + "\r\n", MessageConst.N_MSG_FINISH_T);

            playerControl.LoadBackups();

            // ボタン活性
            disableObjects(true);
        }

        // テストボタン
        private void btnTest_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void linkLabSongRival_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabelSearchHeader llsh = (LinkLabelSearchHeader)sender;

            // 楽曲情報なし
            if (rivalSongs == null || rivalSongs.Count == 0)
            {
                return;
            }

            // 検索する
            string appendViewFlgStr = searchRivalSongWindow.getViewFlgStr();
            if (string.IsNullOrEmpty(appendViewFlgStr))
            {
                CommonGridSearchManager.searchGrid(songGridRival, llsh.SearchStr);
            }
            else
            {
                if (string.IsNullOrEmpty(llsh.SearchStr))
                {
                    CommonGridSearchManager.searchGrid(songGridRival, appendViewFlgStr);
                }
                else
                {
                    CommonGridSearchManager.searchGrid(songGridRival, string.Format("({0}) and {1}", llsh.SearchStr, appendViewFlgStr));
                }
            }

            // タブをアクティブにする
            viewGridCntLabel(labSongGridCntRival, songGridRival, "楽曲：{0}曲");

            /*
             * 不具合：_koukaiOrde列の非表示が解除されてしまう。
             * 　　　　でも非表示列の_nameOrderと_diffIndexは解除されない
             * 　　　　原因不明のため、暫定対応で下の命令で再度非表示にする
             */
            songGridRival.Columns["_koukaiOrder"].Visible = false;

            // タブをアクティブにする
            tabMain.SelectedTab = tabSongJohoPageRival;

        }

        private void backupCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PlayerCombo target = (PlayerCombo)sender;

            // 項目を選択していない
            if (target.SelectedIndex == -1)
            {
                return;
            }

            playerControl.ActivePlayerTo = (Player)target.SelectedItem;

            // 選択中のプレイヤーを選択した
            if (playerControl.ActivePlayer != null)
            {
                if (playerControl.ActivePlayer.Key == playerControl.ActivePlayerTo.Key)
                {
                    playerControl.ActivePlayerTo = null;
                    return;
                }
            }

            // ボタン非活性
            disableObjects(false);

            // コメントファイル保存
            saveCommentFiles();

            // プレイヤー切り替え共通処理
            changePlayerCommon();

            // ボタン活性
            disableObjects(true, playerControl.ActivePlayer.IsBase);
        }

        public void Init()
        {
            Text = $"{SettingConst.WINDOW_TITLE}";

            // 利用期限非表示
            label6.Visible = false;
            labLimit.Visible = false;

#if DEBUG
            btnTest.Visible = true;
#endif
        }

        public void InitControls()
        {
            Init();
            _InitControls(this);
        }

        private void _InitControls(Control c)
        {
            foreach (Control cc in c.Controls)
            {
                _InitControls(cc);

                IBaseControl bc = cc as IBaseControl;
                if (bc != null)
                {
                    bc.Init();
                }
            }
        }

        public void Clear()
        {
            //Text = $"{SettingConst.WINDOW_TITLE}";
        }

        public void ClearControls()
        {
            Clear();
            _ClearControls(this);
        }

        private void _ClearControls(Control c)
        {
            foreach (Control cc in c.Controls)
            {
                _ClearControls(cc);

                IBaseControl bc = cc as IBaseControl;
                if (bc != null)
                {
                    bc.Clear();
                }
            }
        }

        /*
         * ライバル比較_ソート
         */
        private void songGridRivalCompare_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView target = (DataGridView)sender;

            if (target.Rows.Count > 0)
            {
                RivalCompareGridLogic.execSort(target, sender, e);
            }
        }
    }
}
