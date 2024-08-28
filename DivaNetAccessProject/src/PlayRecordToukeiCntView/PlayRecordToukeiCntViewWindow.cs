using DivaNetAccess.src.Common;

namespace DivaNetAccess.src.PlayRecordToukeiCntView
{
    public partial class PlayRecordToukeiCntViewWindow : BaseSearchWindow
    {
        public PlayRecordToukeiCntViewWindow()
        {
            InitializeComponent();
        }

        public void windowView(string title, string str)
        {
            // タイトル設定
            Text = title;

            // カウンタ表示
            tboxPlayRecordToukeiCntView.Text = str;
        }
    }
}
