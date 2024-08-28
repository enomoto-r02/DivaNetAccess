using System.Windows.Forms;

// 検索用共通フォーム
namespace DivaNetAccess.src.Common
{
    public partial class BaseSearchWindow : Form
    {
        public BaseSearchWindow()
        {
            InitializeComponent();
        }

        /*
         * キー押下イベント
         */
        private void BaseSearchWindow_KeyDown(object sender, KeyEventArgs e)
        {

            // Escキー押下でクローズする
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }

        }
    }
}
