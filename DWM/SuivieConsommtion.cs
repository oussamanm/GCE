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
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraTreeList;

namespace DWM
{
    public partial class SuivieConsommtion : DevExpress.XtraEditors.XtraForm
    {
        int IdFa=0;
        public SuivieConsommtion()
        {
            InitializeComponent();
        }
        public SuivieConsommtion(int IdF)
        {
            IdFa = IdF;
            InitializeComponent();
        }


        DataSet DS;
        MySqlCommand NBRCONSO;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        DataView dv;
        DataTable tb;
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();

        Boolean TestDejaOListeSect = false;
        int IdFact = 0;
        string indexIdSect;
        int indexIdRowGridCons;

        ///// **** Void ***///
        private void RempListeBox(TreeList Tl, string Reque, string table)
        {
            if (TestDejaOListeSect == true)
            {
                DS.Tables[table].Clear();
            }
            da = new MySqlDataAdapter(Reque, ClassConnexion.Macon);
            da.Fill(DS, table);

            Tl.DataSource = DS.Tables[table];
            Tl.DataMember = "";
            
            Tl.Refresh();
            TestDejaOListeSect = true;
        }
        private void CalculLesFraisSp(int IdCo, int IdA, int Siyam, int sahm, int NComp, int AComp)
        {
            try
            {

                float chart = 0, autresfr = 0, fraismt = 0, mtpart = 0, totalfinal = 0;
                int DefCons = 0;

                DefCons = NComp - AComp;

                //*** chart imam
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                {
                    ClassConnexion.Macon.Open();
                }
                MySqlCommand CmdChart = new MySqlCommand("select * from  frais where IdFrai=1 and configYesNon=1", ClassConnexion.Macon);
                dr = CmdChart.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    chart = float.Parse(dr["PrixUFrai"].ToString());
                }
                else
                {
                    chart = 0;
                }
                chart = chart * Siyam;
                dr.Close();
                //***Fin chart

                //*** Partition
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                {
                    ClassConnexion.Macon.Open();
                }
                MySqlCommand CmdSahm = new MySqlCommand("select * from  frais where IdFrai=2 and configYesNon=1", ClassConnexion.Macon);
                dr = CmdSahm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    mtpart = float.Parse(dr["PrixUFrai"].ToString());
                }
                else
                {
                    mtpart = 0;
                }
                mtpart = mtpart * sahm;
                dr.Close();
                //***fin partition

                //***Autres frais (Entres)

                //if (ClassConnexion.Macon.State == ConnectionState.Closed)
                //{
                //    ClassConnexion.Macon.Open();
                //}
                //MySqlCommand CmdAutrF = new MySqlCommand("select * from autrefrais,compteur where compteur.IdComp=autrefrais.IdComp and compteur.NumComp=" + numcmp + " and DATE_FORMAT(DateAutFrai, '%m/%Y')='" + datefa + "'", ClassConnexion.Macon);
                //dr = CmdAutrF.ExecuteReader();
                //dr.Read();
                //if (dr.HasRows)
                //{
                //    autresfr = float.Parse(dr["MontantAutFrai"].ToString());
                //}
                //else
                //{
                //    autresfr = 0;
                //}
                //dr.Close();

                //***Fin autre frais


                //***Frais
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlCommand CmdFrais = new MySqlCommand("select * from frais where IdFrai>2 and configYesNon=1", ClassConnexion.Macon);
                dr = CmdFrais.ExecuteReader();
                while (dr.Read())
                {
                    fraismt = fraismt + float.Parse(dr["PrixUFrai"].ToString());
                }
                dr.Close();
                //***Fin Frais

                ClassConnexion.Macon.Close();

                totalfinal = chart + mtpart + fraismt + Configuration.CalculMtConsommtion(DefCons);

                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlCommand CmdInsertCons = new MySqlCommand("insert into consommation(IdFact,IdComp,IdUser,ComptageACons,ComptageNCons,Compsdf,IdAdhCons) values(" + IdFa + "," + IdCo + "," + UserConnecte.IdUser + "," + AComp + "," + NComp + "," + DefCons + "," + IdA + ")", ClassConnexion.Macon);
                CmdInsertCons.ExecuteNonQuery();
                ClassConnexion.Macon.Close();

                int lastconid = Configuration.LastID("consommation", "IdCons");

                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlCommand CmdInsertPaiem = new MySqlCommand("insert into paiement(IdCons,IdFact,IdUser,MontantPaie,PenalitePaie,PayePaie) values(" + lastconid + "," + IdFa + "," + UserConnecte.IdUser + "," + totalfinal + ",0,0)", ClassConnexion.Macon);
                CmdInsertPaiem.ExecuteNonQuery();
                ClassConnexion.Macon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Filter()
        {
            if (checkEditToutsSect.Checked == false)
            {
                dv.RowFilter = "LibelleSect='" + TlSect.FocusedNode[0].ToString() + "' ";
            }
            else
            {
                dv.RowFilter = "";
            }
        }

        Boolean DejaOvrire = false;
        private void RempData()
        {
            if (DejaOvrire==true)
            {
                DS.Tables.Clear();
            }
            da = new MySqlDataAdapter("select NumAppart,NumComp,CompteurNv,secteur.LibelleSect,IdComp,siyam,PartsComp,IdAdherent,null from compteur,secteur  where secteur.IdSect=compteur.IdSect and StatutsComp=1 and compteur.IdComp not in (select IdComp from consommation,facture where  consommation.IdFact=facture.IdFact and facture.IdFact=(select Max(IdFact) from facture)) ", ClassConnexion.Macon);
            da.Fill(DS, "tablen");
            dv.Table = DS.Tables["tablen"];
            dataGridView1.DataSource = dv;
            DejaOvrire = true;

            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[0].HeaderText = "رقم المنزل";
            dataGridView1.Columns[1].HeaderText = "رقم العداد";
            dataGridView1.Columns[2].HeaderText = "العداد القديم";
            dataGridView1.Columns[8].HeaderText = "العداد الجديد";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridViewCellStyle1.Format = "d7";
            dataGridViewCellStyle1.NullValue = "";
            dataGridView1.Columns[8].DefaultCellStyle = dataGridViewCellStyle1;
        }
        private void SuivieConsommtion_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'listeDonneeCompteur1.listecompteurdonnee' table. You can move, or remove it, as needed.
            //this.listecompteurdonneeTableAdapter.Fill(this.listeDonneeCompteur1.listecompteurdonnee);
            DS = new DataSet();
            dv = new DataView();

            RempData();

            checkEditToutsSect.Checked = true;
            RempListeBox(TlSect, "select LibelleSect from Secteur", "Secteur");
            TlSect.Enabled = false;

        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag=="Save")
            {
                try
                {
                    if (dataGridView1.Rows.Count>0)
                    {
                        Boolean HaveNullValue = false;
                        DialogResult DiaR = XtraMessageBox.Show("هل تريد فعلا حفظ المعطيات", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DiaR == DialogResult.Yes)
                        {
                            foreach (DataGridViewRow Row in dataGridView1.Rows)
                            {
                                if (Row.Cells[8].Value.ToString() == "")
                                {
                                    HaveNullValue = true;
                                    break;
                                }
                            }
                            if (HaveNullValue == false)
                            {
                                foreach (DataGridViewRow Row in dataGridView1.Rows)
                                {
                                    CalculLesFraisSp(int.Parse(Row.Cells[4].Value.ToString()), int.Parse(Row.Cells[7].Value.ToString()), int.Parse(Row.Cells[5].Value.ToString()), int.Parse(Row.Cells[6].Value.ToString()), int.Parse(Row.Cells[8].Value.ToString()), int.Parse(Row.Cells[2].Value.ToString()));
                                }
                                RempData();
                                Filter();
                            }
                            else
                            {
                                if (checkEditToutsSect.Checked == true)
                                    XtraMessageBox.Show("المرجو ملأ معطيات الجدول كاملة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                else
                                    XtraMessageBox.Show("المرجو ملأ معطيات الجدول للجولة الحالية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag=="Annuler")
            {
                this.Close();
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            /// Ad condition Enter Number 
            try
            {
                int valueCellAn = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                int valueCellNv = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());

                if (valueCellAn > valueCellNv)
                {
                    XtraMessageBox.Show("قيمة العداد الجديد الذي تم إذخالها غير مناسبة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView1.Rows[e.RowIndex].Cells[8].Value = "";
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("خطأ في إدخال قيمة العداد الجديد");
            }
        }

        private void checkEditToutsSect_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditToutsSect.Checked==false)
            {
                TlSect.Enabled = true;
            }
            else
            {
                TlSect.Enabled = false;
            }
            Filter();
        }
        private void TlSect_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            Filter();
        }

    }
}