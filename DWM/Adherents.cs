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
//using MySql.Data;
using MySql.Data.MySqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DWM.Impression;

namespace DWM
{
    public partial class Adherents : DevExpress.XtraEditors.XtraForm
    {
        public Adherents()
        {
            InitializeComponent();
            controleHeader1.label_menu_btn = "الصفحة الرئيسية/المنخرطون";
        }

        DataSet ds;
        DataView dv;
        string str;
        int IndexRowSelected, IdRowSelected,i;
        MySqlDataAdapter da;
        MySqlCommandBuilder builder;
        Boolean dejaOvr = false;

        ////// Forms ////
        AjouAdh AjouAd = new AjouAdh();
        AjouAdh AjouAdh;
        Sleep FSleep = new Sleep();



        ///// Méthodes
        public void RempAdh()
        {
            if (dejaOvr == true)
                ds.Tables["Adherent"].Clear();

            da = new MySqlDataAdapter("select adherent.IdAdherent,adherent.NomFrAdhe,adherent.PrenomFrAdhe, adherent.NomArAdhe, adherent.PrenomArAdhe, adherent.CINAdhe, case when adherent.SexAdhe =1 then 'ذكر' else 'انثى' end as SexeAd, adherent.DateInscAdhe, adherent.DateNaissAdhe, adherent.LieuNaissAdhe, case when adherent.DeceAdhe=1 then 'حي' else 'متوفى' end as DeceAdhe,case when adherent.ExiAdhe=1 then 'نشط' else 'غير نشط' end as ExiAdhe, count(IdComp) as countCom,concat(utilisateurs.NomUser,' ',utilisateurs.PrenomUser) as NomComp,adherent.DateModAdh from utilisateurs,adherent left join compteur on adherent.IdAdherent=compteur.IdAdherent where utilisateurs.IdUser=adherent.IdUser GROUP by adherent.IdAdherent", ClassConnexion.Macon);
            da.Fill(ds, "Adherent");
            dejaOvr = true;

        }
        public void RempTabAdh()
        {
            dv.Table = ds.Tables["Adherent"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
        }      
        public void Requte()
        {
            foreach (DataRow row in ds.Tables["Adherent"].Rows)
            {
                int index = ds.Tables["Adherent"].Rows.IndexOf(row);

                MySqlCommand CmdCount = new MySqlCommand("select count(IdComp) as countComp from adherent left join compteur on adherent.IdAdherent = compteur.IdAdherent WHERE adherent.IdAdherent =" + int.Parse(ds.Tables["Adherent"].Rows[index]["IdAdherent"].ToString()) + "  and StatutsComp = 1", ClassConnexion.Macon);
                ClassConnexion.DR = CmdCount.ExecuteReader();
                while (ClassConnexion.DR.Read())
                {
                    i = int.Parse(ClassConnexion.DR["countComp"].ToString());
                }
                ds.Tables["Adherent"].Rows[index]["countCom"] = i;

                ClassConnexion.DR.Close();
            }
        }


        private void Adherents_Load(object sender, EventArgs e)
        {
            try
            {
                
                splashScreenManager1.ShowWaitForm();

                ds = new DataSet();
                dv = new DataView();
                //da = new MySqlDataAdapter("select adherent.IdAdherent,adherent.NomFrAdhe,adherent.PrenomFrAdhe, adherent.NomArAdhe, adherent.PrenomArAdhe, adherent.CINAdhe,adherent.NumOrdreAdhe, case when adherent.SexAdhe =1 then 'ذكر' else 'انثى' end as SexeAd, adherent.DateInscAdhe, adherent.DateNaissAdhe, adherent.LieuNaissAdhe, case when adherent.DeceAdhe=1 then 'حي' else 'متوفى' end ,case when adherent.ExiAdhe=1 then 'نشط' else 'غير نشط' end,count(IdComp) as countCom,utilisateurs.PseudoUser,adherent.DateModAdh from adherent left join compteur on adherent.IdAdherent = compteur.IdAdherent ,utilisateurs WHERE utilisateurs.IdUser = adherent.IdUser and StatutsComp=1 group by IdAdherent", ClassConnexion.Macon);

                ClassConnexion.Macon.Open();
                ///// Remplir dataset Adherents
                RempAdh();
                RempTabAdh();
                ///// Remplir Count Compteur de chaque adherent
                Requte();
                ClassConnexion.Macon.Close();

                ///// Remplir Label RbAnnu
                RbAnnu.Text = RbAnnu.Text.ToString() + "(" + Configuration.ExisteEnre("adherent", "ExiAdhe", "0").ToString() + ")";
                ///// Remplir Label RbDea
                RbDea.Text = RbDea.Text.ToString() + "(" + Configuration.ExisteEnre("adherent", "DeceAdhe", "0").ToString() + ")";
                ///// Remplir Label RbDontHave
                ClassConnexion.Macon.Open();
                MySqlCommand Requetext = new MySqlCommand("select count(*) as nbr from adherent where IdAdherent not in(select IdAdherent from compteur where StatutsComp=1)", ClassConnexion.Macon);
                ClassConnexion.DR = Requetext.ExecuteReader();
                ClassConnexion.DR.Read();
                RbDontHave.Text = RbDontHave.Text.ToString() + "(" + ClassConnexion.DR["nbr"].ToString() + ")";
                ClassConnexion.DR.Close();
                ///// Remplir Label RbAll
                MySqlCommand cmdCou = new MySqlCommand("select count(*) from adherent", ClassConnexion.Macon);
                ClassConnexion.DR = cmdCou.ExecuteReader();
                ClassConnexion.DR.Read();
                RbAll.Text = RbAll.Text.ToString() + "(" + ClassConnexion.DR[0].ToString() + ")";
                ClassConnexion.DR.Close();

                ClassConnexion.Macon.Close();

                gridView1.Columns[0].Caption = "ر.ت";
                gridView1.Columns[0].ToolTip = "الرقم الترتيبي";
                gridView1.Columns[1].Caption = "Nom";
                gridView1.Columns[2].Caption = "Prenom";
                gridView1.Columns[3].Caption = "النسب";
                gridView1.Columns[4].Caption = "الإسم";
                gridView1.Columns[5].Caption = "ر.ب.و";
                gridView1.Columns[6].Caption = "الجنس";
                gridView1.Columns[7].Caption = "ت.الإنخراط";
                gridView1.Columns[8].Caption = "ت.الإزدباد";
                gridView1.Columns[9].Caption = "مكان الإزدباد";
                gridView1.Columns[10].Caption = "حالة المنخرط";
                gridView1.Columns[11].Caption = " نشاط المنخرط";
                gridView1.Columns[12].Caption = "العدادات المتصلة";
                gridView1.Columns[13].Caption = "المستخدم";
                gridView1.Columns[14].Caption = "تاريخ التعديل";

                gridView1.Columns[6].Width = 99;
                gridView1.Columns[7].Width = 90;
                gridView1.Columns[8].Width = 90;
                gridView1.Columns[9].Width = 99;
                gridView1.Columns[0].Width = 65;
                gridView1.Columns[10].Width = 95;
                gridView1.Columns[11].Width = 100;
                gridView1.Columns[12].Width = 115;
                gridView1.Columns[14].Width = 90;
                gridView1.Columns[13].Width = 80;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.Macon.Close();
                splashScreenManager1.CloseWaitForm();
            }
        }

        ////// Button Actions 
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag.ToString() == "Ajouter")
                {
                    AjouAd.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag.ToString() == "Modifier")
                {
                    AjouAdh = new AjouAdh(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString()));
                    AjouAdh.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag.ToString() == "Supprimer")
                {
                    if (gridView1.SelectedRowsCount == 1)
                    {
                        DialogResult dr = XtraMessageBox.Show("هل تريد فعلا حذف هذا المنخرط", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            string forma = "yyyy-MM-dd HH:mm:ss";

                            string RequeteSupp = string.Format("UPDATE adherent SET ExiAdhe=0 WHERE IdAdherent={0} ", IdRowSelected);
                            string NMsg = "الرقم الترتيبي : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString() + " | Nom : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[1].ToString() + " | Prénom : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[2].ToString() + " | النسب : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString() + " | الإسم : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[4].ToString() + " | ر.ب.و : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[5].ToString() + " | الجنس : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[6].ToString() + " | تاريخ الإنخراط : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[7].ToString() + " | تاريخ الإزدياد : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[8].ToString() + " | مكان الإزدياد : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[9].ToString() + " | حالة المنخرط : " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[10].ToString();
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف منخرط   - " + DateTime.Now.ToString(forma);

                            Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");
                            if (Configuration.Func(15) == "Indirect")
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else if (Configuration.Func(15) == "Direct")
                                XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("لم تقم بعد بتحديد المنخرط", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (e.Button.Properties.Tag.ToString() == "Imprimer")
                {
                    ConfigImpressionAdhe print = new ConfigImpressionAdhe();
                    print.ShowDialog();
                }

                RempAdh();
                RempTabAdh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        ///// Remplir Count RButtons
        private void RbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (RbAll.Checked == true)
            {
                dv.RowFilter = "";
            }

        }
        private void RbDea_CheckedChanged(object sender, EventArgs e)
        {
            if (RbDea.Checked == true)
            {
                dv.RowFilter = "DeceAdhe like'متوفى'";
            }
        }
        private void RbAnnu_CheckedChanged(object sender, EventArgs e)
        {
            if (RbAnnu.Checked == true)
            {
                dv.RowFilter = "ExiAdhe like 'غير نشط'";
            }
        }
        private void RbDontHave_CheckedChanged(object sender, EventArgs e)
        {
            if (RbDontHave.Checked == true)
            {
                dv.RowFilter = "countCom=0";
            }
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["ExiAdhe"]);
                if (category == "غير نشط")
                {
                    e.Appearance.BackColor = Color.FromArgb(0xE1, 0xFB, 0x13, 0x13);
                    e.Appearance.BackColor2 = Color.FromArgb(0x00, 0x00, 0x00, 0x00);
                }
            }
        }
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            IdRowSelected = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString());
            IndexRowSelected = gridView1.FocusedRowHandle;
        }
    }
}