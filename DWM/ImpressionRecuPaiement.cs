using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.IO;

namespace DWM
{
    public partial class ImpressionRecuPaiement : DevExpress.XtraReports.UI.XtraReport
    {
        public ImpressionRecuPaiement()
        {
            InitializeComponent();
        }

       

        public void infocompteur(string NomComplete, string NumAdhe, string NumComp, string Contra, string IdSecteur, string LiSecteur,string IdUser)
        {
            lbCompany1.Text = NumComp;
            lbOccupation1.Text = LiSecteur+" /  S"+ IdSecteur;
            xrLabel5.Text = Contra;
            xrLabel6.Text = NomComplete + "  ("+ NumAdhe+")";
            xrLabel7.Text = IdUser;
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
