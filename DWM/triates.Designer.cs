namespace DWM
{
    partial class triates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(triates));
            this.windowsUIButtonPanelMain = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
            this.dataSetTraites1 = new DWM.DataSetTraites();
            this.controleFooter1 = new DWM.ControleFooter();
            this.controleHeader1 = new DWM.ControleHeader();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::DWM.WaitForm1), true, true);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gridControlEntr = new DevExpress.XtraGrid.GridControl();
            this.gridViewEntr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.nomcomp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NumComp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MoisMTr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MontantMTr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DatePayerMTr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.view_traiteTableAdapter1 = new DWM.DataSetTraitesTableAdapters.view_traiteTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTraites1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlEntr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEntr)).BeginInit();
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
            toolTipItem1.Text = "إضافة ";
            superToolTip1.Items.Add(toolTipItem1);
            toolTipItem2.Text = "نسخ لائحة الأقساط";
            superToolTip2.Items.Add(toolTipItem2);
            this.windowsUIButtonPanelMain.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", global::DWM.Properties.Resources.Add_32px, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, superToolTip1, true, false, true, null, "Ajouter", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", global::DWM.Properties.Resources.Print_32px, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, superToolTip2, true, false, true, null, "Imprimer", -1, false, false)});
            this.windowsUIButtonPanelMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.windowsUIButtonPanelMain.EnableImageTransparency = true;
            this.windowsUIButtonPanelMain.ForeColor = System.Drawing.Color.White;
            this.windowsUIButtonPanelMain.Location = new System.Drawing.Point(0, 0);
            this.windowsUIButtonPanelMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.windowsUIButtonPanelMain.MaximumSize = new System.Drawing.Size(0, 50);
            this.windowsUIButtonPanelMain.MinimumSize = new System.Drawing.Size(60, 50);
            this.windowsUIButtonPanelMain.Name = "windowsUIButtonPanelMain";
            this.windowsUIButtonPanelMain.Size = new System.Drawing.Size(1184, 50);
            this.windowsUIButtonPanelMain.TabIndex = 82;
            this.windowsUIButtonPanelMain.Text = "windowsUIButtonPanelMain";
            this.windowsUIButtonPanelMain.UseButtonBackgroundImages = false;
            this.windowsUIButtonPanelMain.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.windowsUIButtonPanelMain_ButtonClick);
            // 
            // dataSetTraites1
            // 
            this.dataSetTraites1.DataSetName = "DataSetTraites";
            this.dataSetTraites1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // controleFooter1
            // 
            this.controleFooter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controleFooter1.Location = new System.Drawing.Point(0, 50);
            this.controleFooter1.Name = "controleFooter1";
            this.controleFooter1.Size = new System.Drawing.Size(1184, 40);
            this.controleFooter1.TabIndex = 24;
            // 
            // controleHeader1
            // 
            this.controleHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.controleHeader1.Location = new System.Drawing.Point(0, 0);
            this.controleHeader1.Name = "controleHeader1";
            this.controleHeader1.Size = new System.Drawing.Size(1184, 57);
            this.controleHeader1.TabIndex = 18;
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.controleFooter1);
            this.panel1.Controls.Add(this.windowsUIButtonPanelMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 621);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 90);
            this.panel1.TabIndex = 83;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gridControlEntr);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1184, 564);
            this.panel3.TabIndex = 85;
            // 
            // gridControlEntr
            // 
            this.gridControlEntr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlEntr.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridControlEntr.Location = new System.Drawing.Point(0, 0);
            this.gridControlEntr.LookAndFeel.SkinName = "Office 2016 Colorful";
            this.gridControlEntr.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControlEntr.MainView = this.gridViewEntr;
            this.gridControlEntr.Name = "gridControlEntr";
            this.gridControlEntr.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControlEntr.Size = new System.Drawing.Size(1184, 564);
            this.gridControlEntr.TabIndex = 97;
            this.gridControlEntr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewEntr});
            // 
            // gridViewEntr
            // 
            this.gridViewEntr.Appearance.FixedLine.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gridViewEntr.Appearance.FixedLine.Options.UseFont = true;
            this.gridViewEntr.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gridViewEntr.Appearance.FooterPanel.Options.UseFont = true;
            this.gridViewEntr.Appearance.FooterPanel.Options.UseTextOptions = true;
            this.gridViewEntr.Appearance.FooterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEntr.Appearance.FooterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewEntr.Appearance.GroupFooter.Font = new System.Drawing.Font("Tahoma", 8.75F);
            this.gridViewEntr.Appearance.GroupFooter.Options.UseFont = true;
            this.gridViewEntr.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gridViewEntr.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridViewEntr.Appearance.GroupPanel.ForeColor = System.Drawing.Color.DimGray;
            this.gridViewEntr.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridViewEntr.Appearance.GroupPanel.Options.UseBorderColor = true;
            this.gridViewEntr.Appearance.GroupPanel.Options.UseFont = true;
            this.gridViewEntr.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridViewEntr.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridViewEntr.Appearance.GroupRow.Options.UseFont = true;
            this.gridViewEntr.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.gridViewEntr.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.gridViewEntr.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.DarkBlue;
            this.gridViewEntr.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridViewEntr.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridViewEntr.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewEntr.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridViewEntr.Appearance.HorzLine.BackColor = System.Drawing.Color.LightGray;
            this.gridViewEntr.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridViewEntr.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gridViewEntr.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewEntr.Appearance.Row.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.gridViewEntr.Appearance.Row.Options.UseBackColor = true;
            this.gridViewEntr.Appearance.Row.Options.UseFont = true;
            this.gridViewEntr.Appearance.Row.Options.UseForeColor = true;
            this.gridViewEntr.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewEntr.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEntr.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewEntr.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gridViewEntr.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridViewEntr.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(229)))), ((int)(((byte)(241)))));
            this.gridViewEntr.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gridViewEntr.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridViewEntr.Appearance.SelectedRow.Options.UseFont = true;
            this.gridViewEntr.AppearancePrint.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridViewEntr.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridViewEntr.AppearancePrint.FooterPanel.Options.UseBackColor = true;
            this.gridViewEntr.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.gridViewEntr.AppearancePrint.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.gridViewEntr.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.gridViewEntr.AppearancePrint.HeaderPanel.ForeColor = System.Drawing.Color.DarkBlue;
            this.gridViewEntr.AppearancePrint.HeaderPanel.Options.UseBackColor = true;
            this.gridViewEntr.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.gridViewEntr.AppearancePrint.HeaderPanel.Options.UseForeColor = true;
            this.gridViewEntr.AppearancePrint.Lines.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gridViewEntr.AppearancePrint.Lines.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gridViewEntr.AppearancePrint.Lines.Options.UseBorderColor = true;
            this.gridViewEntr.AppearancePrint.Lines.Options.UseFont = true;
            this.gridViewEntr.AppearancePrint.Row.BackColor = System.Drawing.Color.White;
            this.gridViewEntr.AppearancePrint.Row.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.gridViewEntr.AppearancePrint.Row.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.gridViewEntr.AppearancePrint.Row.Options.UseBackColor = true;
            this.gridViewEntr.AppearancePrint.Row.Options.UseFont = true;
            this.gridViewEntr.AppearancePrint.Row.Options.UseForeColor = true;
            this.gridViewEntr.AppearancePrint.Row.Options.UseTextOptions = true;
            this.gridViewEntr.AppearancePrint.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEntr.AppearancePrint.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewEntr.ColumnPanelRowHeight = 25;
            this.gridViewEntr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.nomcomp,
            this.NumComp,
            this.MoisMTr,
            this.sta,
            this.MontantMTr,
            this.DatePayerMTr});
            this.gridViewEntr.FooterPanelHeight = 25;
            this.gridViewEntr.GridControl = this.gridControlEntr;
            this.gridViewEntr.GroupCount = 1;
            this.gridViewEntr.GroupPanelText = "إسحب خانة هنا.... ";
            this.gridViewEntr.GroupRowHeight = 10;
            this.gridViewEntr.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontantMTr", this.MontantMTr, "المبلغ: {0:c2}")});
            this.gridViewEntr.Name = "gridViewEntr";
            this.gridViewEntr.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewEntr.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewEntr.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridViewEntr.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridViewEntr.OptionsBehavior.ReadOnly = true;
            this.gridViewEntr.OptionsFind.AlwaysVisible = true;
            this.gridViewEntr.OptionsFind.FindNullPrompt = "بحث";
            this.gridViewEntr.OptionsFind.ShowClearButton = false;
            this.gridViewEntr.OptionsFind.ShowCloseButton = false;
            this.gridViewEntr.OptionsFind.ShowFindButton = false;
            this.gridViewEntr.OptionsPrint.ExpandAllDetails = true;
            this.gridViewEntr.OptionsPrint.PrintDetails = true;
            this.gridViewEntr.OptionsView.AllowCellMerge = true;
            this.gridViewEntr.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewEntr.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewEntr.OptionsView.ShowIndicator = false;
            this.gridViewEntr.RowHeight = 25;
            this.gridViewEntr.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.NumComp, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // nomcomp
            // 
            this.nomcomp.Caption = "المشترك";
            this.nomcomp.FieldName = "nomcomp";
            this.nomcomp.Name = "nomcomp";
            this.nomcomp.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.nomcomp.OptionsColumn.Printable = DevExpress.Utils.DefaultBoolean.True;
            this.nomcomp.Visible = true;
            this.nomcomp.VisibleIndex = 0;
            this.nomcomp.Width = 251;
            // 
            // NumComp
            // 
            this.NumComp.Caption = "رقم العداد";
            this.NumComp.FieldName = "NumComp";
            this.NumComp.Name = "NumComp";
            this.NumComp.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.NumComp.Visible = true;
            this.NumComp.VisibleIndex = 0;
            this.NumComp.Width = 128;
            // 
            // MoisMTr
            // 
            this.MoisMTr.Caption = "الفترة";
            this.MoisMTr.FieldName = "MoisMTr";
            this.MoisMTr.Name = "MoisMTr";
            this.MoisMTr.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.MoisMTr.Visible = true;
            this.MoisMTr.VisibleIndex = 1;
            this.MoisMTr.Width = 242;
            // 
            // sta
            // 
            this.sta.Caption = "الأداء";
            this.sta.FieldName = "sta";
            this.sta.Name = "sta";
            this.sta.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.sta.Visible = true;
            this.sta.VisibleIndex = 2;
            this.sta.Width = 105;
            // 
            // MontantMTr
            // 
            this.MontantMTr.AppearanceCell.Options.UseTextOptions = true;
            this.MontantMTr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MontantMTr.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.MontantMTr.Caption = "المبلغ";
            this.MontantMTr.FieldName = "MontantMTr";
            this.MontantMTr.Name = "MontantMTr";
            this.MontantMTr.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.MontantMTr.Visible = true;
            this.MontantMTr.VisibleIndex = 4;
            this.MontantMTr.Width = 282;
            // 
            // DatePayerMTr
            // 
            this.DatePayerMTr.Caption = "تاريخ الأداء";
            this.DatePayerMTr.FieldName = "DatePayerMTr";
            this.DatePayerMTr.Name = "DatePayerMTr";
            this.DatePayerMTr.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.DatePayerMTr.Visible = true;
            this.DatePayerMTr.VisibleIndex = 3;
            this.DatePayerMTr.Width = 302;
            // 
            // view_traiteTableAdapter1
            // 
            this.view_traiteTableAdapter1.ClearBeforeFill = true;
            // 
            // triates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1184, 711);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.controleHeader1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "triates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "الأقساط الشهرية";
            this.Load += new System.EventHandler(this.triates_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTraites1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlEntr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEntr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ControleHeader controleHeader1;
        private ControleFooter controleFooter1;
        private DataSetTraites dataSetTraites1;
        private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel windowsUIButtonPanelMain;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gridControlEntr;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewEntr;
        private DevExpress.XtraGrid.Columns.GridColumn nomcomp;
        private DevExpress.XtraGrid.Columns.GridColumn NumComp;
        private DevExpress.XtraGrid.Columns.GridColumn MoisMTr;
        private DevExpress.XtraGrid.Columns.GridColumn sta;
        private DevExpress.XtraGrid.Columns.GridColumn DatePayerMTr;
        private DevExpress.XtraGrid.Columns.GridColumn MontantMTr;
        private DataSetTraitesTableAdapters.view_traiteTableAdapter view_traiteTableAdapter1;
    }
}