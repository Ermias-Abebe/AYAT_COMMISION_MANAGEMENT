using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using AyatDataAccess;
using static AyatProcessManager.Custom_Classes;
using AyatProcessManager;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCCommission : DevExpress.XtraEditors.XtraUserControl
    {
        #region Declarations
        private DateTime? start_Date { get; set; }
        private DateTime? end_Date { get; set; }
        private DateTime? start_Date_Approved { get; set; }
        private DateTime? end_Date_Approved { get; set; }
        private DateTime? start_Date_Report { get; set; }
        private DateTime? end_Date_Report { get; set; }
        private List<spGetCommissionDetail_Result> commission_Details { get; set; }
        private List<Commission_Summary> commission_Summary { get; set; }
        private List<Commission_Detail> comission_Summary_Detail { get; set; }
        private CommissionPayment commission_Payment { get; set; }
        private List<CommissionPaymentDetail> commission_Payment_Detail { get; set; }
        private List<spGetCommissionPaymentsAndDetails_Result> commission_Payments_And_Details { get; set; }
        private List<Prepared_Commission> Prepared_Commission_Payments { get; set; }
        private Prepared_Commission Selected_Prepared_Commission { get; set; }
        private List<Approved_Commission> Approved_Commission_Payments { get; set; }
        private List<spGetPaidCommissionReport_Result> paid_Com_Rport_Rslt { get; set; }
        private List<Paid_Commission_Report> Paid_Commission_Report { get; set; }

        public event EventHandler close_Button_Clicked;
        #endregion
        public UCCommission()
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

        private void dtStartDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtStartDate.EditValue != null && dtStartDate.EditValue.ToString() != string.Empty)
                    start_Date = Convert.ToDateTime(dtStartDate.EditValue);
                else start_Date = null;
            }
            catch { start_Date = null; }
        }

        private void dtEndDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtEndDate.EditValue != null && dtEndDate.EditValue.ToString() != string.Empty)
                    end_Date = Convert.ToDateTime(dtEndDate.EditValue);
                else end_Date = null;
            }
            catch { end_Date = null; }
        }

        private void CommissionTabbedControlGroup_SelectedPageChanging(object sender, DevExpress.XtraLayout.LayoutTabPageChangingEventArgs e)
        {
            try
            {
                if (e.Page == CommissionTabbedControlGroup.TabPages[0])
                {
                    CommissionUIButtonPanel.Buttons[0].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[1].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[2].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[3].Properties.Visible = true;

                    CommissionUIButtonPanel.Buttons[4].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[5].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[6].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[7].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[8].Properties.Visible = true;
                }
                else if (e.Page == CommissionTabbedControlGroup.TabPages[1])
                {
                    Get_Prepared_Commission_Payments();

                    CommissionUIButtonPanel.Buttons[0].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[1].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[2].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[3].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[4].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[5].Properties.Visible = true;

                    CommissionUIButtonPanel.Buttons[6].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[7].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[8].Properties.Visible = true;
                }
                else if (e.Page == CommissionTabbedControlGroup.TabPages[2])
                {
                    cmbApprovedDateCriteria.EditValue = "THIS MONTH";
                    grdApprovedComPayments.DataSource = null;

                    CommissionUIButtonPanel.Buttons[0].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[1].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[2].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[3].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[4].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[5].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[6].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[7].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[8].Properties.Visible = true;
                }
                else
                {
                    cmbDateCriteriaEmp.EditValue = "THIS MONTH";
                    sleEmp.EditValue = null;
                    txtBankPayNo.Text = string.Empty;
                    grdEmployeePaidComissions.DataSource = null;

                    CommissionUIButtonPanel.Buttons[0].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[1].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[2].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[3].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[4].Properties.Visible = false;
                    CommissionUIButtonPanel.Buttons[5].Properties.Visible = false;

                    CommissionUIButtonPanel.Buttons[6].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[7].Properties.Visible = true;
                    CommissionUIButtonPanel.Buttons[8].Properties.Visible = true;
                }
            }
            catch { }
        }

        private void gvCommissionDetail_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if ((sender as GridView).GetRow(e.RowHandle) as Commission_Detail != null)
                {
                    if (((sender as GridView).GetRow(e.RowHandle) as Commission_Detail).commission_Status == (int)Commission_Status.pendingBankPayment)
                        e.Appearance.BackColor = Color.Red;
                }
            }
            catch { }
        }

        private void btnShowCommissionSummary_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDateCriteria.EditValue != null && (cmbDateCriteria.EditValue.ToString() == "TODAY" || cmbDateCriteria.EditValue.ToString() == "AT THE DAY OF"))
                    commission_Details = AyatProcessManager.AyatProcessManager.Get_Commission_Summary(sleEmployees.EditValue != null ? Convert.ToInt16(sleEmployees.EditValue) : (int?)null, start_Date, start_Date);
                else
                    commission_Details = AyatProcessManager.AyatProcessManager.Get_Commission_Summary(sleEmployees.EditValue != null ? Convert.ToInt16(sleEmployees.EditValue) : (int?)null, start_Date, end_Date);

                if (commission_Details != null && commission_Details.Count > 0)
                {
                    commission_Summary = commission_Details.GroupBy(x => new { x.empID, x.fullName, x.EmployeeType }).Select(y => new Commission_Summary
                    {
                        Employee_ID = y.Key.empID,
                        Employee_Name = y.Key.fullName,
                        Employee_Type = y.Key.EmployeeType,
                        Role = y.First().commissionRole,
                        Commission_Amount = Get_Commission_Amount(y.Key.empID),
                        Commision_Details = Get_Commission_Detail(y.Key.empID)
                    }).ToList();
                    grdCommissionSummary.DataSource = commission_Summary;
                    grdCommissionSummary.RefreshDataSource();
                    gvCommissionSummary.BestFitColumns();
                    gvCommissionDetail.BestFitColumns();
                }
                else grdCommissionSummary.DataSource = null;
                CommissionTabbedControlGroup.SelectedTabPageIndex = 0;
            }
            catch { }
        }

        private void backWindowsUIButtonPanel_Click(object sender, EventArgs e)
        {
            close_Button_Clicked?.Invoke(sender, e);
        }

        private void CommissionUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
            {
                case "new":
                    Initialize_Commission_Payment(false);
                    break;
                case "cancel":
                    close_Button_Clicked?.Invoke(sender, e);
                    break;
                case "save":
                    if (Validate_Commission_Payment_Saving())
                    {
                        commission_Payment = new CommissionPayment
                        {
                            IssuedDate = DateTime.Now,
                            PreparedDate = Convert.ToDateTime(dePreparedDate.EditValue),
                            Amount = commission_Summary.Where(x => x.selected).Sum(y => y.Commission_Amount),
                            Status = (int)Commission_Payment_Status.prepared,
                            IsVoid = false,
                            PreparingUser = DataBuffer.thisUser.ID,
                            PreparingDevice = DataBuffer.thisDevice.ID
                        };
                        commission_Payment.ID = AyatProcessManager.AyatProcessManager.Commission_Payment_Insert(commission_Payment);
                        if (commission_Payment.ID != 0)
                        {
                            comission_Summary_Detail = commission_Summary.Where(x => x.selected).SelectMany(x => x.Commision_Details).ToList();
                            commission_Payment_Detail = comission_Summary_Detail.GroupBy(x => x.commission_ID).Select(t => new CommissionPaymentDetail
                            {
                                CommissionPaymentID = commission_Payment.ID,
                                SalesID = t.First().Sales_ID,
                                EmployeeID = t.First().employee_ID,
                                RoleID = t.First().role_ID.Value,
                                CommissionID = t.Key,
                                Amount = t.First().Commission_Amount
                            }).ToList();
                            if (AyatProcessManager.AyatProcessManager.Commission_Payment_Detail_List_Insert(commission_Payment_Detail))
                            {
                                XtraMessageBox.Show("Commission Payment Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Initialize_Commission_Payment(false);
                            }
                            else
                            {
                                XtraMessageBox.Show("Saving Commission Payment Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Saving Commission Payment Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                case "approve":
                    Selected_Prepared_Commission = gvPreparedCommissionPayments.GetFocusedRow() as Prepared_Commission;
                    if (Selected_Prepared_Commission != null)
                    {
                        if (Validate_Commision_Payment_Approving())
                        {
                            if (AyatProcessManager.AyatProcessManager.Approve_Commission_Payment(Selected_Prepared_Commission.ID, txtBankPaymentNo.Text, Convert.ToDateTime(deBankPayment.EditValue), DateTime.Now, DataBuffer.thisUser.ID, DataBuffer.thisDevice.ID))
                            {
                                XtraMessageBox.Show("Commission Payment Approved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Get_Prepared_Commission_Payments();
                                return;
                            }
                            else
                            {
                                XtraMessageBox.Show("Approving Commission Payment Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Please Select Prepared Commission First!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "export to excel":
                    if (CommissionTabbedControlGroup.SelectedTabPageIndex == 2)
                    {
                        if (grdApprovedComPayments.DataSource as List<Approved_Commission> != null && (grdApprovedComPayments.DataSource as List<Approved_Commission>).Count > 0)
                        {
                            using (SaveFileDialog dialog = new SaveFileDialog())
                            {
                                dialog.Filter = "Excel Files (*.Xls) |*.Xls";
                                dialog.DefaultExt = "Xls";
                                if (dialog.ShowDialog() == DialogResult.OK)
                                    grdApprovedComPayments.ExportToXls(dialog.FileName);
                            }
                        }
                        else
                            XtraMessageBox.Show("There Is No Data To Export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (CommissionTabbedControlGroup.SelectedTabPageIndex == 3)
                    {
                        if(grdEmployeePaidComissions.DataSource as List<Paid_Commission_Report> != null && (grdEmployeePaidComissions.DataSource as List<Paid_Commission_Report>).Count > 0)
                        {
                            using (SaveFileDialog dialog = new SaveFileDialog())
                            {
                                dialog.Filter = "Excel Files (*.Xls) |*.Xls";
                                dialog.DefaultExt = "Xls";
                                if (dialog.ShowDialog() == DialogResult.OK)
                                    grdEmployeePaidComissions.ExportToXls(dialog.FileName);
                            }
                        }
                        else
                            XtraMessageBox.Show("There Is No Data To Export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }

        private void cmbApprovedDateCriteria_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbApprovedDateCriteria.EditValue != null && cmbApprovedDateCriteria.EditValue.ToString() != string.Empty)
                {
                    deApprovedFrom.Properties.ReadOnly = true;
                    deApprovedTo.Properties.ReadOnly = true;
                    deApprovedFrom.Properties.ShowDropDown = ShowDropDown.Never;
                    deApprovedTo.Properties.ShowDropDown = ShowDropDown.Never;
                    layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layConItToApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    switch (cmbApprovedDateCriteria.EditValue.ToString().ToUpper())
                    {
                        case "TODAY":
                            deApprovedFrom.EditValue = DateTime.Now;
                            layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "AT THE DAY OF":
                            deApprovedFrom.EditValue = DateTime.Now;
                            deApprovedFrom.Properties.ReadOnly = false;
                            deApprovedFrom.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS WEEK":
                            deApprovedFrom.EditValue = DateTime.Now.AddDays(-7);
                            deApprovedTo.EditValue = DateTime.Now;
                            layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItToApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS MONTH":
                            deApprovedFrom.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            deApprovedTo.EditValue = DateTime.Now;
                            layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItToApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS YEAR":
                            deApprovedFrom.EditValue = new DateTime(DateTime.Now.Year, 1, 1);
                            deApprovedTo.EditValue = DateTime.Now;
                            layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItToApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "DATE RANGE":
                            deApprovedFrom.Properties.ReadOnly = false;
                            deApprovedTo.Properties.ReadOnly = false;
                            deApprovedFrom.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            deApprovedTo.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            layConItFromApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layConItToApprov.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "ALL":
                            deApprovedFrom.EditValue = null;
                            deApprovedTo.EditValue = null;
                            break;
                    }
                }
            }
            catch { }
        }

        private void deApprovedFrom_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (deApprovedFrom.EditValue != null && deApprovedFrom.EditValue.ToString() != string.Empty)
                    start_Date_Approved = Convert.ToDateTime(deApprovedFrom.EditValue);
                else start_Date_Approved = null;
            }
            catch { start_Date_Approved = null; }
        }

        private void deApprovedTo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (deApprovedTo.EditValue != null && deApprovedTo.EditValue.ToString() != string.Empty)
                    end_Date_Approved = Convert.ToDateTime(deApprovedTo.EditValue);
                else end_Date_Approved = null;
            }
            catch { end_Date_Approved = null; }
        }

        private void btnShowApprovedCommissionPayments_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbApprovedDateCriteria.EditValue != null && (cmbApprovedDateCriteria.EditValue.ToString() == "TODAY" || cmbApprovedDateCriteria.EditValue.ToString() == "AT THE DAY OF"))
                    commission_Payments_And_Details = AyatProcessManager.AyatProcessManager.Get_Commission_Payments_And_Details((int)Commission_Payment_Status.approved, start_Date_Approved, start_Date_Approved);
                else
                    commission_Payments_And_Details = AyatProcessManager.AyatProcessManager.Get_Commission_Payments_And_Details((int)Commission_Payment_Status.approved, start_Date_Approved, end_Date_Approved);

                if (commission_Payments_And_Details != null && commission_Payments_And_Details.Count > 0)
                {
                    Approved_Commission_Payments = commission_Payments_And_Details.GroupBy(x => x.ID).Select(x => new Approved_Commission
                    {
                        Prepared_Date = x.First().PreparedDate,
                        Approved_Date = x.First().ApprovedDate,
                        Amount = x.First().Amount,
                        Bank_Payment_No = x.First().BankPaymentNo,
                        Bank_Payment_Date = x.First().BankPaymentDate,
                        Prepared_By = x.First().PreparingUser,
                        Approved_By = x.First().ApprovingUser,
                        Details = commission_Payments_And_Details.Select(y => new Commission_Payment_Detail
                        {
                            Employee_Name = y.fullName,
                            Role = y.commissionRole,
                            Amount = y.employeeCommission.Value
                        }).ToList()
                    }).ToList();
                    grdApprovedComPayments.DataSource = Approved_Commission_Payments;
                    grdApprovedComPayments.RefreshDataSource();
                    gvApprovedCommissionPayments.BestFitColumns();
                }
                else
                    grdApprovedComPayments.DataSource = null;
            }
            catch { }
        }

        private void btnShowPaidCommissionReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDateCriteriaEmp.EditValue != null && (cmbDateCriteriaEmp.EditValue.ToString() == "TODAY" || cmbDateCriteriaEmp.EditValue.ToString() == "AT THE DAY OF"))
                    paid_Com_Rport_Rslt = AyatProcessManager.AyatProcessManager.Get_Paid_Commssion_Report(sleEmp.EditValue != null ? Convert.ToInt16(sleEmp.EditValue) : (int?)null, !string.IsNullOrEmpty(txtBankPaymentNo.Text) ? txtBankPaymentNo.Text : null, start_Date_Report, start_Date_Report);
                else
                    paid_Com_Rport_Rslt = AyatProcessManager.AyatProcessManager.Get_Paid_Commssion_Report(sleEmp.EditValue != null ? Convert.ToInt16(sleEmp.EditValue) : (int?)null, !string.IsNullOrEmpty(txtBankPaymentNo.Text) ? txtBankPaymentNo.Text : null, start_Date_Report, end_Date_Report);

                if (paid_Com_Rport_Rslt != null && paid_Com_Rport_Rslt.Count > 0)
                {
                    Paid_Commission_Report = paid_Com_Rport_Rslt.GroupBy(x => new { x.CPID, x.EMPID }).Select(y => new Paid_Commission_Report
                    {
                        Employee_Name = y.First().fullName,
                        Employee_Type = y.First().EMPstatus,
                        Role = y.First().commissionRole,
                        Bank_Payment_No = y.First().BankPaymentNo,
                        Amount = paid_Com_Rport_Rslt.Where(x => x.CPID == y.Key.CPID && x.EMPID == y.Key.EMPID).GroupBy(x => x.CPDID).Sum(t => t.First().CPDAmount),
                        Bank_Payment_Date = y.First().BankPaymentDate,
                        User = y.First().UserName,
                        Detail = Get_Paid_Com_Report_Detail(y.Key.CPID, y.Key.EMPID)
                    }).ToList();
                    grdEmployeePaidComissions.DataSource = Paid_Commission_Report;
                    grdEmployeePaidComissions.RefreshDataSource();
                    gvEmployeePaidComissions.BestFitColumns();
                }
                else grdEmployeePaidComissions.DataSource = null;
            }
            catch { }
        }

        private void cmbDateCriteriaEmp_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDateCriteriaEmp.EditValue != null && cmbDateCriteriaEmp.EditValue.ToString() != string.Empty)
                {
                    deEmpFrom.Properties.ReadOnly = true;
                    deEmpTo.Properties.ReadOnly = true;
                    deEmpFrom.Properties.ShowDropDown = ShowDropDown.Never;
                    deEmpTo.Properties.ShowDropDown = ShowDropDown.Never;
                    LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    LayConItReportTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    switch (cmbDateCriteriaEmp.EditValue.ToString().ToUpper())
                    {
                        case "TODAY":
                            deEmpFrom.EditValue = DateTime.Now;
                            LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "AT THE DAY OF":
                            deEmpFrom.EditValue = DateTime.Now;
                            deEmpFrom.Properties.ReadOnly = false;
                            deEmpFrom.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS WEEK":
                            deEmpFrom.EditValue = DateTime.Now.AddDays(-7);
                            deEmpTo.EditValue = DateTime.Now;
                            LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayConItReportTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS MONTH":
                            deEmpFrom.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            deEmpTo.EditValue = DateTime.Now;
                            LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayConItReportTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "THIS YEAR":
                            deEmpFrom.EditValue = new DateTime(DateTime.Now.Year, 1, 1);
                            deEmpTo.EditValue = DateTime.Now;
                            LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayConItReportTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "DATE RANGE":
                            deEmpFrom.Properties.ReadOnly = false;
                            deEmpTo.Properties.ReadOnly = false;
                            deEmpFrom.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            deEmpTo.Properties.ShowDropDown = ShowDropDown.SingleClick;
                            LayConItReportFrom.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayConItReportTo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                        case "ALL":
                            deEmpFrom.EditValue = null;
                            deEmpTo.EditValue = null;
                            break;
                    }
                }
            }
            catch { }
        }

        private void deEmpFrom_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (deEmpFrom.EditValue != null && deEmpFrom.EditValue.ToString() != string.Empty)
                    start_Date_Report = Convert.ToDateTime(deEmpFrom.EditValue);
                else start_Date_Report = null;
            }
            catch { start_Date_Report = null; }
        }

        private void deEmpTo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (deEmpTo.EditValue != null && deEmpTo.EditValue.ToString() != string.Empty)
                    end_Date_Report = Convert.ToDateTime(deEmpTo.EditValue);
                else end_Date_Report = null;
            }
            catch { end_Date_Report = null; }
        }
        #endregion
        #region Methods
        public void Initialize_Commission_Payment(bool first_Time = true)
        {
            try
            {
                if (first_Time)
                {
                    var All_Employees = AyatProcessManager.AyatProcessManager.EmployeeSelectAll();
                    sleEmployees.Properties.DataSource = All_Employees;
                    sleEmp.Properties.DataSource = All_Employees;
                }
                sleEmployees.EditValue = null;
                dePreparedDate.EditValue = DateTime.Now;
                txtCommissionDocumentTotal.Text = "";
                cmbDateCriteria.EditValue = "THIS MONTH";
                grdCommissionSummary.DataSource = null;
                CommissionTabbedControlGroup.SelectedTabPageIndex = 0;
            }
            catch { }
        }

        private decimal Get_Commission_Amount(int? employee_ID)
        {
            return commission_Details.Where(x => x.empID == employee_ID).GroupBy(x => x.commissionID).Sum(y => y.First().amount);
        }

        private List<Commission_Detail> Get_Commission_Detail(int? employee_ID)
        {
            return commission_Details.Where(x => x.empID == employee_ID).Select(y => new Commission_Detail
            {
                Sales_ID = y.salesID,
                Sales_Code = y.saleCode,
                employee_ID = y.empID,
                role_ID = y.commissionRoleID,
                commission_ID = y.commissionID,
                commission_Status = y.commissionStatus,
                Payment_Description = y.paymentPlanDexcription,
                Payment_Date = y.paymentDate,
                Commission_Amount = y.amount,
                Payment_Amount = y.paymentAmount,
                Receipt_NO = y.recieptNo,
                FS_NO = y.FsNo
            }).ToList();
        }

        private List<Report_Detail> Get_Paid_Com_Report_Detail(int CPID, int? EMPID)
        {
            return paid_Com_Rport_Rslt.Where(x => x.CPID == CPID && x.EMPID == EMPID).GroupBy(x => x.CPDID).Select(y => new Report_Detail
            {
                Sales_Code = y.First().salesCode,
                Customer = y.First().Customer,
                Site = y.First().Site,
                Building_No = y.First().BuildingNo,
                Payment_For = y.First().PPDescription,
                Commission_Amount = y.First().amount,
                Payment_Detail = Get_Payment_Detail(y.Key)
            }).ToList();
        }

        private List<Payment_Detail> Get_Payment_Detail(int CPDID)
        {
            return paid_Com_Rport_Rslt.Where(x => x.CPDID == CPDID).Select(y => new Payment_Detail
            {
                FS_No = y.FsNo,
                Receipt_No = y.recieptNo,
                Paid_Amount = y.PAmount,
                Payment_Date = y.PDate
            }).ToList();
        }

        private void Get_Prepared_Commission_Payments()
        {
            try
            {
                commission_Payments_And_Details = AyatProcessManager.AyatProcessManager.Get_Commission_Payments_And_Details((int)Commission_Payment_Status.prepared);
                if (commission_Payments_And_Details != null && commission_Payments_And_Details.Count > 0)
                {
                    Prepared_Commission_Payments = commission_Payments_And_Details.GroupBy(x => x.ID).Select(x => new Prepared_Commission
                    {
                        ID = x.Key,
                        Issued_Date = x.First().IssuedDate,
                        Prepared_Date = x.First().PreparedDate,
                        Amount = x.First().Amount,
                        user = x.First().PreparingUser,
                        deviceName = x.First().PreparingDevice,
                        Details = commission_Payments_And_Details.Select(y => new Commission_Payment_Detail
                        {
                            Employee_Name = y.fullName,
                            Role = y.commissionRole,
                            Amount = y.employeeCommission.Value
                        }).ToList()
                    }).ToList();
                    grdPreparedCommissionPayments.DataSource = Prepared_Commission_Payments;
                    gvPreparedCommissionPayments.BestFitColumns();
                    gvPreparedCommissionPayments.FocusInvalidRow();
                }
                else
                    grdPreparedCommissionPayments.DataSource = null;

                txtBankPaymentNo.Text = "";
                deBankPayment.EditValue = DateTime.Now;
            }
            catch { }
        }

        private bool Validate_Commission_Payment_Saving()
        {
            try
            {
                if (dePreparedDate.EditValue == null)
                {
                    XtraMessageBox.Show("Please Select Prepared Date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                gvCommissionSummary.PostEditor();
                commission_Summary = grdCommissionSummary.DataSource as List<Commission_Summary>;
                if (commission_Summary == null)
                {
                    XtraMessageBox.Show("Please Show Commissions First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commission_Summary.FirstOrDefault(x => x.selected) == null)
                {
                    XtraMessageBox.Show("Please Select Commissions First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commission_Summary.Where(x => x.selected).SelectMany(x => x.Commision_Details).Any(x => x.commission_Status == (int)Commission_Status.pendingBankPayment))
                {
                    XtraMessageBox.Show("There Are Some Commission Payments Pending Bank Payment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        private bool Validate_Commision_Payment_Approving()
        {
            try
            {
                if (string.IsNullOrEmpty(txtBankPaymentNo.Text))
                {
                    XtraMessageBox.Show("Please Enter Bank Payment Number First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (deBankPayment.EditValue == null)
                {
                    XtraMessageBox.Show("Please Select Bank Payment Date First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}
