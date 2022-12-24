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
using DevExpress.XtraEditors.DXErrorProvider;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCRole : DevExpress.XtraEditors.XtraUserControl
    {
        private DXErrorProvider roleErrorProvider { get; set; }
        private int error { get; set; }
        private Role role { get; set; }
        private RoleIncrement selected_Role_Increment { get; set; }
        private List<RoleIncrement> role_Increments { get; set; }

        public event EventHandler close_Button_Clicked;
        public UCRole()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void UCRole_Load(object sender, EventArgs e)
        {
            roleErrorProvider = new DXErrorProvider();
        }
        private void roleWindowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
                {
                    case "save":
                        if (Validate_Role())
                        {
                            if (role == null)
                            {
                                role = new Role()
                                {
                                    description = txtRoleDescription.Text,
                                    parent = sleParent.EditValue != null ? Convert.ToInt16(sleParent.EditValue) : (int?)null,
                                    cashRF = Convert.ToDecimal(txtCashRF.EditValue),
                                    cashSF = Convert.ToDecimal(txtCashSF.EditValue),
                                    loanRF = Convert.ToDecimal(txtCreditRF.EditValue),
                                    type = imcoRoleType.EditValue.ToString() == "Sales Role" ? (int)Custom_Classes.Role_Type.Sales : (imcoRoleType.EditValue.ToString() == "Normal Role" ? (int)Custom_Classes.Role_Type.Normal : (int)Custom_Classes.Role_Type.OtherEmployee),
                                    orderIndex = string.IsNullOrEmpty(txtRoleRank.Text) ? (int?)null : Convert.ToInt16(txtRoleRank.Text)
                                };
                                role.ID = AyatProcessManager.AyatProcessManager.RoleInsert(role);
                            }
                            else
                            {
                                role.description = txtRoleDescription.Text;
                                role.parent = sleParent.EditValue != null ? Convert.ToInt16(sleParent.EditValue) : (int?)null;
                                role.cashRF = Convert.ToDecimal(txtCashRF.EditValue);
                                role.cashSF = Convert.ToDecimal(txtCashSF.EditValue);
                                role.loanRF = Convert.ToDecimal(txtCreditRF.EditValue);
                                role.type = imcoRoleType.EditValue.ToString() == "Sales Role" ? (int)Custom_Classes.Role_Type.Sales : (imcoRoleType.EditValue.ToString() == "Normal Role" ? (int)Custom_Classes.Role_Type.Normal : (int)Custom_Classes.Role_Type.OtherEmployee);
                                role.orderIndex = string.IsNullOrEmpty(txtRoleRank.Text) ? (int?)null : Convert.ToInt16(txtRoleRank.Text);

                                AyatProcessManager.AyatProcessManager.RoleUpdate(role);
                            }

                            AyatProcessManager.AyatProcessManager.DeleteRoleIncrementsByRole(role.ID);
                            if (role_Increments != null && role_Increments.Count > 0)
                            {
                                role_Increments.ForEach(p => p.roleID = role.ID);
                                AyatProcessManager.AyatProcessManager.RoleIncrementsInsertList(role_Increments);
                            }
                            XtraMessageBox.Show("Role Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Initalize_Role();
                        }
                        break;
                    case "cancel":
                        close_Button_Clicked?.Invoke(sender, e);
                        break;
                    case "new":
                        Clear_Controls();
                        roleErrorProvider.ClearErrors();
                        break;
                    case "reports":
                        break;
                }
            }
            catch { }
        }

        private void tvRoles_DoubleClick(object sender, EventArgs e)
        {
            role = (Role)tvRoles.GetFocusedRow();
            if (role != null)
            {
                txtRoleDescription.Text = role.description;
                txtCashRF.EditValue = role.cashRF;
                txtCashSF.EditValue = role.cashSF;
                txtCreditRF.EditValue = role.loanRF;
                sleParent.EditValue = role.parent;
                imcoRoleType.EditValue = role.type == (int)Custom_Classes.Role_Type.Sales ? "Sales Role" : (role.type == (int)Custom_Classes.Role_Type.Normal ? "Normal Role" : "Other Employees");
                txtRoleRank.Text = role.orderIndex != null ? role.orderIndex.ToString() : null;
                gcRoleIncriments.DataSource = role_Increments = AyatProcessManager.AyatProcessManager.RoleIncrementsSelectByRole(role.ID).ToList();
                gcRoleIncriments.RefreshDataSource();
                gvRoleIncriments.BestFitColumns();
            }
        }

        private void gvRoleIncriments_DoubleClick(object sender, EventArgs e)
        {
            selected_Role_Increment = (RoleIncrement)gvRoleIncriments.GetFocusedRow();
            if (selected_Role_Increment != null)
            {
                spnIncrement.EditValue = selected_Role_Increment.increment;
                spnPercentage.EditValue = selected_Role_Increment.percentage;
                role_Increments.Remove(selected_Role_Increment);
                gcRoleIncriments.DataSource = role_Increments;
                gcRoleIncriments.RefreshDataSource();
                gvRoleIncriments.BestFitColumns();
            }
        }

        private void btnAddRoleIncrement_Click(object sender, EventArgs e)
        {
            try
            {
                if(Validate_Role())
                {
                    if(Validate_Role_Incriment())
                    {
                        if (role_Increments == null)
                            role_Increments = new List<RoleIncrement>();

                        role_Increments.Add(new RoleIncrement
                        {
                            percentage = Convert.ToDecimal(spnPercentage.EditValue),
                            increment = Convert.ToDecimal(spnIncrement.EditValue)
                        });

                        gcRoleIncriments.DataSource = role_Increments;
                        gcRoleIncriments.RefreshDataSource();
                        gvRoleIncriments.BestFitColumns();
                        spnPercentage.EditValue = null;
                        spnIncrement.EditValue = null;
                    }
                }
            }
            catch { }
        }

        private void txtRoleRank_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.D1 || e.KeyData == Keys.D2 || e.KeyData == Keys.D3 || e.KeyData == Keys.D4 || e.KeyData == Keys.D5 ||
                    e.KeyData == Keys.D6 || e.KeyData == Keys.D7 || e.KeyData == Keys.D8 || e.KeyData == Keys.D9 || e.KeyData == Keys.D0 ||
                    e.KeyData == Keys.NumPad1 || e.KeyData == Keys.NumPad2 || e.KeyData == Keys.NumPad3 || e.KeyData == Keys.NumPad4 || e.KeyData == Keys.NumPad5 ||
                   e.KeyData == Keys.NumPad6 || e.KeyData == Keys.NumPad7 || e.KeyData == Keys.NumPad8 || e.KeyData == Keys.NumPad9 || e.KeyData == Keys.NumPad0 || e.KeyData == Keys.Back)
                    e.SuppressKeyPress = false;
                else e.SuppressKeyPress = true;
            }
            catch { }
        }
        #endregion

        #region Methods
        private void Clear_Controls()
        {
            role = null;
            role_Increments = null;
            txtRoleDescription.EditValue = null;
            sleParent.EditValue = null;
            txtCashRF.EditValue = null;
            txtCashSF.EditValue = null;
            txtCreditRF.EditValue = null;
            spnPercentage.EditValue = null;
            spnIncrement.EditValue = null;
            imcoRoleType.EditValue = null;
            txtRoleRank.Text = null;
            gcRoleIncriments.DataSource = null;
            gcRoleIncriments.RefreshDataSource();
            gvRoleIncriments.BestFitColumns();
        }
        public void Initalize_Role()
        {
            Clear_Controls();
            DataBuffer.RoleList = AyatProcessManager.AyatProcessManager.RoleSelectAll();
            gcRoles.DataSource = DataBuffer.RoleList;
            gcRoles.RefreshDataSource();
            sleParent.Properties.DataSource = DataBuffer.RoleList != null ? DataBuffer.RoleList.Where(x => x.type != (int)Custom_Classes.Role_Type.OtherEmployee).ToList() : null;
        }
        private bool Validate_Role()
        {
            try
            {
                roleErrorProvider.ClearErrors();
                error = 0;
                if (string.IsNullOrEmpty(txtRoleDescription.Text))
                {
                    roleErrorProvider.SetError(txtRoleDescription, "Please Enter Role Name", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtCashRF.Text))
                {
                    roleErrorProvider.SetError(txtCashRF, "Please Enter Cash RF", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtCashSF.Text))
                {
                    roleErrorProvider.SetError(txtCashSF, "Please Enter Cash SF", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtCreditRF.Text))
                {
                    roleErrorProvider.SetError(txtCreditRF, "Please Enter Credit RF", ErrorType.Critical);
                    error = 1;
                }
                if (imcoRoleType.EditValue == null)
                {
                    roleErrorProvider.SetError(imcoRoleType, "Please Select Role Type", ErrorType.Critical);
                    error = 1;
                }
                else if (imcoRoleType.EditValue.ToString() == "Sales Role")
                {
                    if (string.IsNullOrEmpty(txtRoleRank.Text))
                    {
                        roleErrorProvider.SetError(txtRoleRank, "Please Set Role Rank", ErrorType.Critical);
                        error = 1;
                    }
                }

                return error == 0;
            }
            catch { return false; }
        }
        private bool Validate_Role_Incriment()
        {
            try
            {
                roleErrorProvider.ClearErrors();
                error = 0;
                if (spnPercentage.EditValue == null || string.IsNullOrEmpty(spnPercentage.EditValue.ToString()))
                {
                    roleErrorProvider.SetError(spnPercentage, "Please Enter Percentage", ErrorType.Critical);
                    error = 1;
                }
                if (spnIncrement.EditValue == null || string.IsNullOrEmpty(spnIncrement.EditValue.ToString()))
                {
                    roleErrorProvider.SetError(spnIncrement, "Please Enter Incriment", ErrorType.Critical);
                    error = 1;
                }
                return error == 0;
            }
            catch { return false; }
        }
        #endregion
    }
}
