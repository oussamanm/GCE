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
using DevExpress.XtraTreeList;

namespace DWM
{
    public partial class Factures : DevExpress.XtraEditors.XtraForm
    {
        public Factures()
        {
            InitializeComponent();
        }

        MySqlDataAdapter da;
        DataSet ds;
        MySqlDataReader dr;
        Boolean TestDejaOListeFrais = false;
        Boolean TestDejaOListeTranche = false;
        Boolean TestDejaOvrireData = false;
        string ShortForma = "yyyy-MM-dd";

        private void RempListeBox(TreeList Tl ,string Reque,string table)
        {
            if (TestDejaOListeFrais == true)
            {
                ds.Tables[table].Clear();
            }
            if (TestDejaOListeTranche == true)
            {
                ds.Tables[table].Clear();
            }
            da = new MySqlDataAdapter(Reque,ClassConnexion.Macon);
            da.Fill(ds, table);

            Tl.DataSource = ds.Tables[table];
            Tl.Refresh();
            
        }
        private void RempDate()
        {
            if (TestDejaOvrireData == true)
            {
                ds.Tables["Compteur"].Clear();
            }
            da = new MySqlDataAdapter("SELECT CONCAT(adherent.NomFrAdhe,' ', adherent.PrenomFrAdhe) AS NomComp, adherent.CINAdhe, compteur.NumComp, compteur.siyam,case when compteur.StatutsComp=1 then 'متصل' else 'منفصل' end as StatutsComp, secteur.LibelleSect FROM adherent INNER JOIN compteur ON adherent.IdAdherent = compteur.IdAdherent INNER JOIN secteur ON compteur.IdSect = secteur.IdSect", ClassConnexion.Macon);
            da.Fill(ds, "Compteur");

            TestDejaOvrireData = true;
        }
        public  int ExisEnre()
        {
            ClassConnexion.Macon.Open();
            MySqlCommand Requetext = new MySqlCommand("select count(*) as nbr from Facture  ", ClassConnexion.Macon);
            dr = Requetext.ExecuteReader();
            dr.Read();
            int resultat = int.Parse(dr["nbr"].ToString());
            dr.Close();
            ClassConnexion.Macon.Close();
            return resultat;
        }
        public void Enregi(int IdEnre, string STR, string STR2)
        {
            string forma = "yyyy-MM-dd HH:mm:ss";
            string RequeteModifier = string.Format("update configuration set LibEntr=\\'" + STR + "\\' where IdConf=" + IdEnre);
            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتغيير  " + STR2 + " - " + DateTime.Now.ToString(forma);

            Configuration.Historique(0, RequeteModifier, "", "", MEnt, "", "");
        }
        private void ProccesSelectItem()
        {
            if (i == 1)
            {
                RempListeBox(TlFrais, "SELECT LibelleFraiC, PrixUFraiC FROM fraiscopie WHERE (configYesNon = 1)  AND IdCopie = (SELECT IdFraisC2 FROM facture WHERE IdFact = " + CbDateCons.SelectedValue + ")", "Frais");
                RempListeBox(TlTran, "SELECT LibelleTranC,PrixUTranC FROM tranchescopie WHERE IdCopie =(SELECT IdCopieTran FROM facture WHERE IdFact = " + CbDateCons.SelectedValue + ")", "Tranche");

                labelPeriodeCons.Text = CbDateCons.Text;
                if (ClassConnexion.Macon.State == ConnectionState.Open)
                {
                    ClassConnexion.Macon.Close();
                }
                ClassConnexion.Macon.Open();
                MySqlCommand CmdRemplablePeriode = new MySqlCommand("select DATE_FORMAT(PeriodePaieDFact,\"%d-%m-%Y\") as PeriodeD,DATE_FORMAT(PeriodePaieFFact,\"%d-%m-%Y\") as PeriodeF from facture where IdFact=" + CbDateCons.SelectedValue + "", ClassConnexion.Macon);
                dr = CmdRemplablePeriode.ExecuteReader();
                while (dr.Read())
                {
                    labelPeriodPaieD.Text = dr["PeriodeD"].ToString();
                    labelPeriodPaieF.Text = dr["PeriodeF"].ToString();
                }
                dr.Close();
                ClassConnexion.Macon.Close();
            }
        }
        public void RempChekTraiteAndAutreFr()
        {
            if (Configuration.Func(21)=="0")
            {
                radioButtonNTr.Checked = true;
                radioButtonYTr.Checked = false;
            }
            else
            {
                radioButtonNTr.Checked = false;
                radioButtonYTr.Checked = true;
            }

            if (Configuration.Func(22) == "0")
            {
                radioButtonNAu.Checked = true;
                radioButtonYAu.Checked = false;
            }
            else
            {
                radioButtonNAu.Checked = false;
                radioButtonYAu.Checked = true;
            }
        }
        private void Factures_Load(object sender, EventArgs e)
        {
            try
            {
                if (ExisEnre()<=0)
                {
                    XtraMessageBox.Show("لاتوجد أي فترة إستهلاك", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FormMenu Menu = new FormMenu();
                    Menu.Show();
                    this.Close();
                }

                ds = new DataSet();

                Configuration.RempCombo(CbDateCons, "select IdFact,DATE_FORMAT(PeriodeConsoFact,\"%m-%Y\") as DateCons from facture", "DateCons", "IdFact", "DateCons");
                i = 1;
                RempListeBox(TlFrais, "SELECT LibelleFraiC, PrixUFraiC FROM fraiscopie WHERE (configYesNon = 1) AND IdCopie = (SELECT IdFraisC2 FROM facture WHERE IdFact =" + CbDateCons.SelectedValue + ")", "Frais");
                RempListeBox(TlTran, "SELECT LibelleTranC,PrixUTranC FROM tranchescopie WHERE IdCopie =(SELECT IdCopieTran FROM facture WHERE IdFact = " + CbDateCons.SelectedValue + ")", "Tranche");
                TestDejaOListeTranche = true;
                TestDejaOListeFrais = true;

                RempChekTraiteAndAutreFr();
                ProccesSelectItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        int i=0;
        private void CbDateCons_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProccesSelectItem();
        }

        private void checkEditToutsComp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditToutsComp.Checked == true)
            {
                checkEditComp.Checked = false;
            }
        }
        private void checkEditComp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditComp.Checked == true)
            {
                checkEditToutsComp.Checked = false;
                searchLookUpEditComp.Enabled = true;
                RempDate();
                searchLookUpEditComp.Properties.DataSource = ds.Tables["Compteur"];
                searchLookUpEditComp.Properties.DisplayMember = ds.Tables["Compteur"].Columns["NumComp"].ColumnName;
                searchLookUpEditComp.Properties.ValueMember = ds.Tables["Compteur"].Columns["NumComp"].ColumnName;
            }
            else
            {
                searchLookUpEditComp.Enabled = false;
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Imprimer")
            {
                if (checkEditComp.Checked == true)
                {
                    ConfigImpressionFacture FormConfigImpr = new ConfigImpressionFacture(int.Parse(searchLookUpEditComp.EditValue.ToString()), int.Parse(CbDateCons.SelectedValue.ToString()));
                    FormConfigImpr.ShowDialog(this);
                }
                else if (checkEditToutsComp.Checked = true)
                {
                    ConfigImpressionFacture FormConfigImpr = new ConfigImpressionFacture(int.Parse(CbDateCons.SelectedValue.ToString()));
                    FormConfigImpr.ShowDialog(this);
                }

            }
        }

        private void labelMenu_Click(object sender, EventArgs e)
        {
            FormMenu Menu = new FormMenu();
            Menu.Show();
            this.Close();
        }
        private void panelMenu_Click(object sender, EventArgs e)
        {
            FormMenu Menu = new FormMenu();
            Menu.Show();
            this.Close();
        }
        private void panelControl1_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Login LogFORM = new Login();
            LogFORM.Show();

            this.Hide();
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

        private void radioButtonNTr_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNTr.Checked==true)
            {
                Enregi(21,"0", "إضهار الأقساط الشهرية في الفاتورة");
            }
            else if (radioButtonYTr.Checked==true)
            {
                Enregi(21,"1", "");
            }
        }
        private void radioButtonNAu_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNAu.Checked == true)
            {
                Enregi(22, "0", "");
            }
            else if (radioButtonYAu.Checked == true)
            {
                Enregi(22, "1", "إضهار مبالغ اخرى في الفاتورة");
            }
        }
    }
}