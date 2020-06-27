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
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using DevExpress.XtraReports.UI;

namespace DWM
{
    public partial class Paiement : DevExpress.XtraEditors.XtraForm
    {

        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataSet ds;
        DataSet DSImpression;
        DataView dv;
        Boolean TestDejaO = false;
        Boolean TestDejaOMonths = false;

        int indexSR = 0;
        string forma = "MM-yyyy";
        string ShortForma = "yyyy-MM-dd";
        string ShortFormaSansTi = "ddMMyyyy";
        DateTime VarD;
        float Totale;
        DataTable Table;

        float VarLabelTM = 0;
        float VarLabelTT = 0;
        float VarLabelTP = 0;
        float VarLabelTE = 0;
        float VarLabelReste = 0;
        float VarLabelTotale = 0;
        float VarMontantPayer = 0;
        float VarLabelEntree = 0;

        string NomComplete;
        string NumAdhe;
        string NumComp;
        string Contra;
        string IdSecteur;
        string LiSecteur;
        string IdUser;
        string Secteur;

        ImpressionRecuPaiement report;

        public Paiement()
        {
            InitializeComponent();
            TbNum.Select();
        }

        void RempData()
        {
            gridControl1.DataSource = dv;
            gridControl1.Refresh();

            gridView1.Columns[4].Visible = false;
            gridView1.Columns[6].Visible = false;
            gridView1.Columns[7].Visible = false;
            gridView1.Columns[8].Visible = false;
            gridView1.Columns[10].Visible = false;
            gridView1.Columns[11].Visible = false;
            gridView1.Columns[12].Visible = false;

            gridView1.Columns[0].Caption = "الرقم";
            gridView1.Columns[1].Caption = "المشترك";
            gridView1.Columns[2].Caption = "Adherent";
            gridView1.Columns[3].Caption = "ر.ب.و";
            gridView1.Columns[5].Caption = "حالة المشترك";
            gridView1.Columns[9].Caption = "رقم العداد";
            gridView1.Columns[13].Caption = "الجولة";
        }
        void Vider()
        {
            label17.Text = "";
            label16.Text = "";
            label15.Text = "";
            label14.Text = "";
            label13.Text = "";
            label12.Text = "";
            label11.Text = "";
            label10.Text = "";

            labelTotaleM.Text = "0";
            labelTotaleTr.Text = "0";
            labelTotalPena.Text = "0";
            labelMontantMAc.Text = "0";
            labelMontantTotale.Text = "0";
            labelReste.Text = "0";
            TbMablegh.Text = "0";
            labelTotalEnt.Text = "0";

            VarLabelTM = 0;
            VarLabelTT = 0;
            VarLabelTP = 0;
            VarLabelTE = 0;
            VarLabelReste = 0;
            VarLabelTotale = 0;
            VarMontantPayer = 0;

            checkedListBoxControl1.Items.Clear();
            checkedListBoxControl2.Items.Clear();
            checkedListBoxControl3.Items.Clear();
            checkedListBoxControl4.Items.Clear();

            TbNum.Select();
        }
        void CalculeTotale()
        {
            VarLabelTotale = VarLabelTM + VarLabelTT + VarLabelTP + VarLabelTE;
            labelMontantTotale.Text =Configuration.ConvertToMony(VarLabelTotale);
        }
        void CalculReste()
        {
            
            if (TbMablegh.Text != "")
            {
                VarMontantPayer = float.Parse(TbMablegh.Text);
                Montantpayer = VarMontantPayer;
                labelReste.Text = Configuration.ConvertToMony(Montantpayer - VarLabelTotale);
            }
        }
        void keyDown()
        {
            splashScreenManager1.ShowWaitForm();
            string Prefixe = Configuration.Func(13);

            ClassConnexion.Macon.Open();
            MySqlCommand CmdRech = new MySqlCommand("select C.*,A.IdAdherent as IdAd,A.*,S.IdSect as idSec,S.* from compteur C,adherent A,secteur S where C.IdAdherent=A.IdAdherent and C.IdSect=S.IdSect and C.NumComp=" + int.Parse(TbNum.Text) + " ", ClassConnexion.Macon);
            dr = CmdRech.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                NomComplete = dr["NomArAdhe"].ToString() + " " + dr["PrenomArAdhe"].ToString();
                NumAdhe = dr["IdAd"].ToString();
                NumComp = dr["NumComp"].ToString();
                Contra = Prefixe+ dr["NumComp"].ToString();
                IdSecteur = dr["idSec"].ToString();
                LiSecteur = dr["LibelleSect"].ToString();
                IdUser = UserConnecte.IdUser.ToString();

                Vider();

                if (TestDejaOMonths == true)
                {
                    ds.Tables["Months"].Clear();
                    ds.Tables["Traite"].Clear();
                    ds.Tables["Penalite"].Clear();
                    ds.Tables["Entree"].Clear();
                }
                TestDejaOMonths = true;

                label17.Text = dr["NomArAdhe"].ToString() + " " + dr["PrenomArAdhe"].ToString();
                label16.Text = dr["NumComp"].ToString();
                label15.Text = dr["siyam"].ToString();
                label14.Text = dr["LibelleSect"].ToString();
                if (dr["StatutsComp"].ToString() == "True")
                {
                    label13.Text = "متصل";
                }
                else
                {
                    label13.Text = "منفصل";
                }
                label12.Text = dr["PartsComp"].ToString();
                label11.Text = dr["CINAdhe"].ToString();
                if (dr["DeceAdhe"].ToString() == "True")
                {
                    label10.Text = "على قيد الحياة";
                }
                else
                {
                    label10.Text = "متوفى";
                }
                ClassConnexion.Macon.Close();
                dr.Close();


                ////// Remp Les CheckBoxliSt /////

                RempListCh("select (paiement.MontantPaie+paiement.PenalitePaie) as Mtpen,paiement.*,facture.*,compteur.IdSect,case when DATEDIFF(DATE_FORMAT(now(),\"%Y-%m-%d\"),DATE_FORMAT(facture.PeriodePaieFFact,\"%Y-%m-%d\"))>1 then 1 else 0 end as Pena from paiement,facture,consommation,compteur where paiement.IdFact=facture.IdFact and PayePaie=0 and consommation.IdCons=paiement.IdCons and consommation.IdComp=compteur.IdComp and  compteur.NumComp = " + int.Parse(label16.Text) + " ", "Months", checkedListBoxControl1, "PeriodeConsoFact", "IdPaie");
                RempListCh("select M.*,T.*,T.IdTrai as IdTraite from moistraite M,traite T,compteur C where M.IdTrai=T.IdTrai and C.IdComp=T.IdComp and C.NumComp = " + int.Parse(label16.Text) + " and M.PayerMTr = False order by M.IdMTr desc ", "Traite", checkedListBoxControl2, "MoisMTr","IdMTr");
                MySqlDataAdapter daPena = new MySqlDataAdapter("select P.*,T.LibelleTypePena,MontantPena,concat(LibelleTypePena,' ==> ',MontantPena) as LibPlusMont from penalite P,compteur C,typepenalite T where P.IdComp= C.IdComp and P.IdTypePena=T.IdTypePena and C.NumComp= " + int.Parse(label16.Text) + " and P.PayerPena=0", ClassConnexion.Macon);
                daPena.Fill(ds, "Penalite");
                int CountPena = ds.Tables["Penalite"].Rows.Count;
                for (int i = 0; i < CountPena; i++)
                {
                    CheckedListBoxItem Chk = new CheckedListBoxItem();
                    Chk.Description = ds.Tables["Penalite"].Rows[i]["LibPlusMont"].ToString();
                    Chk.Value = ds.Tables["Penalite"].Rows[i]["IdPena"].ToString();
                    checkedListBoxControl3.Items.Add(Chk);
                }

                MySqlDataAdapter daEntr = new MySqlDataAdapter("select A.*,E.*,C.*,A.IdEntr as IdAutreF from autrefrais A,entres E,categorieentres C,compteur Comp where E.IdEntr=A.IdEntr and E.IdCatEntr=C.IdCatEntr and A.IdComp=Comp.IdComp and  Comp.NumComp = " + int.Parse(label16.Text) + " and A.PayerEntr = False order by A.IdFraisau desc ", ClassConnexion.Macon);
                daEntr.Fill(ds, "Entree");
                int CountEnt = ds.Tables["Entree"].Rows.Count;
                for (int i = 0; i < CountEnt; i++)
                {
                    CheckedListBoxItem Chk = new CheckedListBoxItem();
                    Chk.Description = ds.Tables["Entree"].Rows[i]["LibelleCatEntr"].ToString();
                    Chk.Value = ds.Tables["Entree"].Rows[i]["IdAutreF"].ToString();
                    checkedListBoxControl4.Items.Add(Chk);
                }
                //RempListCh("select A.*,E.*,C.* from autrefrais A,entres E,categorieentres C,compteur Comp where E.IdEntr=A.IdEntr and E.IdCatEntr=C.IdCatEntr and A.IdComp=Comp.IdComp and  Comp.NumComp = " + int.Parse(label16.Text) + " and A.PayerEntr = False order by A.IdFraisau desc ", "Entree", checkedListBoxControl4, "IdFraisau", "C.LibelleCatEntr");



                ////// Remp Label Total CheckedBox  //////

                if (ds.Tables["Months"].Rows.Count != 0)
                {
                    checkedListBoxControl1.Items[checkedListBoxControl1.Items.Count - 1].CheckState = CheckState.Checked;
                    labelMontantMAc.Text = ds.Tables["Months"].Rows[ds.Tables["Months"].Rows.Count - 1]["Mtpen"].ToString();
                    VarLabelTM = float.Parse(ds.Tables["Months"].Rows[ds.Tables["Months"].Rows.Count - 1]["Mtpen"].ToString());

                    labelTotaleM.Text = Configuration.ConvertToMony(VarLabelTM);
                    CalculeTotale();
                }
                //if (ds.Tables["Traite"].Rows.Count != 0)
                //{
                //    labelTotaleTr.Text = ds.Tables["Traite"].Rows[ds.Tables["Traite"].Rows.Count - 1]["MontantMTr"].ToString();
                //    VarLabelTT = 0/*float.Parse(ds.Tables["Traite"].Rows[ds.Tables["Traite"].Rows.Count - 1]["MontantMTr"].ToString())*/;

                //    labelTotaleTr.Text = Configuration.ConvertToMony(VarLabelTT);
                //}
                //labelTotalPena.Text = "0";
                CalculeTotale();

                splashScreenManager1.CloseWaitForm();
            }
            else
            {
                ClassConnexion.Macon.Close();
                dr.Close();

                splashScreenManager1.CloseWaitForm();
                XtraMessageBox.Show("هذا العداد غير موجود", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Vider();
            }
            
        }
        void RempListCh(string requte,string table, CheckedListBoxControl Checkedlist,string columnDesc,string columnvalue)
        {
            MySqlDataAdapter daMonth = new MySqlDataAdapter(requte, ClassConnexion.Macon);
            daMonth.Fill(ds,table);

            int CountMonths = ds.Tables[table].Rows.Count;

            for (int i = 0; i < CountMonths; i++)
            {
                CheckedListBoxItem Chk = new CheckedListBoxItem();

                VarD = DateTime.Parse(ds.Tables[table].Rows[i][columnDesc].ToString());
                Chk.Description = VarD.ToString(forma);
                Chk.Value = ds.Tables[table].Rows[i][columnvalue].ToString();

                Checkedlist.Items.Add(Chk);
                //if (i == CountMonths - 1)
                //{
                //    Chk.CheckState = CheckState.Checked;
                //}
            }
        }

        private void Paiement_Load(object sender, EventArgs e)
        {
            try
            {
                dv = new DataView();
                ds = new DataSet();

                da = new MySqlDataAdapter("SELECT  adherent.IdAdherent, CONCAT(adherent.NomArAdhe, ' ', adherent.PrenomArAdhe) AS NomC, CONCAT(adherent.NomFrAdhe, ' ', adherent.PrenomFrAdhe) AS NomCFr,CINAdhe,ExiAdhe,case when ExiAdhe='True' then 'نشط' else 'غير نشط' end as ExiAdheMo,case when DeceAdhe='True' then 'على قيد الحياة' else 'متوفى' end as DeceAdheMo,DeceAdhe, compteur.IdComp, compteur.NumComp,siyam,PartsComp, secteur.IdSect, secteur.LibelleSect FROM adherent INNER JOIN compteur ON adherent.IdAdherent = compteur.IdAdherent INNER JOIN secteur ON compteur.IdSect = secteur.IdSect", ClassConnexion.Macon);
                da.Fill(ds, "Compteur");
                dv.Table = ds.Tables["Compteur"];

                ////// Remp Penalite Mois //////

                if (Configuration.Func(23) == "0")
                {
                    if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    {
                        ClassConnexion.Macon.Open();
                    }
                    MySqlCommand CmdSetPenalite = new MySqlCommand("update paiement set PenalitePaie=(select LibEntr from configuration where IdConf=7) where PayePaie=0 and IdFact in (select IdFact from facture where DATEDIFF(DATE_FORMAT(now(),\"%Y-%m-%d\"),DATE_FORMAT(PeriodePaieFFact,\"%Y-%m-%d\"))>1); ", ClassConnexion.Macon);
                    CmdSetPenalite.ExecuteNonQuery();

                    ClassConnexion.Macon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
                      
        }

        int VarCountCb;
        float Montantpayer = 0;
        private void CbSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CESect.Checked == true)
            {
                if (VarCountCb > CbSec.Items.Count - 1)
                {
                    dv.RowFilter = "IdSect=" + CbSec.SelectedValue + " ";
                    Vider();
                }
                VarCountCb++;
            }
        }
        private void CESect_CheckedChanged(object sender, EventArgs e)
        {
            if (CESect.Checked == true)
            {
                dv.RowFilter = "";
                CbSec.Enabled = true;
                CECin.Checked = false;
                CENC.Checked = false;

                Configuration.RempCombo(CbSec, "select * from secteur", "Secteur", "IdSect", "LibelleSect");
                dv.RowFilter = "IdSect=" + CbSec.SelectedValue + " ";
            }
            else
            {
                dv.RowFilter = "";
                CbSec.Enabled = false;
                CbSec.DataSource = null;
            }
        }
        private void CECin_CheckedChanged(object sender, EventArgs e)
        {
            if (CECin.Checked == true)
            {
                dv.RowFilter = "";
                TbCin.Enabled = true;
                CENC.Checked = false;
                CESect.Checked = false;
            }
            else
            {
                TbCin.Text = "";
                TbCin.Enabled = false;

            }
        }
        private void CENC_CheckedChanged(object sender, EventArgs e)
        {
            if (CENC.Checked == true)
            {
                dv.RowFilter = "";
                TbNC.Enabled = true;
                CECin.Checked = false;
                CESect.Checked = false;
            }
            else
            {
                TbNC.Text = "";
                TbNC.Enabled = false;
            }
        }
        private void TbCin_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = "CINAdhe like '" + TbCin.Text + "%' ";
        }
        private void TbNC_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = "NomC like'" + TbNC.Text + "%' ";
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

            try
            {
                indexSR = gridView1.GetSelectedRows()[0];
                if (gridView1.SelectedRowsCount < 0)
                {

                }
                else
                {

                    DataRow row = gridView1.GetDataRow(indexSR);
                    label16.Text = row["NumComp"].ToString();
                    gridControl1.Refresh();
                    TbNum.Text = label16.Text;

                    //label17.Text = row["NomC"].ToString();
                    //label15.Text = row["siyam"].ToString();
                    //label14.Text = row["LibelleSect"].ToString();
                    //if (row["LibelleSect"].ToString() == "True")
                    //{
                    //    label13.Text = "متصل";
                    //}
                    //else
                    //{
                    //    label13.Text = "منفصل";
                    //}
                    //label12.Text = row["PartsComp"].ToString();
                    //label11.Text = row["CINAdhe"].ToString();
                    //if (row["DeceAdhe"].ToString() == "True")
                    //{
                    //    label10.Text = "على قيد الحياة";
                    //}
                    //else
                    //{
                    //    label10.Text = "متوفى";
                    //}

                    // Réfrech GridContOprTableCln


                    if (ClassConnexion.Macon.State == ConnectionState.Open)
                    {
                        ClassConnexion.Macon.Close();
                    }
                    keyDown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        ///// Radio Buttons Checked Recherche
        private void RbAdh_CheckedChanged(object sender, EventArgs e)
        {
            if (RbAdh.Checked == true)
            {
                RbCom.Checked = false;
                Vider();
                RempData();
                groupBoxAdh.Enabled = true;
            }
            else
            {
                groupBoxAdh.Enabled = false;
                gridControl1.DataSource = null;
                CESect.Checked = false;
                CECin.Checked = false;
                CENC.Checked = false;
            }
        }
        private void RbCom_CheckedChanged(object sender, EventArgs e)
        {
            if (RbCom.Checked == true)
            {
                RbAdh.Checked = false;
                gridControl1.DataSource = null;
                Vider();
                groupBoxComp.Enabled = true;
            }
            else
            {
                TbNum.Text = "";
                groupBoxComp.Enabled = false;
            }
        }

        private void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            float TotalMonthsChecked=0;
            foreach (CheckedListBoxItem item in checkedListBoxControl1.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    for (int i = 0; i < ds.Tables["Months"].Rows.Count; i++)
                    {
                        if (ds.Tables["Months"].Rows[i]["IdPaie"].ToString() == item.Value.ToString())
                        {
                            if (Configuration.Func(20)=="1")
                            {
                                float VrbmontantEcl = 0;
                                if (ClassConnexion.Macon.State == ConnectionState.Open)
                                {
                                    ClassConnexion.Macon.Close();
                                }
                                ClassConnexion.Macon.Open();

                                MySqlCommand CmdreturnMontantEcl = new MySqlCommand("select MontantEcl from Eclairage where IdFct=" + int.Parse(ds.Tables["Months"].Rows[i]["IdFact"].ToString()) + " and IdSect=" + int.Parse(ds.Tables["Months"].Rows[i]["IdSect"].ToString()) + " ", ClassConnexion.Macon);
                                dr = CmdreturnMontantEcl.ExecuteReader();
                                while (dr.Read())
                                {
                                    VrbmontantEcl = float.Parse(dr["MontantEcl"].ToString());
                                }
                                dr.Close();
                                ClassConnexion.Macon.Close();

                                TotalMonthsChecked = TotalMonthsChecked + float.Parse(ds.Tables["Months"].Rows[i]["Mtpen"].ToString()) + VrbmontantEcl;
                            }
                            else
                            {
                                TotalMonthsChecked = TotalMonthsChecked + float.Parse(ds.Tables["Months"].Rows[i]["Mtpen"].ToString()) ;
                            }
                        }
                    }
                }
                VarLabelTM = TotalMonthsChecked;
                labelTotaleM.Text = Configuration.ConvertToMony(TotalMonthsChecked);

            }
            CalculeTotale();
        }
        private void checkedListBoxControl2_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            float TotalTraiChecked = 0;
            foreach (CheckedListBoxItem item in checkedListBoxControl2.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    for (int i = 0; i < ds.Tables["Traite"].Rows.Count; i++)
                    {
                        if (ds.Tables["Traite"].Rows[i]["IdMTr"].ToString() == item.Value.ToString())
                        {
                            TotalTraiChecked = TotalTraiChecked + float.Parse(ds.Tables["Traite"].Rows[i]["MontantMTr"].ToString());
                        }
                    }
                }
                VarLabelTT = TotalTraiChecked;
                labelTotaleTr.Text = Configuration.ConvertToMony(TotalTraiChecked);
            }
            CalculeTotale();
        }
        private void checkedListBoxControl3_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            float TotalPenaChecked = 0;
            foreach (CheckedListBoxItem item in checkedListBoxControl3.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    for (int i = 0; i < ds.Tables["Penalite"].Rows.Count; i++)
                    {
                        if (item.Value.ToString() == ds.Tables["Penalite"].Rows[i]["IdPena"].ToString())
                        {
                            TotalPenaChecked = TotalPenaChecked + float.Parse(ds.Tables["Penalite"].Rows[i]["MontantPena"].ToString());
                        }
                    }                    
                }
                VarLabelTP = TotalPenaChecked;
                labelTotalPena.Text = Configuration.ConvertToMony(TotalPenaChecked);
            }
            CalculeTotale();
        }
        private void checkedListBoxControl4_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            float TotalEntrChecked = 0;
            foreach (CheckedListBoxItem item in checkedListBoxControl4.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    for (int i = 0; i < ds.Tables["Entree"].Rows.Count; i++)
                    {
                        if (item.Value.ToString() == ds.Tables["Entree"].Rows[i]["IdAutreF"].ToString())
                        {
                            TotalEntrChecked = TotalEntrChecked + float.Parse(ds.Tables["Entree"].Rows[i]["MontantAutFrai"].ToString());
                        }
                    }
                }
                VarLabelTE = TotalEntrChecked;
                labelTotalEnt.Text = Configuration.ConvertToMony(TotalEntrChecked);
            }
            CalculeTotale();
        }

        private void TbMablegh_TextChanged_1(object sender, EventArgs e)
        {
            CalculReste();
        }
        private void labelMontantTotale_TextChanged(object sender, EventArgs e)
        {
            CalculReste();
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Enregistrer")
            {
                if (VarLabelTotale != 0)
                {
                    try
                    {
                        string strMaxIdListePaie = "";
                        Int64 intMaxIdListePaie = 0;
                        int Lastid = 0;
                        Lastid = Configuration.LastID("listepaiement", "IdLPaie")+1;

                        ClassConnexion.Macon.Open();

                        strMaxIdListePaie = Lastid + DateTime.Now.ToString("ddMMyyyy");
                        intMaxIdListePaie = Int64.Parse(strMaxIdListePaie);

                        Table = new DataTable();
                        Table.Columns.Add("id", typeof(Int32));
                        Table.Columns.Add("NumCom", typeof(Int32));
                        Table.Columns.Add("MT", typeof(double));
                        Table.Columns.Add("date", typeof(string));
                        Table.Columns.Add("penalite", typeof(double));
                        Table.Columns.Add("Typ", typeof(string));

                        MySqlCommand cmdRemplirTListePaie = new MySqlCommand("INSERT INTO listepaiement(IdPaie,DatePaie,NumComp,MPCons,MPTrai,MPEntr,MPPena,MPTota) VALUES (" + intMaxIdListePaie + ",'" + DateTime.Now.ToString(ShortForma) + "'," + NumComp + "," + VarLabelTM + "," + VarLabelTT + "," + VarLabelTE + "," + VarLabelTP + "," + VarLabelTotale + ")", ClassConnexion.Macon);
                        cmdRemplirTListePaie.ExecuteNonQuery();


                        ///////// Months ////
                        if (checkedListBoxControl1.Items.Count > 0)
                        {
                            foreach (CheckedListBoxItem item in checkedListBoxControl1.Items)
                            {
                                if (item.CheckState == CheckState.Checked)
                                {
                                    for (int i = 0; i < ds.Tables["Months"].Rows.Count; i++)
                                    {
                                        if (ds.Tables["Months"].Rows[i]["IdPaie"].ToString() == item.Value.ToString())
                                        {
                                            MySqlCommand CmdUpdatePai = new MySqlCommand("update paiement set PayePaie=1, DatePaie='" + DateTime.Now.ToString(ShortForma) + "' ,IdLP="+Lastid+" where IdPaie=" + int.Parse(ds.Tables["Months"].Rows[i]["IdPaie"].ToString()) + "  ", ClassConnexion.Macon);
                                            CmdUpdatePai.ExecuteNonQuery();
                                            Table.Rows.Add(int.Parse(item.Value.ToString()), int.Parse(NumComp), double.Parse(ds.Tables["Months"].Rows[i]["MontantPaie"].ToString()), item.Description.ToString(), double.Parse(ds.Tables["Months"].Rows[i]["PenalitePaie"].ToString()), 1);
                                        }
                                    }
                                }
                            }
                        }

                        ///////// Traite //////
                        if (checkedListBoxControl2.Items.Count > 0)
                        {
                            int IdTrai = 0;
                            float TotalPourSetResteTrai = 0;
                            foreach (CheckedListBoxItem item in checkedListBoxControl2.Items)
                            {
                                if (item.CheckState == CheckState.Checked)
                                {
                                    for (int i = 0; i < ds.Tables["Traite"].Rows.Count; i++)
                                    {
                                        if (ds.Tables["Traite"].Rows[i]["IdMTr"].ToString() == item.Value.ToString())
                                        {
                                            MySqlCommand CmdUpdateMoisTr = new MySqlCommand(" update moistraite set PayerMTr=1,DatePayerMTr='" + DateTime.Now.ToString(ShortForma) + "',IdLP=" + Lastid + " where IdMTr=" + int.Parse(ds.Tables["Traite"].Rows[i]["IdMTr"].ToString()) + " ", ClassConnexion.Macon);
                                            CmdUpdateMoisTr.ExecuteNonQuery();
                                            Table.Rows.Add(int.Parse(item.Value.ToString()), int.Parse(NumComp), double.Parse(ds.Tables["Traite"].Rows[i]["MontantMTr"].ToString()), item.Description.ToString(), 0, 2);
                                            IdTrai = int.Parse(ds.Tables["Traite"].Rows[i]["IdTraite"].ToString());
                                        }
                                    }
                                }
                            }
                            float TotaleTr = 0;
                            if (VarLabelTT != 0)
                            {
                                TotaleTr = VarLabelTT;
                                MySqlCommand CmdUpdateTraite = new MySqlCommand("update traite set ResteTrai= (ResteTrai-" + TotaleTr + ") where IdTrai=" + IdTrai + " ", ClassConnexion.Macon);
                                CmdUpdateTraite.ExecuteNonQuery();
                            }
                        }


                        ///////// Penalite //////
                        if (checkedListBoxControl3.Items.Count > 0)
                        {
                            float TotalPenaChecked = 0;
                            foreach (CheckedListBoxItem item in checkedListBoxControl3.Items)
                            {
                                if (item.CheckState == CheckState.Checked)
                                {
                                    for (int i = 0; i < ds.Tables["Penalite"].Rows.Count; i++)
                                    {
                                        if (item.Value.ToString() == ds.Tables["Penalite"].Rows[i]["IdPena"].ToString())
                                        {                                  
                                            MySqlCommand CmdUpdatePena = new MySqlCommand("update penalite set PayerPena=1 , DatePayerPena='" + DateTime.Now.ToString(ShortForma) + "',IdLP=" + Lastid + " where IdPena=" + int.Parse(ds.Tables["Penalite"].Rows[i]["IdPena"].ToString()) + " ", ClassConnexion.Macon);
                                            CmdUpdatePena.ExecuteNonQuery();
                                            Table.Rows.Add(int.Parse(item.Value.ToString()), int.Parse(NumComp), double.Parse(ds.Tables["Penalite"].Rows[i]["MontantPena"].ToString()), item.Description.ToString(), 0, 3);
                                        }
                                    }
                                }
                            }
                        }

                        ///////// Entree //////
                        if (checkedListBoxControl4.Items.Count > 0)
                        {
                            float TotalEntrChecked = 0;
                            foreach (CheckedListBoxItem item in checkedListBoxControl4.Items)
                            {
                                if (item.CheckState == CheckState.Checked)
                                {
                                    for (int i = 0; i < ds.Tables["Entree"].Rows.Count; i++)
                                    {
                                        if (item.Value.ToString() == ds.Tables["Entree"].Rows[i]["IdAutreF"].ToString())
                                        {
                                            MySqlCommand CmdUpdateEntr = new MySqlCommand("update autrefrais set PayerEntr=1 , DatePayerEntr='" + DateTime.Now.ToString(ShortForma) + "',IdLP=" + Lastid + " where IdEntr=" + int.Parse(ds.Tables["Entree"].Rows[i]["IdAutreF"].ToString()) + " ", ClassConnexion.Macon);
                                            CmdUpdateEntr.ExecuteNonQuery();
                                            Table.Rows.Add(int.Parse(item.Value.ToString()), int.Parse(NumComp), double.Parse(ds.Tables["Entree"].Rows[i]["MontantAutFrai"].ToString()), item.Description.ToString(),0,4);
                                        }
                                    }
                                }
                            }
                        }



                        ClassConnexion.Macon.Close();
                        Vider();
                        labelReste.Text = "0";

                        ///****************************************************************///

                        DSImpression = new DataSet();

                        Table.TableName = "Table1";
                        DSImpression.Tables.Add(Table);

                        ImpressionRecuPaiement report = new ImpressionRecuPaiement();
                        report.DataSource = DSImpression;
                        report.DataMember = DSImpression.Tables["Table1"].TableName;

                        report.load();
                        report.infocompteur(NomComplete, NumAdhe, NumComp, Contra, IdSecteur, LiSecteur, IdUser);
                        //report.FilterString = "[NumCom] = " + int.Parse(NumComp);
                        report.CreateDocument();
                        //ReportPrintTool pt = new ReportPrintTool(report);
                        report.ShowRibbonPreviewDialog();

                        //*********************************************************************///
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    XtraMessageBox.Show("لاتوجد مستحقات لإستخلاصها ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.Button.Properties.Tag == "Vider")
            {
                Vider();
            }
            else if (e.Button.Properties.Tag== "ListePaie")
            {
                ListePaiement LP = new ListePaiement();
                LP.ShowDialog(this);
            }
        }

        ///////*********  Buttons Form ***********//////
        private void label3_Click(object sender, EventArgs e)
        {
            FormMenu fm = new FormMenu();
            fm.Show();
            this.Close();
        }
        private void panelControl1_Click(object sender, EventArgs e)
        {
            Login LogFORM = new Login();
            LogFORM.Show();
            this.Hide();
        }
        private void panelControl3_Click(object sender, EventArgs e)
        {
            Sleep FSleep = new Sleep();
            FSleep.ShowDialog(this);
        }

        private void TbNum_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (RbCom.Checked == true && TbNum.Text != "")
                {
                    keyDown();
                }
            }
        }
    }
}