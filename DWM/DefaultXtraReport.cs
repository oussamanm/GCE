using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;

namespace DWM
{
    public partial class DefaultXtraReport : DevExpress.XtraReports.UI.XtraReport
    {
        public DefaultXtraReport()
        {
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
        public  void RempReport(string LHeader,Boolean UsePanelH,string xrLH11,string xLH11,Boolean UsePanelF,int CountLabF,int CountVisibleF,ArrayList xrAL,ArrayList xAL)
        {
            xrLabelHeader.Text = LHeader.ToString() ;

            if (UsePanelH == false)
                xrPanelH.Visible = false;
            else
            {
                xrPanelH.Visible = true;
                xrLH1.Text = xrLH11;
                xLH1.Text = xLH11;
            }

            if (UsePanelF == false)
            {
                xrPanelF.Visible = false;
                ReportFooter.HeightF = 5;
            }
            else
            {
                xrPanelF.Visible = true;

                if (CountLabF == 1) {
                    ReportFooter.HeightF = 120; xrPanelF.HeightF = 79; }
                else if (CountLabF == 2)
                    ReportFooter.HeightF = 156;

                if (CountVisibleF == 1)
                { 
                    xrL1.Visible = xL1.Visible = true;
                    xrL1.Text = xrAL[0].ToString();
                    xL1.Text = xAL[0].ToString();
                }
                else if (CountVisibleF == 2)
                {
                    xrL1.Visible = xL1.Visible = xrL2.Visible = xL2.Visible = true;
                    xrL1.Text = xrAL[0].ToString(); xrL2.Text = xrAL[1].ToString();
                    xL1.Text = xAL[0].ToString(); xL2.Text = xAL[1].ToString();
                }
                else if (CountVisibleF == 3)
                {
                    xrL1.Visible = xL1.Visible = xrL2.Visible = xL2.Visible = xrL3.Visible = xL3.Visible = true;
                    xrL1.Text = xrAL[0].ToString(); xrL2.Text = xrAL[1].ToString(); xrL3.Text = xrAL[2].ToString();
                    xL1.Text = xAL[0].ToString(); xL2.Text = xAL[1].ToString(); xL3.Text = xAL[2].ToString();

                }
                else if (CountVisibleF == 4)
                {
                    xrL1.Visible = xL1.Visible = xrL2.Visible = xL2.Visible = xrL3.Visible = xL3.Visible = xrL4.Visible = xL4.Visible = true;
                    xrL1.Text = xrAL[0].ToString(); xrL2.Text = xrAL[1].ToString(); xrL3.Text = xrAL[2].ToString(); xrL4.Text = xrAL[3].ToString();
                    xL1.Text = xAL[0].ToString(); xL2.Text = xAL[1].ToString(); xL3.Text = xAL[2].ToString(); xL4.Text = xAL[3].ToString();
                }
            }

           
        }


        private GridControl control;
        public GridControl GridControl
        {
            get
            {
                return control;
            }
            set
            {
                control = value;
                printableComponentContainer1.PrintableComponent = control;
            }
        }

    }
}
