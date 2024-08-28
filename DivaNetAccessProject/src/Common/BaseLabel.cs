using System.Windows.Forms;

namespace DivaNetAccess.src.Common
{
    public partial class BaseLabel : Label, IBaseControl
    {
        public BaseLabel()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            Text = "";
        }

        public void Init()
        {
            Text = "";
        }
    }
}
