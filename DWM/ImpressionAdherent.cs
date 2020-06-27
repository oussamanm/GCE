using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.IO;


namespace DWM
{
    public partial class ImpressionAdherent : DevExpress.XtraReports.UI.XtraReport
    {
        public ImpressionAdherent()
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


    }
}
