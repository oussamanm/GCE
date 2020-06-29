namespace DWM
{
    partial class Adherents
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Adherents));
            this.windowsUIButtonPanelMain = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
            this.controleFooter1 = new DWM.ControleFooter();
            this.controleHeader1 = new DWM.ControleHeader();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RbDontHave = new System.Windows.Forms.RadioButton();
            this.RbAnnu = new System.Windows.Forms.RadioButton();
            this.RbAll = new System.Windows.Forms.RadioButton();
            this.RbDea = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::DWM.WaitForm1), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // windowsUIButtonPanelMain
            // 
            this.windowsUIButtonPanelMain.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.windowsUIButtonPanelMain.AppearanceButton.Hovered.FontSizeDelta = -1;
            this.windowsUIButtonPanelMain.AppearanceButton.Hovered.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(116)))), ((int)(((byte)(3)))));
            this.windowsUIButtonPanelMain.AppearanceButton.Hovered.Options.UseBackColor = true;
            this.windowsUIButtonPanelMain.AppearanceButton.Hovered.Options.UseFont = true;
            this.windowsUIButtonPanelMain.AppearanceButton.Hovered.Options.UseForeColor = true;
            this.windowsUIButtonPanelMain.AppearanceButton.Normal.Font = new System.Drawing.Font("Arial Rounded MT Bold", 0.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowsUIButtonPanelMain.AppearanceButton.Normal.Options.UseFont = true;
            this.windowsUIButtonPanelMain.AppearanceButton.Pressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(159)))), ((int)(((byte)(159)))));
            this.windowsUIButtonPanelMain.AppearanceButton.Pressed.FontSizeDelta = -1;
            this.windowsUIButtonPanelMain.AppearanceButton.Pressed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(159)))), ((int)(((byte)(159)))));
            this.windowsUIButtonPanelMain.AppearanceButton.Pressed.Options.UseBackColor = true;
            this.windowsUIButtonPanelMain.AppearanceButton.Pressed.Options.UseFont = true;
            this.windowsUIButtonPanelMain.AppearanceButton.Pressed.Options.UseForeColor = true;
            this.windowsUIButtonPanelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            toolTipItem1.Text = "إضافة منخرط جديد";
            superToolTip1.Items.Add(toolTipItem1);
            toolTipItem2.Text = "تعديل";
            superToolTip2.Items.Add(toolTipItem2);
            toolTipItem3.Text = "طباعة لائحة المنخرطين";
            superToolTip3.Items.Add(toolTipItem3);
            this.windowsUIButtonPanelMain.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", global::DWM.Properties.Resources.Add_32px, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, superToolTip1, true, false, true, null, "Ajouter", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", global::DWM.Properties.Resources.Edit_32px, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, superToolTip2, true, false, true, null, "Modifier", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", global::DWM.Properties.Resources.Remove_User_Male_32px, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "Supprimer", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", global::DWM.Properties.Resources.Print_32px, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, superToolTip3, true, false, true, null, "Imprimer", -1, false, false)});
            this.windowsUIButtonPanelMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.windowsUIButtonPanelMain.EnableImageTransparency = true;
            this.windowsUIButtonPanelMain.ForeColor = System.Drawing.Color.White;
            this.windowsUIButtonPanelMain.Location = new System.Drawing.Point(0, 621);
            this.windowsUIButtonPanelMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.windowsUIButtonPanelMain.MaximumSize = new System.Drawing.Size(0, 50);
            this.windowsUIButtonPanelMain.MinimumSize = new System.Drawing.Size(60, 50);
            this.windowsUIButtonPanelMain.Name = "windowsUIButtonPanelMain";
            this.windowsUIButtonPanelMain.Size = new System.Drawing.Size(1184, 50);
            this.windowsUIButtonPanelMain.TabIndex = 80;
            this.windowsUIButtonPanelMain.Text = "windowsUIButtonPanelMain";
            this.windowsUIButtonPanelMain.UseButtonBackgroundImages = false;
            this.windowsUIButtonPanelMain.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.windowsUIButtonPanelMain_ButtonClick);
            // 
            // controleFooter1
            // 
            this.controleFooter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controleFooter1.Location = new System.Drawing.Point(0, 671);
            this.controleFooter1.Name = "controleFooter1";
            this.controleFooter1.Size = new System.Drawing.Size(1184, 40);
            this.controleFooter1.TabIndex = 1;
            // 
            // controleHeader1
            // 
            this.controleHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.controleHeader1.Location = new System.Drawing.Point(0, 0);
            this.controleHeader1.Name = "controleHeader1";
            this.controleHeader1.Size = new System.Drawing.Size(1184, 57);
            this.controleHeader1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridControl1.Location = new System.Drawing.Point(3, 56);
            this.gridControl1.LookAndFeel.SkinName = "Office 2016 Colorful";
            this.gridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControl1.Size = new System.Drawing.Size(1178, 557);
            this.gridControl1.TabIndex = 81;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FixedLine.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gridView1.Appearance.FixedLine.Options.UseFont = true;
            this.gridView1.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gridView1.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.GroupPanel.ForeColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.GroupPanel.Image = global::DWM.Properties.Resources.blue;
            this.gridView1.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.GroupPanel.Options.UseBorderColor = true;
            this.gridView1.Appearance.GroupPanel.Options.UseFont = true;
            this.gridView1.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridView1.Appearance.GroupPanel.Options.UseImage = true;
            this.gridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.DarkBlue;
            this.gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridView1.Appearance.HorzLine.BackColor = System.Drawing.Color.LightGray;
            this.gridView1.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridView1.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.Row.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.gridView1.Appearance.Row.Options.UseBackColor = true;
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Appearance.Row.Options.UseForeColor = true;
            this.gridView1.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(229)))), ((int)(((byte)(241)))));
            this.gridView1.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.SelectedRow.Options.UseFont = true;
            this.gridView1.AppearancePrint.Lines.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gridView1.AppearancePrint.Lines.Options.UseFont = true;
            this.gridView1.ColumnPanelRowHeight = 35;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupPanelText = "إسحب خانة هنا.... ";
            this.gridView1.GroupRowHeight = 3;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "بحث";
            this.gridView1.OptionsFind.ShowClearButton = false;
            this.gridView1.OptionsFind.ShowCloseButton = false;
            this.gridView1.OptionsFind.ShowFindButton = false;
            this.gridView1.OptionsPrint.PrintSelectedRowsOnly = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowHeight = 30;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            this.gridView1.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RbDontHave);
            this.panel1.Controls.Add(this.RbAnnu);
            this.panel1.Controls.Add(this.RbAll);
            this.panel1.Controls.Add(this.RbDea);
            this.panel1.Controls.Add(this.gridControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 711);
            this.panel1.TabIndex = 82;
            // 
            // RbDontHave
            // 
            this.RbDontHave.AutoSize = true;
            this.RbDontHave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.RbDontHave.Font = new System.Drawing.Font("Tahoma", 9F);
            this.RbDontHave.Location = new System.Drawing.Point(724, 76);
            this.RbDontHave.Name = "RbDontHave";
            this.RbDontHave.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RbDontHave.Size = new System.Drawing.Size(121, 18);
            this.RbDontHave.TabIndex = 3;
            this.RbDontHave.Text = "لايتوفرون على عداد";
            this.RbDontHave.UseVisualStyleBackColor = false;
            this.RbDontHave.CheckedChanged += new System.EventHandler(this.RbDontHave_CheckedChanged);
            // 
            // RbAnnu
            // 
            this.RbAnnu.AutoSize = true;
            this.RbAnnu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.RbAnnu.Font = new System.Drawing.Font("Tahoma", 9F);
            this.RbAnnu.Location = new System.Drawing.Point(874, 76);
            this.RbAnnu.Name = "RbAnnu";
            this.RbAnnu.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RbAnnu.Size = new System.Drawing.Size(80, 18);
            this.RbAnnu.TabIndex = 2;
            this.RbAnnu.Text = "المنسحبون";
            this.RbAnnu.UseVisualStyleBackColor = false;
            this.RbAnnu.CheckedChanged += new System.EventHandler(this.RbAnnu_CheckedChanged);
            // 
            // RbAll
            // 
            this.RbAll.AutoSize = true;
            this.RbAll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.RbAll.Checked = true;
            this.RbAll.Font = new System.Drawing.Font("Tahoma", 9F);
            this.RbAll.Location = new System.Drawing.Point(1077, 76);
            this.RbAll.Name = "RbAll";
            this.RbAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RbAll.Size = new System.Drawing.Size(57, 18);
            this.RbAll.TabIndex = 0;
            this.RbAll.TabStop = true;
            this.RbAll.Text = "الجمبع";
            this.RbAll.UseVisualStyleBackColor = false;
            this.RbAll.CheckedChanged += new System.EventHandler(this.RbAll_CheckedChanged);
            // 
            // RbDea
            // 
            this.RbDea.AutoSize = true;
            this.RbDea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.RbDea.Font = new System.Drawing.Font("Tahoma", 9F);
            this.RbDea.Location = new System.Drawing.Point(983, 76);
            this.RbDea.Name = "RbDea";
            this.RbDea.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RbDea.Size = new System.Drawing.Size(65, 18);
            this.RbDea.TabIndex = 1;
            this.RbDea.Text = "المتوفون";
            this.RbDea.UseVisualStyleBackColor = false;
            this.RbDea.CheckedChanged += new System.EventHandler(this.RbDea_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(99)))), ((int)(((byte)(177)))));
            this.panel2.BackgroundImage = global::DWM.Properties.Resources.Home_32px;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel2.Location = new System.Drawing.Point(213, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(33, 24);
            this.panel2.TabIndex = 87;
            this.panel2.Click += new System.EventHandler(this.label3_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(99)))), ((int)(((byte)(177)))));
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 24);
            this.label3.TabIndex = 86;
            this.label3.Text = "الصفحة الرئيسية/المنخرطون";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 20;
            // 
            // Adherents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1184, 711);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.windowsUIButtonPanelMain);
            this.Controls.Add(this.controleFooter1);
            this.Controls.Add(this.controleHeader1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Adherents";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "المنخرطين";
            this.Load += new System.EventHandler(this.Adherents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ControleHeader controleHeader1;
        private ControleFooter controleFooter1;
        private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel windowsUIButtonPanelMain;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton RbAnnu;
        private System.Windows.Forms.RadioButton RbDea;
        private System.Windows.Forms.RadioButton RbAll;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private System.Windows.Forms.RadioButton RbDontHave;
    }
}