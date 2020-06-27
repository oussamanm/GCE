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

namespace DWM
{
    public partial class ListeTrans : DevExpress.XtraEditors.XtraForm
    {
        public ListeTrans()
        {
            InitializeComponent();
        }

        DataSet ds;
        MySqlCommand CmdListTrans;
        MySqlDataAdapter da;

        private void ListeTrans_Load(object sender, EventArgs e)
        {

            ds = new DataSet();

            da = new MySqlDataAdapter(" select transformations.IdTrans,transformations.IdAdherentancien as tt,transformations.IdAdherentnouveau as yy,(select concat(adherent.NomArAdhe,' ',adherent.PrenomArAdhe) from adherent,transformations where adherent.IdAdherent=transformations.IdAdherentancien and transformations.IdAdherentnouveau=yy and adherent.IdAdherent=tt)as AncienAdh, transformations.IdAdherentnouveau as ss,(select concat(adherent.NomArAdhe,' ',adherent.PrenomArAdhe) from adherent,transformations where adherent.IdAdherent=transformations.IdAdherentnouveau and transformations.IdAdherentancien=tt and adherent.IdAdherent=yy)as NouvAdh,transformations.DateTrans,transformations.DescriptionTrans from transformations,adherent where transformations.IdAdherentancien = adherent.IdAdherent ", ClassConnexion.Macon);
            da.Fill(ds, "ListeTrans");
            gridControl1.DataSource = ds.Tables["ListeTrans"];

            gridView1.Columns[2].Visible = false;
            gridView1.Columns[0].Width = 40;

            gridView1.Columns[0].Caption = "ر.التحويل";
            gridView1.Columns[1].Caption = "ر.المنخرط السابف للعداد";
            gridView1.Columns[3].Caption = "اسم المنخرط السابف";
            gridView1.Columns[4].Caption = "ر.المنخرط الحالي للعداد";
            gridView1.Columns[5].Caption = "اسم المنخرط الحالي";
            gridView1.Columns[6].Caption = "تاريخ التحويل";
            gridView1.Columns[7].Caption = "معلومات عن التحويل";
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag== "Annuler")
            {
                this.Close();
            }
        }
    }
}