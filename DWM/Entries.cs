using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Text;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;

namespace DWM
{
    public partial class Entries : DevExpress.XtraEditors.XtraForm
    {
        public Entries()
        {
            InitializeComponent();
        }
        MySqlDataAdapter dr;
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        string str;
        int IndexRowSelected, IdRowSelected;
        int visible = 0;
        MySqlDataAdapter da;
        MySqlCommandBuilder builder;


        ////// Forms ////
        AjouEnt addEnt = new AjouEnt();
        AjouEnt AjouEn;
        Sleep FSleep = new Sleep();

        public void RempEnt()
        {
            if (ds.Tables["entres"] != null)
            {
                ClassConnexion.DR.Close();
                ds.Tables["entres"].Clear();
                // da = new MySqlDataAdapter("select e.idEntr,LibEntr,MontantEntr,concat(NomUser,' ',PrenomUser) as l,DateEntr,LibelleCatEntr,case when idFraisau!=0 then 'لا' else 'نعم' end as paye from entres e,categorieentres c,utilisateurs u where  e.idUser=u.idUser and e.idCatEntr=c.idCatEntr  ", ClassConnexion.Macon);
                da = new MySqlDataAdapter("call procEntre() ", ClassConnexion.Macon);
                da.Fill(ds, "entres");
            }
            else
            {
                da = new MySqlDataAdapter("call procEntre()  ", ClassConnexion.Macon);
                da.Fill(ds, "entres");
            }
        }
        public void RempTabEnt()
        {
            dv.Table = ds.Tables["entres"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
        }

        private void Entries_Load(object sender, EventArgs e)
        {

            try
            {
                splashScreenManager1.ShowWaitForm();

                RempEnt();
                RempTabEnt();

                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag.ToString() == "Ajouter")
                {
                    visible = 1;
                    if (addEnt.ShowDialog() == DialogResult.Yes)
                    {
                        RempEnt();
                        RempTabEnt();
                    }
                    else
                    {
                        addEnt.ShowDialog(this);
                        this.Hide();
                    }
                }
                else if (e.Button.Properties.Tag.ToString() == "Modifier")
                {
                    if (gridView1.SelectedRowsCount > 0)
                    {
                        AjouEn = new AjouEnt(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString()));

                        if (AjouEn.ShowDialog() == DialogResult.Yes)
                        {
                            RempEnt();
                            RempTabEnt();
                        }
                        else
                        {
                            AjouEn.ShowDialog(this);
                            this.Hide();
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                else if (e.Button.Properties.Tag.ToString() == "suprimer")
                {
                    DialogResult drt = XtraMessageBox.Show("هل تريد فعلا الحذف ؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (gridView1.SelectedRowsCount > 0)
                    {
                        if (drt == DialogResult.OK)
                        {
                            string forma = "yyyy-MM";
                            DateTime dt = new DateTime();
                            DateTime dt2 = new DateTime();
                            dt = DateTime.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[4].ToString()).AddMonths(1);
                            if (gridView1.GetDataRow(gridView1.FocusedRowHandle)[6].ToString() == "")
                            {
                                splashScreenManager1.ShowWaitForm();
                                ClassConnexion.Macon.Open();
                                MySqlCommand Cmd = new MySqlCommand("delete from entres where  idEntr=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0], ClassConnexion.Macon);
                                Cmd.ExecuteNonQuery();
                                ClassConnexion.Macon.Close();

                                RempEnt();
                                RempTabEnt();
                                splashScreenManager1.CloseWaitForm();
                                XtraMessageBox.Show(" نم حذف هذا السطر");
                            }
                            else if (DateTime.Parse(dt.ToString(forma)) > DateTime.Parse(DateTime.Now.ToString(forma)))
                            {
                                splashScreenManager1.ShowWaitForm();
                                ClassConnexion.Macon.Open();

                                MySqlCommand Cmd = new MySqlCommand("delete from entres where  idEntr=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0], ClassConnexion.Macon);
                                MySqlCommand Cmd2 = new MySqlCommand("delete from autreFrais where  idEntr=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0], ClassConnexion.Macon);

                                Cmd.ExecuteNonQuery();
                                Cmd2.ExecuteNonQuery();

                                ClassConnexion.Macon.Close();

                                RempEnt();
                                RempTabEnt();
                                splashScreenManager1.CloseWaitForm();
                                XtraMessageBox.Show("نم حذف هذا السطر");

                            }
                            else
                                XtraMessageBox.Show("لا يمكنك حذف هذا السطر");
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد حذفه");
                }
                else if (e.Button.Properties.Tag.ToString() == "Imprimer")
                {
                    ConfigImpressionEntr ConfigImpressEntr = new ConfigImpressionEntr();
                    ConfigImpressEntr.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void panelControl1_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Login LoginFORM = new Login();
            LoginFORM.Show();
            this.Close();
            splashScreenManager1.CloseWaitForm();
        }
        private void panel2_Click(object sender, EventArgs e)
        {
            FormMenu MENU = new FormMenu();
            MENU.Show();

            this.Close();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            FormMenu MENU = new FormMenu();
            MENU.Show();

            this.Close();
        }
        private void panelControlRedui_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panelControl3_Click(object sender, EventArgs e)
        {
            FSleep.ShowDialog(this);
        }

    }
}