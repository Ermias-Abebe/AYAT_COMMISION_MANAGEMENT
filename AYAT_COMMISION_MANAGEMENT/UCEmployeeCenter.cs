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
using AyatProcessManager;
using AyatDataAccess;
using DevExpress.XtraEditors.DXErrorProvider;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCEmployeeCenter : DevExpress.XtraEditors.XtraUserControl
    {
        public static int? employee_ID { get; set; }
        private Employee employee { get; set; }
        private int error { get; set; }
        private DXErrorProvider employeeErrorProvider { get; set; }

        public event EventHandler close_Button_Clicked;
        public UCEmployeeCenter()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void windowsUIButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch (e.Button.Properties.Caption)
            {
                case "Save":
                    if (Validate_Employee())
                    {
                        if (employee != null)
                        {
                            employee.firstName = txtFirstName.Text;
                            employee.middleName = txtMiddleName.Text;
                            employee.lastName = txtLastName.Text;
                            employee.fullName = Append_Name(txtFirstName.Text, txtMiddleName.Text, txtLastName.Text);
                            employee.gender = genderImageComboBoxEdit.EditValue.ToString();
                            employee.role = Convert.ToInt32(sleRole.EditValue);
                            employee.isActive = chkisactive.Checked;
                            employee.status = imcoStatus.EditValue.ToString();
                            employee.hiredate = Convert.ToDateTime(HireDateDateEdit.EditValue);
                            employee.dob = Convert.ToDateTime(DateOfBirthDateAndTime.EditValue);
                            employee.city = txtCity.Text;
                            employee.homephone = HomePhoneTextEdit.Text;
                            employee.mobilephone = MobilePhoneTextEdit.Text;
                            employee.email = EmailTextEdit.Text;
                            employee.supervisor = sleSupervisor.EditValue == null ? 0 : Convert.ToInt32(sleSupervisor.EditValue);
                            employee.remark = memoRemark.Text;
                            if (PEEmployeePhoto.Image != null)
                                employee.picture = MainForm.ImageToByteArray(PEEmployeePhoto.Image);


                            if (AyatProcessManager.AyatProcessManager.EmployeeUpdate(employee))
                            {
                                XtraMessageBox.Show("Employee Updated Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                DataBuffer.EmployeeList = AyatProcessManager.AyatProcessManager.EmployeeSelectAll();
                                employee_ID = null;
                                Initalize_Employee_Maintain();
                            }
                            else
                                XtraMessageBox.Show("Updating Employee Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            employee = new Employee
                            {
                                firstName = txtFirstName.Text,
                                middleName = txtMiddleName.Text,
                                lastName = txtLastName.Text,
                                fullName = Append_Name(txtFirstName.Text, txtMiddleName.Text, txtLastName.Text),
                                gender = genderImageComboBoxEdit.EditValue.ToString(),
                                role = Convert.ToInt32(sleRole.EditValue),
                                isActive = chkisactive.Checked,
                                status = imcoStatus.EditValue.ToString(),
                                hiredate = Convert.ToDateTime(HireDateDateEdit.EditValue),
                                dob = Convert.ToDateTime(DateOfBirthDateAndTime.EditValue),
                                city = txtCity.Text,
                                homephone = HomePhoneTextEdit.Text,
                                mobilephone = MobilePhoneTextEdit.Text,
                                email = EmailTextEdit.Text,
                                supervisor = sleSupervisor.EditValue == null ? 0 : Convert.ToInt32(sleSupervisor.EditValue),
                                remark = memoRemark.Text,
                                picture = PEEmployeePhoto.Image != null ? MainForm.ImageToByteArray(PEEmployeePhoto.Image) : null
                            };
                            if (AyatProcessManager.AyatProcessManager.EmployeeInsert(employee))
                            {
                                XtraMessageBox.Show("Employee Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                DataBuffer.EmployeeList = AyatProcessManager.AyatProcessManager.EmployeeSelectAll();
                                employee_ID = null;
                                Initalize_Employee_Maintain();
                            }
                            else
                                XtraMessageBox.Show("Saving Employee Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                case "New":
                    employee_ID = null;
                    Initalize_Employee_Maintain();
                    break;
                case "Close":
                    close_Button_Clicked?.Invoke(sender, e);
                    break;


            }

        }
        private void backWindowsUIButtonPanel_Click(object sender, EventArgs e)
        {
            close_Button_Clicked?.Invoke(sender, e);
        }
        #endregion

        #region Methods
        public void Initalize_Employee_Maintain()
        {
            try
            {
                if (employeeErrorProvider == null)
                    employeeErrorProvider = new DXErrorProvider();
                employeeErrorProvider.ClearErrors();
                sleSupervisor.Properties.DataSource = DataBuffer.EmployeeList;
                sleRole.Properties.DataSource = DataBuffer.RoleList.Where(x => x.type != (int)Custom_Classes.Role_Type.OtherEmployee).ToList();
                if (employee_ID == null)
                {
                    employee = null;
                    txtFirstName.Text = "";
                    txtMiddleName.Text = "";
                    txtLastName.Text = "";
                    genderImageComboBoxEdit.EditValue = null;
                    sleRole.EditValue = null;
                    imcoStatus.EditValue = null;
                    HireDateDateEdit.EditValue = DateTime.Now;
                    DateOfBirthDateAndTime.EditValue = null;
                    txtCity.Text = "";
                    HomePhoneTextEdit.Text = "";
                    MobilePhoneTextEdit.Text = "";
                    EmailTextEdit.Text = "";
                    sleSupervisor.EditValue = null;
                    chkisactive.Checked = true;
                    PEEmployeePhoto.Image = null;
                    memoRemark.Text = "";
                    grdEmployeeCommissionHistory.DataSource = null;
                    grdEmployeeCommissionHistory.RefreshDataSource();
                    gvEmployeeCommissionHistory.BestFitColumns();
                }
                else
                {
                    employee = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == employee_ID);
                    if (employee != null)
                    {
                        txtFirstName.Text = employee.firstName;
                        txtMiddleName.Text = employee.middleName;
                        txtLastName.Text = employee.lastName;
                        genderImageComboBoxEdit.EditValue = employee.gender;
                        sleRole.EditValue = employee.role;
                        imcoStatus.EditValue = employee.status;
                        HireDateDateEdit.EditValue = employee.hiredate;
                        DateOfBirthDateAndTime.EditValue = employee.dob;
                        txtCity.EditValue = employee.city;
                        HomePhoneTextEdit.Text = employee.homephone;
                        MobilePhoneTextEdit.Text = employee.mobilephone;
                        EmailTextEdit.Text = employee.email;
                        sleSupervisor.EditValue = employee.supervisor;
                        chkisactive.Checked = employee.isActive;
                        memoRemark.Text = employee.remark;
                        grdEmployeeCommissionHistory.DataSource = AyatProcessManager.AyatProcessManager.Get_Employee_Commission_Summary(employee.ID);
                        grdEmployeeCommissionHistory.RefreshDataSource();
                        gvEmployeeCommissionHistory.BestFitColumns();
                    }
                }
            }
            catch { }
        }
        private bool Validate_Employee()
        {
            try
            {
                employeeErrorProvider.ClearErrors();
                error = 0;
                if (string.IsNullOrEmpty(txtFirstName.Text))
                {
                    employeeErrorProvider.SetError(txtFirstName, "Please Enter First Name", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtMiddleName.Text))
                {
                    employeeErrorProvider.SetError(txtMiddleName, "Please Enter Middle Name", ErrorType.Critical);
                    error = 1;
                }
                if (genderImageComboBoxEdit.EditValue == null || genderImageComboBoxEdit.EditValue.ToString() == "")
                {
                    employeeErrorProvider.SetError(genderImageComboBoxEdit, "Please Select Gender", ErrorType.Critical);
                    error = 1;
                }
                if (sleRole.EditValue == null || sleRole.EditValue.ToString() == "")
                {
                    employeeErrorProvider.SetError(sleRole, "Please Select Department", ErrorType.Critical);
                    error = 1;
                }
                if (imcoStatus.EditValue == null || imcoStatus.EditValue.ToString() == "")
                {
                    employeeErrorProvider.SetError(imcoStatus, "Please Select Status", ErrorType.Critical);
                    error = 1;
                }
                else if (imcoStatus.EditValue.ToString() == "Freelance" && sleRole.EditValue != null && DataBuffer.RoleList.FirstOrDefault(x => x.ID == (int)sleRole.EditValue).orderIndex != 1)
                {
                    employeeErrorProvider.SetError(imcoStatus, "Only Sales Agents Can Be Freelancer", ErrorType.Critical);
                    error = 1;
                }
                if (HireDateDateEdit.EditValue == null || HireDateDateEdit.EditValue.ToString() == "")
                {
                    employeeErrorProvider.SetError(HireDateDateEdit, "Please Select Hire Date", ErrorType.Critical);
                    error = 1;
                }
                if (DateOfBirthDateAndTime.EditValue == null || DateOfBirthDateAndTime.EditValue.ToString() == "")
                {
                    employeeErrorProvider.SetError(DateOfBirthDateAndTime, "Please Select Date Of Birth", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtCity.Text))
                {
                    employeeErrorProvider.SetError(txtCity, "Please Enter City", ErrorType.Critical);
                    error = 1;
                }
                return error == 0;
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
        #endregion
    }
}
