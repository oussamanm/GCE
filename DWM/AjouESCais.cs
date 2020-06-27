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
    public partial class AjouESCais : DevExpress.XtraEditors.XtraForm
    {

        int int_TypeEntr, int_TypeOpr,IdReq = 0;
        MySqlDataReader dr;
        string formatt = "yyyy-MM-dd HH:mm:ss";
        string AMsgE = "";
        public AjouESCais()
        {
            InitializeComponent();
        }
        public AjouESCais(int TypeEntr,int TypeOpr)
        {
            int_TypeEntr = TypeEntr;
            int_TypeOpr = TypeOpr;
            InitializeComponent();
        }
        public AjouESCais(int TypeEntr, int TypeOpr,int IdE)
        {
            int_TypeEntr = TypeEntr;
            int_TypeOpr = TypeOpr;
            IdReq = IdE;
            InitializeComponent();
        }
        public void RempChampPourModEntr()
        {
            string strReq = "";
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            if (int_TypeEntr ==1)
            {
                strReq = " select DateEntr,MontantEntr,CatEntres_IdCatEntr,Caisse_IdCais,DescEntr from entrescais where IdEnt = " + IdReq + " ";
            }
            else if(int_TypeEntr ==2)
            {
                //strReq = " select DateSort,MontantSort,CatSorties_IdCatSort,Caisse_IdCais,DescSort,CrediSort from sortiescais where IdSort = " + IdReq + " ";
                strReq = " select DateSort,MontantSort,CatSorties_IdCatSort,Caisse_IdCais,DescSort from sortiescais where IdSort = " + IdReq + " ";
            }

            using (MySqlCommand CmdRempChamps = new MySqlCommand(strReq,ClassConnexion.Macon))
            {
                using ( dr  = CmdRempChamps.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dr.Read();
                    
                    dateEditDateES.EditValue = dr[0].ToString();
                    textEditMtn.EditValue = dr[1].ToString();
                    comboBoxCatES.SelectedValue = dr[2].ToString();
                    comboBoxCais.SelectedValue = dr[3].ToString();
                    textBoxDesc.Text = dr[4].ToString();

                    if (int_TypeEntr == 1)
                        AMsgE = " تاريخ المدخول : " + dr[0].ToString() + " | المبلغ : " + dr[1].ToString() + " | نوع المدخول : " + dr[2].ToString() + " | المحفضة : " + dr[3].ToString() + " | الوصف : " + dr[4].ToString() + "  ";
                    else if (int_TypeEntr == 2)
                    {
                        //string strCredit="";

                        //if(dr["CrediSort"].ToString()== "0")
                        //{
                        //    radioGroupPaieSort.SelectedIndex = 0;
                        //    radioGroupPaieSort.Enabled = false;
                        //    strCredit = "نعم";
                        //}
                        //else
                        //{
                        //    radioGroupPaieSort.SelectedIndex = 1;
                        //    strCredit = "لا";
                        //}

                        AMsgE = " تاريخ المصروف : " + dr[0].ToString() + " | المبلغ : " + dr[1].ToString() + " | نوع المصروف : " + dr[2].ToString() + " | المحفضة : " + dr[3].ToString() + " | الوصف : " + dr[4].ToString() + " ";
                    }

                }
            }
        }
        public void ClearChamps()
        {
            dateEditDateES.EditValue = "";
            textBoxDesc.Text = "";
            textEditMtn.Text = "";
        }


        ///// Load
        private void AjouESCais_Load(object sender, EventArgs e)
        {
            try
            {
                if (int_TypeEntr == 1)
                {
                    /// Ajou EntresCais
                    
                    if (int_TypeOpr == 1)
                    {
                        Configuration.RempComboSimple(comboBoxCais, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                        Configuration.RempComboSimple(comboBoxCatES, "select IdCatEntr,LibCatEntr from catentres where VisibCatEntr=1", "IdCatEntr", "LibCatEntr");
                    }
                    else if (int_TypeOpr == 2)
                    {
                        Configuration.RempComboSimple(comboBoxCais, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                        Configuration.RempComboSimple(comboBoxCatES, "select IdCatEntr,LibCatEntr from catentres where VisibCatEntr=1", "IdCatEntr", "LibCatEntr");
                        RempChampPourModEntr();
                    }
                }
                else if (int_TypeEntr == 2)
                {
                    labelEntet.Text = "إضافة مصروف جديد";
                    labelCatES.Text = ":       نوع المصروف";
                    //labelPaieSort.Visible = true;
                    //radioGroupPaieSort.Visible = true;

                    if (int_TypeOpr == 1)
                    {
                        Configuration.RempComboSimple(comboBoxCais, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                        Configuration.RempComboSimple(comboBoxCatES, "select IdCatSort,LibCatSort from catsorties where VisibCatSort=1", "IdCatSort", "LibCatSort");
                    }
                    else if (int_TypeOpr == 2)
                    {
                        Configuration.RempComboSimple(comboBoxCais, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                        Configuration.RempComboSimple(comboBoxCatES, "select IdCatSort,LibCatSort from catsorties where VisibCatSort=1", "IdCatSort", "LibCatSort");
                        RempChampPourModEntr();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        ///// Code
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Enregistrer")
                {
                    DateTime DateEntr;

                    if (comboBoxCais.SelectedValue != null && comboBoxCatES.SelectedValue != null  && dateEditDateES.EditValue != null && textEditMtn.EditValue != null)
                    {
                        if (int_TypeEntr == 1)
                        {
                            if (int_TypeOpr == 1)
                            {
                                /// Ajou Entres
                                DateEntr = DateTime.Parse(dateEditDateES.EditValue.ToString());

                                string RequeteAjou = string.Format("insert into entrescais (DateEntr,MontantEntr,DescEntr,IdSrcEntr,CatEntres_IdCatEntr,Caisse_IdCais,IdUser) values( \\'{0}\\',\\'{1}\\',\\'{2}\\',0,{3},{4},{5} ) ", DateEntr.ToString(formatt), textEditMtn.EditValue, textBoxDesc.Text, comboBoxCatES.SelectedValue, comboBoxCais.SelectedValue, UserConnecte.IdUser);
                                string NMsg = " تاريخ المدخول : " + DateEntr.ToString(formatt) + " | المبلغ : " + textEditMtn.EditValue + " | نوع المدخول : " + comboBoxCatES.SelectedValue + " | المحفضة : " + comboBoxCais.SelectedValue + " | الوصف : " + textBoxDesc.Text + "  ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة مدخول جديد   -" + DateTime.Now.ToString(formatt);
                                Configuration.Historique(1, RequeteAjou, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    XtraMessageBox.Show("تمة الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ClearChamps();
                            }
                            else if (int_TypeOpr == 2)
                            {
                                /// Modif Entres
                                DateEntr = DateTime.Parse(dateEditDateES.EditValue.ToString());

                                string RequeteModi = string.Format("UPDATE entrescais SET DateEntr=\\'{0}\\',MontantEntr=\\'{1}\\',DescEntr=\\'{2}\\',CatEntres_IdCatEntr={3},Caisse_IdCais={4},IdUser={5} WHERE IdEnt=" + IdReq + " ", DateEntr.ToString(formatt), textEditMtn.EditValue, textBoxDesc.Text, comboBoxCatES.SelectedValue, comboBoxCais.SelectedValue, UserConnecte.IdUser);
                                string NMsg = " تاريخ المدخول : " + DateEntr.ToString(formatt) + " | المبلغ : " + textEditMtn.EditValue + " | نوع المدخول : " + comboBoxCatES.SelectedValue + " | المحفضة : " + comboBoxCais.SelectedValue + " | الوصف : " + textBoxDesc.Text + "  ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل في المدخول   - " + DateTime.Now.ToString(formatt);
                                Configuration.Historique(0, RequeteModi, AMsgE, NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (int_TypeEntr == 2)
                        {
                            //string SelectedPaieSort = "";

                            if (int_TypeOpr == 1)
                            {
                                /// Ajou Sorties
                                DateEntr = DateTime.Parse(dateEditDateES.EditValue.ToString());
                                //if (radioGroupPaieSort.EditValue == "0")
                                //    SelectedPaieSort = "نعم";
                                //else
                                //    SelectedPaieSort = "لا";

                                //string RequeteAjou = string.Format("insert into sortiescais (DateSort,MontantSort,DescSort,CatSorties_IdCatSort,Caisse_IdCais,CrediSort,IdUser) values( \\'{0}\\',\\'{1}\\',\\'{2}\\',{3},{4},{5},{6} ) ", DateEntr.ToString(formatt), textEditMtn.EditValue, textBoxDesc.Text, comboBoxCatES.SelectedValue, comboBoxCais.SelectedValue, radioGroupPaieSort.EditValue, UserConnecte.IdUser);
                                string RequeteAjou = string.Format("insert into sortiescais (DateSort,MontantSort,DescSort,CatSorties_IdCatSort,Caisse_IdCais,IdUser) values( \\'{0}\\',\\'{1}\\',\\'{2}\\',{3},{4},{5} ) ", DateEntr.ToString(formatt), textEditMtn.EditValue, textBoxDesc.Text, comboBoxCatES.SelectedValue, comboBoxCais.SelectedValue, UserConnecte.IdUser);
                                string NMsg = " تاريخ المصروف : " + DateEntr.ToString(formatt) + " | المبلغ : " + textEditMtn.EditValue + " | نوع المصروف : " + comboBoxCatES.SelectedValue + " | المحفضة : " + comboBoxCais.SelectedValue + " | الوصف : " + textBoxDesc.Text + " ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة مصروف جديد   -" + DateTime.Now.ToString(formatt);
                                Configuration.Historique(1, RequeteAjou, "", NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    XtraMessageBox.Show("تمة الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ClearChamps();
                            }
                            else if (int_TypeOpr == 2)
                            {
                                /// Modif Sorties
                                DateEntr = DateTime.Parse(dateEditDateES.EditValue.ToString());
                                //if (radioGroupPaieSort.EditValue == "0")
                                //    SelectedPaieSort = "نعم";
                                //else
                                //    SelectedPaieSort = "لا";

                                //string RequeteModi = string.Format("UPDATE sortiescais SET DateSort=\\'{0}\\',MontantSort=\\'{1}\\',DescSort=\\'{2}\\',CatSorties_IdCatSort={3},Caisse_IdCais={4},IdUser={5},CrediSort={6} WHERE IdSort=" + IdReq + " ", DateEntr.ToString(formatt), textEditMtn.EditValue, textBoxDesc.Text, comboBoxCatES.SelectedValue, comboBoxCais.SelectedValue, UserConnecte.IdUser, radioGroupPaieSort.EditValue);
                                string RequeteModi = string.Format("UPDATE sortiescais SET DateSort=\\'{0}\\',MontantSort=\\'{1}\\',DescSort=\\'{2}\\',CatSorties_IdCatSort={3},Caisse_IdCais={4},IdUser={5} WHERE IdSort=" + IdReq + " ", DateEntr.ToString(formatt), textEditMtn.EditValue, textBoxDesc.Text, comboBoxCatES.SelectedValue, comboBoxCais.SelectedValue, UserConnecte.IdUser);
                                string NMsg = " تاريخ المصروف : " + DateEntr.ToString(formatt) + " | المبلغ : " + textEditMtn.EditValue + " | نوع المصروف : " + comboBoxCatES.SelectedValue + " | المحفضة : " + comboBoxCais.SelectedValue + " | الوصف : " + textBoxDesc.Text + " ";
                                string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل في المصروف   - " + DateTime.Now.ToString(formatt);
                                Configuration.Historique(0, RequeteModi, AMsgE, NMsg, MEnt, "", "");

                                if (Configuration.Func(15) == "Indirect")
                                    XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (e.Button.Properties.Tag == "Annuler")
                {
                    ClearChamps();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}