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
    public partial class Sleep : DevExpress.XtraEditors.XtraForm
    {
        public Sleep()
        {
            InitializeComponent();
        }


        private void Sleep_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label1.Text = UserConnecte.NomUser + " " + UserConnecte.PrenomUser;
            label2.Text = UserConnecte.LibelleType;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {      
                int nbrchar = textBox1.Text.Length;
                if (nbrchar >= 8)
                {
                    if (UserConnecte.PasseUser == password.getMd5Hash(textBox1.Text))
                    {
                        UserConnecte.CompHeur = DateTime.Now;
                        this.Close();
                    }
                    else
                    {
                        textBox1.BackColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    textBox1.BackColor = System.Drawing.Color.White;
                }         
        }


    }
}