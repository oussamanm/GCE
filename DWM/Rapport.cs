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

namespace DWM
{
    public partial class Rapport : DevExpress.XtraEditors.XtraForm
    {
        public Rapport()
        {
            InitializeComponent();
        }

        DataSet ds;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        Boolean TestDejaOvrire = false;

        private void RempData(string Req1,string Req2)
        {
            if (TestDejaOvrire== true)
            {
                ds.Tables["Entres"].Clear();
                ds.Tables["Sorties"].Clear();
            }

            da = new MySqlDataAdapter(Req1,ClassConnexion.Macon);
            da.Fill(ds, "Entres");

            da = new MySqlDataAdapter(Req2, ClassConnexion.Macon);
            da.Fill(ds, "Sorties");
            TestDejaOvrire = true;
        }
        private void RempGrid()
        {
            gridControlEntr.DataSource = ds.Tables["Entres"];
            gridControlSort.DataSource = ds.Tables["Sorties"];
            gridControlEntr.Refresh();
            gridControlSort.Refresh();
        }
        private void FilterData()
        {
            string Re1="", Re2 = "";
            DateTime DateAnnePre = new DateTime();

            if (checkBoxAnn.Checked == false && checkBoxAddOldMtn.Checked== false)
            {
                Re1 = "select IFNULL(sum(ec.MontantEntr), 0) as MtnEntr, case when ec.CatEntres_IdCatEntr = 3 then(select c.LibelleCatEntr from entres e, categorieentres c where e.IdEntr = ec.IdSrcEntr and e.IdCatEntr = c.IdCatEntr) else ce.LibCatEntr end as LibEntr from entrescais ec,catentres ce where ec.CatEntres_IdCatEntr = ce.IdCatEntr and ec.SuppEntr = 0 group by ec.CatEntres_IdCatEntr";
                Re2 = "select IFNULL(sum(sc.MontantSort), 0) as MtnSort,cs.LibCatSort as LibSort  from sortiescais sc,catsorties cs where sc.CatSorties_IdCatSort = cs.IdCatSort and SuppSort = 0 group by sc.CatSorties_IdCatSort";
            }
            else if (checkBoxAnn.Checked == true)
            {
                if(CbAnn.SelectedValue != "")
                {

                    if (checkBoxAddOldMtn.Checked == false)
                        Re1 = "select IFNULL(sum(ec.MontantEntr), 0) as MtnEntr, case when ec.CatEntres_IdCatEntr = 3 then(select c.LibelleCatEntr from entres e, categorieentres c where e.IdEntr = ec.IdSrcEntr and e.IdCatEntr = c.IdCatEntr) else ce.LibCatEntr end as LibEntr from entrescais ec,catentres ce where ec.CatEntres_IdCatEntr = ce.IdCatEntr and ec.SuppEntr = 0 and DATE_FORMAT(DateEntr,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' group by ec.CatEntres_IdCatEntr";
                    else
                        Re1 = "select IFNULL(((select sum(MontantEntr) from entrescais where SuppEntr=0 and Date_Format(DateEntr,\"%Y\")='"+ (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString() + "')- (select sum(MontantSort) from sortiescais where SuppSort=0 and Date_Format(DateSort,\"%Y\")='" + (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString() + "')),0) as MtnEntr,'رصيد السنة الماضية' as LibEntr UNION ALL select IFNULL(sum(ec.MontantEntr), 0) as MtnEntr, case when ec.CatEntres_IdCatEntr = 3 then(select c.LibelleCatEntr from entres e, categorieentres c where e.IdEntr = ec.IdSrcEntr and e.IdCatEntr = c.IdCatEntr) else ce.LibCatEntr end as LibEntr from entrescais ec,catentres ce where ec.CatEntres_IdCatEntr = ce.IdCatEntr and ec.SuppEntr = 0 and DATE_FORMAT(DateEntr,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' group by ec.CatEntres_IdCatEntr ";

                    Re2 = "select IFNULL(sum(sc.MontantSort), 0) as MtnSort,cs.LibCatSort as LibSort  from sortiescais sc,catsorties cs where sc.CatSorties_IdCatSort = cs.IdCatSort and SuppSort = 0 and DATE_FORMAT(DateSort,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "'  group by sc.CatSorties_IdCatSort";
                }
            }
            RempData(Re1,Re2);
            RempGrid();
        }
        private float CalculeRequte(string Req)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();
            float Resu = 0;
            using (MySqlCommand Cmd = new MySqlCommand(Req,ClassConnexion.Macon))
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
        private void Rapport_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                FilterData();
                RempFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RempFooter()
        {
            float MtnAdhe, MtnEntr, MtnSort, MtnRest, MtnCred, MtnCrean, MtnCais, MtnBan, MtnTotal ;
            MtnAdhe = MtnEntr = MtnSort = MtnRest = MtnCred = MtnCrean = MtnCais = MtnBan = MtnTotal = 0;
            MtnAdhe = CalculeRequte("select count(*) from adherent where ExiAdhe=1");

            MtnEntr = float.Parse(ds.Tables["Entres"].Compute("Sum(MtnEntr)","").ToString());
            MtnSort = float.Parse(ds.Tables["Sorties"].Compute("Sum(MtnSort)", "").ToString());

            if (checkBoxAnn.Checked == false)
            {
                //MtnEntr = CalculeRequte("select sum(MontantEntr) from entrescais where SuppEntr=0");
                //MtnSort = CalculeRequte("select sum(MontantSort) from sortiescais where SuppSort=0");
                MtnRest = MtnEntr - MtnSort;

                float MtnRcais = CalculeRequte("select (select sum(MontantEntr) from entrescais where SuppEntr =0 and Caisse_IdCais=1)- (select sum(MontantSort) from sortiescais where SuppSort =0 and Caisse_IdCais=1)");
                float MtnTransP = CalculeRequte("select sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisDes=1");
                float MtnTransN = CalculeRequte("select sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisSrc=1");
                MtnCais = (MtnRcais + MtnTransP) - MtnTransN;
                MtnBan = MtnRest - MtnCais;

                MtnCrean = CalculeRequte("select (select IFNULL(sum(MontantPaie),0) from paiement where PayePaie=0)+ (select IFNULL(sum(MontantMTr),0) from moistraite where PayerMTr=0)+ (select IFNULL(sum(MontantAutFrai),0) from autrefrais where PayerEntr=0)+ (select IFNULL(sum(MontantPena),0) from penalite where PayerPena=0)");
                MtnCred = CalculeRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0");
                MtnTotal = (MtnRest + MtnCrean) - MtnCred;
            }
            else
            {
                //MtnEntr = CalculeRequte("select sum(MontantEntr) from entrescais where SuppEntr=0 and Date_Format(DateEntr,\"%Y\") ='"+CbAnn.SelectedValue.ToString()+"' ");
                //MtnSort = CalculeRequte("select sum(MontantSort) from sortiescais where SuppSort=0 and Date_Format(DateSort,\"%Y\") ='"+CbAnn.SelectedValue.ToString()+"' ");
                MtnRest = MtnEntr - MtnSort;

                float MtnRcais = CalculeRequte("select (select sum(MontantEntr) from entrescais where SuppEntr =0 and Caisse_IdCais=1 and Date_Format(DateEntr,\"%Y\") ='"+CbAnn.SelectedValue.ToString()+ "')- (select sum(MontantSort) from sortiescais where SuppSort =0 and Caisse_IdCais=1 and Date_Format(DateSort,\"%Y\") ='" + CbAnn.SelectedValue.ToString() + "' )");
                float MtnTransP = CalculeRequte("select sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisDes=1 and Date_Format(DateTransf,\"%Y\")= '"+CbAnn.SelectedValue.ToString()+"' ");
                float MtnTransN = CalculeRequte("select sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisSrc=1 and Date_Format(DateTransf,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "' ");
                MtnCais = (MtnRcais + MtnTransP) - MtnTransN;
                MtnBan = MtnRest - MtnCais;

                MtnCrean = CalculeRequte("select (select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and date_format(F.PeriodeConsoFact,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and date_format(DateAutFrai,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' )+ (select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and date_format(DatePena,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "') ");
                MtnCred = CalculeRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0 and Date_Format(DateCrd,\"%Y\") = '" + CbAnn.SelectedValue.ToString() + "' ");
                MtnTotal = (MtnRest + MtnCrean) - MtnCred;
            }

            LbMtnAdh.Text = Configuration.ConvertToMony(MtnAdhe);
            LbMtnEntr.Text = Configuration.ConvertToMony(MtnEntr);
            LbMtnSort.Text = Configuration.ConvertToMony(MtnSort);
            LbMtnRest.Text = Configuration.ConvertToMony(MtnRest);

            LbMtnCais.Text = Configuration.ConvertToMony(MtnCais);
            LbMtnCaisB.Text = Configuration.ConvertToMony(MtnBan);

            LbMtnCrean.Text = Configuration.ConvertToMony(MtnCrean);
            LbMtnCred.Text = Configuration.ConvertToMony(MtnCred);
            LbMntTot.Text = Configuration.ConvertToMony(MtnTotal);

        }
        private void checkBoxAnn_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAnn.Checked == true)
            {
                CbAnn.Enabled = true;
                checkBoxAddOldMtn.Enabled = true;
                CbAnn.SelectedIndexChanged -= new EventHandler(CbAnn_SelectedIndexChanged);
                Configuration.RempComboSimple(CbAnn, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
                CbAnn.SelectedIndexChanged += new EventHandler(CbAnn_SelectedIndexChanged);
                CbAnn.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");

            }
            else
            {
                CbAnn.Enabled = false;
                checkBoxAddOldMtn.Checked = false;
                checkBoxAddOldMtn.Enabled = false;
                CbAnn.Text = "";
            }
            FilterData();
            RempFooter();
        }
        private void checkBoxAddOldMtn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxAddOldMtn.Checked == true)
                {
                    if (checkBoxAnn.Checked == false)
                    {
                        checkBoxAddOldMtn.Checked = false;
                        return;
                    }
                    FilterData();
                    RempFooter();
                }
                else
                {
                    FilterData();
                    RempFooter();
                }
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

        private void windowsUIButtonPanelCrean_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    FilterData();
                    RempFooter();
                }
                else if (e.Button.Properties.Tag == "Printt")
                {
                    MessageBox.Show("Test");
                    ConfigImpressReportFina CPrint = new ConfigImpressReportFina();
                    CPrint.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}