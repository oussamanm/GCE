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

        private void ControleFooter_Load(object sender, EventArgs e)
        {
            //label7.Text= Configuration.Func(1);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //XtraMessageBox.Show("Tele : c \n\n Site web : www.khadamat-tech.com \n\n Email : oussama.nm_15@hotmail.com ", "Supports", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InfoConx Info = new InfoConx();
            Info.ShowDialog();
        }
    }
}
