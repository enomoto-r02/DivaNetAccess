using System.Windows.Forms;

namespace DivaNetAccess.src.Common
{
    public partial class BaseHeaderLabel : Label, IBaseControl
    {
        public bool InitView { get; set; }

        public BaseHeaderLabel()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            Visible = InitView;
        }

        public void Init()
        {
            Visible = InitView;
#if DEBUG
            Font = new System.Drawing.Font("MS UI Gothic", 8.2F, System.Drawing.FontStyle.Bold);
#endif
        }
    }
}
