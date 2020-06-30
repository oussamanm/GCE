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
using System.IO;
using System.Collections.ObjectModel;
using System.Xml;
using DevExpress.XtraReports.UI;

namespace DWM
{
    public partial class ConsommationMensuelle : DevExpress.XtraEditors.XtraForm
    {
        public ConsommationMensuelle()
        {
            InitializeComponent();
            controleHeader1.label_menu_btn = "الصفحة الرئيسية / الإستهلاك الشهري";
        }

        MySqlCommand AJOUTERFACT,CMDRECH, ANCOMP, CMDLAFAC, CMDMTFACT, DATEFACT, NBRCOM, NBRCONSO, CHRTIMAM, AUTREFRAIS, FRAIS, MTPARTICIPATION, AJOUCONSOM, AJOUPAIEMENT;
        MySqlDataAdapter COMXML;

        MySqlDataReader DR, DR2, DR3, DR4, DR6, DR7, DR8, DR9, DR10, DR11, DR12, DR13;
        MySqlDataAdapter DAdh;
        DataSet DS;

        int numcmp, dercomp = 0, nvcomp = 0, idComp, quantite = 0, testex = 0, testcomp = 0, testmtcons = 0, syam = 0, sahm = 1, idcompteur = 0, IdAdh = 0;
        string datefa = "", dqtefqctfichier;
        int IdFact = 0, monthfa = 0, yeafa = 0;

        string nomar = "------", libellesect = "------";
        int lasidfact, idcons;
        Boolean br = false;

        float MtPy = 0;
        int NbrDef = int.Parse(Configuration.Func(24));
        Boolean ExistEnrg = false;


        /////******* Void
        private void clear()
        {
            label9.Text = "------";
            label10.Text = "------";
            label11.Text = "------";
            label15.Text = "------";
            label14.Text = "------";
            NouveauCompteur.Text = "";
            NumCompteur.Text = "";
            NumCompteur.Select();
        }
        private void prog()
        {
            /// Fill Progress Bar

            int totalcomp = 0, totalcons = 0, valeur = 0;
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            NBRCOM = new MySqlCommand("select count(*) as nbr from compteur where StatutsComp=1", ClassConnexion.Macon);
            DR7 = NBRCOM.ExecuteReader();
            DR7.Read();

            if (DR7.HasRows)
                totalcomp = int.Parse(DR7["nbr"].ToString());

            DR7.Close();
            ClassConnexion.Macon.Close();


            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            NBRCONSO = new MySqlCommand("select count(*) as nbr from consommation,facture where consommation.IdFact=facture.IdFact and facture.IdFact=(select Max(IdFact) from facture)", ClassConnexion.Macon);
            DR8 = NBRCONSO.ExecuteReader();
            DR8.Read();

            if (DR8.HasRows)
                totalcons = int.Parse(DR8["nbr"].ToString());
            DR8.Close();
            ClassConnexion.Macon.Close();
            valeur = totalcons * 100 / totalcomp;

            progressBarControl1.Text = valeur.ToString();

        }
        private void load()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                DATEFACT = new MySqlCommand("select  DATE_FORMAT(PeriodeConsoFact, '%Y') as yearfa,DATE_FORMAT(PeriodeConsoFact, '%m') as monthfa,DATE_FORMAT(PeriodeConsoFact, '%m/%Y') as datet,DATE_FORMAT(PeriodeConsoFact, '%m-%Y') as dqtefqctfichier,IdFact,DATE_FORMAT(PeriodePaieDFact, '%d/%m/%Y') as debd,DATE_FORMAT(PeriodePaieFFact, '%d/%m/%Y') as findate  from facture  order by IdFact DESC Limit 0,1", ClassConnexion.Macon);
                DR6 = DATEFACT.ExecuteReader();
                DR6.Read();

                if (DR6.HasRows)
                {
                    ExistEnrg = true;

                    IdFact = int.Parse(DR6["IdFact"].ToString());
                    datefa = DR6["datet"].ToString();
                    monthfa = int.Parse(DR6["monthfa"].ToString());
                    yeafa = int.Parse(DR6["yearfa"].ToString());
                    dqtefqctfichier = DR6["dqtefqctfichier"].ToString();

                    label19.Text = DR6["debd"].ToString();
                    label21.Text = DR6["findate"].ToString();
                }
                else
                {
                    ExistEnrg = false;
                    DateTime Dtnow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    monthfa = Dtnow.Month;
                    yeafa = Dtnow.Year;
                }

                DR6.Close();
                ClassConnexion.Macon.Close();

                lasidfact = IdFact;
                label17.Text = datefa.ToString();

                prog();
                NumCompteur.Select();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                splashScreenManager1.CloseWaitForm();
            }
        }


        /////******* Load
        private void ConsommationMensuelle_Load(object sender, EventArgs e)
        {
            load();
        }

        /////******* Btn Menu
        private void BtLsCons_Click(object sender, EventArgs e)
        {
            S_ListeConso ListeCons = new S_ListeConso();
            ListeCons.ShowDialog(this);
        }
        private void BtLsSuiCon_Click(object sender, EventArgs e)
        {
            SuivieConsommtion FSleep = new SuivieConsommtion(IdFact);
            FSleep.ShowDialog(this);

            load();
        }
        private void BtnLsPeriod_Click(object sender, EventArgs e)
        {
            ListePeriodeConsommation LPC = new ListePeriodeConsommation();
            LPC.ShowDialog(this);
        }

        //// Imprimer Liste de Compteur
        private void BtPrintLsDon_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                DS = new DataSet();
                DAdh = new MySqlDataAdapter("select * from ListeCompteurDonnee", ClassConnexion.Macon);
                DAdh.Fill(DS, "ListeCompteurDonnee");

                ListeDonneesCompteurs Report = new DWM.ListeDonneesCompteurs();
                Report.DataSource = DS;
                Report.Load();
                splashScreenManager1.CloseWaitForm();

                Report.ShowRibbonPreview();
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
        }

        //////****** Code

        //// Ajouter une facture
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DialogResult DR = XtraMessageBox.Show("هل تريد فعلا اضافة فاتورة ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DR == DialogResult.Yes)
            {
                DateTime dateD, dateF, datePerio, datesys;

                string CommandeInsert = "";
                datesys = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                datePerio = new DateTime(yeafa, monthfa, 01);

                int iduser = int.Parse(UserConnecte.IdUser.ToString());
                int lastid = Configuration.LastID("fraiscopie", "IdCopie");
                int lastidCtr = Configuration.LastID("tranchescopie", "IdCopie");

                if (ExistEnrg == true)
                {              
                    if (datePerio > datesys || datePerio.ToString("MM-yyyy") == datesys.ToString("MM-yyyy"))
                    {
                        XtraMessageBox.Show("لا يمكنك إظافة الفاتورة حاليا ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        datePerio = datePerio.AddMonths(1);
                        dateD = new DateTime(yeafa, monthfa, (int.Parse(Configuration.Func(3))));
                        dateD = dateD.AddMonths(NbrDef);
                        dateF = dateD.AddDays(int.Parse(Configuration.Func(4)) - 1);
                        CommandeInsert = "insert into facture(IdFraisC2,IdUser,PeriodeConsoFact,PeriodePaieDFact,PeriodePaieFFact,IdCopieTran) values(" + lastid + "," + iduser + ",'" + datePerio.ToString("yyyy-MM-dd") + "','" + dateD.ToString("yyyy-MM-dd") + "','" + dateF.ToString("yyyy-MM-dd") + "'," + lastidCtr + ")";
                    }
                }
                else
                {
                    dateD = new DateTime(yeafa, monthfa, (int.Parse(Configuration.Func(3))));
                    dateD = dateD.AddMonths(NbrDef);
                    dateF = dateD.AddDays(int.Parse(Configuration.Func(4)) - 1);
                    CommandeInsert = "insert into facture(IdFraisC2,IdUser,PeriodeConsoFact,PeriodePaieDFact,PeriodePaieFFact,IdCopieTran) values(" + lastid + "," + iduser + ",'" + datePerio.ToString("yyyy-MM-dd") + "','" + dateD.ToString("yyyy-MM-dd") + "','" + dateF.ToString("yyyy-MM-dd") + "'," + lastidCtr + ")";
                }

                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                AJOUTERFACT = new MySqlCommand(CommandeInsert, ClassConnexion.Macon);
                AJOUTERFACT.ExecuteNonQuery();
                ClassConnexion.Macon.Clone();

                prog();
                NumCompteur.Select();
                this.Refresh();
                load();
            }
        }
  
        private void NouveauCompteur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (NouveauCompteur.Text != "")
                {
                    if (dercomp <= int.Parse(NouveauCompteur.Text))
                    {
                        e.SuppressKeyPress = true;

                        float chart = 0, autresfr = 0, fraismt = 0, mtpart = 0, totalfinal = 0;

                        // chart imam
                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        CHRTIMAM = new MySqlCommand("select * from  frais where IdFrai=1 and configYesNon=1", ClassConnexion.Macon);
                        DR9 = CHRTIMAM.ExecuteReader();
                        DR9.Read();
                        if (DR9.HasRows)
                            chart = float.Parse(DR9["PrixUFrai"].ToString());
                        else
                            chart = 0;
                        chart = chart * syam;
                        DR9.Close();
                        ClassConnexion.Macon.Close();

                        //Fin chart

                        //Autres frais

                        //if (ClassConnexion.Macon.State == ConnectionState.Closed)
                        //    ClassConnexion.Macon.Open();

                        //AUTREFRAIS = new MySqlCommand("select * from autrefrais,compteur where compteur.IdComp=autrefrais.IdComp and compteur.NumComp=" + numcmp + " and DATE_FORMAT(DateAutFrai, '%m/%Y')='" + datefa + "'", ClassConnexion.Macon);
                        //DR10 = AUTREFRAIS.ExecuteReader();
                        //DR10.Read();
                        //if (DR10.HasRows)
                        //    autresfr = float.Parse(DR10["MontantAutFrai"].ToString());
                        //else
                        //    autresfr = 0;

                        //DR10.Close();
                        //ClassConnexion.Macon.Close();

                        // Fin autre frais


                        //Frais

                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        FRAIS = new MySqlCommand("select * from frais where IdFrai>2 and configYesNon=1", ClassConnexion.Macon);
                        DR11 = FRAIS.ExecuteReader();

                        while (DR11.Read())
                        {
                            fraismt = fraismt + float.Parse(DR11["PrixUFrai"].ToString());
                        }

                        DR11.Close();
                        ClassConnexion.Macon.Close();

                        //Fin Frais

                        //MT Participation

                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        MTPARTICIPATION = new MySqlCommand("select * from  frais where IdFrai=2 and configYesNon=1", ClassConnexion.Macon);
                        DR12 = MTPARTICIPATION.ExecuteReader();
                        DR12.Read();
                        if (DR12.HasRows)
                            mtpart = float.Parse(DR12["PrixUFrai"].ToString());
                        else
                            mtpart = 0;

                        mtpart = mtpart * sahm;
                        DR12.Close();
                        ClassConnexion.Macon.Close();

                        totalfinal = mtpart + fraismt + /*autresfr + */  chart + Configuration.CalculMtConsommtion(int.Parse(NouveauCompteur.Text) - dercomp);


                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        AJOUCONSOM = new MySqlCommand("insert into consommation(IdFact,IdComp,IdUser,ComptageACons,ComptageNCons,Compsdf,IdAdhCons) values(" + IdFact + "," + idcompteur + "," + UserConnecte.IdUser + "," + dercomp + "," + NouveauCompteur.Text + "," + (int.Parse(NouveauCompteur.Text) - dercomp) + "," + IdAdh + ")", ClassConnexion.Macon);
                        AJOUCONSOM.ExecuteNonQuery();
                        ClassConnexion.Macon.Close();

                        int lastconid = Configuration.LastID("consommation", "IdCons");

                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        AJOUPAIEMENT = new MySqlCommand("insert into paiement(IdCons,IdFact,IdUser,MontantPaie,PenalitePaie,PayePaie) values(" + lastconid + "," + IdFact + "," + UserConnecte.IdUser + "," + totalfinal + ",0,0)", ClassConnexion.Macon);
                        AJOUPAIEMENT.ExecuteNonQuery();
                        ClassConnexion.Macon.Close();

                        NouveauCompteur.Text = "";
                        NouveauCompteur.Enabled = false;

                        label14.Text = totalfinal.ToString();

                        XtraMessageBox.Show("تمت الإضافة بنجاح ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        prog();
                        clear();
                    }
                    else
                        XtraMessageBox.Show("العداد الجديد الذي تم إذخاله غير مناسب ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    XtraMessageBox.Show("يجب ادخال قيمة العداد الجديد ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                /* -- FIN key Entrer --*/
            }
        }
        private void NumCompteur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (NumCompteur.Text != "")
                    {
                        label9.Text = "------";
                        label10.Text = "------";
                        label11.Text = "------";
                        label15.Text = "------";
                        label14.Text = "------";

                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        CMDRECH = new MySqlCommand("select * from compteur,adherent,secteur where compteur.IdAdherent=adherent.IdAdherent and secteur.IdSect=compteur.IdSect AND compteur.NumComp=" + NumCompteur.Text + " and StatutsComp=1", ClassConnexion.Macon);

                        DR = CMDRECH.ExecuteReader();
                        DR.Read();

                        if (DR.HasRows)
                        {
                            e.SuppressKeyPress = true;
                            numcmp = int.Parse(NumCompteur.Text);
                            syam = int.Parse(DR["siyam"].ToString());
                            sahm = int.Parse(DR["PartsComp"].ToString());
                            dercomp = int.Parse(DR["CompteurNv"].ToString());
                            br = true;
                            testex = 1;
                            idComp = int.Parse(DR["IdComp"].ToString());
                            idcompteur = int.Parse(DR["IdComp"].ToString());
                            IdAdh = int.Parse(DR["IdAdherent"].ToString());
                            nomar = DR["NomArAdhe"].ToString() + " " + DR["PrenomArAdhe"].ToString();
                            libellesect = DR["LibelleSect"].ToString();

                        }
                        else
                        {
                            testex = 0;
                            br = false;
                            nomar = "------";
                            libellesect = "------";
                            XtraMessageBox.Show("رقم العداد الذي تم إدخاله غير موجود ", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        DR.Close();

                        if (testex == 1)
                        {
                            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                ClassConnexion.Macon.Close();

                            CMDLAFAC = new MySqlCommand("select * from consommation where IdComp=" + idComp + " and IdFact=" + lasidfact + "", ClassConnexion.Macon);
                            DR2 = CMDLAFAC.ExecuteReader();
                            DR2.Read();

                            if (DR2.HasRows)
                            {
                                br = false;
                                testcomp = 1;
                                testmtcons = 1;
                                nvcomp = int.Parse(DR2["ComptageNCons"].ToString());
                                quantite = int.Parse(DR2["Compsdf"].ToString());
                                idcons = int.Parse(DR2["IdCons"].ToString());
                            }
                            else
                            {
                                br = true;
                                testcomp = 0;
                                quantite = 0;
                                testmtcons = 0;
                            }
                            DR2.Close();
                        }

                        if (testmtcons == 1)
                        {
                            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                ClassConnexion.Macon.Open();

                            CMDMTFACT = new MySqlCommand("select * from paiement where IdCons=" + idcons, ClassConnexion.Macon);
                            DR4 = CMDMTFACT.ExecuteReader();
                            DR4.Read();

                            if (DR4.HasRows)
                                MtPy = float.Parse(DR4["MontantPaie"].ToString());
                            DR4.Close();
                        }
                        else
                            MtPy = 0;

                        if (testcomp == 1)
                        {
                            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                ClassConnexion.Macon.Open();

                            ANCOMP = new MySqlCommand("select * from consommation where IdComp=" + idComp + " order by IdCons DESC Limit 0,1", ClassConnexion.Macon);
                            DR3 = ANCOMP.ExecuteReader();
                            DR3.Read();

                            if (DR3.HasRows)
                            {
                                NouveauCompteur.Enabled = true;
                                testcomp = 1;
                                dercomp = int.Parse(DR3["ComptageNCons"].ToString());
                                nvcomp = 0;
                            }
                            else
                                testcomp = 0;

                            DR3.Close();

                        }

                        label9.Text = nomar;
                        label10.Text = libellesect;
                        label11.Text = dercomp.ToString();
                        NouveauCompteur.Enabled = br;
                        label15.Text = quantite.ToString();
                        label14.Text = MtPy.ToString();
                    }
                    else
                        XtraMessageBox.Show("يجب ادخال رقم العداد ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
            }
        }

        ///////**** Impo & Expo    
        //exportation   
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "إستيراد الملف|*.XML";
                saveFileDialog1.FileName = dqtefqctfichier;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    splashScreenManager1.ShowWaitForm();

                    if (ClassConnexion.Macon.State == ConnectionState.Closed)
                        ClassConnexion.Macon.Open();

                    COMXML = new MySqlDataAdapter("select c.CompteurNv as nvcomp,a.IdAdherent as idadhr,c.PartsComp as part,c.siyam as siya,c.IdComp as idcomp,concat(NomArAdhe,' ',PrenomArAdhe) as nomcom,c.NumComp as numcom,s.LibelleSect as sect from adherent a,secteur s,compteur c where a.IdAdherent=c.IdAdherent and c.IdSect=s.IdSect and c.StatutsComp=1", ClassConnexion.Macon);
                    DataSet DS = new DataSet();
                    COMXML.Fill(DS, "dataset");

                    string lastcon = "0";


                    StreamWriter Fichierxml = new StreamWriter(saveFileDialog1.FileName);
                    Fichierxml.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
                    Fichierxml.Write("<Compteurs>\r\n");

                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {

                        //** code was deleted
                        //**

                        Fichierxml.Write("<Compteur saisir='0' IdAdh='" + DS.Tables[0].Rows[i]["idadhr"].ToString() + "' NomPreAdh='" + DS.Tables[0].Rows[i]["nomcom"].ToString() + "' IdCompteur='" + DS.Tables[0].Rows[i]["idcomp"].ToString() + "'  sahm='" + DS.Tables[0].Rows[i]["part"].ToString() + "'  NumeroCompteur='" + DS.Tables[0].Rows[i]["numcom"].ToString() + "' DerniereConsomation='" + DS.Tables[0].Rows[i]["nvcomp"].ToString() + "' NouveauConsomation='0' Quantite='0' Sectreur='" + DS.Tables[0].Rows[i]["sect"].ToString() + "' Siyam='" + DS.Tables[0].Rows[i]["siya"].ToString() + "'  Idfacture='" + IdFact + "' PeriodeConsomation='" + dqtefqctfichier + "' ></Compteur>\r\n");

                        //  DR13.Close();
                    }

                    Fichierxml.Write("</Compteurs>\r\n");
                    Fichierxml.Close();

                    ClassConnexion.Macon.Close();
                    splashScreenManager1.CloseWaitForm();
                    XtraMessageBox.Show("تم الإستيراد بنجاح ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }


        }
        //exportation
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fichierxml = new OpenFileDialog();
            if (fichierxml.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    splashScreenManager1.ShowWaitForm();

                    XmlDocument fichier = new XmlDocument();
                    fichier.Load(fichierxml.FileName);

                    XmlNodeList Listecompteur = fichier.GetElementsByTagName("Compteur");

                    foreach (XmlNode Compteur in Listecompteur)
                    {

                        if (int.Parse(Compteur.Attributes["saisir"].Value) != 0)
                        {


                            if (ClassConnexion.Macon.State == ConnectionState.Open)
                            {
                                ClassConnexion.Macon.Close();
                                ClassConnexion.Macon.Open();
                            }
                            else
                            {
                                ClassConnexion.Macon.Open();
                            }



                            CMDLAFAC = new MySqlCommand("select * from consommation where IdComp=" + Compteur.Attributes["IdCompteur"].Value + " and IdFact=" + lasidfact + "", ClassConnexion.Macon);
                            DR2 = CMDLAFAC.ExecuteReader();
                            DR2.Read();

                            if (!DR2.HasRows)
                            {
                                float chart = 0, autresfr = 0, fraismt = 0, mtpart = 0, totalfinal = 0;

                                // Traite
                                // Fin Traite


                                // chart imam

                                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                {
                                    ClassConnexion.Macon.Open();
                                }


                                CHRTIMAM = new MySqlCommand("select * from  frais where IdFrai=1 and configYesNon=1", ClassConnexion.Macon);
                                DR9 = CHRTIMAM.ExecuteReader();
                                DR9.Read();
                                if (DR9.HasRows)
                                {
                                    chart = float.Parse(DR9["PrixUFrai"].ToString());
                                }
                                else
                                {
                                    chart = 0;
                                }

                                chart = chart * int.Parse(Compteur.Attributes["Siyam"].Value);

                                DR9.Close();
                                ClassConnexion.Macon.Close();

                                //Fin chart


                                //Autres frais
                                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                {
                                    ClassConnexion.Macon.Open();
                                }


                                AUTREFRAIS = new MySqlCommand("select * from autrefrais,compteur where compteur.IdComp=autrefrais.IdComp and compteur.NumComp=" + Compteur.Attributes["NumeroCompteur"].Value + " and DATE_FORMAT(DateAutFrai, '%m/%Y')='" + datefa + "'", ClassConnexion.Macon);
                                DR10 = AUTREFRAIS.ExecuteReader();
                                DR10.Read();
                                if (DR10.HasRows)
                                {

                                    autresfr = float.Parse(DR10["MontantAutFrai"].ToString());
                                }
                                else
                                {
                                    autresfr = 0;
                                }


                                DR10.Close();
                                ClassConnexion.Macon.Close();

                                // Fin autre frais



                                //Frais

                                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                {
                                    ClassConnexion.Macon.Open();
                                }


                                FRAIS = new MySqlCommand("select * from frais where IdFrai>2 and configYesNon=1", ClassConnexion.Macon);
                                DR11 = FRAIS.ExecuteReader();

                                while (DR11.Read())
                                {
                                    fraismt = fraismt + float.Parse(DR11["PrixUFrai"].ToString());
                                }


                                DR11.Close();
                                ClassConnexion.Macon.Close();

                                //Fin Frais

                                //MT Participation

                                if (ClassConnexion.Macon.State == ConnectionState.Open)
                                {
                                    ClassConnexion.Macon.Close();
                                    ClassConnexion.Macon.Open();
                                }
                                else
                                {
                                    ClassConnexion.Macon.Open();
                                }


                                MTPARTICIPATION = new MySqlCommand("select * from  frais where IdFrai=2 and configYesNon=1", ClassConnexion.Macon);
                                DR12 = MTPARTICIPATION.ExecuteReader();
                                DR12.Read();
                                if (DR12.HasRows)
                                {
                                    mtpart = float.Parse(DR12["PrixUFrai"].ToString());
                                }
                                else
                                {
                                    mtpart = 0;
                                }

                                mtpart = mtpart * int.Parse(Compteur.Attributes["sahm"].Value);

                                DR12.Close();
                                ClassConnexion.Macon.Close();

                                int qua = int.Parse(Compteur.Attributes["Quantite"].Value);

                                totalfinal = mtpart + fraismt + autresfr + chart + Configuration.CalculMtConsommtion(qua);




                                if (ClassConnexion.Macon.State == ConnectionState.Open)
                                {
                                    ClassConnexion.Macon.Close();
                                    ClassConnexion.Macon.Open();
                                }
                                else
                                {
                                    ClassConnexion.Macon.Open();
                                }

                                AJOUCONSOM = new MySqlCommand("insert into consommation(IdFact,IdComp,IdUser,ComptageACons,ComptageNCons,Compsdf,IdAdhCons) values(" + IdFact + "," + Compteur.Attributes["IdCompteur"].Value + "," + UserConnecte.IdUser + "," + Compteur.Attributes["DerniereConsomation"].Value + "," + Compteur.Attributes["NouveauConsomation"].Value + "," + Compteur.Attributes["Quantite"].Value + "," + Compteur.Attributes["IdAdh"].Value + ")", ClassConnexion.Macon);
                                AJOUCONSOM.ExecuteNonQuery();
                                ClassConnexion.Macon.Close();

                                int lastconid = Configuration.LastID("consommation", "IdCons");


                                if (ClassConnexion.Macon.State == ConnectionState.Open)
                                {
                                    ClassConnexion.Macon.Close();
                                    ClassConnexion.Macon.Open();
                                }
                                else
                                {
                                    ClassConnexion.Macon.Open();
                                }
                                AJOUPAIEMENT = new MySqlCommand("insert into paiement(IdCons,IdFact,IdUser,MontantPaie,PenalitePaie,PayePaie) values(" + lastconid + "," + IdFact + "," + UserConnecte.IdUser + "," + totalfinal + ",0,0)", ClassConnexion.Macon);
                                AJOUPAIEMENT.ExecuteNonQuery();
                                ClassConnexion.Macon.Close();

                                prog();

                            }


                        }

                        /* Fin foreach*/
                    }

                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.ToString());
                }
                finally
                {
                    splashScreenManager1.CloseWaitForm();
                    XtraMessageBox.Show("تم التصدير بنجاح ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //MessageBox.Show("Chemain : "+fichierxml.FileName);
            }
        }

    }
}