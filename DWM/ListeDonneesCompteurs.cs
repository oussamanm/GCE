using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MySql.Data.MySqlClient;
using System.Data;

namespace DWM
{
    public partial class ListeDonneesCompteurs : DevExpress.XtraReports.UI.XtraReport
    {
        public ListeDonneesCompteurs()
        {
            InitializeComponent();
        }

        MySqlCommand infofacture;
        MySqlDataReader DR;

        public void Load()
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

            infofacture = new MySqlCommand("select * from facture order by IdFact DESC Limit 0,1", ClassConnexion.Macon);
            DR = infofacture.ExecuteReader();
            DR.Read();

            if (DR.HasRows)
            {
                DateTime datcon = DateTime.Parse(DR["PeriodeConsoFact"].ToString());
                DateTime datdeb = DateTime.Parse(DR["PeriodePaieDFact"].ToString());
                DateTime datfin = DateTime.Parse(DR["PeriodePaieFFact"].ToString());
              
                periodeconsom.Text = datcon.ToString("MM-yyyy");
                periodeD.Text= datdeb.ToString("dd-MM-yyyy");
                periodeF.Text = datfin.ToString("dd-MM-yyyy");
            }
            ClassConnexion.Macon.Close();
            DR.Close();
            string Information = Configuration.Func(12);
            string wanted_path = wanted_path = AppDomain.CurrentDomain.BaseDirectory;
            xrPictureBox1.ImageUrl = wanted_path + "\\Resources\\" + Configuration.Func(8);
            nomassoc.Text = Configuration.Func(1);
            adresse.Text = Configuration.Func(10);
            Tel.Text = Configuration.Func(11);
            email.Text = Configuration.Func(9);
            xrBarCode23.Text = Configuration.Func(9) + "\r\n" + DateTime.Now + "\r\n\r\n\r\n" + Information;


        }

    }
}
