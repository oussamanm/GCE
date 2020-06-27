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
using MySql.Data;

namespace DWM
{
    public partial class Eclairage : DevExpress.XtraEditors.XtraForm
    {
        public Eclairage()
        {
            InitializeComponent();
        }

        DataSet ds;
        DataView dv;
        MySqlDataAdapter da;
        MySqlCommandBuilder builder;
        Boolean dejaOvr = false;


        ////// Forms ////
        Sleep FSleep = new Sleep();

        ///// Méthodes
        public void RempEcl()
        {
            if (dejaOvr == true)
            {
                ds.Tables["Eclairage"].Clear();
            }
            da = new MySqlDataAdapter("select DATE_FORMAT(PeriodeConsoFact,\"%m-%Y\") as DateCons,S.LibelleSect,E.* from Eclairage E,Facture F,Secteur S where E.IdFct=F.IdFact and E.IdSect=S.IdSect", ClassConnexion.Macon);
            da.Fill(ds, "Eclairage");
            dejaOvr = true;
        }
        public void RempTabEcl()
        {
            dv.Table = ds.Tables["Eclairage"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
        }
        private void Eclairage_Load(object sender, EventArgs e)
        {
            ds = new DataSet();
            dv = new DataView();

            ClassConnexion.Macon.Open();
            ///// Remplir dataset Adherents
            RempEcl();
            RempTabEcl();

            ClassConnexion.Macon.Close();

            gridView1.Columns[2].Visible = false;
            gridView1.Columns[3].Visible = false;
            gridView1.Columns[4].Visible = false;
            gridView1.Columns[6].Visible = false;


            gridView1.Columns[0].Caption = "فترة الإستهلاك";
            gridView1.Columns[1].Caption = "الجولة";
            gridView1.Columns[5].Caption = "المبلغ";
            //gridView1.Columns[6].Caption = "المستخدم";
            gridView1.Columns[7].Caption = "تاريخ التعديل";


        }

        AjouEcl AjouE = new AjouEcl();
        AjouEcl AjouEMod ;
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag.ToString() == "Ajouter")
            {
                try
                {
                    AjouE.ShowDialog();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag.ToString() == "Modifier")
            {
                try
                {
                    AjouEMod = new AjouEcl(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[2].ToString()));
                    AjouEMod.ShowDialog(this);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag.ToString() == "Supprimer")
            {
                if (gridView1.SelectedRowsCount == 1)
                {
                    DialogResult dr = XtraMessageBox.Show("هل تريد فعلا حذف هذا الإستهلاك", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            string forma = "yyyy-MM-dd HH:mm:ss";

                            string RequeteSupp = string.Format("delete from Eclairage where IdEcl={0} ", int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[2].ToString()));
                            string NMsg = "الفاتورة : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString() + " | الجولة : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[1].ToString() + " | المبلغ : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[5].ToString();
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف إستهلاك   - " + DateTime.Now.ToString(forma);

                            Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");

                            if (Configuration.Func(15) == "Indirect")
                            {
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (Configuration.Func(15) == "Direct")
                            {
                                XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.ToString());
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("لم تقم بعد بتحديد إستهلاك كهربائي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            splashScreenManager1.ShowWaitForm();
            RempEcl();
            splashScreenManager1.CloseWaitForm();
        }

        private void panelControl1_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Login LogFORM = new Login();
            LogFORM.Show();

            this.Close();
            splashScreenManager1.CloseWaitForm();
        }

        private void panelControl3_Click(object sender, EventArgs e)
        {
            Sleep FSleep = new Sleep();

            FSleep.ShowDialog(this);
        }

        private void panelControlRedui_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            FormMenu MENU = new FormMenu();
            MENU.Show();

            this.Hide();
        }
    }
}