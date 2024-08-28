using DivaNetAccess.src.Common;
using System;
using System.Windows.Forms;

namespace DivaNetAccess.src.searchSong
{
    public partial class SearchCollectionCardWindow : BaseSearchWindow
    {
        // 検索条件
        private readonly SearchCollectionCardDetail searchCollectionCardDetail;

        public SearchCollectionCardWindow()
        {
            InitializeComponent();

            searchCollectionCardDetail = new SearchCollectionCardDetail();
        }

        /*
         * 検索条件取得
         */
        public string getSearchStr()
        {
            return searchCollectionCardDetail.getSearchStrAll();
        }

        /*
         * ウインドウクローズ処理
         */
        private void searchSongWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 検索条件を設定

            // カード名
            searchCollectionCardDetail.setCardNameSearchStr(txtCardName);

            // 種類
            CheckBox[] chks = new CheckBox[] { chkTypeSong, chkTypeMiku, chkTypeRin, chkTypeLen, chkTypeLuka, chkTypeMeiko, chkTypeKaito, chkTypeHasei };
            searchCollectionCardDetail.setTypeSearchStr(chks);

            // 枚数
            searchCollectionCardDetail.setMaisuSearchStr(txtMaisuOver, txtMaisuUnder);

            // 初回獲得日時
            searchCollectionCardDetail.setGetDateSearchStr(txtGetDateBefore, txtGetDateAfter);
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
        }

        /*
         * 閉じるボタン
         */
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
