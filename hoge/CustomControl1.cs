using System.Windows.Forms;

namespace hoge
{
    public partial class CustomControl1 : Label
    {
        public bool Test { get; set; }

        public CustomControl1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
