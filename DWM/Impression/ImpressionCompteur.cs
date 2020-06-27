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
using DevExpress.XtraReports.UI;

namespace DWM
{
    public partial class ImpressionCompteur : DevExpress.XtraEditors.XtraForm
    {
        public ImpressionCompteur()
        {
            InitializeComponent();
        }

        MySqlDataAdapter dada;
        DataSet dscompteur;

        DataSetCompteur DScompteurSecteurAdh;
        MySqlDataAdapter DAcompteurSecteurAdh;

        private void ImpressionCompteur_Load(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();

            dscompteur = new DataSet();
            //Secteurs
            dada = new MySqlDataAdapter("select concat('S',IdSect,' / ',LibelleSect) as sect,IdSect from secteur", ClassConnexion.Macon);
            dada.Fill(dscompteur, "secteur");
            combosecteur.DataSource = dscompteur.Tables["secteur"];
            combosecteur.ValueMember = "IdSect";
            combosecteur.DisplayMember = "sect";


            splashScreenManager1.CloseWaitForm();
        }

      
        int status,secteur,Tout;

        private void checkEdit3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkEdit3.Checked)
            {
                combosecteur.Enabled = true;
            }
            else
            {
                combosecteur.Enabled = false;
            }
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag== "Imprimer")
            {
                secteur = 0; Tout = 0;

                if (checkEdit3.Checked && checkEdit2.Checked && checkEdit1.Checked)
                {
                    secteur = int.Parse(combosecteur.SelectedValue.ToString());
                    Tout = 1;
                }
                else if (checkEdit3.Checked && checkEdit2.Checked)
                {
                    secteur = int.Parse(combosecteur.SelectedValue.ToString());
                    status = 1;
                }
                else if (checkEdit3.Checked && checkEdit1.Checked)
                {
                    secteur = int.Parse(combosecteur.SelectedValue.ToString());
                    status = 0;
                }
                else if (!checkEdit3.Checked && checkEdit2.Checked && checkEdit1.Checked)
                {
                    Tout = 1;
                }
                else if (!checkEdit3.Checked && checkEdit2.Checked)
                {
                    status = 1;
                }
                else if (!checkEdit3.Checked && checkEdit1.Checked)
                {
                    status = 0;
                }
                else if (!checkEdit3.Checked && !checkEdit1.Checked && !checkEdit2.Checked)
                {
                    Tout = 1;
                }
                else if (checkEdit3.Checked && !checkEdit1.Checked && !checkEdit2.Checked)
                {
                    Tout = 1;
                }

                DScompteurSecteurAdh = new DataSetCompteur();
                DAcompteurSecteurAdh = new MySqlDataAdapter("select * from compteurcdherentcecteur", ClassConnexion.Macon);
                DAcompteurSecteurAdh.Fill(DScompteurSecteurAdh, "compteurcdherentcecteur");
                ImpressionCompteurs Report = new ImpressionCompteurs();
                Report.load();


                if (Tout == 1 && secteur != 0)
                {
                    Report.FilterString = string.Format("[IdSect] = {0} ", secteur);
                   
                }
                else if (Tout == 1 && secteur == 0)
                {

                }
                else if (Tout == 0 && secteur == 0)

                {
                    if (status == 0)
                    {
                        Report.FilterString = "[StatutsComp] = 'False' ";                     
                    }
                    else
                    {
                        Report.FilterString = "[StatutsComp]='True'";
                    }
                }
                else if (Tout == 0 && secteur != 0)
                {
                    if (status == 0)
                    {
                        Report.FilterString = string.Format("[StatutsComp] = 'False' AND [IdSect] = {0}", secteur);                      
                    }
                    else
                    {
                        Report.FilterString = string.Format("[StatutsComp] = 'True' AND [IdSect] = {0}", secteur);                     
                    }

                }

                Report.ShowPreviewDialog();
            }
            else if (e.Button.Properties.Tag == "Annuler")
            {
                this.Close();
            }
        }
    }
}