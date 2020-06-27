using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using DevExpress.XtraCharts;
using System.IO;

namespace DWM
{
    public partial class impressionFactures : DevExpress.XtraReports.UI.XtraReport
    {
        public impressionFactures()
        {
            InitializeComponent();
        }
        MySqlDataReader dr;
        public void load(int IdFctu)
        {
            string Information = Configuration.Func(12);
            string wanted_path = wanted_path = AppDomain.CurrentDomain.BaseDirectory;
            xrPictureBox1.ImageUrl = wanted_path + "\\Resources\\" + Configuration.Func(8);
            nomassoc.Text = Configuration.Func(1);
            adresse.Text = Configuration.Func(10);
            Tel.Text = Configuration.Func(11);
            email.Text = Configuration.Func(9);
            xrBarCode23.Text = Configuration.Func(9) + "\r\n" + DateTime.Now + "\r\n\r\n\r\n" + Information;

            ClassConnexion.Macon.Open();
            MySqlCommand Cmd = new MySqlCommand("SELECT date_format(PeriodeConsoFact,\"%m/%Y\") as perio, date_format(PeriodePaieDFact,\"%d/%m/%Y\") as PPeriodePaieDFact, date_format(PeriodePaieFFact,\"%d/%m/%Y\") as PPeriodePaieFFact  FROM  facture where IdFact=" + IdFctu + " ", ClassConnexion.Macon);
            dr = Cmd.ExecuteReader();
            while (dr.Read())
            {
                LbPerioCons.Text = dr["perio"].ToString();
                LbPerioD.Text = dr["PPeriodePaieDFact"].ToString();
                LbPerioF.Text = dr["PPeriodePaieFFact"].ToString();
            }
            dr.Close();
            ClassConnexion.Macon.Close();
        }

        public float summ(string requete)
        {
            float resultat = 0;
            try
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

                MySqlCommand FAC = new MySqlCommand(requete, ClassConnexion.Macon);
                dr = FAC.ExecuteReader();
                dr.Read();


                if (dr[0].ToString() != "")
                {

                    resultat = float.Parse(dr[0].ToString());
                }
                else
                {

                    resultat = 0;
                }

                dr.Close();
                ClassConnexion.Macon.Close();
            }
            catch (Exception EX)
            {
            }


            return resultat;
        }

        public void grapfremp(int IdFctu)
        {
            xrChartCons.BeginInit();
            Series seriesbar = xrChartCons.Series[0];

            seriesbar.Points.Clear();

            if (ClassConnexion.Macon.State == ConnectionState.Open)
            {
                ClassConnexion.Macon.Close();
                ClassConnexion.Macon.Open();
            }
            else
            {
                ClassConnexion.Macon.Open();
            }

            MySqlCommand FACTUR = new MySqlCommand("select date_format(PeriodeConsoFact,\"%m-%Y\") as datefact,IdFact from facture order by IdFact desc limit 0,6", ClassConnexion.Macon);
            dr = FACTUR.ExecuteReader();
            DataTable table = new DataTable();
            table.Columns.Add("datef", typeof(string));
            table.Columns.Add("idfact", typeof(int));

            DataRow row;

            while (dr.Read())
            {
                row = table.NewRow();

                row["datef"] = "- " + dr["datefact"].ToString();
                row["idfact"] = int.Parse(dr["IdFact"].ToString());
                table.Rows.Add(row);
            }

            dr.Close();
            ClassConnexion.Macon.Close();
            table.DefaultView.Sort = "idfact asc";
            table = table.DefaultView.ToTable();

            DataSetFactures ds = new DataSetFactures();
            

            for (int i = 0; i < table.Rows.Count; i++)
            {

                float somme = summ("select sum(c.Compsdf) as nbr from compteur Comp,consommation c,paiement p where Comp.IdComp=c.IdComp and c.IdCons=p.IdCons and p.IdFact=" + table.Rows[i][1].ToString() + " and Comp.NumComp="+LbNumCmp.Text);
                seriesbar.Points.Add(new SeriesPoint(table.Rows[i][0], somme));
               
            }


            xrChartCons.EndInit();

        }

        private void impressionFactures_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //grapfremp(1);
            //xrLabel5.Text = LbNumCmp.Text;
        }

        private void impressionFactures_ParametersRequestValueChanged(object sender, DevExpress.XtraReports.Parameters.ParametersRequestValueChangedEventArgs e)
        {
           
        }

        private void impressionFactures_DataSourceDemanded(object sender, EventArgs e)
        {
        
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
        }

        private void GroupHeader1_AfterPrint(object sender, EventArgs e)
        {
          
        }

        private void GroupHeader1_BandLevelChanged(object sender, EventArgs e)
        {
     
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
         
        }

        private void PageFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }

        private void PageFooter_AfterPrint(object sender, EventArgs e)
        {
           
        }

        private void PageFooter_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
      
        }

        private void LbNumCmp_TextChanged(object sender, EventArgs e)
        {
            grapfremp(1);
           
        }
    }
}
