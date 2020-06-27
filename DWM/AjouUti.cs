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
    public partial class AjouUti : DevExpress.XtraEditors.XtraForm
    {
        public AjouUti()
        {
            InitializeComponent();
        }
        public AjouUti(int op)
        {
            opp = op;
            InitializeComponent();
        }

        int opp = -1;
        string MsgEn;
        string Requete;
        string Requete2;
        string Requete3, MsgNv,MsgAn;
        string pseudotest = "";
        String formatdt = "yyyy-MM-dd HH:mm:ss";

        public Boolean pseudoExiste()
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            MySqlCommand CmdTest2 = new MySqlCommand("select * from utilisateurs where PseudoUser='" + textEdit3.Text +"'", ClassConnexion.Macon);
            ClassConnexion.DR = CmdTest2.ExecuteReader();
            ClassConnexion.DR.Read();
            Boolean b = ClassConnexion.DR.HasRows;
            ClassConnexion.DR.Close();
            return b;
        }
        public void ajout()
        {
            if (ClassConnexion.Macon.State == ConnectionState.Open)
            {
                ClassConnexion.DR.Close(); ClassConnexion.Macon.Close();
                ClassConnexion.Macon.Open();
            }
            else
            {
                ClassConnexion.Macon.Open();
            }
            string Pass = password.getMd5Hash(textEdit4.Text);

            MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة مستخدم جديد  (" + DateTime.Now.ToString(formatdt) + ") :";
            MsgNv = "إسم المستخدم  " + textEdit1.Text + " " + textEdit2.Text + " - الصفة " + comboBox2.Text + " - الإسم المستعار " + textEdit3.Text + " - البريد  الإلكتروني " + textEdit5.Text + " - الهاتف " + textEdit6.Text;
            Requete3 = "insert into utilisateurs(idType,NomUser,PrenomUser,PseudoUser,PasseUser,EmailUser,TeleUser,bloque)  values(" + comboBox2.SelectedValue + ",\\'" + textEdit2.Text + "\\',\\'" + textEdit1.Text + "\\',\\'" + textEdit3.Text + "\\',\\'" + Pass + "\\',\\'" + textEdit5.Text + "\\',\\'" + textEdit6.Text + "\\',0)";

            Configuration.Historique(1, Requete3, "", MsgNv, MsgEn, "", "");
            ClassConnexion.Macon.Close();
            XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void Modifier()
        {
            if (ClassConnexion.Macon.State == ConnectionState.Open)
            {
                ClassConnexion.DR.Close(); ClassConnexion.Macon.Close();
                ClassConnexion.Macon.Open();
            }
            else
                ClassConnexion.Macon.Open();

            MySqlCommand Cmd;
            string Pass = password.getMd5Hash(textEdit4.Text.ToString());

            if (textEdit4.Text != "")
                Requete3 = "update utilisateurs set idType=" + comboBox2.SelectedValue + ",NomUser=\\'" + textEdit2.Text + "\\',PrenomUser=\\'" + textEdit1.Text + "\\',PseudoUser=\\'" + textEdit3.Text + "\\',PasseUser=\\'" + Pass + "\\',EmailUser=\\'" + textEdit5.Text + "\\',TeleUser=\\'" + textEdit6.Text + "\\' where idUser=" + opp;
            else
                Requete3 = "update utilisateurs set idType=" + comboBox2.SelectedValue + ",NomUser=\\'" + textEdit2.Text + "\\',PrenomUser=\\'" + textEdit1.Text + "\\',PseudoUser=\\'" + textEdit3.Text + "\\',EmailUser=\\'" + textEdit5.Text + "\\',TeleUser=\\'" + textEdit6.Text + "\\' where idUser=" + opp ;

            MsgEn = "قام  " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بتعديل مستخدم  (" + DateTime.Now.ToString(formatdt) + ") :";
            MsgNv = "إسم المستخدم  " + textEdit1.Text + " " + textEdit2.Text + " - الصفة " + comboBox2.Text + " - الإسم المستعار " +  textEdit3.Text + " - البريد  الإلكتروني " + textEdit5.Text + " - الهاتف " + textEdit6.Text;

            Configuration.Historique(0, Requete3, MsgAn, MsgNv, MsgEn, "", "");         
            ClassConnexion.Macon.Close();
            XtraMessageBox.Show(" تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AjouUti_Load(object sender, EventArgs e)
        {
            Configuration.RempCombo(comboBox2, "select * from typeutilisateur", "typeutilisateur", "idType", "LibelleType");
            if (opp != -1)
            {
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlCommand Cmd = new MySqlCommand("select * from utilisateurs where idUser=" + opp, ClassConnexion.Macon);
                ClassConnexion.DR = Cmd.ExecuteReader();
                while (ClassConnexion.DR.Read())
                {
                    comboBox2.SelectedValue = ClassConnexion.DR[1];
                    textEdit2.Text =ClassConnexion.DR[2].ToString();
                    textEdit1.Text = ClassConnexion.DR[3].ToString();
                    textEdit3.Text = ClassConnexion.DR[4].ToString();
                    textEdit4.Text="" ;
                    textEdit5.Text = ClassConnexion.DR[6].ToString();
                    textEdit6.Text = ClassConnexion.DR[7].ToString();
                }
                    
                pseudotest = textEdit3.Text;
                MsgAn = "إسم المستخدم  " + textEdit1.Text + " " + textEdit2.Text + " - الصفة " + comboBox2.SelectedText + " - الإسم المستعار " + textEdit3.Text + " - البريد  الإلكتروني " + textEdit4.Text + " - الهاتف " + textEdit5.Text;
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag.ToString() == "Enregistrer")
            {
                if (opp == -1)
                {
                    if (comboBox2.Text == "" || textEdit2.Text == "" || textEdit1.Text == "" || textEdit3.Text == "" || textEdit4.Text == "")
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        if (textEdit4.Text.Length != 8)
                            XtraMessageBox.Show("كلمة المرور يجب أن تحتوي على 8 أحرف أو أرقام", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            if (!pseudoExiste()) {
                                ajout();
                                textEdit2.Text = textEdit1.Text = textEdit3.Text = textEdit4.Text = textEdit5.Text = textEdit6.Text = "";
                            }
                            else
                                XtraMessageBox.Show("الإسم المستعار موجود من قبل", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    if (comboBox2.Text == "" || textEdit2.Text == "" || textEdit1.Text == "" || textEdit3.Text == "" || textEdit4.Text == "")
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        if ((textEdit4.Text.Length != 8) && (textEdit4.Text.Length != 0))
                            XtraMessageBox.Show("كلمة المرور يجب أن تحتوي على 8 أحرف أو أرقام", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            if (pseudotest == textEdit3.Text) {
                                Modifier();
                                textEdit2.Text = textEdit1.Text = textEdit3.Text = textEdit4.Text = textEdit5.Text = textEdit6.Text = "";
                            }
                            else
                            {
                                if (!pseudoExiste())
                                {
                                    Modifier();
                                    textEdit2.Text = textEdit1.Text = textEdit3.Text = textEdit4.Text = textEdit5.Text = textEdit6.Text = "";
                                }
                                else
                                    XtraMessageBox.Show("الإسم المستعار موجود من قبل", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else if (e.Button.Properties.Tag.ToString() == "Annuler")
            {            
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }

        }
    }
}