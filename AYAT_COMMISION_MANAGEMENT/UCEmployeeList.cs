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
using AyatDataAccess;
using AyatProcessManager;
using static AyatProcessManager.Custom_Classes;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCEmployeeList : DevExpress.XtraEditors.XtraUserControl
    {
        private List<Employee_Detail> employees { get; set; }
        private Employee_Detail selected_Employee { get; set; }

        public event EventHandler show_Employee_Center;

        public event EventHandler show_Dashboard;
        public UCEmployeeList()
        {
            InitializeComponent();

        }

        #region Event Handler
        private void employeeListWindowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
            {
                case "new":
                    UCEmployeeCenter.employee_ID = null;
                    show_Employee_Center?.Invoke(sender, e);
                    break;
                case "cancel":
                    show_Dashboard?.Invoke(sender, e);
                    break;
                case "edit":
                    tvEmployees_DoubleClick(null, null);
                    break; 
            }
        }
        private void tileItemFilter_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                Populate_Employees((sender as TileItem).Tag == null ? null : (sender as TileItem).Tag.ToString());
            }
            catch { }
        }
        private void tvEmployees_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                selected_Employee = (Employee_Detail)tvEmployees.GetFocusedRow();
                if (selected_Employee != null)
                {
                    UCEmployeeCenter.employee_ID = selected_Employee.code;
                    show_Employee_Center?.Invoke(sender, e);
                }
            }
            catch { }
        }
        #endregion

        #region Methods
        public void Initalize_Employee_List()
        {
            Populate_Employees();
            Populate_Employee_Types();
            filterTileControl.SelectedItem = tlStatusAll;
        }
        private void Populate_Employees(string employee_Status = null)
        {
            try
            {
                employees = new List<Employee_Detail>();
                foreach (Employee emp in string.IsNullOrEmpty(employee_Status) ? DataBuffer.EmployeeList : DataBuffer.EmployeeList.Where(x => x.status == employee_Status))
                {
                    employees.Add(new Employee_Detail
                    {
                        code = emp.ID,
                        FullName = emp.fullName,
                        Address = emp.city,
                        PhoneNo = emp.mobilephone,
                        Photo = emp.picture == null ? Properties.Resources.EMP : MainForm.byteArrayToImage(emp.picture),
                        Email = emp.email
                    });
                }
                gcEmployees.DataSource = employees;
                gcEmployees.RefreshDataSource();
            }
            catch { }
        }
        private void Populate_Employee_Types()
        {
            try
            {
                tlStatusAll.Text = DataBuffer.EmployeeList.Count.ToString();
                tlStatusSalaried.Text = DataBuffer.EmployeeList.Where(s => s.status == "Salaried").Count().ToString();
                tlStatusFreelance.Text = DataBuffer.EmployeeList.Where(s => s.status == "Freelance").Count().ToString();
            }
            catch { }
        }
        #endregion
    }
}
