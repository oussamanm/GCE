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
using System.IO;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.XtraGrid;
using System.Globalization;

namespace DWM
{
    public partial class Config : DevExpress.XtraEditors.XtraForm
    {
        public Config()
        {
            InitializeComponent();
            controleHeader1.label_menu_btn = "الصفحة الرئيسية / إعدادات ";
        }

        string AplliqPenaLastMonth="";
        string periodepy,jourPy,datentrinfo,nbrjourinfo,Mtpenalite,act,Tel,email,nomassoc,addres,requeteinsert, requeteinsert2,permission,tranchesmethode, permissionnv, tranchesmethodenv,Eclairage, EclairageNV,Penali_Mo, Penali_MoYN;
        string imgurl;
        string wanted_path;
        DataSet DS,DSROSOM;
        MySqlDataAdapter DA,DAROSOM;
        MySqlCommandBuilder BD;

        MySqlCommand CMD;

        int mod=0;

        string messa, MsgEn,Requete, Requete3;
        string MsgNv = "", MsgAn="", Requete2="";

        String formatdt = "yyyy-MM-dd HH:mm:ss";
        int typop = 0;
        int typeprorasm = 0;

        string MsgAnc = "";
        string msgnv = "";
        string Mesent = "";
        string Requeteaj = "";

        Color slateBlue2 = Color.FromArgb(41, 57, 86);
        Color slateBlue = Color.FromArgb(0, 174, 219);

        public void enr(int IdEnre,string STR,string STR2)
        {
            string forma = "yyyy-MM-dd HH:mm:ss";

            string RequeteAjou = string.Format("update configuration set LibEntr=\\'"+ STR + "\\' where IdConf=" + IdEnre );        

          //  string NMsg = "الرقم الترتيبي : " + TbNumO.Text + " | Nom : " + TbNom.Text + " | Prénom : " + TbPre.Text + " | النسب : " + TbNas.Text + " | الإسم : " + TbIsm.Text + " | ر.ب.و : " + TbCIN.Text + " | الجنس : " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description + " | تاريخ الإنخراط : " + dateEdit1.EditValue + " | تاريخ الإزدياد : " + dateEdit2.EditValue + " | مكان الإزدياد : " + TbVil.Text + " | حالة المنخرط : " + radioGroup2.Properties.Items[radioGroup2.SelectedIndex].Description;
            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتغيير  "+ STR2+" - " + DateTime.Now.ToString(forma);
        
            Configuration.Historique(0, RequeteAjou, "", "", MEnt, "", "");            
        }
        public void remplir()
        {
            chatr.Text = gridView1.GetDataRow(gridView1.FocusedRowHandle)[1].ToString();
            prix.Text= gridView1.GetDataRow(gridView1.FocusedRowHandle)[2].ToString();
            quantitemax.Text= gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString();
            typop = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString());
        }
        void vider()
        {
            chatr.Text = "";
            prix.Text = "";
            quantitemax.Text = "";
            typop = 0;
        }
        void viderrosom()
        {
            rasm.Text = "";
            mtrasm.Text = "";
            descrip.Text = "";

            radioButton1.Checked = true;
           
            typeprorasm = 0;
        }
        void rempgrid(GridView gridv, GridControl grid, string desc, string lib, string requete)
        {
            DataSet datas = new DataSet();
            MySqlDataAdapter dataada = new MySqlDataAdapter(requete, ClassConnexion.Macon);
            dataada.Fill(datas, "data");
            grid.DataSource = datas.Tables[0];
            grid.Refresh();

            gridv.Columns[0].Caption = "الرقم الترتيبي";
            gridv.Columns[1].Caption = lib;
            gridv.Columns[2].Caption = desc;
            gridv.Columns[3].Caption = "المستخدم";
        }
        void viderchamps(TextEdit txt, TextBox txtbox)
        {
            txt.Text = "";
            txtbox.Text = "";

            typmds = 0;
        }
        void rempchamp(TextEdit txt, TextBox txtbox, GridView gridv)
        {
            txt.Text = gridv.GetDataRow(gridv.FocusedRowHandle)[1].ToString();
            txtbox.Text = gridv.GetDataRow(gridv.FocusedRowHandle)[2].ToString();

            typmds = int.Parse(gridv.GetDataRow(gridv.FocusedRowHandle)[0].ToString());
        }


        // *********************** Table referencielle

        public void remplirrosom()
        {
            rasm.Text = gridView2.GetDataRow(gridView2.FocusedRowHandle)[2].ToString();
            mtrasm.Text = gridView2.GetDataRow(gridView2.FocusedRowHandle)[3].ToString();
            descrip.Text = gridView2.GetDataRow(gridView2.FocusedRowHandle)[5].ToString();

            if (gridView2.GetDataRow(gridView2.FocusedRowHandle)[1].ToString()=="1")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
          typeprorasm = int.Parse(gridView2.GetDataRow(gridView2.FocusedRowHandle)[0].ToString());
        }
        private void Config_Load(object sender, EventArgs e)
        {
            //wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            wanted_path = AppDomain.CurrentDomain.BaseDirectory;
            //string path1 = AppDomain.CurrentDomain.BaseDirectory; //working
            //string path2 = System.IO.Directory.GetCurrentDirectory(); //working
            //string path3 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase); // results ==> file:c\...


            periodepy = Configuration.Func(3);
            jourPy= Configuration.Func(4);
            datentrinfo= Configuration.Func(5);
            nbrjourinfo= Configuration.Func(6);
            Mtpenalite= Configuration.Func(7);
            act=  Configuration.Func(13);





            //info association

            Tel = Configuration.Func(11);
            email= Configuration.Func(9);
            nomassoc = Configuration.Func(1);
            addres= Configuration.Func(10);

            textEdit1.Text = nomassoc;
            textEdit2.Text = email;
            textEdit7.Text = addres;
            textEdit8.Text = Tel;

            textEditperiodepy.Text = periodepy;
            textEditnbrjourpy.Text= jourPy;
            textEdit4.Text = datentrinfo;
            textEdit3.Text= nbrjourinfo;
            textEdit6.Text= Mtpenalite;
            textEdit5.Text= act;
            label10.Text = act + "125";


            //Page 2 association
            imgurl= wanted_path+ @"Resources\" + Configuration.Func(8);
            pictureBox1.ImageLocation = imgurl;


            DS = new DataSet();
            DA = new MySqlDataAdapter("select IdTran,LibelleTran,PrixUTran,SeuilMinTran from tranches order by IdTran ASC", ClassConnexion.Macon);
            DA.Fill(DS, "tranches");
            DA.Fill(DS, "tranches2");
            BD = new MySqlCommandBuilder(DA);

            gridControl1.DataSource = DS.Tables[0];
            gridControl2.DataSource = DS.Tables[1];
            gridControl2.Refresh();
            gridControl1.Refresh();

            gridView1.Columns[0].Visible = false;
            gridView1.Columns[1].Caption = "الأشطر";
            gridView1.Columns[2].Caption = "المبلغ بالدرهم";
            gridView1.Columns[3].Caption = "الحد الأقصى للشطر";

            if (gridView1.SelectedRowsCount == 1)
            {
                remplir();
            }


        }


        //////'''''''''''''''  الأشطر  ''''''''''''''''''/////
        private void tileItem1_ItemClick(object sender, TileItemEventArgs e)
        {

            foreach (TileItem item in tileGroup5.Items)
            {
                item.AppearanceItem.Normal.BackColor = slateBlue2;
            }
            tileItem1.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
        }
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            remplir();
        }
        private void windowsUIButtonPanel9_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "AJ")
            {
                try
                {
                    if (chatr.Text == "" || prix.Text == "" || quantitemax.Text == "")
                    {
                        XtraMessageBox.Show("حقل فارغ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //************************ AJOUTER & Modifier
                        if (typop == 0)
                        {
                            //----- Ajouter
                            if (DS.Tables[1].Rows.Count == 0)
                            {
                                DataRow row = DS.Tables[1].NewRow();

                                row[0] = 1;
                                row[1] = chatr.Text;
                                row[2] = prix.Text;


                                row[3] = quantitemax.Text;

                                DS.Tables[1].Rows.Add(row);

                                gridView1.RefreshData();

                                typop = int.Parse(DS.Tables[1].Rows[DS.Tables[1].Rows.Count - 1][0].ToString());

                                XtraMessageBox.Show("تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                mod = 1;
                            }
                            else
                            {
                                if (int.Parse(quantitemax.Text.ToString()) > int.Parse(DS.Tables[1].Rows[DS.Tables[1].Rows.Count - 1][3].ToString()))
                                {
                                    DataRow row = DS.Tables[1].NewRow();

                                    row[0] = int.Parse(DS.Tables[1].Rows[DS.Tables[1].Rows.Count - 1][0].ToString()) + 1;
                                    row[1] = chatr.Text;
                                    row[2] = prix.Text;
                                    row[3] = quantitemax.Text;

                                    DS.Tables[1].Rows.Add(row);

                                    gridView1.RefreshData();

                                    typop = int.Parse(DS.Tables[1].Rows[DS.Tables[1].Rows.Count - 1][0].ToString());

                                    XtraMessageBox.Show("تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    mod = 1;
                                }
                                else
                                {
                                    XtraMessageBox.Show("الحد الأقصى للشرط يجب أن يكون أكبر من الشطر السابق", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            //----- Modifier

                            DS.Tables[1].Rows[gridView1.FocusedRowHandle][1] = chatr.Text;
                            DS.Tables[1].Rows[gridView1.FocusedRowHandle][2] = prix.Text;
                            DS.Tables[1].Rows[gridView1.FocusedRowHandle][3] = quantitemax.Text;

                            gridView1.RefreshData();
                            mod = 1;
                            XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        }

                        //************************
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag == "VI")
            {
                vider();
            }
            else if (e.Button.Properties.Tag == "SU")
            {
                DialogResult DRes = XtraMessageBox.Show("هل تريد فعلا حذف هذا الشرط", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (DRes == DialogResult.Yes)
                {

                    try
                    {
                        if (typop != 0)
                        {
                            if (int.Parse(DS.Tables[1].Rows[gridView1.FocusedRowHandle][0].ToString()) == int.Parse(DS.Tables[1].Rows[int.Parse((gridView1.RowCount.ToString())) - 1][0].ToString()))
                            {
                                DS.Tables[1].Rows[int.Parse(gridView1.RowCount.ToString()) - 1].Delete();

                                gridView1.RefreshData();
                                vider();
                                typop = 0;
                                mod = 1;

                                XtraMessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                XtraMessageBox.Show("لا يمكنك حذف هذا الشرط ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("لم تقم بإختيار أي شطر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.ToString());
                    }
                }
            }
        }
        int lign1 = 0;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult DRese = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DRese == DialogResult.Yes)
            {
                if (mod == 0)
                    XtraMessageBox.Show("لا توجد تعديلات للإرسال", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    splashScreenManager1.ShowWaitForm();

                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        if (lign1 == 1)
                        {
                            requeteinsert += ",";
                            requeteinsert2 += ",";
                        }
                        requeteinsert += " (\\'" + gridView1.GetDataRow(i)[1].ToString() + "\\'," + gridView1.GetDataRow(i)[2].ToString() + "," + gridView1.GetDataRow(i)[3].ToString() + "," + UserConnecte.IdUser + ")";
                        requeteinsert2 += " ( \\'" + gridView1.GetDataRow(i)[1].ToString() + "\\'," + gridView1.GetDataRow(i)[2].ToString() + "," + gridView1.GetDataRow(i)[3].ToString() + "," + (Configuration.LastID("tranchescopie", "IdCopie") + 1) + "," + (i + 1) + ")";

                        lign1 = 1;

                        MsgNv += "\r\n" + gridView1.GetDataRow(i)[1].ToString() + "     " + gridView1.GetDataRow(i)[2].ToString() + "     " + gridView1.GetDataRow(i)[3].ToString();

                        //MessageBox.Show(requeteinsert);
                    }

                    lign1 = 0;
                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        MsgAn += "\r\n" + DS.Tables[0].Rows[i][1].ToString() + "     " + DS.Tables[0].Rows[i][2].ToString() + "     " + DS.Tables[0].Rows[i][3].ToString();
                    }


                    MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديلات على الأشطر  (" + DateTime.Now.ToString(formatdt) + ") :";
                    Requete = "TRUNCATE TABLE tranches";
                    Requete2 = "insert into tranches(LibelleTran,PrixUTran,SeuilMinTran,IdUser) values" + requeteinsert;
                    Requete3 = "insert into tranchescopie(LibelleTranC,PrixUTranC,SeuilMinTranC,IdCopie,IdTran) values" + requeteinsert2;

                    Configuration.Historique(0, Requete, MsgAn, MsgNv, MsgEn, Requete2, Requete3);

                    requeteinsert = "";
                    requeteinsert2 = "";
                    MsgNv = "";
                    MsgAn = "";
                    MsgEn = "";
                    Requete = "";
                    Requete2 = "";
                    Requete3 = "";

                    splashScreenManager1.CloseWaitForm();

                    XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        //////'''''''''''''''  Frais  ''''''''''''''''''/////
        int mdrosom = 0;
        int ROSOMENT = 0;
        private void tileItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                if (ROSOMENT == 0)
                {
                    DSROSOM = new DataSet();
                    DAROSOM = new MySqlDataAdapter("select IdFrai,configYesNon,LibelleFrai,PrixUFrai,case when configYesNon=1 then 'ظاهر' else 'مخفي' end as sta,DescriptionFrai from frais order by IdFrai ASC", ClassConnexion.Macon);
                    DAROSOM.Fill(DSROSOM, "frais");
                    DAROSOM.Fill(DSROSOM, "frais2");

                    gridControl4.DataSource = DSROSOM.Tables[0];
                    gridControl3.DataSource = DSROSOM.Tables[1];
                    gridControl4.Refresh();
                    gridControl3.Refresh();

                    gridView2.Columns[0].Visible = false;
                    gridView2.Columns[1].Visible = false;
                    gridView2.Columns[2].Caption = "الرسوم";
                    gridView2.Columns[3].Caption = "المبلغ بالدرهم";
                    gridView2.Columns[4].Caption = "الحالة";
                    gridView2.Columns[5].Caption = "وصف";

                    ROSOMENT = 1;

                    if (gridView2.SelectedRowsCount == 1)
                    {
                        remplirrosom();
                    }
                }

                foreach (TileItem item in tileGroup5.Items)
                {
                    item.AppearanceItem.Normal.BackColor = slateBlue2;
                }
                tileItem2.AppearanceItem.Normal.BackColor = slateBlue;

                navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);

                splashScreenManager1.CloseWaitForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanel10_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "AJ")
            {
                if (rasm.Text == "" || mtrasm.Text == "")
                    XtraMessageBox.Show("حقل فارغ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    //************************ AJOUTER & Modifier
                    if (typeprorasm == 0)
                    {
                        //----- Ajouter ---------//

                        DataRow row = DSROSOM.Tables[1].NewRow();

                        if (DSROSOM.Tables[1].Rows.Count <= 0)
                            row[0] = 1;
                        else
                            row[0] = int.Parse(DSROSOM.Tables[1].Rows[DSROSOM.Tables[1].Rows.Count - 1][0].ToString()) + 1;
                        if (radioButton1.Checked)
                        {
                            row[1] = 1;
                            row[4] = "ظاهر";
                        }
                        else
                        {
                            row[1] = 0;
                            row[4] = "مخفي";
                        }

                        row[2] = rasm.Text;
                        row[3] = mtrasm.Text;
                        row[5] = descrip.Text;

                        DSROSOM.Tables[1].Rows.Add(row);

                        gridView2.RefreshData();

                        typop = int.Parse(DSROSOM.Tables[1].Rows[DSROSOM.Tables[1].Rows.Count - 1][0].ToString());

                        XtraMessageBox.Show("تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mdrosom = 1;
                    }
                    else
                    {
                        //----- Modifier  ---------//
                        if (radioButton1.Checked)
                        {
                            DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][1] = 1;
                            DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][4] = "ظاهر";
                        }
                        else
                        {
                            DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][1] = 0;
                            DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][4] = "مخفي";
                        }
                        DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][2] = rasm.Text;
                        DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][3] = mtrasm.Text;
                        DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][5] = descrip.Text;

                        gridView2.RefreshData();
                        mdrosom = 1;
                        XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //************************
                }
            }
            else if (e.Button.Properties.Tag == "VI")
                viderrosom();
            else if (e.Button.Properties.Tag == "SU")
            {
                DialogResult DRes = XtraMessageBox.Show("هل تريد فعلا حذف هذا الرسم", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (DRes == DialogResult.Yes)
                {
                    if (typeprorasm != 0)
                    {
                        if (int.Parse(DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][0].ToString()) != 1 && int.Parse(DSROSOM.Tables[1].Rows[gridView2.FocusedRowHandle][0].ToString()) != 2)
                        {
                            DSROSOM.Tables[1].Rows[int.Parse(gridView2.RowCount.ToString()) - 1].Delete();

                            gridView2.RefreshData();
                            viderrosom();
                            typeprorasm = 0;
                            mdrosom = 1;

                            XtraMessageBox.Show("تم الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            XtraMessageBox.Show("لا يمكنك حذف هذا الرسم ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        XtraMessageBox.Show("لم تقم بإختيار أي رسم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult DRese = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DRese == DialogResult.Yes)
            {

                if (mdrosom == 0)
                    XtraMessageBox.Show("لا توجد تعديلات للإرسال", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {

                    splashScreenManager1.ShowWaitForm();

                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        if (lign1 == 1)
                        {
                            requeteinsert += ",";
                            requeteinsert2 += ",";
                        }
                        //  IdFrai,configYesNon,LibelleFrai,PrixUFrai,case when configYesNon=1 then 'ظاهر' else 'مخفي' end as sta,DescriptionFrai
                        requeteinsert += " (\\'" + gridView2.GetDataRow(i)[2].ToString() + "\\',\\'" + gridView2.GetDataRow(i)[5].ToString() + "\\'," + gridView2.GetDataRow(i)[3].ToString() + "," + gridView2.GetDataRow(i)[1].ToString() + "," + UserConnecte.IdUser + ")";
                        requeteinsert2 += " (\\'" + gridView2.GetDataRow(i)[2].ToString() + "\\',\\'" + gridView2.GetDataRow(i)[5].ToString() + "\\'," + gridView2.GetDataRow(i)[3].ToString() + "," + gridView2.GetDataRow(i)[1].ToString() + "," + (Configuration.LastID("fraiscopie", "IdCopie") + 1) + "," + (i + 1) + ")";

                        lign1 = 1;

                        MsgNv += "\r\n" + gridView2.GetDataRow(i)[3].ToString() + "     " + gridView2.GetDataRow(i)[5].ToString() + "     " + gridView2.GetDataRow(i)[2].ToString();

                        //MessageBox.Show(requeteinsert);
                    }
                    lign1 = 0;

                    if (DSROSOM.Tables[0].Rows.Count <= 0)
                    {
                        MsgAn = "";
                    }
                    else
                    {
                        for (int i = 0; i < DSROSOM.Tables[0].Rows.Count; i++)
                        {
                            MsgAn += "\r\n" + DSROSOM.Tables[0].Rows[i][3].ToString() + "     " + DSROSOM.Tables[0].Rows[i][5].ToString() + "     " + DSROSOM.Tables[0].Rows[i][2].ToString();
                        }
                    }

                    MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديلات على الرسوم  (" + DateTime.Now.ToString(formatdt) + ") :";
                    Requete = "TRUNCATE TABLE frais";
                    Requete2 = "insert into frais(LibelleFrai,DescriptionFrai,PrixUFrai,configYesNon,IdUser) values" + requeteinsert;
                    Requete3 = "insert into  fraiscopie(LibelleFraiC,DescriptionFraiC,PrixUFraiC,configYesNon,IdCopie,IdFrais) values" + requeteinsert2;

                    Configuration.Historique(0, Requete, MsgAn, MsgNv, MsgEn, Requete2, Requete3);

                    requeteinsert = "";
                    requeteinsert2 = "";
                    MsgNv = "";
                    MsgAn = "";
                    MsgEn = "";
                    Requete = "";
                    Requete2 = "";
                    Requete3 = "";

                    splashScreenManager1.CloseWaitForm();

                    XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }
        private void gridView2_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                int category = int.Parse(View.GetRowCellValue(e.RowHandle, View.Columns["configYesNon"]).ToString());
                if (category == 0)
                {
                    e.Appearance.BackColor = Color.Salmon;
                    e.Appearance.BackColor2 = Color.SeaShell;
                }
            }
        }
        private void gridView2_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            remplirrosom();
        }


        //////'''''''''''''''  Secteurs  ''''''''''''''''''/////
        int typmds = 0;
        private void tileItem3_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();

            foreach (TileItem item in tileGroup5.Items)
            {
                item.AppearanceItem.Normal.BackColor = slateBlue2;
            }
            tileItem3.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);


            rempgrid(gridView3, gridControl5, "الوصف", "الجولة", "select IdSect,LibelleSect,DescriptionSect,concat(NomUser,' ',PrenomUser) as nmcom from secteur,utilisateurs where secteur.IdUser=utilisateurs.IdUser");

            if (gridView3.SelectedRowsCount == 1)
                rempchamp(textEdit10, textBox1, gridView3);

            splashScreenManager1.CloseWaitForm();
        }
        private void windowsUIButtonPanel11_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "AJ")
                {
                    if (textEdit10.Text == "")
                        XtraMessageBox.Show("حقل فارغ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        //************************ AJOUTER & Modifier

                        if (typmds == 0)
                        {
                            //----- Ajouter

                            MsgAnc = "";
                            msgnv = "الجولة : " + textEdit10.Text + " | الوصف : " + textBox1.Text;
                            Mesent = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة جولة (" + DateTime.Now.ToString(formatdt) + ") :";
                            Requeteaj = "insert into secteur(LibelleSect,DescriptionSect,IdUser) values(\\'" + textEdit10.Text + "\\',\\'" + textBox1.Text + "\\'," + UserConnecte.IdUser + ")";

                            Configuration.Historique(1, Requeteaj, MsgAnc, msgnv, Mesent, "", "");

                            rempgrid(gridView3, gridControl5, "الوصف", "الجولة", "select IdSect,LibelleSect,DescriptionSect,concat(NomUser,' ',PrenomUser) as nmcom from secteur,utilisateurs where secteur.IdUser=utilisateurs.IdUser");

                            if (gridView3.SelectedRowsCount == 1)
                                rempchamp(textEdit10, textBox1, gridView3);

                            XtraMessageBox.Show("تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //----- Modifier

                            MsgAnc = "الجولة : " + gridView3.GetDataRow(gridView3.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridView3.GetDataRow(gridView3.FocusedRowHandle)[2].ToString();
                            msgnv = "الجولة : " + textEdit10.Text + " | الوصف : " + textBox1.Text;
                            Mesent = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل على جولة (" + DateTime.Now.ToString(formatdt) + ") :";
                            Requeteaj = "update secteur set LibelleSect=\\'" + textEdit10.Text + "\\',DescriptionSect=\\'" + textBox1.Text + "\\',IdUser=" + UserConnecte.IdUser + " where IdSect=" + typmds;

                            Configuration.Historique(0, Requeteaj, MsgAnc, msgnv, Mesent, "", "");
                            rempgrid(gridView3, gridControl5, "الوصف", "الجولة", "select IdSect,LibelleSect,DescriptionSect,concat(NomUser,' ',PrenomUser) as nmcom from secteur,utilisateurs where secteur.IdUser=utilisateurs.IdUser");

                            if (gridView3.SelectedRowsCount == 1)
                                rempchamp(textEdit10, textBox1, gridView3);

                            XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        //************************
                    }
                }
                else if (e.Button.Properties.Tag == "VI")
                {
                    viderchamps(textEdit10, textBox1);
                }
                else if (e.Button.Properties.Tag == "Supprimer")
                {
                    DialogResult DRes = XtraMessageBox.Show("هل تريد فعلا حذف هذه الجولة", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DRes == DialogResult.Yes)
                    {
                        if (typmds != 0)
                        {
                            if (Configuration.ExisteEnre("Compteur", "IdSect", typmds.ToString()) == 0)
                            {
                                string RequeteSupp = string.Format("Delete from Secteur WHERE IdSect={0} ", gridView3.GetDataRow(gridView3.FocusedRowHandle)[0].ToString() );
                                string NMsg = "الرقم الترتيبي : " + gridView3.GetDataRow(gridView3.FocusedRowHandle)[0].ToString() + " | الجولة : " + gridView3.GetDataRow(gridView3.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridView3.GetDataRow(gridView3.FocusedRowHandle)[2].ToString();
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف جولة   - " + DateTime.Now.ToString(formatdt);

                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");
                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else if (Configuration.Func(15) == "Direct")
                                    XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                rempgrid(gridView3, gridControl5, "الوصف", "الجولة", "select IdSect,LibelleSect,DescriptionSect,concat(NomUser,' ',PrenomUser) as nmcom from secteur,utilisateurs where secteur.IdUser=utilisateurs.IdUser");

                                if (gridView3.SelectedRowsCount == 1)
                                    rempchamp(textEdit10, textBox1, gridView3);
                            }
                            else
                                XtraMessageBox.Show("لا يمكنك حذف هذه الجولة ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            XtraMessageBox.Show("لم تقم بإختيار أي جولة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridView3_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            rempchamp(textEdit10, textBox1, gridView3);
        }


        //////'''''''''''''''  Type Entrée  ''''''''''''''''''/////
        private void tileItem4_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();


            foreach (TileItem item in tileGroup5.Items)
            {
                item.AppearanceItem.Normal.BackColor = slateBlue2;
            }
            tileItem4.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);

            rempgrid(gridView4, gridControl6, "الوصف", "أنواع المداخل", "select IdCatEntr,LibelleCatEntr,DescriptionCatEntr,concat(NomUser,' ',PrenomUser) as nmcom from categorieentres,utilisateurs where categorieentres.IdUser=utilisateurs.IdUser");

            if (gridView4.SelectedRowsCount == 1)
                rempchamp(textEdit9, textBox2, gridView4);

            splashScreenManager1.CloseWaitForm();
        }
        private void gridView4_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            rempchamp(textEdit9, textBox2, gridView4);
        }
        private void windowsUIButtonPanel12_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "AJ")
            {

                if (textEdit9.Text == "")
                    XtraMessageBox.Show("حقل فارغ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    //************************ AJOUTER & Modifier
                    if (typmds == 0)
                    {
                        //----- Ajouter

                        MsgAnc = "";
                        msgnv = "النوع : " + textEdit9.Text + " | الوصف : " + textBox1.Text;
                        Mesent = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة نوع من الخدمات (" + DateTime.Now.ToString(formatdt) + ") :";
                        Requeteaj = "insert into categorieentres(LibelleCatEntr,DescriptionCatEntr,IdUser) values(\\'" + textEdit9.Text + "\\',\\'" + textBox2.Text + "\\'," + UserConnecte.IdUser + ")";

                        Configuration.Historique(1, Requeteaj, MsgAnc, msgnv, Mesent, "", "");

                        rempgrid(gridView4, gridControl6, "الوصف", "أنواع المداخل", "select IdCatEntr,LibelleCatEntr,DescriptionCatEntr,concat(NomUser,' ',PrenomUser) as nmcom from categorieentres,utilisateurs where categorieentres.IdUser=utilisateurs.IdUser");

                        if (gridView4.SelectedRowsCount == 1)
                            rempchamp(textEdit9, textBox2, gridView4);

                        XtraMessageBox.Show("تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //----- Modifier

                        //----- Ajouter

                        MsgAnc = "النوع : " + gridView4.GetDataRow(gridView4.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridView4.GetDataRow(gridView4.FocusedRowHandle)[2].ToString();
                        msgnv = "النوع : " + textEdit9.Text + " | الوصف : " + textBox1.Text;
                        Mesent = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل على نوع المداخل (" + DateTime.Now.ToString(formatdt) + ") :";
                        Requeteaj = "update categorieentres set LibelleCatEntr=\\'" + textEdit9.Text + "\\',DescriptionCatEntr=\\'" + textBox2.Text + "\\',IdUser=" + UserConnecte.IdUser + " where IdCatEntr=" + typmds;

                        Configuration.Historique(0, Requeteaj, MsgAnc, msgnv, Mesent, "", "");

                        rempgrid(gridView4, gridControl6, "الوصف", "أنواع المداخل", "select IdCatEntr,LibelleCatEntr,DescriptionCatEntr,concat(NomUser,' ',PrenomUser) as nmcom from categorieentres,utilisateurs where categorieentres.IdUser=utilisateurs.IdUser");

                        if (gridView4.SelectedRowsCount == 1)
                            rempchamp(textEdit9, textBox2, gridView4);

                        XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //************************
                }
            }
            else if (e.Button.Properties.Tag == "VI")
            {
                viderchamps(textEdit9, textBox2);
            }
            else if (e.Button.Properties.Tag == "Supprimer")
            {
                //----- Supprimer
                DialogResult DRes = XtraMessageBox.Show("هل تريد فعلا الحذف", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (DRes == DialogResult.Yes)
                {
                    if (typmds != 0)
                    {
                        if (Configuration.ExisteEnre("Entres", "IdCatEntr", typmds.ToString()) == 0)
                        {
                            string RequeteSupp = string.Format("Delete from categorieentres WHERE IdCatEntr={0} ", gridView4.GetDataRow(gridView4.FocusedRowHandle)[0].ToString());
                            string NMsg = "الرقم الترتيبي : " + gridView4.GetDataRow(gridView4.FocusedRowHandle)[0].ToString() + " | نوع الخدمة : " + gridView4.GetDataRow(gridView4.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridView4.GetDataRow(gridView4.FocusedRowHandle)[2].ToString();
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف نوع الخدمات   - " + DateTime.Now.ToString(formatdt);

                            Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");
                            if (Configuration.Func(15) == "Indirect")
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else if (Configuration.Func(15) == "Direct")
                                XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            rempgrid(gridView4, gridControl6, "الوصف", "أنواع المداخل", "select IdCatEntr,LibelleCatEntr,DescriptionCatEntr,concat(NomUser,' ',PrenomUser) as nmcom from categorieentres,utilisateurs where categorieentres.IdUser=utilisateurs.IdUser");

                            if (gridView4.SelectedRowsCount == 1)
                                rempchamp(textEdit9, textBox2, gridView4);
                        }
                        else
                            XtraMessageBox.Show("لا يمكنك حذف هذا النوع  ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        XtraMessageBox.Show("لم تقم بالإختيار ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }



        //////''''''''''''''' Type Penalite '''''''''''''''''/////
        private void tileItem5_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();

            foreach (TileItem item in tileGroup5.Items)
            {
                item.AppearanceItem.Normal.BackColor = slateBlue2;
            }
            tileItem5.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);

            rempgrid(gridView5, gridControl7, "الوصف", "أنواع المخالفات", "select IdTypePena,LibelleTypePena,	DescriptionTypePena,concat(NomUser,' ',PrenomUser) as nmcom from typepenalite,utilisateurs where typepenalite.IdUser=utilisateurs.IdUser");

            if (gridView5.SelectedRowsCount == 1)
            {
                rempchamp(textEdit11, textBox3, gridView5);
            }

            splashScreenManager1.CloseWaitForm();
        }
        private void gridView5_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            rempchamp(textEdit11, textBox3, gridView5);
            typmdsViolations = int.Parse(gridView5.GetDataRow(gridView5.FocusedRowHandle)[0].ToString());
        }
        int typmdsViolations = 0;
        private void windowsUIButtonPanel13_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "AJ")
            {
                if (textEdit11.Text == "")
                    XtraMessageBox.Show("حقل فارغ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    //************************ AJOUTER & Modifier
                    if (typmdsViolations == 0)
                    {
                        //----- Ajouter

                        MsgAnc = "";
                        msgnv = "النوع : " + textEdit11.Text + " | الوصف : " + textBox3.Text;
                        Mesent = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة نوع من المخالفات (" + DateTime.Now.ToString(formatdt) + ") :";
                        Requeteaj = "insert into typepenalite(LibelleTypePena,DescriptionTypePena,IdUser) values(\\'" + textEdit11.Text + "\\',\\'" + textBox3.Text + "\\'," + UserConnecte.IdUser + ")";
                        Configuration.Historique(1, Requeteaj, MsgAnc, msgnv, Mesent, "", "");

                        rempgrid(gridView5, gridControl7, "الوصف", "أنواع المخالفات", "select IdTypePena,LibelleTypePena,	DescriptionTypePena,concat(NomUser,' ',PrenomUser) as nmcom from typepenalite,utilisateurs where typepenalite.IdUser=utilisateurs.IdUser");

                        if (gridView5.SelectedRowsCount == 1)
                        {
                            rempchamp(textEdit11, textBox3, gridView5);
                        }

                        XtraMessageBox.Show("تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);                      
                    }
                    else
                    {
                        //----- Modifier

                        MsgAnc = "النوع : " + gridView5.GetDataRow(gridView5.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridView5.GetDataRow(gridView5.FocusedRowHandle)[2].ToString();
                        msgnv = "النوع : " + textEdit11.Text + " | الوصف : " + textBox3.Text;
                        Mesent = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل على نوع المخالفات (" + DateTime.Now.ToString(formatdt) + ") :";
                        Requeteaj = "update typepenalite set LibelleTypePena=\\'" + textEdit11.Text + "\\',DescriptionTypePena=\\'" + textBox3.Text + "\\',IdUser=" + UserConnecte.IdUser + " where IdTypePena=" + typmdsViolations;
                        Configuration.Historique(0, Requeteaj, MsgAnc, msgnv, Mesent, "", "");

                        rempgrid(gridView4, gridControl6, "الوصف", "أنواع المخالفات", "select IdCatEntr,LibelleCatEntr,DescriptionCatEntr,concat(NomUser,' ',PrenomUser) as nmcom from categorieentres,utilisateurs where categorieentres.IdUser=utilisateurs.IdUser");

                        if (gridView5.SelectedRowsCount == 1)
                            rempchamp(textEdit11, textBox3, gridView5);

                        XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //************************
                }
            }
            else if (e.Button.Properties.Tag == "VI")
            {
                viderchamps(textEdit11, textBox3);
            }
            else if (e.Button.Properties.Tag == "Supprimer")
            {
                //----- Supprimer
                DialogResult DRes = XtraMessageBox.Show("هل تريد فعلا الحذف", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (DRes == DialogResult.Yes)
                {
                    if (typmdsViolations != 0)
                    {
                        if (Configuration.ExisteEnre("penalite", "IdTypePena", typmdsViolations.ToString()) == 0)
                        {
                            string RequeteSupp = string.Format("Delete from typepenalite WHERE IdTypePena={0} ", gridView5.GetDataRow(gridView5.FocusedRowHandle)[0].ToString());
                            string NMsg = "الرقم الترتيبي : " + gridView5.GetDataRow(gridView5.FocusedRowHandle)[0].ToString() + " | نوع الغرامة : " + gridView5.GetDataRow(gridView5.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridView5.GetDataRow(gridView5.FocusedRowHandle)[2].ToString();
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف نوع الغرامات   - " + DateTime.Now.ToString(formatdt);

                            Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");
                            if (Configuration.Func(15) == "Indirect")
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else if (Configuration.Func(15) == "Direct")
                                XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            rempgrid(gridView5, gridControl7, "الوصف", "أنواع المخالفات", "select IdTypePena,LibelleTypePena,	DescriptionTypePena,concat(NomUser,' ',PrenomUser) as nmcom from typepenalite,utilisateurs where typepenalite.IdUser=utilisateurs.IdUser");
                            if (gridView5.SelectedRowsCount == 1)
                                rempchamp(textEdit11, textBox3, gridView5);
                        }
                        else
                            XtraMessageBox.Show("لا يمكنك حذف هذا النوع ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        XtraMessageBox.Show("لم تقم بالإختيار ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }


        //////''''''''''''''' Config App''''''''''''''''''/////

        ///// Load
        private void tileItemadherent_ItemClick(object sender, TileItemEventArgs e)
        {
            foreach (TileItem item in tileGroup5.Items)
            {
                item.AppearanceItem.Normal.BackColor = slateBlue2;
            }
            tileItemadherent.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);

            // ********************

            Penali_Mo = Configuration.Func(23);
            Eclairage = Configuration.Func(20);
            tranchesmethode = Configuration.Func(14);
            permission = Configuration.Func(15);

            Penali_MoYN = Penali_Mo;
            EclairageNV = Eclairage;
            tranchesmethodenv = tranchesmethode;
            permissionnv = permission;

            // Penali_Mo
            if (Penali_Mo == "0")
            {
                RBAut.Checked = true;
                ApplierPena.Enabled = false;
            }
            else
            {
                RBManu.Checked = true;
                ApplierPena.Enabled = true;
            }

            // Eclairage 
            if (Eclairage == "1")
            {
                radioButton8.Checked = true;
            }
            else
            {
                radioButton7.Checked = true;
            }

            // طريقة حساب الأشطر
            if (tranchesmethode == "Fort")
            {
                radioButton4.Checked = true;
            }
            else
            {
                radioButton3.Checked = true;
            }

            // طريقة عمل البرنامج
            if (permission == "Indirect")
            {
                radioButton5.Checked = true;
            }
            else
            {
                radioButton6.Checked = true;
            }

            //******************
        }

        //// Node
        private void textEdit5_EditValueChanged(object sender, EventArgs e)
        {
            label10.Text = textEdit5.Text + "125";
        }
        private void windowsUIButtonPanel3_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (act != textEdit5.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        enr(13, textEdit5.Text, "العقدة");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEdit5.Text = act;
            }
        }

        //// Penalitée Auto/Manu
        private void RBAut_CheckedChanged(object sender, EventArgs e)
        {
            if (RBAut.Checked)
            {
                ApplierPena.Enabled = false;
                AplliqPenaLastMonth = "";
                Penali_MoYN = "0";
            }
            else
            {
                ApplierPena.Enabled = true;
                Penali_MoYN = "1";
            }
        }
        private void RBManu_CheckedChanged(object sender, EventArgs e)
        {
            if (RBManu.Checked)
            {
                ApplierPena.Enabled = true;
                Penali_MoYN = "1";
            }
            else
            {
                ApplierPena.Enabled = false;
                Penali_MoYN = "0";
            }
        }
        private void windowsUIButtonPanelAplliqPena_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (Penali_Mo != Penali_MoYN)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        if (RBManu.Checked && AplliqPenaLastMonth=="Yes")
                        {
                            //// Set Tax
                            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                ClassConnexion.Macon.Open();

                            MySqlCommand CmdSetPenalite = new MySqlCommand("update paiement set PenalitePaie=(select LibEntr from configuration where IdConf=7) where PayePaie=0 and IdFact in (select max(IdFact) from facture where DATEDIFF(DATE_FORMAT(now(),\"%Y-%m-%d\"),DATE_FORMAT(PeriodePaieFFact,\"%Y-%m-%d\"))>1); ", ClassConnexion.Macon);
                            //MySqlCommand CmdSetPenalite = new MySqlCommand("update paiement set PenalitePaie=(select LibEntr from configuration where IdConf=7) where PayePaie=0 and IdFact in (select Max(IdFact)-1 from facture );", ClassConnexion.Macon);
                            CmdSetPenalite.ExecuteNonQuery();
                            ClassConnexion.Macon.Close();
                        }
                        enr(23, Penali_MoYN, "طريقة تطبيق الدعيرة");

                        XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "Annuler")
            {
                if (Penali_Mo == "1")
                {                   
                    RBManu.Checked = true;
                }
                else
                {
                    RBAut.Checked = true;
                }
            }
        }
        private void ApplierPena_Click(object sender, EventArgs e)
        {
            AplliqPenaLastMonth = "Yes";
        }


        //// طريقة حساب الأشطر
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                tranchesmethodenv="Facil";
            }
            else
            {
                tranchesmethodenv = "Fort";
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                tranchesmethodenv = "Fort";
            }
            else
            {
                tranchesmethodenv = "Facil";
            }
        }
        private void windowsUIButtonPanel14_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (tranchesmethode != tranchesmethodenv)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        enr(14, tranchesmethodenv, "طريقة حساب الأشطر");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                if (tranchesmethode == "Fort")
                {
                    radioButton4.Checked = true;
                }
                else
                {
                    radioButton3.Checked = true;
                }
            }
        }


        ////  طريقة عمل البرنامج Direct/Indirect 
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                permissionnv = "Direct";
            }
            else
            {
                permissionnv = "Indirect";
            }
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                permissionnv = "Indirect";
            }
            else
            {
                permissionnv = "Direct";
            }
        }
        private void windowsUIButtonPanel15_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (permission != permissionnv)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        enr(15, permissionnv, "طريقة عمل البرنامج");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                if (permission == "Indirect")
                {
                    radioButton5.Checked = true;
                }
                else
                {
                    radioButton6.Checked = true;
                }
            }
        }


        //// Utilisation de l'Eclairage 
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                EclairageNV = "1";
            }
            else
            {
                EclairageNV = "0";
            }
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                EclairageNV = "1";
            }
            else
            {
                EclairageNV = "0";
            }
        }
        private void windowsUIButtonPanel16_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (Eclairage != EclairageNV)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        enr(20, EclairageNV, "الكهرباء");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                if (Eclairage == "1")
                {
                    radioButton8.Checked = true;
                }
                else
                {
                    radioButton7.Checked = true;
                }
            }
        }

        ////   مدة إدخال المعطيات
        private void windowsUIButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (datentrinfo != textEdit4.Text && nbrjourinfo != textEdit3.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes)
                    {
                        enr(5, textEdit4.Text, "تاريخ بداية إدخال المعطيات");
                        enr(6, textEdit3.Text, "مدة إدخال المعطيات");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (datentrinfo != textEdit4.Text)
                {
                    enr(5, textEdit4.Text, "تاريخ بداية إدخال المعطيات");
                    XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (nbrjourinfo != textEdit3.Text)
                {
                    enr(6, textEdit3.Text, "مدة إدخال المعطيات");
                    XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (e.Button.Properties.Tag == "anuller")
            {
                textEdit3.Text = nbrjourinfo;
                textEdit4.Text = datentrinfo;
            }
        }

        ////    مدة فترة التحصيل
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                if (periodepy != textEditperiodepy.Text && jourPy != textEditnbrjourpy.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DR == DialogResult.Yes)
                    {
                        enr(3, textEditperiodepy.Text, "تاريخ بداية فترة التحصيل");
                        enr(4, textEditnbrjourpy.Text, "مدة فترة التحصيل");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (periodepy != textEditperiodepy.Text)
                {
                    enr(3, textEditperiodepy.Text, "تاريخ بداية فترة التحصيل");
                    XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (jourPy != textEditnbrjourpy.Text)
                {
                    enr(4, textEditnbrjourpy.Text, "مدة فترة التحصيل");
                    XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEditperiodepy.Text = periodepy;
                textEditnbrjourpy.Text = jourPy;
            }
        }

        ////  windowsUIButtonPanel   مبلغ غرامة التأخ
        private void windowsUIButtonPanel2_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {

                if (Mtpenalite != textEdit6.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DR == DialogResult.Yes)
                    {
                        enr(7, textEdit6.Text, "مبلغ غرامة التأخر");

                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEdit6.Text = Mtpenalite;
            }
        }



        ////////'''''''''''''' Page Informations association  ''''''''''''''''''/////

        ///// Load
        private void tileItemcompteur_ItemClick(object sender, TileItemEventArgs e)
        {
            foreach (TileItem item in tileGroup5.Items)
            {
                item.AppearanceItem.Normal.BackColor = slateBlue2;
            }
            tileItemcompteur.AppearanceItem.Normal.BackColor = slateBlue;

            navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
        }
        //// إسم الجمعية
        private void windowsUIButtonPanel4_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                
                if (nomassoc != textEdit1.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DR == DialogResult.Yes)
                    {
                        enr(1, textEdit1.Text, "إسم الجمعية");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEdit1.Text = nomassoc;

            }
        }
        //// البريد الإلكتروني
        private void windowsUIButtonPanel5_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                
                if (email != textEdit2.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DR == DialogResult.Yes)
                    {
                        enr(9, textEdit2.Text, "البريد الإلكتروني");

                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEdit2.Text = email;

            }
        }
        //// عنوان الجمعية
        private void windowsUIButtonPanel6_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {

                if (addres != textEdit7.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DR == DialogResult.Yes)
                    {
                        enr(10, textEdit7.Text, "عنوان الجمعية");

                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }



            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEdit7.Text = addres;

            }
        }

        //// هاتف الجمعية
        private void windowsUIButtonPanel7_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {

                if (Tel != textEdit8.Text)
                {
                    DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DR == DialogResult.Yes)
                    {
                        enr(11, textEdit8.Text, "هاتف الجمعية");
                        XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                textEdit8.Text = Tel;
            }
        }

        ///// شعار الجمعية
        private void windowsUIButtonPanel8_ButtonClick_1(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag == "Modifier")
            {
                // MessageBox.Show("URL IMG : "+ imgurl+" \r\n Img pictur : "+ pictureBox1.ImageLocation);

                try
                {
                    if (imgurl != pictureBox1.ImageLocation)
                    {
                        Image fil;

                        Random aleatoire = new Random();
                        string nom1 = aleatoire.Next(1, 2500).ToString();
                        string nom2 = aleatoire.Next(1, 2500).ToString();
                        string nom3 = aleatoire.Next(1, 2500).ToString();
                        string nom4 = aleatoire.Next(1, 2500).ToString();
                        string nomimg = nom1 + nom2 + nom3 + nom4 + ".png";
                        //string Lien = wanted_path + "\\Resources\\full logo" + nomimg;

                        string Lien = wanted_path + "\\Resources\\full logo";


                        SaveFileDialog svdlg = new SaveFileDialog();
                        fil = Image.FromFile(pictureBox1.ImageLocation);

                        File.Copy(pictureBox1.ImageLocation, Lien);

                        DialogResult DR = XtraMessageBox.Show("هل تريد فعلا إرسال هذا التعديل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (DR == DialogResult.Yes)
                        {
                            enr(8, "full logo" + nomimg, "شعار الجمعية");

                            XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag == "annuler")
            {
                pictureBox1.ImageLocation = imgurl;
            }
        }
        //// Uploade logo
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //*******************************

            OpenFileDialog fichierxml = new OpenFileDialog();
            if (fichierxml.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = fichierxml.FileName;
            }
            //******************************
        }



        ////////'''''''''''''' Page Entres & Sorties  ''''''''''''''''''/////

        DataSet ds;
        MySqlDataReader dr;
        MySqlDataAdapter da;
        string formatt = "yyyy-MM-dd HH:mm:ss";

        ///// Load

        Boolean TestdejaOvrCatEntr, TestdejaOvrCatSort = false;
        Boolean TesteIfAjouOrMod, TesteIfAjouOrModCatSort = false; // True == Ajou & False = Modif

        int IndexIdCatEntr, IndexIdCatSort = 0;


        string AMsg, AMsgCS = "";
        int RadioVisibUpd, RadioVisibUpdSort = 1;
        string RadioStrVisibUpd, RadioStrVisibUpdSort = "";

        ///// Void
        private int TestSuppCatEntrOrSort(string strReq)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            MySqlCommand Requetext = new MySqlCommand(strReq, ClassConnexion.Macon);
            dr = Requetext.ExecuteReader();
            dr.Read();
            int resultat = int.Parse(dr[0].ToString());
            dr.Close();
            ClassConnexion.Macon.Close();
            return resultat;
        }
        public void RempInterTextB()
        {
            textEditCatEntr.Text = gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[1].ToString();
            textBoxDescCatEntr.Text = gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[2].ToString();
            if (gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[4].ToString()== "1")
            {
                radioButtonYesCE.Checked = true;
                radioButtonNoCE.Checked = false;
            }
            else
            {
                radioButtonYesCE.Checked = false;
                radioButtonNoCE.Checked = true;
            }
        }
        public void RempDataCateEntr()
        {
            if (TestdejaOvrCatEntr== true)
            {
                ds.Tables["CatEntres"].Clear();
            }
            da = new MySqlDataAdapter("select IdCatEntr,LibCatEntr,DescCatEntr, case when VisibCatEntr= 1 then 'ضاهر' else 'مخفي' end as VisibCatEntr2,VisibCatEntr from catentres", ClassConnexion.Macon);
            da.Fill(ds, "CatEntres");
            TestdejaOvrCatEntr = true;
        }
        public void RempGridCatEntr()
        {
            gridControlCatEntr.DataSource = ds.Tables["CatEntres"];
            gridControlCatEntr.Refresh();

            gridViewCatEntr.Columns[0].Visible = false;
            gridViewCatEntr.Columns["VisibCatEntr"].Visible = false;

            gridViewCatEntr.Columns[1].Caption = "نوع المدخول";
            gridViewCatEntr.Columns[2].Caption = "الوصف";
            gridViewCatEntr.Columns[3].Caption = "الحالة";

        }
        //------------------------------//
        public void RempInterTextBCatSort()
        {
            textEditCatSort.Text = gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[1].ToString();
            textBoxDescCatSort.Text = gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[2].ToString();
            if (gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[4].ToString() == "1")
            {
                radioButtonYesCS.Checked = true;
                radioButtonNoCS.Checked = false;
            }
            else
            {
                radioButtonYesCS.Checked = false;
                radioButtonNoCS.Checked = true;
            }
        }
        public void RempDataCateSort()
        {
            if (TestdejaOvrCatSort == true)
            {
                ds.Tables["CatSorties"].Clear();
            }
            da = new MySqlDataAdapter("select IdCatSort,LibCatSort,DescCatSort, case when VisibCatSort= 1 then 'ضاهر' else 'مخفي' end as VisibCatSort2,VisibCatSort from catsorties", ClassConnexion.Macon);
            da.Fill(ds, "catsorties");
            TestdejaOvrCatSort = true;
        }
        public void RempGridCatSort()
        {
            gridControlCatSort.DataSource = ds.Tables["CatSorties"];
            gridControlCatSort.Refresh();

            gridViewCatSort.Columns[0].Visible = false;
            gridViewCatSort.Columns["VisibCatSort"].Visible = false;

            gridViewCatSort.Columns[1].Caption = "نوع المصروف";
            gridViewCatSort.Columns[2].Caption = "الوصف";
            gridViewCatSort.Columns[3].Caption = "الحالة";

        }

        ///// Load
        private void tileItemEntrandSort_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();

                ds = new DataSet();

                foreach (TileItem item in tileGroup5.Items)
                {
                    item.AppearanceItem.Normal.BackColor = slateBlue2;
                }
                tileItemEntrandSort.AppearanceItem.Normal.BackColor = slateBlue;

                /// CatEntres
                TestdejaOvrCatEntr = false;
                RempDataCateEntr();
                if (ds.Tables["CatEntres"].Rows.Count >0)
                {
                    RempGridCatEntr();
                    RempInterTextB();
                }

                /// CatSorties
                TestdejaOvrCatSort = false;
                RempDataCateSort();
                if (ds.Tables["CatSorties"].Rows.Count > 0)
                {
                    RempGridCatSort();
                    RempInterTextBCatSort();
                }

                navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                splashScreenManager1.CloseWaitForm();
            }
        }

        ////*** Code CatEntres
        private void windowsUIButtonPanelCatEntr_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Save")
                {
                    if (textEditCatEntr.Text == "")
                    {
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (TesteIfAjouOrMod == true)
                    {
                        ////******* Ajou

                        int RadioVisib=1;
                        string RadioStrVisib = "";

                        if (radioButtonYesCE.Checked== true)
                        {
                            RadioVisib = 1;
                            RadioStrVisib = "ضاهر";
                        }
                        else
                        {
                            RadioVisib = 0;
                            RadioStrVisib = "مخفي";
                        }

                        string RequeteModi = string.Format("insert into catentres(LibCatEntr,DescCatEntr,VisibCatEntr) Values( \\'{0}\\',\\'{1}\\',\\'{2}\\' ) ", textEditCatEntr.Text, textBoxDescCatEntr.Text, RadioVisib);
                        string NMsg = " نوع المدخول : " + textEditCatEntr.Text + " | الوصف : " + textBoxDescCatEntr.Text + " | الحالة : " + RadioStrVisib + " ";
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة نوع المدخول   - " + DateTime.Now.ToString(formatt);

                        Configuration.Historique(1, RequeteModi, "", NMsg, MEnt, "", "");

                        if (Configuration.Func(15) == "Indirect")
                        {
                            XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (Configuration.Func(15) == "Direct")
                        {
                            XtraMessageBox.Show("تمة الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        RempDataCateEntr();
                        RempGridCatEntr();
                        RempInterTextB();
                    }
                    else
                    {
                        ////******* Update

                        if (IndexIdCatEntr >= 0)
                        {
                            if (radioButtonYesCE.Checked == true)
                            {
                                RadioVisibUpd = 1;
                                RadioStrVisibUpd = "ضاهر";
                            }
                            else
                            {
                                RadioVisibUpd = 0;
                                RadioStrVisibUpd = "مخفي";
                            }

                            string RequeteModi = string.Format("UPDATE catentres SET LibCatEntr=\\'{0}\\',DescCatEntr=\\'{1}\\',VisibCatEntr=\\'{2}\\' WHERE IdCatEntr=" + IndexIdCatEntr + " ", textEditCatEntr.Text, textBoxDescCatEntr.Text, RadioVisibUpd);

                            string NMsg = " نوع المدخول : " + textEditCatEntr.Text + " | الوصف : " + textBoxDescCatEntr.Text + " | الحالة : " + RadioStrVisibUpd + " ";
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل في نوع المدخول   - " + DateTime.Now.ToString(formatt);
                            //MessageBox.Show("RequeteModi ="+RequeteModi.ToString());
                            Configuration.Historique(0, RequeteModi, AMsg, NMsg, MEnt, "", "");

                            if (Configuration.Func(15) == "Indirect")
                            {
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (Configuration.Func(15) == "Direct")
                            {
                                XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            RempDataCateEntr();
                            RempGridCatEntr();
                            RempInterTextB();
                        }
                        else
                        {
                            XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                        }
                    }

                }
                else if (e.Button.Properties.Tag == "Clear")
                {
                    textEditCatEntr.Text = "";
                    textBoxDescCatEntr.Text = "";
                    IndexIdCatEntr = -1;

                    TesteIfAjouOrMod = true;
                }
                else if (e.Button.Properties.Tag == "Delete")
                {
                    ////******* Delete

                    if (gridViewCatEntr.SelectedRowsCount == 1)
                    {
                        DialogResult dr = XtraMessageBox.Show("هل تريد فعلا حذف هذا النوع", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            if (TestSuppCatEntrOrSort("select count(*) from entrescais where CatEntres_IdCatEntr ="+IndexIdCatEntr+" ") <= 0)
                            {
                                string RequeteSupp = string.Format("delete from catentres WHERE IdCatEntr={0} ", IndexIdCatEntr);
                                string NMsg = "الرقم الترتيبي : " + gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[0].ToString() + " | نوع المدخول : " + gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[2].ToString() + " | الحالة : " + gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[3].ToString() ;
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف نوع المدخول   - " + DateTime.Now.ToString(formatt);

                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                {
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (Configuration.Func(15) == "Direct")
                                {
                                    XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                RempDataCateEntr();
                                RempGridCatEntr();
                                RempInterTextB();
                            }
                            else
                            {
                                XtraMessageBox.Show("لايمكن حدف هذا النوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }                         
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("يجب تحديد السطر المراد حدفه");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }      
        private void gridViewCatEntr_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                
                IndexIdCatEntr = int.Parse(gridViewCatEntr.GetDataRow(gridViewCatEntr.FocusedRowHandle)[0].ToString());
                RempInterTextB();
                TesteIfAjouOrMod = false;

                if (radioButtonYesCE.Checked == true)
                {
                    RadioVisibUpd = 1;
                    RadioStrVisibUpd = "ضاهر";
                }
                else
                {
                    RadioVisibUpd = 0;
                    RadioStrVisibUpd = "مخفي";
                }
                AMsg = " نوع المدخول : " + textEditCatEntr.Text + " | الوصف : " + textBoxDescCatEntr.Text + " | الحالة : " + RadioStrVisibUpd + " ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /////*** Code CateSorties
        private void windowsUIButtonPanelCatSort_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Save")
                {
                    if (textEditCatSort.Text == "")
                    {
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (TesteIfAjouOrModCatSort == true)
                    {
                        ////******* Ajou

                        int RadioVisibSort = 1;
                        string RadioStrVisibSort = "";

                        if (radioButtonYesCS.Checked == true)
                        {
                            RadioVisibSort = 1;
                            RadioStrVisibSort = "ضاهر";
                        }
                        else
                        {
                            RadioVisibSort = 0;
                            RadioStrVisibSort = "مخفي";
                        }

                        string RequeteModi = string.Format("insert into catsorties(LibCatSort,DescCatSort,VisibCatSort) Values( \\'{0}\\',\\'{1}\\',\\'{2}\\' ) ", textEditCatSort.Text, textBoxDescCatSort.Text, RadioVisibSort);
                        string NMsg = " نوع المدخول : " + textEditCatSort.Text + " | الوصف : " + textBoxDescCatSort.Text + " | الحالة : " + RadioStrVisibSort + " ";
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة نوع المدخول   - " + DateTime.Now.ToString(formatt);

                        Configuration.Historique(1, RequeteModi, "", NMsg, MEnt, "", "");

                        if (Configuration.Func(15) == "Indirect")
                        {
                            XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (Configuration.Func(15) == "Direct")
                        {
                            XtraMessageBox.Show("تمة الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        RempDataCateSort();
                        RempGridCatSort();
                        RempInterTextBCatSort();
                    }
                    else
                    {
                        ////******* Update

                        if (IndexIdCatSort >= 0)
                        {
                            if (radioButtonYesCS.Checked == true)
                            {
                                RadioVisibUpdSort = 1;
                                RadioStrVisibUpdSort = "ضاهر";
                            }
                            else
                            {
                                RadioVisibUpdSort = 0;
                                RadioStrVisibUpdSort = "مخفي";
                            }

                            string RequeteModi = string.Format("UPDATE catsorties SET LibCatSort=\\'{0}\\',DescCatSort=\\'{1}\\',VisibCatSort=\\'{2}\\' WHERE IdCatSort=" + IndexIdCatSort + " ", textEditCatSort.Text, textBoxDescCatSort.Text, RadioVisibUpdSort);

                            string NMsg = " نوع المصروف : " + textEditCatSort.Text + " | الوصف : " + textBoxDescCatSort.Text + " | الحالة : " + RadioStrVisibUpdSort + " ";
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل في نوع المصروف   - " + DateTime.Now.ToString(formatt);
                            Configuration.Historique(0, RequeteModi, AMsg, AMsgCS, MEnt, "", "");

                            if (Configuration.Func(15) == "Indirect")
                            {
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (Configuration.Func(15) == "Direct")
                            {
                                XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            RempDataCateSort();
                            RempGridCatSort();
                            RempInterTextBCatSort();
                        }
                        else
                        {
                            XtraMessageBox.Show("يجب تحديد السطر المراد تعديله");
                        }
                    }

                }
                else if (e.Button.Properties.Tag == "Clear")
                {
                    ////******* Clear

                    textEditCatSort.Text = "";
                    textBoxDescCatSort.Text = "";
                    IndexIdCatSort = -1;

                    TesteIfAjouOrModCatSort = true;
                }
                else if (e.Button.Properties.Tag == "Delete")
                {
                    ////******* Delete

                    if (gridViewCatSort.SelectedRowsCount == 1)
                    {
                        DialogResult dr = XtraMessageBox.Show("هل تريد فعلا حذف هذا النوع", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            if (TestSuppCatEntrOrSort("select count(*) from sortiescais where CatSorties_IdCatSort =" + IndexIdCatSort + " ") <= 0)
                            {
                                string RequeteSupp = string.Format("delete from catsorties WHERE IdCatSort={0} ", IndexIdCatSort);
                                string NMsg = "الرقم الترتيبي : " + gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[0].ToString() + " | نوع المصروف : " + gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[1].ToString() + " | الوصف : " + gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[2].ToString() + " | الحالة : " + gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[3].ToString();
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف نوع المدخول   - " + DateTime.Now.ToString(formatt);

                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                {
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (Configuration.Func(15) == "Direct")
                                {
                                    XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                RempDataCateSort();
                                RempGridCatSort();
                                RempInterTextBCatSort();
                            }
                            else
                            {
                                XtraMessageBox.Show("لايمكن حدف هذا النوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("يجب تحديد السطر المراد حدفه");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridViewCatSort_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {

                IndexIdCatSort = int.Parse(gridViewCatSort.GetDataRow(gridViewCatSort.FocusedRowHandle)[0].ToString());
                RempInterTextBCatSort();
                TesteIfAjouOrModCatSort = false;

                if (radioButtonYesCS.Checked == true)
                {
                    RadioVisibUpdSort = 1;
                    RadioStrVisibUpdSort = "ضاهر";
                }
                else
                {
                    RadioVisibUpdSort = 0;
                    RadioStrVisibUpdSort = "مخفي";
                }
                AMsgCS = " نوع المدخول : " + textEditCatSort.Text + " | الوصف : " + textBoxDescCatSort.Text + " | الحالة : " + RadioStrVisibUpdSort + " ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        ////////'''''''''''''' Page Categ Caiss  ''''''''''''''''''/////


        Boolean TestdejaOvrCatCais,TestAjouOrModi = false;
        int IndexCurrantR = 0;
        DataSet dsCaisse;

        ///// Void
        private void RempDataCatCais()
        {
            if (TestdejaOvrCatCais == true)
            {
                dsCaisse.Tables["Caisse"].Clear();
            }
            da = new MySqlDataAdapter("select IdCais,LibCais,RefCais,DescCais,caisse.IdUser,concat(NomUser,' ',PrenomUser) as NomComp from caisse,utilisateurs where caisse.IdUser = utilisateurs.IdUser", ClassConnexion.Macon);
            da.Fill(dsCaisse, "Caisse");
            TestdejaOvrCatCais = true;
        }
        private void RempGridCatCais()
        {
            gridControlCatCaisse.DataSource = dsCaisse.Tables["Caisse"];
            gridControlCatCaisse.Refresh();
            if (gridViewCatCaisse.RowCount > 0)
            {
                IndexCurrantR = int.Parse(gridViewCatCaisse.GetDataRow(gridViewCatCaisse.FocusedRowHandle)[0].ToString());
            }
        }
        private void RempTextBCatCais()
        {
            TestAjouOrModi = true;
            textEditLibCais.Text = gridViewCatCaisse.GetDataRow(gridViewCatCaisse.FocusedRowHandle)[1].ToString();
            textEditRefCais.Text = gridViewCatCaisse.GetDataRow(gridViewCatCaisse.FocusedRowHandle)[2].ToString();
            textBoxDescCais.Text = gridViewCatCaisse.GetDataRow(gridViewCatCaisse.FocusedRowHandle)[3].ToString();
        }
        private void Clear()
        {
            textEditLibCais.Text = "";
            textEditRefCais.Text = "";
            textBoxDescCais.Text = "";
        }

        ///// Load
        private void tileItemCateCais_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                dsCaisse = new DataSet();

                foreach (TileItem item in tileGroup5.Items)
                {
                    item.AppearanceItem.Normal.BackColor = slateBlue2;
                }               
                tileItemCateCais.AppearanceItem.Normal.BackColor = slateBlue;


                TestdejaOvrCatCais = false;
                RempDataCatCais();
                if (dsCaisse.Tables["Caisse"].Rows.Count > 0)
                {
                    RempGridCatCais();
                    RempTextBCatCais();
                }

                navigationFrame1.SelectedPageIndex = tileGroup5.Items.IndexOf(e.Item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                splashScreenManager1.CloseWaitForm();
            }
        }

        //// Code
        private void gridViewCatCaisse_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                //TestAjouOrModi = true;
                IndexCurrantR = int.Parse(gridViewCatCaisse.GetDataRow(gridViewCatCaisse.FocusedRowHandle)[0].ToString());
                RempTextBCatCais();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowsUIButtonPanelCatCais_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Enregistrer")
                {
                    if (textEditLibCais.Text == "")
                    {
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (TestAjouOrModi == false)
                    {
                        //// Ajou

                        string RequeteAjou = string.Format("insert into caisse(LibCais,RefCais,DescCais,IdUser) Values( \\'{0}\\',\\'{1}\\',\\'{2}\\',{3} ) ", textEditLibCais.Text, textEditRefCais.Text, textBoxDescCais.Text,UserConnecte.IdUser);
                        string NMsg = " إسم الحساب : " + textEditLibCais.Text + " | الرقم : " + textEditRefCais.Text + " | الوصف : " + textBoxDescCais.Text + " ";
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة حساب جديد   - " + DateTime.Now.ToString(formatt);

                        Configuration.Historique(1, RequeteAjou, "", NMsg, MEnt, "", "");

                        if (Configuration.Func(15) == "Indirect")
                        {
                            XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (Configuration.Func(15) == "Direct")
                        {
                            XtraMessageBox.Show("تمة الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        RempDataCatCais();
                        RempGridCatCais();
                        RempTextBCatCais();
                    }
                    else
                    {
                        //// Modif

                        if (textEditLibCais.Text == "")
                        {
                            XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (IndexCurrantR >= 0)
                        {
                            string RequeteModi = string.Format("UPDATE caisse SET LibCais=\\'{0}\\',RefCais=\\'{1}\\',DescCais=\\'{2}\\' WHERE IdCais=" + IndexCurrantR + " ", textEditLibCais.Text, textEditRefCais.Text, textBoxDescCais.Text);

                            string NMsg = " إسم الحساب : " + textEditLibCais.Text + " | الرقم : " + textEditRefCais.Text + " | الوصف : " + textBoxDescCais + " ";
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل في معلومات الحساب   - " + DateTime.Now.ToString(formatt);
                            Configuration.Historique(0, RequeteModi, AMsg, AMsgCS, MEnt, "", "");

                            if (Configuration.Func(15) == "Indirect")
                            {
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (Configuration.Func(15) == "Direct")
                            {
                                XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            RempDataCatCais();
                            RempGridCatCais();
                            RempTextBCatCais();
                        }
                    }
                }
                else if (e.Button.Properties.Tag == "Vider")
                {
                    TestAjouOrModi = false;
                    IndexCurrantR = -1;
                    Clear();
                }
                else if (e.Button.Properties.Tag == "Supprimer")
                {
                    ////******* Delete

                    if (gridViewCatCaisse.SelectedRowsCount == 1)
                    {
                        DialogResult dr = XtraMessageBox.Show("هل تريد فعلا حذف هذا الحساب", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            if (TestSuppCatEntrOrSort("select count(*) from entrescais where Caisse_IdCais =" + IndexCurrantR + " ") <= 0 && TestSuppCatEntrOrSort("select count(*) from sortiescais where Caisse_IdCais =" + IndexCurrantR + " ") <= 0)
                            {
                                string RequeteSupp = string.Format("delete from caisse WHERE IdCais={0} ", IndexCurrantR);
                                string NMsg = " إسم الحساب : " + textEditLibCais.Text + " | الرقم : " + textEditRefCais.Text + " | الوصف : " + textBoxDescCais + " ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بحدف حساب   - " + DateTime.Now.ToString(formatt);

                                Configuration.Historique(1, RequeteSupp, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                {
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (Configuration.Func(15) == "Direct")
                                {
                                    XtraMessageBox.Show("تم الحدف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                RempDataCatCais();
                                RempGridCatCais();
                                RempTextBCatCais();
                            }
                            else
                            {
                                XtraMessageBox.Show("لايمكن حدف هذا الحساب", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("يجب تحديد السطر المراد حدفه");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}