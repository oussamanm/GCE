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
    public partial class Utilisateurs : DevExpress.XtraEditors.XtraForm
    {
        public Utilisateurs()
        {
            InitializeComponent();
            controleHeader1.label_menu_btn = "الصفحة الرئيسية/المستخدمين";
        }
        MySqlDataAdapter dr;
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        string str;
        int IndexRowSelected;
        int IdRowSelected;

        MySqlDataAdapter da;
        MySqlCommandBuilder builder;
        AjouUti AjouUti = new AjouUti();
        AjouUti AjouUt;
        Sleep FSleep = new Sleep();
        String formatdt = "yyyy-MM-dd HH:mm:ss";
        string MsgNv, MsgEn;
        public void RempUti()
        {
            if (ds.Tables["utilisateurs"] != null)
            {
                ClassConnexion.DR.Close();
                ds.Tables["utilisateurs"].Clear();
                da = new MySqlDataAdapter("select u.*,LibelleType from utilisateurs u,typeutilisateur t where  u.idType=t.idType and bloque=0", ClassConnexion.Macon);
                da.Fill(ds, "utilisateurs");
            }
            else
            {
                da = new MySqlDataAdapter("select u.*,LibelleType from utilisateurs u,typeutilisateur t where  u.idType=t.idType and bloque=0", ClassConnexion.Macon);
                da.Fill(ds, "utilisateurs");
            }

        }
        public void RempTabUti()
        {
            dv.Table = ds.Tables["utilisateurs"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
        }
        private void Utilisateurscs_Load(object sender, EventArgs e)
        {
            try
            {

                RempUti();
                RempTabUti();
                gridView1.Columns[0].Caption = "الرقم الترتيبي";
                gridView1.Columns[2].Caption = "النسب";
                gridView1.Columns[3].Caption = "الإسم";
                gridView1.Columns[4].Caption = " الإسم المستعار";
                gridView1.Columns[6].Caption = "البريد الإلكتروني";
                gridView1.Columns[7].Caption = "الهاتف";
                gridView1.Columns[10].Caption = "الصفة";
                gridView1.Columns[5].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[8].Visible = false;
                gridView1.Columns[9].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }




        }
        private void label3_Click(object sender, EventArgs e)
        {
            FormMenu fm = new FormMenu();
            fm.ShowDialog();
            this.Hide();
        }


        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag.ToString() == "Ajouter")
                {
                    //.ShowDialog(this);
                    if (AjouUti.ShowDialog() == DialogResult.Yes)
                    {
                        RempUti();
                        RempTabUti();
                    }
                    else
                    {
                        AjouUti.ShowDialog(this);
                        this.Hide();
                    }
                }
                else if (e.Button.Properties.Tag.ToString() == "Modifier")
                {
                    if (gridView1.SelectedRowsCount > 0)
                    {
                        if (int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)["IdType"].ToString()) != Math.Abs(UserConnecte.IdType-3))
                        {
                            AjouUt = new AjouUti(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString()));

                            if (AjouUt.ShowDialog() == DialogResult.Yes)
                            {
                                RempUti();
                                RempTabUti();
                            }
                            else
                            {
                                AjouUti.ShowDialog(this);
                                this.Hide();
                            }
                        }
                        else
                            XtraMessageBox.Show("لا يمكن تعديل هاذا الحساب", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                else if (e.Button.Properties.Tag.ToString() == "suprimer")
                {
                    if (gridView1.SelectedRowsCount > 0)
                    {
                        DialogResult drt = XtraMessageBox.Show("هل تريد فعلا الحذف ؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        if (drt == DialogResult.OK)
                        {
                            if (gridView1.GetDataRow(gridView1.FocusedRowHandle)[8].ToString() != "1")
                            {
                                string[] table = { "adherent", "autrefrais", "categorieentres", "compteur", "consommation", "entres", "facture", "frais", "paiement", "penalite", "secteur", "traite", "tranches", "typepenalite","caisse","credit", "entrescais", "sortiescais" };
                                int nbr = 0;
                                for (int i = 0; i < table.Length; i++)
                                {
                                    if (Configuration.ExisteEnre(table[i], "idUser", gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString()) > 0)
                                    {
                                        nbr = 1;
                                        break;
                                    }
                                }
                                if (nbr != 1)
                                {
                                    string Requete3 = "delete from utilisateurs where  idUser=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0];

                                    MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بحذف مستخدم   (" + DateTime.Now.ToString(formatdt) + ") :";
                                    MsgNv = "إسم المستخدم  " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[2] + " " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[3]
                                             + " - الصفة " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[1]
                                             + " - الإسم المستعار " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[4]
                                             + " - البريد  الإلكتروني " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[6]
                                             + " - الهاتف " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[7];

                                    Configuration.Historique(1, Requete3, "", MsgNv, MsgEn, "", "");
                                    XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    string Requete3 = "update utilisateurs set bloque=1 where idUser=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0];
                                    MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بحذف مستخدم, بعد الموافقة سيتم حظره فقط لأنه مرتبط بإحدى العمليات   (" + DateTime.Now.ToString(formatdt) + ") :";
                                    MsgNv = "إسم المستخدم  " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[2] + " " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[3]
                                             + " - الصفة " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[1]
                                             + " - الإسم المستعار " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[4]
                                             + " - البريد  الإلكتروني " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[6]
                                             + " - الهاتف " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[7];

                                    Configuration.Historique(1, Requete3, "", MsgNv, MsgEn, "", "");
                                    XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                                XtraMessageBox.Show(" هذا المستخدم محظور من قبل", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                RempUti();
                RempTabUti();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}