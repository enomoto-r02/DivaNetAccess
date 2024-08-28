using DivaNetAccess.src.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DivaNetAccess.src.Logic
{
    public partial class SearchPlayRecordWindow : BaseSearchWindow
    {
        // 検索条件
        readonly SearchPlayRecordDetail searchPlayRecordDetail;

        public SearchPlayRecordWindow(Dictionary<string, SongData> Songs)
        {
            InitializeComponent();

            searchPlayRecordDetail = new SearchPlayRecordDetail();
            radioOptionNone.Checked = true;
        }

        /*
         * 検索条件取得
         */
        public string getSearchStr()
        {
            return searchPlayRecordDetail.getSearchStrAll();
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
            }

            // パネルはどうやって設定すれば…。 とりあえず力技
            radioOptionNone.Checked = true;
        }

        /*
         * 閉じるボタン
         */
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /*
         * ウインドウクローズ処理
         */
        private void searchPlayRecordWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 検索条件を設定

            // 難易度
            CheckBox[] chks = new CheckBox[] { chkDiffEa, chkDiffNo, chkDiffHa, chkDiffEx, chkDiffExExt };
            searchPlayRecordDetail.setDiffSearchStr(chks);

            // ★
            searchPlayRecordDetail.setStarSearchStr(txtStar);

            // CLEAR
            chks = new CheckBox[] {
                chkClearN, chkClearC, chkClearG, chkClearE, chkClearP
            };
            searchPlayRecordDetail.setClearSearchStr(chks);

            // TRIAL
            chks = new CheckBox[] {
                chkTrialN, chkTrialC, chkTrialG, chkTrialE, chkTrialP
            };
            searchPlayRecordDetail.setTrialSearchStr(chks);

            // 曲名
            searchPlayRecordDetail.setSongNameSearchStr(txtSongName);

            // 日時
            searchPlayRecordDetail.setDateSearchStr(txtDateBefore, txtDateAfter);

            // 場所
            searchPlayRecordDetail.setPlaceSearchStr(txtPlace);

            // 達成率
            searchPlayRecordDetail.setTasseirituSearchStr(txtTasseirituOver, txtTasseirituUnder);

            // 達成率_更新
            searchPlayRecordDetail.setTasseirituNewRecordSearchStr(chkTasseirituNewRecord);

            // スコア
            searchPlayRecordDetail.setScoreSearchStr(txtScoreOver, txtScoreUnder);

            // スコア_更新
            searchPlayRecordDetail.setScoreNewRecordSearchStr(chkScoreNewRecord);

            // COOL率
            searchPlayRecordDetail.setCoolPSearchStr(txtCoolPOver, txtCoolPUnder);

            // FINE率
            searchPlayRecordDetail.setFinePSearchStr(txtFinePOver, txtFinePUnder);

            // SAFE率
            searchPlayRecordDetail.setSafePSearchStr(txtSafePOver, txtSafePUnder);

            // SAD率
            searchPlayRecordDetail.setSadPSearchStr(txtSadPOver, txtSadPUnder);

            // WORST/WRONG率
            searchPlayRecordDetail.setWorstPSearchStr(txtWorstPOver, txtWorstPUnder);

            // 同時押し/HOLD率
            searchPlayRecordDetail.setHoldSearchStr(txtHoldOver, txtHoldUnder);

            // オプション
            searchPlayRecordDetail.setOptionSearchStr(pnlOption);

            // メモ
            searchPlayRecordDetail.setMemoSearchStr(txtMemo);

            // TODO
            //searchPlayRecordDetail.setViewFlgSearchStr(txt);

        }
    }
}
