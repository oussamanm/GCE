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
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace DWM
{
    public partial class EntSortCais : DevExpress.XtraEditors.XtraForm
    {
        public EntSortCais()
        {
            InitializeComponent();
        }
        Boolean TestDejaEntE, TestDejaEntS = false;
        DataSet ds;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        DataView dvE, dvS;
        int IndexEntrSelected, IndexSortSelected = 0;
        string formatt = "yyyy-MM-dd HH:mm:ss";
        string formatSimple = "yyyy-MM-dd";
        DateTime DateDEntrFilter, DateDSortFilter;
        DateTime DateFEntrFilter, DateFSortFilter;

        //// Void Generale:
        private int TestSuppCatEntrOrSort(string strReq)
        {
            int int_Resul = 0;
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            using (MySqlCommand CmdTestSupp = new MySqlCommand(strReq, ClassConnexion.Macon))
            {
                using (dr = CmdTestSupp.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dr.Read())
                    {
                        int_Resul = int.Parse(dr[0].ToString());
                        return int_Resul;
                    }
                }
            }
            return -1;
        }
        private void FilterGenerale(string StrFilter, Boolean UsePlus, CheckBox Ch1, CheckBox Ch2, CheckBox Ch3, CheckBox Ch4,System.Windows.Forms.ComboBox Cb1, System.Windows.Forms.ComboBox Cb2,DateEdit DE1, DateEdit DE2,string Col1,string Col2,string Col3,DataView dv)
        {
            //string StrFilter = " ";
            //Boolean UsePlus = false;

            if (Ch1.Checked == true)
            {
                if (Cb1.SelectedValue != "")
                {
                    if (UsePlus == false)
                    {
                        StrFilter = StrFilter + Col1 + " = " + Cb1.SelectedValue + " ";
                        UsePlus = true;
                    }
                    else
                    {
                        StrFilter = StrFilter + " And " + Col1 + " = " + Cb1.SelectedValue + " ";
                    }
                }
            }
            if (Ch2.Checked == true)
            {
                if (Cb2.SelectedValue != "")
                {
                    if (UsePlus == false)
                    {
                        StrFilter = StrFilter + Col2 + " = " + Cb2.SelectedValue + " ";
                        UsePlus = true;
                    }
                    else
                    {
                        StrFilter = StrFilter + " And " + Col2 + " = " + Cb2.SelectedValue + " ";
                    }
                }
            }
            if (Ch3.Checked == true && Ch4.Checked == false)
            {
                if (DE1.EditValue.ToString() != "")
                {
                    if (UsePlus == false)
                    {
                        //DateDEntrFilter = DateTime.Parse(DE1.EditValue.ToString());
                        StrFilter = StrFilter + Col3 + " = '" + DateTime.Parse(DE1.EditValue.ToString()).ToString(formatSimple) + "' ";
                        UsePlus = true;
                    }
                    else
                    {
                        StrFilter = StrFilter + " And " + Col3 + " = '" + DateTime.Parse(DE1.EditValue.ToString()).ToString(formatSimple) + "' ";
                    }
                }
            }
            if (Ch3.Checked == true && Ch4.Checked == true)
            {
                if (DE1.EditValue.ToString() != "" || DE2.EditValue.ToString() != "")
                {
                    if (UsePlus == false)
                    {
                        //DateDEntrFilter = DateTime.Parse(DE1.EditValue.ToString());
                        //DateFEntrFilter = DateTime.Parse(DE2.EditValue.ToString());
                        StrFilter = StrFilter + Col3 + " >= '" + DateTime.Parse(DE1.EditValue.ToString()).ToString(formatSimple) + "' And " +Col3 + " <=  '" + DateTime.Parse(DE2.EditValue.ToString()).ToString(formatSimple) + "' ";                       
                    }
                    else
                    {
                        StrFilter = StrFilter + " And " + Col3 + " >= '" + DateTime.Parse(DE1.EditValue.ToString()).ToString(formatSimple) + "' And " + Col3 + " <= '" + DateTime.Parse(DE2.EditValue.ToString()).ToString(formatSimple) + "' ";
                    }
                }
            }
            //MessageBox.Show("StrFilter " + StrFilter);
            dv.RowFilter = StrFilter;
        }

        /////// ****************** Entres Caisse **********************////////

        ///// Void 
        private void RempDataEntr()
        {
            if (TestDejaEntE==true)
            {
                ds.Tables["EntrCais"].Clear();
            }

            da = new MySqlDataAdapter("select E.*,CE.LibCatEntr,C.LibCais,concat(U.NomUser,' ',U.PrenomUser) as NomCompUser from entrescais E,catentres CE,caisse C,utilisateurs U where E.CatEntres_IdCatEntr = CE.IdCatEntr and E.Caisse_IdCais = C.IdCais and E.IdUser=U.IdUser and E.SuppEntr=0 and CE.IdCatEntr > 4 ", ClassConnexion.Macon);
            da.Fill(ds, "EntrCais");
            dvE.Table = ds.Tables["EntrCais"];
            TestDejaEntE = true;
        }
        private void RemGridEntr()
        {
            gridControlEntr.DataSource = dvE;
            gridControlEntr.Refresh();
        }
        private void FilterGridEntr()
        {
            if (checkBoxEntrCais.Checked == true && checkBoxDateDEntr.Checked==false && checkBoxDateFEntr.Checked == false)
            {
                while (comboBoxEntr.SelectedValue != "")
                {
                    dvE.RowFilter = " Caisse_IdCais= " + comboBoxEntr.SelectedValue + " ";
                    return;
                }
            }
            else if (checkBoxEntrCais.Checked == true && checkBoxDateDEntr.Checked == true && checkBoxDateFEntr.Checked == false)
            {
                while (comboBoxEntr.SelectedValue != "" && dateEditDateDEntr.EditValue.ToString() !="")
                {
                    DateDEntrFilter = DateTime.Parse(dateEditDateDEntr.EditValue.ToString());
                    dvE.RowFilter = " Caisse_IdCais= " + comboBoxEntr.SelectedValue + " and DateEntr = '"+ DateDEntrFilter.ToString(formatt)+ "' ";
                    return;
                }
            }
            else if (checkBoxEntrCais.Checked == false && checkBoxDateDEntr.Checked == true && checkBoxDateFEntr.Checked == false)
            {
                if (dateEditDateDEntr.EditValue.ToString() != "")
                {
                    DateDEntrFilter = DateTime.Parse(dateEditDateDEntr.EditValue.ToString());
                    dvE.RowFilter = "DateEntr = '" + DateDEntrFilter.ToString(formatt) + "' ";
                }
            }
            else if (checkBoxEntrCais.Checked == false && checkBoxDateDEntr.Checked == true && checkBoxDateFEntr.Checked == true)
            {
                if (dateEditDateDEntr.EditValue.ToString() != "" && dateEditDateFEntr.EditValue.ToString() != "")
                {
                    DateDEntrFilter = DateTime.Parse(dateEditDateDEntr.EditValue.ToString());
                    DateFEntrFilter = DateTime.Parse(dateEditDateFEntr.EditValue.ToString());
                    dvE.RowFilter = "DateEntr >= '" + DateDEntrFilter.ToString(formatt) + "' and DateEntr <= '" + DateFEntrFilter.ToString(formatt) + "' ";
                }
            }
            else if (checkBoxEntrCais.Checked == true && checkBoxDateDEntr.Checked == true && checkBoxDateFEntr.Checked == true)
            {
                if (dateEditDateDEntr.EditValue.ToString() != "" && dateEditDateFEntr.EditValue.ToString() != "")
                {
                    DateDEntrFilter = DateTime.Parse(dateEditDateDEntr.EditValue.ToString());
                    DateFEntrFilter = DateTime.Parse(dateEditDateFEntr.EditValue.ToString());
                    dvE.RowFilter = "DateEntr >= '" + DateDEntrFilter.ToString(formatt) + "' and DateEntr <= '" + DateFEntrFilter.ToString(formatt) + "' ";
                }
            }
            else
            {
                dvE.RowFilter = "";
            }
        }

        ///// Code
        private void simpleButtonEntr_Click(object sender, EventArgs e)
        {
            try
            {
                RempDataEntr();
                RemGridEntr();

                navigationFrame1.SelectedPageIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelEntr_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Ajouter")
                {
                    AjouESCais AjE = new AjouESCais(1, 1);
                    AjE.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag == "Modifier")
                {
                    if (IndexEntrSelected != 0)
                    {
                        AjouESCais ModE = new AjouESCais(1, 2, IndexEntrSelected);
                        ModE.ShowDialog(this);
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                else if (e.Button.Properties.Tag == "Supprimer")
                {                 
                    if (IndexEntrSelected > 0)
                    {
                        DialogResult dres = XtraMessageBox.Show("هل تريد فعلا حذف هذاالمدخول", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dres == DialogResult.Yes)
                        {
                            if (TestSuppCatEntrOrSort("select IdSrcEntr from entrescais where IdEnt =" + IndexEntrSelected + " ") == 0)
                            {
                                string RequeteSupp = string.Format("update entrescais set SuppEntr=1,IdUserSupp={0},DateSuppEntr= \\'{1}\\' WHERE IdEnt={2} ", UserConnecte.IdUser,DateTime.Now.ToString(formatt),IndexEntrSelected);
                                string NMsg = "الرقم الترتيبي : " + gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)[0].ToString() + "  تاريخ المدخول : " + gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)[1].ToString() + " | المبلغ : " + gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)[2].ToString() + " | نوع المدخول: " + gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)["LibCatEntr"].ToString() + " | المحفضة : " + gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)["LibCais"].ToString() + " | الوصف : " + gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)[3].ToString() + " ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف المدخول من قائمة المداخيل   - " + DateTime.Now.ToString(formatt);
                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else if (Configuration.Func(15) == "Direct")
                                    XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                                XtraMessageBox.Show("لايمكن حدف هذا النوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد حدفه","", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (e.Button.Properties.Tag == "Imprimer")
                {
                    splashScreenManager1.ShowWaitForm();

                    ArrayList xrAL = new ArrayList(2);
                    ArrayList xAL = new ArrayList(2);
                    xrAL.Add("عدد المداخيل");
                    xrAL.Add("المجموع");

                    xAL.Add(gridViewEntr.RowCount.ToString());
                    xAL.Add(MontantEntr.SummaryText.ToString());

                    gridViewEntr.Columns[0] .VisibleIndex = 7;
                    gridViewEntr.Columns[1].VisibleIndex = 6;
                    gridViewEntr.Columns[2].VisibleIndex = 5;
                    gridViewEntr.Columns[3].VisibleIndex = 4;
                    gridViewEntr.Columns[7].VisibleIndex = 3;
                    gridViewEntr.Columns[8].VisibleIndex = 2;
                    gridViewEntr.Columns[9].VisibleIndex = 1;

                    DefaultXtraReport report = new DefaultXtraReport();
                    report.GridControl = gridControlEntr;
                    report.load();

                    string StrLBH = "";
                    string StrLH = "";
                    Boolean UseHeader = true;
                    if (checkBoxDateDEntr.Checked == true && checkBoxDateFEntr.Checked == false && dateEditDateDEntr.EditValue.ToString() != "")
                    {
                        StrLBH = "لفترة";
                        StrLH = DateTime.Parse(dateEditDateDEntr.EditValue.ToString()).ToString(formatSimple);
                    }
                    else if (checkBoxDateDEntr.Checked == true && checkBoxDateFEntr.Checked == true && dateEditDateDEntr.EditValue.ToString() != "" && dateEditDateFEntr.EditValue.ToString() != "")
                    {
                        StrLBH = "للفترة بين";
                        StrLH = DateTime.Parse(dateEditDateDEntr.EditValue.ToString()).ToString(formatSimple) + " & " + DateTime.Parse(dateEditDateFEntr.EditValue.ToString()).ToString(formatSimple);
                    }
                    else
                        UseHeader = false;
                    
                    report.RempReport("لائحة المداخيل", UseHeader, StrLBH, StrLH, true, 1, 2, xrAL, xAL);
                    //report.DataSource = ds;

                    report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    report.CreateDocument();
                    splashScreenManager1.CloseWaitForm();
                    new DevExpress.XtraReports.UI.ReportPrintTool(report).ShowPreview();

                    gridViewEntr.Columns[0].VisibleIndex = 0;
                    gridViewEntr.Columns[1].VisibleIndex = 1;
                    gridViewEntr.Columns[2].VisibleIndex = 2;
                    gridViewEntr.Columns[3].VisibleIndex = 3;
                    gridViewEntr.Columns[7].VisibleIndex = 4;
                    gridViewEntr.Columns[8].VisibleIndex = 5;
                    gridViewEntr.Columns[9].VisibleIndex = 6;

                }

                RempDataEntr();
                RemGridEntr();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridViewEntr_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                IndexEntrSelected = int.Parse(gridViewEntr.GetDataRow(gridViewEntr.FocusedRowHandle)[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //// Filter GridView

        string StrFE = "";
        Boolean UsePE = false;
        private void checkBoxEntrCais_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxEntrCais.Checked == true)
                {
                    comboBoxEntr.Enabled = true;

                    comboBoxEntr.SelectedIndexChanged -= new EventHandler(comboBoxEntr_SelectedIndexChanged);
                    Configuration.RempCombo(comboBoxEntr, "select IdCais,LibCais from caisse", "LibCaisse", "IdCais", "LibCais");
                    comboBoxEntr.SelectedIndexChanged += new EventHandler(comboBoxEntr_SelectedIndexChanged);
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);

                }
                else
                {
                    comboBoxEntr.Enabled = false;
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void checkBoxCatEntr_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxCatEntr.Checked == true)
                {
                    comboBoxCatEntr.Enabled = true;

                    comboBoxCatEntr.SelectedIndexChanged -= new EventHandler(comboBoxCatEntr_SelectedIndexChanged);
                    Configuration.RempCombo(comboBoxCatEntr, "select IdCatEntr,LibCatEntr from catentres where VisibCatEntr=1", "LibCatEntres", "IdCatEntr", "LibCatEntr");
                    comboBoxCatEntr.SelectedIndexChanged += new EventHandler(comboBoxCatEntr_SelectedIndexChanged);
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE,checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);

                }
                else
                {
                    comboBoxCatEntr.Enabled = false;
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE,checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void checkBoxDateDEntr_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxDateDEntr.Checked == true)
                {
                    dateEditDateDEntr.EditValue = DateTime.Now.ToShortDateString();
                    dateEditDateDEntr.Enabled = true;
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
                }
                else
                {
                    checkBoxDateFEntr.Checked = false;
                    dateEditDateDEntr.Enabled = false;
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void checkBoxDateFEntr_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxDateFEntr.Checked == false)
                {
                    dateEditDateFEntr.Enabled = false;
                    //FilterGridEntr();
                    FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
                }
                else
                {
                    if (checkBoxDateDEntr.Checked == true)
                    {
                        dateEditDateFEntr.EditValue = DateTime.Now.ToShortDateString();
                        dateEditDateFEntr.Enabled = true;
                        //FilterGridEntr();
                        FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBoxEntr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void comboBoxCatEntr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //FilterGridEntr();
                FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dateEditDateDEntr_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //FilterGridEntr();
                FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dateEditDateFEntr_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //FilterGridEntr();
                FilterGenerale(StrFE, UsePE, checkBoxEntrCais, checkBoxCatEntr, checkBoxDateDEntr, checkBoxDateFEntr, comboBoxEntr, comboBoxCatEntr, dateEditDateDEntr, dateEditDateFEntr, "Caisse_IdCais", "CatEntres_IdCatEntr", "DateEntr", dvE);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /////// ****************** Sorties Caisse **********************////////

        ///// Void 
        private void RempDataSort()
        {
            if (TestDejaEntS == true)
            {
                ds.Tables["SortCais"].Clear();
            }

            //da = new MySqlDataAdapter("select S.*,case when CrediSort=0 then 'نعم' else 'لا' end as PaieSort,CS.LibCatSort,C.LibCais,concat(U.NomUser,' ',U.PrenomUser) as NomCompUser from sortiescais S,catsorties CS,caisse C,utilisateurs U where S.CatSorties_IdCatSort = CS.IdCatSort and S.Caisse_IdCais = C.IdCais and S.IdUser = U.IdUser and S.SuppSort=0  ", ClassConnexion.Macon);
            da = new MySqlDataAdapter("select S.*,CS.LibCatSort,C.LibCais,concat(U.NomUser,' ',U.PrenomUser) as NomCompUser from sortiescais S,catsorties CS,caisse C,utilisateurs U where S.CatSorties_IdCatSort = CS.IdCatSort and S.Caisse_IdCais = C.IdCais and S.IdUser = U.IdUser and S.SuppSort=0  ", ClassConnexion.Macon);
            da.Fill(ds, "SortCais");
            dvS.Table = ds.Tables["SortCais"];
            TestDejaEntS = true;
        }
        private void RemGridSort()
        {
            gridControlSort.DataSource = dvS;
            gridControlSort.Refresh();
        }

        //// Code
        private void simpleButtonSort_Click(object sender, EventArgs e)
        {
            try
            {
                RempDataSort();
                RemGridSort();

                navigationFrame1.SelectedPageIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelMainSort_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Ajouter")
                {
                    AjouESCais AjS = new AjouESCais(2,1);
                    AjS.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag == "Modifier")
                {
                    if (IndexSortSelected != 0)
                    {
                        AjouESCais ModS = new AjouESCais(2, 2, IndexSortSelected);
                        ModS.ShowDialog(this);
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                else if (e.Button.Properties.Tag == "Supprimer")
                {
                    if (IndexSortSelected > 0)
                    {
                        DialogResult dres = XtraMessageBox.Show("هل تريد فعلا حذف هذاالمصروف", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dres == DialogResult.Yes)
                        {
                            //if (TestSuppCatEntrOrSort("select IdSrcEntr from entrescais where IdEnt =" + IndexSortSelected + " ") == 0)
                            //{
                            string RequeteSupp = string.Format("update sortiescais set SuppSort=1 ,IdUserSupp={0} , DateSuppSort=\\'{1}\\' WHERE IdSort={2} ", UserConnecte.IdUser, DateTime.Now.ToString(formatt), IndexSortSelected);
                            string NMsg = "الرقم الترتيبي : " + gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)[0].ToString() + "  تاريخ المصروف : " + gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)[1].ToString() + " | المبلغ : " + gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)[2].ToString() + " | نوع الصروف: " + gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)["LibCatSort"].ToString() + " | المحفضة : " + gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)["LibCais"].ToString() + " | الوصف : " + gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)[3].ToString() + " ";
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف مصروف من قائمة المصاريف   - " + DateTime.Now.ToString(formatt);
                            Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");

                            if (Configuration.Func(15) == "Indirect")
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else if (Configuration.Func(15) == "Direct")
                                XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //}
                            //else
                            //    XtraMessageBox.Show("لايمكن حدف هذا النوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد حدفه", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (e.Button.Properties.Tag == "Imprimer")
                {
                    splashScreenManager1.ShowWaitForm();

                    ArrayList xrAL = new ArrayList(2);
                    ArrayList xAL = new ArrayList(2);
                    xrAL.Add("عدد المصاريف");
                    xrAL.Add("المجموع");

                    xAL.Add(gridViewSort.RowCount.ToString());
                    xAL.Add(MontantSort.SummaryText.ToString());

                    gridViewSort.Columns[0].VisibleIndex = 6;
                    gridViewSort.Columns[1].VisibleIndex = 5;
                    gridViewSort.Columns[2].VisibleIndex = 4;
                    gridViewSort.Columns[3].VisibleIndex = 3;
                    gridViewSort.Columns[4].VisibleIndex = 2;
                    gridViewSort.Columns[5].VisibleIndex = 1;


                    DefaultXtraReport report = new DefaultXtraReport();
                    report.GridControl = gridControlSort;
                    report.load();

                    string StrLBH="";
                    string StrLH = "";
                    Boolean UseHeader = true;
                    if (checkBoxDateDSort.Checked == true && checkBoxDateFSort.Checked == false && dateEditDateDSort.EditValue.ToString()!=""){
                        StrLBH = "لفترة";
                        StrLH = DateTime.Parse(dateEditDateDSort.EditValue.ToString()).ToString(formatSimple);
                    }
                    else if (checkBoxDateDSort.Checked == true && checkBoxDateFSort.Checked == true && dateEditDateDSort.EditValue.ToString() !="" && dateEditDateFSort.EditValue.ToString() !=""){
                        StrLBH ="للفترة بين";
                        StrLH = DateTime.Parse(dateEditDateDSort.EditValue.ToString()).ToString(formatSimple) +" & "+ DateTime.Parse(dateEditDateFSort.EditValue.ToString()).ToString(formatSimple); 
                    }
                    else
                        UseHeader = false;

                    report.RempReport("لائحة المصاريف", UseHeader, StrLBH, StrLH, true, 1,2, xrAL, xAL);
                    //report.DataSource = ds;

                    report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    report.CreateDocument();
                    splashScreenManager1.CloseWaitForm();
                    report.ShowPreviewDialog();


                    gridViewSort.Columns[0].VisibleIndex = 1;
                    gridViewSort.Columns[1].VisibleIndex = 2;
                    gridViewSort.Columns[2].VisibleIndex = 3;
                    gridViewSort.Columns[3].VisibleIndex = 4;
                    gridViewSort.Columns[4].VisibleIndex = 5;
                    gridViewSort.Columns[5].VisibleIndex = 6;

                }

                RempDataSort();
                RemGridSort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void gridViewSort_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                IndexSortSelected = int.Parse(gridViewSort.GetDataRow(gridViewSort.FocusedRowHandle)[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //// Filter GridView

        string StrF = "";
        Boolean UseP = false;
        private void checkBoxTouts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (checkBoxTouts.Checked == true)
                //{
                //    checkBoxPaie.Checked = false;
                //    checkBoxCredit.Checked = false;
                //}
                //else if (checkBoxPaie.Checked == true)
                //{
                //    checkBoxTouts.Checked = false;
                //    checkBoxCredit.Checked = false;
                //}
                //else if (checkBoxCredit.Checked == true)
                //{
                //    checkBoxTouts.Checked = false;
                //    checkBoxPaie.Checked = false;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void radioGroupFiltrePaie_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (radioGroupFiltrePaie.SelectedIndex == 0)
            //    {
            //        StrF = "";
            //        UseP = false;
            //        FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            //    }
            //    else if(radioGroupFiltrePaie.SelectedIndex == 1)
            //    {
            //        StrF = " CrediSort = 0  ";
            //        UseP = true;
            //        FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            //    }
            //    else if (radioGroupFiltrePaie.SelectedIndex == 2)
            //    {
            //        StrF = " CrediSort = 1  ";
            //        UseP = true;
            //        FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        private void checkBoxSortCais_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxSortCais.Checked == true)
                {
                    comboBoxSortCais.Enabled = true;

                    comboBoxSortCais.SelectedIndexChanged -= new EventHandler(comboBoxSortCais_SelectedIndexChanged);
                    Configuration.RempCombo(comboBoxSortCais, "select IdCais,LibCais from caisse", "LibCaisse", "IdCais", "LibCais");
                    comboBoxSortCais.SelectedIndexChanged += new EventHandler(comboBoxSortCais_SelectedIndexChanged);

                    FilterGenerale(StrF, UseP,checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);

                }
                else
                {
                    comboBoxSortCais.Enabled = false;
                    FilterGenerale(StrF, UseP,checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void checkBoxCatSort_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxCatSort.Checked == true)
                {
                    comboBoxCatSort.Enabled = true;
                    comboBoxCatSort.SelectedIndexChanged -= new EventHandler(comboBoxCatSort_SelectedIndexChanged);
                    Configuration.RempCombo(comboBoxCatSort, "select IdCatSort,LibCatSort from catsorties where VisibCatSort=1", "LibCatSorties", "IdCatSort", "LibCatSort");
                    comboBoxCatSort.SelectedIndexChanged += new EventHandler(comboBoxCatSort_SelectedIndexChanged);
                    FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                }
                else
                {
                    comboBoxCatSort.Enabled = false;
                    FilterGenerale( " ",false,checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void checkBoxDateDSort_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxDateDSort.Checked == true)
                {
                    dateEditDateDSort.EditValue = DateTime.Now.ToShortDateString();
                    dateEditDateDSort.Enabled = true;
                    FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                }
                else
                {
                    checkBoxDateFSort.Checked = false;
                    dateEditDateDSort.Enabled = false;
                    FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void checkBoxDateFSort_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxDateFSort.Checked == false)
                {
                    dateEditDateFSort.Enabled = false;
                    FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                }
                else
                {
                    if (checkBoxDateFSort.Checked == true)
                    {
                        dateEditDateFSort.EditValue = DateTime.Now.ToShortDateString();
                        dateEditDateFSort.Enabled = true;
                        FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void comboBoxSortCais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void comboBoxCatSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dateEditDateDSort_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dateEditDateFSort_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrF, UseP, checkBoxSortCais, checkBoxCatSort, checkBoxDateDSort, checkBoxDateFSort, comboBoxSortCais, comboBoxCatSort, dateEditDateDSort, dateEditDateFSort, "Caisse_IdCais", "CatSorties_IdCatSort", "DateSort", dvS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /////// ****************** Historique **********************////////

        Boolean testDejaOvrireHist = false;
        //// Void

        private void RemDataHist()
        {
            if (testDejaOvrireHist == true)
            {
                ds.Tables["Hist"].Clear();
            }
            da = new MySqlDataAdapter("select id,type,case when type=1 then 'مدخول' else 'مصروف' end as typee,idsrc,montant,descc,idcat, case when type=1 then (select LibCatEntr from catentres where IdCatEntr=idcat) else (select LibCatSort from catsorties where IdCatSort=idcat) end as libcat,idcaiss,(select LibCais from caisse where IdCais = idcaiss) as libcais ,(select concat(NomUser,' ',PrenomUser) from utilisateurs where IdUser = idusersupp) as idusersuppp , datesupp from viewhissuppentrsort ", ClassConnexion.Macon);
            da.Fill(ds, "Hist");
            testDejaOvrireHist = true;
        }
        private void RempGridHis()
        {
            gridControlHisSupp.DataSource = ds.Tables["Hist"];
            gridControlHisSupp.Refresh();
        }

        //// Load
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                RemDataHist();
                RempGridHis();
                navigationFrame1.SelectedPageIndex = 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //// Génerale 
        private void Sorties_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                dvE = new DataView();
                dvS = new DataView();

                simpleButtonEntr_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void panelControl1_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Login LogFORM = new Login();
            LogFORM.Show();

            this.Close();
            splashScreenManager1.CloseWaitForm();
        }
        private void panelControl3_Click(object sender, EventArgs e)
        {
            Sleep FSleep = new Sleep();
            FSleep.ShowDialog(this);
        }
        private void panelControlRedui_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panel2_Click_1(object sender, EventArgs e)
        {
            FormMenu MENU = new FormMenu();
            MENU.Show();
            this.Hide();
        }


        /////// ****************** Credit **********************////////

        Boolean testDejaOvrireCredi = false;
        DataView dvCrd;
        int IndexCrdSelected, IndexCrdSelected_IdEntr = 0;

        //// Void
        float MontCrd, MontNPCrd, MontGCrd = 0;
        int IdCCrd = 0;
        string AnneeCrd = "";
        private float CalculRequte(string Req)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
            {
                ClassConnexion.Macon.Open();
            }
            float Resu = 0;
            MySqlCommand Cmd = new MySqlCommand(Req, ClassConnexion.Macon);
            dr = Cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr[0].ToString() != "")
                {
                    Resu = float.Parse(dr[0].ToString());
                    dr.Close();
                    ClassConnexion.Macon.Close();
                    return Resu;
                }
            }
            dr.Close();
            ClassConnexion.Macon.Close();
            return 0;
        }
        private void RempDataCredit()
        {
            dvCrd = new DataView();

            if (testDejaOvrireCredi == true)
                ds.Tables["Credit"].Clear();

            da = new MySqlDataAdapter("select Crd.IdCrd,Crd.LibCrd,Crd.DateCrd,Crd.MontantCrd,Crd.DescCrd,Crd.PaieCrd,case when Crd.PaieCrd = 0 then 'لا' else 'نعم' end as PaieCrdd,Crd.DatePaieCrd,Crd.SuppCrd,Crd.DateSuppCrd,Crd.IdCais,Crd.IdEntr,Crd.IdSort,Date_Format(Crd.DateCrd,\"%Y\") as AnneeDateCrd,(select LibCais from caisse C where C.IdCais=Crd.IdCais) as LibCais,(select concat(NomUser,' ',PrenomUser) from utilisateurs where IdUser = Crd.IdUser) as NomCUser from Credit Crd where Crd.SuppCrd=0 ", ClassConnexion.Macon);
            da.Fill(ds, "Credit");
            dvCrd.Table = ds.Tables["Credit"];
            testDejaOvrireCredi = true;
        } 
        private void RempGridCredit()
        {
            gridControlCredit.DataSource = dvCrd;
            gridControlCredit.Refresh();
        }
        private void RempFooterCrd()
        {
            if (checkBoxAnneCredit.Checked == false && checkBoxCaissCredit.Checked == false)
            {
                lbAnnesCrd.Text = "- - - -";
                MontCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0 ");
                MontNPCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=1 ");
                MontGCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 ");
            }
            else if (checkBoxAnneCredit.Checked == true && checkBoxCaissCredit.Checked == false)
            {
                lbAnnesCrd.Text = AnneeCrd;
                MontCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0 and Date_Format(DateCrd,\"%Y\")='" + AnneeCrd + "' ");
                MontNPCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=1 and Date_Format(DateCrd,\"%Y\")='" + AnneeCrd + "' ");
                MontGCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0  and Date_Format(DateCrd,\"%Y\")='" + AnneeCrd + "' ");
            }
            else if (checkBoxAnneCredit.Checked == false && checkBoxCaissCredit.Checked == true)
            {
                lbAnnesCrd.Text = "- - - -";
                MontCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0 and IdCais=" + IdCCrd + " ");
                MontNPCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=1 and dCais=" + IdCCrd + " ");
                MontGCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0  and dCais=" + IdCCrd + " ");
            }
            else if (checkBoxAnneCredit.Checked == true && checkBoxCaissCredit.Checked == true)
            {
                lbAnnesCrd.Text = AnneeCrd;
                MontCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=0 and IdCais=" + IdCCrd + " and Date_Format(DateCrd,\"%Y\")='" + AnneeCrd + "' ");
                MontNPCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0 and PaieCrd=1 and IdCais=" + IdCCrd + " and Date_Format(DateCrd,\"%Y\")='" + AnneeCrd + "' ");
                MontGCrd = CalculRequte("select sum(MontantCrd) from credit where SuppCrd=0  and IdCais=" + IdCCrd + " and Date_Format(DateCrd,\"%Y\")='" + AnneeCrd + "' ");
            }

            LbTotaleCrd.Text = Configuration.ConvertToMony(MontNPCrd);
            LbTotaleNPCrd.Text = Configuration.ConvertToMony(MontCrd);
            LbTotaleGCrd.Text = Configuration.ConvertToMony(MontGCrd);
        }

        //// Load
        private void simpleButtonCredit_Click(object sender, EventArgs e)
        {
            try
            {
                RempDataCredit();
                RempGridCredit();
                RempFooterCrd();
                navigationFrame1.SelectedPageIndex = 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        ///Code
        private void windowsUIButtonPanelCredit_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Ajouter")
                {
                    AjouCrd AjCrd = new AjouCrd(0, 0);
                    AjCrd.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag == "Modifier")
                {
                    if (IndexCrdSelected != 0)
                    {
                        AjouCrd MdCrd = new AjouCrd(1, IndexCrdSelected);
                        MdCrd.ShowDialog(this);
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                }
                else if (e.Button.Properties.Tag == "Supprimer")
                {
                    if (IndexCrdSelected > 0)
                    {
                        DialogResult dres = XtraMessageBox.Show("هل تريد فعلا حذف هذاالدين", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dres == DialogResult.Yes)
                        {
                            if (Configuration.ReturnValueMax("select PaieCrd from credit where IdCrd = " + IndexCrdSelected  ) != "1")
                            {
                                string RequeteSupp = string.Format("update credit set SuppCrd=1 ,IdUserSupp={0} , DateSuppCrd=\\'{1}\\' WHERE IdCrd={2} ", UserConnecte.IdUser, DateTime.Now.ToString(formatt), IndexCrdSelected);
                                string RequeteSuppEntrCais = string.Format("update entrescais set SuppEntr=1 ,IdUserSupp={0} , DateSuppEntr=\\'{1}\\' WHERE IdEnt={2} ", UserConnecte.IdUser, DateTime.Now.ToString(formatt), IndexCrdSelected_IdEntr);

                                string NMsg = "الرقم الترتيبي : " + gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)[0].ToString() + " |  تاريخ الدين : " + gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)[2].ToString() + " | من طرف : " + gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)[1].ToString() + " | المبلغ : " + gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)[3].ToString() + " | الوصف: " + gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)["DescCrd"].ToString() + " | المحفضة : " + gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)["LibCais"].ToString() + " ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف دين من قائمة الديون   - " + DateTime.Now.ToString(formatt);
                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, RequeteSuppEntrCais, "");

                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else if (Configuration.Func(15) == "Direct")
                                    XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                                XtraMessageBox.Show("لايمكن حدف هذا الدين", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        XtraMessageBox.Show("يجب تحديد السطر المراد حدفه", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (e.Button.Properties.Tag == "Imprimer")
                {
                    splashScreenManager1.ShowWaitForm();

                    ArrayList xrAL = new ArrayList(4);
                    ArrayList xAL = new ArrayList(4);
                    xrAL.Add("عدد الديون");
                    xrAL.Add("الديون المؤداة");
                    xrAL.Add("الديون الغير المؤداة");
                    xrAL.Add("المجموع");

                    xAL.Add(gridViewCredit.RowCount.ToString());
                    double MtnPai = 0, MtnNPai = 0, Totale = 0;
                    for (int i = 0; i < gridViewCredit.DataRowCount; i++)
                    {
                        DataRow row = gridViewCredit.GetDataRow(i);
                        if (row["PaieCrdd"].ToString() == "نعم")
                            MtnPai += double.Parse(row["MontantCrd"].ToString());
                        else if (row["PaieCrdd"].ToString() == "لا")
                            MtnNPai += double.Parse(row["MontantCrd"].ToString());

                        Totale += double.Parse(row["MontantCrd"].ToString());
                    }
                    xAL.Add(MtnPai.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));
                    xAL.Add(MtnNPai.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));
                    xAL.Add(Totale.ToString("C2", CultureInfo.CreateSpecificCulture("FR-MA")));

                    gridViewCredit.Columns["IdCrd"].VisibleIndex = 10;
                    gridViewCredit.Columns["LibCrd"].VisibleIndex = 9;
                    gridViewCredit.Columns["DateCrd"].VisibleIndex = 8;
                    gridViewCredit.Columns["MontantCrd"].VisibleIndex = 7;
                    gridViewCredit.Columns["DescCrd"].VisibleIndex = 6;
                    gridViewCredit.Columns["PaieCrdd"].VisibleIndex = 5;
                    gridViewCredit.Columns["DatePaieCrd"].VisibleIndex = 4;
                    gridViewCredit.Columns["LibCais"].VisibleIndex = 3;
                    gridViewCredit.Columns["IdEntr"].VisibleIndex = 2;
                    gridViewCredit.Columns["IdSort"].VisibleIndex = 1;


                    DefaultXtraReport report = new DefaultXtraReport();
                    report.GridControl = gridControlCredit;
                    report.load();

                    string StrLBH = "";
                    string StrLH = "";
                    Boolean UseHeader = true;
                    if (checkBoxAnneCredit.Checked == true)
                    {
                        StrLBH = "عن سنة ";
                        StrLH = comboBoxAnneCredit.SelectedValue.ToString();
                    }
                    else if (checkBoxDDCredit.Checked == true)
                    {
                        if (checkBoxDFCredit.Checked == false && dateEditDateDCredit.EditValue.ToString() != "")
                        {
                            StrLBH = " : لفترة";
                            StrLH = DateTime.Parse(dateEditDateDCredit.EditValue.ToString()).ToString(formatSimple);
                        }
                        else if (checkBoxDFCredit.Checked == true && dateEditDateDCredit.EditValue.ToString() != "" && dateEditDateFCredit.EditValue.ToString() != "")
                        {
                            StrLBH = " : للفترة بين";
                            StrLH = DateTime.Parse(dateEditDateDCredit.EditValue.ToString()).ToString(formatSimple) + " & " + DateTime.Parse(dateEditDateFCredit.EditValue.ToString()).ToString(formatSimple) ;
                        }
                    }
                    else
                        UseHeader = false;

                    report.RempReport("لائحة الديون", UseHeader, StrLBH, StrLH, true, 2, 4, xrAL, xAL);
                    //report.DataSource = ds;

                    report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    report.CreateDocument();
                    splashScreenManager1.CloseWaitForm();
                    new DevExpress.XtraReports.UI.ReportPrintTool(report).ShowPreview();

                    gridViewCredit.Columns["IdCrd"].VisibleIndex = 1;
                    gridViewCredit.Columns["LibCrd"].VisibleIndex = 2;
                    gridViewCredit.Columns["DateCrd"].VisibleIndex = 3;
                    gridViewCredit.Columns["MontantCrd"].VisibleIndex = 4;
                    gridViewCredit.Columns["DescCrd"].VisibleIndex = 5;
                    gridViewCredit.Columns["PaieCrdd"].VisibleIndex = 6;
                    gridViewCredit.Columns["DatePaieCrd"].VisibleIndex = 7;
                    gridViewCredit.Columns["LibCais"].VisibleIndex = 8;
                    gridViewCredit.Columns["IdEntr"].VisibleIndex = 9;
                    gridViewCredit.Columns["IdSort"].VisibleIndex = 10;
                    //MessageBox.Show("Test");
                }

                RempDataCredit();
                RempGridCredit();
                RempFooterCrd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void gridViewCredit_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                IndexCrdSelected = int.Parse(gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)[0].ToString());
                IndexCrdSelected_IdEntr = int.Parse(gridViewCredit.GetDataRow(gridViewCredit.FocusedRowHandle)["IdEntr"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //// Filter
        string StrFCrd = "";
        Boolean UsePCrd = false;

        private void radioGroupFilterCredit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroupFilterCredit.SelectedIndex == 0)
                {
                    StrFCrd = "";
                    UsePCrd = false;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                }
                else if (radioGroupFilterCredit.SelectedIndex == 1)
                {
                    StrFCrd = " PaieCrd = 1  ";
                    UsePCrd = true;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                }
                else if (radioGroupFilterCredit.SelectedIndex == 2)
                {
                    StrFCrd = " PaieCrd = 0 ";
                    UsePCrd = true;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBoxAnneCredit_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAnneCredit.Checked == true)
            {
                comboBoxAnneCredit.Enabled = true;

                comboBoxAnneCredit.SelectedIndexChanged -= new EventHandler(comboBoxAnneCredit_SelectedIndexChanged);
                Configuration.RempComboSimple(comboBoxAnneCredit, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
                comboBoxAnneCredit.SelectedIndexChanged += new EventHandler(comboBoxAnneCredit_SelectedIndexChanged);
                comboBoxAnneCredit_SelectedIndexChanged(sender,e);

                //FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                //RempFooterCrd();
            }
            else
            {
                comboBoxAnneCredit.Enabled = false;
                FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                RempFooterCrd();
            }
        }
        private void checkBoxCaissCredit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxCaissCredit.Checked == true)
                {
                    comboBoxCaissCredit.Enabled = true;

                    comboBoxCaissCredit.SelectedIndexChanged -= new EventHandler(comboBoxCaissCredit_SelectedIndexChanged);
                    Configuration.RempComboSimple(comboBoxCaissCredit, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                    comboBoxCaissCredit.SelectedIndexChanged += new EventHandler(comboBoxCaissCredit_SelectedIndexChanged);
                    comboBoxCaissCredit_SelectedIndexChanged(sender,e);

                    //FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                    //RempFooterCrd();
                }
                else
                {
                    comboBoxCaissCredit.Enabled = false;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                    RempFooterCrd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBoxDDCredit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxDDCredit.Checked == true)
                {
                    dateEditDateDCredit.EditValue = DateTime.Now.ToShortDateString();
                    dateEditDateDCredit.Enabled = true;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                }
                else
                {
                    dateEditDateDCredit.Enabled = false;
                    checkBoxDFCredit.Checked = false;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void checkBoxDFCredit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxDFCredit.Checked == true)
                {
                    if (checkBoxDDCredit.Checked == true)
                    {
                        dateEditDateFCredit.EditValue = DateTime.Now.ToShortDateString();
                        dateEditDateFCredit.Enabled = true;
                        FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                    }
                }
                else
                {
                    dateEditDateFCredit.Enabled = false;
                    FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBoxAnneCredit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AnneeCrd = comboBoxAnneCredit.SelectedValue.ToString();
                FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                RempFooterCrd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void comboBoxCaissCredit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IdCCrd = int.Parse(comboBoxCaissCredit.SelectedValue.ToString());
                FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
                RempFooterCrd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dateEditDateDCredit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dateEditDateFCredit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterGenerale(StrFCrd, UsePCrd, checkBoxAnneCredit, checkBoxCaissCredit, checkBoxDDCredit, checkBoxDFCredit, comboBoxAnneCredit, comboBoxCaissCredit, dateEditDateDCredit, dateEditDateFCredit, "AnneeDateCrd", "IdCais", "DateCrd", dvCrd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        /////// ****************** Creances **********************////////

        Boolean testDejaOvrireCrea = false;
        string AnneeSelcted,typeSelected = "";
        DateTime Dtyear = new DateTime();
        DataView dvCrean;
        float SumMtnCrean = 0;

        //// Void
        private void RempCheckedListeMonthsGen(string strr, CheckedListBoxControl ChLB)
        {
            if (strr != "")
            {
                strr = "11-11-" + strr;
                Dtyear = DateTime.Parse(strr);
                ChLB.Items.Clear();

                var months = Enumerable.Range(1, 12).Select(p => new DateTime(Dtyear.Year, p, 1));
                foreach (var month in months)
                {
                    string str = month.ToString("MM-yyyy");
                    if (str == DateTime.Now.ToString("MM-yyyy"))
                        ChLB.Items.Add(str, true);
                    else
                        ChLB.Items.Add(str, false);
                }
            }
        }
        private void RempDataGridCrean(string typeSelectedd)
        {
            dvCrean = new DataView();
            string Requ = "";

            if (testDejaOvrireCrea == true)
                ds.Tables["Crean"].Clear();

            if (typeSelectedd == "0")
                Requ = "select P.IdPaie as IdCr, Concat(A.PrenomArAdhe,' ',A.NomArAdhe) as NomComp, Cm.NumComp, S.LibelleSect, P.MontantPaie as MtnCr, date_format(F.PeriodeConsoFact,\"%m-%Y\") as DateCr,'إستهلاك الماء + الرسوم + غرامة التأخر' as DescCr from adherent A,compteur Cm,secteur S,consommation Cn,paiement P,facture F where A.IdAdherent = Cm.IdAdherent and Cm.IdSect = S.IdSect and Cn.IdComp=Cm.IdComp and Cn.IdCons = P.IdCons and P.IdFact = F.IdFact and Cn.IdFact=F.IdFact and P.PayePaie=0 ";
            else if(typeSelectedd == "1")
                Requ = "select Mt.IdMTr as IdCr, Concat(A.PrenomArAdhe,' ',A.NomArAdhe) as NomComp, Cm.NumComp, S.LibelleSect, Mt.MontantMTr as MtnCr, Mt.MoisMTr as DateCr,' ' as DescCr  from adherent A,compteur Cm,secteur S,traite Tr,moistraite Mt where A.IdAdherent = Cm.IdAdherent and Cm.IdSect = S.IdSect and Tr.IdComp=Cm.IdComp and Tr.IdTrai = Mt.IdTrai and Mt.PayerMTr=0 ";
            else if (typeSelectedd == "2")
                Requ = "select f.IdEntr as IdCr, Concat(A.PrenomArAdhe,' ',A.NomArAdhe) as NomComp, Cm.NumComp, S.LibelleSect, f.MontantAutFrai as MtnCr, date_format(f.DateAutFrai,\"%d-%m-%Y\") as DateCr,ce.LibelleCatEntr as DescCr from adherent A,compteur Cm,secteur S,autrefrais f,entres e,categorieentres ce where A.IdAdherent = Cm.IdAdherent and Cm.IdSect = S.IdSect and f.IdComp = cm.IdComp and f.IdEntr=e.IdEntr and e.IdCatEntr=ce.IdCatEntr and f.PayerEntr=0";
            else if (typeSelectedd == "3")
                Requ = "select p.IdPena as IdCr, Concat(A.PrenomArAdhe,' ',A.NomArAdhe) as NomComp, Cm.NumComp, S.LibelleSect, p.MontantPena as MtnCr, date_format(p.DatePena,\"%d-%m-%Y\") as DateCr, tp.LibelleTypePena as DescCr from adherent A, compteur Cm, secteur S, penalite p,typepenalite tp where A.IdAdherent = Cm.IdAdherent and Cm.IdSect = S.IdSect and p.IdComp = cm.IdComp and tp.IdTypePena = p.IdTypePena and p.PayerPena=0 ";

            da = new MySqlDataAdapter(Requ, ClassConnexion.Macon);
            da.Fill(ds, "Crean");
            dvCrean.Table = ds.Tables["Crean"];
            testDejaOvrireCrea = true;
        }
        private void RempGridCrean()
        {
            gridControlCrean.DataSource = dvCrean;
            gridControlCrean.Refresh();
        }
        private void FilterGridParChLB()
        {
            try
            {
                if (ChLBCrean.CheckedItemsCount == 0)
                    dvCrean.RowFilter = "DateCr like '%" + CbAnnCrean.SelectedValue.ToString() + "' ";
                if (ChLBCrean.CheckedItemsCount > 1)
                {
                    string Query2 = "";
                    int icount = 0;
                    foreach (var item in ChLBCrean.CheckedItems)
                    {
                        if (icount == ChLBCrean.CheckedItems.Count - 1)
                            Query2 += " DateCr = '" + item + "'";
                        else
                            Query2 += " DateCr = '" + item + "' OR ";
                        icount++;
                    }
                    dvCrean.RowFilter = Query2;
                }
                else if (ChLBCrean.CheckedItemsCount == 1)
                    dvCrean.RowFilter = "DateCr  = '" + ChLBCrean.CheckedItems[0].ToString() + "' ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RempFooterCrean()
        {
            if (CbAnnCrean.Items.Count > 0 && CbAnnCrean.SelectedValue.ToString() != "")
            {

                LbAnnCrean.Text = CbAnnCrean.SelectedValue.ToString();
                float SumMtnCrean = CalculRequte("select ((select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and date_format(F.PeriodeConsoFact,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "')+(select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "') + (select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and date_format(DateAutFrai,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "') + (select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and date_format(DatePena,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "'))");
                
                    
                //if (typeSelected == "0")
                //    SumMtnCrean = CalculRequte("select IFNULL(sum(P.MontantPaie),0) as MtnCr from paiement P,facture F where P.IdFact =F.IdFact and P.PayePaie=0 and date_format(F.PeriodeConsoFact,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString()+"' ");
                //else if (typeSelected == "1")
                //    SumMtnCrean = CalculRequte("select IFNULL(sum(MontantMTr),0) as MtnCr from moistraite where PayerMTr=0 and date_format(MoisMTr,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "' ");
                //else if (typeSelected == "2")
                //    SumMtnCrean = CalculRequte("select IFNULL(sum(MontantAutFrai),0) as MtnCr from autrefrais where PayerEntr=0 and date_format(DateAutFrai,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "' ");
                //else if (typeSelected == "3")
                //    SumMtnCrean = CalculRequte("select IFNULL(sum(MontantPena),0) as MtnCr from penalite where PayerPena=0 and date_format(DatePena,\"%Y\")='" + CbAnnCrean.SelectedValue.ToString() + "' ");

                LbMtnCrean.Text = Configuration.ConvertToMonyC(SumMtnCrean);
            }
        }

        //// Load
        private void simpleButtonCrean_Click(object sender, EventArgs e)
        {
            try
            {
                CbAnnCrean.SelectedIndexChanged -= new EventHandler(CbAnnCrean_SelectedIndexChanged);
                Configuration.RempComboSimple(CbAnnCrean, "select distinct DATE_FORMAT(PeriodeConsoFact,\"%Y\") as DateCons from facture order by DateCons desc", "DateCons", "DateCons");
                CbAnnCrean.SelectedIndexChanged += new EventHandler(CbAnnCrean_SelectedIndexChanged);

                if(CbAnnCrean.Items.Count <= 0)
                {
                    MessageBox.Show("لاتوجد أي فترة إستهلاك");
                    navigationFrame1.SelectedPageIndex = 4;
                    return;
                }
                else
                    CbAnnCrean_SelectedIndexChanged(sender, e);

                CbTypeCrean.SelectedIndex = 0;
                RempCheckedListeMonthsGen(CbAnnCrean.SelectedValue.ToString(), ChLBCrean);
                if (typeSelected != "-1")
                {
                    RempDataGridCrean(typeSelected);
                    RempGridCrean();
                    FilterGridParChLB();
                    RempFooterCrean();
                }

                navigationFrame1.SelectedPageIndex = 4;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// Code    
        private void windowsUIButtonPanelCrean_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Refresh")
                {
                    RempDataGridCrean(typeSelected);
                    RempGridCrean();
                    FilterGridParChLB();
                    RempFooterCrean();
                }
                else if (e.Button.Properties.Tag == "Print")
                {
                    splashScreenManager1.ShowWaitForm();

                    ArrayList xrAL = new ArrayList(2);
                    ArrayList xAL = new ArrayList(2);
                    xrAL.Add("عدد المستحقات");
                    xrAL.Add("المجموع");

                    xAL.Add(gridViewCrean.RowCount.ToString());
                    xAL.Add(gridColumn30.SummaryText.ToString());

                    gridViewCrean.Columns[0].VisibleIndex = 7;
                    gridViewCrean.Columns[1].VisibleIndex = 6;
                    gridViewCrean.Columns[2].VisibleIndex = 5;
                    gridViewCrean.Columns[3].VisibleIndex = 4;
                    gridViewCrean.Columns[4].VisibleIndex = 3;
                    gridViewCrean.Columns[5].VisibleIndex = 2;
                    gridViewCrean.Columns[6].VisibleIndex = 1;

                    DefaultXtraReport report = new DefaultXtraReport();
                    report.GridControl = gridControlCrean;
                    report.load();

                    string StrLBH = "";
                    string StrLH = "";
                    string StrHH = "";
                    int i = 0;
                    Boolean cha = true;

                    if (CbAnnCrean.SelectedValue !="" && ChLBCrean.CheckedItems.Count==1)
                        StrLBH = ": لفترة";
                    else if (CbAnnCrean.SelectedValue != "" && ChLBCrean.CheckedItems.Count >1)
                        StrLBH = " : للفترات ";
                    else if(CbAnnCrean.SelectedValue != "" && ChLBCrean.CheckedItems.Count ==0)
                        StrLBH = " : عن سنة ";

                    for (i = 0; i < ChLBCrean.ItemCount; i++)
                    {
                        if (ChLBCrean.Items[i].CheckState == CheckState.Checked)
                        {
                            if (cha == true)
                            {
                                StrLH = ChLBCrean.Items[i].ToString();
                                cha = false;
                            }
                            else
                                StrLH = StrLH + "  &  " + ChLBCrean.Items[i].ToString();
                        }
                    }

                    if (cha == true)
                        StrLH = CbAnnCrean.SelectedValue.ToString();

                    StrHH = "لائحة المستحقات : " + CbTypeCrean.Items[int.Parse(typeSelected)].ToString();
                    report.RempReport(StrHH, true, StrLBH, StrLH, true, 1, 2, xrAL, xAL);

                    report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                    report.CreateDocument();
                    splashScreenManager1.CloseWaitForm();
                    new DevExpress.XtraReports.UI.ReportPrintTool(report).ShowPreview();

                    gridViewCrean.Columns[0].VisibleIndex = 1;
                    gridViewCrean.Columns[1].VisibleIndex = 2;
                    gridViewCrean.Columns[2].VisibleIndex = 3;
                    gridViewCrean.Columns[3].VisibleIndex = 4;
                    gridViewCrean.Columns[4].VisibleIndex = 5;
                    gridViewCrean.Columns[5].VisibleIndex = 6;
                    gridViewCrean.Columns[6].VisibleIndex = 7;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbAnnCrean_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AnneeSelcted = CbAnnCrean.SelectedValue.ToString();
                if(CbAnnCrean.SelectedValue != "")
                    RempCheckedListeMonthsGen(CbAnnCrean.SelectedValue.ToString(), ChLBCrean);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void CbTypeCrean_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                typeSelected = CbTypeCrean.SelectedIndex.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}