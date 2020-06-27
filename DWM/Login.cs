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
using MySql.Data.MySqlClient;

namespace DWM
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
        }

        public void ShowHide(Panel l, Panel p)
        {
            panel11.Hide();
            panel3.Hide();
            panel8.Hide();
            panel10.Hide();
            l.Show();
            p.Show();
        }
        void connecter()
        {
            var Pseudo = textBox1.Text;
            var Pass = textBox2.Text;

            if (Pseudo != "" && Pass != "")
            {
                try
                {
                    Pass = password.getMd5Hash(Pass);
                    ClassConnexion.Macon.Open();
                    MySqlCommand Requetuser = new MySqlCommand("select * from utilisateurs where PseudoUser='" + Pseudo + "' and PasseUser='" + Pass + "' and bloque=0", ClassConnexion.Macon);
                    Requetuser.CommandTimeout = 60;
                    ClassConnexion.DR = Requetuser.ExecuteReader();
                    ClassConnexion.DR.Read();

                    if (ClassConnexion.DR.HasRows)
                    {
                        ClassConnexion.DR.Close();
                        ClassConnexion.Macon.Close();
                        UserConnecte.login(Pseudo, Pass);
                        FormMenu fm = new FormMenu();
                        this.Close();
                        fm.Show();
                    }
                    else
                    {
                        ClassConnexion.DR.Close();
                        ClassConnexion.Macon.Close();
                        MessageBox.Show(" المعلومات التي تم إدخالها خاطئة");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("يجب إدخال الإسم والرقم السري");
            }
        }


        private void panel5_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            ShowHide(panel3 ,panel8);
        }
        private void panel4_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
            ShowHide(panel10, panel11);
        }
        private void panel6_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            ShowHide(panel3, panel8);
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            textBox2.Focus();
            ShowHide(panel11, panel10);
        }      
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            ShowHide(panel11, panel10);
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            ShowHide(panel11, panel10);
        }
        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                textBox2.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pictureEdit2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void pictureEdit3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            connecter();
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            ShowHide(panel3, panel8);
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                connecter();
            }
        }
    }
}