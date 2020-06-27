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
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace DWM
{
    public partial class S_ListeConso : DevExpress.XtraEditors.XtraForm
    {
        public S_ListeConso()
        {
            InitializeComponent();
        }

        DataSet ds;
        MySqlDataAdapter da;
        DataView dv;
        MySqlDataReader dr;
        Boolean TestDejaOListeCons = false;
        Boolean dejaOvrTableLCons = false;
        
        DataTable Table;

        ////*** VOID **** ///////
        private void RempListeBox(TreeList Tl, string Reque, string table)
        {
            if (TestDejaOListeCons == true)
            {
                ds.Tables[table].Clear();
            }
            da = new MySqlDataAdapter(Reque, ClassConnexion.Macon);
            da.Fill(ds, table);

            Tl.DataSource = ds.Tables[table];
            Tl.Refresh();
        }
        public void RempLCons()
        {
            if (dejaOvrTableLCons == true)
            {
                ds.Tables["ListeCons"].Clear();
            }
            da = new MySqlDataAdapter("select Cn.IdCons,Concat(A.PrenomArAdhe,' ',A.NomArAdhe) as NomComp,Cm.NumComp,S.LibelleSect,Cn.ComptageACons,Cn.ComptageNCons,(Cn.ComptageNCons-Cn.ComptageACons) as Calcul1,P.MontantPaie,P.IdPaie as IdPaiePrinci,F.IdFact as IdFactPrinci,(select MontantPaie- (select sum(V_Prix) from lsthelpget_montsansfrais where V_configYesNon=1 and V_IdPaie= IdPaiePrinci and V_IdFAct=IdFactPrinci) from paiement where IdPaie=IdPaiePrinci and IdFact=IdFactPrinci ) as Calcul2 ,case when P.PayePaie=0 then 'لا' else 'نعم' end as PayePai from adherent A,compteur Cm,secteur S,consommation Cn,paiement P,facture F where A.IdAdherent = Cm.IdAdherent and Cm.IdSect = S.IdSect and Cn.IdComp=Cm.IdComp and Cn.IdCons = P.IdCons and P.IdFact = F.IdFact and Cn.IdFact=F.IdFact ", ClassConnexion.Macon);
            da.Fill(ds, "ListeCons");
            dejaOvrTableLCons = true;

        }
        public void RempGridCons()
        {
            dv.Table = ds.Tables["ListeCons"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
        }

        private void S_ListeConso_Load(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                ds = new DataSet();
                dv = new DataView();

                if (Configuration.ExisteEnreSansCondition("Facture")!=0)
                {
                    RempListeBox(TLPerio, "select IdFact, DATE_FORMAT(PeriodeConsoFact,\"%m-%Y\") as DateCons from facture order by IdFact Desc ", "Periode");
                    RempLCons();
                    RempGridCons();
                    dv.RowFilter = "IdFactPrinci = '" + TLPerio.FocusedNode[0].ToString() + "' ";
                    gridControl1.Refresh();
                    splashScreenManager1.CloseWaitForm();
                }
                else
                {
                    splashScreenManager1.CloseWaitForm();
                    XtraMessageBox.Show("لاتوجد أي فترة إستهلاك", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void TLPerio_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                dv.RowFilter = "IdFactPrinci = '" + TLPerio.FocusedNode[0].ToString() + "' ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Imprimer")
                {
                    ImpressionListeConso report = new ImpressionListeConso();

                    gridView1.Columns[0].VisibleIndex = 9;
                    gridView1.Columns[1].VisibleIndex = 8;
                    gridView1.Columns[2].VisibleIndex = 7;
                    gridView1.Columns[3].VisibleIndex = 6;
                    gridView1.Columns[4].VisibleIndex = 5;
                    gridView1.Columns[5].VisibleIndex = 4;
                    gridView1.Columns[6].VisibleIndex = 3;
                    gridView1.Columns[7].VisibleIndex = 2;
                    gridView1.Columns[10].VisibleIndex = 1;
                    gridView1.Columns[11].VisibleIndex = 0;

                    report.GridControl = gridControl1;

                    double MtnPai = 0, MtnNPai = 0,Totale=0 ;
                    for (int i = 0; i < gridView1.DataRowCount; i++)
                    {
                        DataRow row = gridView1.GetDataRow(i);
                        if (row[11].ToString() == "نعم")
                            MtnPai += double.Parse(row[7].ToString());
                        else if (row[11].ToString() == "لا")
                            MtnNPai += double.Parse(row[7].ToString());

                        Totale +=double.Parse(row[7].ToString());
                    }

                    report.Load();
                    report.RempLabel(TLPerio.FocusedNode[1].ToString(), gridColumn7.SummaryText.ToString(), MtnPai.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")), MtnNPai.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")), Totale.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));
                    report.CreateDocument();
                    report.PrintingSystem.Document.AutoFitToPagesWidth = 1;

                    new ReportPrintTool(report).ShowPreview();

                    gridView1.Columns[0].VisibleIndex = 0;
                    gridView1.Columns[1].VisibleIndex = 1;
                    gridView1.Columns[2].VisibleIndex = 2;
                    gridView1.Columns[3].VisibleIndex = 3;
                    gridView1.Columns[4].VisibleIndex = 4;
                    gridView1.Columns[5].VisibleIndex = 5;
                    gridView1.Columns[6].VisibleIndex = 6;
                    gridView1.Columns[7].VisibleIndex = 7;
                    gridView1.Columns[10].VisibleIndex = 8;
                    gridView1.Columns[11].VisibleIndex = 9;
                }
                else if (e.Button.Properties.Tag == "Annuler")
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}