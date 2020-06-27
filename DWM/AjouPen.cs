using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace DWM
{
    public partial class AjouPen : Form
    {
        int opp = -1;
        public AjouPen()
        {
            InitializeComponent();
        }
        public AjouPen(int op)
        {
            opp = op;
            InitializeComponent();
        }
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        string str;
        
        int IndexRowSelected;
        int IdRowSelected;
        string AMsg="";
        MySqlDataAdapter da;
        MySqlCommandBuilder builder;
        Boolean TestDejaOvrireData = false;
        string forma = "yyyy-MM-dd HH:mm:ss";
        DateTime dt = new DateTime();

        void Clear()
        {
            comboBox1.EditValue = 0;
            textBox2.Clear();
            dateEdit1.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
        }
        private void RempDate()
        {
            if (TestDejaOvrireData == true)
            {
                ds.Tables["Compteurs"].Clear();
            }
            da = new MySqlDataAdapter("SELECT CONCAT(adherent.NomFrAdhe,' ', adherent.PrenomFrAdhe) AS NomComp, adherent.CINAdhe, compteur.NumComp,compteur.IdComp,compteur.IdSect, compteur.siyam,case when compteur.StatutsComp=1 then 'متصل' else 'منفصل' end as StatutsComp, secteur.LibelleSect FROM adherent INNER JOIN compteur ON adherent.IdAdherent = compteur.IdAdherent INNER JOIN secteur ON compteur.IdSect = secteur.IdSect", ClassConnexion.Macon);
            da.Fill(ds, "Compteurs");

            TestDejaOvrireData = true;
        }

        private void AjouPen_Load(object sender, EventArgs e)
        {
            Clear();
            RempDate();
            comboBox1.Properties.DataSource = ds.Tables["Compteurs"];
            comboBox1.Properties.DisplayMember = ds.Tables["Compteurs"].Columns["NumComp"].ColumnName;
            comboBox1.Properties.ValueMember = ds.Tables["Compteurs"].Columns["IdComp"].ColumnName;
            Configuration.RempCombo(comboBox2, "select * from typepenalite", "typepenalite", "idTypePena", "LibelleTypePena");
            if (opp != -1)
            {
                ClassConnexion.Macon.Open();

                MySqlCommand Cmd = new MySqlCommand("select idPena,idComp,idTypePena,MontantPena,DatePena,DescriptionPena,PayerPena from penalite where idPena=" + opp + "", ClassConnexion.Macon);
                ClassConnexion.DR = Cmd.ExecuteReader();

                ClassConnexion.DR.Read();

                comboBox1.EditValue = ClassConnexion.DR[1];
                comboBox2.SelectedValue = ClassConnexion.DR[2];
                textBox1.Text = ClassConnexion.DR[3].ToString();
                dateEdit1.Text = ClassConnexion.DR[4].ToString();
                textBox2.Text = ClassConnexion.DR[5].ToString();

                if (ClassConnexion.DR[6].ToString() == "True")
                    textBox1.Enabled = false;

                dt = DateTime.Parse(dateEdit1.Text);
                AMsg = "رقم العداد : " + comboBox1.Text + " | نوع الغرامة : " + comboBox2.Text + " | المبلغ : " + textBox1.Text + " | تاريخ الغرامة : " + dt.ToString(forma) + " | الوصف : " + textBox2.Text;

                ClassConnexion.Macon.Close();
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag.ToString() == "Enregistrer")
            {
                if (textBox1.Text == "" || textBox2.Text == "" || dateEdit1.Text == "" || comboBox1.EditValue.ToString() == "" ||comboBox2.SelectedValue.ToString() == "")
                    MessageBox.Show("خطأ في إدخال المعلومات");                       
                else
                {
                    if (opp == -1)
                    {
                        dt =DateTime.Parse( dateEdit1.Text);
                        string RequeteAjou = string.Format("insert into penalite(idComp,idUser,idTypePena,DescriptionPena,MontantPena,DatePena,PayerPena)  values("+comboBox1.EditValue+","+UserConnecte.IdUser+","+comboBox2.SelectedValue+",\\'"+textBox2.Text+"\\',"+textBox1.Text+",\\'"+dt.ToString(forma) +"\\',0) ");

                        string NMsg = "رقم العداد : " + comboBox1.Text + " | نوع الغرامة : " + comboBox2.Text + " | المبلغ : " + textBox1.Text + " | تاريخ المخالفة : " + dt.ToString(forma) + " | وصف المخالفة : " + textBox2.Text;
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة غرامة جديدة  - " + DateTime.Now.ToString(forma);
                        Console.Write (RequeteAjou);
                        Configuration.Historique(1, RequeteAjou, "", NMsg, MEnt, "", "");
                                                        
                        XtraMessageBox.Show(" تم الإرسال بنجاح".ToString());
                        Clear();
                    }
                    else
                    {
                        dt = DateTime.Parse(dateEdit1.Text);
                        string RequeteAjou = string.Format("update penalite set idComp=" + comboBox1.EditValue + ",idTypePena=" + comboBox2.SelectedValue + ",DescriptionPena=\\'" + textBox2.Text + "\\',MontantPena=\\'" + textBox1.Text + "\\',DatePena=\\'" + dt.ToString(forma) + "\\' where idPena=" + opp);
                        string NMsg = "رقم العداد : " + comboBox1.Text + " | نوع الغرامة : " + comboBox2.Text + " | المبلغ : " + textBox1.Text + " | تاريخ المخالفة : " + dt.ToString(forma) + " | وصف المخالفة : " + textBox2.Text;
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل غرامة  - " + DateTime.Now.ToString(forma);
                            
                        Configuration.Historique(0, RequeteAjou, AMsg, NMsg, MEnt, "", "");
                        XtraMessageBox.Show(" تم الإرسال بنجاح".ToString());
                    }
                }
            }
            else if (e.Button.Properties.Tag.ToString() == "Annuler")
            {
                comboBox1.EditValue = 0;
                comboBox2.SelectedIndex = 0;
                textBox2.Clear();
                dateEdit1.Text = "";
                textBox2.Text = "";
                    
                this.Close();
            }
        }


    }
}
