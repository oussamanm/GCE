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
    public partial class LicenseKey : DevExpress.XtraEditors.XtraForm
    {
        public LicenseKey()
        {
            InitializeComponent();
        }

        Boolean ValidateSomme = false;
        Boolean ValidateSameInBD = false;
        string formatt = "yyyy-MM-dd HH:mm:ss";
        int NbrEx;
        string StrIdHd;

        MySqlDataReader dr;

        private void LicenseKey_Load(object sender, EventArgs e)
        {
            StrIdHd = HardwareInfo.GetBIOSserNo();
        }


        private void pictureEditClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
 
        private void pictureEdit3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtActiver2_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            try
            {
                string Serie, oldSerie;
                int i1, i2, i3, i4, inbrm = 0;
                int T = 0;


                i1 = int.Parse(TEKey1.Text);
                i2 = int.Parse(TEKey2.Text);
                i3 = int.Parse(TEKey3.Text);
                i4 = int.Parse(TEKey4.Text);

                oldSerie = i1 + "-" + i2 + "-" + i3 + "-" + i4;

                i1 = i1 % 1000;
                i1 += 5000;
                i4 = i4 / 10;
                string i4s = i4.ToString();
                i4s = i4s + "2";
                i4 = int.Parse(i4s);
                T = i1 + i2 + i3 + i4;
                Serie = i1 + "-" + i2 + "-" + i3 + "-" + i4;

                // Calculate nbr of Months
                inbrm = i2 % 10;
                if (inbrm == 0)
                    NbrEx = 3650;
                else
                    NbrEx = inbrm * 30;

                /// Teste For somme
                if (T == 15648)
                    ValidateSomme = true;
                else
                    ValidateSomme = false;

                /// Teste For if Exist in BD
                ClassConnexion.Macon.Open();
                MySqlCommand Cmd = new MySqlCommand("select * from donnetraite where SerieDTr ='" + oldSerie + "' ", ClassConnexion.Macon);
                dr = Cmd.ExecuteReader();
                if (dr.HasRows == true)
                    ValidateSameInBD = false;
                else
                    ValidateSameInBD = true;

                dr.Close();
                ClassConnexion.Macon.Close();
                
                if (ValidateSomme == true && ValidateSameInBD == true)
                {
                    StrIdHd = HardwareInfo.GetBIOSserNo();

                    if (ClassConnexion.Macon.State == ConnectionState.Closed)
                        ClassConnexion.Macon.Open();

                    MySqlCommand CmdUpdateV = new MySqlCommand("update configuration set LibEntr=True where IdConf=16", ClassConnexion.Macon);
                    MySqlCommand CmdUpdateDateV = new MySqlCommand("update configuration set LibEntr='" + DateTime.Now.ToString(formatt) + "' where IdConf=17", ClassConnexion.Macon);
                    MySqlCommand CmdUpdateNbrM = new MySqlCommand("update configuration set LibEntr=" + NbrEx + " where IdConf=19", ClassConnexion.Macon);
                    MySqlCommand CmdInsert = new MySqlCommand("insert into donnetraite (SerieDTr,DateDTr,NbrCh) VALUES ('" + oldSerie + "','" + DateTime.Now.ToString(formatt) + "'," + NbrEx + ")", ClassConnexion.Macon);
                    MySqlCommand CmdInsertID = new MySqlCommand("update configuration set LibEntr='" + StrIdHd + "' where IdConf=18", ClassConnexion.Macon);

                    CmdUpdateV.ExecuteNonQuery();
                    CmdUpdateDateV.ExecuteNonQuery();
                    CmdUpdateNbrM.ExecuteNonQuery();
                    CmdInsert.ExecuteNonQuery();
                    CmdInsertID.ExecuteNonQuery();

                    ClassConnexion.Macon.Close();

                    splashScreenManager1.CloseWaitForm();

                    Login LogFORM = new Login();
                    LogFORM.Show();

                    this.Close();
                }
                else
                {
                    splashScreenManager1.CloseWaitForm();
                    XtraMessageBox.Show("رقم التفعيل غير صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            InfoConx InfConx = new InfoConx();
            InfConx.ShowDialog(this);
        }
    }
}