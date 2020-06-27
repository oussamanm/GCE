using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace DWM
{
    public partial class Penalites : Form
    {
        public Penalites()
        {
            InitializeComponent();
        }
        DataSet ds=new DataSet();
        DataView dv=new DataView();
        string str;
        int IndexRowSelected;
        int IdRowSelected;
        MySqlDataAdapter da;

        ////// Forms ////
        AjouPen addPen = new AjouPen();
        AjouPen AjouPe;
        Sleep FSleep = new Sleep();


        public void RempPen()
        {
            if (ds.Tables["penalite"]!=null) 
                ds.Tables["penalite"].Clear();

            da = new MySqlDataAdapter("select idPena,NumComp,concat(A.PrenomArAdhe,' ',A.NomArAdhe) as NomComp,LibelleTypePena,MontantPena,DatePena,case when PayerPena=1 then 'نعم' else 'لا' end as paye,DatePayerPena,DescriptionPena from penalite p,compteur c,adherent A,typepenalite t,utilisateurs u where  p.idUser=u.idUser and p.idComp=c.idComp  and A.IdAdherent=C.IdAdherent and p.idTypePena=t.idTypePena  ", ClassConnexion.Macon);
            da.Fill(ds, "penalite");

        }
        public void RempTabPen()
        {
            dv.Table = ds.Tables["penalite"];
            gridControl1.DataSource = dv;
            gridControl1.Refresh();
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag.ToString() == "Ajouter")
                    addPen.ShowDialog(this);
                else if (e.Button.Properties.Tag.ToString() == "Modifier")
                {
                    if (gridView1.SelectedRowsCount > 0)
                    {
                        AjouPe = new AjouPen(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString()));
                        AjouPe.ShowDialog(this);
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                else if (e.Button.Properties.Tag.ToString() == "Supprimer")
                {
                    string boolPaiePena = "";

                    DialogResult drt = XtraMessageBox.Show("هل تريد فعلا حذف المخالفة؟", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (gridView1.SelectedRowsCount > 0)
                    {
                        if (drt == DialogResult.OK)
                        {
                            boolPaiePena = gridView1.GetDataRow(gridView1.FocusedRowHandle)[6].ToString();

                            if (boolPaiePena == "نعم")
                                XtraMessageBox.Show("لا بمكن إزالة مخالفة مدفوعة", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                            {
                                string forma = "yyyy-MM-dd HH:mm:ss";
                                string RequeteSupp = string.Format("delete from penalite where  idPena=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0] + " and PayerPena=0");
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف مخالفة   - " + DateTime.Now.ToString(forma);
                                string NMsg = "رقم المخالفة : " + gridView1.GetDataRow(1)[0].ToString() + " | نوع المخامفة : " + gridView1.GetDataRow(1)[3].ToString() + " | رقم العداد : " + gridView1.GetDataRow(1)[1].ToString() + " | مالك العداد : " + gridView1.GetDataRow(1)[2].ToString();

                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد حذفه");
                }
                else if (e.Button.Properties.Tag.ToString() == "Imprimer")
                {
                    splashScreenManager1.ShowWaitForm();

                    ArrayList xrAL = new ArrayList(4);
                    ArrayList xAL = new ArrayList(4);
                    xrAL.Add("عدد المخالفات");
                    xrAL.Add("مجموع المدفوعة");
                    xrAL.Add("مجموع المستحقة");
                    xrAL.Add("المجموع");

                    double MtnPai = 0, MtnNPai = 0, Totale = 0;
                    for (int i = 0; i < gridView1.DataRowCount; i++)
                    {
                        DataRow row = gridView1.GetDataRow(i);
                        if (row[6].ToString() == "نعم")
                            MtnPai += double.Parse(row[4].ToString());
                        else if (row[6].ToString() == "لا")
                            MtnNPai += double.Parse(row[4].ToString());

                        Totale += double.Parse(row[4].ToString());
                    }

                    xAL.Add(gridView1.RowCount.ToString());
                    xAL.Add(MtnPai.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));
                    xAL.Add(MtnNPai.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));
                    xAL.Add(Totale.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));

                    gridView1.Columns[0].VisibleIndex = 8;
                    gridView1.Columns[1].VisibleIndex = 7;
                    gridView1.Columns[2].VisibleIndex = 6;
                    gridView1.Columns[3].VisibleIndex = 5;
                    gridView1.Columns[4].VisibleIndex = 4;
                    gridView1.Columns[5].VisibleIndex = 3;
                    gridView1.Columns[6].VisibleIndex = 2;
                    gridView1.Columns[7].VisibleIndex = 1;
                    gridView1.Columns[8].VisibleIndex = 0;

                    DefaultXtraReport report = new DefaultXtraReport();
                    report.GridControl = gridControl1;
                    report.load();
                    report.RempReport("لائحة المخالفات", false, "", "", true, 2, 4, xrAL, xAL);
                    //report.DataSource = ds;

                    report.CreateDocument();
                    splashScreenManager1.CloseWaitForm();
                    report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    new ReportPrintTool(report).ShowPreview();

                    gridView1.Columns[0].VisibleIndex = 0;
                    gridView1.Columns[1].VisibleIndex = 1;
                    gridView1.Columns[2].VisibleIndex = 2;
                    gridView1.Columns[3].VisibleIndex = 3;
                    gridView1.Columns[4].VisibleIndex = 4;
                    gridView1.Columns[5].VisibleIndex = 5;
                    gridView1.Columns[6].VisibleIndex = 6;
                    gridView1.Columns[7].VisibleIndex = 7;
                    gridView1.Columns[8].VisibleIndex = 8;
                }
                RempPen();
                RempTabPen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Penalites_Load(object sender, EventArgs e)
        {
            try
            {

                RempPen();
                RempTabPen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string datepaiement = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);
                if (datepaiement == "")
                {
                    e.Appearance.BackColor = Color.FromArgb(0xE1, 0xFB, 0x13, 0x13);
                    e.Appearance.BackColor2 = Color.FromArgb(0x00, 0x00, 0x00, 0x00);
                }
            }
        }

        //// Generale
        private void label3_Click(object sender, EventArgs e)
        {
            FormMenu fm = new FormMenu();
            fm.ShowDialog();
            this.Close();
        }
        private void panel2_Click(object sender, EventArgs e)
        {
            FormMenu fm = new FormMenu();
            fm.ShowDialog();
            this.Close();
        }
        private void panelControlRedui_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panelControl3_Click(object sender, EventArgs e)
        {
            FSleep.ShowDialog(this);
        }
        private void panelControl1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
