using DivaNetAccess.src.Common;
using System.Windows.Forms;

namespace DivaNetAccess.src
{
    public partial class PlayerCombo : ComboBox, IBaseControl
    {
        public Player Player;

        public PlayerCombo()
        {
            InitializeComponent();

            Player = new Player();
        }

        public void Init()
        {
            Player = new Player();
            this.Items.Clear();
            this.Text = "";
        }

        public void Clear()
        {
            ;
        }
    }
}
