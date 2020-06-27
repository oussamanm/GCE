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
    public partial class ConfigImpressReportFina : DevExpress.XtraEditors.XtraForm
    {
        DataSet ds;
        double fltCrd = 0, fltCrea = 0, S_MtnEntr = 0, S_MtnSort = 0;
        string formatt = "yyyy-MM-dd";
        Boolean UseOldEntr=false;
        public ConfigImpressReportFina()
        {
            InitializeComponent();
        }

        public ConfigImpressReportFina(Boolean B_UseOldEntr)
        {
            UseOldEntr = B_UseOldEntr;
            InitializeComponent();
        }

        private void ConfigImpressReportFina_Load(object sender, EventArgs e)
        {
            try
            {
                Configuration.RempComboSimple(CbAnn, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
                CbAnn.SelectedValue = Configuration.ReturnValueMax("select Max(DATE_FORMAT(PeriodeConsoFact,\"%Y\")) from facture");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Imprimer")
                {
                    splashScreenManager1.ShowWaitForm();
                    string ReqEntr,ReqSort,ReqCais,ReqCrd,ReqCrn,DateD,DateF;
                    ReqEntr=ReqSort=ReqCais=ReqCrd=ReqCrn=DateD=DateF = "";
                    Boolean TypeDate,UseCandC = false; //TypeDate ==> type de date equal false=Annes or true=Perio
                    TypeDate = Boolean.Parse(RGDate.EditValue.ToString());
                    UseCandC = Boolean.Parse(RGUseCAndC.EditValue.ToString());
                    DateTime DateFrom, DateTo;

                    ///****************************************************************///
                    
                    ds = new DataSet();

                    if (TypeDate == false && CbAnn.SelectedValue !="")
                    {

                        if (UseOldEntr == false)
                            ReqEntr = "select IFNULL(sum(SecTable.MtnEntr),0) as MtnEntr,SecTable.NewCatEntr as LibEntr from ( select IFNULL(sum(MontantEntr), 0) as MtnEntr, case when IdCatEntr<= 4 then LibCatEntr else 'مداخيل اخرى' end as NewCatEntr from entrescais,catentres where CatEntres_IdCatEntr = IdCatEntr and SuppEntr = 0 and Date_Format(DateEntr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' group by CatEntres_IdCatEntr asc ) as SecTable group by SecTable.NewCatEntr asc order by LibEntr desc";
                        else
                            ReqEntr = string.Format("select IFNULL(((select sum(MontantEntr) from entrescais where SuppEntr = 0 and Date_Format(DateEntr,\"%Y\")='{0}')- (select sum(MontantSort) from sortiescais where SuppSort=0 and Date_Format(DateSort,\"%Y\")='{0}')),0) as MtnEntr,'رصيد السنة الماضية' as LibEntr UNION ALL select IFNULL(sum(SecTable.MtnEntr),0) as MtnEntr,SecTable.NewCatEntr as LibEntr from ( select IFNULL(sum(MontantEntr), 0) as MtnEntr, case when IdCatEntr<= 4 then LibCatEntr else 'مداخيل اخرى' end as NewCatEntr from entrescais,catentres where CatEntres_IdCatEntr = IdCatEntr and SuppEntr = 0 and Date_Format(DateEntr,\"%Y\")='{1}' group by CatEntres_IdCatEntr asc ) as SecTable group by SecTable.NewCatEntr asc order by LibEntr desc", (int.Parse(CbAnn.SelectedValue.ToString()) - 1).ToString(),CbAnn.SelectedValue.ToString());

                        ReqSort = "select IFNULL(sum(sc.MontantSort), 0) as MtnSort,cs.LibCatSort as LibSort  from sortiescais sc,catsorties cs where sc.CatSorties_IdCatSort = cs.IdCatSort and SuppSort = 0 and DATE_FORMAT(DateSort,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "'  group by sc.CatSorties_IdCatSort";
                        ReqCais = "select IdCais,LibCais,(select IFNULL(sum(MontantEntr),0) from entrescais where SuppEntr =0 and Caisse_IdCais=C.IdCais and Date_Format(DateEntr,\"%Y\") ='" + CbAnn.SelectedValue.ToString() + "')- (select IFNULL(sum(MontantSort),0) from sortiescais where SuppSort =0 and Caisse_IdCais=C.IdCais and Date_Format(DateSort,\"%Y\") ='" + CbAnn.SelectedValue.ToString() + "' )+(select IFNULL(sum(MontantTransf),0) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisDes=C.IdCais and Date_Format(DateTransf,\"%Y\")= '" + CbAnn.SelectedValue.ToString() + "')-(select IFNULL(sum(MontantTransf),0) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisSrc=C.IdCais and Date_Format(DateTransf,\"%Y\")= '" + CbAnn.SelectedValue.ToString()+"') as MtnCais from caisse C "; 
                    }
                    else if (TypeDate == true)
                    {
                        DateFrom = DateTime.Parse(DEFrom.EditValue.ToString());
                        DateTo = DateTime.Parse(DETo.EditValue.ToString());

                        ReqEntr = "select IFNULL(sum(SecTable.MtnEntr),0) as MtnEntr,SecTable.NewCatEntr as LibEntr from ( select IFNULL(sum(MontantEntr), 0) as MtnEntr, case when IdCatEntr<= 4 then LibCatEntr else 'مداخيل اخرى' end as NewCatEntr from entrescais,catentres where CatEntres_IdCatEntr = IdCatEntr and SuppEntr = 0 and DateEntr BETWEEN CAST('"+DateFrom.ToString(formatt)+"' as DATE) and CAST('"+DateTo.ToString(formatt)+"' as Date) group by CatEntres_IdCatEntr asc ) as SecTable group by SecTable.NewCatEntr asc order by SecTable.NewCatEntr desc";

                        //ReqEntr = "select IFNULL(sum(ec.MontantEntr), 0) as MtnEntr,ce.LibCatEntr as LibEntr from entrescais ec,catentres ce where ec.CatEntres_IdCatEntr = ce.IdCatEntr and ec.SuppEntr = 0 and DateEntr BETWEEN CAST('" + DateFrom.ToString(formatt) + "' as DATE) and CAST('"+DateTo.ToString(formatt)+ "' as DATE) group by ec.CatEntres_IdCatEntr";
                        ReqSort = "select IFNULL(sum(sc.MontantSort), 0) as MtnSort,cs.LibCatSort as LibSort  from sortiescais sc,catsorties cs where sc.CatSorties_IdCatSort = cs.IdCatSort and SuppSort = 0 and DateSort BETWEEN CAST('" + DateFrom.ToString(formatt) + "' as DATE) and CAST('" + DateTo.ToString(formatt) + "' as DATE)  group by sc.CatSorties_IdCatSort";
                        ReqCais = string.Format("select IdCais,LibCais,(select IFNULL(sum(MontantEntr),0) from entrescais where SuppEntr =0 and Caisse_IdCais=C.IdCais and DateEntr BETWEEN CAST('{0}' AS DATE) and CAST('{1}' AS DATE) )- (select IFNULL(sum(MontantSort),0) from sortiescais where SuppSort =0 and Caisse_IdCais=C.IdCais and DateSort BETWEEN CAST('{0}' AS DATE) AND CAST('{1}' AS DATE) )+(select IFNULL(sum(MontantTransf),0) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisDes=C.IdCais and DateTransf BETWEEN CAST('{0}' AS DATE) and CAST('{1}' AS DATE) ) -(select IFNULL(sum(MontantTransf),0) as MtnTransf from transfcais where SuppTransf = 0 and Caisse_IdCaisSrc=C.IdCais and DateTransf BETWEEN CAST('{0}' as DATE) and CAST('{1}' as DATE)) as MtnCais from caisse C ", DateFrom.ToString(formatt), DateTo.ToString(formatt));
                    }

                    /// Table Vide
                    DataTable ddd = new DataTable();
                    ddd.TableName = "ddd";
                    ds.Tables.Add(ddd);

                    /// Data Sorties
                    using (MySqlDataAdapter da = new MySqlDataAdapter(ReqSort, ClassConnexion.Macon)){
                        da.Fill(ds, "DateSortiess");}

                    /// Date Entres 
                    using (MySqlDataAdapter da = new MySqlDataAdapter(ReqEntr, ClassConnexion.Macon)){
                        da.Fill(ds, "DateEntress");
                        //da.Fill(ds, "DateEntress");
                    }

                    /// Data Caisse
                    using (MySqlDataAdapter da = new MySqlDataAdapter(ReqCais, ClassConnexion.Macon)){
                        da.Fill(ds, "DateCaisse");}

                    /// Data C&C  And MtnENtr / MtnSort  
                    if (ds.Tables["DateEntress"].Compute("sum(MtnEntr)", "").ToString() == "0" || ds.Tables["DateEntress"].Compute("sum(MtnEntr)", "").ToString() =="")
                        S_MtnEntr = 0;
                    else
                        S_MtnEntr = double.Parse(ds.Tables["DateEntress"].Compute("sum(MtnEntr)", "").ToString());

                    if (ds.Tables["DateSortiess"].Compute("sum(MtnSort)", "").ToString() == "0" || ds.Tables["DateSortiess"].Compute("sum(MtnSort)", "").ToString() == "")
                        S_MtnSort = 0;
                    else
                        S_MtnSort = double.Parse(ds.Tables["DateSortiess"].Compute("sum(MtnSort)", "").ToString());

                    if (UseCandC == true)
                    {
                        if (TypeDate == false)
                        {
                            ReqCrd = "select IFNULL(sum(MontantCrd),0) as MtnCrd from credit where SuppCrd=0 and PaieCrd=0 and Date_Format(DateCrd,\"%Y\") = '" + CbAnn.SelectedValue.ToString() + "' ";
                            ReqCrn = "select (select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and date_format(F.PeriodeConsoFact,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and date_format(DateAutFrai,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' )+ (select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and date_format(DatePena,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "') ";
                            fltCrd = double.Parse(Configuration.CalculeRequte(ReqCrd));
                            fltCrea = double.Parse(Configuration.CalculeRequte(ReqCrn));
                        }
                        else
                        {
                            DateFrom = DateTime.Parse(DEFrom.EditValue.ToString());
                            DateTo = DateTime.Parse(DETo.EditValue.ToString());

                            ReqCrd = "select IFNULL(sum(MontantCrd),0) as MtnCrd from credit where SuppCrd=0 and PaieCrd=0 and DateCrd BETWEEN CAST('" + DateFrom.ToString(formatt) + "' as Date) AND CAST('" + DateTo.ToString(formatt) + "' as Date) ";
                            ReqCrn = "select (select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and F.PeriodeConsoFact BETWEEN CAST('" + DateFrom.ToString(formatt) + "' as DATE) AND CAST('" + DateTo.ToString(formatt) + "' as DATE))+ (select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%m-%Y\")>='" + DateFrom.ToString(formatt) + "' and date_format(MoisMTr,\"%m-%Y\")<='" + DateTo.ToString(formatt) + "')+ (select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and DateAutFrai BETWEEN CAST('" + DateFrom.ToString(formatt) + "' as DATE) and CAST('" + DateTo.ToString(formatt) + "' as DATE) )+ (select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and DatePena BETWEEN CAST('" + DateFrom.ToString(formatt) + "' AS DATE) AND CAST('" + DateTo.ToString(formatt) + "' AS DATE)) ";
                            fltCrd = double.Parse(Configuration.CalculeRequte(ReqCrd));
                            fltCrea = double.Parse(Configuration.CalculeRequte(ReqCrn));
                        }
                    }
                    else
                    {
                        //ReqCrd = "select IFNULL(sum(MontantCrd),0) as MtnCrd from credit where SuppCrd=0 and PaieCrd=0 and Date_Format(DateCrd,\"%Y\") = '" + CbAnn.SelectedValue.ToString() + "' ";
                        //ReqCrn = "select (select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and date_format(F.PeriodeConsoFact,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "')+ (select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and date_format(DateAutFrai,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "' )+ (select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and date_format(DatePena,\"%Y\")='" + CbAnn.SelectedValue.ToString() + "') ";
                        //fltCrd = double.Parse(Configuration.CalculeRequte(ReqCrd));
                        //fltCrea = double.Parse(Configuration.CalculeRequte(ReqCrd));
                    }

                    if (TypeDate == false)
                        DateD = CbAnn.SelectedValue.ToString();
                    else
                    {
                        DateD = DEFrom.EditValue.ToString();
                        DateF = DETo.EditValue.ToString();
                    }

                    ImpressionReportFinancial report = new ImpressionReportFinancial(Boolean.Parse(ChENbr.Checked.ToString()),TbNbr.EditValue.ToString(),TypeDate, DateD, DateF,TbDesc.Text,UseCandC, Boolean.Parse(ChEPre.Checked.ToString()), Boolean.Parse(ChETre.Checked.ToString()), Boolean.Parse(ChESec.Checked.ToString()), fltCrd, fltCrea, S_MtnEntr, S_MtnSort);
                    report.DataSource = ds;

                    XRSubreport subreport1 = report.FindControl("xrSubreport1", true) as XRSubreport;
                    XRSubreport subreport2 = report.FindControl("xrSubreport2", true) as XRSubreport;

                    XtraReport2 r2 = new XtraReport2();
                    r2.DataSource = ds.Tables["DateEntress"];
                    subreport1.ReportSource = r2;

                    XtraReport1 r1 = new XtraReport1();
                    r1.DataSource = ds.Tables["DateSortiess"];
                    subreport2.ReportSource = r1;

                    CalculatedField CalculField = new CalculatedField();
                    report.CalculatedFields.Add(CalculField);
                    CalculField.DataSource = report.DataSource;
                    CalculField.DataMember = "DateEntress";
                    CalculField.FieldType = FieldType.Double;
                    CalculField.DisplayName = "Field1Sum";
                    CalculField.Name = "Field1Sum";
                    CalculField.Expression = "SUM([MtnEntr])";

                    CalculatedField CalculField2 = new CalculatedField();
                    report.CalculatedFields.Add(CalculField2);
                    CalculField2.DataSource = report.DataSource;
                    CalculField2.DataMember = "DateSortiess";
                    CalculField2.FieldType = FieldType.Double;
                    CalculField2.DisplayName = "Field2Sum";
                    CalculField2.Name = "Field2Sum";
                    CalculField2.Expression = "SUM([MtnSort])";

                    //CalculatedField CalculField3 = new CalculatedField();
                    //report.CalculatedFields.Add(CalculField3);
                    //CalculField3.FieldType = FieldType.Double;
                    //CalculField3.DataSource = report.DataSource;
                    //CalculField3.DisplayName = "CalculatedField3";
                    //CalculField3.Name = "CalculatedField3";
                    //CalculField3.Expression = "[MtnEntr]-[MtnSort]";


                    report.CalculatedFields.AddRange(new CalculatedField[] { CalculField, CalculField2/*, CalculField3*/ });
                    report.FindControl("xrLabelSumEntr", true).DataBindings.Add("Text", ds, "DateEntress.Field1Sum", "{0:c2}");
                    report.FindControl("xrLabelSumSort", true).DataBindings.Add("Text", ds, "DateSortiess.Field2Sum", "{0:c2}");
                    //report.FindControl("xrLabel9", true).DataBindings.Add("Text", null, "CalculatedField3", "{0:c2}");
                    //report.FindControl("xrLabel9", true).Text = "100";
                    report.load();
                    report.RempContenu();

                    report.CreateDocument();
                    splashScreenManager1.CloseWaitForm();
                    new ReportPrintTool(report).ShowPreview();

                    //*********************************************************************///
                }
                else if (e.Button.Properties.Tag == "Annuler")
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RGDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RGDate.SelectedIndex == 0)
                {
                    CbAnn.Enabled = true;
                    DEFrom.Enabled = DETo.Enabled = false;
                    //LbUseCAndC.Enabled = true;
                    //RGUseCAndC.Enabled = true;
                }
                else
                {
                    CbAnn.Enabled = false;
                    DEFrom.Enabled = DETo.Enabled = true;

                    //LbUseCAndC.Enabled = false;
                    //RGUseCAndC.SelectedIndex = 1;
                    //RGUseCAndC.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ChENbr_CheckedChanged(object sender, EventArgs e)
        {
            if (ChENbr.Checked == false)
                TbNbr.Enabled = false;
            else
                TbNbr.Enabled = true;

        }
    }
}