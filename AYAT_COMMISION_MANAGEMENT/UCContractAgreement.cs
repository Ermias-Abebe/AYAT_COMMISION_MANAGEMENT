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
using DevExpress.XtraEditors.DXErrorProvider;
using AyatDataAccess;
using AyatProcessManager;
using DevExpress.XtraLayout;
using System.Globalization;
using static AyatProcessManager.Custom_Classes;
using DevExpress.XtraGrid.Views.Base;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCContractAgreement : DevExpress.XtraEditors.XtraUserControl
    {
        #region Declaration
        public event EventHandler close_Button_Clicked;
        private DXErrorProvider salesErrorProvider { get; set; }
        private CultureInfo[] cultures { get; set; }
        private BindingList<Payment_Plan> payment_Plan_Obj { get; set; }
        private PaymentPlan payment_Plan { get; set; }
        private Commission commission { get; set; }
        private List<Role_Employees> role_Employees { get; set; }
        private int error { get; set; }
        private Sale sales { get; set; }

        private decimal first_Pay;

        private decimal pay_Amount;

        private decimal remaining_Amount;

        private decimal paid_Amount;

        private decimal? comm_Incriment;

        private decimal? commission_Incr;

        private string commission_Remark;
        private ColumnView column_View { get; set; }
        private Role_Employees focused_Role { get; set; }
        private Employee role_Selected_Employee { get; set; }
        #endregion

        public UCContractAgreement()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void UCContractAgreement_Load(object sender, EventArgs e)
        {
            try
            {
                salesErrorProvider = new DXErrorProvider();
                cmbCountry.Properties.Items.Clear();
                cmbCountry.Properties.Items.AddRange(GetCountries());
            }
            catch { }
        }
        private void salesCenterUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
            {

                case "next":
                    switch (navigationFrameSalesCenter.SelectedPageIndex)
                    {
                        case 0:
                            if (Validate_Sales_Info())
                            {
                                salesCenterUIButtonPanel.Buttons[0].Properties.Visible = true;
                                salesCenterUIButtonPanel.Buttons[1].Properties.Visible = true;
                                salesCenterUIButtonPanel.Buttons[2].Properties.Visible = true;
                                salesCenterUIButtonPanel.Buttons[3].Properties.Visible = true;
                                salesCenterUIButtonPanel.Buttons[4].Properties.Visible = false;
                                salesCenterUIButtonPanel.Buttons[5].Properties.Visible = false;
                                navigationFrameSalesCenter.SelectedPageIndex = 1;
                            }
                            break;
                        case 1:
                            salesCenterUIButtonPanel.Buttons[0].Properties.Visible = false;
                            salesCenterUIButtonPanel.Buttons[1].Properties.Visible = false;
                            salesCenterUIButtonPanel.Buttons[2].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[3].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[4].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[5].Properties.Visible = true;
                            navigationFrameSalesCenter.SelectedPageIndex = 2;
                            break;
                    }
                    break;
                case "back":
                    switch (navigationFrameSalesCenter.SelectedPageIndex)
                    {
                        case 1:
                            salesCenterUIButtonPanel.Buttons[0].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[1].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[2].Properties.Visible = false;
                            salesCenterUIButtonPanel.Buttons[3].Properties.Visible = false;
                            salesCenterUIButtonPanel.Buttons[4].Properties.Visible = false;
                            salesCenterUIButtonPanel.Buttons[5].Properties.Visible = false;
                            navigationFrameSalesCenter.SelectedPageIndex = 0;
                            break;
                        case 2:
                            salesCenterUIButtonPanel.Buttons[0].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[1].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[2].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[3].Properties.Visible = true;
                            salesCenterUIButtonPanel.Buttons[4].Properties.Visible = false;
                            salesCenterUIButtonPanel.Buttons[5].Properties.Visible = false;
                            navigationFrameSalesCenter.SelectedPageIndex = 1;
                            break;
                    }
                    break;
                case "save":
                    if (Vaidate_Payment_Plan())
                    {
                        sales = new Sale
                        {
                            code = txtSalesCode.Text,
                            Type = cmbSalesType.EditValue.ToString() == "Cash" ? (int)Sales_Type.Cash : (int)Sales_Type.Credit,
                            Date = Convert.ToDateTime(deSalesDate.EditValue),
                            Total = Convert.ToDecimal(txtTotal.Text),
                            FirstPayment = !string.IsNullOrEmpty(txtFirstPayment.Text) ? Convert.ToDecimal(txtFirstPayment.Text) : 0,
                            VAT = Math.Round((Convert.ToDecimal(txtTotal.Text) * (decimal)0.15) / (decimal)1.15, 2, MidpointRounding.AwayFromZero),
                            Ad = !string.IsNullOrEmpty(txtAdvertizing.Text) ? Convert.ToDecimal(txtAdvertizing.Text) : 0,
                            RegFee = !string.IsNullOrEmpty(txtRegFee.Text) ? Convert.ToDecimal(txtRegFee.Text) : 0,
                            FAName = Append_Name(txtFirstAppFirstName.Text, txtFirstAppMiddleName.Text, txtFirstAppLastName.Text),
                            SAName = Append_Name(txtSecondAppFirstName.Text, txtSecondAppMiddleName.Text, txtSecondAppLastName.Text),
                            EthCity = txtEthCity.Text,
                            EthSubCity = txtEthSubCity.Text,
                            EthWoreda = txtEthWoreda.Text,
                            EthHouseNo = txtEthHouseNo.Text,
                            EthHomePhone = txtEthHomePhone.Text,
                            EthMobileNo = txtEthMobilePhone.Text,
                            EthEmail = txtEthEmail.Text,
                            Country = cmbCountry.EditValue != null ? cmbCountry.EditValue.ToString() : null,
                            City = txtOutCity.Text,
                            POBox = txtOutPOBox.Text,
                            StreetCode = txtOutStreetCode.Text,
                            HomePhone = txtOutHomePhone.Text,
                            MobilePhone = txtOutMobilePhone.Text,
                            Email = txtOutEmail.Text,
                            Site = txtSite.Text,
                            HouseType = cmbHouseType.EditValue.ToString() == "Semi Furnished" ? (int)House_Type.Semi_Finished : (int)House_Type.Fully_Furnished,
                            BuildingNo = txtBuildingNo.Text,
                            FloorNo = Convert.ToInt16(txtFloorNo.Text),
                            HouseNo = txtHouseNo.Text,
                            Area = Convert.ToDecimal(txtArea.Text),
                            BedRoom = Convert.ToInt16(txtBedRoom.Text),
                            isActive = true,
                            status = (int)Sales_Status.prepared,
                            PrepareuserID = DataBuffer.thisUser.ID,
                            PreparedeviceID = DataBuffer.thisDevice.ID
                        };
                        sales.ID = AyatProcessManager.AyatProcessManager.SalesInsert(sales).Value;
                        payment_Plan_Obj = gcPaymentPlan.DataSource as BindingList<Payment_Plan>;
                        gvRoles.PostEditor();
                        role_Employees = gcRoles.DataSource as List<Role_Employees>;


                        if (role_Employees != null && role_Employees.Count > 0)
                        {
                            role_Selected_Employee = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == role_Employees.FirstOrDefault(y => y.role_Index == 1).role_Employee);
                            if (role_Selected_Employee != null)
                            {
                                if (role_Selected_Employee.status != "Freelance" && DataBuffer.RoleList.FirstOrDefault(x => x.ID == role_Selected_Employee.role).type != (int)Role_Type.Normal)
                                {
                                    if (DataBuffer.RoleList.FirstOrDefault(x => x.type == (int)Role_Type.OtherEmployee) != null)
                                    {
                                        role_Employees.Add(new Role_Employees
                                        {
                                            role_Id = DataBuffer.RoleList.FirstOrDefault(x => x.type == (int)Role_Type.OtherEmployee).ID,
                                            role_Name = DataBuffer.RoleList.FirstOrDefault(x => x.type == (int)Role_Type.OtherEmployee).description,
                                            role_Type = DataBuffer.RoleList.FirstOrDefault(x => x.type == (int)Role_Type.OtherEmployee).type
                                        });
                                    }
                                }
                            }
                            foreach (var role_Emp in role_Employees)
                            {
                                if (sales.Type == (int)Sales_Type.Cash)
                                {
                                    if (sales.HouseType == (int)House_Type.Semi_Finished)
                                        role_Emp.role_Commission_Rate = DataBuffer.RoleList.FirstOrDefault(x => x.ID == role_Emp.role_Id).cashSF;
                                    else
                                        role_Emp.role_Commission_Rate = DataBuffer.RoleList.FirstOrDefault(x => x.ID == role_Emp.role_Id).cashRF;
                                }
                                else
                                    role_Emp.role_Commission_Rate = DataBuffer.RoleList.FirstOrDefault(x => x.ID == role_Emp.role_Id).loanRF;
                            }
                        }

                        if (payment_Plan_Obj != null && payment_Plan_Obj.Count > 0)
                        {
                            first_Pay = sales.FirstPayment.Value;
                            pay_Amount = 0;
                            remaining_Amount = 0;
                            paid_Amount = 0;
                            foreach (var pay in payment_Plan_Obj)
                            {
                                if (pay.payment == 0 && pay.comission == 0)
                                    continue;
                                pay_Amount = (sales.Total * pay.payment) / 100;
                                if (first_Pay > 0)
                                {
                                    if (first_Pay >= pay_Amount)
                                    {
                                        remaining_Amount = 0;
                                        paid_Amount = pay_Amount;
                                        first_Pay = first_Pay - pay_Amount;
                                    }
                                    else
                                    {
                                        remaining_Amount = pay_Amount - first_Pay;
                                        paid_Amount = first_Pay;
                                        first_Pay = 0;
                                    }
                                    payment_Plan = new PaymentPlan
                                    {
                                        salesID = sales.ID,
                                        description = pay.description,
                                        payRate = pay.payment,
                                        payAmount = pay_Amount,
                                        RemainingAmount = remaining_Amount,
                                        paidAmount = paid_Amount,
                                        payStatus = remaining_Amount > 0 ? (int)Payment_Status.notPaid : (int)Payment_Status.paid,
                                        commRate = pay.comission,
                                        commAmount = 0,
                                    };
                                }
                                else
                                {
                                    payment_Plan = new PaymentPlan
                                    {
                                        salesID = sales.ID,
                                        description = pay.description,
                                        payRate = pay.payment,
                                        payAmount = pay_Amount,
                                        RemainingAmount = pay_Amount,
                                        paidAmount = 0,
                                        payStatus = (int)Payment_Status.notPaid,
                                        commRate = pay.comission,
                                        commAmount = 0,
                                    };
                                }
                                payment_Plan.ID = AyatProcessManager.AyatProcessManager.PaymentPlanInsert(payment_Plan).Value;
                                if (pay.comission > 0)
                                {
                                    if (role_Employees != null && role_Employees.Count > 0)
                                    {
                                        foreach (var role_Emp in role_Employees)
                                        {
                                            if ((role_Emp.role_Type == (int)Role_Type.OtherEmployee || role_Emp.role_Employee != null) && role_Emp.role_Commission_Rate > 0)
                                            {
                                                commission_Incr = null;
                                                commission_Remark = null;
                                                commission = new Commission
                                                {
                                                    salesID = sales.ID,
                                                    payID = payment_Plan.ID,
                                                    empID = role_Emp.role_Employee != null ? role_Emp.role_Employee.Value : (int?)null,
                                                    role = role_Emp.role_Id,
                                                    amount = Math.Round(Calculate_Commission(sales.Total, sales.VAT, sales.Ad == null ? 0 : sales.Ad.Value, role_Emp.role_Commission_Rate, pay.comission, role_Emp.role_Id, sales.FirstPayment, ref commission_Incr, ref commission_Remark), 2, MidpointRounding.AwayFromZero),
                                                    rate = commission_Incr == null ? role_Emp.role_Commission_Rate : role_Emp.role_Commission_Rate + commission_Incr.Value,
                                                    status = payment_Plan.payStatus == (int)Payment_Status.paid ? (int)Commission_Status.available : (int)Commission_Status.notAvailable,
                                                    remark = commission_Remark
                                                };
                                                AyatProcessManager.AyatProcessManager.CommissionInsert(commission);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        XtraMessageBox.Show("Sales Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initalize_Sales();
                    }
                    break;
                case "close":
                    close_Button_Clicked?.Invoke(sender, e);
                    break;
                case "reports":

                    break;
            }
        }
        private void gvRoles_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                column_View = (ColumnView)sender;
                if (column_View.FocusedColumn.FieldName == "role_Employee")
                {
                    focused_Role = column_View.GetFocusedRow() as Role_Employees;
                    if (focused_Role != null && focused_Role.role_Index == 1)
                        ((SearchLookUpEdit)column_View.ActiveEditor).Properties.DataSource = DataBuffer.EmployeeList;
                    else
                        ((SearchLookUpEdit)column_View.ActiveEditor).Properties.DataSource = DataBuffer.EmployeeList.Where(x => x.role == focused_Role.role_Id).ToList();
                }
            }
            catch { }
        }
        private void sleEmployees_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gvRoles.PostEditor();
                focused_Role = gvRoles.GetFocusedRow() as Role_Employees;
                if (focused_Role != null && focused_Role.role_Index != null && focused_Role.role_Index.Value == 1)
                {
                    role_Selected_Employee = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == focused_Role.role_Employee);
                    if (role_Selected_Employee != null)
                    {
                        if (role_Selected_Employee.status == "Freelance" || DataBuffer.RoleList.FirstOrDefault(x => x.ID == role_Selected_Employee.role).type == (int)Role_Type.Normal)
                        {
                            role_Employees = new List<Role_Employees>();
                            role_Employees.Add(focused_Role);
                            sleEmployees.DataSource = DataBuffer.EmployeeList;
                            gcRoles.DataSource = role_Employees;
                            gcRoles.RefreshDataSource();
                        }
                        else if (DataBuffer.RoleList.FirstOrDefault(x => x.ID == role_Selected_Employee.role).type == (int)Role_Type.Sales)
                        {
                            role_Employees = new List<Role_Employees>();
                            role_Employees.Add(focused_Role);
                            role_Employees.AddRange(DataBuffer.RoleList.Where(x => x.type == (int)Role_Type.Normal || (x.orderIndex > DataBuffer.RoleList.FirstOrDefault(y => y.ID == role_Selected_Employee.role).orderIndex)).Select(x => new Role_Employees
                            {
                                role_Id = x.ID,
                                role_Name = x.description,
                                role_Type = x.type,
                                role_Index = x.orderIndex
                            }).ToList());
                            gcRoles.DataSource = role_Employees;
                            gcRoles.RefreshDataSource();
                        }
                    }
                    else
                    {
                        role_Employees = DataBuffer.RoleList.Where(x => x.type != (int)Role_Type.OtherEmployee).Select(x => new Role_Employees
                        {
                            role_Id = x.ID,
                            role_Name = x.description,
                            role_Type = x.type,
                            role_Index = x.orderIndex
                        }).ToList();
                        gcRoles.DataSource = role_Employees;
                        gcRoles.RefreshDataSource();
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Methods
        public List<string> GetCountries()
        {
            try
            {
                cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.SpecificCultures);
                if (cultures != null && cultures.Length > 0)
                {
                    return cultures.Where(C => C.LCID != 127 & !C.IsNeutralCulture).Select(C => new { new RegionInfo(C.LCID).EnglishName, new RegionInfo(C.LCID).Name }).ToList().GroupBy(C => new { C.EnglishName, C.Name }).Select(C => C.Key.EnglishName).ToList();
                }
                else return new List<string>();
            }
            catch { return new List<string>(); }
        }
        public void Initalize_Sales()
        {
            try
            {
                navigationFrameSalesCenter.SelectedPageIndex = 0;
                salesCenterUIButtonPanel.Buttons[0].Properties.Visible = true;
                salesCenterUIButtonPanel.Buttons[1].Properties.Visible = true;
                salesCenterUIButtonPanel.Buttons[2].Properties.Visible = false;
                salesCenterUIButtonPanel.Buttons[3].Properties.Visible = false;
                salesCenterUIButtonPanel.Buttons[4].Properties.Visible = false;
                salesCenterUIButtonPanel.Buttons[5].Properties.Visible = false;
                txtSalesCode.Text = "";
                cmbSalesType.EditValue = null;
                deSalesDate.EditValue = DateTime.Now;
                txtTotal.Text = "";
                txtFirstPayment.Text = "";
                txtAdvertizing.Text = "";
                txtRegFee.Text = "";
                txtFirstAppFirstName.Text = "";
                txtFirstAppMiddleName.Text = "";
                txtFirstAppLastName.Text = "";
                txtSecondAppFirstName.Text = "";
                txtSecondAppMiddleName.Text = "";
                txtSecondAppLastName.Text = "";
                txtEthCity.Text = "";
                txtEthSubCity.Text = "";
                txtEthWoreda.Text = "";
                txtEthHouseNo.Text = "";
                txtEthHomePhone.Text = "";
                txtEthMobilePhone.Text = "";
                txtEthEmail.Text = "";
                cmbCountry.EditValue = null;
                txtOutCity.Text = "";
                txtOutPOBox.Text = "";
                txtOutStreetCode.Text = "";
                txtOutHomePhone.Text = "";
                txtOutMobilePhone.Text = "";
                txtOutEmail.Text = "";
                txtSite.Text = "";
                txtBuildingNo.Text = "";
                txtFloorNo.Text = "";
                txtHouseNo.Text = "";
                txtArea.Text = "";
                txtBedRoom.Text = "";
                cmbHouseType.EditValue = null;

                payment_Plan_Obj = new BindingList<Payment_Plan>();
                gcPaymentPlan.DataSource = payment_Plan_Obj;
                gcPaymentPlan.RefreshDataSource();

                if (DataBuffer.RoleList != null && DataBuffer.RoleList.Where(x => x.type != (int)Custom_Classes.Role_Type.OtherEmployee).ToList().Count > 0)
                {
                    role_Employees = DataBuffer.RoleList.Where(x => x.type != (int)Role_Type.OtherEmployee).Select(x => new Role_Employees
                    {
                        role_Id = x.ID,
                        role_Name = x.description,
                        role_Type = x.type,
                        role_Index = x.orderIndex
                    }).ToList();
                    gcRoles.DataSource = role_Employees;
                    gcRoles.RefreshDataSource();
                    sleEmployees.DataSource = DataBuffer.EmployeeList;
                }
            }
            catch { }
        }
        private bool Validate_Sales_Info()
        {
            try
            {
                error = 0;
                salesErrorProvider.ClearErrors();
                if (string.IsNullOrEmpty(txtSalesCode.Text))
                {
                    salesErrorProvider.SetError(txtSalesCode, "Please Enter Sales Code", ErrorType.Critical);
                    error = 1;
                }
                if (cmbSalesType.EditValue == null || cmbSalesType.EditValue.ToString() == string.Empty)
                {
                    salesErrorProvider.SetError(cmbSalesType, "Please Select Sales Type", ErrorType.Critical);
                    error = 1;
                }
                if (deSalesDate.EditValue == null || deSalesDate.EditValue.ToString() == string.Empty)
                {
                    salesErrorProvider.SetError(deSalesDate, "Please Select Sales Date", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtTotal.Text))
                {
                    salesErrorProvider.SetError(txtTotal, "Please Enter Sales Total Amount", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtFirstAppFirstName.Text))
                {
                    salesErrorProvider.SetError(txtFirstAppFirstName, "Please Enter Applicant First Name", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtFirstAppMiddleName.Text))
                {
                    salesErrorProvider.SetError(txtFirstAppMiddleName, "Please Enter Applicant Middle Name", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtSite.Text))
                {
                    salesErrorProvider.SetError(txtSite, "Please Enter Site", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtBuildingNo.Text))
                {
                    salesErrorProvider.SetError(txtBuildingNo, "Please Enter Building No", ErrorType.Critical);
                    error = 1;
                }
                if (cmbHouseType.EditValue == null || cmbHouseType.EditValue.ToString() == string.Empty)
                {
                    salesErrorProvider.SetError(cmbHouseType, "Please Select House Type", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtFloorNo.Text))
                {
                    salesErrorProvider.SetError(txtFloorNo, "Please Enter Floor No", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtHouseNo.Text))
                {
                    salesErrorProvider.SetError(txtHouseNo, "Please Enter House No", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtArea.Text))
                {
                    salesErrorProvider.SetError(txtArea, "Please Enter Area", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtBedRoom.Text))
                {
                    salesErrorProvider.SetError(txtBedRoom, "Please Enter Bed Room", ErrorType.Critical);
                    error = 1;
                }

                return error == 0;
            }
            catch { return false; }
        }
        private bool Vaidate_Payment_Plan()
        {
            try
            {
                gvPaymentPlan.PostEditor();
                payment_Plan_Obj = gcPaymentPlan.DataSource as BindingList<Payment_Plan>;
                if (payment_Plan_Obj != null && payment_Plan_Obj.Count > 0)
                {
                    if (payment_Plan_Obj.Sum(x => x.payment) != 100)
                    {
                        XtraMessageBox.Show("Please Enter Valid Payment Plan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (payment_Plan_Obj.Sum(x => x.comission) != 100)
                    {
                        XtraMessageBox.Show("Please Enter Valid Payment Plan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    XtraMessageBox.Show("Please Enter Payment Plan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
        private string Append_Name(string first_Name, string middle_Name, string last_Name)
        {
            if (!string.IsNullOrEmpty(first_Name))
            {
                if (!string.IsNullOrEmpty(middle_Name))
                {
                    if (!string.IsNullOrEmpty(last_Name))
                        return string.Format("{0} {1} {2}", first_Name, middle_Name, last_Name);
                    else
                        return string.Format("{0} {1}", first_Name, middle_Name);
                }
                else
                {
                    if (!string.IsNullOrEmpty(last_Name))
                        return string.Format("{0} {1}", first_Name, last_Name);
                    else return first_Name;
                }
            }
            else return null;
        }
        private decimal Calculate_Commission(decimal total, decimal VAT, decimal Ad, decimal role_Commission,decimal payment_Commission, int role_ID,decimal? first_Payment, ref decimal? incr, ref string remark)
        {
            try
            {
                if(first_Payment != null && first_Payment > 0)
                {
                    comm_Incriment = null;
                    var role_Incriment = AyatProcessManager.AyatProcessManager.RoleIncrementsSelectByRole(role_ID);
                    if (role_Incriment != null && role_Incriment.Count > 0)
                    {
                        foreach(var inc in role_Incriment)
                        {
                            if (role_Incriment.IndexOf(inc) == 0)
                            {
                                if ((first_Payment.Value / total * 100) == inc.percentage)
                                {
                                    comm_Incriment = inc.increment;
                                    incr = comm_Incriment;
                                    remark = incr.ToString() + "% commission Increase b/c " + inc.percentage + "% first Payment";
                                    break;
                                }
                            }
                            else
                            {
                                if (role_Incriment.ElementAt(role_Incriment.IndexOf(inc) - 1).percentage > (first_Payment.Value / total * 100) && (first_Payment.Value / total * 100) >= inc.percentage)
                                {
                                    comm_Incriment = inc.increment;
                                    incr = comm_Incriment;
                                    remark = incr.ToString() + "% commission Increase b/c " + inc.percentage + "% first Payment";
                                    break;
                                }
                            }
                        }
                    }
                    if(comm_Incriment != null)
                        return (total - VAT - Ad) * ((role_Commission + comm_Incriment.Value) / 100) * (payment_Commission / 100);
                    else
                        return (total - VAT - Ad) * (role_Commission / 100) * (payment_Commission / 100);
                }
                else
                    return (total - VAT - Ad) * (role_Commission / 100) * (payment_Commission / 100);
            }
            catch
            {
                throw;
            }
        }
        #endregion        
    }
}
