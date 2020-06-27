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
using MetroFramework;

namespace DWM
{
    public partial class AjouCompteur : DevExpress.XtraEditors.XtraForm
    {

        int idcom = 0;

        public AjouCompteur()
        {
            InitializeComponent();
        }
        public AjouCompteur(int idcompteur)
        {
            idcom = idcompteur;
            InitializeComponent();        
        }

        DataSet dscompteur;
        MySqlDataAdapter dada;
        MySqlDataReader DR;
        MySqlCommand ModCompteur, Comajouter;

        int compteurnumnc;
        string statutcomp = "متصل";
        string Requete, MsgAn, MsgNv,MsgEn;

        int  siyam, NumComp, PartsComp;
        string secteurmd;

        private void AjouCompteur_Load(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (idcom!=0)
                {
                    label15.Text = "تعديل العداد";

                    AncCompteur.Enabled = false;
                    NouveauCompteur.Enabled = false;

                    dscompteur = new DataSet();

                    dada = new MySqlDataAdapter("select IdAdherent,CINAdhe,concat(NomArAdhe,' ',PrenomArAdhe) as membar from adherent ", ClassConnexion.Macon);
                    dada.Fill(dscompteur, "adherent");

                    lookAdh.Properties.DataSource= dscompteur.Tables["adherent"];
                    lookAdh.Properties.ValueMember = "IdAdherent";
                    lookAdh.Properties.DisplayMember = "membar";
                    lookAdh.Properties.View = gridLookUpEdit1View;

                       
                    //Secteurs
                    dada = new MySqlDataAdapter("select LibelleSect as sect,IdSect from secteur", ClassConnexion.Macon);
                    dada.Fill(dscompteur, "secteur");
                    comboBox1.DataSource = dscompteur.Tables["secteur"];
                    comboBox1.ValueMember = "IdSect";
                    comboBox1.DisplayMember = "sect";

                    lookAdh.Enabled = false;
                   
                    ClassConnexion.Macon.Open();
                    ModCompteur = new MySqlCommand("select compteur.*,secteur.LibelleSect from compteur,secteur where secteur.IdSect=compteur.IdSect and IdComp=" + idcom, ClassConnexion.Macon);
                    DR=ModCompteur.ExecuteReader();
                    DR.Read();

                    comboBox1.SelectedValue = DR["IdSect"].ToString();

                 
                    lookAdh.EditValue = DR["IdAdherent"].ToString();

                    Siyam.Value = int.Parse(DR["siyam"].ToString());
                    NumCompteur.Value= int.Parse(DR["NumComp"].ToString());
                    partcom.Value= int.Parse(DR["PartsComp"].ToString());

                    AncCompteur.Text =  DR["CompteurAn"].ToString();
                    NouveauCompteur.Text = DR["CompteurNv"].ToString();


                    secteurmd = DR["LibelleSect"].ToString();
                    siyam = int.Parse(DR["siyam"].ToString());
                    NumComp= int.Parse(DR["NumComp"].ToString());
                    PartsComp= int.Parse(DR["PartsComp"].ToString());

                    if (!Boolean.Parse(DR["StatutsComp"].ToString()))
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = true;
                    }
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;

                    compteurnumnc = int.Parse(DR["NumComp"].ToString());

                    DR.Close();
                    ClassConnexion.Macon.Close();
                }
                else
                {
                    dscompteur = new DataSet();

                    dada = new MySqlDataAdapter("select IdAdherent,CINAdhe,concat(NomArAdhe,' ',PrenomArAdhe) as membar from adherent where DeceAdhe=1 order by IdAdherent ASC", ClassConnexion.Macon);
                    dada.Fill(dscompteur, "adherent");

                    lookAdh.Properties.DataSource = dscompteur.Tables["adherent"];
                    lookAdh.Properties.ValueMember = "IdAdherent";
                    lookAdh.Properties.DisplayMember = "membar";
                    lookAdh.Properties.View = gridLookUpEdit1View;

                    //Secteurs
                    dada = new MySqlDataAdapter("select LibelleSect as sect,IdSect from secteur", ClassConnexion.Macon);
                    dada.Fill(dscompteur, "secteur");
                    comboBox1.DataSource = dscompteur.Tables["secteur"];
                    comboBox1.ValueMember = "IdSect";
                    comboBox1.DisplayMember = "sect";
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
            finally
            {
                splashScreenManager1.CloseWaitForm();
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Enregistrer")
            {
                if (idcom == 0)
                {
                    splashScreenManager1.ShowWaitForm();

                    int lastid = Configuration.LastID("compteur", "IdComp") + 1;
                    int statcomp = 0;

                    if (radioButton1.Checked)
                        statcomp = 1;

                    String formatdt = "yyyy-MM-dd HH:mm:ss";

                    if (Configuration.ExisteEnre("compteur", "NumComp", NumCompteur.Value.ToString()) != 0)
                    {
                        splashScreenManager1.CloseWaitForm();
                        XtraMessageBox.Show("رقم العداد موجود من قبل المرجو تغييره", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (lookAdh.EditValue=="0")
                        {
                            splashScreenManager1.CloseWaitForm();
                            XtraMessageBox.Show("لم تقم بإختيار المشترك", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (int.Parse(NouveauCompteur.Text.ToString()) >= int.Parse(AncCompteur.Text.ToString()))
                            {
                                if (statcomp==0)
                                    statutcomp = "منفصل";
                                else
                                    statutcomp = "متصل";

                                MsgAn = "";
                                MsgNv = "المشترك : "+lookAdh.Text+ " | الصيام : "+ Siyam.Value+" | الجولة : "+ comboBox1.Text +" | رقم العداد : "+ NumCompteur.Value+" | عدد الأسهم : "+ partcom.Value+" | حالة العداد : "+ statutcomp;
                                MsgEn = "قام "+UserConnecte.NomUser+" "+UserConnecte.PrenomUser+" بإضافة عداد جديد ("+DateTime.Now.ToString(formatdt)+") :";

                                Requete = "insert into compteur values(" + lastid + "," + Siyam.Value + "," + lookAdh.EditValue + "," + comboBox1.SelectedValue + "," + NumCompteur.Value + "," + statcomp + "," + partcom.Value + "," + UserConnecte.IdUser + ",\\'" + DateTime.Now.ToString(formatdt) + "\\',"+AncCompteur.Text+","+NouveauCompteur.Text+ "," + NumApp.Value + ")";
                               
                                Configuration.Historique(1, Requete, MsgAn, MsgNv, MsgEn,"","");
                                Siyam.Value = 0;
                                comboBox1.SelectedIndex = 0;
                                NumCompteur.Value = 0;
                                partcom.Value = 1;
                                NouveauCompteur.Text = "0";
                                AncCompteur.Text = "0";

                                splashScreenManager1.CloseWaitForm();
                                XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                splashScreenManager1.CloseWaitForm();
                                XtraMessageBox.Show("يجب أن يكون العداد الجديد أكبر من القديم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    splashScreenManager1.ShowWaitForm();
                    String formatdt = "yyyy-MM-dd HH:mm:ss";

                    if (compteurnumnc != NumCompteur.Value)
                    {
                        if (Configuration.ExisteEnre("compteur", "NumComp", NumCompteur.Value.ToString()) > 0)
                        {
                            splashScreenManager1.CloseWaitForm();
                            XtraMessageBox.Show("رقم العداد موجود من قبل المرجو تغييره", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MsgAn = "المشترك : " + lookAdh.Text + " | الصيام : " + siyam.ToString() + " | الجولة : " + secteurmd + " | رقم العداد : " + NumComp + " | عدد الأسهم : " + PartsComp.ToString() + " | حالة العداد : " + statutcomp;
                            MsgNv = "المشترك : " + lookAdh.Text + " | الصيام : " + Siyam.Value + " | الجولة : " + comboBox1.Text + " | رقم العداد : " + NumCompteur.Value + " | عدد الأسهم : " + partcom.Value + " | حالة العداد : " + statutcomp;
                            MsgEn = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتغيير معلومات هذا العداد (" + DateTime.Now.ToString(formatdt) + ") :";
                            Requete = "Update compteur set siyam=" + Siyam.Value + ",IdSect=" + comboBox1.SelectedValue + ",NumComp=" + NumCompteur.Value + ",PartsComp=" + partcom.Value + ",DateModComp=\\'" + DateTime.Now.ToString(formatdt) + "\\',NumAppart=" + NumApp.Value + " where IdComp=" + idcom;
                            Configuration.Historique(0, Requete, MsgAn, MsgNv, MsgEn, "", "");

                            splashScreenManager1.CloseWaitForm();
                            XtraMessageBox.Show(" تم التعديل بنجاح", "تعديل العداد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MsgAn = "المشترك : " + lookAdh.Text + " | الصيام : " + siyam.ToString() + " | الجولة : " + secteurmd + " | رقم العداد : " + NumComp + " | عدد الأسهم : " + PartsComp.ToString() + " | حالة العداد : " + statutcomp;
                        MsgNv = "المشترك : " + lookAdh.Text + " | الصيام : " + Siyam.Value + " | الجولة : " + comboBox1.Text + " | رقم العداد : " + NumCompteur.Value + " | عدد الأسهم : " + partcom.Value + " | حالة العداد : " + statutcomp;
                        MsgEn = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتغيير معلومات هذا العداد (" + DateTime.Now.ToString(formatdt) + ") :";
                        Requete = "Update compteur set siyam=" + Siyam.Value + ",IdSect=" + comboBox1.SelectedValue + ",NumComp=" + NumCompteur.Value + ",PartsComp=" + partcom.Value + ",DateModComp=\\'" + DateTime.Now.ToString(formatdt) + "\\',NumAppart=" + NumApp.Value + " where IdComp=" + idcom;
                        Configuration.Historique(0, Requete, MsgAn, MsgNv, MsgEn, "", "");

                        splashScreenManager1.CloseWaitForm();
                        XtraMessageBox.Show(" تم التعديل بنجاح", "تعديل العداد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            else if(e.Button.Properties.Tag == "Annuler")
            {
                   this.DialogResult = DialogResult.Yes;
                  // on quitte le Form2
                  this.Close();
            }
        }

    }
}