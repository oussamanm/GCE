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

namespace DWM
{
    public partial class InfoCaisse : DevExpress.XtraEditors.XtraForm
    {
        int IdC;
        Boolean testdejaovrireTablePrin,testdejaovrire1, testdejaovrire2, testdejaovrire3, testdejaovrire4 = false;
        float F_Entr, F_Sort, F_Reste, F_Reste2, F_Plus, F_Moins = 0;
        DataSet ds;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        string AnnesSelect;
        DataTable TabTransDes, TabTransSrc;

        public InfoCaisse()
        {
            InitializeComponent();
        }
        public InfoCaisse(int Id_Cais)
        {
            IdC = Id_Cais;
            InitializeComponent();
        }

        //// Void
        private void RempDataTransf(string Req,string Tab, int Typ)
        {
            if ( Typ==1 && testdejaovrire3 == true)
                ds.Tables["TabDes"].Clear();
            if (Typ == 2 &&testdejaovrire4 == true)
                ds.Tables["TabSrc"].Clear();

            da = new MySqlDataAdapter(Req,ClassConnexion.Macon);
            da.Fill(ds, Tab);

            if (Typ == 1)
                testdejaovrire3 = true;
            if (Typ == 2)
                testdejaovrire4 = true;          
        }
        private void RempTablePrinci(string strYear)
        {
            if (testdejaovrireTablePrin == true)
                ds.Tables["TablePrinc"].Clear();

            using (MySqlCommand Cmd = new MySqlCommand("dwm.Proc_RempTableInfoCais", ClassConnexion.Macon))
            {
                Cmd.CommandType = CommandType.StoredProcedure;

                MySqlParameter Para1 = new MySqlParameter("@StrYear", MySqlDbType.VarChar);
                Para1.Value = strYear;
                Para1.Direction = ParameterDirection.Input;

                MySqlParameter Para2 = new MySqlParameter("@StrIdCais", MySqlDbType.Int16);
                Para2.Value = IdC;
                Para2.Direction = ParameterDirection.Input;

                Cmd.Parameters.Add(Para1);
                Cmd.Parameters.Add(Para2);

                using (MySqlDataAdapter da = new MySqlDataAdapter(Cmd))
                {
                    da.Fill(ds, "TablePrinc");
                    testdejaovrireTablePrin = true;
                    TLTablePrin.DataSource = ds.Tables["TablePrinc"];
                    TLTablePrin.Refresh();
                }
            }
        }


        private void RempLV(TreeList Tl, string Reque, string table,int typee)
        {
            if (typee == 1 && testdejaovrire1 == true)
                ds.Tables["TabEntr"].Clear();
            if (typee == 2 && testdejaovrire2 == true)
                ds.Tables["TabSort"].Clear();

            da = new MySqlDataAdapter(Reque, ClassConnexion.Macon);
            da.Fill(ds, table);

            Tl.DataSource = ds.Tables[table];
            Tl.Refresh();

            if (typee == 1)
                testdejaovrire1 = true;
            else if(typee == 2)
                testdejaovrire2 = true;
        }
        private  void RempAllTL()
        {
            RempLV(TLEntresGen, "select CE.LibCatEntr,sum(EC.MontantEntr) as MtnEntr from entrescais EC,catentres CE where EC.CatEntres_IdCatEntr = CE.IdCatEntr and Date_Format(DateEntr,\"%Y\")='" + AnnesSelect + "' and EC.SuppEntr=0 and EC.Caisse_IdCais=" + IdC + " group by CatEntres_IdCatEntr", "TabEntr",1);
            RempLV(TLSortiesGen, "select CS.LibCatSort,sum(SC.MontantSort) as MtnSort from sortiescais SC,catsorties CS where SC.CatSorties_IdCatSort = CS.IdCatSort and SC.SuppSort=0 and Date_Format(SC.DateSort,\"%Y\")='" + AnnesSelect + "' and SC.Caisse_IdCais=" + IdC + " group by SC.CatSorties_IdCatSort", "TabSort",2);

            RempDataTransf("select Caisse_IdCaisSrc,Caisse_IdCaisDes,(select LibCais from Caisse where IdCais=Caisse_IdCaisSrc ) as LibSrc ,(select LibCais from Caisse where IdCais=Caisse_IdCaisDes ) as LibDes,1,sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisDes=" + IdC + " and Date_Format(DateTransf,\"%Y\")= '" + AnnesSelect + "' ", "TabDes", 1);
            RempDataTransf("select Caisse_IdCaisSrc,Caisse_IdCaisDes,(select LibCais from Caisse where IdCais=Caisse_IdCaisSrc ) as LibSrc ,(select LibCais from Caisse where IdCais=Caisse_IdCaisDes ) as LibDes,2,sum(MontantTransf) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisSrc=" + IdC + " and Date_Format(DateTransf,\"%Y\")='" + AnnesSelect + "' ", "TabSrc", 2);

            DataTable MergeTab = new DataTable();
            MergeTab = ds.Tables["TabDes"];
            MergeTab.Merge(ds.Tables["TabSrc"]);

            TLTransf.DataSource = MergeTab;

            /// Remp TablePrinc
            RempTablePrinci(AnnesSelect);

        }
        private void RempFooter()
        {
            F_Entr = F_Sort = F_Reste = F_Reste2 = F_Plus = F_Moins = 0;

            F_Entr = float.Parse(TLEntresGen.GetSummaryValue(TLEntresGen.Columns[1]).ToString());
            F_Sort = float.Parse(TLSortiesGen.GetSummaryValue(TLSortiesGen.Columns[1]).ToString());
            F_Reste = F_Entr - F_Sort;
            if (ds.Tables["TabDes"].Rows[0]["MtnTransf"].ToString() != "")
                F_Plus = float.Parse(ds.Tables["TabDes"].Rows[0]["MtnTransf"].ToString());
            if (ds.Tables["TabSrc"].Rows[0]["MtnTransf"].ToString() != "")
                F_Moins = float.Parse(ds.Tables["TabSrc"].Rows[0]["MtnTransf"].ToString());

            LbTransRestt.Text = Configuration.ConvertToMony((F_Plus - F_Moins));
            F_Reste2 = (F_Reste + F_Plus) - (F_Moins);

            LbCaisEntr.Text = Configuration.ConvertToMonyC(F_Entr);
            LbCaisSort.Text = Configuration.ConvertToMonyC(F_Sort);
            LbCaisReste.Text = Configuration.ConvertToMonyC(F_Reste);
            LbCaisCred.Text = Configuration.ConvertToMonyC(F_Reste2);
            LbAnneesCaisss.Text = AnnesSelect;
        }
        
        //// Code
        private void InfoCaisse_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();

                CBAnnesCaiss.SelectedIndexChanged -= new EventHandler(CBAnnesCaiss_SelectedIndexChanged);
                Configuration.RempComboSimple(CBAnnesCaiss, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
                CBAnnesCaiss.SelectedIndexChanged += new EventHandler(CBAnnesCaiss_SelectedIndexChanged);

                CBAnnesCaiss.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
                AnnesSelect = CBAnnesCaiss.SelectedValue.ToString();

                RempAllTL();
                RempFooter();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Back")
            {
                this.Close();
            }
        }
        private void CBAnnesCaiss_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AnnesSelect = CBAnnesCaiss.SelectedValue.ToString();
                RempAllTL();
                RempFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}