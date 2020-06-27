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


namespace DWM
{
    public partial class ConfigImpressionEntr : DevExpress.XtraEditors.XtraForm
    {
        public ConfigImpressionEntr()
        {
            InitializeComponent();
        }

        DataSet ds;
        MySqlDataAdapter da;


        private void ConfigImpressionEntr_Load(object sender, EventArgs e)
        {
            checkEdit1.Checked = false;
            radioGroup1.Enabled = false;
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked==true)
                radioGroup1.Enabled = true;
            if (checkEdit1.Checked == false)
                radioGroup1.Enabled = false;
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            string strFilter;
            if (e.Button.Properties.Tag=="Imprimer")
            {
                if (checkEdit1.Checked==true)
                {
                    if (radioGroup1.SelectedIndex == 0)
                        strFilter = "[IdFraisau]='0'";
                    else
                        strFilter = "[IdFraisau]='1'";

                    splashScreenManager1.ShowWaitForm();
                    ImpressionEntrees Report = new ImpressionEntrees();
                    Report.load();
                    Report.FilterString = strFilter;

                    splashScreenManager1.CloseWaitForm();
                    Report.ShowPreviewDialog();
                }
                else
                {
                    splashScreenManager1.ShowWaitForm();
                    ImpressionEntrees Report = new ImpressionEntrees();
                    Report.load();
                    splashScreenManager1.CloseWaitForm();
                    Report.ShowPreviewDialog();
                }

            }
            else if (e.Button.Properties.Tag== "Annuler")
            {
                this.Close();
            }
        }
    }
}