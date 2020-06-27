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
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;


namespace DWM
{
    public partial class ConfigImpressionFacture : DevExpress.XtraEditors.XtraForm
    {
        MySqlDataAdapter da;
        DataSet ds;
        string StrReque;
        Int64 NumCompteur = 0;
        int IdFac;
        MySqlDataReader dr;
        Boolean DejeOvriredsFct = false;

        DataTable Table = new DataTable();
        DataSet DsFct;

        public ConfigImpressionFacture()
        {
            InitializeComponent();
        }
        public ConfigImpressionFacture(int IdFact)
        {
            IdFac = IdFact;
            InitializeComponent();

        }
        public ConfigImpressionFacture(int IdC, int IdFact)
        {
            NumCompteur = IdC;
            IdFac = IdFact;

            InitializeComponent();
        }

        public void CalculMtConsommtion(int IdComp,int T_IdAdhe,string T_NomC,int T_Siyam,int T_Part,Int32 T_NumComp,string T_LibSect,float T_CompA,float T_CompN,float T_Comps,string T_Contra, int T_IdSect, int T_NumApp)
        {
            try
            {
                float Reste = 0;
                float quantite = 0;
                int LastSeuilMin = 0;
                int Comp = 0;

                if (ClassConnexion.Macon.State == ConnectionState.Open)
                {
                    ClassConnexion.Macon.Close();
                }
                ClassConnexion.Macon.Open();

                ///// Calcule Count Tranches /////
                MySqlCommand RequeCount = new MySqlCommand("select count(*) as nbr from tranchescopie where  IdTranC >=0  and IdCopie=(select IdCopieTran from facture where IdFact=" + IdFac + ") ", ClassConnexion.Macon);
                dr = RequeCount.ExecuteReader();
                dr.Read();
                int NbrTran = int.Parse(dr["nbr"].ToString());
                dr.Close();

                ///// Calcule quantite Cons //////
                MySqlCommand CmdreturnQuaCons = new MySqlCommand("select Compsdf from consommation where IdComp=" + IdComp + " and IdFact=" + IdFac + " ", ClassConnexion.Macon);
                dr = CmdreturnQuaCons.ExecuteReader();
                while (dr.Read())
                {
                    quantite = float.Parse(dr["Compsdf"].ToString());
                }
                dr.Close();
                Reste = quantite;
                string TypeCalculCons = Configuration.Func(14);

                int j = 0;

                ///// Remplire Table (Table) //////
                if (ClassConnexion.Macon.State == ConnectionState.Open)
                {
                    ClassConnexion.Macon.Close();
                }
                ClassConnexion.Macon.Open();
                MySqlCommand Requetext = new MySqlCommand("select LibelleTranC,SeuilMinTranC,PrixUTranC from tranchescopie,facture where tranchescopie.IdCopie=facture.IdCopieTran and facture.IdFact=" + IdFac + " order by SeuilMinTranC asc", ClassConnexion.Macon);
                dr = Requetext.ExecuteReader();
                while (dr.Read())
                {
                    if (TypeCalculCons == "Facil")
                    {
                        //// Facile
                        Comp++;
                        if (Comp == NbrTran)
                        {
                            Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), Reste, dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect,T_NumApp);
                            Reste = 0;
                            break;
                        }

                        if (Reste >= (float.Parse(dr["SeuilMinTranC"].ToString()) - LastSeuilMin))
                        {
                            Reste = Reste - (float.Parse(dr["SeuilMinTranC"].ToString()) - LastSeuilMin);
                            Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), (float.Parse(dr["SeuilMinTranC"].ToString()) - LastSeuilMin), dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect,T_NumApp);
                            LastSeuilMin = int.Parse(dr["SeuilMinTranC"].ToString());
                        }
                        else
                        {
                            Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), Reste, dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                            Reste = 0;
                        }
                    }
                    else
                    {
                        //// Fort
                        Comp++;
                        if (Comp == NbrTran)
                        {
                            Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), Reste, dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                            Reste = 0;
                            break;
                        }

                        if (quantite <= float.Parse(dr["SeuilMinTranC"].ToString()))
                        {
                            if (j == 0)
                            {
                                Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), quantite, dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                                Reste = 0;
                                j = 1;
                            }
                            else
                            {
                                Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), 0, dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                            }
                        }
                        else
                        {
                            Table.Rows.Add(IdComp, dr["LibelleTranC"].ToString(), 0, dr["PrixUTranC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
            }

        }
        public void Eclairage(int IdComp, int T_IdAdhe, string T_NomC, int T_Siyam, int T_Part, Int32 T_NumComp, string T_LibSect, float T_CompA, float T_CompN, float T_Comps, string T_Contra, int T_IdSect, int T_NumApp)
        {
            try
            {
                float MontantEclai = 0;
                ClassConnexion.Macon.Open();
                MySqlCommand RequeAutreFrai = new MySqlCommand("select * from Eclairage where IdSect=" + T_IdSect + " and IdFct=" + IdFac , ClassConnexion.Macon);
                dr = RequeAutreFrai.ExecuteReader();

                while (dr.Read())
                {
                    MontantEclai = float.Parse(dr["MontantEcl"].ToString());
                }
                Table.Rows.Add(IdComp, " (Escalier/Public) الكهرباء", 1, MontantEclai, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
            }
        }
        public void fraisFact(int IdComp, int T_IdAdhe, string T_NomC, int T_Siyam, int T_Part, Int32 T_NumComp, string T_LibSect, float T_CompA, float T_CompN, float T_Comps, string T_Contra,int T_IdSect, int T_NumApp)
        {
            try
            {
                int Siyam = 0;
                int Parte = 0;
                ClassConnexion.Macon.Open();

                MySqlCommand Requetext = new MySqlCommand("select distinct fraiscopie.* from fraiscopie, facture, consommation where fraiscopie.IdCopie = facture.IdFraisC2 and consommation.IdComp = " + IdComp + "  and facture.IdFact = " + IdFac + " and fraiscopie.configYesNon = 1 order by IdFraisC", ClassConnexion.Macon);
                dr = Requetext.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["IdFrais"].ToString() == "1")
                    {
                        foreach (DataRow item in ds.Tables["TablePrinc"].Rows)
                        {
                            if (item["IdComp"].ToString() == IdComp.ToString())
                            {
                                Siyam = int.Parse(item["siyam"].ToString());
                            }
                        }
                        Table.Rows.Add(IdComp, dr["libellefraiC"].ToString(), Siyam, dr["PrixUFraiC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                    }
                    else if (dr["IdFrais"].ToString() == "2")
                    {
                        foreach (DataRow item in ds.Tables["TablePrinc"].Rows)
                        {
                            if (item["IdComp"].ToString() == IdComp.ToString())
                            {
                                Parte = int.Parse(item["PartsComp"].ToString());
                            }
                        }
                        Table.Rows.Add(IdComp, dr["libellefraiC"].ToString(), Parte, dr["PrixUFraiC"].ToString(), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                    }
                    else
                    {
                        Table.Rows.Add(IdComp, dr["libellefraiC"].ToString(), 1, float.Parse(dr["PrixUFraiC"].ToString()), T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
            }
        }
        public void AutreFact(int IdComp, int T_IdAdhe, string T_NomC, int T_Siyam, int T_Part, Int32 T_NumComp, string T_LibSect, float T_CompA, float T_CompN, float T_Comps, string T_Contra, int T_IdSect, int T_NumApp)
        {
            try
            {
                float MontantAutreF = 0;
                ClassConnexion.Macon.Open();
                MySqlCommand RequeAutreFrai = new MySqlCommand("select * from autrefrais where IdComp=" + IdComp + " and DATE_FORMAT(DateAutFrai,\"%m-%Y\")= DATE_FORMAT((select PeriodeConsoFact from Facture where IdFact=" + IdFac + "),\"%m-%Y\") ", ClassConnexion.Macon);
                dr = RequeAutreFrai.ExecuteReader();

                while (dr.Read())
                {
                    MontantAutreF = MontantAutreF + float.Parse(dr["MontantAutFrai"].ToString());
                }
                if (MontantAutreF == 0)
                {
                    Table.Rows.Add(IdComp, "مبالغ اخرى", 0, MontantAutreF, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                }
                else
                {
                    Table.Rows.Add(IdComp, "مبالغ اخرى", 1, MontantAutreF, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
            }
        }
        public void PaieFact(int IdComp, int T_IdAdhe, string T_NomC, int T_Siyam, int T_Part, Int32 T_NumComp, string T_LibSect, float T_CompA, float T_CompN, float T_Comps, string T_Contra, int T_IdSect, int T_NumApp)
        {
            try
            {
                float MontantPai = 0;
                ClassConnexion.Macon.Open();
                MySqlCommand RequePaieFact = new MySqlCommand("select paiement.* from paiement,facture,consommation where paiement.IdFact = facture.IdFact and paiement.IdCons=consommation.IdCons and consommation.IdComp=" + IdComp + " and paiement.PayePaie = 0 and facture.IdFact <>" + IdFac + " ", ClassConnexion.Macon);
                dr = RequePaieFact.ExecuteReader();
                while (dr.Read())
                {
                    MontantPai = MontantPai + (float.Parse(dr["MontantPaie"].ToString()) + float.Parse(dr["PenalitePaie"].ToString()));
                }

                if (MontantPai == 0)
                {
                    Table.Rows.Add(IdComp, "الديون السابقة", 0, MontantPai, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                }
                else
                {
                    Table.Rows.Add(IdComp, "الديون السابقة", 1, MontantPai, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.Macon.Close();
            }
        }
        public void TraiteFact(int IdComp, int T_IdAdhe, string T_NomC, int T_Siyam, int T_Part, Int32 T_NumComp, string T_LibSect, float T_CompA, float T_CompN, float T_Comps, string T_Contra, int T_IdSect, int T_NumApp)
        {
            try
            {
                float MontantTr = 0;
                ClassConnexion.Macon.Open();
                MySqlCommand RequeTraiFact = new MySqlCommand("select moistraite.* from moistraite,traite where traite.IdTrai = moistraite.IdTrai and traite.IdComp=" + IdComp + " and moistraite.MoisMTr= DATE_FORMAT((select PeriodeConsoFact from Facture where IdFact=" + IdFac + "),\"%m/%Y\")", ClassConnexion.Macon);
                dr = RequeTraiFact.ExecuteReader();
                while (dr.Read())
                {
                    MontantTr = float.Parse(dr["MontantMTr"].ToString());
                }

                if (MontantTr == 0)
                {
                    Table.Rows.Add(IdComp, "الأقساط الشهرية", 0, MontantTr, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                }
                else
                {
                    Table.Rows.Add(IdComp, "الأقساط الشهرية", 1, MontantTr, T_IdAdhe, T_NomC, T_Siyam, T_Part, T_NumComp, T_LibSect, T_CompA, T_CompN, T_Comps, T_Contra, T_IdSect, T_NumApp);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.Macon.Close();
            }
        }

        private void ConfigImpressionFacture_Load(object sender, EventArgs e)
        {
            ds = new DataSet();

            if (IdFac != 0)
            {
                
                if (NumCompteur==0)
                {
                    if (Configuration.Func(20)=="0")
                    {
                        StrReque = "SELECT compteur.IdComp, compteur.siyam, compteur.NumComp, compteur.StatutsComp, compteur.PartsComp, adherent.IdAdherent, CONCAT(adherent.NomArAdhe, ' ', adherent.PrenomArAdhe) AS NomCompl,consommation.ComptageACons, consommation.ComptageNCons, consommation.Compsdf, concat('S',secteur.IdSect,' / ',secteur.LibelleSect) as sect,secteur.IdSect, configuration.LibEntr, consommation.IdAdhCons,compteur.NumAppart  FROM secteur INNER JOIN compteur ON secteur.IdSect = compteur.IdSect INNER JOIN consommation ON compteur.IdComp = consommation.IdComp INNER JOIN facture ON consommation.IdFact = facture.IdFact INNER JOIN adherent ON consommation.IdAdhCons = adherent.IdAdherent, configuration WHERE (facture.IdFact = " + IdFac + ") AND (configuration.IdConf = 13)";
                    }
                    else
                    {
                        StrReque = "SELECT compteur.IdComp, compteur.siyam, compteur.NumComp, compteur.StatutsComp, compteur.PartsComp, adherent.IdAdherent, CONCAT(adherent.NomArAdhe, ' ', adherent.PrenomArAdhe) AS NomCompl,consommation.ComptageACons, consommation.ComptageNCons, consommation.Compsdf, concat('S',secteur.IdSect,' / ',secteur.LibelleSect) as sect,secteur.IdSect, configuration.LibEntr, consommation.IdAdhCons ,eclairage.MontantEcl,compteur.NumAppart  FROM secteur INNER JOIN compteur ON secteur.IdSect = compteur.IdSect INNER JOIN consommation ON compteur.IdComp = consommation.IdComp INNER JOIN facture ON consommation.IdFact = facture.IdFact INNER JOIN adherent ON consommation.IdAdhCons = adherent.IdAdherent, configuration,eclairage WHERE (facture.IdFact = " + IdFac + ") AND (configuration.IdConf = 13) and eclairage.IdFct=Facture.IdFact and compteur.IdSect=eclairage.IdSect";

                    }
                }
                else
                {
                    ChESect.Enabled = false;
                    CbSec.Enabled = false;
                    label1.Enabled = false;

                    if (Configuration.Func(20) == "0")
                    {
                        StrReque = "SELECT compteur.IdComp, compteur.siyam, compteur.NumComp, compteur.StatutsComp, compteur.PartsComp, adherent.IdAdherent, CONCAT(adherent.NomArAdhe, ' ', adherent.PrenomArAdhe) AS NomCompl,consommation.ComptageACons, consommation.ComptageNCons, consommation.Compsdf, concat('S',secteur.IdSect,' / ',secteur.LibelleSect) as sect,secteur.IdSect, configuration.LibEntr, consommation.IdAdhCons,compteur.NumAppart FROM secteur INNER JOIN compteur ON secteur.IdSect = compteur.IdSect INNER JOIN consommation ON compteur.IdComp = consommation.IdComp INNER JOIN facture ON consommation.IdFact = facture.IdFact INNER JOIN adherent ON consommation.IdAdhCons = adherent.IdAdherent, configuration WHERE (facture.IdFact = " + IdFac + ") AND NumComp=" + NumCompteur + " AND (configuration.IdConf = 13)";
                    }
                    else
                    {
                        StrReque = "SELECT compteur.IdComp, compteur.siyam, compteur.NumComp, compteur.StatutsComp, compteur.PartsComp, adherent.IdAdherent, CONCAT(adherent.NomArAdhe, ' ', adherent.PrenomArAdhe) AS NomCompl,consommation.ComptageACons, consommation.ComptageNCons, consommation.Compsdf, concat('S',secteur.IdSect,' / ',secteur.LibelleSect) as sect,secteur.IdSect, configuration.LibEntr, consommation.IdAdhCons ,eclairage.MontantEcl,compteur.NumAppart FROM secteur INNER JOIN compteur ON secteur.IdSect = compteur.IdSect INNER JOIN consommation ON compteur.IdComp = consommation.IdComp INNER JOIN facture ON consommation.IdFact = facture.IdFact INNER JOIN adherent ON consommation.IdAdhCons = adherent.IdAdherent, configuration,eclairage WHERE (facture.IdFact = " + IdFac + ") and NumComp=" + NumCompteur + " AND (configuration.IdConf = 13) and eclairage.IdFct=Facture.IdFact and compteur.IdSect=eclairage.IdSect";

                    }
                }

                MySqlDataAdapter da = new MySqlDataAdapter(StrReque, ClassConnexion.Macon);
                da.Fill(ds, "TablePrinc");
            }

            DsFct = new DataSet();

            Table.Columns.Add("IdComp", typeof(int));
            Table.Columns.Add("Libelle", typeof(string));
            Table.Columns.Add("Quantity", typeof(float));
            Table.Columns.Add("PrixU", typeof(float));
            Table.Columns.Add("IdAdh", typeof(int));
            Table.Columns.Add("NomCompl", typeof(string));
            Table.Columns.Add("siyam", typeof(int));
            Table.Columns.Add("PartsComp", typeof(int));
            Table.Columns.Add("NumComp", typeof(int));
            Table.Columns.Add("LibelleSect", typeof(string));
            Table.Columns.Add("ComptageACons", typeof(float));
            Table.Columns.Add("ComptageNCons", typeof(float));
            Table.Columns.Add("Compsdf", typeof(float));
            Table.Columns.Add("LibEntr", typeof(string));
            Table.Columns.Add("IdSect", typeof(int));
            Table.Columns.Add("NumAppart", typeof(int));



            ///// remp Combo box Secteur //////
            Configuration.RempCombo(CbSec, "select concat('S',IdSect,' / ',LibelleSect) as sect,IdSect from secteur", "Secteur", "IdSect", "sect");
           
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Imprimer")
                {

                    foreach (DataRow item in ds.Tables["TablePrinc"].Rows)
                    {
                        CalculMtConsommtion(int.Parse(item["IdComp"].ToString()), int.Parse(item["IdAdherent"].ToString()), item["NomCompl"].ToString(), int.Parse(item["siyam"].ToString()), Int32.Parse(item["PartsComp"].ToString()), int.Parse(item["NumComp"].ToString()), item["sect"].ToString(), float.Parse(item["ComptageACons"].ToString()), float.Parse(item["ComptageNCons"].ToString()), float.Parse(item["Compsdf"].ToString()), item["LibEntr"].ToString(), int.Parse(item["IdSect"].ToString()), int.Parse(item["NumAppart"].ToString()));
                        if (Configuration.Func(20)=="1")
                        {
                            Eclairage(int.Parse(item["IdComp"].ToString()), int.Parse(item["IdAdherent"].ToString()), item["NomCompl"].ToString(), int.Parse(item["siyam"].ToString()), Int32.Parse(item["PartsComp"].ToString()), int.Parse(item["NumComp"].ToString()), item["sect"].ToString(), float.Parse(item["ComptageACons"].ToString()), float.Parse(item["ComptageNCons"].ToString()), float.Parse(item["Compsdf"].ToString()), item["LibEntr"].ToString(), int.Parse(item["IdSect"].ToString()), int.Parse(item["NumAppart"].ToString()));
                        }
                        fraisFact(int.Parse(item["IdComp"].ToString()), int.Parse(item["IdAdherent"].ToString()), item["NomCompl"].ToString(), int.Parse(item["siyam"].ToString()), Int32.Parse(item["PartsComp"].ToString()), int.Parse(item["NumComp"].ToString()), item["sect"].ToString(), float.Parse(item["ComptageACons"].ToString()), float.Parse(item["ComptageNCons"].ToString()), float.Parse(item["Compsdf"].ToString()), item["LibEntr"].ToString(), int.Parse(item["IdSect"].ToString()), int.Parse(item["NumAppart"].ToString()));
                        if (Configuration.Func(22)=="1")
                        {
                            AutreFact(int.Parse(item["IdComp"].ToString()), int.Parse(item["IdAdherent"].ToString()), item["NomCompl"].ToString(), int.Parse(item["siyam"].ToString()), Int32.Parse(item["PartsComp"].ToString()), int.Parse(item["NumComp"].ToString()), item["sect"].ToString(), float.Parse(item["ComptageACons"].ToString()), float.Parse(item["ComptageNCons"].ToString()), float.Parse(item["Compsdf"].ToString()), item["LibEntr"].ToString(), int.Parse(item["IdSect"].ToString()), int.Parse(item["NumAppart"].ToString()));
                        }
                        PaieFact(int.Parse(item["IdComp"].ToString()), int.Parse(item["IdAdherent"].ToString()), item["NomCompl"].ToString(), int.Parse(item["siyam"].ToString()), Int32.Parse(item["PartsComp"].ToString()), int.Parse(item["NumComp"].ToString()), item["sect"].ToString(), float.Parse(item["ComptageACons"].ToString()), float.Parse(item["ComptageNCons"].ToString()), float.Parse(item["Compsdf"].ToString()), item["LibEntr"].ToString(), int.Parse(item["IdSect"].ToString()), int.Parse(item["NumAppart"].ToString()));
                        if (Configuration.Func(21) == "1")
                        {
                            TraiteFact(int.Parse(item["IdComp"].ToString()), int.Parse(item["IdAdherent"].ToString()), item["NomCompl"].ToString(), int.Parse(item["siyam"].ToString()), Int32.Parse(item["PartsComp"].ToString()), int.Parse(item["NumComp"].ToString()), item["sect"].ToString(), float.Parse(item["ComptageACons"].ToString()), float.Parse(item["ComptageNCons"].ToString()), float.Parse(item["Compsdf"].ToString()), item["LibEntr"].ToString(), int.Parse(item["IdSect"].ToString()), int.Parse(item["NumAppart"].ToString()));
                        }
                    }

                    if (DejeOvriredsFct == false)
                    {
                        DsFct.Tables.Add(Table);
                        //MessageBox.Show("idComp==>"+ Table.Rows[i][0].ToString() + " | Libelle==>" + Table.Rows[i][1].ToString() + " | Quntity==>" + Table.Rows[i][2].ToString() + " |==>Prix" + Table.Rows[i][3].ToString());
                    }
                    DejeOvriredsFct = true;

                    impressionFactures ReportFct = new impressionFactures();
                    ReportFct.DataSource = DsFct;
                    ReportFct.DataMember = DsFct.Tables[0].TableName;
                    if (ChESect.Checked == true)
                    {
                        ReportFct.FilterString = "[IdSect]=" + int.Parse(CbSec.SelectedValue.ToString());
                    }

                    ReportFct.load(IdFac);

                    ReportFct.CreateDocument();
                    ReportFct.ShowPreviewDialog();

                    Table.Clear();
                }
                else if (e.Button.Properties.Tag == "Annuler")
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void ChESect_CheckedChanged(object sender, EventArgs e)
        {
            if (ChESect.Checked == true)
            {
                CbSec.Enabled = true;
            }
            else
            {
                CbSec.Enabled = false;
            }
        }

    }
}