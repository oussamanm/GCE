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
    public partial class AjouCrd : DevExpress.XtraEditors.XtraForm
    {
        MySqlDataAdapter da;
        DataSet ds;
        int TypeOpr = 0;
        int IdCrd;
        MySqlDataReader dr;
        string formatt = "yyyy-MM-dd HH:mm:ss";
        string AMsgE = "";
        string RequeteModi, RequeteModi2;
        int int_PaieCrd = 0;

        public AjouCrd()
        {
            InitializeComponent();
        }
        public AjouCrd(int Opr , int IdCr)
        {
            TypeOpr = Opr;
            IdCrd = IdCr;
            InitializeComponent();
        }

        private void ClearChamps()
        {
            TbLib.Text = "";
            TbDesc.Text = "";
            TbMtn.Text = "";
            DeDate.EditValue = "";

        }
        private void RempControlsData()
        {
            string strReq = "";
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            strReq = " select LibCrd,DateCrd,MontantCrd,DescCrd,PaieCrd,IdCais from credit where IdCrd = " + IdCrd + " ";
            using (MySqlCommand CmdRempChamps = new MySqlCommand(strReq, ClassConnexion.Macon))
            {
                using (dr = CmdRempChamps.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dr.Read();

                    TbLib.Text = dr[0].ToString();
                    CbCais.SelectedValue = dr[5].ToString();
                    DeDate.EditValue = dr[1].ToString();
                    TbMtn.Text = dr[2].ToString();
                    radioGroupCrd.EditValue = dr[4].ToString();
                    TbDesc.Text = dr[3].ToString();

                    int_PaieCrd = int.Parse(radioGroupCrd.EditValue.ToString());
                    string strCredit = "";
                    if (dr["PaieCrd"].ToString() == "1")
                    {
                        radioGroupCrd.SelectedIndex = 0;
                        radioGroupCrd.Enabled = false;
                        strCredit = "نعم";
                    }
                    else
                    {
                        radioGroupCrd.SelectedIndex = 1;
                        strCredit = "لا";
                    }
                    AMsgE = " تاريخ الدين : " + dr[1].ToString() + " | من طرف : " + dr[0].ToString() + " | المبلغ : " + dr[2].ToString() + " | الوصف : " + dr[3].ToString() + " | خالص : " + strCredit + " | المحفضة : " + dr[5].ToString() + "  ";                   

                }
            }
        }

        private void AjouCrd_Load(object sender, EventArgs e)
        {
            try
            {
                if (TypeOpr == 0)
                {
                    /// Ajou
                    Configuration.RempComboSimple(CbCais, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                }
                else
                {
                    /// Modif
                    Configuration.RempComboSimple(CbCais, "select IdCais,LibCais from caisse", "IdCais", "LibCais");
                    radioGroupCrd.Visible = true;
                    LbPaie.Visible = true;
                    RempControlsData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void windowsUIButtonPanelMain_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "Save")
                {
                    DateTime DateCrd;
                    if (CbCais.SelectedValue != null && DeDate.EditValue != null && TbMtn.EditValue != null && TbMtn.EditValue != null)
                    {
                        if (TypeOpr == 0)
                        {
                            // Ajou Entres

                            DateCrd = DateTime.Parse(DeDate.EditValue.ToString());

                            string RequeteAjouEntr = string.Format("insert into entrescais (IdEnt,DateEntr,MontantEntr,DescEntr,CatEntres_IdCatEntr,Caisse_IdCais,IdUser) values({0},\\'{1}\\',\\'{2}\\',\\'{3}\\',{4},{5},{6} ) ", Configuration.LastID("entrescais", "IdEnt") + 1, DateCrd.ToString(formatt), TbMtn.Text, TbDesc.Text, 5, CbCais.SelectedValue, UserConnecte.IdUser);
                            string RequeteAjouCrd = string.Format("insert into credit (LibCrd,DateCrd,MontantCrd,DescCrd,PaieCrd,IdEntr,IdCais,IdUser) values( \\'{0}\\',\\'{1}\\',\\'{2}\\',\\'{3}\\',{4},{5},{6},{7} ) ", TbLib.Text, DateCrd.ToString(formatt), TbMtn.Text, TbDesc.Text, radioGroupCrd.EditValue.ToString(), Configuration.LastID("entrescais", "IdEnt") + 1, CbCais.SelectedValue, UserConnecte.IdUser);

                            string NMsg = " من طرف : " + TbLib.Text + " | تاريخ الدين : " + DateCrd.ToString(formatt) + " | المبلغ : " + TbMtn + " | نوع المدخول : aدينa | المحفضة : " + CbCais.SelectedValue + " | الوصف : " + TbDesc.Text + "  ";
                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + "  بإضافة دين جديد   -" + DateTime.Now.ToString(formatt);
                            Configuration.Historique(1, RequeteAjouEntr, "", NMsg, MEnt, RequeteAjouCrd, "");

                            if (Configuration.Func(15) == "Indirect")
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                XtraMessageBox.Show("تمة الإضافة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearChamps();
                        }
                        else if (TypeOpr == 1)
                        {
                            /// Modif Entres
                            DateCrd = DateTime.Parse(DeDate.EditValue.ToString());
                            string NMsg = "";

                            
                            if (int_PaieCrd.ToString() == radioGroupCrd.EditValue.ToString())
                            {
                                RequeteModi = string.Format("UPDATE credit SET LibCrd=\\'{0}\\',DateCrd=\\'{1}\\',MontantCrd=\\'{2}\\',DescCrd=\\'{3}\\'  WHERE IdCrd=" + IdCrd + " ", TbLib.Text, DateCrd.ToString(formatt), TbMtn.Text, TbDesc.Text);
                                RequeteModi2 = "";
                                NMsg = " من طرف : " + TbLib.Text + " | تاريخ الدين : " + DateCrd.ToString(formatt) + " | المبلغ : " + TbMtn + " | نوع المدخول : aدينa | المحفضة : " + CbCais.SelectedValue + " | الوصف : " + TbDesc.Text + "  ";
                            }
                            else
                            {
                                if (radioGroupCrd.EditValue.ToString()== "1")
                                {
                                     RequeteModi2 = string.Format("UPDATE credit SET LibCrd=\\'{0}\\',DateCrd=\\'{1}\\',MontantCrd=\\'{2}\\',DescCrd=\\'{3}\\',PaieCrd={4},DatePaieCrd=\\'{5}\\',IdSort={6} WHERE IdCrd=" + IdCrd + " ", TbLib.Text, DateCrd.ToString(formatt), TbMtn.Text, TbDesc.Text, radioGroupCrd.EditValue.ToString(), DateTime.Now.ToString(formatt), Configuration.LastID("sortiescais", "IdSort")+1);
                                     RequeteModi = string.Format("insert into sortiescais(IdSort,DateSort,MontantSort,DescSort,CatSorties_IdCatSort,Caisse_IdCais,IdUser) values ({0},\\'{1}\\',\\'{2}\\',\\'{3}\\',{4},{5},{6})",Configuration.LastID("sortiescais","IdSort")+1, DateTime.Now.ToString(formatt), TbMtn.Text, TbDesc.Text,1,CbCais.SelectedValue,UserConnecte.IdUser);
                                    NMsg = " من طرف : " + TbLib.Text + " | تاريخ الدين : " + DateCrd.ToString(formatt) + " | المبلغ : " + TbMtn + " | نوع المدخول : aدينa | المحفضة : " + CbCais.SelectedValue + " | الوصف : " + TbDesc.Text + "  | خالص :"+radioGroupCrd.Properties.Items[radioGroupCrd.SelectedIndex].Description+ "| بتاريخ : "+ DateTime.Now.ToString(formatt) + "  ";
                                }
                            }

                            string MEnt = " قام " + UserConnecte.NomUser + " " + UserConnecte.PrenomUser + " بتعديل في الديون   - " + DateTime.Now.ToString(formatt);
                            Configuration.Historique(0, RequeteModi, AMsgE, NMsg, MEnt, RequeteModi2, "");

                            if (Configuration.Func(15) == "Indirect")
                                XtraMessageBox.Show("تم الإرسال بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                XtraMessageBox.Show("تم التعديل بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("خطأ في إدخال المعلومات", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (e.Button.Properties.Tag == "Close")
                {
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