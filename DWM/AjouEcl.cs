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
    public partial class AjouEcl : DevExpress.XtraEditors.XtraForm
    {
        int IdEcl = 0;
        string formatt = "yyyy-MM-dd HH:mm:ss";

        public AjouEcl()
        {
            InitializeComponent();
        }

        public AjouEcl(int IdEcll)
        {
            IdEcl = IdEcll;
            
            InitializeComponent();
        }

        MySqlDataAdapter da;
        DataSet ds;
        MySqlDataReader dr;

        public float count(string requete)
        {
            float resultat = 0;
            try
            {
                if (ClassConnexion.Macon.State == ConnectionState.Open)
                {
                    ClassConnexion.Macon.Close();
                    ClassConnexion.Macon.Open();
                }
                else
                {
                    ClassConnexion.Macon.Open();
                }

                MySqlDataReader DR;
                MySqlCommand CMD = new MySqlCommand(requete, ClassConnexion.Macon);
                DR = CMD.ExecuteReader();
                DR.Read();
                if (DR.HasRows)
                {

                    resultat = float.Parse(DR[0].ToString());
                }
                else
                {

                    resultat = 0;
                }

                DR.Close();
                ClassConnexion.Macon.Close();

            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.ToString());
            }

            return resultat;
        }
        public void Vider()
        {
            comboBox1.Text = "";
            textEdit2.Text = "";
        }

        private void AjouEcl_Load(object sender, EventArgs e)
        {
            ds = new DataSet();

            //Secteurs
            da = new MySqlDataAdapter("select LibelleSect as sect,IdSect from secteur", ClassConnexion.Macon);
            da.Fill(ds, "secteur");
            comboBox1.DataSource = ds.Tables["secteur"];
            comboBox1.ValueMember = "IdSect";
            comboBox1.DisplayMember = "sect";

            Configuration.RempCombo(CbDateCons, "select IdFact,DATE_FORMAT(PeriodeConsoFact,\"%m-%Y\") as DateCons from facture order by IdFact Desc limit 0,1", "DateCons", "IdFact", "DateCons");

            if (IdEcl != 0)
            {
                comboBox1.Enabled = false;

                ClassConnexion.Macon.Open();
                MySqlCommand ModCompteur = new MySqlCommand("select * from Eclairage where IdEcl="+IdEcl, ClassConnexion.Macon);
                dr = ModCompteur.ExecuteReader();
                dr.Read();

                comboBox1.SelectedValue = dr["IdSect"].ToString();
                CbDateCons.SelectedValue = dr["IdFct"].ToString();
                textEdit2.Text = dr["MontantEcl"].ToString();

                dr.Close();
                ClassConnexion.Macon.Close();

            }
            else
            {
                comboBox1.Enabled = true;
            }

        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag.ToString() == "Enregistrer")
            {
                try
                {
                    string RequeteAjou;

                    if (IdEcl == 0)
                    {
                        if (comboBox1.Text == "" || textEdit2.Text == "" )
                        {
                            XtraMessageBox.Show("المرجو إدخال جميع المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            if (count("select count(*) from Eclairage where IdFct='" + CbDateCons.SelectedValue + "' and IdSect='" + comboBox1.SelectedValue + "' ") != 0)
                            {
                                XtraMessageBox.Show("هذه الفاتورة موجودة من قبل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                RequeteAjou = string.Format("insert into Eclairage(IdFct,IdSect,MontantEcl,User,DateMod) values("+ CbDateCons.SelectedValue + ","+ comboBox1.SelectedValue + ","+ textEdit2.Text + ","+ UserConnecte.IdUser + ",\\'"+ DateTime.Now.ToString(formatt) + "\\') ");

                                string NMsg = "الفاتورة : " + CbDateCons.SelectedText + " | الجوبة : " + comboBox1.SelectedText + " | المبلغ : " + textEdit2.Text ;
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة إستهلاك الكهرباء  - " + DateTime.Now.ToString(formatt);

                                Configuration.Historique(1, RequeteAjou, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                {
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (Configuration.Func(15) == "Direct")
                                {
                                    XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                               
                            }
                            Vider();
                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "" || textEdit2.Text == "")
                        {
                            XtraMessageBox.Show("المرجو إدخال جميع المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {

                            RequeteAjou = string.Format("update  Eclairage set MontantEcl='"+ textEdit2.Text + "' ,DateMod=\\'" + DateTime.Now.ToString(formatt) + "\\' where IdEcl='"+IdEcl+"' ");

                            string NMsg ="";
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل إستهلاك الكهرباء   - " + DateTime.Now.ToString(formatt);

                            Configuration.Historique(0, RequeteAjou, "", NMsg, MEnt, "", "");

                            if (Configuration.Func(15) == "Indirect")
                            {
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (Configuration.Func(15) == "Direct")
                            {
                                XtraMessageBox.Show(" تمت التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag.ToString() == "Annuler")
            {
                try
                {
                    this.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}