using System.Windows.Forms;

namespace DivaNetAccess.src.Common
{
    class LinkLabelSearchHeader : LinkLabel, IBaseControl
    {
        private string _SearchStr;

        public string SearchStr
        {
            set
            {
                _SearchStr = value;
            }
            get
            {
                return _SearchStr;
            }
        }

        public LinkLabelSearchHeader()
        {

        }

        public void Init()
        {
            _SearchStr = "";
            Text = "";
        }

        public void Clear()
        {
            _SearchStr = "";
            Text = "";
        }
    }
}
