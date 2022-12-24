namespace AYAT_COMMISION_MANAGEMENT
{
    partial class Form1
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
            DevExpress.XtraGrid.GridControl grdEmployeeCommissionHistory;
            this.gvEmployeeCommissionHistory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.completeProgressBar = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            grdEmployeeCommissionHistory = new DevExpress.XtraGrid.GridControl();
            ((System.ComponentModel.ISupportInitialize)(grdEmployeeCommissionHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmployeeCommissionHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.completeProgressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // grdEmployeeCommissionHistory
            // 
            grdEmployeeCommissionHistory.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            grdEmployeeCommissionHistory.Location = new System.Drawing.Point(-402, 20);
            grdEmployeeCommissionHistory.MainView = this.gvEmployeeCommissionHistory;
            grdEmployeeCommissionHistory.Name = "grdEmployeeCommissionHistory";
            grdEmployeeCommissionHistory.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.completeProgressBar});
            grdEmployeeCommissionHistory.Size = new System.Drawing.Size(1604, 410);
            grdEmployeeCommissionHistory.TabIndex = 27;
            grdEmployeeCommissionHistory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEmployeeCommissionHistory});
            // 
            // gvEmployeeCommissionHistory
            // 
            this.gvEmployeeCommissionHistory.ColumnPanelRowHeight = 50;
            this.gvEmployeeCommissionHistory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn4,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn7,
            this.gridColumn12});
            this.gvEmployeeCommissionHistory.DetailHeight = 437;
            this.gvEmployeeCommissionHistory.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.gvEmployeeCommissionHistory.GridControl = grdEmployeeCommissionHistory;
            this.gvEmployeeCommissionHistory.Name = "gvEmployeeCommissionHistory";
            this.gvEmployeeCommissionHistory.OptionsBehavior.Editable = false;
            this.gvEmployeeCommissionHistory.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvEmployeeCommissionHistory.OptionsDetail.EnableMasterViewMode = false;
            this.gvEmployeeCommissionHistory.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvEmployeeCommissionHistory.OptionsView.ShowGroupPanel = false;
            this.gvEmployeeCommissionHistory.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gvEmployeeCommissionHistory.OptionsView.ShowIndicator = false;
            this.gvEmployeeCommissionHistory.RowHeight = 30;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn1.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.Caption = "Contract Code";
            this.gridColumn1.FieldName = "code";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 93;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn4.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.Caption = "Customer";
            this.gridColumn4.FieldName = "FAName";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 93;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn2.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.Caption = "Date";
            this.gridColumn2.FieldName = "Date";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 93;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn3.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.Caption = "Sales Total";
            this.gridColumn3.FieldName = "Total";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 93;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn5.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.Caption = "Paid";
            this.gridColumn5.FieldName = "paidCom";
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 93;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn6.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.Caption = "%Paid";
            this.gridColumn6.ColumnEdit = this.completeProgressBar;
            this.gridColumn6.FieldName = "paidPercent";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 93;
            // 
            // completeProgressBar
            // 
            this.completeProgressBar.Name = "completeProgressBar";
            this.completeProgressBar.ProgressPadding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.completeProgressBar.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            this.completeProgressBar.ShowTitle = true;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn10.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn10.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn10.AppearanceHeader.Options.UseFont = true;
            this.gridColumn10.Caption = "Available";
            this.gridColumn10.FieldName = "availableCom";
            this.gridColumn10.MinWidth = 25;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 6;
            this.gridColumn10.Width = 94;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn11.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn11.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn11.AppearanceHeader.Options.UseFont = true;
            this.gridColumn11.Caption = "Pending";
            this.gridColumn11.FieldName = "pendingCom";
            this.gridColumn11.MinWidth = 25;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 7;
            this.gridColumn11.Width = 94;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn7.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.Caption = "Unavailable";
            this.gridColumn7.FieldName = "unavailableCom";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 8;
            this.gridColumn7.Width = 94;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceHeader.BackColor = System.Drawing.Color.White;
            this.gridColumn12.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.gridColumn12.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn12.AppearanceHeader.Options.UseFont = true;
            this.gridColumn12.Caption = "Total";
            this.gridColumn12.FieldName = "totalCom";
            this.gridColumn12.MinWidth = 25;
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 9;
            this.gridColumn12.Width = 94;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(grdEmployeeCommissionHistory);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(grdEmployeeCommissionHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmployeeCommissionHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.completeProgressBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Views.Grid.GridView gvEmployeeCommissionHistory;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar completeProgressBar;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
    }
}