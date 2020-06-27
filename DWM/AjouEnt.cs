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

namespace DWM
{
    public partial class AjouEnt : DevExpress.XtraEditors.XtraForm
    {
       
        int opp = -1;
        string AMsg = "";
        int a = 0;
        int b = 0;
        DataSet ds1 = new DataSet();
        MySqlDataAdapter da;
        Boolean TestDejaOvrireData = false;
        DataView dv;
        //int idEntr = Configuration.LastID("entres", "idEntr")+1;



        public AjouEnt()
        {
            InitializeComponent();
        }
        public AjouEnt(int op)
        {
            opp = op;
            InitializeComponent();
        }
        private void RempDate()
        {
            if (TestDejaOvrireData == true)
            {
                ds1.Tables["Compteurs"].Clear();
            }
            da = new MySqlDataAdapter("SELECT CONCAT(adherent.NomFrAdhe,' ', adherent.PrenomFrAdhe) AS NomComp, adherent.CINAdhe, compteur.NumComp,compteur.IdComp,compteur.IdSect, compteur.siyam,case when compteur.StatutsComp=1 then 'متصل' else 'منفصل' end as StatutsComp, secteur.LibelleSect FROM adherent INNER JOIN compteur ON adherent.IdAdherent = compteur.IdAdherent INNER JOIN secteur ON compteur.IdSect = secteur.IdSect", ClassConnexion.Macon);
            da.Fill(ds1, "Compteurs");
           
            TestDejaOvrireData = true;
        }
        public void ajout()
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
            {
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Open();
            }

            string forma = "yyyy-MM-dd HH:mm:ss";
            DateTime dt = new DateTime();
            float Mt = float.Parse(textEdit2.Text.ToString());
            MySqlCommand Cmd = new MySqlCommand("insert into entres(idFraisau,idCatEntr,idUser,LibEntr,MontantEntr,DateEntr)  values(0," + comboBox2.SelectedValue + "," + UserConnecte.IdUser + ",'" + textEdit1.Text + "'," + Mt + ",'" + DateTime.Now.ToString(forma) + "') ", ClassConnexion.Macon);
            Cmd.ExecuteNonQuery();

            ClassConnexion.DR.Close();ClassConnexion.Macon.Close();
            

            textEdit1.Text = "";
            textEdit2.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.Text = "";
            splashScreenManager1.CloseWaitForm();
            XtraMessageBox.Show("تمت الإضافة بنجاح");
        }
        public void ajout2()
        {
            
            string forma = "yyyy-MM-dd HH:mm:ss";
            DateTime dt = new DateTime();

            if (ClassConnexion.Macon.State == ConnectionState.Open)
            {
                ClassConnexion.DR.Close();ClassConnexion.Macon.Close();
                ClassConnexion.Macon.Open();
            }
            else
            {
                ClassConnexion.Macon.Open();
            }
            float Mt = float.Parse(textEdit2.Text.ToString());

            MySqlCommand Cmd = new MySqlCommand("insert into entres(idFraisau,idCatEntr,idUser,LibEntr,MontantEntr,DateEntr)  values(1," + comboBox2.SelectedValue + "," + UserConnecte.IdUser + ",'" + textEdit1.Text + "'," + Mt + ",'" + DateTime.Now.ToString(forma) + "') ", ClassConnexion.Macon);
            Cmd.ExecuteNonQuery();
            MySqlCommand Cmd2 = new MySqlCommand("insert into autrefrais(idComp,idEntr,idUser,MontantAutFrai,DateAutFrai)  values(" + comboBox3.EditValue + ", maxID()," + UserConnecte.IdUser + "," + Mt  + ",'" + DateTime.Now.ToString(forma) + "') ", ClassConnexion.Macon);
            Cmd2.ExecuteNonQuery();

            ClassConnexion.DR.Close();
            ClassConnexion.Macon.Close();
         

            textEdit1.Text = "";
            textEdit2.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.Text = "";

            splashScreenManager1.CloseWaitForm();
            XtraMessageBox.Show("تمت الإضافة بنجاح");


        }

        public void Modifier()
        {
            if (ClassConnexion.Macon.State == ConnectionState.Open)
            {
                ClassConnexion.DR.Close();ClassConnexion.Macon.Close();
                ClassConnexion.Macon.Open();
            }
            else
            {
                ClassConnexion.Macon.Open();
            }
            string forma = "yyyy-MM-dd HH:mm:ss";
            DateTime dt = new DateTime();
            float Mt = float.Parse(textEdit2.Text.ToString());
            MySqlCommand Cmd = new MySqlCommand("update entres set idFraisau=0,idCatEntr=" + comboBox2.SelectedValue + ",idUser=" + UserConnecte.IdUser + ",LibEntr='" + textEdit1.Text + "',MontantEntr=" + Mt + ",DateEntr='" + DateTime.Now.ToString(forma) + "' where idEntr=" + opp, ClassConnexion.Macon);
            Cmd.ExecuteNonQuery();

            MySqlCommand CmdTest2 = new MySqlCommand("select * from autreFrais where idEntr=" + opp, ClassConnexion.Macon);
            ClassConnexion.DR = CmdTest2.ExecuteReader();
            ClassConnexion.DR.Read();
            Boolean b = ClassConnexion.DR.HasRows;
            ClassConnexion.DR.Close();
            
            if (b)
            {
                MySqlCommand Cmd2;
                Cmd2 = new MySqlCommand("delete from autrefrais where  idEntr =" + opp, ClassConnexion.Macon);
               Cmd2.ExecuteNonQuery();
            }
           
           


            //string NMsg = "اسم المدخول : " + textEdit1.Text + " | نوع المدخول : " + comboBox2.Text + " | المبلغ : " + textEdit2.Text + " | تاريخ الإضافة : " + DateTime.Now.ToString(forma);
            //string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل مدخول قديم  - " + DateTime.Now.ToString(forma);
            //MessageBox.Show("NMsg =" + NMsg);
            //MessageBox.Show("AMsg " + AMsg);
            //Configuration.Historique(1, RequeteMod, AMsg, NMsg, MEnt, "", "");
            ClassConnexion.DR.Close();
            ClassConnexion.Macon.Close();
            
            splashScreenManager1.CloseWaitForm();

            textEdit1.Text = "";
            textEdit2.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.Text= "";

            XtraMessageBox.Show("تم التعديل بنجاح");
        }
        public void Modifier2()
        {


            string forma = "yyyy-MM-dd HH:mm:ss";
           
            DateTime dt = new DateTime();
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
            {
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Open();
            }


            MySqlCommand CmdTest = new MySqlCommand("select * from autreFrais where idEntr="+opp,ClassConnexion.Macon);
            ClassConnexion.DR = CmdTest.ExecuteReader();
            ClassConnexion.DR.Read();
            Boolean b = ClassConnexion.DR.HasRows;
            ClassConnexion.DR.Close();
            MySqlCommand Cmd2;
            float Mt = float.Parse(textEdit2.Text.ToString());

            if (b)
              Cmd2 = new MySqlCommand("update autrefrais set idComp = " + comboBox3.EditValue + ", idUser = " + UserConnecte.IdUser + ", MontantAutFrai = " + Mt + ", DateAutFrai = '" + DateTime.Now.ToString(forma) + "' where idEntr =" + opp, ClassConnexion.Macon);
            else
                Cmd2 = new MySqlCommand("insert into autrefrais(idComp,idEntr,idUser,MontantAutFrai,DateAutFrai)  values(" + comboBox3.EditValue + ","+opp+"," + UserConnecte.IdUser + "," + Mt + ",'" + DateTime.Now.ToString(forma) + "') ", ClassConnexion.Macon);

            Cmd2.ExecuteNonQuery();
            MySqlCommand Cmd = new MySqlCommand("update entres set idFraisau = 1, idCatEntr = " + comboBox2.SelectedValue + ", idUser = " + UserConnecte.IdUser + ", LibEntr ='" + textEdit1.Text + "', MontantEntr = " + Mt + ", DateEntr ='" + DateTime.Now.ToString(forma) + "' where idEntr =" + opp, ClassConnexion.Macon);
            Cmd.ExecuteNonQuery();
            ClassConnexion.Macon.Close();
           
          

                   textEdit1.Text = "";
            textEdit2.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.Text = "";

            splashScreenManager1.CloseWaitForm();

            XtraMessageBox.Show("تم التعديل بنجاح");
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (a==1)
            {
                if (this.Height == 420)
                {
                    panel1.Hide();
                    this.Height = 330;
                }
                else
                {
                    this.Height = 420;
                    panel1.Show();

                }
            }
        }

        private void AjouEnt_Load(object sender, EventArgs e)
        {
           
            Configuration.RempCombo(comboBox2, "select * from categorieentres", "categorieentres", "idCatEntr", "LibelleCatEntr");

            Configuration.RempCombo(comboBox1, "select * from secteur", "secteur", "idSect", "LibelleSect");
            b = 1;

            if (opp!=-1)
            {

                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlCommand Cmd3 = new MySqlCommand("select * from entres where idEntr=" + opp , ClassConnexion.Macon);
                ClassConnexion.DR = Cmd3.ExecuteReader();
                ClassConnexion.DR.Read();
                if (ClassConnexion.DR[1].ToString() == "0")
                    radioGroup2.SelectedIndex = 0;
                else
                    radioGroup2.SelectedIndex = 1;
                
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();



                if (radioGroup2.SelectedIndex == 0)
            {
                   
                    if (ClassConnexion.Macon.State == ConnectionState.Closed)
                        ClassConnexion.Macon.Open();

                    MySqlCommand Cmd = new MySqlCommand("select idCatEntr,LibEntr,MontantEntr from entres e where e.idEntr=" + opp, ClassConnexion.Macon);

                    ClassConnexion.DR = Cmd.ExecuteReader();
                    while (ClassConnexion.DR.Read()) {
                        textEdit1.Text = ClassConnexion.DR [1].ToString();
                        textEdit2.Text = ClassConnexion.DR [2].ToString();
                        comboBox2.SelectedValue = ClassConnexion.DR [0];
                        
                    }
                    
                ClassConnexion.DR.Close();
                    ClassConnexion.Macon.Close();
              
            }
            else
                {

                    RempDate();
                    comboBox3.Properties.DataSource = ds1.Tables["Compteurs"];
                    comboBox3.Properties.DisplayMember = ds1.Tables["Compteurs"].Columns["NumComp"].ColumnName;
                    comboBox3.Properties.ValueMember = ds1.Tables["Compteurs"].Columns["IdComp"].ColumnName;

                    if (ClassConnexion.Macon.State == ConnectionState.Closed)
                        ClassConnexion.Macon.Open();

                    MySqlCommand Cmd = new MySqlCommand("select idCatEntr,LibEntr,MontantEntr,idComp from entres e,autrefrais f where e.idEntr=" + opp + " and e.idEntr=f.idEntr", ClassConnexion.Macon);
                    ClassConnexion.DR = Cmd.ExecuteReader();
                    while (ClassConnexion.DR.Read()) {
                   comboBox2.SelectedValue = ClassConnexion.DR[0];
                    textEdit1.Text = ClassConnexion.DR[1].ToString();
                    textEdit2.Text = ClassConnexion.DR[2].ToString();
                    comboBox3.EditValue = ClassConnexion.DR[3].ToString();
                                     }
                    
                    ClassConnexion.DR.Close();
                    ClassConnexion.Macon.Close();

                    
                }
          
            }
            else
            {
               

            }
         a = 1;
            if (radioGroup2.SelectedIndex == 0)
            {
                this.Height = 330;
                panel1.Hide();

            }
            else
            {
                this.Height = 420;
                panel1.Show();
            }
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {

            if (e.Button.Properties.Tag.ToString() == "Enregistrer")
            {
                try
                {
                    if (opp == -1) { 
                    if (radioGroup2.SelectedIndex == 0)
                    {
                            splashScreenManager1.ShowWaitForm();
                            if (textEdit1.Text == "" || textEdit2.Text == ""  || comboBox2.Text == "" )
                        {

                                string msg = "خطأ في إدخال المعلومات";
                                splashScreenManager1.CloseWaitForm();

                                XtraMessageBox.Show(msg);
                            }
                        else
                        {
                                ajout();

                            }

                        }
                        else
                    {
                            splashScreenManager1.ShowWaitForm();
                            if (textEdit1.Text == "" || textEdit2.Text == ""  || comboBox2.Text == "" || comboBox1.Text == "" || comboBox3.Text == "")
                        {
                            string msg = "خطأ في إدخال المعلومات";
                                splashScreenManager1.CloseWaitForm();
                                XtraMessageBox.Show(msg);
                               
                            }
                        else
                        {
                          
                                ajout2();
                            }
                    }
                    }
                    else
                    {
                        if (radioGroup2.SelectedIndex == 0)
                        {
                            splashScreenManager1.ShowWaitForm();
                            if (textEdit1.Text == "" || textEdit2.Text == "" || comboBox2.Text == "" )
                            {
                                string msg = "خطأ في إدخال المعلومات";
                                splashScreenManager1.CloseWaitForm();

                                XtraMessageBox.Show(msg);
                            }
                            else
                            {
                              
                                Modifier();


                            }
                        }
                        else
                        {
                                       splashScreenManager1.ShowWaitForm();

                            if (textEdit1.Text == "" || textEdit2.Text == "" || comboBox2.Text == "" || comboBox1.Text == "" || comboBox3.Text == "")
                            {
                                string msg = "خطأ في إدخال المعلومات";
                                splashScreenManager1.CloseWaitForm();
                        
                                XtraMessageBox.Show(msg);
                            }
                            else
                            {
                              
                                Modifier2();


                            }
                        }
                        }
                }
                catch (Exception Ex)
                {
                    XtraMessageBox.Show(Ex.ToString());
                }
            }
            else if (e.Button.Properties.Tag.ToString() == "Annuler")
            {
                this.Height = 420;
                panel1.Show();
                this.DialogResult = DialogResult.Yes;
                try
                {
                    this.Close();
                }
                catch (Exception Ex)
                {
                    XtraMessageBox.Show(Ex.ToString());
                }
            }

            



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (b==1)
            {
                RempDate();
                dv = new DataView();
                dv.Table = ds1.Tables["Compteurs"];
                dv.RowFilter = "idSect='"+ comboBox1.SelectedValue +"'";
                comboBox3.Properties.DataSource = dv;
                comboBox3.Properties.DisplayMember = ds1.Tables["Compteurs"].Columns["NumComp"].ColumnName;
                comboBox3.Properties.ValueMember = ds1.Tables["Compteurs"].Columns["IdComp"].ColumnName;
            }

        }


    }
}