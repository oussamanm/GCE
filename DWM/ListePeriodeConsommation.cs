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
using MySql.Data.MySqlClient;
using MySql.Data;
using DevExpress.XtraTreeList;

namespace DWM
{
    public partial class ListePeriodeConsommation : DevExpress.XtraEditors.XtraForm
    {
        public ListePeriodeConsommation()
        {
            InitializeComponent();
        }

        DataView dv, dvTran, dvFrias;

        DataSet ds;
        MySqlDataAdapter da;
        Boolean dejaOvrTableLCons=false;
        Boolean TestDejaOListeFrais = false;
        Boolean TestDejaOListeTranche = false;
        int indexRowTrn, indexRowFra = 0;
        Boolean bln=false;

        ///// Void 
        public void RempLPerio()
        {
            if (dejaOvrTableLCons == true)
                ds.Tables["PerioCons"].Clear();

            da = new MySqlDataAdapter("select IdFact,Date_Format(PeriodeConsoFact,\"%m-%Y\"),PeriodePaieDFact,PeriodePaieFFact,IdUser,IdFraisC2,IdCopieTran from Facture", ClassConnexion.Macon);
            da.Fill(ds, "PerioCons");
            dejaOvrTableLCons = true;
        }
        public void RempGridPerio()
        {
            dv.Table = ds.Tables["PerioCons"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
            bln = true;
            gridView1.Columns[0].Caption = "ر.الفترة";
            gridView1.Columns[1].Caption = "الفترة";
            gridView1.Columns[2].Caption = "ت.ب التحصيل";
            gridView1.Columns[2].ToolTip = "تاريخ بداية التحصيل";
            gridView1.Columns[3].Caption = "ت.ن التحصيل";
            gridView1.Columns[3].ToolTip = "تاريخ نهاية التحصيل";
            gridView1.Columns[4].Caption = "ر.المستخدم";
            gridView1.Columns[5].Visible = false;
            gridView1.Columns[6].Visible = false;

        }
        private void RempListeBox(TreeList Tl, string Reque, string table,DataView dav)
        {
            if (TestDejaOListeFrais == true)
                ds.Tables[table].Clear();
            if (TestDejaOListeTranche == true)
                ds.Tables[table].Clear();

            da = new MySqlDataAdapter(Reque, ClassConnexion.Macon);
            da.Fill(ds, table);
            dav.Table = ds.Tables[table];
            Tl.DataSource = dav;
            Tl.Refresh();

        }
        DataTable dt1;
        DataTable dt2;

        public void FilterTL(int Index1,int Index2)
        {
            dvTran.RowFilter = "IdCopie= "+ Index1 + " ";
            dvFrias.RowFilter = "IdCopie= " + Index2 + " ";
        }
        private void ListePeriodeConsommation_Load(object sender, EventArgs e)
        {
            try
            {
                dv = new DataView();
                dvTran = new DataView();
                dvFrias = new DataView();
                ds = new DataSet();

                RempLPerio();
                RempGridPerio();
                RempListeBox(TlFrais, "SELECT LibelleFraiC, PrixUFraiC,IdCopie FROM fraiscopie WHERE (configYesNon = 1) ", "Frais", dvFrias);
                RempListeBox(TlTran, "SELECT LibelleTranC,PrixUTranC,IdCopie FROM tranchescopie ", "Tranche", dvTran);

                indexRowTrn = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)["IdCopieTran"].ToString());
                indexRowFra = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)["IdFraisC2"].ToString());

                FilterTL(indexRowTrn, indexRowFra);
                dt1 = dvTran.ToTable();
                dt2 = dvFrias.ToTable();

                TlTran.DataSource = dt1;
                TlFrais.DataSource = dt2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag=="Annuler")
            {
                this.Close();
            }
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (bln==true)
            {
                indexRowTrn = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)["IdCopieTran"].ToString());
                indexRowFra = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)["IdFraisC2"].ToString());

                FilterTL(indexRowTrn, indexRowFra);
                dt1 = dvTran.ToTable();
                dt2 = dvFrias.ToTable();

                TlTran.DataSource = dt1;
                TlFrais.DataSource = dt2;

            }
        }
    }
}