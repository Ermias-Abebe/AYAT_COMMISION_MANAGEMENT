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
using static AyatProcessManager.Custom_Classes;
using DevExpress.XtraGrid.Views.Grid;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCUserList : DevExpress.XtraEditors.XtraUserControl
    {
        private int error { get; set; }
        public event EventHandler show_Dashboard;
        public UserName SelectedUser { get; set; }
        private DXErrorProvider userErrorProvider { get; set; }
        public UCUserList()
        {
            InitializeComponent();
        }

       
        public void PopulateUserAccessPrivilage(int userid)
        {
            List<Access> SavedAccess = DataBuffer.AccessesList.Where(x=> x.userID == userid).ToList();
            List<int> accesspriv = new List<int>();
            if (SavedAccess != null && SavedAccess.Count>0)
            {
                accesspriv = SavedAccess.Select(x=> x.accessPrivilage).ToList();
            }


            List<AccessPrivilageDTO> userAccessPrivilageDTO = DataBuffer.AccessPrivilageDTOList.Select(x=> new AccessPrivilageDTO
            {
                select = accesspriv.Contains(x.id),
                id = x.id,
                description = x.description,
                group = x.group
            }).ToList();

            gcAccessData.DataSource = userAccessPrivilageDTO;
            gvAccessData.ExpandAllGroups();
        }
        public void Initalize_User()
        {
            if (userErrorProvider == null)
                userErrorProvider = new DXErrorProvider();

            userErrorProvider.ClearErrors();

             
            SelectedUser = null;
            DataBuffer.UserNameList = AyatProcessManager.AyatProcessManager.UserNameSelectAll();
            gcUserList.DataSource = DataBuffer.UserNameList;
            sleEmployeelist.Properties.DataSource = DataBuffer.EmployeeList;
            sleEmployeelist.Properties.DisplayMember = "fullName";
            sleEmployeelist.Properties.ValueMember = "ID";

        }
         
        private bool Validate_User()
        {
            try
            {
                userErrorProvider.ClearErrors();
                error = 0;
                if (sleEmployeelist.EditValue == null || sleEmployeelist.EditValue.ToString() == "")
                {
                    userErrorProvider.SetError(sleEmployeelist, "Please Select Employee", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    userErrorProvider.SetError(txtUserName, "Please Enter User Name", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtUserPassword.Text))
                {
                    userErrorProvider.SetError(txtUserPassword, "Please Enter User Password", ErrorType.Critical);
                    error = 1;
                }
                if (string.IsNullOrEmpty(txtPasswordHint.Text))
                {
                    userErrorProvider.SetError(txtPasswordHint, "Please Enter Password Hint", ErrorType.Critical);
                    error = 1;
                }
                return error == 0;
            }
            catch { return false; }
        } 

        private void employeeListWindowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
            {
                case "save":
                    if (xtcUsertabcontrol.SelectedTabPage == xtpUserMaintain)
                    {
                        if (Validate_User())
                        {
                            UserName user = new UserName()
                            {
                                empID = Convert.ToInt32(sleEmployeelist.EditValue.ToString()),
                                name = txtUserName.Text,
                                password = AyatProcessManager.SecurityManager.Encrypt(txtUserPassword.Text),
                                hint = txtPasswordHint.Text,
                                isActive = chkisactive.Checked,
                                remark = txtRemark.Text
                            };
                            if (SelectedUser == null)
                            {
                                if (AyatProcessManager.AyatProcessManager.UserNameInsert(user))
                                {
                                    XtraMessageBox.Show("User Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    DataBuffer.UserNameList = AyatProcessManager.AyatProcessManager.UserNameSelectAll();
                                    gcUserList.DataSource = DataBuffer.UserNameList;
                                    ClearControl();
                                }
                                else
                                { XtraMessageBox.Show("User Saved Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                            }
                            else
                            {
                                user.ID = SelectedUser.ID;
                                if (AyatProcessManager.AyatProcessManager.UserNameUpdate(user))
                                {
                                    XtraMessageBox.Show("User Updated Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    DataBuffer.UserNameList = AyatProcessManager.AyatProcessManager.UserNameSelectAll();
                                    gcUserList.DataSource = DataBuffer.UserNameList;
                                    ClearControl();
                                }
                                else
                                    XtraMessageBox.Show("User Update Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                    }
                    else if (xtcUsertabcontrol.SelectedTabPage == xtpUserAccess)
                    {
                        gvAccessData.PostEditor();
                        UserName data = (UserName)tvUserList.GetFocusedRow();
                        if (data != null)
                        {
                            bool deleted = AyatProcessManager.AyatProcessManager.DeleteAccessPrivilageByUserid(data.ID);
                            if (deleted)
                            {

                                List<AccessPrivilageDTO> SelectedAccess = (List<AccessPrivilageDTO>) gcAccessData.DataSource;
                                SelectedAccess = SelectedAccess.Where(x=> x.select).ToList();
                                if (SelectedAccess != null && SelectedAccess.Count > 0)
                                {
                                    List<Access> newaccess = SelectedAccess.Select(x => new Access()
                                    {
                                        userID = data.ID,
                                        accessPrivilage = x.id

                                    }).ToList();
                                    bool save = AyatProcessManager.AyatProcessManager.accessInsertList(newaccess);
                                    DataBuffer.AccessesList = AyatProcessManager.AyatProcessManager.AccessPrivilageSelectAll();
                                    PopulateUserAccessPrivilage(data.ID);
                                    if (save)
                                        XtraMessageBox.Show("User Access Privilage Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    else
                                        XtraMessageBox.Show("User Access Privilage Saving Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                        }
                    }
                    break;
                case "cancel":
                    show_Dashboard?.Invoke(sender, e);
                    break;
                case "new":
                    ClearControl();
                    break;
                case "reports":
                    break;
            }
        }

        private void ClearControl()
        {
            txtUserName.Text = null;
            sleEmployeelist.EditValue = null;
            txtUserPassword.Text = null;
            txtPasswordHint.Text = null;
            txtRemark.Text = null;
            chkisactive.Checked = true;
            SelectedUser = null;
        }

        private void tvUserList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (xtcUsertabcontrol.SelectedTabPage == xtpUserAccess)
            //{
            //    UserName data = (UserName)tvUserList.GetFocusedRow();
            //    if (data != null)
            //    {
            //        PopulateUserAccessPrivilage(data.ID);
            //    }
            //}
        }

        private void tvUserList_DoubleClick(object sender, EventArgs e)
        {
            UserName data = (UserName)tvUserList.GetFocusedRow();
            if(data != null)
            {
                SelectedUser = data;
                txtUserName.Text = data.name;
                sleEmployeelist.EditValue = data.empID;
                txtUserPassword.Text = AyatProcessManager.SecurityManager.Decrypt(data.password);
                txtPasswordHint.Text = data.hint;
                txtRemark.Text = data.remark;
                chkisactive.Checked = data.isActive;
            }
        }

        private void sleEmployeelist_EditValueChanged(object sender, EventArgs e)
        {
            if (SelectedUser == null && sleEmployeelist.EditValue != null && !string.IsNullOrEmpty(sleEmployeelist.EditValue.ToString()))
            {
                Employee selectedemp = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID.ToString() == sleEmployeelist.EditValue.ToString());

                txtUserName.Text = selectedemp.firstName + " " + (string.IsNullOrEmpty(selectedemp.middleName) ? (string.IsNullOrEmpty(selectedemp.lastName) ? "" : selectedemp.lastName.Substring(0, 1)) : selectedemp.middleName.Substring(0, 1));
                txtPasswordHint.EditValue = null;
                txtUserPassword.EditValue = null;
                txtRemark.EditValue = null;
                chkisactive.Checked = true;
                SelectedUser = null;
            }

        }

        private void tvUserList_Click(object sender, EventArgs e)
        {
            if (xtcUsertabcontrol.SelectedTabPage == xtpUserAccess)
            {
                UserName data = (UserName)tvUserList.GetFocusedRow();
                if (data != null)
                {
                    PopulateUserAccessPrivilage(data.ID);
                }
            }
        }

        private void gvAccessData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["select"]);
                if (category == "Checked")
                {
                    e.Appearance.BackColor = Color.Green;
                    e.Appearance.BackColor2 = Color.Green;
                }
            }
        }

        private void lcUserMainitain_Click(object sender, EventArgs e)
        {
            xtcUsertabcontrol.SelectedTabPage = xtpUserMaintain; 
        }

        private void lcUserAccessPrivilage_Click(object sender, EventArgs e)
        {
            xtcUsertabcontrol.SelectedTabPage = xtpUserAccess;
        }
    }
    public class UserDetail
    {
        public int code { get; set; }
        public string EmployeeFullName { get; set; }
        public string UserFullName { get; set; }
        public string Password { get; set; }
        public string hint { get; set; }
        public Image Photo { get; set; }
    }
}
