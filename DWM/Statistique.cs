using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;
using MySql.Data.MySqlClient;
//using System.Collections;
using DevExpress.XtraEditors.Controls;

namespace DWM
{
    public partial class Statistique : DevExpress.XtraEditors.XtraForm
    {
        public Statistique()
        {
            InitializeComponent();
            controleHeader1.label_menu_btn = "الصفحة الرئيسية  /  إحصائيات";
            gestiondata();
        }

        MySqlCommand CMDCOM,FACTUR;
        MySqlDataReader DR,DR2,dr;
        DataSet ds,DSCompteur;
        MySqlDataAdapter da, DACompteur;
        DataView dv,dv2,dv3,dv4,dvEntrGen;
        DataTable table;
        DataRow row;


        ///// Void
        public float summ(string requete)
        {
            float resultat=0;
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
                DR2 = FAC.ExecuteReader();
                DR2.Read();

                if (DR2[0].ToString()!="")
                {
                    resultat = float.Parse(DR2[0].ToString());
                }
                else
                {
                    resultat = 0;
                }
                DR2.Close();
                ClassConnexion.Macon.Close();
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
            return resultat;
        }
        public void statistiqueconsommation()
        {
            Series seriesbar = barconsom.Series[0];
            seriesbar.Points.Clear();

            if (gridView1.SelectedRowsCount == 1)
                {
                  int iddhselected = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString());
             
                  rempcomteur(iddhselected);
               
                if (int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[6].ToString())!=0)
                {
                    SetEditValueByIndex(gridLookUpEdit1, 0);

                    if (barconsom.Titles.Count!=0)
                    {
                        barconsom.Titles.RemoveAt(0);
                    }
                     ChartTitle chartTitle1 = new ChartTitle();
                    chartTitle1.Text =  gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString()+" "+ gridView1.GetDataRow(gridView1.FocusedRowHandle)[4].ToString()+" - عداد رقم : "+ gridLookUpEdit1.Text;
                    barconsom.Titles.Add(chartTitle1);
                }
                else
                {
                    if (barconsom.Titles.Count != 0)
                    {
                        barconsom.Titles.RemoveAt(0);
                    }
                }
            }       
        }
        public void RempAdh()
        {
            
            da = new MySqlDataAdapter("select adherent.IdAdherent,adherent.NomFrAdhe,adherent.PrenomFrAdhe, adherent.NomArAdhe, adherent.PrenomArAdhe,  case when adherent.SexAdhe =1 then 'ذكر' else 'انثى' end as SexeAd,count(IdComp) as countCom from utilisateurs,adherent left join compteur on adherent.IdAdherent=compteur.IdAdherent where utilisateurs.IdUser=adherent.IdUser GROUP by adherent.IdAdherent", ClassConnexion.Macon);
            da.Fill(ds, "Adherent");
          
            gridControl1.DataSource = ds.Tables["Adherent"];
            gridControl1.Refresh();       
        }
        int i;
        public void Requte()
        {
            // DataColumn column = ds.Tables["Adherent"].Columns.Add("Countt", typeof(Int32));
            foreach (DataRow row in ds.Tables["Adherent"].Rows)
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
                int index = ds.Tables["Adherent"].Rows.IndexOf(row);

                MySqlCommand CmdCount = new MySqlCommand("select count(IdComp) as countComp from adherent left join compteur on adherent.IdAdherent = compteur.IdAdherent WHERE adherent.IdAdherent =" + int.Parse(ds.Tables["Adherent"].Rows[index]["IdAdherent"].ToString()) + "  and StatutsComp = 1", ClassConnexion.Macon);
                ClassConnexion.DR = CmdCount.ExecuteReader();
                while (ClassConnexion.DR.Read())
                {
                    i = int.Parse(ClassConnexion.DR["countComp"].ToString());
                }
                ds.Tables["Adherent"].Rows[index]["countCom"] = i;

                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
            }
        }
        public  float count(string requete)
        {
            float resultat=0;
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

                MySqlDataReader DR;
                MySqlCommand CMD = new MySqlCommand(requete, ClassConnexion.Macon);
                DR = CMD.ExecuteReader();
                DR.Read();
                if (DR.HasRows)
                {
                    resultat = float.Parse(DR[0].ToString());
                }
                else
                {
                    resultat = 0;
                }
                DR.Close();
                ClassConnexion.Macon.Close();              
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }

            return resultat;
        }
        public float ReturnvaluefromReq(string requete)
        {
            float resultat = 0;
            try
            {
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                {
                    ClassConnexion.Macon.Open();
                }

                MySqlDataReader dr;
                MySqlCommand cmd = new MySqlCommand(requete, ClassConnexion.Macon);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
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
                MessageBox.Show(EX.ToString());
            }
            return resultat;
        }
        public void grapfremp()
        {
            barconsom.BeginInit();
            Series seriesbar = barconsom.Series[0];

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

            FACTUR = new MySqlCommand("select date_format(PeriodeConsoFact,\"%m-%Y\") as datefact,IdFact from facture order by IdFact desc limit 0,6", ClassConnexion.Macon);
            DR2 = FACTUR.ExecuteReader();
            table = new DataTable();
            table.Columns.Add("datef", typeof(string));
            table.Columns.Add("idfact", typeof(int));



            while (DR2.Read())
            {
                row = table.NewRow();
                row["datef"] = "- " + DR2["datefact"].ToString();
                row["idfact"] = int.Parse(DR2["IdFact"].ToString());
                table.Rows.Add(row);
            }

            DR2.Close();
            ClassConnexion.Macon.Close();
            table.DefaultView.Sort = "idfact asc";
            table = table.DefaultView.ToTable();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                float somme = summ("select sum(c.Compsdf) as nbr from consommation c,paiement p where c.IdCons=p.IdCons and p.IdFact=" + table.Rows[i][1].ToString() + " and c.IdComp=" + gridLookUpEdit1.EditValue);
                seriesbar.Points.Add(new SeriesPoint(table.Rows[i][0], somme));
            }
            barconsom.EndInit();
        }
        public void SetEditValueByIndex(GridLookUpEdit edit , int index)
        {
            Object keyValue = edit.Properties.GetKeyValue(index);
            edit.EditValue = keyValue;
        }        
        public void statcompteurs()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                         
                DSCompteur = new DataSet();
                DACompteur = new MySqlDataAdapter("select * from secteur", ClassConnexion.Macon);
                DACompteur.Fill(DSCompteur, "secteur");

                chartControlCompteur.BeginInit();

                //Compteur Fermer
                if (checkBoxOuvrcomp.Checked && checkBoxFermcompt.Checked && checkBoxConsom.Checked && checkBoxperiodcon.Checked)
                {
                    
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات حسب الجولة لفترة  : "+comboBoxPeriodeCon.Text;
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];
                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = true;
                    fermer.ShowInLegend = true;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt0 = count("select count(*) from compteur where StatutsComp=0 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        nbrcompt1 = count("select count(*) from compteur where StatutsComp=1 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and paiement.IdFact="+comboBoxPeriodeCon.SelectedValue+" and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());


                        ouvr.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt1));
                        fermer.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt0));
                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
                }else if (checkBoxOuvrcomp.Checked && checkBoxFermcompt.Checked && checkBoxConsom.Checked )
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                     titre.Text = "العدادات حسب الجولة - " + DateTime.Now.ToString("yyyy-MM-dd");
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = true;
                    fermer.ShowInLegend = true;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt0 = count("select count(*) from compteur where StatutsComp=0 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        nbrcompt1 = count("select count(*) from compteur where StatutsComp=1 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons  and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());


                        ouvr.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt1));
                        fermer.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt0));
                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
                }else if (checkBoxOuvrcomp.Checked && checkBoxFermcompt.Checked )
                {

                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات حسب الجولة - ";
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();
                    ouvr.ShowInLegend = true;
                    fermer.ShowInLegend = true;
                    consom.ShowInLegend = false;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt0 = count("select count(*) from compteur where StatutsComp=0 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        nbrcompt1 = count("select count(*) from compteur where StatutsComp=1 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                      
                        
                        ouvr.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt1));
                        fermer.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt0));
                      

                    }
                    chartControlCompteur.EndInit();

                }else if(checkBoxOuvrcomp.Checked && checkBoxFermcompt.Checked==false && checkBoxConsom.Checked && checkBoxperiodcon.Checked)
                {
                
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات المتصلة حسب الجولة لفترة  : " + comboBoxPeriodeCon.Text;
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = true;
                    fermer.ShowInLegend = false;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {         
                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt1 = count("select count(*) from compteur where StatutsComp=1 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and paiement.IdFact="+comboBoxPeriodeCon.SelectedValue+" and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                        ouvr.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt1));
                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
                }
                else if (checkBoxOuvrcomp.Checked && checkBoxFermcompt.Checked == false && checkBoxConsom.Checked )
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                     titre.Text = "العدادات المتصلة حسب الجولة - " + DateTime.Now.ToString("yyyy-MM-dd");
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = true;
                    fermer.ShowInLegend = false;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt1 = count("select count(*) from compteur where StatutsComp=1 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());


                        ouvr.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt1));
                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
                }else if(checkBoxOuvrcomp.Checked && checkBoxFermcompt.Checked == false)
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات المتصلة حسب الجولة - " + DateTime.Now.ToString("yyyy-MM-dd");
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = true;
                    fermer.ShowInLegend = false;
                    consom.ShowInLegend = false;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {
                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt1 = count("select count(*) from compteur where StatutsComp=1 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                        ouvr.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt1));
                    }
                    chartControlCompteur.EndInit();
                }else if (checkBoxOuvrcomp.Checked == false && checkBoxFermcompt.Checked  && checkBoxConsom.Checked && checkBoxperiodcon.Checked)
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات المنفصلة حسب الجولة لفترة  : " + comboBoxPeriodeCon.Text;
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = false;
                    fermer.ShowInLegend = true;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt0 = count("select count(*) from compteur where StatutsComp=0 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and paiement.IdFact=" + comboBoxPeriodeCon.SelectedValue + " and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                        fermer.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt0));
                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));
                    }
                    chartControlCompteur.EndInit();
                }
                else if (checkBoxOuvrcomp.Checked == false && checkBoxFermcompt.Checked  && checkBoxConsom.Checked)
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات المنفصلة حسب الجولة - " + DateTime.Now.ToString("yyyy-MM-dd");
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = false;
                    fermer.ShowInLegend = true;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt0 = count("select count(*) from compteur where StatutsComp=0 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                         fermer.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt0));
                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
                }
                else if (checkBoxOuvrcomp.Checked == false && checkBoxFermcompt.Checked )
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "العدادات المنفصلة حسب الجولة - " + DateTime.Now.ToString("yyyy-MM-dd");
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = false;
                    fermer.ShowInLegend = true;
                    consom.ShowInLegend = false;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {

                        float nbrcompt0, nbrcompt1, consomma;
                        nbrcompt0 = count("select count(*) from compteur where StatutsComp=0 and IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                         fermer.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), nbrcompt0));

                    }
                    chartControlCompteur.EndInit();
                }
                else if (checkBoxOuvrcomp.Checked == false && checkBoxFermcompt.Checked == false && checkBoxConsom.Checked && checkBoxperiodcon.Checked)
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "الإستهلاك الشهري حسب الجولة لفترة  : " + comboBoxPeriodeCon.Text;
                    chartControlCompteur.Titles.Add(titre);


                    Series ouvr = chartControlCompteur.Series[0];

                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = false;
                    fermer.ShowInLegend = false;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {
                        float nbrcompt0, nbrcompt1, consomma;
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and paiement.IdFact=" + comboBoxPeriodeCon.SelectedValue + " and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
                }
                else if (checkBoxOuvrcomp.Checked == false && checkBoxFermcompt.Checked == false && checkBoxConsom.Checked )
                {
                    if (chartControlCompteur.Titles.Count != 0)
                    {
                        chartControlCompteur.Titles.RemoveAt(0);
                    }

                    ChartTitle titre = new ChartTitle();
                    titre.Text = "الإستهلاك الشهري حسب الجولة   : " + DateTime.Now.ToString("yyyy-MM-dd");
                    chartControlCompteur.Titles.Add(titre);

                    Series ouvr = chartControlCompteur.Series[0];
                    Series fermer = chartControlCompteur.Series[1];
                    Series consom = chartControlCompteur.Series[2];

                    ouvr.Points.Clear();
                    fermer.Points.Clear();
                    consom.Points.Clear();

                    ouvr.ShowInLegend = false;
                    fermer.ShowInLegend = false;
                    consom.ShowInLegend = true;

                    for (int i = 0; i < DSCompteur.Tables[0].Rows.Count; i++)
                    {
                        float nbrcompt0, nbrcompt1, consomma;                    
                        consomma = summ("select sum(Compsdf) from compteur,consommation,paiement where paiement.IdCons=consommation.IdCons and compteur.IdComp=consommation.IdComp and  IdSect=" + DSCompteur.Tables[0].Rows[i]["IdSect"].ToString());

                        consom.Points.Add(new SeriesPoint(DSCompteur.Tables[0].Rows[i]["LibelleSect"].ToString(), consomma));

                    }
                    chartControlCompteur.EndInit();
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
        public void rempcomteur(int idadh)
        {           
            DataSet  dscompteur = new DataSet();
            MySqlDataAdapter dada = new MySqlDataAdapter("select IdComp,concat(NumComp,' - ',LibelleSect) as numla from compteur,secteur where secteur.IdSect=compteur.IdSect and StatutsComp=1 and compteur.IdAdherent=" + idadh, ClassConnexion.Macon);
            dada.Fill(dscompteur, "compteur");

            gridLookUpEdit1.Properties.DataSource = dscompteur.Tables["compteur"];
            gridLookUpEdit1.Properties.ValueMember = "IdComp";
            gridLookUpEdit1.Properties.DisplayMember = "numla";
            gridLookUpEdit1.Properties.View = gridLookUpEdit1View;
            
        }
        public void gestiondata()
        {
          
            if (Secteuradherent.Series.Count>0)
            {
                
                Secteuradherent.BeginInit();
                Series series = Secteuradherent.Series[0];
                Series seriessexe = Secteuradherent.Series[1];

                series.Points.Clear();
                seriessexe.Points.Clear();

                series.Points.Add(new SeriesPoint("الأحياء", +count(" select count(*) from adherent where DeceAdhe=1 ")));
                series.Points.Add(new SeriesPoint("المتوفون", + count(" select count(*) from adherent where DeceAdhe=0 ")));
                seriessexe.Points.Add(new SeriesPoint("الرجال", +count(" select count(*) from adherent where SexAdhe=1 ")));
                seriessexe.Points.Add(new SeriesPoint("النساء", +count(" select count(*) from adherent where SexAdhe=0 ")));

                Secteuradherent.EndInit();
                
            }

           
        }
        

        private void Statistique_Load(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                ds = new DataSet();
                Color slateBlue = Color.FromArgb(0, 174, 219);
                tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue;

                RempAdh();
                Requte();

                gridView1.Columns[0].Caption = "ر.ت";
                gridView1.Columns[1].Caption = "Nom";
                gridView1.Columns[2].Caption = "Prénom";
                gridView1.Columns[3].Caption = "النسب";
                gridView1.Columns[4].Caption = "الإسم";
                gridView1.Columns[5].Caption = "الجنس";
                gridView1.Columns[6].Caption = "عدد العدادات";

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

        ///// Buttons Prints
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            chartControlCompteur.ShowRibbonPrintPreview();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            barconsom.ShowRibbonPrintPreview();
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            chartControl1.ShowRibbonPrintPreview();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxperiodcon.Checked)
            {          
                comboBoxPeriodeCon.Enabled = true;
            }
            else
            {
                comboBoxPeriodeCon.Enabled = false;
            }
            statcompteurs();
        }
        private void checkBoxConsom_CheckedChanged(object sender, EventArgs e)
        {
            statcompteurs();
        }
        private void checkBoxFermcompt_CheckedChanged(object sender, EventArgs e)
        {
            statcompteurs();
        }
        private void checkBoxOuvrcomp_CheckedChanged(object sender, EventArgs e)
        {
            statcompteurs();
        }
        private void CBYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBYear.SelectedValue != null)
                RempdataChartCons("select * from viewchartonage where YFct='" + CBYear.SelectedValue + "' order by IdF asc ");
            else
                RempdataChartCons("select * from viewchartonage order by IdF asc");
            ProccesSelectItem();
        }
        private void comboBoxPeriodeCon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changer==1)
                statcompteurs();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Secteuradherent.ShowRibbonPrintPreview();
        }


        DataSet dsCons;

        public void RempChartCons()
        {
            try
            {
                float nbrcompt0, nbrcompt1;
                string str1 ,str2= "";
                if (checkBox1.Checked && checkBox2.Checked)
                {
                    chartControl1.BeginInit();
                    if (chartControl1.Titles.Count != 0)
                        chartControl1.Titles.RemoveAt(0);

                    chartControl1.Series[0].Points.Clear();
                    chartControl1.Series[1].Points.Clear();

                    chartControl1.Series[0].ShowInLegend = true;
                    chartControl1.Series[1].ShowInLegend = true;
                    for (int i = 0; i < dsCons.Tables["StCons"].Rows.Count; i++)
                    {
                        nbrcompt0 = ReturnvaluefromReq("select Sumton from viewchartonage where IdF=" + dsCons.Tables["StCons"].Rows[i]["IdF"].ToString());
                        nbrcompt1 = ReturnvaluefromReq("select TotalPaie from viewchartonage where IdF=" + dsCons.Tables["StCons"].Rows[i]["IdF"].ToString());
                        str1 = "'" + dsCons.Tables["StCons"].Rows[i]["PCFct"].ToString() + "'";
                        str2 = "'" + dsCons.Tables["StCons"].Rows[i]["PCFct"].ToString() + "'"; ;

                        DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(str1, new object[] {
                    ((object)(nbrcompt0))});
                        DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint(str2, new object[] {
                    ((object)(nbrcompt1))});

                        chartControl1.Series[0].Points.Add(seriesPoint1);
                        chartControl1.Series[1].Points.Add(seriesPoint2);
                    }
                    chartControl1.EndInit();
                }
                else if (checkBox1.Checked==true && checkBox2.Checked==false)
                {
                    chartControl1.BeginInit();

                    if (chartControl1.Titles.Count != 0)
                    {
                        chartControl1.Titles.RemoveAt(0);
                    }

                    chartControl1.Series[0].Points.Clear();
                    chartControl1.Series[1].Points.Clear();

                    chartControl1.Series[0].ShowInLegend = true;
                    chartControl1.Series[1].ShowInLegend = false;

                    for (int i = 0; i < dsCons.Tables["StCons"].Rows.Count; i++)
                    {
                        nbrcompt0 = ReturnvaluefromReq("select Sumton from viewchartonage where IdF=" + dsCons.Tables["StCons"].Rows[i]["IdF"].ToString());                 
                        str1 = "'" + dsCons.Tables["StCons"].Rows[i]["PCFct"].ToString() + "'";
                        DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(str1, new object[] {
                    ((object)(nbrcompt0))});
                        chartControl1.Series[0].Points.Add(seriesPoint1);
                    }
                    chartControl1.EndInit();
                }
                else if (checkBox1.Checked == false && checkBox2.Checked == true)
                {
                    chartControl1.BeginInit();
                    if (chartControl1.Titles.Count != 0)
                    {
                        chartControl1.Titles.RemoveAt(0);
                    }

                    chartControl1.Series[0].Points.Clear();
                    chartControl1.Series[1].Points.Clear();

                    chartControl1.Series[0].ShowInLegend = false;
                    chartControl1.Series[1].ShowInLegend = true;
                    for (int i = 0; i < dsCons.Tables["StCons"].Rows.Count; i++)
                    {
                        nbrcompt1 = ReturnvaluefromReq("select TotalPaie from viewchartonage where IdF=" + dsCons.Tables["StCons"].Rows[i]["IdF"].ToString());
                        str2 = "'" + dsCons.Tables["StCons"].Rows[i]["PCFct"].ToString() + "'"; ;
                        DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint(str2, new object[] {
                        ((object)(nbrcompt1))});
                        chartControl1.Series[1].Points.Add(seriesPoint2);
                    }
                    chartControl1.EndInit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void ProccesSelectItem()
        {
            if (i == 1)
                RempChartCons();
        }
        private void RempdataChartCons(string req)
        {
            dsCons = new DataSet();
            da = new MySqlDataAdapter(req, ClassConnexion.Macon);
            da.Fill(dsCons, "StCons");
        }

        ///// click Item 
        DataTable Table;
        Color slateBlue2 = Color.FromArgb(41, 57, 86);
        Color slateBlue = Color.FromArgb(0, 174, 219);

        private void tileItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemReport.AppearanceItem.Normal.BackColor = slateBlue2;

                tileItem1.AppearanceItem.Normal.BackColor = slateBlue;

                Configuration.RempCombo(CBYear, "select IdF,YFct from viewchartonage group by YFct", "TableYear", "YFct", "YFct");
                if (CBYear.SelectedValue != null)
                    RempdataChartCons("select * from viewchartonage where YFct='" + CBYear.SelectedValue + "' order by IdF asc ");
                else
                    RempdataChartCons("select * from viewchartonage order by IdF asc");

                i = 1;
                ProccesSelectItem();

                navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
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
        private void tileItemPena_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                changer = 0;
                DataSet DScom = new DataSet();
                MySqlDataAdapter DAperCON = new MySqlDataAdapter("select  date_format(PeriodeConsoFact,\" %m-%Y\") as datcon, facture.* from facture order by IdFact DESC", ClassConnexion.Macon);
                DAperCON.Fill(DScom, "data");
                comboBoxPeriodeCon.DataSource = DScom.Tables[0];
                comboBoxPeriodeCon.ValueMember = "IdFact";
                comboBoxPeriodeCon.DisplayMember = "datcon";
                changer = 1;

                tileItem1.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue2;
                tileItemReport.AppearanceItem.Normal.BackColor = slateBlue2;

                tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue;

                statcompteurs();
                navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void tileItemPaiem_ItemClick(object sender, TileItemEventArgs e)
        {
            tileItem1.AppearanceItem.Normal.BackColor = slateBlue2;
            tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue2;
            tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue2;
            tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue2;
            tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue2;
            tileItemReport.AppearanceItem.Normal.BackColor = slateBlue2;

            tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
        }
        private void tileItemEntres_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (navigationFrame1.SelectedPageIndex != tileGroup5.Items.IndexOf(e.Item))
                {

                    tileItem1.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemReport.AppearanceItem.Normal.BackColor = slateBlue2;

                    tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue;

                    LoadEntresGen();
                    navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
                }
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
        private void tileItemSorties_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (navigationFrame1.SelectedPageIndex != tileGroup5.Items.IndexOf(e.Item))
                {
                    tileItem1.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemReport.AppearanceItem.Normal.BackColor = slateBlue2;

                    tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue;

                    LoadSort();
                    navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
                }
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
        private void tileItemCaisse_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (navigationFrame1.SelectedPageIndex != tileGroup5.Items.IndexOf(e.Item))
                {
                    tileItem1.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemReport.AppearanceItem.Normal.BackColor = slateBlue2;

                    tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue;

                    LoadCaisse();
                    navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
                }
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
        private void tileItemReport_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (navigationFrame1.SelectedPageIndex != tileGroup5.Items.IndexOf(e.Item))
                {
                    tileItem1.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemEntres.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemSorties.AppearanceItem.Normal.BackColor = slateBlue2;
                    tileItemCaisse.AppearanceItem.Normal.BackColor = slateBlue2;

                    tileItemReport.AppearanceItem.Normal.BackColor = slateBlue;

                    LoadReport();
                    navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
                }            
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
        private void tileItemTest_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
            LoadPageGraph();

        }

        ///// Checked Changed chart Conso/Paie /Perio
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            RempChartCons();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RempChartCons();
        }


        int changer;

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
           
            grapfremp();
            if (barconsom.Titles.Count != 0)
            {
                barconsom.Titles.RemoveAt(0);
            }

            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString() + " " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[4].ToString() + " - عداد رقم : " + gridLookUpEdit1.Text;
            barconsom.Titles.Add(chartTitle1);
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            statistiqueconsommation();
        }

        ///////"""""""""""" Entres Gen """""""""""""///////


        ///'''' Buttons Menu
        private void BtnEntGen_Click(object sender, EventArgs e)
        {
            navigationFrameEntres.SelectedPageIndex = 0;

            BtnEntGen.Appearance.BackColor = Color.White;
            BtEntPaie.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPena.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntEntres.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntTrai.Appearance.BackColor = SystemColors.ButtonFace;

            /// Load Entres Gen
            //LoadEntresGen();
        }
        private void BtEntPaie_Click(object sender, EventArgs e)
        {
            navigationFrameEntres.SelectedPageIndex = 1;

            BtEntPaie.Appearance.BackColor = Color.White;
            BtnEntGen.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPena.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntEntres.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntTrai.Appearance.BackColor = SystemColors.ButtonFace;

            /// Load Entres Paie
            LoadEntresPaie();
        }
        private void BtEntTrai_Click(object sender, EventArgs e)
        {
            navigationFrameEntres.SelectedPageIndex = 3;

            BtEntTrai.Appearance.BackColor = Color.White;
            BtnEntGen.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPena.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntEntres.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPaie.Appearance.BackColor = SystemColors.ButtonFace;

            /// Load Entres Trai
            LoadEntresTrai();
        }
        private void BtEntPena_Click(object sender, EventArgs e)
        {
            navigationFrameEntres.SelectedPageIndex = 4;

            BtEntPena.Appearance.BackColor = Color.White;
            BtnEntGen.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntTrai.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntEntres.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPaie.Appearance.BackColor = SystemColors.ButtonFace;

            /// Load Entres Trai
            LoadEntresPena();
        }
        private void BtEntEntres_Click(object sender, EventArgs e)
        {
            navigationFrameEntres.SelectedPageIndex = 2;

            BtEntEntres.Appearance.BackColor = Color.White;
            BtnEntGen.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntTrai.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPena.Appearance.BackColor = SystemColors.ButtonFace;
            BtEntPaie.Appearance.BackColor = SystemColors.ButtonFace;
            /// Load Entres Trai
            LoadEntresEntr();
        }
        private void ButtonAEntr_Click(object sender, EventArgs e)
        {
            navigationFrameEntres.SelectedPageIndex = 6;

            foreach (SimpleButton item in panelControl2.Controls)
            {
                item.Appearance.BackColor = SystemColors.ButtonFace;
            }
            ButtonAEntr.Appearance.BackColor = Color.White;

            /// Load Entres Trai
            LoadEntresAEntr();
        }
        ///'''' Void 
        Boolean dejaOvrTableEntresGene = false;
        Boolean testEntreSelecIndexChangGen = false;
        DataTable TabAnnesMtnT ;
        float VMtnPaie, VMtnPR, VMtnTaxe, VMtnTrai, VMtnEntr, VMtnPena,VMtnAEntr, VMtnGen = 0;

        private void RempCheckedListeMonthsGen(string strr,CheckedListBoxControl ChLB)
        {
            if (strr != "")
            {
                strr = "11-11-" + strr;
                Dtyear = DateTime.Parse(strr);
                ChLB.Items.Clear();

                var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
                foreach (var month in months)
                {
                    string str = month.ToString("MM-yyyy");
                    if (str == DateTime.Now.ToString("MM-yyyy"))
                        ChLB.Items.Add(str, true);
                    else
                        ChLB.Items.Add(str, false);
                }
            }
        }
        private void RempRgroupeMonths(string strr,RadioGroup RG)
        {
            strr = "11-11-" + strr;
            Dtyear = DateTime.Parse(strr);
            RG.Properties.Items.Clear();

            var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
            foreach (var month in months)
            {
                string str = month.ToString("MM-yyyy");
                RadioGroupItem Itam = new RadioGroupItem();
                Itam.Description = str;
                RG.Properties.Items.Add(Itam);
            }
        }
        private void RempDataEntresGene(string strYear)
        {
            if (dejaOvrTableEntresGene == true)
                ds.Tables["ListeEntrGene"].Clear();

            using (MySqlCommand cmd = new MySqlCommand("dwm.TESTINSERTINTOTABLETEMP", ClassConnexion.Macon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter parm1 = new MySqlParameter("@StrYear", MySqlDbType.VarChar);
                parm1.Value = strYear;
                parm1.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(parm1);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(ds, "ListeEntrGene");
                    dejaOvrTableEntresGene = true;
                    dvEntrGen = new DataView();
                    dvEntrGen.Table = ds.Tables["ListeEntrGene"];
                }
            }
        }
        private void RempTableEntresGene()
        {
            TLEntresGen.DataSource = dvEntrGen;
            TLEntresGen.Refresh();
        }
        private void LoadEntresGen()
        {
            Configuration.RempCombo(CbAnneesGen, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons", "DateCons");
            CbAnneesGen.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            RempRgroupeMonths(CbAnneesGen.SelectedValue.ToString(),RGMonths);
            RempDataEntresGene(CbAnneesGen.SelectedValue.ToString());
            RempTableEntresGene();
            RempFooterMtnGen();

            if (testEntreSelecIndexChangGen == true)
            {
                FilterGridEntrGen();
                RempFooterMtnGen();
            }
            //RempTileViewAnnMtnT();
            testEntreSelecIndexChangGen = true;

        }
        private void RempFooterMtnGen()
        {
            if (CbAnneesGen.Items.Count > 0 && CbAnneesGen.SelectedValue.ToString() != "")
            {
                float MtnPaie = CalculRequte("select ReturnSumMtnPaie('" + CbAnneesGen.SelectedValue.ToString() + "')");
                float MtnPR = CalculRequte("select sum(PenaEntrPaie) from entpaie where PayeEntrPaie=1 and date_format(DatePEntPaie,\"%Y\")='" + CbAnneesGen.SelectedValue.ToString() + "' ");
                float MtnTaxe = CalculRequte("select sum(V_Prix) from lsthelpget_montsansfrais where V_PayePaie=1 and Date_Format(V_DatePaie,\"%Y\")='" + CbAnneesGen.SelectedValue.ToString() + "' ");
                float MtnTrai = CalculRequte("select sum(MontantMTr) from moistraite where PayerMTr=1 and date_format(DatePayerMTr,\"%Y\")='" + CbAnneesGen.SelectedValue.ToString() + "' ");
                float MtnEntr = CalculRequte("select sum(MontantEntr) from ententres where PayerEntr=1 and Date_Format(DatePayerEntr,\"%Y\")='" + CbAnneesGen.SelectedValue.ToString() + "' ");
                float MtnPena = CalculRequte("select sum(MontantPena) from entpena where date_format(DatePayerPena, \"%Y\") = '" + CbAnneesGen.SelectedValue.ToString() + "' ");
                float MtnAEntr = CalculeRequte("select sum(MontantEntr) from entrescais where CatEntres_IdCatEntr>4 and SuppEntr=0 and Date_Format(DateEntr,\"%Y\") = '" + CbAnneesGen.SelectedValue.ToString() + "' ");

                float MtnGen = MtnPaie + MtnPR + MtnTaxe + MtnTrai + MtnEntr + MtnPena + MtnAEntr;

                LbMtnPaieGen.Text = Configuration.ConvertToMonyC(MtnPaie).ToString();
                LbMtnPenaRGen.Text = Configuration.ConvertToMonyC(MtnPR).ToString();
                LbMtnTaxeGen.Text = Configuration.ConvertToMonyC(MtnTaxe).ToString();
                LbMtnTraiGen.Text = Configuration.ConvertToMonyC(MtnTrai).ToString();
                LbMtnEntrGen.Text = Configuration.ConvertToMonyC(MtnEntr).ToString();
                LbMtnPenaGen.Text = Configuration.ConvertToMonyC(MtnPena).ToString();
                LbMtnGen.Text = Configuration.ConvertToMonyC(MtnGen).ToString();
                LbMtnAEntrGen.Text = Configuration.ConvertToMonyC(MtnAEntr).ToString();
            }
        }       
        private void FilterGridEntrGen()
        {
            if (CbAnneesGen.SelectedValue != "")
            {
                dvEntrGen.RowFilter = "PeriodM='" + RGMonths.Properties.Items[RGMonths.SelectedIndex].Description + "' ";
            }
        }

        //// TileView
        private void RempTileViewAnnMtnT()
        {



            TabAnnesMtnT.Rows.Add("2013", "--- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH");
            TabAnnesMtnT.Rows.Add("2014", "--- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH");
            TabAnnesMtnT.Rows.Add("2015", "--- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH");
            TabAnnesMtnT.Rows.Add("2016", "--- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH");
            TabAnnesMtnT.Rows.Add("2017", "--- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH");
            TabAnnesMtnT.Rows.Add("2018", "--- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH", "---,-- DH");

            gridControlAnnesMtnT.DataSource = TabAnnesMtnT;
            gridControlAnnesMtnT.Refresh();

        }
        public void ReturnMtnOfAllEntres(string strAnne)
        {
            VMtnPaie = CalculRequte("select ReturnSumMtnPaie('" + strAnne + "')");
            VMtnPR = CalculRequte("select sum(PenaEntrPaie) from entpaie where PayeEntrPaie=1 and date_format(DatePEntPaie,\"%Y\")='" + strAnne + "' ");
            VMtnTaxe = CalculRequte("select sum(V_Prix) from lsthelpget_montsansfrais where V_PayePaie=1 and Date_Format(V_DatePaie,\"%Y\")='" + strAnne + "' ");
            VMtnTrai = CalculRequte("select sum(MontantMTr) from moistraite where PayerMTr=1 and date_format(DatePayerMTr,\"%Y\")='" + strAnne + "' ");
            VMtnEntr = CalculRequte("select sum(MontantEntr) from ententres where PayerEntr=1 and Date_Format(DatePayerEntr,\"%Y\")='" + strAnne + "' ");
            VMtnPena = CalculRequte("select sum(MontantPena) from entpena where date_format(DatePayerPena, \"%Y\") = '" + strAnne + "' ");
            VMtnAEntr = CalculRequte("select sum(MontantEntr) from entrescais where date_format(DateEntr, \"%Y\") = '" + strAnne + "' and SuppEntr=0 and CatEntres_IdCatEntr>4 ");
            VMtnGen = VMtnPaie + VMtnPR + VMtnTaxe + VMtnTrai + VMtnEntr + VMtnPena + VMtnAEntr;

            //VMtnGen = CalculRequte("select sum(MontantEntr) from entrescais where Date_Format(DateEntr,\"%Y\")='" + strAnne + "' ");
            //TabAnnesMtnT.Rows.Add(strAnne, Configuration.ConvertToMony(VMtnGen).ToString());
            TabAnnesMtnT.Rows.Add(strAnne, Configuration.ConvertToMony(VMtnPaie).ToString(), Configuration.ConvertToMony(VMtnPR).ToString() , Configuration.ConvertToMony(VMtnTaxe).ToString() , Configuration.ConvertToMony(VMtnTrai).ToString(), Configuration.ConvertToMony(VMtnEntr).ToString(), Configuration.ConvertToMony(VMtnPena).ToString(),Configuration.ConvertToMony(VMtnAEntr).ToString(), Configuration.ConvertToMony(VMtnGen).ToString());
        }
        private void RempTileViewWithReturnData()
        {
            TabAnnesMtnT = new DataTable();
            TabAnnesMtnT.TableName = "TableAMtnT";
            TabAnnesMtnT.Columns.Add("Annees", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnPaie", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnPR", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnTaxe", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnTrai", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnEntr", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnPena", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnAEntr", typeof(string));
            TabAnnesMtnT.Columns.Add("MtnTotale", typeof(string));

            for (int i = 0; i < CbAnneesGen.Items.Count; i++)
            {
                ReturnMtnOfAllEntres(CbAnneesGen.GetItemText(CbAnneesGen.Items[i]).ToString());
            }

            gridControlAnnesMtnT.DataSource = TabAnnesMtnT;
            gridControlAnnesMtnT.Refresh();
        }

        //DataTable TabB;
        //DataView dvDFin;
        //DateTime dt = new DateTime();
        //private void RempTableDFin(string strYear)
        //{
        //    TabB = new DataTable();
        //    TabB.TableName = "TabBName";
        //    TabB.Columns.Add("PeriodY", typeof(string));
        //    TabB.Columns.Add("PeriodM", typeof(string));
        //    TabB.Columns.Add("MtnSTaxes", typeof(double));
        //    TabB.Columns.Add("TypeEntrr", typeof(string));

        //    strYear = "11-11-" + strYear;
        //    dt = DateTime.Parse(strYear);
        //    MessageBox.Show("dt year = "+ dt.Year.ToString());

        //    var months = Enumerable.Range(1, 12).Select(p => new DateTime(dt.Year, p, 1));
        //    foreach (var month in months)
        //    {
        //        string strMonths = month.ToString("MM-yyyy");
        //        MessageBox.Show("First Foreach strMonths ="+ strMonths);

        //        foreach (DataRow item in ds.Tables["ListeEntrGene"].Rows)
        //        {
        //            ////Type 1 Paie
        //            if (item[3].ToString() == "1")
        //            {
        //                if (item[1].ToString() == strMonths)
        //                {
        //                    TabB.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[4].ToString());
        //                }
        //                else
        //                    TabB.Rows.Add(dt.Year.ToString(), strMonths, 0, item[4].ToString());
        //            }
        //            else if (item[3].ToString() == "2")
        //            {
        //                ////Type 2 Pena Paie
        //                if (item[1].ToString() == strMonths )
        //                {
        //                    TabB.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[4].ToString());
        //                }
        //                else
        //                    TabB.Rows.Add(dt.Year.ToString(), strMonths, 0, item[4].ToString());
        //            }
        //            else if (item[3].ToString() == "3")
        //            {
        //                ////Type 3 Taxes
        //                if (item[1].ToString() == strMonths )
        //                {
        //                    TabB.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[4].ToString());
        //                }
        //                else
        //                    TabB.Rows.Add(dt.Year.ToString(), strMonths, 0, item[4].ToString());
        //            }
        //            else if (item[3].ToString() == "4")
        //            {
        //                ////Type 4 Trai
        //                if (item[1].ToString() == strMonths )
        //                {
        //                    TabB.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[4].ToString());
        //                }
        //                else
        //                    TabB.Rows.Add(dt.Year.ToString(), strMonths, 0, item[4].ToString());
        //            }
        //            else if (item[3].ToString() == "5")
        //            {
        //                ////Type 5 Services
        //                if (item[1].ToString() == strMonths )
        //                {
        //                    TabB.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[4].ToString());
        //                }
        //                else
        //                    TabB.Rows.Add(dt.Year.ToString(), strMonths, 0, item[4].ToString());
        //            }
        //            else if (item[3].ToString() == "6")
        //            {
        //                ////Type 6 Pena
        //                if (item[1].ToString() == strMonths )
        //                {
        //                    TabB.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[4].ToString());
        //                }
        //                else
        //                    TabB.Rows.Add(dt.Year.ToString(), strMonths, 0, item[4].ToString());
        //            }
        //        }
        //    }
        //}
        //private void RempLVDFin()
        //{
        //    dvDFin = new DataView(TabB);

        //    TLEntresGen.DataSource = dvDFin;
        //    TLEntresGen.Refresh();
        //}
        //private void FilterDFin()
        //{
        //    try
        //    {
        //        if (CbAnneesGen.SelectedValue != "")
        //        {
        //            dvDFin.RowFilter = "PeriodM='" + RGMonths.Properties.Items[RGMonths.SelectedIndex].Description + "' ";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}


        private void CbAnneesGen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RGMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGridEntrGen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    LbAnneesGen.Text = CbAnneesGen.SelectedValue.ToString();
                    RempRgroupeMonths(CbAnneesGen.SelectedValue.ToString(),RGMonths);
                    RempDataEntresGene(CbAnneesGen.SelectedValue.ToString());
                    RempTableEntresGene();
                    FilterGridEntrGen();
                    RempFooterMtnGen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void BtnRempTileView_Click(object sender, EventArgs e)
        {
            RempTileViewWithReturnData();
        }

        ///////"""""""""""" Entres Paie"""""""""""""///////

        ///'''' Void 
        DateTime Dtyear = new DateTime();
        Boolean testEntreSelecIndexChang = false;
        Boolean dejaOvrTableEntresPaie = false;
        float SumMtnPaie,SumMtnPena,SumMtnTotale = 0;
        MySqlCommand mySqlCommand;

        private void RempCheckedListeMonths(string strr)
        {
            if (strr!="")
            {
                strr = "11-11-" + strr;
                Dtyear = DateTime.Parse(strr);
                ChLBMonths.Items.Clear();

                var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
                foreach (var month in months)
                {
                    string str = month.ToString("MM-yyyy");
                    if (str == DateTime.Now.ToString("MM-yyyy"))
                    {
                        ChLBMonths.Items.Add(str, true);
                    }
                    else
                    {
                        ChLBMonths.Items.Add(str, false);
                    }
                }
            }
        }
        private void LoadEntresPaie()
        {
            Configuration.RempCombo(CBAnnes, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons", "DateCons");
            CBAnnes.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            RempCheckedListeMonthsGen(CBAnnes.SelectedValue.ToString(),ChLBMonths);
            RempDataEntresPaie();
            RempGridEntresPaie();
            FilterGrid();
            RempFooterAnnees();
            testEntreSelecIndexChang = true;
        }
        private void RempDataEntresPaie()
        {
            if (dejaOvrTableEntresPaie == true)
            {
                ds.Tables["ListeEntrPaie"].Clear();
            }
            da = new MySqlDataAdapter("select IdEntPaie, IdPEntPaie as IdPaiePrinci, IdCEntPaie, IdFactEntPaie as IdFactPrinci, (select Concat(A.PrenomArAdhe,' ',A.NomArAdhe) from adherent A,consommation c where c.IdCons=IdCEntPaie and c.IdAdhCons=A.IdAdherent) as NomComp, (select NumComp from compteur CP,consommation CN where CN.IdCons=IdCEntPaie and CN.IdComp=CP.IdComp) as NComp, DatePEntPaie, DATE_FORMAT(PerioEntrPaie,\"%m-%Y\") as PerioEntrPaie, (select MontantPaie-(select sum(V_Prix) from lsthelpget_montsansfrais where V_configYesNon=1 and V_IdPaie= IdPaiePrinci and V_IdFAct=IdFactPrinci) from paiement where IdPaie=IdPaiePrinci and IdFact=IdFactPrinci ) as MontantSTaxe, PenaEntrPaie,(PenaEntrPaie + (select MontantPaie-(select sum(V_Prix) from lsthelpget_montsansfrais where V_configYesNon=1 and V_IdPaie= IdPaiePrinci and V_IdFAct=IdFactPrinci) from paiement where IdPaie=IdPaiePrinci and IdFact=IdFactPrinci )) as TotalePaiAndPena,DATE_FORMAT(DatePEntPaie,\"%m-%Y\") as DatePEntPaieformat from entpaie", ClassConnexion.Macon);
            da.Fill(ds, "ListeEntrPaie");
            dejaOvrTableEntresPaie = true;
        }
        private void RempGridEntresPaie()
        {
            dv = new DataView();
            dv.Table = ds.Tables["ListeEntrPaie"];
            gridControlEntresPaie.DataSource = dv;
            gridControlEntresPaie.Refresh();

            //gridViewEPaie.Columns[2].Visible = false;
            //gridViewEPaie.Columns[3].Visible = false;

            gridViewEPaie.Columns[0].Caption = "ر.ت";
            gridViewEPaie.Columns[1].Caption = "ر.الأداء";
            gridViewEPaie.Columns["NomComp"].Caption = "المشترك";
            gridViewEPaie.Columns["NComp"].Caption = "ر.العداد";
            gridViewEPaie.Columns["DatePEntPaie"].Caption = "ت.الأداء";
            gridViewEPaie.Columns["PerioEntrPaie"].Caption = "الفترة";
            gridViewEPaie.Columns["MontantSTaxe"].Caption = "م.بدون رسوم";
            gridViewEPaie.Columns["PenaEntrPaie"].Caption = "غرامة التأخر";
            gridViewEPaie.Columns["TotalePaiAndPena"].Caption = "المجموع";

            gridViewEPaie.Columns["MontantSTaxe"].ToolTip = "المبلغ بدون رسوم";
        }
        private void FilterGrid()
        {
            try
            {
                if (ChLBMonths.CheckedItemsCount == 0)
                    dv.RowFilter = "DatePEntPaieformat like '%" + CBAnnes.SelectedValue.ToString() + "' ";
                if (ChLBMonths.CheckedItemsCount>1)
                {
                    string Query2 = "";
                    int icount = 0;
                    foreach (var item in ChLBMonths.CheckedItems)
                    {
                        if(icount== ChLBMonths.CheckedItems.Count-1)
                            Query2 += " DatePEntPaieformat = '" + item + "'";
                        else
                            Query2 += " DatePEntPaieformat = '" + item + "' OR ";
                        icount++;
                    }


                    dv.RowFilter = Query2;
                }
                else if (ChLBMonths.CheckedItemsCount == 1)
                {
                    dv.RowFilter = "DatePEntPaieformat  = '" + ChLBMonths.CheckedItems[0].ToString() + "' ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private float CalculRequte(string Req)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            float Resu = 0;
            MySqlCommand Cmd = new MySqlCommand(Req, ClassConnexion.Macon);
            dr = Cmd.ExecuteReader();
            if(dr.Read())
            {
                if (dr[0].ToString() != "")
                {
                    Resu = float.Parse(dr[0].ToString());
                    dr.Close();
                    ClassConnexion.Macon.Close();
                    return Resu;
                }
            }
            dr.Close();
            ClassConnexion.Macon.Close();
            return 0;
        }
        private void RempFooterAnnees()
        {
            if (CBAnnes.Items.Count>0 && CBAnnes.SelectedValue.ToString() != "")
            {
                LbAnne.Text = CBAnnes.SelectedValue.ToString();

                SumMtnPaie = CalculRequte("select ReturnSumMtnPaie('" + CBAnnes.SelectedValue.ToString() + "')");
                SumMtnPena = CalculRequte("select sum(PenaEntrPaie) from entpaie where PayeEntrPaie=1 and date_format(DatePEntPaie,\"%Y\")='" + CBAnnes.SelectedValue.ToString() + "' ");
                SumMtnTotale = SumMtnPaie + SumMtnPena;

                LbMntPaie.Text = Configuration.ConvertToMony(SumMtnPaie); 
                LbMntPena.Text = Configuration.ConvertToMony(SumMtnPena);
                LbMntTotale.Text = Configuration.ConvertToMony(SumMtnTotale);
            }
        }

        ////''''' Code
        private void windowsUIButtonPanelPaie_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterGrid();
                    RempFooterAnnees();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CBAnnes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                while (testEntreSelecIndexChang == true)
                {
                    RempCheckedListeMonthsGen(CBAnnes.SelectedValue.ToString(), ChLBMonths);
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        ///////"""""""""""" Entres Traite """""""""""""///////

        ///'''' Void
        
        Boolean testEntreSelecIndexChangTrai=false;
        Boolean dejaOvrTableEntresTrai = false;
        float MtnTrai , SumMtnTrai  = 0;
        private void LoadEntresTrai()
        {
            try
            {
                Configuration.RempCombo(CbAnnesTrai, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateConsPena", "DateCons", "DateCons");
                CbAnnesTrai.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
                RempCheckedListeMonthsGen(CbAnnesTrai.SelectedValue.ToString(), ChLBTraite);
                RempDataEntresTrai();
                RempGridEntresTrai();
                FilterGridTrai();
                RempFooterAnneesTrai();
                testEntreSelecIndexChangTrai = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RempDataEntresTrai()
        {
            if (dejaOvrTableEntresTrai == true)
                ds.Tables["ListeEntrTrai"].Clear();

            da = new MySqlDataAdapter("select IdMTr,(select distinct concat(V.NomArAdhe,' ',V.PrenomArAdhe) from moistraite MTS,traite TS,compteurcdherentcecteur V where MTS.IdTrai=TS.IdTrai and TS.IdComp=V.IdComp and MT.IdTrai = MTS.IdTrai) as NomComp ,(select distinct V.NumComp from moistraite MTS,traite TS,compteurcdherentcecteur V where MTS.IdTrai=TS.IdTrai and TS.IdComp=V.IdComp and MT.IdTrai = MTS.IdTrai) as NumComp,MoisMTr,DatePayerMTr,date_format(DatePayerMTr,\"%m-%Y\") as PeriodPayerTrai,MontantMTr from moistraite MT where MT.PayerMTr=1 ", ClassConnexion.Macon);
            da.Fill(ds, "ListeEntrTrai");
            dejaOvrTableEntresTrai = true;
        }
        private void RempGridEntresTrai()
        {
            dv2 = new DataView();
            dv2.Table = ds.Tables["ListeEntrTrai"];
            gridControlTrai.DataSource = dv2;
            gridControlTrai.Refresh();

            gridViewETrai.Columns[0].Caption = "ر.ت";
            gridViewETrai.Columns["NomComp"].Caption = "المشترك";
            gridViewETrai.Columns["NumComp"].Caption = "ر.العداد";
            gridViewETrai.Columns["MoisMTr"].Caption = "الفترة";
            gridViewETrai.Columns["DatePayerMTr"].Caption = "تاريخ الأداء";
            gridViewETrai.Columns["MontantMTr"].Caption = "المبلغ";
        }
        private void FilterGridTrai()
        {
            try
            {
                if (ChLBTraite.CheckedItemsCount == 0)
                    dv2.RowFilter = "PeriodPayerTrai like '%" + CbAnnesTrai.SelectedValue.ToString() + "' ";

                //MessageBox.Show("ChLBTraite.CheckedItemsCount ==> "+ ChLBTraite.CheckedItemsCount.ToString());
                if (ChLBTraite.CheckedItemsCount > 1)
                {
                    string Query2 = "";
                    int icount = 0;
                    foreach (var item in ChLBTraite.CheckedItems)
                    {
                        if (icount == ChLBTraite.CheckedItems.Count - 1)
                            Query2 += " PeriodPayerTrai= '" + item + "'";
                        else
                            Query2 += " PeriodPayerTrai= '" + item + "' OR ";
                        icount++;
                    }
                    dv2.RowFilter = Query2;
                }
                else if (ChLBTraite.CheckedItemsCount == 1)
                {
                    dv2.RowFilter = "PeriodPayerTrai = '" + ChLBTraite.CheckedItems[0].ToString() + "' ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RempFooterAnneesTrai()
        {
            if (CbAnnesTrai.Items.Count > 0 && CbAnnesTrai.SelectedValue.ToString() != "")
            {
                LbAnneeTrai.Text = CbAnnesTrai.SelectedValue.ToString();
                SumMtnTrai = CalculRequte("select sum(MontantMTr) from moistraite where PayerMTr=1 and date_format(DatePayerMTr,\"%Y\")='" + CbAnnesTrai.SelectedValue.ToString() + "' ");

                LbMontantPTrai.Text = Configuration.ConvertToMony(SumMtnTrai);
            }
        }


        ///'''' Code
        private void windowsUIButtonPanelTrai_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterGridTrai();
                    RempFooterAnneesTrai();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbAnnesTrai_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                while (testEntreSelecIndexChangTrai == true)
                {
                    RempCheckedListeMonthsGen(CbAnnesTrai.SelectedValue.ToString(),ChLBTraite);
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        ///////"""""""""""" Entres Penalites """""""""""""///////

        ///'''' Void
        Boolean testEntreSelecIndexChangPena=false;
        Boolean dejaOvrTableEntresPena = false;
        float MtnP=0;
        private void LoadEntresPena()
        {
            Configuration.RempCombo(CbAnneesPena, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateConsPena", "DateCons", "DateCons");
            CbAnneesPena.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            RempCheckedListeMonthsGen(CbAnneesPena.SelectedValue.ToString(), ChLBPena);
            RempDataEntresPena();
            RempGridEntresPena();
            FilterGridPena();
            RempFooterAnneesPena();
            testEntreSelecIndexChangPena = true;
        }
        private void RempCheckedListeMonthsPena(string strr)
        {
            strr = "11-11-" + strr;
            Dtyear = DateTime.Parse(strr);
            ChLBPena.Items.Clear();

            var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
            foreach (var month in months)
            {
                string str = month.ToString("MM-yyyy");
                if (str == DateTime.Now.ToString("MM-yyyy"))
                    ChLBPena.Items.Add(str, true);
                else
                    ChLBPena.Items.Add(str, false);
            }
        }
        private void RempDataEntresPena()
        {
            if (dejaOvrTableEntresPena == true)
                ds.Tables["ListeEntrPena"].Clear();

            da = new MySqlDataAdapter("select IdEnt,IdPena,IdComp as IdC,(select NumComp from compteur where IdComp=IdC) as NumComp,(select Concat(A.PrenomArAdhe,' ',A.NomArAdhe) from compteur C,adherent A where C.IdComp=IdC and C.IdAdherent=A.IdAdherent) as NomComp,IdTypePena as IdTPena,(select LibelleTypePena from typepenalite where IdTypePena= IdTPena) as TypePena,DatePena,DATE_FORMAT(DatePena,\"%m-%Y\") as PerioEntrPena,DatePayerPena,MontantPena,DATE_FORMAT(DatePayerPena,\"%m-%Y\") as DatePayerPenaFRM from entpena", ClassConnexion.Macon);
            da.Fill(ds, "ListeEntrPena");
            dejaOvrTableEntresPena = true;
        }
        private void RempGridEntresPena()
        {
            dv3 = new DataView();
            dv3.Table = ds.Tables["ListeEntrPena"];
            gridControlPena.DataSource = dv3;
            gridControlPena.Refresh();

            gridViewPena.Columns[0].Caption = "ر.ت";
            gridViewPena.Columns[1].Caption = "ر.الغرامة";
            gridViewPena.Columns["NomComp"].Caption = "المشترك";
            gridViewPena.Columns["NumComp"].Caption = "ر.العداد";
            gridViewPena.Columns["TypePena"].Caption = "نوع الغرامة";
            gridViewPena.Columns["DatePena"].Caption = "ت.الغرامة";
            gridViewPena.Columns["DatePayerPena"].Caption = "ت.الأداء";
            gridViewPena.Columns["MontantPena"].Caption = "المجموع";
        }
        private void FilterGridPena()
        {
            try
            {
                if (ChLBPena.CheckedItemsCount == 0)
                    dv3.RowFilter = "DatePayerPenaFRM like '%" + CbAnneesPena.SelectedValue.ToString() + "' ";
                if (ChLBPena.CheckedItemsCount > 1)
                {
                    string Query2 = "";
                    int icount = 0;
                    foreach (var item in ChLBPena.CheckedItems)
                    {
                        if (icount == ChLBPena.CheckedItems.Count - 1)
                            Query2 += " DatePayerPenaFRM= '" + item + "'";
                        else
                            Query2 += " DatePayerPenaFRM= '" + item + "' OR ";
                        icount++;
                    }
                    dv3.RowFilter = Query2;
                }
                else if (ChLBPena.CheckedItemsCount == 1)
                    dv3.RowFilter = "DatePayerPenaFRM = '" + ChLBPena.CheckedItems[0].ToString() + "' ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RempFooterAnneesPena()
        {
            if (CbAnneesPena.Items.Count > 0 && CbAnneesPena.SelectedValue.ToString() != "")
            {
                MtnP = CalculRequte("select sum(MontantPena) from entpena where date_format(DatePayerPena, \"%Y\") = '" + CbAnneesPena.SelectedValue.ToString() + "' ");
                LbAnneesPena.Text = CbAnneesPena.SelectedValue.ToString();
                LbMtnPena.Text = Configuration.ConvertToMony(MtnP);
            }
        }

        ///'''' Code
        private void windowsUIButtonPanelPena_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterGridPena();
                    RempFooterAnneesPena();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbAnneesPena_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                while (testEntreSelecIndexChangPena == true)
                {
                    RempCheckedListeMonthsGen(CbAnneesPena.SelectedValue.ToString(), ChLBPena);
                    //RempCheckedListeMonthsPena(CbAnneesPena.SelectedValue.ToString());
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        ///////"""""""""""" Entres AutreEntres """""""""""""///////

        ///'''' Void
        Boolean testEntreSelecIndexChangAEntr = false;
        Boolean dejaOvrTableEntresAEntr = false;
        float MtnAEntr = 0;
        DataView dvAE;
        private void LoadEntresAEntr()
        {
            Configuration.RempCombo(CbAnneesAEntr, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateConsPena", "DateCons", "DateCons");
            CbAnneesAEntr.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            RempCheckedListeMonthsGen(CbAnneesAEntr.SelectedValue.ToString(), ChLBAEntr);
            RempDataEntresAEntr();
            RempGridEntresAEntr();
            FilterGridAEntr();
            RempFooterAnneesAEntr();
            testEntreSelecIndexChangAEntr = true;
        }
        private void RempCheckedListeMonthsAEntr(string strr)
        {
            strr = "11-11-" + strr;
            Dtyear = DateTime.Parse(strr);
            ChLBAEntr.Items.Clear();

            var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
            foreach (var month in months)
            {
                string str = month.ToString("MM-yyyy");
                if (str == DateTime.Now.ToString("MM-yyyy"))
                {
                    ChLBAEntr.Items.Add(str, true);
                }
                else
                {
                    ChLBAEntr.Items.Add(str, false);
                }
            }
        }
        private void RempDataEntresAEntr()
        {
            if (dejaOvrTableEntresAEntr == true)
                ds.Tables["EntresCais"].Clear();

            da = new MySqlDataAdapter("select E.IdEnt,CE.LibCatEntr,C.LibCais,E.DateEntr,E.MontantEntr,Date_Format(E.DateEntr,\"%Y\") as DateEntrY,Date_Format(E.DateEntr,\"%m-%Y\") as DateEntrM from entrescais E,catentres CE,caisse C where E.CatEntres_IdCatEntr = CE.IdCatEntr and E.Caisse_IdCais = C.IdCais and E.SuppEntr=0 and CE.IdCatEntr > 4", ClassConnexion.Macon);
            da.Fill(ds, "EntresCais");
            dejaOvrTableEntresAEntr = true;
        }
        private void RempGridEntresAEntr()
        {
            dvAE = new DataView();
            dvAE.Table = ds.Tables["EntresCais"];
            gridControlAEntr.DataSource = dvAE;
            gridControlAEntr.Refresh();
        }
        private void FilterGridAEntr()
        {
            try
            {
                if (ChLBAEntr.CheckedItemsCount == 0)
                {
                    dvAE.RowFilter = "DateEntrY like '%" + CbAnneesAEntr.SelectedValue.ToString() + "' ";
                }
                if (ChLBAEntr.CheckedItemsCount > 1)
                {
                    string Query2 = "";
                    int icount = 0;
                    foreach (var item in ChLBAEntr.CheckedItems)
                    {
                        if (icount == ChLBAEntr.CheckedItems.Count - 1)
                            Query2 += " DateEntrM= '" + item + "'";
                        else
                            Query2 += " DateEntrM= '" + item + "' OR ";
                        icount++;
                    }
                    dvAE.RowFilter = Query2;
                }
                else if (ChLBAEntr.CheckedItemsCount == 1)
                {
                    dvAE.RowFilter = "DateEntrM = '" + ChLBAEntr.CheckedItems[0].ToString() + "' ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RempFooterAnneesAEntr()
        {
            if (CbAnneesAEntr.Items.Count > 0 && CbAnneesAEntr.SelectedValue.ToString() != "")
            {
                MtnAEntr = CalculRequte("select sum(MontantEntr) from entrescais where CatEntres_IdCatEntr>4 and SuppEntr=0 and Date_Format(DateEntr,\"%Y\") = '" + CbAnneesAEntr.SelectedValue.ToString() + "' ");
                LbAnneAEntr.Text = CbAnneesAEntr.SelectedValue.ToString();
                LbMtnAEntr.Text = Configuration.ConvertToMony(MtnAEntr);
            }
        }

        ///'''' Code
        private void windowsUIButtonPanelAEntr_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterGridAEntr();
                    RempFooterAnneesAEntr();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbAnneesAEntr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                while (testEntreSelecIndexChangAEntr == true)
                {
                    RempCheckedListeMonthsGen(CbAnneesAEntr.SelectedValue.ToString(), ChLBAEntr);
                    //RempCheckedListeMonthsPena(CbAnneesPena.SelectedValue.ToString());
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        ///////"""""""""""" Entres Entr """""""""""""///////

        ///'''' Void
        Boolean testEntreSelecIndexChangEntr = false;
        Boolean dejaOvrTableEntresEntr = false;
        float MtnEntEntres =0; 
        private void LoadEntresEntr()
        {
            try
            {
                Configuration.RempCombo(CbAnneesEntr, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateConsPena", "DateCons", "DateCons");
                CbAnneesEntr.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
                RempCheckedListeMonthsGen(CbAnneesEntr.SelectedValue.ToString(), ChLBMonthsEntr);
                RempDataEntresEntr();
                RempGridEntresEntr();
                FilterGridEntr();
                RempFooterAnneesEntr();
                testEntreSelecIndexChangEntr = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RempCheckedListeMonthsEntr(string strr)
        {
            strr = "11-11-" + strr;
            Dtyear = DateTime.Parse(strr);
            ChLBMonthsEntr.Items.Clear();

            var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
            foreach (var month in months)
            {
                string str = month.ToString("MM-yyyy");
                if (str == DateTime.Now.ToString("MM-yyyy"))
                {
                    ChLBMonthsEntr.Items.Add(str, true);
                }
                else
                {
                    ChLBMonthsEntr.Items.Add(str, false);
                }
            }
        }
        private void RempDataEntresEntr()
        {
            if (dejaOvrTableEntresEntr == true)
                ds.Tables["ListeEntrEntr"].Clear();

            da = new MySqlDataAdapter("select IdEnt,IdEntr,IdComp as IdC,(select NumComp from compteur where IdComp=IdC) as NumComp,(select Concat(A.PrenomArAdhe,' ',A.NomArAdhe) from compteur C,adherent A where C.IdComp=IdC and C.IdAdherent=A.IdAdherent) as NomComp,IdCatEntr as IdTEntr,(select LibelleCatEntr from categorieentres where IdCatEntr= IdTEntr) as TypeEntr,DateEntr,DATE_FORMAT(DatePayerEntr,\"%m-%Y\") as PeriodEntrEntrs,DatePayerEntr,MontantEntr from ententres where PayerEntr=1 ", ClassConnexion.Macon);
            da.Fill(ds, "ListeEntrEntr");
            dejaOvrTableEntresEntr = true;
        }
        private void RempGridEntresEntr()
        {
            dv4 = new DataView();
            dv4.Table = ds.Tables["ListeEntrEntr"];
            gridControlEntr.DataSource = dv4;
            gridControlEntr.Refresh();

            gridViewEntr.Columns[0].Caption = "ر.ت";
            gridViewEntr.Columns[1].Caption = "ر.الخدمة";
            gridViewEntr.Columns["NomComp"].Caption = "المشترك";
            gridViewEntr.Columns["NumComp"].Caption = "ر.العداد";
            gridViewEntr.Columns["TypeEntr"].Caption = "نوع الخدمة";
            gridViewEntr.Columns["DateEntr"].Caption = "ت.الخدمة";
            gridViewEntr.Columns["DatePayerEntr"].Caption = "ت.الأداء";
            gridViewEntr.Columns["MontantEntr"].Caption = "المجموع";
        }
        private void FilterGridEntr()
        {
            try
            {
                if (ChLBMonthsEntr.CheckedItemsCount == 0)
                {
                    dv4.RowFilter = "PeriodEntrEntrs like '%"+CbAnneesEntr.SelectedValue.ToString()+"' ";
                }
                if (ChLBMonthsEntr.CheckedItemsCount > 1)
                {
                    string Query2 = "";
                    int icount = 0;
                    foreach (var item in ChLBMonthsEntr.CheckedItems)
                    {
                        if (icount == ChLBMonthsEntr.CheckedItems.Count - 1)
                            Query2 += " PeriodEntrEntrs= '" + item + "'";
                        else
                            Query2 += " PeriodEntrEntrs= '" + item + "' OR ";
                        icount++;
                    }
                    dv4.RowFilter = Query2;
                }
                if (ChLBMonthsEntr.CheckedItems.Count == 1)
                {
                    dv4.RowFilter = "PeriodEntrEntrs = '" + ChLBMonthsEntr.CheckedItems[0].ToString() + "' ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RempFooterAnneesEntr()
        {
            if (CbAnneesEntr.Items.Count > 0  && CbAnneesEntr.SelectedValue.ToString() != "")
            {
                MtnEntEntres = CalculRequte("select sum(MontantEntr) from ententres where PayerEntr=1 and Date_Format(DatePayerEntr,\"%Y\")='" + CbAnneesEntr.SelectedValue.ToString() + "' ");
                LbAnneesEntr.Text = CbAnneesEntr.SelectedValue.ToString();
                LbMtnEntr.Text = Configuration.ConvertToMony(MtnEntEntres);
            }
        }

        ///'''' Code
        private void windowsUIButtonPanelEntr_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterGridEntr();
                    RempFooterAnneesEntr();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbAnneesEntr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                while (testEntreSelecIndexChangEntr == true)
                {
                    RempCheckedListeMonthsGen(CbAnneesEntr.SelectedValue.ToString(), ChLBMonthsEntr);
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        ///////"""""""""""" Sorteis """""""""""""///////

        DataView dvSort;
        Boolean dejaOvrTableSort = false;
        Boolean testEntreSelecIndexChangSort = false;
        DataTable TabAnnesMtnSort;

        float MtnSortFooter,MtnCrediSort,MtnSortTileV = 0;

        ///'''' Void
        private void RempDataSortGene(string strYear)
        {
            dvSort = new DataView();

            if (dejaOvrTableSort == true)
                ds.Tables["Sorties"].Clear();

            using (MySqlCommand cmd = new MySqlCommand("dwm.RempDataSumSorties", ClassConnexion.Macon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter parm1 = new MySqlParameter("@StrYear", MySqlDbType.VarChar);
                parm1.Value = strYear;
                parm1.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(parm1);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(ds, "Sorties");
                    dejaOvrTableSort = true;
                    dvSort.Table = ds.Tables["Sorties"];
                }
            }
        }
        private void RempTableSort()
        {
            TLSort.DataSource = dvSort;
            TLSort.Refresh();
        }
        private void LoadSort()
        {
            Configuration.RempComboSimple(CbAnnesSort, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
            CbAnnesSort.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            RempRgroupeMonths(CbAnnesSort.SelectedValue.ToString(),RGMonthsSort);
            RempDataSortGene(CbAnnesSort.SelectedValue.ToString());
            RempTableSort();
            RempFooterMtnSort();

            if (testEntreSelecIndexChangSort == true)
            {
                FilterGridSort();
            }
            //RempTileViewDefaultValueSort();
            testEntreSelecIndexChangSort = true;

        }
        private void RempFooterMtnSort()
        {
            if (CbAnnesSort.Items.Count > 0 && CbAnnesSort.SelectedValue.ToString() != "")
            {
                MtnSortFooter = CalculRequte("select sum(MontantSort) from sortiescais where  Date_Format(DateSort,\"%Y\")='" + CbAnnesSort.SelectedValue.ToString() + "' and SuppSort=0 ");
                //MtnCrediSort = CalculRequte("select sum(MontantSort) from sortiescais where  Date_Format(DateSort,\"%Y\")='" + CbAnnesSort.SelectedValue.ToString() + "' and SuppSort=0 and CrediSort=1 ");
                lbAnnesSort.Text = CbAnnesSort.SelectedValue.ToString();
                LbTotaleSort.Text = Configuration.ConvertToMony(MtnSortFooter);
            }
        }
        private void FilterGridSort()
        {
            if (CbAnnesSort.SelectedValue != "")
                dvSort.RowFilter = "MonthsSort ='" + RGMonthsSort.Properties.Items[RGMonthsSort.SelectedIndex].Description + "' ";
        }

        //// TileView
        private void RempTileViewDefaultValueSort()
        {


            TabAnnesMtnSort.Rows.Add("2014", "12 000,00");
            TabAnnesMtnSort.Rows.Add("2015", "199,50");
            TabAnnesMtnSort.Rows.Add("2016", "2 250,00");
            TabAnnesMtnSort.Rows.Add("2017", "200,00");
            TabAnnesMtnSort.Rows.Add("2018", "120,00");

            gridControlSort.DataSource = TabAnnesMtnSort;
            gridControlSort.Refresh();

        }


        public void ReturnMtnOfSort(string strAnne)
        {
            MtnSortTileV = CalculRequte("select sum(MontantSort) from sortiescais where SuppSort=0 and  Date_Format(DateSort,\"%Y\")='" + strAnne + "' ");

            TabAnnesMtnSort.Rows.Add(strAnne, Configuration.ConvertToMony(MtnSortTileV).ToString());
        }
        private void RempTileViewWithReturnDataSort()
        {
            TabAnnesMtnSort = new DataTable();

            TabAnnesMtnSort.TableName = "TableMtnSort";
            TabAnnesMtnSort.Columns.Add("Annees", typeof(string));
            TabAnnesMtnSort.Columns.Add("MtnTotale", typeof(string));

            for (int i = 0; i < CbAnnesSort.Items.Count; i++)
            {
                ReturnMtnOfSort(CbAnnesSort.GetItemText(CbAnnesSort.Items[i]).ToString());
            }

            gridControlSort.DataSource = TabAnnesMtnSort;
            gridControlSort.Refresh();
        }

        //// Code
        private void CbAnnesSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //while (testEntreSelecIndexChangGen == true)
                //{



                //    break;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RGMonthsSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGridSort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanel2_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    RempRgroupeMonths(CbAnnesSort.SelectedValue.ToString(), RGMonthsSort);
                    RempDataSortGene(CbAnnesSort.SelectedValue.ToString());
                    RempTableSort();
                    FilterGridSort();
                    RempFooterMtnSort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void SBtnRempTileSort_Click(object sender, EventArgs e)
        {
            RempTileViewWithReturnDataSort();
        }



        ///////"""""""""""" Caisse """""""""""""///////

        Boolean TestDejaOvrCaisse = false;

        DataView dvCais;

        int IndexIdCais =-1;

        ////Void
        private void LoadCaisse()
        {
            Configuration.RempComboSimple(CBAnnesCaiss, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
            CBAnnesCaiss.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            RempDataCaisse();
            RempGridCaisse();
            if(gridViewCais.DataRowCount > 0 )
            {
                IndexIdCais = int.Parse(gridViewCais.GetDataRow(gridViewCais.FocusedRowHandle)[0].ToString());
                FillFooterCais();
            }
        }
        private void RempDataCaisse()
        {
            dvCais = new DataView();

            if (TestDejaOvrCaisse ==true)
                ds.Tables["Caisse"].Clear();

            da = new MySqlDataAdapter("select IdCais,LibCais,RefCais,DescCais,IdUser,(select concat(NomUser,' ',PrenomUser) from utilisateurs where IdUser = C.IdUser) as User from caisse C", ClassConnexion.Macon);
            da.Fill(ds, "Caisse");
            dvCais.Table = ds.Tables["Caisse"];
            TestDejaOvrCaisse = true;
        }
        private void RempGridCaisse()
        {
            gridControlCais.DataSource = dvCais;
            gridControlCais.Refresh();
        }
        private void RempFooterCais(int IdC)
        {
            float EntrCais, SortCais, ResteCais,Credit = 0;

            Credit = CalculeRequte("select sum(MontantCrd) from credit where PaieCrd=0 and SuppCrd=0 and IdCais="+IdC+" ");
            EntrCais = CalculRequte("select sum(MontantEntr) from entrescais where SuppEntr=0 and Caisse_IdCais="+IdC+" ");
            SortCais = CalculRequte("select sum(MontantSort) from sortiescais where SuppSort=0 and Caisse_IdCais=" + IdC + " ");
            ResteCais = EntrCais - SortCais;

            LbAnneesCais.Text = "- - - -";
            LbCaisCred.Text = Configuration.ConvertToMony(Credit);
            LbCaisEntr.Text = Configuration.ConvertToMony(EntrCais);
            LbCaisSort.Text = Configuration.ConvertToMony(SortCais);
            LbCaisReste.Text = Configuration.ConvertToMony(ResteCais);
        }

        private void RempFooterCaisWithDate(int IdC,string Annee)
        {
            float EntrCais, SortCais, ResteCais = 0;

            EntrCais = CalculRequte("select sum(MontantEntr) from entrescais where SuppEntr=0 and Caisse_IdCais=" + IdC + " and Date_Format(DateEntr,\"%Y\")='"+ Annee + "' ");
            SortCais = CalculRequte("select sum(MontantSort) from sortiescais where SuppSort=0 and Caisse_IdCais=" + IdC + " and Date_Format(DateSort,\"%Y\")='" + Annee + "' ");
            ResteCais = EntrCais - SortCais;

            LbAnneesCais.Text = Annee;
            LbCaisEntr.Text = Configuration.ConvertToMony(EntrCais);
            LbCaisSort.Text = Configuration.ConvertToMony(SortCais);
            LbCaisReste.Text = Configuration.ConvertToMony(ResteCais);
        }
        private void FillFooterCais()
        {
            if (IndexIdCais >= 0)
            {
                if (ChBAnnesCais.Checked == false)
                    RempFooterCais(IndexIdCais);
                else
                    if (CBAnnesCaiss.SelectedValue != "")
                    RempFooterCaisWithDate(IndexIdCais, CBAnnesCaiss.SelectedValue.ToString());
            }
        }

        //// Code
        private void ChBAnnesCais_CheckedChanged(object sender, EventArgs e)
        {
            if (ChBAnnesCais.Checked == true)
            {
                CBAnnesCaiss.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
                CBAnnesCaiss.Enabled = true;
                FillFooterCais();
            }
            else
            {
                CBAnnesCaiss.Text = "";
                CBAnnesCaiss.Enabled = false;
                FillFooterCais();
            }
        }
        private void windowsUIButtonPanelCais_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    LoadCaisse();
                    FillFooterCais();
                }
                else if (e.Button.Properties.Tag == "ListeOpr")
                {
                    if (IndexIdCais >= 0)
                    {
                        InfoCaisse IC = new InfoCaisse(IndexIdCais);
                        IC.ShowDialog(this);
                    }
                    else
                        XtraMessageBox.Show(" لم يتم تحديد أي حساب", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                else if (e.Button.Properties.Tag == "Print")
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridViewCais_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                IndexIdCais = int.Parse(gridViewCais.GetDataRow(gridViewCais.FocusedRowHandle)[0].ToString());

                FillFooterCais();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridViewCais_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
        }
        private void CBAnnesCaiss_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillFooterCais();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        ///////"""""""""""" Report """""""""""""///////

        Boolean TestDejaOvrireR = false;

        //// Void
        private void RempData(string Req1, string Req2)
        {
            if (TestDejaOvrireR == true)
            {
                ds.Tables["REntres"].Clear();
                ds.Tables["RSorties"].Clear();
            }

            da = new MySqlDataAdapter(Req1, ClassConnexion.Macon);
            da.Fill(ds, "REntres");

            da = new MySqlDataAdapter(Req2, ClassConnexion.Macon);
            da.Fill(ds, "RSorties");
            TestDejaOvrireR = true;
        }
        private void RempGrid()
        {
            gridControlREntr.DataSource = ds.Tables["REntres"];
            gridControlRSort.DataSource = ds.Tables["RSorties"];
            gridControlREntr.Refresh();
            gridControlRSort.Refresh();
        }
        private void FilterData()
        {
            string Re1 = "", Re2 = "";
            DateTime DateAnnePre = new DateTime();

            if (CbAnn.SelectedValue != "")
            {

                if (checkBoxAddOldMtn.Checked == false)
                {
                    //Re1 = "select IFNULL(sum(ec.MontantEntr), 0) as MtnEntr, ce.LibCatEntr as LibEntr from entrescais ec,catentres ce where ec.CatEntres_IdCatEntr = ce.IdCatEntr and ec.SuppEntr = 0 and DATE_FORMAT(DateEntr,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' group by ec.CatEntres_IdCatEntr";
                    Re1 = "select IFNULL(sum(SecTable.MtnEntr),0) as MtnEntr,SecTable.NewCatEntr as LibEntr  from ( select IFNULL(sum(MontantEntr), 0) as MtnEntr, case when IdCatEntr<= 4 then LibCatEntr else 'مداخيل اخرى' end as NewCatEntr from entrescais,catentres where CatEntres_IdCatEntr = IdCatEntr and SuppEntr = 0 and Date_Format(DateEntr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' group by CatEntres_IdCatEntr asc ) as SecTable group by SecTable.NewCatEntr asc order by SecTable.NewCatEntr desc";
                }
                else
                {
                    //Re1 = "select IFNULL(((select sum(MontantEntr) from entrescais where SuppEntr=0 and Date_Format(DateEntr,\"%Y\")='" + (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString() + "')- (select sum(MontantSort) from sortiescais where SuppSort=0 and Date_Format(DateSort,\"%Y\")='" + (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString() + "')),0) as MtnEntr,'رصيد السنة الماضية' as LibEntr UNION ALL select IFNULL(sum(ec.MontantEntr), 0) as MtnEntr, ce.LibCatEntr as LibEntr from entrescais ec,catentres ce where ec.CatEntres_IdCatEntr = ce.IdCatEntr and ec.SuppEntr = 0 and DATE_FORMAT(DateEntr,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' group by ec.CatEntres_IdCatEntr ";
                    Re1 = "select IFNULL(((select sum(MontantEntr) from entrescais where SuppEntr=0 and Date_Format(DateEntr,\"%Y\")='" + (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString() + "')- (select sum(MontantSort) from sortiescais where SuppSort=0 and Date_Format(DateSort,\"%Y\")='" + (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString() + "')),0) as MtnEntr,'رصيد السنة الماضية' as LibEntr UNION ALL select IFNULL(sum(SecTable.MtnEntr),0) as MtnEntr,SecTable.NewCatEntr as LibEntr  from ( select IFNULL(sum(MontantEntr), 0) as MtnEntr, case when IdCatEntr<= 4 then LibCatEntr else 'مداخيل اخرى' end as NewCatEntr from entrescais,catentres where CatEntres_IdCatEntr = IdCatEntr and SuppEntr = 0 and Date_Format(DateEntr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' group by CatEntres_IdCatEntr asc ) as SecTable group by SecTable.NewCatEntr asc order by LibEntr desc ";
                }
                Re2 = "select IFNULL(sum(sc.MontantSort), 0) as MtnSort,cs.LibCatSort as LibSort  from sortiescais sc,catsorties cs where sc.CatSorties_IdCatSort = cs.IdCatSort and SuppSort = 0 and DATE_FORMAT(DateSort,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "'  group by sc.CatSorties_IdCatSort";
            }

            RempData(Re1, Re2);
            RempGrid();
        }
        private float CalculeRequte(string Req)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();
            float Resu = 0;
            using (MySqlCommand Cmd = new MySqlCommand(Req, ClassConnexion.Macon))
            {
                dr = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.Read())
                {
                    if (dr[0].ToString() != "")
                    {
                        Resu = float.Parse(dr[0].ToString());
                        dr.Close();
                        return Resu;
                    }
                }
                dr.Close();
                return 0;
            }
        }
        private void RempFooter()
        {
            float MtnAdhe, MtnEntr, MtnSort, MtnRest, MtnCred, MtnCrean, MtnCais, MtnBan, MtnTotal;
            MtnAdhe = MtnEntr = MtnSort = MtnRest = MtnCred = MtnCrean = MtnCais = MtnBan = MtnTotal = 0;
            MtnAdhe = CalculeRequte("select count(*) from adherent where ExiAdhe=1");

            if (ds.Tables["REntres"].Rows.Count > 0)
                MtnEntr = float.Parse(ds.Tables["REntres"].Compute("Sum(MtnEntr)", "").ToString());
            else
                MtnEntr = 0;

            if (ds.Tables["RSorties"].Rows.Count > 0)
                MtnSort = float.Parse(ds.Tables["RSorties"].Compute("Sum(MtnSort)", "").ToString());
            else
                MtnSort = 0;

            //MtnEntr = CalculeRequte("select sum(MontantEntr) from entrescais where SuppEntr=0 and Date_Format(DateEntr,\"%Y\") ='"+CbAnn.SelectedValue.ToString()+"' ");
            //MtnSort = CalculeRequte("select sum(MontantSort) from sortiescais where SuppSort=0 and Date_Format(DateSort,\"%Y\") ='"+CbAnn.SelectedValue.ToString()+"' ");
            MtnRest = MtnEntr - MtnSort;

            float MtnRcais = CalculeRequte("select (select sum(MontantEntr) from entrescais where SuppEntr =0 and Caisse_IdCais=1 and Date_Format(DateEntr,\"%Y\") ='" + CbAnn.SelectedValue.ToString() + "')- (select sum(MontantSort) from sortiescais where SuppSort =0 and Caisse_IdCais=1 and Date_Format(DateSort,\"%Y\") ='" + CbAnn.SelectedValue.ToString() + "' )");
            float MtnTransP = CalculeRequte("select sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisDes=1 and Date_Format(DateTransf,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' ");
            float MtnTransN = CalculeRequte("select sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisSrc=1 and Date_Format(DateTransf,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' ");
            MtnCais = (MtnRcais + MtnTransP) - MtnTransN;
            MtnBan = MtnRest - MtnCais;

            MtnCrean = CalculeRequte("select (select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and date_format(F.PeriodeConsoFact,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and date_format(DateAutFrai,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' )+ (select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and date_format(DatePena,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "') ");
            MtnCred = CalculeRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0 and Date_Format(DateCrd,\"%Y\") = '" + CbAnn.SelectedValue.ToString() + "' ");
            MtnTotal = (MtnRest + MtnCrean) - MtnCred;

            LbAnnRapp.Text = CbAnn.SelectedValue.ToString();
            LbMtnAdh.Text = MtnAdhe.ToString();
            LbRMtnEntr.Text = Configuration.ConvertToMony(MtnEntr);
            LbMtnSort.Text = Configuration.ConvertToMony(MtnSort);
            LbMtnRest.Text = Configuration.ConvertToMony(MtnRest);

            LbMtnCais.Text = Configuration.ConvertToMony(MtnCais);
            LbMtnCaisB.Text = Configuration.ConvertToMony(MtnBan);

            LbMtnCrean.Text = Configuration.ConvertToMony(MtnCrean);
            LbMtnCred.Text = Configuration.ConvertToMony(MtnCred);
            LbMntTot.Text = Configuration.ConvertToMony(MtnTotal);

        }

        ////Load
        private void LoadReport()
        {
            CbAnn.SelectedIndexChanged -= new EventHandler(CbAnn_SelectedIndexChanged);
            Configuration.RempComboSimple(CbAnn, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
            CbAnn.SelectedIndexChanged += new EventHandler(CbAnn_SelectedIndexChanged);
            CbAnn.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");

            FilterData();
            RempFooter();
        }

        //// Code

        private void checkBoxAddOldMtn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FilterData();
                RempFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbAnn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterData();
                RempFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelReport_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterData();
                    RempFooter();
                }
                else if (e.Button.Properties.Tag == "Print")
                {
                    ConfigImpressReportFina CPrint = new ConfigImpressReportFina(checkBoxAddOldMtn.Checked);
                    CPrint.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /////
        DataSet dschart;
        ///// Void 
        string SelectedYear = "";
        private void RempDataGraphEntres(string strYear,int strType, string NameTab)
        {
            //if (dejaOvrTableEntresGene == true)
            //    dschart.Tables["ListeEntrGene"].Clear();

            using (MySqlCommand cmd = new MySqlCommand("dwm.RempGraphEntres", ClassConnexion.Macon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter parm1 = new MySqlParameter("@StrYear", MySqlDbType.VarChar);
                parm1.Value = strYear;
                parm1.Direction = ParameterDirection.Input;

                MySqlParameter parm2 = new MySqlParameter("@StrType", MySqlDbType.VarChar);
                parm2.Value = strType;
                parm2.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(parm1);
                cmd.Parameters.Add(parm2);

                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(dschart, NameTab);

                }
            }
        }
        private void RempDataChart()
        {
            dschart = new DataSet();

            RempDataGraphEntres(SelectedYear, 1, "dsPaie");
            RempDataGraphEntres(SelectedYear, 2, "dsTrai");
            RempDataGraphEntres(SelectedYear, 3, "dsServ");
            RempDataGraphEntres(SelectedYear, 4, "dsPena");
            RempDataGraphEntres(SelectedYear, 5, "dsAEntr");

        }
        private void RempChart()
        {
            chartControlEntres.BeginInit();

            if (chartControlEntres.Titles.Count != 0)
            {
                chartControlEntres.Titles.RemoveAt(0);
            }

            ChartTitle titre = new ChartTitle();
            titre.Text = " بيان المداخيل لسنة  : " + CbYearChart.Text;
            chartControlEntres.Titles.Add(titre);

            Series SPaie = chartControlEntres.Series[0];
            Series STrai = chartControlEntres.Series[1];
            Series SServ = chartControlEntres.Series[2];
            Series SPena = chartControlEntres.Series[3];
            Series SAEntr = chartControlEntres.Series[4];

            SPaie.Points.Clear();
            STrai.Points.Clear();
            SServ.Points.Clear();
            SPena.Points.Clear();
            SAEntr.Points.Clear();

            for (int i = 0; i < dschart.Tables["dsAEntr"].Rows.Count; i++)
            {
                SPaie.Points.Add(new SeriesPoint(dschart.Tables["dsPaie"].Rows[i]["PeriodM"].ToString(), dschart.Tables["dsPaie"].Rows[i]["Mtn"].ToString()));
                STrai.Points.Add(new SeriesPoint(dschart.Tables["dsTrai"].Rows[i]["PeriodM"].ToString(), dschart.Tables["dsTrai"].Rows[i]["Mtn"].ToString()));
                SServ.Points.Add(new SeriesPoint(dschart.Tables["dsServ"].Rows[i]["PeriodM"].ToString(), dschart.Tables["dsServ"].Rows[i]["Mtn"].ToString()));
                SPena.Points.Add(new SeriesPoint(dschart.Tables["dsPena"].Rows[i]["PeriodM"].ToString(), dschart.Tables["dsPena"].Rows[i]["Mtn"].ToString()));
                SAEntr.Points.Add(new SeriesPoint(dschart.Tables["dsAEntr"].Rows[i]["PeriodM"].ToString(), dschart.Tables["dsAEntr"].Rows[i]["Mtn"].ToString()));

            }
            chartControlEntres.EndInit();
        }
        private void LoadPageGraph()
        {
            CbYearChart.SelectedIndexChanged -= new EventHandler(CbYearChart_SelectedIndexChanged);
            Configuration.RempCombo(CbYearChart, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons", "DateCons");
            CbYearChart.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            SelectedYear = CbYearChart.SelectedValue.ToString();
            CbYearChart.SelectedIndexChanged += new EventHandler(CbYearChart_SelectedIndexChanged);

           
            RempDataChart();
            RempChart();
        }

        ///// Code

        private void CbYearChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedYear = CbYearChart.SelectedValue.ToString();
                RempDataChart();
                RempChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void simpleButtonPrintChart_Click(object sender, EventArgs e)
        {
            try
            {
                chartControlEntres.ShowRibbonPrintPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

