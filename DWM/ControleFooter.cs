using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DWM
{
    public partial class ControleFooter : UserControl
    {
        public ControleFooter()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            InfoConx Info = new InfoConx();
            Info.ShowDialog();
        }

    }
}
