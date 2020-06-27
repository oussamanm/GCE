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
using DevExpress.XtraBars.Navigation;

namespace DWM
{
    public partial class FormMenu : DevExpress.XtraEditors.XtraForm
    {
        public FormMenu()
        {
            InitializeComponent();
        }
        DataSet ds;
        MySqlDataReader dr;
        MySqlDataAdapter da, daa;
        MySqlCommandBuilder builderda, builderdaa;
        string forma = "yyyy-MM-dd HH:mm:ss";
        int vartest=0;
        int img = 0;
        string str_TypeEntr;

        public void RempNotifi()
        {
            try
            {
                if (str_TypeEntr == "Direct")
                    flyoutPanel1.Visible = simpleButton1.Visible = false;
                else if(str_TypeEntr == "Indirect")
                {
                    ///**********************************************///
                    ds = new DataSet();

                    if (tileBarGroup1.Items.Count>0)
                        tileBarGroup1.Items.Clear();

                    if (tileBarGroup2.Items.Count>0)
                        tileBarGroup2.Items.Clear();

                    da = new MySqlDataAdapter("SELECT * FROM  historiquemod WHERE historiquemod.IdUserEff in(select IdUser from utilisateurs where IdType != "+UserConnecte.IdType+" )  and IdUserEff != "+UserConnecte.IdUser+" AND IdUserVal = 0 ", ClassConnexion.Macon);
                    da.Fill(ds, "TableMsgR");
                    builderda = new MySqlCommandBuilder(da);
                    for (int i = 0; i < ds.Tables["TableMsgR"].Rows.Count; i++)
                    {
                        /// Declare data Row ////
                        DataRow item = ds.Tables["TableMsgR"].Rows[i];
                    
                        /// Declare TileItemElement ///
                        TileItemElement tileItemElementt = new TileItemElement();
                        tileItemElementt.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopRight;
                        tileItemElementt.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        tileItemElementt.Appearance.Normal.ForeColor = System.Drawing.Color.DimGray;

                        /// Declare TileBarItem ///
                        TileBarItem tilebarr = new TileBarItem();
                        tilebarr.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
                        tilebarr.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
                        tilebarr.AppearanceItem.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
                        tilebarr.ShowItemShadow = DevExpress.Utils.DefaultBoolean.True;
                        tilebarr.Elements.Add(tileItemElementt);

                        /// Remplire TileItem ///
                        tileItemElementt.Text = item["MsgEn"].ToString();
                        tilebarr.Tag = i.ToString();
                        tilebarr.Id = int.Parse(item["TypeOpr"].ToString());

                        tileBarGroup1.Items.Add(tilebarr);
                    }

                    ///**********************************************///

                    daa = new MySqlDataAdapter("SELECT * FROM  historiquemod WHERE IdUserEff = " + UserConnecte.IdUser + " AND IdUserVal = 0", ClassConnexion.Macon);
                    daa.Fill(ds, "TableMsgE");
                    builderdaa = new MySqlCommandBuilder(daa);

                    for (int i = 0; i < ds.Tables["TableMsgE"].Rows.Count; i++)
                    {
                        /// Declare data Row ////
                        DataRow item = ds.Tables["TableMsgE"].Rows[i];

                        /// Declare TileItemElement ///
                        TileItemElement tileItemElementt = new TileItemElement();
                        tileItemElementt.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopRight;
                        tileItemElementt.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        tileItemElementt.Appearance.Normal.ForeColor = System.Drawing.Color.DimGray;

                        /// Declare TileBarItem ///
                        TileBarItem tilebarr = new TileBarItem();
                        tilebarr.Elements.Add(tileItemElementt);
                        tilebarr.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
                        tilebarr.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
                        tilebarr.AppearanceItem.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
                        tilebarr.ShowItemShadow = DevExpress.Utils.DefaultBoolean.True;

                        /// Remplire TileItem ///
                        tileItemElementt.Text = item["MsgEn"].ToString();
                        tilebarr.Tag = i.ToString();
                        tilebarr.Id = int.Parse(item["TypeOpr"].ToString());

                        tileBarGroup2.Items.Add(tilebarr);
                    }
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public float count(string requete)
        {
            float resultat = 0;
            try
            {
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlDataReader DR;
                MySqlCommand CMD = new MySqlCommand(requete, ClassConnexion.Macon);
                DR = CMD.ExecuteReader();
                DR.Read();
                if (DR.HasRows)
                    resultat = float.Parse(DR[0].ToString());
                else
                    resultat = 0;

                DR.Close();
                ClassConnexion.Macon.Close();
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }
            return resultat;
        }
        private void FormMenu_Load(object sender, EventArgs e)
        {
            try
            {
                Configuration.TestIn();

                // Message Eclairage
                if (Configuration.Func(20) == "0")
                {
                    tileItemEclai.Visible = false;
                    tileGroup4.Visible = false;
                }
                else
                {
                    tileGroup4.Visible = true;
                    tileItemEclai.Visible = true;
                    tileItemFinan.ItemSize = TileItemSize.Wide;
                    tileItemTraite.ItemSize = TileItemSize.Wide;
                    tileGroup4.Items.Add(tileItemTraite);

                    int DerFact = Configuration.LastID("facture", "IdFact");
                    int nbrSecteur = int.Parse(count("select count(*) from secteur").ToString());
                    int nbrEclairage = int.Parse(count("select count(*) from eclairage where IdFct=" + DerFact).ToString());

                    if (nbrSecteur != nbrEclairage)
                        timer2.Start();
                }

                str_TypeEntr = Configuration.Func(15);

                //تاريخ بداية إدخال المعطيات

                if (str_TypeEntr == "Direct")
                {
                    flyoutPanel1.Visible = false;
                    simpleButton1.Visible = false;
                }
                else if (str_TypeEntr == "Indirect")
                {
                    simpleButton1.Visible = true;
                }

                ClassConnexion.Macon.Open();
                MySqlCommand Alertnot = new MySqlCommand("SELECT * FROM  historiquemod WHERE historiquemod.IdUserEff in(select IdUser from utilisateurs where IdType != " + UserConnecte.IdType + " )  and IdUserEff != " + UserConnecte.IdUser + " AND IdUserVal = 0",ClassConnexion.Macon);
                dr = Alertnot.ExecuteReader();

                dr.Read();

                if (dr.HasRows)
                {
                    vartest = 1;
                }
                ClassConnexion.Macon.Close();
                dr.Close();

                if (vartest==1)
                {
                    timer1.Interval = 300;
                    timer1.Start();
                }

                DateTime date1 = new DateTime(2000, DateTime.Now.Month, int.Parse(Configuration.Func(5)));
                DateTime date3 = new DateTime(2000, DateTime.Now.Month, int.Parse(Configuration.Func(5)));
                DateTime date2 = new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day);
                date3 = date3.AddDays(int.Parse(Configuration.Func(6)));

                int result = DateTime.Compare(date2, date1);
                int result2 = DateTime.Compare(date3, date1);

                if (date2 >= date1 && date2 < date3)
                {
                    tileItemConso.Enabled = true;
                }
                else
                {
                    tileItemConso.Enabled = false;
                }


                /////Desactiver ItemTileBar /////

                if (UserConnecte.IdType == 4)
                {
                    simpleButton1.Visible = false;
                    tileItemAdher.Enabled = false;
                    tileItemUser.Enabled = false;
                    tileItemCont.Enabled = false;
                    tileItemPena.Enabled = false;
                    tileItemEntr.Enabled = false;
                    tileItemFinan.Enabled = false;
                    tileItemPara.Enabled = false;
                }
                else if (UserConnecte.IdType == 3)
                {
                    simpleButton1.Visible = false;
                    tileItemAdher.Enabled = false;
                    tileItemUser.Enabled = false;
                    tileItemCont.Enabled = false;
                    tileItemPena.Enabled = false;
                    tileItemEntr.Enabled = false;
                    tileItemPara.Enabled = false;
                    tileItemConso.Enabled = false;
                    tileItemFact.Enabled = false;
                    tileItemPaiem.Enabled = false;
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }

            //تاريخ بداية إدخال المعطيات  
        }

        private void panelControlRedui_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panelControl1_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Login LogFORM = new Login();
            LogFORM.Show();
            this.Close();
            splashScreenManager1.CloseWaitForm();       
        }
        private void panelControl3_Click(object sender, EventArgs e)
        {
            Sleep FSleep = new Sleep();
            FSleep.ShowDialog(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (img==1)
            {
                simpleButton1.Image = Properties.Resources.icons8_Notification_Filled_32;
                img = 0;
            }
            else
            {
                simpleButton1.Image = Properties.Resources.Notification_32px;
                img = 1;
            }
        }

        private void tileItemAdher_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Adherents ADHFORM = new Adherents();
            ADHFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemConso_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            ConsommationMensuelle ConFORM = new ConsommationMensuelle();
            ConFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemCont_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Compteurs ComFORM = new Compteurs();
            ComFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemFinan_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Statistique Stati = new Statistique();
            Stati.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemPaiem_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Paiement ConFORM = new Paiement();
            ConFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemFact_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Factures FactFORM = new Factures();
            FactFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemPena_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Penalites PenaFORM = new Penalites();
            PenaFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemEntrSortCais_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            EntSortCais ESCais = new EntSortCais();
            ESCais.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemUser_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Utilisateurs utlFORM = new Utilisateurs();
            utlFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemEntr_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Entries EntriesFORM = new Entries();
            EntriesFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemPara_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Config ConfigFORM = new Config();
            ConfigFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemTraite_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            triates triatesFORM = new triates();
            triatesFORM.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }
        private void tileItemEclai_ItemClick(object sender, TileItemEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            Eclairage Eclai = new Eclairage();
            Eclai.Show();

            this.Hide();
            splashScreenManager1.CloseWaitForm();
        }


        //// ******* Buttons Show Hide *******////
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RempNotifi();
            flyoutPanel1.ShowPopup();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            flyoutPanel1.HideBeakForm();
        }

        //// ******** tile Items Notifications ****////
        private void tileBar1_ItemClick(object sender, TileItemEventArgs e)
        {
            e.Item.Checked = true;
        }
        private void tileBar2_ItemClick(object sender, TileItemEventArgs e)
        {
            e.Item.Checked = true;
        }

        private void tileBar1_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            try
            {
                string Req,Req1,Req2 ;
                if (e.Item.Name == "ContextButton1")
                {
                    foreach (TileBarItem item in tileBarGroup1.Items)
                    {
                        if (item.Checked == true)
                        {
                            DialogResult Dialog = XtraMessageBox.Show("هل تريد فعلا الموافقة", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (Dialog == DialogResult.Yes)
                            {
                                Req = ds.Tables["TableMsgR"].Rows[int.Parse(item.Tag.ToString())]["Requete"].ToString();
                                Req1 = ds.Tables["TableMsgR"].Rows[int.Parse(item.Tag.ToString())]["RequteSupp1"].ToString();
                                Req2 = ds.Tables["TableMsgR"].Rows[int.Parse(item.Tag.ToString())]["RequeteSupp2"].ToString();
                                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                                    ClassConnexion.Macon.Open();

                                MySqlCommand CmdApplRequ = new MySqlCommand(Req, ClassConnexion.Macon);
                                MySqlCommand CmdApplRequ1 = new MySqlCommand(Req1, ClassConnexion.Macon);
                                MySqlCommand CmdApplRequ2 = new MySqlCommand(Req2, ClassConnexion.Macon);

                                CmdApplRequ.ExecuteNonQuery();
                                if (Req1!= "")
                                    CmdApplRequ1.ExecuteNonQuery();
                                if (Req2!="")
                                    CmdApplRequ2.ExecuteNonQuery();

                                ClassConnexion.Macon.Close();

                                ds.Tables["TableMsgR"].Rows[int.Parse(item.Tag.ToString())]["IdUserVal"] = UserConnecte.IdUser;
                                ds.Tables["TableMsgR"].Rows[int.Parse(item.Tag.ToString())]["DateVal"] = DateTime.Now.ToString(forma);

                                da.Update(ds, "TableMsgR");
                                ds.AcceptChanges();
                                XtraMessageBox.Show("تمت الموافقة بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                flyoutPanel1.HideBeakForm();
                            }
                        }
                    }

                }
                else if (e.Item.Name == "ContextButton2")
                {
                    foreach (TileBarItem item in tileBarGroup1.Items)
                    {
                        if (item.Checked == true)
                        {
                            DialogResult Dialog = XtraMessageBox.Show("هل تريد فعلا الموافقة", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (Dialog == DialogResult.Yes)
                            {
                                ds.Tables["TableMsgR"].Rows[int.Parse(item.Tag.ToString())].Delete();

                                da.Update(ds, "TableMsgR");
                                ds.AcceptChanges();

                                XtraMessageBox.Show("تم الإلغاء بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                flyoutPanel1.HideBeakForm();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void tileBar2_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            if (e.Item.Name == "ContextButton2")
            {
                foreach (TileBarItem item in tileBarGroup2.Items)
                {
                    if (item.Checked == true)
                    {
                        DialogResult Dialog = XtraMessageBox.Show("هل تريد فعلا الموافقة", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (Dialog == DialogResult.Yes)
                        {
                            ds.Tables["TableMsgE"].Rows[int.Parse(item.Tag.ToString())].Delete();

                            daa.Update(ds, "TableMsgE");
                            ds.AcceptChanges();

                            flyoutPanel1.HideBeakForm();
                        }
                    }
                }
            }
        }

        ///******* Double Click Item ******///
        private void tileBar1_ItemDoubleClick(object sender, TileItemEventArgs e)
        {
            if (e.Item.Id==1)
            {
                InfoHisto FrmInfo = new InfoHisto(e.Item.Text, ds.Tables["TableMsgR"].Rows[int.Parse(e.Item.Tag.ToString())]["MsgNo"].ToString());
                FrmInfo.ShowDialog();
            }
            else if (e.Item.Id == 0)
            {
                InfoHisto FrmInfo = new InfoHisto(e.Item.Text, ds.Tables["TableMsgR"].Rows[int.Parse(e.Item.Tag.ToString())]["MsgAn"].ToString(), ds.Tables["TableMsgR"].Rows[int.Parse(e.Item.Tag.ToString())]["MsgNo"].ToString());
                FrmInfo.ShowDialog();
            }
        }
        private void tileBar2_ItemDoubleClick(object sender, TileItemEventArgs e)
        {
            if (e.Item.Id == 1)
            {
                InfoHisto FrmInfo = new InfoHisto(e.Item.Text, ds.Tables["TableMsgE"].Rows[int.Parse(e.Item.Tag.ToString())]["MsgNo"].ToString());
                FrmInfo.ShowDialog();
            }
            else if (e.Item.Id == 0)
            {
                InfoHisto FrmInfo = new InfoHisto(e.Item.Text, ds.Tables["TableMsgE"].Rows[int.Parse(e.Item.Tag.ToString())]["MsgAn"].ToString(), ds.Tables["TableMsgE"].Rows[int.Parse(e.Item.Tag.ToString())]["MsgNo"].ToString());
                FrmInfo.ShowDialog();
            }
        }


        int TimerEcl=0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (TimerEcl==0)
            {
                tileItemEclai.Elements[1].Image = Properties.Resources.Spiral_Bulb_64px;
                TimerEcl = 1;
            }
            else
            {
                tileItemEclai.Elements[1].Image = Properties.Resources.Spiral_Bulb_64px_1;
                TimerEcl = 0;
            }
        }

    }
}