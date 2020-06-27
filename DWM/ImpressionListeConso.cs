using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;

namespace DWM
{
    public partial class ImpressionListeConso : DevExpress.XtraReports.UI.XtraReport
    {
        public ImpressionListeConso()
        {
            InitializeComponent();
        }
        string str_Perio = "";

        public void Load()
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

        public void RempLabel(string Perio,string SumCons, string SumConsPaie, string SumCounsNPaie, string Tot)
        {
            this.xLPerio.Text = Perio;
            this.xLabelSumCons.Text = SumCons;
            this.xLabelSumConsPai.Text = SumConsPaie;
            this.xLabelSumConsNPai.Text = SumCounsNPaie;
            this.Totale.Text = Tot;

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
