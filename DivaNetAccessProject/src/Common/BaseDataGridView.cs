using System.Reflection;
using System.Windows.Forms;

namespace DivaNetAccess.src.Common
{
    public class BaseDataGridView : DataGridView, IBaseControl
    {
        public BaseDataGridView()
        {
            EnableDoubleBuffering();
        }

        public void Init()
        {

        }

        public void Clear()
        {

        }

        // コントロールのDoubleBufferedプロパティをTrueにする
        // https://dobon.net/vb/dotnet/control/doublebuffered.html
        private void EnableDoubleBuffering()
        {
            GetType().InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               this,
               new object[] { true });
        }
    }
}
