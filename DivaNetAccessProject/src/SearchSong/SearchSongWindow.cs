using DivaNetAccess.src.Common;
using DivaNetAccess.src.Logic;
using DivaNetAccess.src.myList;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DivaNetAccess.src.searchSong
{
    public partial class SearchSongWindow : BaseSearchWindow
    {
        // 検索条件
        private readonly SearchSongDetail searchDetail;

        public SearchSongWindow(bool isPlayer, MyListEntity myListEnt)
        {
            InitializeComponent();

            searchDetail = new SearchSongDetail(myListEnt);

            // ライバル楽曲検索に不要なものは表示しない
            if (isPlayer == false)
            {
                // ラベルと項目をパネルにした方がいいのでは。
                txtMemo.Visible = false;
                labMemo.Visible = false;
                Height -= txtMemo.Height;
                btnClose.Location = new Point(btnClose.Location.X, btnClose.Location.Y - txtMemo.Height);
                btnClear.Location = new Point(btnClear.Location.X, btnClear.Location.Y - txtMemo.Height);
            }

            // マイリストプルダウン設定
            cmbMyList.Items.Add("");

            if (myListEnt != null)
            {
                foreach (myListData myList in myListEnt.myLists)
                {
                    string viewStr = string.Format("マイリスト{0}: {1}", myList.listNo, myList.listName);

                    // 筐体使用ありか
                    if (myList.useKyotaiNo > -1)
                    {
                        viewStr += " (*)";
                    }

                    if (cmbMyList.Items.IndexOf(viewStr) == -1)
                    {
                        cmbMyList.Items.Add(viewStr);
                    }
                }
            }
        }

        /*
         * 検索条件取得
         */
        public string getSearchSongStr()
        {
            searchDetail.viewFlgStr = CommonGridSearchManager.getChkboxSearchStrForViewFlg("_viewFlg", chkAllViewFlg);
            return searchDetail.getSearchStrAll();
        }

        /*
         * 検索条件取得＠楽曲情報のフラグ文字列のみ
         *   検索ウインドウを使わずに呼ばれる場合の対応
         */
        public string getViewFlgStr()
        {
            searchDetail.viewFlgStr = CommonGridSearchManager.getChkboxSearchStrForViewFlg("_viewFlg", chkAllViewFlg);
            return searchDetail.getSearchStrAll();
        }

        /*
         * ウインドウクローズ処理
         */
        private void searchSongWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 検索条件を設定

            // 難易度
            CheckBox[] chks = new CheckBox[] { chkDiffEa, chkDiffNo, chkDiffHa, chkDiffEx, chkDiffExExt };
            searchDetail.diffSearchStr = CommonGridSearchManager.getChkboxSearchDiff("diff", chks);

            // ★
            searchDetail.starSearchStr = CommonGridSearchManager.getTextBoxSearchStar("star", txtStar);

            // 曲名
            searchDetail.nameSearchStr = CommonGridSearchManager.getNameSearchStrLike("name", txtSongName);

            // CLEAR
            chks = new CheckBox[] { chkClearC, chkClearG, chkClearE, chkClearP, chkClearN };
            searchDetail.clearSearchStr = CommonGridSearchManager.getChkboxSearchStr("clear", chks);

            // TRIAL
            chks = new CheckBox[] { chkTrialC, chkTrialG, chkTrialE, chkTrialP, chkTrialN };
            searchDetail.trialSearchStr = CommonGridSearchManager.getChkboxSearchStr("trial", chks);

            // HISPEED
            chks = new CheckBox[] { chkHisOn, chkHisOff, };
            searchDetail.hispeedSearchStr = CommonGridSearchManager.getChkboxSearchStr("hispeed", chks);

            // HIDDEN
            chks = new CheckBox[] { chkHidOn, chkHidOff, };
            searchDetail.hiddenSearchStr = CommonGridSearchManager.getChkboxSearchStr("hidden", chks);

            // SUDDEN
            chks = new CheckBox[] { chkSudOn, chkSudOff, };
            searchDetail.suddenSearchStr = CommonGridSearchManager.getChkboxSearchStr("sudden", chks);

            // 達成率
            searchDetail.tasserituSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseiritu", txtTasseirituOver, txtTasseirituUnder);

            // 理論値
            searchDetail.rironSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseirituRiron", txtRironOver, txtRironUnder);

            // 差
            searchDetail.saSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseirituRironSa", txtSaOver, txtSaUnder);

            // スコア
            searchDetail.scoreSearchStr = CommonGridSearchManager.getNumberSearchStr("score", txtScoreOver, txtScoreUnder);

            // 連続パーフェクトトライアル_現在
            searchDetail.trialNowSearchStr = CommonGridSearchManager.getNumberSearchStr("trialNow", txtTrialNowOver, txtTrialNowUnder);

            // 連続パーフェクトトライアル_最高
            searchDetail.trialMaxSearchStr = CommonGridSearchManager.getNumberSearchStr("trialMax", txtTrialMaxOver, txtTrialMaxUnder);

            // 更新日
            searchDetail.dateSearchStr = CommonGridSearchManager.getDateSearchStr("date", txtDateBefore, txtDateAfter);

            // 順位
            searchDetail.rankSearchStr = CommonGridSearchManager.getNumberSearchStr("rank", txtRankOver, txtRankUnder);

            // メモ
            searchDetail.memoSearchStr = CommonGridSearchManager.getNameSearchStrLike("memo", txtMemo);

            // マイリスト
            searchDetail.myListSearchStr = CommonGridSearchManager.getNameSearchStrIn("name", searchDetail.getMyListSongNameArray(cmbMyList.SelectedIndex));

            // 
            searchDetail.viewFlgStr = CommonGridSearchManager.getChkboxSearchStrForViewFlg("_viewFlg", chkAllViewFlg);
        }

        /*
         * クリアボタン
         */
        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                // チェックボックス
                if (control.GetType() == typeof(CheckBox))
                {
                    CheckBox c = (CheckBox)control;
                    c.Checked = false;
                }

                // テキストボックス
                if (control.GetType() == typeof(TextBox))
                {
                    TextBox t = (TextBox)control;
                    t.Text = "";
                }

                // コンボボックス
                if (control.GetType() == typeof(ComboBox))
                {
                    ComboBox c = (ComboBox)control;
                    c.SelectedIndex = 0;
                }
            }
        }

        /*
         * 閉じるボタン
         */
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void setChkViewFlgSong(bool chkState)
        {
            chkAllViewFlg.Checked = chkState;
            searchDetail.viewFlgStr = CommonGridSearchManager.getChkboxSearchStrForViewFlg("_viewFlg", chkAllViewFlg);
        }

        public CheckBox GetChkAllViewFlg()
        {
            return chkAllViewFlg;
        }
    }
}
