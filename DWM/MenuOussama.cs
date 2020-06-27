using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DWM
{
    public partial class MenuOussama : Form
    {
        public MenuOussama()
        {
            InitializeComponent();
        }

        private void tileNavCategory1_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            //MessageBox.Show(navButton11.GetType().ToString());

            foreach (DevExpress.XtraBars.Navigation.NavButton item in tileNavPane1.Buttons)
            {
                item.Appearance.BackColor = Color.FromArgb(0x00, 0x00, 0x63, 0xB1);
                item.Appearance.ForeColor = Color.White;
            }


            e.Element.Appearance.BackColor = Color.White;
            e.Element.Appearance.ForeColor = Color.FromArgb(0x00, 0x40, 0x40, 0x40);
        }
    }
}
