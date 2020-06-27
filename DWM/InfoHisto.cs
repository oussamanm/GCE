using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DWM
{
    public partial class InfoHisto : DevExpress.XtraEditors.XtraForm
    {
        string StrMsgEn;
        string StrMsgNo;
        string StrMsgAn;
        Boolean TypeOpr;

        public InfoHisto()
        {
            InitializeComponent();
        }
        public InfoHisto(string MsgEn, string MsgNo)
        {
            TypeOpr = false;
            StrMsgEn = MsgEn;
            StrMsgNo = MsgNo;
            InitializeComponent();
        }
        public InfoHisto(string MsgEn, string MsgAn, string MsgNo)
        {
            TypeOpr = true;
            StrMsgEn = MsgEn;
            StrMsgAn = MsgAn;
            StrMsgNo = MsgNo;
            InitializeComponent();
        }

        private void InfoHisto_Load(object sender, EventArgs e)
        {
            if (TypeOpr == true)
            {
                label1.Visible = true;
                memoEdit2.Visible = true;

                label.Text = StrMsgEn;
                memoEdit1.Text = StrMsgAn;
                memoEdit2.Text = StrMsgNo;

            }
            else
            {
                label1.Visible = false;
                memoEdit2.Visible = false;

                label.Text = StrMsgEn;
                memoEdit1.Text = StrMsgNo;
            }
        }
    }
}