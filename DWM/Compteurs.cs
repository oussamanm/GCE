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
using DevExpress.XtraGrid.Views.Grid;

namespace DWM
{
    public partial class Compteurs : DevExpress.XtraEditors.XtraForm
    {
        public Compteurs()
        {
            InitializeComponent();
            controleHeader1.label_menu_btn = "الصفحة الرئيسية/العدادات";
        }

        DataSet ds;
        DataView dv, DV;
        string str;
        MySqlDataAdapter da;
        MySqlCommandBuilder builder;

        int prm = 0;
        int debut = 0;
        int nbrcompteur;

        string MsgAn = "";
        string MsgNv = "";
        string   MsgEn = "";
        string Requete = "";
        String formatdt = "yyyy-MM-dd HH:mm:ss";

        public void Compteurfiltre(int fi)
        {
            try
            {
                if (debut!=0)
                    splashScreenManager1.ShowWaitForm();
              

                if (prm != 0)
                    ds.Tables["compteur"].Clear();
                if (fi == 0)
                    da = new MySqlDataAdapter("SELECT concat(NomArAdhe,' ',PrenomArAdhe) as Nomcomp,compteur.NumAppart,LibelleSect,compteur.*,case when StatutsComp=1 then 'متصل' else 'منفصل' end as sta,concat(NomUser,' ',PrenomUser) as us,DateModComp FROM compteur,utilisateurs,adherent,secteur where adherent.IdAdherent=compteur.IdAdherent and utilisateurs.IdUser=compteur.IdUser and secteur.IdSect=compteur.IdSect and StatutsComp=" + fi + " Group by IdComp ASC", ClassConnexion.Macon);
                else if (fi == 1)
                    da = new MySqlDataAdapter("SELECT concat(NomArAdhe,' ',PrenomArAdhe) as Nomcomp,compteur.NumAppart ,LibelleSect,compteur.*,case when StatutsComp=1 then 'متصل' else 'منفصل' end as sta,concat(NomUser,' ',PrenomUser) as us,DateModComp FROM compteur,utilisateurs,adherent,secteur where adherent.IdAdherent=compteur.IdAdherent and utilisateurs.IdUser=compteur.IdUser and secteur.IdSect=compteur.IdSect and StatutsComp=" + fi + " Group by IdComp ASC", ClassConnexion.Macon);
                else
                    da = new MySqlDataAdapter("SELECT concat(NomArAdhe,' ',PrenomArAdhe) as Nomcomp,compteur.NumAppart ,LibelleSect,compteur.*,case when StatutsComp=1 then 'متصل' else 'منفصل' end as sta,concat(NomUser,' ',PrenomUser) as us,DateModComp FROM compteur,utilisateurs,adherent,secteur where adherent.IdAdherent=compteur.IdAdherent and utilisateurs.IdUser=compteur.IdUser and secteur.IdSect=compteur.IdSect Group by IdComp ASC", ClassConnexion.Macon);

                da.Fill(ds, "compteur");
                DV.Table= ds.Tables["compteur"];
                gridControl1.DataSource = DV;
                gridControl1.Refresh();
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
            finally
            {
                if (debut!=0)
                    splashScreenManager1.CloseWaitForm();
            }

            
            
        }
       public void nbrfiltre()
        {
            nbrcompteur = Configuration.ExisteEnre("compteur", "StatutsComp", "1")+ Configuration.ExisteEnre("compteur", "StatutsComp", "0");
            radioButton1.Text = "جميع العدادات ("+ nbrcompteur + ")";
            radioButton2.Text = "العدادات المنفصلة (" + Configuration.ExisteEnre("compteur", "StatutsComp", "0") + ")";
            radioButton3.Text = "العدادات المتصلة (" + Configuration.ExisteEnre("compteur", "StatutsComp", "1") + ")";
        }

        private void Compteurs_Load(object sender, EventArgs e)
        {
            try
            {            
                splashScreenManager1.ShowWaitForm();

                nbrfiltre();

                ds = new DataSet();
                dv = new DataView();
                DV = new DataView();


                Compteurfiltre(2);
                prm = 1;

                gridView1.Columns[5].Visible = false;
                gridView1.Columns[6].Visible = false;
                //status
                gridView1.Columns[8].Visible = false;
                //IDuser
                gridView1.Columns[10].Visible = false;
                //Datemod
                gridView1.Columns[11].Visible = false;
                gridView1.Columns[14].Visible = false;

                gridView1.Columns[0].Caption = "المشترك";
                gridView1.Columns[1].Caption = "رقم المنزل";
                gridView1.Columns[2].Caption = "الجولة";
                gridView1.Columns[3].Caption = "ر.ت للعداد";
                gridView1.Columns[4].Caption = "الصيام";
                gridView1.Columns[7].Caption = "رقم العداد";
                gridView1.Columns[9].Caption = "الأسهم";
                gridView1.Columns[12].Caption = "العداد القديم";
                gridView1.Columns[13].Caption = "العداد الجديد";
                gridView1.Columns[15].Caption = "حالةالعداد";
                gridView1.Columns[16].Caption = "المستخدم";
                gridView1.Columns[17].Caption = "تاريخ التعديل";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ClassConnexion.Macon.Close();
                splashScreenManager1.CloseWaitForm();
                debut = 1;
            }
           
        }
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {

            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                Boolean category = Boolean.Parse(View.GetRowCellValue(e.RowHandle, View.Columns["StatutsComp"]).ToString());
                if (category == false)
                {
                    e.Appearance.BackColor = Color.Salmon;
                    e.Appearance.BackColor2 = Color.SeaShell;
                }
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                DV.RowFilter = "StatutsComp=0";
                //Compteurfiltre(0);
            }
            
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                DV.RowFilter = "StatutsComp=1";
               // Compteurfiltre(1);
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                DV.RowFilter = "" ;
                //Compteurfiltre(2);
            }
        }


        AjouCompteur Formcomp = new AjouCompteur();
        ChangerAdhComp Formconvert;
        AjouCompteur Formcompmod;

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Ajouter")
                {
                    if (Formcomp.ShowDialog() == DialogResult.Yes)
                    {
                        nbrfiltre();
                        if (radioButton2.Checked)
                            Compteurfiltre(0);

                        if (radioButton3.Checked)
                            Compteurfiltre(1);

                        if (radioButton1.Checked)
                            Compteurfiltre(2);
                    }
                    else
                        Formcomp.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag == "convert")
                {
                    if (gridView1.SelectedRowsCount == 1)
                    {
                        Formconvert = new ChangerAdhComp(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString()));

                        if (Formconvert.ShowDialog() == DialogResult.Yes)
                        {
                            nbrfiltre();

                            if (radioButton2.Checked)
                                Compteurfiltre(0);
                            if (radioButton3.Checked)
                                Compteurfiltre(1);
                            if (radioButton1.Checked)
                                Compteurfiltre(2);
                        }
                        else
                        {
                            Formconvert.ShowDialog(this);
                            if (radioButton2.Checked)
                                Compteurfiltre(0);
                            if (radioButton3.Checked)
                                Compteurfiltre(1);
                            if (radioButton1.Checked)
                                Compteurfiltre(2);
                        }
                    }
                    else
                        XtraMessageBox.Show("لم تقم بعد بتحديد العداد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else if (e.Button.Properties.Tag == "Imprimer")
                {
                    ImpressionCompteur Formconimp = new ImpressionCompteur();
                    Formconimp.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag == "Modifier")
                {
                    if (gridView1.SelectedRowsCount == 1)
                    {
                        Formcompmod = new AjouCompteur(int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString()));
                        if (Formcompmod.ShowDialog() == DialogResult.Yes)
                        {
                            nbrfiltre();

                            if (radioButton2.Checked)
                                Compteurfiltre(0);
                            if (radioButton3.Checked)
                                Compteurfiltre(1);
                            if (radioButton1.Checked)
                                Compteurfiltre(2);
                        }
                        else
                            Formcompmod.ShowDialog(this);
                    }
                    else
                        XtraMessageBox.Show("لم تقم بعد بتحديد العداد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.Button.Properties.Tag == "arret")
                {
                    if (gridView1.SelectedRowsCount == 1)
                    {
                        string messa = "";
                        if (gridView1.GetDataRow(gridView1.FocusedRowHandle)[8].ToString() == "True")
                        {
                            messa = "هل تريد فعلا فصل العداد ؟ ";
                            MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بفصل العداد رقم  " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[7].ToString() + " للمنخرط " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString() + " (" + DateTime.Now.ToString(formatdt) + ") :";
                            Requete = "update compteur set StatutsComp=0 where IdComp=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString();
                        }
                        else
                        {
                            messa = "هل تريد فعلا ربط العداد ؟ ";
                            MsgEn = "قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بربط العداد رقم " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[7].ToString() + " للمنخرط " + gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString() + " (" + DateTime.Now.ToString(formatdt) + ") :";
                            Requete = "update compteur set StatutsComp=1 where IdComp=" + gridView1.GetDataRow(gridView1.FocusedRowHandle)[3].ToString();
                        }

                        DialogResult dr = XtraMessageBox.Show(messa, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            Configuration.Historique(1, Requete, MsgAn, MsgNv, MsgEn, "", "");
                            XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (radioButton2.Checked)
                            Compteurfiltre(0);

                        if (radioButton3.Checked)
                            Compteurfiltre(1);

                        if (radioButton1.Checked)
                            Compteurfiltre(2);
                    }
                    else
                        XtraMessageBox.Show("لم تقم بعد بتحديد العداد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else if (e.Button.Properties.Tag == "ListeTrans")
                {
                    ListeTrans LTrans = new ListeTrans();
                    LTrans.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void gridControl1_Click(object sender, EventArgs e)
        {
            //if (gridView1.SelectedRowsCount == 1)
            //{
            //    MessageBox.Show(gridView1.GetDataRow(gridView1.FocusedRowHandle)[7].ToString());
            //}
        }
    }
}