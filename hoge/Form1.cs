using DivaNetAccessLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hoge
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Dictionary<string, BaseEntity> l1 = new Dictionary<string, BaseEntity>();
            Dictionary<string, BaseEntity> l2 = new Dictionary<string, BaseEntity>();

            MemoData aaa = new MemoData("aaa","bbb");
            l1.Add(aaa.Key, aaa);
            MemoData bbb = new MemoData("aaa", "bbb");
            l2.Add(bbb.Key, bbb);
            MemoData ccc = new MemoData("aaa", "ccc");
            l2.Add(ccc.Key, ccc);

            List<BaseEntity> l3 = BaseEntityUtil.GetMergeEntityList((Dictionary<string, BaseEntity>)l1, (Dictionary<string, BaseEntity>)l2);
        }
    }
}
