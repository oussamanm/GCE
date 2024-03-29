﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DWM
{
    public partial class ControleHeader : UserControl
    {
        public ControleHeader()
        {
            InitializeComponent();
        }

        public string label_menu_btn = null;

        private void ControleHeader_Load(object sender, EventArgs e)
        {
            if (label_menu_btn != null)
                labelControlHome.Text = label_menu_btn;
            label1.Text = UserConnecte.NomUser + " " + UserConnecte.PrenomUser;
            label2.Text = UserConnecte.LibelleType;
        }

        private void windowsUIButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                if (e.Button.Properties.Tag == "quit")
                {
                    Login LogFORM = new Login();
                    LogFORM.Show();

                    this.ParentForm.Hide();
                }
                else if (e.Button.Properties.Tag == "sleep")
                {
                    Sleep FSleep = new Sleep();
                    FSleep.ShowDialog(this);
                }
                else if (e.Button.Properties.Tag == "minimize")
                {
                    this.ParentForm.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void labelControlHome_Click(object sender, EventArgs e)
        {
            FormMenu MENU = new FormMenu();
            MENU.Show();

            this.ParentForm.Hide();
        }
    }
}
