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

namespace DWM
{
    public partial class ListePaiement : DevExpress.XtraEditors.XtraForm
    {
        public ListePaiement()
        {
            InitializeComponent();
        }

        DataSet ds;
        DataSet dss;
        DataSet DSImpression;
        MySqlDataAdapter da;
        DataView dv;
        MySqlDataReader dr;
        Boolean TestDejaOListePerio = false;
        DataTable Table;
        ImpressionRecuPaiement report;
        DataTable Table2;


        private void RempDataTable()
        {
            //da = new MySqlDataAdapter(" select IdPaie,NumComp,DatePaie,DATE_FORMAT(DatePaie,\"%m-%Y\") as dt,MPCons,MPTrai,MPEntr,MPPena,MPTota from listepaiement where DATE_FORMAT(DatePaie,\"%m-%Y\") = '" + DATEC +"' ", ClassConnexion.Macon);
            da = new MySqlDataAdapter(" select IdPaie,NumComp,DatePaie,DATE_FORMAT(DatePaie,\"%m-%Y\") as dt,MPCons,MPTrai,MPEntr,MPPena,MPTota,IdLPaie from listepaiement ", ClassConnexion.Macon);
            da.Fill(ds, "ListePaie");
            dv.Table = ds.Tables["ListePaie"];
            gridControl1.DataSource = dv;

            gridView1.Columns[3].Visible = false;
            gridView1.Columns[9].Visible = false;

            gridView1.Columns[0].Caption = "ر.الأداء";
            gridView1.Columns[1].Caption = "ر.العداد";
            gridView1.Columns[2].Caption = "تاريخ الأداء";
            gridView1.Columns[4].Caption = "م.الاستهلاك";
            gridView1.Columns[5].Caption = "م.الأقساط";
            gridView1.Columns[6].Caption = "م.الخدمات";
            gridView1.Columns[7].Caption = "م.المخالفات";
            gridView1.Columns[8].Caption = "مجموع الأداء";
        }

        private void ListePaiement_Load(object sender, EventArgs e)
        {
            try
            {
                dss = new DataSet();
                ds = new DataSet();
                dv = new DataView();

                Table = new DataTable();
                Table.Columns.Add("DateCons");
                int int_Year = DateTime.Now.Year;
                //RempListeBox(TLPerio2, "select IdFact, DATE_FORMAT(PeriodeConsoFact,\"%m-(DATE_FORMAT(NOW(),\"%Y))\") as DateCons from facture ", "Periode");

                if (DateTime.Now.Month < 6)
                    int_Year = DateTime.Now.Year - 1;


                var months = Enumerable.Range(1, 12 ).Select(p => new DateTime(int_Year, p, 1));
                foreach (var month in months)
                {
                    string str= month.ToString("MM-yyyy");
                    Table.Rows.Add(str);
                }

                TLPerio2.DataSource = Table;
                TLPerio2.Refresh();

                RempDataTable();
                dv.RowFilter = "dt= '" + TLPerio2.FocusedNode[0].ToString() + "' ";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TLPerio2_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            dv.RowFilter = "dt= '"+ TLPerio2.FocusedNode[0].ToString() + "' ";
        }

        string NomComplete;
        string NumAdhe;
        string NumComp;
        string Contra;
        string IdSecteur;
        string LiSecteur;
        string IdUser;
        string Secteur;

        private void RempTable2(DataTable Tb,string Req,string Ch1, string Ch3, string Ch4, string Ch5, string Ch6)
        {
            MySqlCommand CmdRempTable2Imp = new MySqlCommand(Req, ClassConnexion.Macon);
            dr = CmdRempTable2Imp.ExecuteReader();
            while (dr.Read())
            {
                if (Ch6=="1")
                    Tb.Rows.Add(int.Parse(dr[Ch1].ToString()), int.Parse(NumComp), double.Parse(dr[Ch3].ToString()), dr[Ch4].ToString(), double.Parse(dr[Ch5].ToString()), int.Parse(Ch6));
                else
                    Tb.Rows.Add(int.Parse(dr[Ch1].ToString()), int.Parse(NumComp), double.Parse(dr[Ch3].ToString()), dr[Ch4].ToString(), double.Parse(Ch5), int.Parse(Ch6));
            }
            dr.Close();
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag=="Imprimer")
            {
                try
                {
                    if (gridView1.SelectedRowsCount == 1)
                    {
                        string Prefixe = Configuration.Func(13);

                        Table2 = new DataTable();
                        Table2.Columns.Add("id", typeof(Int32));
                        Table2.Columns.Add("NumCom", typeof(Int32));
                        Table2.Columns.Add("MT", typeof(double));
                        Table2.Columns.Add("date", typeof(string));
                        Table2.Columns.Add("penalite", typeof(double));
                        Table2.Columns.Add("Typ", typeof(string));

                        if (ClassConnexion.Macon.State == ConnectionState.Closed)
                            ClassConnexion.Macon.Open();

                        DSImpression = new DataSet();

                        MySqlCommand CmdRech = new MySqlCommand("select C.*,A.IdAdherent as IdAd,A.*,S.IdSect as idSec,S.* from compteur C,adherent A,secteur S where C.IdAdherent=A.IdAdherent and C.IdSect=S.IdSect and C.NumComp=" + int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[1].ToString()) + " ", ClassConnexion.Macon);
                        dr = CmdRech.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            NumAdhe = dr["IdAd"].ToString();
                            NumComp = dr["NumComp"].ToString();
                            NomComplete = dr["NomArAdhe"].ToString() + " " + dr["PrenomArAdhe"].ToString();
                            Contra = Prefixe + dr["NumComp"].ToString();
                            IdSecteur = dr["idSec"].ToString();
                            LiSecteur = dr["LibelleSect"].ToString();
                            IdUser = UserConnecte.IdUser.ToString();
                        }
                        dr.Close();

                        RempTable2(Table2, "select p.*,DATE_FORMAT(f.PeriodeConsoFact,\"%m-%Y\") as PCons,f.IdFact from Paiement p,facture f where p.IdFact=f.IdFact and  IdLP=" + index_IdLP + " ", "IdPaie", "MontantPaie", "PCons", "PenalitePaie", "1");
                        RempTable2(Table2, "select * from moistraite  where IdLP=" + index_IdLP + " ", "IdMTr", "MontantMTr", "MoisMTr", "0", "2");
                        RempTable2(Table2, "select IdPena,penalite.IdTypePena,MontantPena,CONCAT(typepenalite.LibelleTypePena,' ',DATE_FORMAT(DatePena,\"%d-%m-%Y\")) as LibAndDate ,PayerPena,IdLP from penalite,typepenalite where penalite.IdTypePena= typepenalite.IdTypePena and IdLP=" + index_IdLP + " ", "IdPena", "MontantPena", "LibAndDate", "0", "3");
                        RempTable2(Table2, "select IdFraisau,MontantAutFrai,DATE_FORMAT(DateAutFrai,\"%d-%m-%Y\") as DateAutF,IdLP from autrefrais  where IdLP=" + index_IdLP + " ", "IdFraisau", "MontantAutFrai", "DateAutF", "0", "4");

                        Table2.TableName = "TableLP";
                        DSImpression.Tables.Add(Table2);

                        ImpressionRecuPaiement report = new ImpressionRecuPaiement();
                        report.DataSource = DSImpression;
                        report.DataMember = DSImpression.Tables["TableLP"].TableName;

                        report.load();
                        report.infocompteur(NomComplete, NumAdhe, NumComp, Contra, IdSecteur, LiSecteur, IdUser);
                        //report.FilterString = "[NumCom] = " + int.Parse(NumComp);
                        report.CreateDocument();
                        //ReportPrintTool pt = new ReportPrintTool(report);
                        report.ShowRibbonPreviewDialog();

                    }
                    else
                        XtraMessageBox.Show("لم تقم بتحديد التوصيل المراد طباعته", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag == "Annuler")
            {
                this.Close();
            }
        }

        int index_IdLP = 0;
        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            index_IdLP = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)["IdLPaie"].ToString());
        }
    }
}