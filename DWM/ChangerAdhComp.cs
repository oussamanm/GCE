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
    public partial class ChangerAdhComp : DevExpress.XtraEditors.XtraForm
    {
        int id;

        public ChangerAdhComp(int idcompteue)
        {
            InitializeComponent();
            id = idcompteue;

        }

        DataSet dscompteur;
        MySqlDataAdapter dada;
        MySqlDataReader DR;

        MySqlCommand Comajouter, Comselect,comupdate,tranajou;

        int idadh,partcompteur, Siyaman=0;
        string Requete, MsgAn, MsgNv, MsgEn, secteuran, adhan,requetesupp1, requetesupp2;

        private void ChangerAdhComp_Load(object sender, EventArgs e)
        {
            descritran.BackColor = Color.FromArgb(255, 255, 255);
            try
            {

                splashScreenManager1.ShowWaitForm();
                dscompteur = new DataSet();

                ClassConnexion.Macon.Open();
                Comselect = new MySqlCommand("select concat(NomArAdhe,' ',PrenomArAdhe,' | ',CINAdhe,' | ',compteur.IdAdherent) as memb,concat(NomArAdhe,' ',PrenomArAdhe) as nompre,compteur.IdAdherent as idadhi,NumComp,IdSect,siyam,PartsComp,StatutsComp,compteur.IdComp from compteur,adherent where compteur.IdAdherent=adherent.IdAdherent and compteur.IdComp=" + id, ClassConnexion.Macon);
                DR = Comselect.ExecuteReader();

                DR.Read();


                adhanc.Text=DR["memb"].ToString();
                adhan= DR["nompre"].ToString();


                if (DR["siyam"].ToString() != "")
                {
                    Siyam.Value = int.Parse(DR["siyam"].ToString());
                    Siyaman= int.Parse(DR["siyam"].ToString());
                }
                

                partcom.Value= int.Parse(DR["PartsComp"].ToString());
                partcompteur= int.Parse(DR["PartsComp"].ToString());

                label7.Text = DR["NumComp"].ToString();
              
                if (Boolean.Parse(DR["StatutsComp"].ToString())==false)
                {
                   
                    radioButton2.Checked = true;
                }
              

                 idadh = int.Parse(DR["idadhi"].ToString());
                int idsecsel= int.Parse(DR["IdSect"].ToString());
                DR.Close();

                ClassConnexion.Macon.Close();

                

                dada = new MySqlDataAdapter("select IdAdherent,CINAdhe,concat(NomArAdhe,' ',PrenomArAdhe) as membar from adherent where ExiAdhe=1 and IdAdherent<>"+ idadh + " order by IdAdherent ASC", ClassConnexion.Macon);
                dada.Fill(dscompteur, "adherent");


                CbAdh.Properties.DataSource = dscompteur.Tables["adherent"];
                CbAdh.Properties.ValueMember = "IdAdherent";
                CbAdh.Properties.DisplayMember = "membar";
                CbAdh.Properties.View = gridLookUpEdit1View;

          

                //Secteurs
                dada = new MySqlDataAdapter("select LibelleSect as sect,IdSect from secteur", ClassConnexion.Macon);
                dada.Fill(dscompteur, "secteur");
                combosecteur.DataSource = dscompteur.Tables["secteur"];
                combosecteur.ValueMember = "IdSect";
                combosecteur.DisplayMember = "sect";

              combosecteur.SelectedValue = idsecsel;
                // MessageBox.Show(idsecsel.ToString());
                secteuran = combosecteur.Text;

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
            if (e.Button.Properties.Tag== "Enregistrer")
            {

                if (CbAdh.EditValue == "0")
                {
                   
                    XtraMessageBox.Show("لم تقم بإختيار المشترك", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    splashScreenManager1.ShowWaitForm();

                    try
                    {
                        int statcomp = 0;

                        if (radioButton1.Checked)
                            statcomp = 1;

                        String formatdt = "yyyy-MM-dd HH:mm:ss";
                        int lastid = Configuration.LastID("transformations", "IdTrans") + 1;

                        MsgAn = "المشترك : " + adhan + " | الصيام : " + Siyaman + " | الجولة : " + secteuran + " | رقم العداد : " + label7.Text + " | عدد الأسهم : " + partcompteur;
                        MsgNv = "المشترك : " + CbAdh.Text + " | الصيام : " + Siyam.Value + " | الجولة : " + combosecteur.Text + " | رقم العداد : " + label7.Text + " | عدد الأسهم : " + partcom.Value;
                        MsgEn = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتحويل عداد (" + DateTime.Now.ToString(formatdt) + ") :";

                        Requete = "update compteur set siyam=" + Siyam.Value + ",IdAdherent=" + CbAdh.EditValue + ",IdSect=" + combosecteur.SelectedValue + ",StatutsComp=" + statcomp + ",PartsComp=" + partcom.Value + ",DateModComp=\\'" + DateTime.Now.ToString(formatdt) + "\\',IdUser=" + UserConnecte.IdUser + " where IdComp=" + id;
                        requetesupp1 = "insert into transformations value(" + lastid + ", " + idadh + ", " + CbAdh.EditValue + ",\\'" + DateTime.Now.ToString(formatdt) + "\\', \\'" + descritran.Text+"\\')";
                   
                         Configuration.Historique(0, Requete, MsgAn, MsgNv, MsgEn,requetesupp1, "");

                        splashScreenManager1.CloseWaitForm();
                        XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception EX)
                    {

                        MessageBox.Show(EX.ToString());
                    }
                }

               
               

            }
            else
            {
                this.DialogResult = DialogResult.Yes;
                
                // on quitte le Form2

                this.Close();

            }
        }
    }
}