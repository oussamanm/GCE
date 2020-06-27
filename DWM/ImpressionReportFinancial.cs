using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using MySql.Data.MySqlClient;

namespace DWM
{
    public partial class ImpressionReportFinancial : DevExpress.XtraReports.UI.XtraReport
    {
        public ImpressionReportFinancial()
        {
            InitializeComponent();
        }
        
        string intNbr = "- - - -";
        string DateD,DateF,strDesc = "";
        Boolean UseNbr,TypeDate, bUseCAndC, bPre, bTre, bSec;
        DataSet ds;

        private void ImpressionReportFinancial_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.DataMember = "";
        }

        string formatt = "yyyy-MM-dd";
        DateTime DateDD, DateFF;

        double f_MtnCrd =0, f_MtnCrea=0,f_MtnEntr,f_MtnSort;
        DateTime dt;

        public ImpressionReportFinancial(Boolean Use_Nbr,string int_Nbr,Boolean B_TypeDate,string str_DateD,string str_DateF, string str_Desc,Boolean B_UseCAndC,Boolean b_Pre, Boolean b_Tre, Boolean b_Sec,double MtnCrd, double MtnCrea,double MtnEntr,double MtnSort)
        {
            UseNbr = Use_Nbr;
            intNbr = int_Nbr.ToString();
            TypeDate = B_TypeDate;
            if (B_TypeDate == false)
                DateD = str_DateD;
            else
            {
                DateD = str_DateD;
                DateF = str_DateF;
            }
            strDesc = str_Desc;
            bUseCAndC=B_UseCAndC;
            f_MtnCrd = MtnCrd;
            f_MtnCrea = MtnCrea;
            f_MtnEntr = MtnEntr;
            f_MtnSort = MtnSort;
            bPre = b_Pre;
            bTre = b_Tre;
            bSec = b_Sec;

            InitializeComponent();
        }
        public void load()
        {
            string Information = Configuration.Func(12);
            string wanted_path = wanted_path = AppDomain.CurrentDomain.BaseDirectory;
            xrPictureBox1.ImageUrl = wanted_path + "\\Resources\\" + Configuration.Func(8);
            nomassoc.Text = Configuration.Func(1);
            adresse.Text = Configuration.Func(10);
            Tel.Text = Configuration.Func(11);
            email.Text = Configuration.Func(9);
            xrBarCode23.Text = Configuration.Func(9) + "\r\n" + DateTime.Now + "\r\n\r\n\r\n" + Information;
        }
        public void RempContenu()
        {
            /// Num Met
            if (UseNbr == true)
            {
                XLUseNbr.Visible = XLNbr.Visible = true;
                XLNbr.Text = intNbr;
            }
            else
                XLUseNbr.Visible = XLNbr.Visible = false;

            /// Date
            if (TypeDate == false)
                XLAnne.Text = DateD;
            else
            {
                DateDD = DateTime.Parse(DateD);
                DateFF = DateTime.Parse(DateF);

                XTypeAnne.Text = "للفترة بين : ";
                XAnd.Visible = XAnneF.Visible = true;
                XLAnne.Text = DateDD.ToString("dd/MM/yyyy");
                XAnneF.Text = DateFF.ToString("dd/MM/yyyy");
            }
            /// Desc
            XLDesc.Text = strDesc;

            /// MtnRest
            double MtnRes = f_MtnEntr - f_MtnSort;
            xrLabelSumRest.Text = Configuration.ConvertToMonyC(float.Parse(MtnRes.ToString()));

            /// C And C
            if (bUseCAndC == true)
            {


                double MtnTotal = (MtnRes + f_MtnCrea) - f_MtnCrd;
                xrLabelMtnCred.Text = Configuration.ConvertToMonyC(float.Parse(f_MtnCrd.ToString()));
                xrLabelMtnCrea.Text = Configuration.ConvertToMonyC(float.Parse(f_MtnCrea.ToString()));
                xrLabelMtnTota.Text = Configuration.ConvertToMonyC(float.Parse(MtnTotal.ToString()));
                
            }
            else
            {
                xrPanelCAndC.Visible = false;
            }

            /// Signa
            if (bPre == true)
            {
                XLPres.Visible = true;
                XPres.Visible = true;
                XPres.Text = Configuration.CalculeRequte("select NomCompArb from utilisateurs where IdType=1 and bloque=0");
            }
            if (bTre == true)
            {
                XLTres.Visible = true;
                XTre.Visible = true;
                XTre.Text = Configuration.CalculeRequte("select NomCompArb from utilisateurs where IdType=2 and bloque=0");
            }
            if (bSec == true)
            {
                XSec.Visible = true;
                XLSec.Visible = true;
                XSec.Text = Configuration.CalculeRequte("select NomCompArb from utilisateurs where IdType=3 and bloque=0");
            }
        }

    }
}
