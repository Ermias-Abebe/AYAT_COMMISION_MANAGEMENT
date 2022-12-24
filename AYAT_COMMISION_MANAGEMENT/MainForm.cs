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
using AyatProcessManager;
using System.IO;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {

        public MainForm()
        {
            InitializeComponent();
            CheckUserAccessPrivilage();
        }

        private void tileBarItemDashboard_ItemClick(object sender, TileItemEventArgs e)
        {
            ucDashboard1.Initalize_Dashboard();
            navigationFrame.SelectedPageIndex = 0;
        }

        private void tilBarItemEmployees_ItemClick(object sender, TileItemEventArgs e)
        {
            ucEmployeeList1.Initalize_Employee_List();
            navigationFrame.SelectedPageIndex = 1;
        }

        private void tileBarItemContractAgreement_ItemClick(object sender, TileItemEventArgs e)
        {
            ucSalesCenter1.Initalize_Sales();
            navigationFrame.SelectedPageIndex = 2;
        }

        private void tileBarItemContractAgreementDoc_ItemClick(object sender, TileItemEventArgs e)
        {
            ucContractAgreementBrowser1.Initalize_Contract_Agreement_Document();
            navigationFrame.SelectedPageIndex = 9;
        }
        private void tileBarItemCollection_ItemClick(object sender, TileItemEventArgs e)
        {
            ucPayment1.Initalize_Payment();
            navigationFrame.SelectedPageIndex = 3;
        }
        private void tileBarItemRoles_ItemClick(object sender, TileItemEventArgs e)
        {
            ucRole1.Initalize_Role();
            navigationFrame.SelectedPageIndex = 4;
        }

        private void tileBarItemReports_ItemClick(object sender, TileItemEventArgs e)
        {
            ucReport1.Initalize_Report();
            navigationFrame.SelectedPageIndex = 5;
        }

        private void tileBarItemUsers_ItemClick(object sender, TileItemEventArgs e)
        {
            ucUserList1.Initalize_User();
            navigationFrame.SelectedPageIndex = 6;
        }
        private void tileBarItemCommission_ItemClick(object sender, TileItemEventArgs e)
        {
            ucCommission1.Initialize_Commission_Payment();
            navigationFrame.SelectedPageIndex = 8;
        }
        private void ucEmployeeCenter1_close_Button_Clicked(object sender, EventArgs e)
        {
            ucEmployeeList1.Initalize_Employee_List();
            navigationFrame.SelectedPageIndex = 1;
        }
         
        private void ucEmployeeList1_show_Emplyee_Center(object sender, EventArgs e)
        {
            ucEmployeeCenter1.Initalize_Employee_Maintain();
            navigationFrame.SelectedPageIndex = 7;
        }

        private void ucEmployeeList1_show_Dashboard(object sender, EventArgs e)
        {
            navigationFrame.SelectedPageIndex = 0;
        }

        private void ucSalesCenter1_close_Button_Clicked(object sender, EventArgs e)
        {
            navigationFrame.SelectedPageIndex = 0;
        }

        private void ucPayment1_close_Button_Clicked(object sender, EventArgs e)
        {
            navigationFrame.SelectedPageIndex = 0;
        }

        private void UcCommission1_close_Button_Clicked(object sender, System.EventArgs e)
        {
            navigationFrame.SelectedPageIndex = 0;
        }
        private void UcRole1_close_Button_Clicked(object sender, System.EventArgs e)
        {
            navigationFrame.SelectedPageIndex = 0;
        }
        private void ucContractAgreementBrowser1_close_Button_Clicked(object sender, EventArgs e)
        {
            navigationFrame.SelectedPageIndex = 0;
        }
        private void CheckUserAccessPrivilage()
        {
            tileBarItemUsers.Visible = false;
            tileBarItemReports.Visible = false;
            tileBarItemCollection.Visible = false;
            tileBarItemContractAgreement.Visible = false;
            tileBarItemContractAgreementDoc.Visible = false;
            tilBarItemEmployees.Visible = false;
            tileBarItemRoles.Visible = false;
            tileBarItemCommission.Visible = false;



            List<int> AccessIDList = DataBuffer.thisUserAccessPrivilages.Select(x => x.accessPrivilage).ToList();

            if (AccessIDList.Contains(1))
            {
                tileBarItemUsers.Visible = true;
                tileBarItemReports.Visible = true;
                tileBarItemCollection.Visible = true;
                tileBarItemContractAgreement.Visible = true;
                tileBarItemContractAgreementDoc.Visible = true;
                tilBarItemEmployees.Visible = true;
                tileBarItemRoles.Visible = true;
                tileBarItemCommission.Visible = true;
            }
            else
            {
                if (AccessIDList.Contains(2))
                {
                    tileBarItemContractAgreement.Visible = true;
                }
                if (AccessIDList.Contains(3))
                {
                    tileBarItemCommission.Visible = true;
                }
                if (AccessIDList.Contains(4))
                {
                    tileBarItemCollection.Visible = true;
                }
                if (AccessIDList.Contains(5))
                {
                    tileBarItemReports.Visible = true;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ucDashboard1.Initalize_Dashboard();
        }
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length))
                {
                    ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                    return Image.FromStream(ms, true);//Exception occurs here
                }
            }
            catch
            {
                return null;
            }
        }
    }
}