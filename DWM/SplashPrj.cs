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
using System.ServiceProcess;
using MySql.Data.MySqlClient;


namespace DWM
{
    public partial class SplashPrj : DevExpress.XtraEditors.XtraForm
    {
        public SplashPrj()
        {
            InitializeComponent();
        }
        MySqlDataReader dr;
        Boolean ShowFormSer = true;


        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SplashPrj_Load(object sender, EventArgs e)
        {         
            try
            {
                // Check if has License to Enter
                Configuration.TestIn();

                ClassConnexion.Macon.Open();
                // Check validation d'entree
                MySqlCommand Cmd = new MySqlCommand("select LibEntr from configuration where IdConf=16", ClassConnexion.Macon);
                dr = Cmd.ExecuteReader();
                dr.Read();

                if (dr[0].ToString() == "0")
                    ShowFormSer = true;
                else
                    ShowFormSer = false;

                dr.Close();
                ClassConnexion.Macon.Close();

                timer1.Start();   
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("المرجو تشغبل WampServer  ");
                Application.Exit();
            }
        }

        int i = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            if (i==2)
            {
                timer1.Stop();

                if (ShowFormSer== true)
                {
                    LicenseKey FormLk = new LicenseKey();
                    FormLk.Show();
                }
                else
                {
                    Login LogFORM = new Login();
                    LogFORM.Show();
                }
                this.Hide();
            }
        }

    }
}