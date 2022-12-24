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

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCCollection : DevExpress.XtraEditors.XtraUserControl
    {
        private Sale selected_Sale { get; set; }
        private List<PaymentPlan> sales_Payment_Plan { get; set; }
        private PaymentPlan selected_Payment_Plan { get; set; }
        private decimal remaining_Amount { get; set; }
        private decimal payment { get; set; }
        private decimal paid_Amount { get; set; }
        private List<Commission> payment_Commissions { get; set; }
        private Payment payment_Obj { get; set; }

        public event EventHandler close_Button_Clicked;
        private DXErrorProvider paymentErrorProvider { get; set; }
        private int error { get; set; }
        public UCCollection()
        {
            InitializeComponent();
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {
            tabbedControlGroup1.SelectedTabPageIndex = 1;
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            tabbedControlGroup1.SelectedTabPageIndex = 0;
        }

        public void Initalize_Payment()
        {
            try
            {
                sleSales.Properties.DataSource = AyatProcessManager.AyatProcessManager.Get_Sales_By_Status((int)Sales_Status.approved);
                sleSales.EditValue = null;
                txtCustomer.Text = ""; 
                txtSite.Text = "";
                txtBuildingNo.Text = "";
                txtHouseNo.Text = "";
                txtSalesDate.Text = "";
                txtSalesTotal.Text = "";
                txtTotalPaid.Text = "";
                txtRemaining.Text = "";
                txtFsNo.Text = "";
                txtReceiptNo.Text = "";
                txtAmount.Text = "";
                dePaymentDate.EditValue = DateTime.Now;
                txtReceivedBy.Text = "";
                gcRemainingPayments.DataSource = null;
                gcPaidPayments.DataSource = null;
                tabbedControlGroup1.SelectedTabPageIndex = 0;
            }
            catch { }
        }

        private void sleSales_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                selected_Sale = sleSales.GetSelectedDataRow() as Sale;
                if (selected_Sale != null)
                {
                    sales_Payment_Plan = AyatProcessManager.AyatProcessManager.GetPaymentPlanBySalesID(selected_Sale.ID);
                    if (sales_Payment_Plan != null && sales_Payment_Plan.Count > 0)
                    {
                        if (sales_Payment_Plan.FirstOrDefault(x => x.payStatus == (int)Payment_Status.notPaid) != null)
                        {
                            txtCustomer.Text = selected_Sale.FAName;
                            txtSite.Text = selected_Sale.Site;
                            txtBuildingNo.Text = selected_Sale.BuildingNo;
                            txtHouseNo.Text = selected_Sale.HouseNo;
                            txtSalesDate.Text = selected_Sale.Date.ToString();
                            txtSalesTotal.Text = selected_Sale.Total.ToString();
                            txtTotalPaid.Text = sales_Payment_Plan.Sum(x => x.paidAmount).ToString();
                            txtRemaining.Text = sales_Payment_Plan.Sum(x => x.RemainingAmount).ToString();
                            txtFsNo.Text = "";
                            txtReceiptNo.Text = "";
                            txtAmount.Text = "";
                            dePaymentDate.EditValue = DateTime.Now;
                            txtReceivedBy.Text = DataBuffer.thisUser.name;
                            gcRemainingPayments.DataSource = sales_Payment_Plan.Where(x => x.payStatus == (int)Payment_Status.notPaid).ToList();
                            gcRemainingPayments.RefreshDataSource();
                            gvRemainingPayments.BestFitColumns();
                            gcPaidPayments.DataSource = sales_Payment_Plan.Where(x => x.payStatus == (int)Payment_Status.paid).ToList();
                            gcPaidPayments.RefreshDataSource();
                            gvPaidPayments.BestFitColumns();
                        }
                        else
                        {
                            XtraMessageBox.Show("All Payments For The Selected Sales Are Compeleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sleSales.EditValue = null;
                            txtCustomer.Text = "";
                            txtSite.Text = "";
                            txtBuildingNo.Text = "";
                            txtHouseNo.Text = "";
                            txtSalesDate.Text = "";
                            txtSalesTotal.Text = "";
                            txtTotalPaid.Text = "";
                            txtRemaining.Text = "";
                            txtFsNo.Text = "";
                            txtReceiptNo.Text = "";
                            txtAmount.Text = "";
                            dePaymentDate.EditValue = DateTime.Now;
                            txtReceivedBy.Text = "";
                            gcRemainingPayments.DataSource = null;
                            gcPaidPayments.DataSource = null;
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Please Select Sales With A Payment Plan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sleSales.EditValue = null;
                        txtCustomer.Text = "";
                        txtSite.Text = "";
                        txtBuildingNo.Text = "";
                        txtHouseNo.Text = "";
                        txtSalesDate.Text = "";
                        txtSalesTotal.Text = "";
                        txtTotalPaid.Text = "";
                        txtRemaining.Text = "";
                        txtFsNo.Text = "";
                        txtReceiptNo.Text = "";
                        txtAmount.Text = "";
                        dePaymentDate.EditValue = DateTime.Now;
                        txtReceivedBy.Text = "";
                        gcRemainingPayments.DataSource = null;
                        gcPaidPayments.DataSource = null;
                    }
                }
                else
                {
                    txtCustomer.Text = "";
                    txtSite.Text = "";
                    txtBuildingNo.Text = "";
                    txtHouseNo.Text = "";
                    txtSalesDate.Text = "";
                    txtSalesTotal.Text = "";
                    txtTotalPaid.Text = "";
                    txtRemaining.Text = "";
                    txtFsNo.Text = "";
                    txtReceiptNo.Text = "";
                    txtAmount.Text = "";
                    dePaymentDate.EditValue = DateTime.Now;
                    txtReceivedBy.Text = "";
                    gcRemainingPayments.DataSource = null;
                    gcRemainingPayments.RefreshDataSource();
                    gcPaidPayments.DataSource = null;
                    gcPaidPayments.RefreshDataSource();
                }
            }
            catch { }
        } 

        private void PaymentUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
            {
                case "new":
                    sleSales.EditValue = null;
                    break;
                case "cancel":
                    close_Button_Clicked?.Invoke(sender, e);
                    break;
                case "save":
                    if (Validate_Payment())
                    {
                        payment = Convert.ToDecimal(txtAmount.Text);
                        foreach (var payment_Plan in sales_Payment_Plan.Where(x => x.payStatus == (int)Payment_Status.notPaid).ToList())
                        {
                            if (payment > 0)
                            {
                                if (payment >= payment_Plan.RemainingAmount)
                                {
                                    payment_Obj = new Payment
                                    {
                                        salesID = payment_Plan.salesID,
                                        payID = payment_Plan.ID,
                                        FsNo = txtFsNo.Text,
                                        recieptNo = txtReceiptNo.Text,
                                        date = Convert.ToDateTime(dePaymentDate.EditValue),
                                        amount = payment_Plan.RemainingAmount
                                    };

                                    if (!AyatProcessManager.AyatProcessManager.PaymentInsert(payment_Obj))
                                    {
                                        XtraMessageBox.Show("Failed To Save Payment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                    }
                                    remaining_Amount = 0;
                                    paid_Amount = payment_Plan.payAmount;
                                    payment = payment - payment_Plan.RemainingAmount;
                                }
                                else
                                {
                                    payment_Obj = new Payment
                                    {
                                        salesID = payment_Plan.salesID,
                                        payID = payment_Plan.ID,
                                        FsNo = txtFsNo.Text,
                                        recieptNo = txtReceiptNo.Text,
                                        date = Convert.ToDateTime(dePaymentDate.EditValue),
                                        amount = payment
                                    };

                                    if (!AyatProcessManager.AyatProcessManager.PaymentInsert(payment_Obj))
                                    {
                                        XtraMessageBox.Show("Failed To Save Payment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                    }

                                    remaining_Amount = payment_Plan.RemainingAmount - payment;
                                    paid_Amount = payment_Plan.paidAmount + payment;
                                    payment = 0;
                                }
                                payment_Plan.RemainingAmount = remaining_Amount;
                                payment_Plan.paidAmount = paid_Amount;
                                payment_Plan.payStatus = remaining_Amount > 0 ? (int)Payment_Status.notPaid : (int)Payment_Status.paid;

                                if (AyatProcessManager.AyatProcessManager.PaymentPlanUpdate(payment_Plan))
                                {
                                    if (payment_Plan.payStatus == (int)Payment_Status.paid)
                                    {
                                        payment_Commissions = AyatProcessManager.AyatProcessManager.GetCommissionByPaymentPlanId(payment_Plan.ID);
                                        if (payment_Commissions != null && payment_Commissions.Count > 0)
                                        {
                                            payment_Commissions.ForEach(x => x.status = (int)Commission_Status.available);
                                            if (!AyatProcessManager.AyatProcessManager.Update_Commission_List(payment_Commissions))
                                            {
                                                XtraMessageBox.Show("Failed To Save Payment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    XtraMessageBox.Show("Failed To Save Payment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }
                            }
                        }
                        XtraMessageBox.Show("Payment Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sleSales.EditValue = null;
                    }
                    break;
            }
        }
        private bool Validate_Payment()
        {
            try
            {
                error = 0;
                paymentErrorProvider.ClearErrors();
                if (sleSales.EditValue == null || string.IsNullOrEmpty(sleSales.EditValue.ToString()))
                {
                    paymentErrorProvider.SetError(sleSales, "Please Select Sales", ErrorType.Critical);
                    error = 1;
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtReceiptNo.Text) && !string.IsNullOrEmpty(txtFsNo.Text))
                    {
                        var existing_Payment = AyatProcessManager.AyatProcessManager.Get_Payment_By_FS_And_Receipt_No(txtFsNo.Text, txtReceiptNo.Text);
                        if (existing_Payment != null && existing_Payment.Count > 0)
                        {
                            paymentErrorProvider.SetError(txtFsNo, "There Is Already A Payment With The Same FS And Receipt Number", ErrorType.Critical);
                            paymentErrorProvider.SetError(txtReceiptNo, "There Is Already A Payment With The Same FS And Receipt Number", ErrorType.Critical);
                            error = 1;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtFsNo.Text))
                        {
                            paymentErrorProvider.SetError(txtFsNo, "Please Enter Payment FS Number", ErrorType.Critical);
                            error = 1;
                        }
                        if (string.IsNullOrEmpty(txtReceiptNo.Text))
                        {
                            paymentErrorProvider.SetError(txtReceiptNo, "Please Enter Payment Receipt Number", ErrorType.Critical);
                            error = 1;
                        }
                    }
                    if(string.IsNullOrEmpty(txtAmount.Text) || Convert.ToDecimal(txtAmount.Text) == 0)
                    {
                        paymentErrorProvider.SetError(txtAmount, "Please Enter Payment Amount", ErrorType.Critical);
                        error = 1;
                    }
                    if(dePaymentDate.EditValue == null || string.IsNullOrEmpty(dePaymentDate.EditValue.ToString()))
                    {
                        paymentErrorProvider.SetError(dePaymentDate, "Please Select Payment Date", ErrorType.Critical);
                        error = 1;
                    }
                }
                return error == 0;
            }
            catch { return false; }
        }

        private void gvRemainingPayments_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                selected_Payment_Plan = gvRemainingPayments.GetFocusedRow() as PaymentPlan;
                if (selected_Payment_Plan != null && sales_Payment_Plan.FirstOrDefault(x => x.ID < selected_Payment_Plan.ID && x.payStatus == (int)Payment_Status.notPaid) == null)
                {
                    txtAmount.Text = selected_Payment_Plan.RemainingAmount.ToString();
                }
            }
            catch { }
        }

        private void UCCollection_Load(object sender, EventArgs e)
        {
            paymentErrorProvider = new DXErrorProvider();
        }
    }
}
