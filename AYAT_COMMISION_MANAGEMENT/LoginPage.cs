using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class LoginPage : DevExpress.XtraEditors.XtraForm
    {
        public bool Authenticated { get; set; }
        private DXErrorProvider login_Error_Provider { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            PopulateControls();
        }

        private void PopulateControls()
        {
            login_Error_Provider = new DXErrorProvider();
            DataBuffer.UserNameList = AyatProcessManager.AyatProcessManager.UserNameSelectAll();
            cmbUserName.Properties.DataSource = DataBuffer.UserNameList.Where(p => p.isActive).ToList();
        }

        private void SBLogin_Click(object sender, EventArgs e)
        {
            ValidateAndLogin();
        }

        private void ValidateAndLogin()
        {
            try
            {
                login_Error_Provider.ClearErrors();
                if (cmbUserName.EditValue != null)
                {
                    if (txtPassword.EditValue != null && !string.IsNullOrEmpty(txtPassword.EditValue.ToString()))
                    {
                        UserName selectedUser = DataBuffer.UserNameList.FirstOrDefault(p => p.ID == Convert.ToInt32(cmbUserName.EditValue));
                        //if (selectedUser == null)
                        //    selectedUser = ProcessManager.GetUserNameByid(Convert.ToInt32(cmbUserName.EditValue));
                        if (selectedUser != null)
                        {
                           // string inc = SecurityManager.Encrypt(txtPassword.EditValue.ToString());
                            if (selectedUser.password == SecurityManager.Encrypt(txtPassword.EditValue.ToString()))
                            {
                                Authenticated = true;
                                DataBuffer.thisUser = selectedUser;
                                DataBuffer.thisUserAccessPrivilages = DataBuffer.AccessesList.Where(x=> x.userID == selectedUser.ID).ToList();
                                CreateActivityLog("Authentication", "Logged In", DataBuffer.thisUser.ID, DataBuffer.thisUser.ID);
                                this.Close();
                            }
                            else
                            {
                                CreateActivityLog("Authentication", "Wrong Password", Convert.ToInt32(cmbUserName.EditValue), Convert.ToInt32(cmbUserName.EditValue));
                                XtraMessageBox.Show("Wrong Password! Please Try Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Unable To Find User!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        login_Error_Provider.SetError(txtPassword, "Please enter password first.", ErrorType.Critical);
                        XtraMessageBox.Show("Please Enter Password First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    login_Error_Provider.SetError(cmbUserName, "Please select user first.", ErrorType.Critical);
                    XtraMessageBox.Show("Please Select User First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateActivityLog(string type, string description, int reference, int userID)
        {
            ActivityLog activityLog = new ActivityLog()
            {
                type = type,
                reference = reference,
                description = description,
                dateCreated = DateTime.Now,
                userID = userID,
                deviceID = DataBuffer.thisDevice.ID,
                note = null,
                remark = null
            };
            AyatProcessManager.AyatProcessManager.ActivityLogInsert(activityLog);
        }

        private void SBCancel_Click(object sender, EventArgs e)
        {
            Authenticated = false;
            this.Close();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValidateAndLogin();
            }
        }
    }
}