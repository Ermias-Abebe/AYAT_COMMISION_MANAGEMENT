using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using AyatDataAccess;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using AyatProcessManager;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCContractAgreementBrowser : DevExpress.XtraEditors.XtraUserControl
    {
        #region Declarations
        private List<DXMenuItem> menu_Items { get; set; }
        private List<Sale> Filtered_Sales { get; set; }
        private List<AyatProcessManager.Custom_Classes.Agreement_Info> agreements { get; set; }
        private AyatProcessManager.Custom_Classes.Agreement_Info selected_Agreement { get; set; }

        public event EventHandler close_Button_Clicked;
        #endregion

        public UCContractAgreementBrowser()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void cmbDateCriteria_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDateCriteria.EditValue != null && cmbDateCriteria.EditValue.ToString() != string.Empty)
                {
                    dtStartDate.Properties.ReadOnly = true;
                    dtEndDate.Properties.ReadOnly = true;
                    dtStartDate.Properties.ShowDropDown = ShowDropDown.Never;
                    dtEndDate.Properties.ShowDropDown = ShowDropDown.Never;
                    layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layConItTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    switch (cmbDateCriteria.EditValue.ToString().ToUpper())
                    {
                        case "TODAY":
                            dtStartDate.EditValue = DateTime.Now;
                            layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "AT THE DAY OF":
                            dtStartDate.EditValue = DateTime.Now;
                            dtStartDate.Properties.ReadOnly = false;
                            dtStartDate.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS WEEK":
                            dtStartDate.EditValue = DateTime.Now.AddDays(-7);
                            dtEndDate.EditValue = DateTime.Now;
                            layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS MONTH":
                            dtStartDate.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            dtEndDate.EditValue = DateTime.Now;
                            layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS YEAR":
                            dtStartDate.EditValue = new DateTime(DateTime.Now.Year, 1, 1);
                            dtEndDate.EditValue = DateTime.Now;
                            layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "DATE RANGE":
                            dtStartDate.Properties.ReadOnly = false;
                            dtEndDate.Properties.ReadOnly = false;
                            dtStartDate.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            dtEndDate.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            layConItFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "ALL":
                            dtStartDate.EditValue = null;
                            dtEndDate.EditValue = null;
                            break;
                    }
                }
            }
            catch { }
        }
        private void btnShowContractAgreements_Click(object sender, EventArgs e)
        {
            try
            {
                agreements = new List<Custom_Classes.Agreement_Info>();
                Filtered_Sales = AyatProcessManager.AyatProcessManager.Get_Contract_Agreements(txtContractAgreementNo.Text, (int?)imcoStatus.EditValue, (DateTime?)dtStartDate.EditValue, (DateTime?)dtEndDate.EditValue);
                if (Filtered_Sales != null && Filtered_Sales.Count > 0)
                {
                    if (imcoStatus.EditValue != null && imcoStatus.EditValue.ToString() == "0")
                    {
                        gColPreparedBy.Visible = true;
                        gColApprovedBy.Visible = false;
                    }
                    else
                    {
                        gColPreparedBy.Visible = true;
                        gColApprovedBy.Visible = true;
                    }
                    agreements = Filtered_Sales.Select(x => new AyatProcessManager.Custom_Classes.Agreement_Info
                    {
                        ID = x.ID,
                        status = x.status,
                        code = x.code,
                        customer = x.FAName,
                        prepared_Date = x.Date,
                        prepared_By = DataBuffer.UserNameList.FirstOrDefault(y => y.ID == x.PrepareuserID).name,
                        approved_By = x.ApproveuserID != null ? DataBuffer.UserNameList.FirstOrDefault(y => y.ID == x.ApproveuserID).name : null,
                        total_Amount = x.Total,
                        site = x.Site,
                        building_No = x.BuildingNo,
                        house_No = x.HouseNo
                    }).ToList();
                }
                grdContractAgreements.DataSource = agreements;
                gvContractAgreements.BestFitColumns();
            }
            catch { }
        }
        private void gvContractAgreements_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                selected_Agreement = gvContractAgreements.GetFocusedRow() as AyatProcessManager.Custom_Classes.Agreement_Info;
                if (selected_Agreement != null && selected_Agreement.status == (int)AyatProcessManager.Custom_Classes.Sales_Status.prepared)
                {
                    e.Menu.Items.Add(new DXMenuItem("Approve"));
                    e.Menu.Items[0].Click += Context_Menu_Click;
                }
            }
            catch { }
        }
        private void Context_Menu_Click(object sender, EventArgs e)
        {
            try
            {
                switch ((sender as DXMenuItem).Caption)
                {
                    case "View":
                        UCReport.DisplayReportFromExternalSource(null, null, null, "Specific Sales");
                        break;
                    case "Approve":
                        selected_Agreement = gvContractAgreements.GetFocusedRow() as AyatProcessManager.Custom_Classes.Agreement_Info;
                        if (selected_Agreement != null)
                        {
                            if (AyatProcessManager.AyatProcessManager.Approve_Contract_Agreement(selected_Agreement.ID))
                            {
                                XtraMessageBox.Show("Contract Agreement No. " + selected_Agreement.code + Environment.NewLine + "Has Been Successfully Approved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                btnShowContractAgreements.PerformClick();
                            }
                            else
                                XtraMessageBox.Show("Contract Agreement No. " + selected_Agreement.code + Environment.NewLine + "Approval Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
            catch { }
        }
        private void backWindowsUIButtonPanel_Click(object sender, EventArgs e)
        {
            close_Button_Clicked?.Invoke(sender, e);
        }
        #endregion

        #region Methods
        public void Initalize_Contract_Agreement_Document()
        {
            try
            {
                imcoStatus.EditValue = 0;
                txtContractAgreementNo.Text = string.Empty;
                cmbDateCriteria.EditValue = "TODAY";
                grdContractAgreements.DataSource = null;
            }
            catch { }
        }
        #endregion
    }
}
