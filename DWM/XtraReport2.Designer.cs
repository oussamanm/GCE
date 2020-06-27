namespace DWM
{
    partial class XtraReport2
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTableEntr = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellMtnEntr = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellLibEntr = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.dataSetReportFinan1 = new DWM.DataSetReportFinan();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableEntr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetReportFinan1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableEntr});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 31.25001F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableEntr
            // 
            this.xrTableEntr.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.xrTableEntr.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableEntr.Dpi = 100F;
            this.xrTableEntr.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.xrTableEntr.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTableEntr.Name = "xrTableEntr";
            this.xrTableEntr.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTableEntr.SizeF = new System.Drawing.SizeF(358F, 31.25001F);
            this.xrTableEntr.StylePriority.UseBackColor = false;
            this.xrTableEntr.StylePriority.UseBorderColor = false;
            this.xrTableEntr.StylePriority.UseBorders = false;
            this.xrTableEntr.StylePriority.UseFont = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellMtnEntr,
            this.xrTableCellLibEntr});
            this.xrTableRow4.Dpi = 100F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCellMtnEntr
            // 
            this.xrTableCellMtnEntr.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DateEntress.MtnEntr")});
            this.xrTableCellMtnEntr.Dpi = 100F;
            this.xrTableCellMtnEntr.Font = new System.Drawing.Font("Tahoma", 10F);
            this.xrTableCellMtnEntr.Name = "xrTableCellMtnEntr";
            this.xrTableCellMtnEntr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCellMtnEntr.StylePriority.UseFont = false;
            this.xrTableCellMtnEntr.StylePriority.UsePadding = false;
            this.xrTableCellMtnEntr.StylePriority.UseTextAlignment = false;
            this.xrTableCellMtnEntr.Text = "xrTableCellMtnEntr";
            this.xrTableCellMtnEntr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellMtnEntr.Weight = 0.272573036266063D;
            // 
            // xrTableCellLibEntr
            // 
            this.xrTableCellLibEntr.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCellLibEntr.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DateEntress.LibEntr")});
            this.xrTableCellLibEntr.Dpi = 100F;
            this.xrTableCellLibEntr.Font = new System.Drawing.Font("Tahoma", 10F);
            this.xrTableCellLibEntr.Multiline = true;
            this.xrTableCellLibEntr.Name = "xrTableCellLibEntr";
            this.xrTableCellLibEntr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCellLibEntr.StylePriority.UseBackColor = false;
            this.xrTableCellLibEntr.StylePriority.UseFont = false;
            this.xrTableCellLibEntr.StylePriority.UseTextAlignment = false;
            this.xrTableCellLibEntr.Text = "الواجب أداؤه\r\n";
            this.xrTableCellLibEntr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellLibEntr.Weight = 0.735280185413796D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 2.083333F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // dataSetReportFinan1
            // 
            this.dataSetReportFinan1.DataSetName = "DataSetReportFinan";
            this.dataSetReportFinan1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // XtraReport2
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataSource = this.dataSetReportFinan1;
            this.Margins = new System.Drawing.Printing.Margins(38, 52, 0, 2);
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTableEntr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetReportFinan1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DataSetReportFinan dataSetReportFinan1;
        private DevExpress.XtraReports.UI.XRTable xrTableEntr;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellMtnEntr;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellLibEntr;
    }
}
