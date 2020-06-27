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
using DevExpress.XtraReports.UI;

namespace DWM.Impression
{
    public partial class ConfigImpressionAdhe : DevExpress.XtraEditors.XtraForm
    {
        public ConfigImpressionAdhe()
        {
            InitializeComponent();
        }

        DataSetAdherent dsAdhe;
        MySqlDataAdapter daAdhe;
        string str="";
        void Teste(CheckEdit Ch1,CheckEdit Ch2)
        {
            if (Ch1.Checked==true)
            {
                Ch2.Checked = false;
            }
        }

        private void Filtrer()
        {
            if (ChEHomme.Checked==false && ChEFemme.Checked==false && ChELif.Checked== false && ChEDec.Checked == false && ChEnExis.Checked== false)
            {
                str="";
            }
            else
            {
                if (ChEHomme.Checked == true && ChELif.Checked == true && ChEnExis.Checked == false)
                    str= "[SexAdhe]='True' and [DeceAdhe]='True' ";
                else if (ChEHomme.Checked == true && ChELif.Checked == true && ChEnExis.Checked == true)
                    str= "[SexAdhe]='True' and [DeceAdhe]='True' and [ExiAdhe]='False'";
                else if (ChEHomme.Checked == true && ChELif.Checked == false && ChEnExis.Checked == false)
                {
                    if (ChEDec.Checked == false)
                        str= "[SexAdhe]='True' ";
                    else
                        str= "[SexAdhe]='True' and [DeceAdhe]='False' ";
                }
                else if (ChEHomme.Checked == true && ChELif.Checked == false && ChEnExis.Checked == true)
                {
                    if (ChEDec.Checked == false)
                        str= "[SexAdhe]='True' and [ExiAdhe]='False'";
                    else
                        str= "[SexAdhe]='True' and [DeceAdhe]='False' and [ExiAdhe]='False'";
                }
                else if (ChEHomme.Checked == false && ChELif.Checked == false && ChEnExis.Checked == true)
                {
                    if (ChEFemme.Checked==false)
                    {
                        if (ChEDec.Checked==false)
                            str= "[ExiAdhe]='False'";
                        else
                            str= "[DeceAdhe]='False' and [ExiAdhe]='False'";
                    }
                    else
                    {
                        if (ChEDec.Checked == false)
                            str= "[SexAdhe]='False' and [ExiAdhe]='False'";
                        else
                            str= "[SexAdhe]='False' and [DeceAdhe]='False' and [ExiAdhe]='False'";
                    }
                }
                else if (ChEHomme.Checked == false && ChELif.Checked == false && ChEnExis.Checked == false)
                {
                    if (ChEFemme.Checked == false)
                    {
                        if (ChEDec.Checked == true)
                            str= "[DeceAdhe]='False'";
                        else
                            str= "";
                    }
                    else
                    {
                        if (ChEDec.Checked == false)
                            str= "[SexAdhe]='False'";
                        else
                            str= "[SexAdhe]='False' and [DeceAdhe]='False'";
                    }
                }
                else if (ChEHomme.Checked == false && ChELif.Checked == true && ChEnExis.Checked == false)
                {
                    if (ChEFemme.Checked == false)
                        str= "[DeceAdhe]='True'";
                    else
                        str= "[SexAdhe]='False' and [DeceAdhe]='True'";
                }
                else if (ChEHomme.Checked == false && ChELif.Checked == true && ChEnExis.Checked == true)
                {
                    if (ChEFemme.Checked == false)
                        str= "[DeceAdhe]='True' and [ExiAdhe]='False'";
                    else
                        str= "[SexAdhe]='False' and [DeceAdhe]='True' and [ExiAdhe]='False'";
                }
            }
        }
        private void ConfigImpressionAdhe_Load(object sender, EventArgs e)
        {






        }

        private void ChEHomme_CheckedChanged(object sender, EventArgs e)
        {
            Teste(ChEHomme, ChEFemme);
            Filtrer();
        }
        private void ChEFemme_CheckedChanged(object sender, EventArgs e)
        {
            Teste(ChEFemme, ChEHomme);
            Filtrer();
        }
        private void ChELif_CheckedChanged(object sender, EventArgs e)
        {
            Teste(ChELif, ChEDec);
            Filtrer();
        }
        private void ChEDec_CheckedChanged(object sender, EventArgs e)
        {
            Teste(ChEDec,ChELif);
            Filtrer();
        }
        private void ChEnExis_CheckedChanged(object sender, EventArgs e)
        {
            Filtrer();
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag=="Imprimer")
            {
                splashScreenManager1.ShowWaitForm();
                //dsAdhe = new DataSetAdherent();
                //daAdhe = new MySqlDataAdapter("select * from adherent", ClassConnexion.Macon);
                //daAdhe.Fill(dsAdhe, "adherent");
                ImpressionAdherent Report = new ImpressionAdherent();
                Report.load();
                Filtrer();
                Report.FilterString = str;

                splashScreenManager1.CloseWaitForm();
                Report.ShowPreviewDialog();
            }
            else if (e.Button.Properties.Tag == "Annuler")
                this.Close();
        }
    }
}