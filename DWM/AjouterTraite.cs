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
    public partial class AjouterTraite : DevExpress.XtraEditors.XtraForm
    {
        public AjouterTraite()
        {
            InitializeComponent();
        }


        void vider()
        {
            textEdit1.Text = "";
            textEdit2.Text = "";
            dateEdit1.Text = "";
            date2 = "";
            Datdebut = "";
            DatdebutFix = "";
            requetemois = "";
            requeteprincipal = "";
            Requete2 = "";
            Requete3 = "";
            MsgNv = "";
            MtTotal = 0;
            Mtmensul = 0;
            Restmt = 0;

            lookAdh.Refresh();
            gridLookUpEdit1.Refresh();
        }
        void rempedit(string requete, GridLookUpEdit Grid,string membre,string value)
        {
            try
            {
                DataSet dscompteur;
                MySqlDataAdapter dada;
                dscompteur = new DataSet();
                dada = new MySqlDataAdapter(requete, ClassConnexion.Macon);
                dada.Fill(dscompteur, "data");

                Grid.Properties.DataSource = dscompteur.Tables["data"];
                Grid.Properties.ValueMember = value;
                Grid.Properties.DisplayMember = membre;
                Grid.Properties.View = gridLookUpEdit1View;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
           
        }

        DataSet adh;
        MySqlDataAdapter dadadh;
        DataView DV;

        float MtTotal,Mtmensul,Restmt=0;
        string Datdebut, DatdebutFix, date2;
        string requetemois,requeteprincipal;

        string MsgEn ;
        string Requete ;
        string Requete2 ;
        string Requete3, MsgNv;


        String formatdt = "yyyy-MM-dd HH:mm:ss";

        private void AjouterTraite_Load(object sender, EventArgs e)
        {
            try
            {
                rempedit("select IdAdherent,CINAdhe,concat(NomArAdhe,' ',PrenomArAdhe) as membar from adherent", lookAdh, "membar", "IdAdherent");
                ch = 1;
                DV = new DataView();
                adh = new DataSet();

                dadadh = new MySqlDataAdapter("select compteur.IdComp,NumComp,LibelleSect,IdAdherent from compteur,secteur where secteur.IdSect=compteur.IdSect ", ClassConnexion.Macon);
                dadadh.Fill(adh, "data");
                DV.RowFilter = "IdAdherent="+ lookAdh.EditValue;
                DV.Table= adh.Tables["data"];
                gridLookUpEdit1.Properties.DataSource = DV;
                gridLookUpEdit1.Properties.ValueMember = "IdComp";
                gridLookUpEdit1.Properties.DisplayMember = "NumComp";
               // gridLookUpEdit1.Properties.View = gridLookUpEdit1View;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Annuler")
            {
                this.DialogResult = DialogResult.Yes;
                // on quitte le Form2
                this.Close();
            }
            else if (e.Button.Properties.Tag == "Enregistrer")
            {
                if (lookAdh.EditValue=="0" || gridLookUpEdit1.EditValue=="0" || textEdit1.Text=="0" || textEdit2.Text=="0" || dateEdit1.Text=="")
                    XtraMessageBox.Show("حقل فارغ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (float.Parse(textEdit1.Text.ToString())<float.Parse(textEdit2.Text.ToString()))
                        XtraMessageBox.Show("يجب أن يكون المبلغ أكبر من المبلغ الشهري", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        splashScreenManager1.ShowWaitForm();
                        //float MtTotal, Mtmensul;
                        //DateTime Datdebut;
                        MtTotal = float.Parse(textEdit1.Text.ToString());
                        Mtmensul = float.Parse(textEdit2.Text.ToString());

                        Datdebut = dateEdit1.Text.ToString();
                        DatdebutFix= dateEdit1.Text.ToString();
                        Restmt = MtTotal;
                        int su = 0;
                        //requetemois
                        // MessageBox.Show("Montant total : "+ MtTotal+"  -- Mt Par mois : "+ Mtmensul+"  -- Date debut :  " + Datdebut.ToString("yyyy-MM-dd"));

                         
                        while (Restmt>0)
                        {
                            if (su==1)
                            {
                                requetemois += ",";
                                Datdebut = DateTime.Parse(Datdebut).AddMonths(1).ToString("yyyy-MM");
                            }
                            else
                                Datdebut = DateTime.Parse(Datdebut).ToString("yyyy-MM");

                            if (Restmt< Mtmensul)
                            {
                                Mtmensul = Restmt;
                                Restmt = 0;
                            }
                            requetemois += "(\\'"+ Datdebut+"\\',"+ Mtmensul+ ",0,NULL,(select max(IdTrai) from traite))";
                            Restmt = Restmt - Mtmensul;
                            su = 1;
                        }
                        date2 = DateTime.Parse(Datdebut).ToString("yyyy-MM-dd");

                        requeteprincipal = "insert into traite(IdComp,IdUser,MontantTTrai,MontantMTrai,ResteTrai,DateDTrai,DateFTrai) values(" + gridLookUpEdit1.EditValue + "," + UserConnecte.IdUser + "," + MtTotal + "," + Mtmensul + "," + MtTotal + ",\\'" + DateTime.Parse(DatdebutFix).ToString("yyyy-MM-dd") + "\\',\\'" + date2 + "\\') ";

                        MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة في الأقساط الشهرية  (" + DateTime.Now.ToString(formatdt) + ") :";
                        MsgNv = "عداد رقم  "+gridLookUpEdit1.Text+" - المبلغ "+ MtTotal+" تاريخ بداية التسديد "+ DateTime.Parse(Datdebut).ToString("yyyy-MM-dd");
                        Requete3 = "insert into moistraite(MoisMTr,MontantMTr,PayerMTr,DatePayerMTr,IdTrai) values" + requetemois;
                        Configuration.Historique(1, requeteprincipal, "", MsgNv, MsgEn, Requete3, "");
                        splashScreenManager1.CloseWaitForm();
                        XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        vider();
                        //****
                    }
                }
            }
        }

        int ch = 0;
        private void lookAdh_EditValueChanged(object sender, EventArgs e)
        {
            if (ch == 1)
            {
                DV.RowFilter = "IdAdherent=" + lookAdh.EditValue;
            }
        }
    }
}