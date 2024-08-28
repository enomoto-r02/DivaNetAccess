using DivaNetAccess.src.Common;
using DivaNetAccess.src.Logic;
using DivaNetAccess.src.myList;
using System;
using System.Windows.Forms;

namespace DivaNetAccess.src
{
    public partial class SearchRivalCompareWindow : BaseSearchWindow
    {
        // 検索条件
        private readonly SearchRivalCompare searchRivalCompare;

        public SearchRivalCompareWindow(MyListEntity myListEnt)
        {
            InitializeComponent();

            searchRivalCompare = new SearchRivalCompare(myListEnt);

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

        #region  検索条件取得

        /*
         * 検索条件取得
         */
        public string getSearchSongStr()
        {
            return searchRivalCompare.getSearchStrAll();
        }

        #endregion

        #region  閉じるボタン

        /*
         * 閉じるボタン
         */
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region ウインドウクローズ処理

        /*
         * ウインドウクローズ処理
         */
        private void searchRivalCompareWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 検索条件を設定

            // 難易度
            CheckBox[] chks = new CheckBox[] { chkDiffEa, chkDiffNo, chkDiffHa, chkDiffEx, chkDiffExExt };
            searchRivalCompare.diffSearchStr = CommonGridSearchManager.getChkboxSearchDiff("diff", chks);

            // ★
            searchRivalCompare.starSearchStr = CommonGridSearchManager.getTextBoxSearchStar("star", txtStar);

            // 曲名
            searchRivalCompare.nameSearchStr = CommonGridSearchManager.getNameSearchStrLike("name", txtSongName);

            // 達成率
            searchRivalCompare.tasserituSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseiritu", txtTasseirituOver, txtTasseirituUnder);

            // 達成率(ライバル)
            searchRivalCompare.tasserituRivalSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseiritu_rival", txtTasseirituRivalOver, txtTasseirituRivalUnder);

            // 達成率(差)
            searchRivalCompare.tasserituSaSearchStr = CommonGridSearchManager.getNumberSearchStr("tasseiritu_sa", txtTasseirituSaOver, txtTasseirituSaUnder);

            // スコア
            searchRivalCompare.scoreSearchStr = CommonGridSearchManager.getNumberSearchStr("score", txtScoreOver, txtScoreUnder);

            // スコア(ライバル)
            searchRivalCompare.scoreRivalSearchStr = CommonGridSearchManager.getNumberSearchStr("score_rival", txtScoreRivalOver, txtScoreRivalUnder);

            // スコア(差)
            searchRivalCompare.scoreSaSearchStr = CommonGridSearchManager.getNumberSearchStr("score_sa", txtScoreSaOver, txtScoreSaUnder);

            // 自分が未プレイの楽曲は表示しない
            searchRivalCompare.playerNotPlaySearchStr = CommonGridSearchManager.getNotPlaySearchStr("tasseiritu", chkPlayerNotPlay);

            // ライバルが未プレイの楽曲は表示しない
            searchRivalCompare.rivalNotPlaySearchStr = CommonGridSearchManager.getNotPlaySearchStr("tasseiritu_rival", chkRivalNotPlay);

            // マイリスト
            searchRivalCompare.myListSearchStr = CommonGridSearchManager.getNameSearchStrIn("name", searchRivalCompare.getMyListSongNameArray(cmbMyList.SelectedIndex));
        }

        #endregion

        #region クリアボタン

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

        #endregion

        public void setChkViewFlgSong(bool chkState)
        {
            chkAllViewFlg.Checked = chkState;
            searchRivalCompare.viewFlgStr = CommonGridSearchManager.getChkboxSearchStrForViewFlg("_viewFlg", chkAllViewFlg);
        }

        public CheckBox GetChkAllViewFlg()
        {
            return chkAllViewFlg;
        }
    }
}
