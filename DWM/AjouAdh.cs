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
    public partial class AjouAdh : DevExpress.XtraEditors.XtraForm
    {
        int IdAdherent = 0;
        string formatt = "yyyy-MM-dd HH:mm:ss";
        string AMsg;

        public AjouAdh()
        {
            InitializeComponent();
        }
        public AjouAdh(int IdAdh)
        {
            IdAdherent = IdAdh;
            InitializeComponent();
        }
        void Clear()
        {
            foreach (Control textEdit in this.Controls)
            {
                if (textEdit is TextEdit)
                {
                    (textEdit as TextEdit).ResetText();
                }
                if (textEdit is DateEdit)
                {
                    (textEdit as DateEdit).ResetText();
                }
            }
        }


        private void AjouAdh_Load(object sender, EventArgs e)
        {
            if (IdAdherent == 0)
            {
                radioGroupNumAdh.Enabled = true;
                radioGroupNumAdh.SelectedIndex = 0;
            }
            else
            {
                radioGroupNumAdh.SelectedIndex = 1;
                radioGroupNumAdh.Enabled = false;
                TbNumO.Enabled = false;

                ClassConnexion.Macon.Open();

                MySqlCommand Cmd = new MySqlCommand("select * from adherent where IdAdherent=" + IdAdherent + "", ClassConnexion.Macon);
                ClassConnexion.DR = Cmd.ExecuteReader();

                ClassConnexion.DR.Read();

                TbNumO.Text = ClassConnexion.DR[0].ToString();
                TbNom.Text = ClassConnexion.DR[1].ToString();
                TbPre.Text = ClassConnexion.DR[2].ToString();
                TbNas.Text = ClassConnexion.DR[3].ToString();
                TbIsm.Text = ClassConnexion.DR[4].ToString();
                TbCIN.Text = ClassConnexion.DR[5].ToString();
                if (ClassConnexion.DR[6].ToString() == "False")
                {
                    radioGroup1.SelectedIndex = 1;
                }
                else if (ClassConnexion.DR[6].ToString() == "True")
                {
                    radioGroup1.SelectedIndex = 0;
                }
                dateEdit1.Text = ClassConnexion.DR[7].ToString();
                dateEdit2.Text = ClassConnexion.DR[8].ToString();
                TbVil.Text = ClassConnexion.DR[9].ToString();
                if (ClassConnexion.DR[10].ToString() == "False")
                {
                    radioGroup2.SelectedIndex = 1;
                }
                else if (ClassConnexion.DR[10].ToString() == "True")
                {
                    radioGroup2.SelectedIndex = 0;
                }

                AMsg = "الرقم الترتيبي : " + TbNumO.Text + " | Nom : " + TbNom.Text + " | Prénom : " + TbPre.Text + " | النسب : " + TbNas.Text + " | الإسم : " + TbIsm.Text + " | ر.ب.و : " + TbCIN.Text + " | الجنس : " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description + " | تاريخ الإنخراط : " + dateEdit1.EditValue + " | تاريخ الإزدياد : " + dateEdit2.EditValue + " | مكان الإزدياد : " + TbVil.Text + " | حالة المنخرط : " + radioGroup2.Properties.Items[radioGroup2.SelectedIndex].Description;
                ClassConnexion.Macon.Close();
            }
        }

        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag.ToString() == "Enregistrer")
            {
                try
                {
                    string RequeteAjou;

                    if (IdAdherent == 0)
                    {
                        DateTime DateT1, DateT2,DateT3;
                        DateT1 = DateTime.Parse(dateEdit1.EditValue.ToString());
                        DateT2 = DateTime.Parse(dateEdit2.EditValue.ToString());


                        if (radioGroupNumAdh.SelectedIndex == 0)
                        {
                            if (TbCIN.Text == "" || TbIsm.Text == "" || TbNas.Text == "" || TbNom.Text == "" || TbPre.Text == "")
                            {
                                XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            RequeteAjou = string.Format("insert into adherent values(0,\\'{0}\\',\\'{1}\\',\\'{2}\\',\\'{3}\\',\\'{4}\\',{5},\\'{6}\\',\\'{7}\\',\\'{8}\\',{9},0,{10},\\'{11}\\',\\'{12}\\') ", TbNom.Text, TbPre.Text, TbIsm.Text, TbNas.Text, TbCIN.Text, radioGroup1.EditValue, DateT1.ToString(formatt), DateT2.ToString(formatt), TbVil.Text, radioGroup2.EditValue, UserConnecte.IdUser, DateTime.Now.ToString(formatt), password.getMd5Hash(TbCIN.Text));
                        }
                        else
                        {
                            if ( TbNumO.Text=="" || TbCIN.Text == "" || TbIsm.Text == "" || TbNas.Text == "" || TbNom.Text == "" || TbPre.Text == "")
                            {
                                XtraMessageBox.Show("المرجو إدخال جميع المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (Configuration.ExisteEnre("adherent", "IdAdherent", TbNumO.EditValue.ToString()) != 0)
                            {
                                XtraMessageBox.Show("رقم المنخرط موجود من قبل المرجو تغييره", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            RequeteAjou = string.Format("insert into adherent values({0},\\'{1}\\',\\'{2}\\',\\'{3}\\',\\'{4}\\',\\'{5}\\',{6},\\'{7}\\',\\'{8}\\',\\'{9}\\',{10},0,{11},\\'{12}\\',\\'{13}\\') ", TbNumO.Text, TbNom.Text, TbPre.Text, TbIsm.Text, TbNas.Text, TbCIN.Text, radioGroup1.EditValue, DateT1.ToString(formatt), DateT2.ToString(formatt), TbVil.Text, radioGroup2.EditValue, UserConnecte.IdUser, DateTime.Now.ToString(formatt), password.getMd5Hash(TbCIN.Text));
                        }

                        string NMsg = "الرقم الترتيبي : " + TbNumO.Text + " | Nom : " + TbNom.Text + " | Prénom : " + TbPre.Text + " | النسب : " + TbNas.Text + " | الإسم : " + TbIsm.Text + " | ر.ب.و : " + TbCIN.Text + " | الجنس : " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description + " | تاريخ الإنخراط : " + dateEdit1.EditValue.ToString() + " | تاريخ الإزدياد : " + dateEdit2.EditValue.ToString() + " | مكان الإزدياد : " + TbVil.Text + " | حالة المنخرط : " + radioGroup2.Properties.Items[radioGroup2.SelectedIndex].Description;
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بإضافة منخرط جديد  - " + DateTime.Now.ToString(formatt);

                        Configuration.Historique(1, RequeteAjou, "", NMsg, MEnt, "", "");

                        if (Configuration.Func(15) == "Indirect")
                        {
                            XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (Configuration.Func(15) == "Direct")
                        {
                            XtraMessageBox.Show(" تمت الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        Clear();

                    }
                    else
                    {
                        if (TbCIN.Text == "" || TbIsm.Text == "" || TbNas.Text == "" || TbNom.Text == "" || TbPre.Text == "")
                        {
                            XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        DateTime DateT1, DateT2;
                        DateT1 = DateTime.Parse(dateEdit1.EditValue.ToString());
                        DateT2 = DateTime.Parse(dateEdit2.EditValue.ToString());

                        string RequeteModi = string.Format("UPDATE adherent SET NomFrAdhe=\\'{0}\\',PrenomFrAdhe=\\'{1}\\',NomArAdhe=\\'{2}\\',PrenomArAdhe=\\'{3}\\',CINAdhe=\\'{4}\\',SexAdhe={5},DateInscAdhe=\\'{6}\\',DateNaissAdhe=\\'{7}\\',LieuNaissAdhe=\\'{8}\\',DeceAdhe={9},IdUser={10},DateModAdh=\\'{11}\\' WHERE IdAdherent=" + IdAdherent + " ", TbNom.Text, TbPre.Text, TbIsm.Text, TbNas.Text, TbCIN.Text, radioGroup1.EditValue, DateT1.ToString(formatt), DateT2.ToString(formatt), TbVil.Text, radioGroup2.EditValue, UserConnecte.IdUser, DateTime.Now.ToString(formatt));

                        string NMsg = "الرقم الترتيبي : " + TbNumO.Text + " | Nom : " + TbNom.Text + " | Prénom : " + TbPre.Text + " | النسب : " + TbNas.Text + " | الإسم : " + TbIsm.Text + " | ر.ب.و : " + TbCIN.Text + " | الجنس : " + radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description + " | تاريخ الإنخراط : " + dateEdit1.EditValue + " | تاريخ الإزدياد : " + dateEdit2.EditValue + " | مكان الإزدياد : " + TbVil.Text + " | حالة المنخرط : " + radioGroup2.Properties.Items[radioGroup2.SelectedIndex].Description;
                        string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل منخرط   - " + DateTime.Now.ToString(formatt);

                        Configuration.Historique(0, RequeteModi, AMsg, NMsg, MEnt, "", "");

                        if (Configuration.Func(15) == "Indirect")
                        {
                            XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (Configuration.Func(15) == "Direct")
                        {
                            XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        Clear();
                        this.Close();
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

        private void radioGroupNumAdh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupNumAdh.SelectedIndex==0)
            {
                TbNumO.Text = "";
                TbNumO.Enabled = false;
            }
            else
            {
                TbNumO.Text = "";
                TbNumO.Enabled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime DateT1, DateT2;
            DateT1 = DateTime.Parse(dateEdit1.EditValue.ToString());
        }
    }
}