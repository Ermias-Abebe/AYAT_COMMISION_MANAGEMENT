using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AyatDataAccess;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraTab;
using AyatProcessManager;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using AYAT_COMMISION_MANAGEMENT.Report;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCReport : XtraUserControl
    {
        #region Search Criteria
        public string ID { get; set; }
        public string user { get; set; }
        public string Employee { get; set; }
        public string Name { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public string Site { get; set; }
        public string Building { get; set; }
        public string FloorNo { get; set; }
        public string HouseNo { get; set; }
        public string HouseType { get; set; }
        public string Area { get; set; }
        public string bedroom { get; set; }
        public string device { get; set; }
        public string isactive { get; set; }
        public string SalesSummaryBy { get; set; }
        #endregion

        public UCReport()
        {
            InitializeComponent();
        }
        public void Initalize_Report()
        {
            GeReportListData();
            GetSearchCriteriaData();
            cmbDateCriteria.EditValue = "Show All";
        }
        public void GeReportListData()
        {
            List<ReportDTO> Reportlist = new List<ReportDTO>()
            {
               
                new ReportDTO()
                {
                    code = 11,
                    reportname= "Commission Summary Report",
                    parent = 1

                },
                new ReportDTO()
                {
                    code = 12,
                    reportname= "Commission Payment Plan Report",
                    parent = 1

                }, 
                new ReportDTO()
                {
                    code = 21,
                    reportname= "Sales Summarized By",
                    parent = 2

                },
                new ReportDTO()
                {
                    code = 22,
                    reportname= "Sales Detail Report",
                    parent = 2

                },
                new ReportDTO()
                {
                    code = 23,
                    reportname= "InActive Sale Reports",
                    parent = 2

                },
                new ReportDTO()
                {
                    code = 24,
                    reportname= "Payment Report",
                    parent = 2

                },
                new ReportDTO()
                {
                    code = 25,
                    reportname= "Employee Commission Report",
                    parent = 2

                },
                new ReportDTO()
                {
                    code = 26,
                    reportname= "Employee Report",
                    parent = 2

                }

             //"Sales Summarized By Users",
             //"Sales Summarized By Site",
             //"Sales Summarized By Date",
             //"Sales Summarized By House Type",
             //"Sales Summarized By Area",
             //"Sales Summarized By BedRoom",
             //"Sales Summarized By Device",
             //"InActive Sales"
            };







            var reportdata = Reportlist.ToList();
            tlReportList.DataSource = reportdata;
            //tlReportList.Focused();

            tlReportList.SetFocusedValue(reportdata.FirstOrDefault().reportname);
            //tlReportList.KeyFieldName = "code";
            //tlReportList.ParentFieldName = "parent";
            //tlReportList.CheckBoxFieldName = "select";
            //tlReportList.ExpandAll();

        }
        public void GetSearchCriteriaData()
        {

            List<Sale> salesList = AyatProcessManager.AyatProcessManager.SalesSelectAll();
            sleSalesList.Properties.DataSource = salesList.Select(s => new { id = s.ID, code = s.code, Name = s.FAName, date = s.Date.ToShortDateString() });
            sleSalesList.Properties.DisplayMember = "code";
            sleSalesList.Properties.ValueMember = "id";


            List<string> CustomerNameList = salesList.Select(x => x.FAName).Distinct().OrderBy(x => x).ToList(); //AyatProcessManager.AyatProcessManager.GetAllSalesCustomerName().OrderBy(y => y).ToList(); ;
            sleCustomerName.Properties.DataSource = CustomerNameList.Select(x => new { CustomerName = x });
            sleCustomerName.Properties.DisplayMember = "CustomerName";
            sleCustomerName.Properties.ValueMember = "CustomerName";


            List<string> SiteLocationList = salesList.Select(x => x.Site).Distinct().OrderBy(x => x).ToList(); //AyatProcessManager.AyatProcessManager.GetAllSalesSites().OrderBy(y => y).ToList(); ;
            sleSalesLocation.Properties.DataSource = SiteLocationList.Select(x => new { SiteName = x });
            sleSalesLocation.Properties.DisplayMember = "SiteName";
            sleSalesLocation.Properties.ValueMember = "SiteName";


            List<string> BuildingNoList = salesList.Select(x => x.BuildingNo).Distinct().OrderBy(x => x).ToList(); //AyatProcessManager.AyatProcessManager.GetAllSalesBuildingNo().OrderBy(y => y).ToList(); ;
            sleSalesBuilding.Properties.DataSource = BuildingNoList.Select(x => new { Building = x });
            sleSalesBuilding.Properties.DisplayMember = "Building";
            sleSalesBuilding.Properties.ValueMember = "Building";



            List<string> HouseNoList = salesList.Select(x => x.HouseNo).Distinct().OrderBy(x => x).ToList(); //AyatProcessManager.AyatProcessManager.GetAllSalesHouseNo().OrderBy(y => y).ToList(); ;
            sleSalesHouseNo.Properties.DataSource = HouseNoList.Select(x => new { HouseNo = x });
            sleSalesHouseNo.Properties.DisplayMember = "HouseNo";
            sleSalesHouseNo.Properties.ValueMember = "HouseNo";

            List<HouseTypeDTO> HouseTypeList = new List<HouseTypeDTO>()
            {
                new HouseTypeDTO()
                {
                    id =0,
                    HouseType = "Semi Furnished"

                }, new HouseTypeDTO()
                {
                    id =1,
                    HouseType = "Fully Furnished"

                }
            };
            leSalesHouseType.Properties.DataSource = HouseTypeList;
            leSalesHouseType.Properties.DisplayMember = "HouseType";
            leSalesHouseType.Properties.ValueMember = "id";

            List<decimal> SalesAreaList = salesList.Select(x => x.Area).Distinct().OrderBy(x => x).ToList(); //AyatProcessManager.AyatProcessManager.GetAllSalesArea().OrderBy(y=> y).ToList();
            sleSalesArea.Properties.DataSource = SalesAreaList.Select(x => new { HouseArea = x });
            sleSalesArea.Properties.DisplayMember = "HouseArea";
            sleSalesArea.Properties.ValueMember = "HouseArea";

            sleDevice.Properties.DataSource = DataBuffer.DeviceList.Select(x => new { id = x.ID, Device = x.deviceName });
            sleDevice.Properties.DisplayMember = "Device";
            sleDevice.Properties.ValueMember = "id";

            sleUser.Properties.DataSource = DataBuffer.UserNameList.Select(x => new { id = x.ID, UserName = x.name });
            sleUser.Properties.DisplayMember = "UserName";
            sleUser.Properties.ValueMember = "id";

            sleEmployee.Properties.DataSource = DataBuffer.EmployeeList;
            sleEmployee.Properties.DisplayMember = "fullName";
            sleEmployee.Properties.ValueMember = "ID";


            txtFloorNo.EditValue = null;
            txtSalesBedRoom.EditValue = null;





        }
        public void ClearSearchCriteriaData()
        {
            sleSalesList.EditValue = null;
            sleCustomerName.EditValue = null;
            sleSalesLocation.EditValue = null;
            sleSalesBuilding.EditValue = null;
            sleSalesHouseNo.EditValue = null;
            leSalesHouseType.EditValue = null;
            sleSalesArea.EditValue = null;
            sleDevice.EditValue = null;
            sleUser.EditValue = null;
            sleEmployee.EditValue = null;
            txtFloorNo.EditValue = null;
            txtSalesBedRoom.EditValue = null;
        }
        private void windowsUIButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            try
            {
                tlReportList.PostEditor();
                switch ((e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton).Caption.ToLower())
                {
                    case "show":

                        populateFilterObjects();

                        // List<ReportDTO> SelectedReportList = ((List<ReportDTO>)tlReportList.DataSource).Where(x=> x.select).ToList();

                        ReportDTO SelectedReport = (ReportDTO)tlReportList.GetFocusedRow();

                        //if (SelectedReportList != null && SelectedReportList.Count>0) 
                        if (SelectedReport != null)
                        {
                            //foreach (ReportDTO report in SelectedReportList)
                            //{

                            switch (SelectedReport.reportname)
                            {
                                case "Commission Summary Report":
                                    List<Sale> SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(ID, user, Name, startDateTime, endDateTime, Site, Building, FloorNo, HouseNo, HouseType, Area, bedroom, device, isactive, null, null);
                                    if (SalesList != null && SalesList.Count > 0)
                                    {
                                        CommissionGeneralReport rep = new CommissionGeneralReport(SalesList, 1);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    };
                                    break;
                                case "Commission Payment Plan Report":

                                    SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(ID, user, Name, startDateTime, endDateTime, Site, Building, FloorNo, HouseNo, HouseType, Area, bedroom, device, isactive, null, null);
                                    if (SalesList != null && SalesList.Count > 0)
                                    {
                                        CommissionGeneralReport rep = new CommissionGeneralReport(SalesList, 2);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    break;

                                case "Sales Summarized By":

                                    string columnName = "";
                                    string GroupType = "";
                                    if (SalesSummaryBy == "Site")
                                    {
                                        columnName = "Site";
                                        GroupType = "House Site: ";
                                    }
                                    else if (SalesSummaryBy == "Customer")
                                    {
                                        columnName = "FAName";
                                        GroupType = "Customer Name: ";
                                    }
                                    else if (SalesSummaryBy == "Users")
                                    {
                                        columnName = "userEmployeeName";
                                        GroupType = "Prepared By: ";
                                    }
                                    else if (SalesSummaryBy == "Date")
                                    {
                                        columnName = "Date";
                                        GroupType = "Issued Date: ";
                                    }
                                    else if (SalesSummaryBy == "House Type")
                                    {
                                        columnName = "HouseType";
                                        GroupType = "House Type: ";
                                    }
                                    else if (SalesSummaryBy == "Area")
                                    {
                                        columnName = "Area";
                                        GroupType = "House Area: ";
                                    }
                                    else if (SalesSummaryBy == "BedRoom")
                                    {
                                        columnName = "BedRoom";
                                        GroupType = "Bedroom: ";
                                    }
                                    else if (SalesSummaryBy == "Device")
                                    {
                                        columnName = "deviceName";
                                        GroupType = "Device: ";
                                    }


                                    SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(ID, user, Name, startDateTime, endDateTime, Site, Building, FloorNo, HouseNo, HouseType, Area, bedroom, device, isactive, null, null);
                                    if (SalesList != null && SalesList.Count > 0)
                                    {
                                        SalesSummarizedByReport rep = new SalesSummarizedByReport(SalesList, columnName, SelectedReport.reportname, GroupType);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    break;

                                case "InActive Sales":

                                    SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(ID, user, Name, startDateTime, endDateTime, Site, Building, FloorNo, HouseNo, HouseType, Area, bedroom, device, "0", null, null);
                                    if (SalesList != null && SalesList.Count > 0)
                                    {
                                        SalesSummarizedByReport rep = new SalesSummarizedByReport(SalesList, "", SelectedReport.reportname, "", false);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    break;
                                case "Sales Detail Report":
                                    SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(ID, user, Name, startDateTime, endDateTime, Site, Building, FloorNo, HouseNo, HouseType, Area, bedroom, device, isactive, null, null);
                                    if (SalesList != null && SalesList.Count > 0)
                                    {
                                        SalesDetailedReport rep = new SalesDetailedReport(SalesList);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    break;
                                case "Employee Commission Report":
                                    SalesList = AyatProcessManager.AyatProcessManager.GetSalesByCommissionEmployeeId(Employee);
                                    //SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(ID, user, Name, startDateTime, endDateTime, Site, Building, FloorNo, HouseNo, HouseType, Area, bedroom, device, isactive);
                                    if (SalesList != null && SalesList.Count > 0)
                                    {
                                        EmployeeCommissionReport rep = new EmployeeCommissionReport(SalesList, Employee);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    break;
                                case "Employee Report":
                                    if (DataBuffer.EmployeeList != null && DataBuffer.EmployeeList.Count > 0)
                                    {
                                        EmployeeReport rep = new EmployeeReport();
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }

                                    break;
                                case "Payment Report":
                                   List<spGetPaymentListByDateRange_Result> PaymentList = AyatProcessManager.AyatProcessManager.GetSalesPaymentWithRange(startDateTime, endDateTime);
                                    if (PaymentList != null && PaymentList.Count > 0)
                                    {
                                        PaymentReport rep = new PaymentReport(PaymentList);
                                        //ShowReport(rep);
                                        rep.ShowDialog();
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    break;


                            }

                            // }
                        }
                        else
                        {
                            XtraMessageBox.Show("Please Select Report First !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                }
            }
            catch (Exception io)
            {

            }
        }

        private void populateFilterObjects()
        {
            clearCriteriaOBJ();
            if (sleSalesList.EditValue != null && !string.IsNullOrEmpty(sleSalesList.EditValue.ToString()))
            {
                ID = sleSalesList.EditValue.ToString();
            }
            if (sleUser.EditValue != null && !string.IsNullOrEmpty(sleUser.EditValue.ToString()))
            {
                user = sleUser.EditValue.ToString();
            }
            if (sleEmployee.EditValue != null && !string.IsNullOrEmpty(sleEmployee.EditValue.ToString()))
            {
                Employee = sleEmployee.EditValue.ToString();
            }


            
            if (sleCustomerName.EditValue != null && !string.IsNullOrEmpty(sleCustomerName.EditValue.ToString()))
            {
                Name = sleUser.EditValue.ToString();
            }
            if (sleSalesLocation.EditValue != null && !string.IsNullOrEmpty(sleSalesLocation.EditValue.ToString()))
            {
                Site = sleSalesLocation.EditValue.ToString();
            }
            if (sleSalesBuilding.EditValue != null && !string.IsNullOrEmpty(sleSalesBuilding.EditValue.ToString()))
            {
                Building = sleSalesBuilding.EditValue.ToString();
            }
            if (txtFloorNo.EditValue != null && !string.IsNullOrEmpty(txtFloorNo.EditValue.ToString()))
            {
                FloorNo = txtFloorNo.EditValue.ToString();
            }
            if (sleSalesHouseNo.EditValue != null && !string.IsNullOrEmpty(sleSalesHouseNo.EditValue.ToString()))
            {
                HouseNo = sleSalesHouseNo.EditValue.ToString();
            }
            if (leSalesHouseType.EditValue != null && !string.IsNullOrEmpty(leSalesHouseType.EditValue.ToString()))
            {
                HouseType = leSalesHouseType.EditValue.ToString();
            }
            if (sleSalesArea.EditValue != null && !string.IsNullOrEmpty(sleSalesArea.EditValue.ToString()))
            {
                Area = sleSalesArea.EditValue.ToString();
            }

            if (sleDevice.EditValue != null && !string.IsNullOrEmpty(sleDevice.EditValue.ToString()))
            {
                device = sleDevice.EditValue.ToString();
            }
            if (txtSalesBedRoom.EditValue != null && !string.IsNullOrEmpty(txtSalesBedRoom.EditValue.ToString()))
            {
                bedroom = txtSalesBedRoom.EditValue.ToString();
            }
            if (cmbSummaryBy.Text != null && !string.IsNullOrEmpty(cmbSummaryBy.Text))
            {
                SalesSummaryBy = cmbSummaryBy.Text;
            }

            string dateCriteria = cmbDateCriteria.EditValue != null ? cmbDateCriteria.EditValue.ToString() : null;
            if (dateCriteria != null)
            {
                if (dtStartDate.EditValue != null && !string.IsNullOrEmpty(dtStartDate.EditValue.ToString()) && dtEndDate.EditValue != null && !string.IsNullOrEmpty(dtEndDate.EditValue.ToString()))
                {
                    if (dateCriteria == "Current Date" || dateCriteria == "At the day of")
                    {
                      //  DateTime start =Convert.ToDateTime

                        startDateTime = dtStartDate.DateTime.Date.ToString();
                        endDateTime = dtStartDate.DateTime.Date.AddHours(23).AddMinutes(59).ToString();

                    }
                    else
                    {
                        startDateTime = dtStartDate.DateTime.Date.ToString();
                        endDateTime = dtEndDate.DateTime.Date.AddHours(23).AddMinutes(59).ToString();
                        //startDateTime = dtStartDate.EditValue.ToString();
                        //endDateTime = dtEndDate.EditValue.ToString();
                    }
                }
                else
                {
                    startDateTime = null;
                    endDateTime = null;
                }
            }



            if (chkIsActive.Checked)
            {
                isactive = "1";
            }
            else
            {
                isactive = "0";
            }

        }

        private void clearCriteriaOBJ()
        {
            ID = null;
            user = null;
            Employee = null;
            Name = null;
            startDateTime = null;
            endDateTime = null;
            Site = null;
            Building = null;
            FloorNo = null;
            HouseNo = null;
            HouseType = null;
            Area = null;
            bedroom = null;
            device = null;
            isactive = null;
        }

        //XtraReport xtraReport { get; set; }
        public void ShowReport(XtraReport report)
        {
            report.RequestParameters = false;
            //xtraReport = report;



            if (!xtcReportTab.TabPages.Any(p => p.Name == "Report_" + report.Name))
            {
                if (report != null)
                {
                    XtraTabPage xtraTabPage = new XtraTabPage()
                    {
                        Name = "Report_" + report.Name,
                        Text = report.Name
                    };
                    ucReportDetail ucReportDetail1 = new ucReportDetail(report)
                    {
                        Dock = DockStyle.Fill
                    };
                    xtraTabPage.Controls.Add(ucReportDetail1);
                    xtcReportTab.TabPages.Add(xtraTabPage);
                    xtcReportTab.SelectedTabPage = xtraTabPage;
                }
            }
            else
            {
                XtraTabPage xtraTabPage = xtcReportTab.TabPages.FirstOrDefault(p => p.Name == "Report_" + report.Name);
                xtcReportTab.SelectedTabPage = xtraTabPage;
            }
        }

        private void cmbTransType_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                var clickedControl = sender as DevExpress.XtraEditors.SearchLookUpEdit;
                if (clickedControl != null)
                {
                    clickedControl.EditValue = null;
                }
                else
                {
                    var clickedControl1 = sender as DevExpress.XtraEditors.LookUpEdit;
                    if (clickedControl1 != null)
                    {
                        clickedControl1.EditValue = null;
                    }
                }
            }
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            GetSearchCriteriaData();
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            ClearSearchCriteriaData();
        }

        private void cmbDateCriteria_EditValueChanged(object sender, EventArgs e)
        {
            dtStartDate.Enabled = true;
            DateTime currentDateTime = DateTime.Now;
            string dateCriteria = cmbDateCriteria.EditValue != null ? cmbDateCriteria.EditValue.ToString() : null;
            dtEndDate.Enabled = true;
            lciStartDate.Text = "From";
            dtStartDate.Properties.ShowDropDown = ShowDropDown.Never;
            dtEndDate.Properties.ShowDropDown = ShowDropDown.Never;
            switch (dateCriteria)
            {
                case "Show All":
                    dtEndDate.EditValue = null;
                    dtStartDate.EditValue = null;
                    lciEndDate.Visibility = LayoutVisibility.Never;
                    lciStartDate.Visibility = LayoutVisibility.Never;
                    break;
                case "Current Date":
                    dtStartDate.Text = currentDateTime.Date.ToString("MM-dd-yyyy");
                    dtEndDate.Text = currentDateTime.Date.AddHours(23).AddMinutes(59).ToString("MM-dd-yyyy");
                    lciStartDate.Text = "Date";
                    dtStartDate.Properties.ReadOnly = true;
                    dtEndDate.Properties.ReadOnly = true;
                    lciEndDate.Visibility = LayoutVisibility.Never;
                    lciStartDate.Visibility = LayoutVisibility.Always;
                    break;
                case "Current Week":
                    var weekStart = DayOfWeek.Monday;
                    DateTime firstdayOfTheWeek = currentDateTime.AddDays(weekStart - currentDateTime.DayOfWeek);
                    dtStartDate.Text = firstdayOfTheWeek.Date.ToString("MM-dd-yyyy");
                    string WeekDate = currentDateTime.Date.AddHours(23).AddMinutes(59).ToString("MM-dd-yyyy");
                    dtEndDate.Text = WeekDate;
                    dtStartDate.Properties.ReadOnly = true;
                    dtEndDate.Properties.ReadOnly = true;
                    lciEndDate.Visibility = LayoutVisibility.Always;
                    lciStartDate.Visibility = LayoutVisibility.Always;
                    break;
                case "Current Month":
                    var startDateM = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
                    dtStartDate.Text = startDateM.Date.ToString("MM-dd-yyyy");
                    string Month = currentDateTime.Date.AddHours(23).AddMinutes(59).ToString("MM-dd-yyyy");
                    dtEndDate.Text = Month;
                    dtStartDate.Properties.ReadOnly = true;
                    dtEndDate.Properties.ReadOnly = true;
                    lciEndDate.Visibility = LayoutVisibility.Always;
                    lciStartDate.Visibility = LayoutVisibility.Always;
                    break;
                case "Current Year":
                    var startDateY = new DateTime(currentDateTime.Year, 1, 1);
                    dtStartDate.Text = startDateY.Date.ToString("MM-dd-yyyy");
                    Month = currentDateTime.Date.AddHours(23).AddMinutes(59).ToString("MM-dd-yyyy");
                    dtEndDate.Text = Month;
                    dtStartDate.Properties.ReadOnly = true;
                    dtEndDate.Properties.ReadOnly = true;
                    lciEndDate.Visibility = LayoutVisibility.Always;
                    lciStartDate.Visibility = LayoutVisibility.Always;
                    break;
                case "At the day of":
                    dtStartDate.EditValue = currentDateTime.Date.ToShortDateString();
                    dtStartDate.Properties.ReadOnly = false;
                    lciStartDate.Text = "At the day of ";
                    lciEndDate.Visibility = LayoutVisibility.Never;
                    lciStartDate.Visibility = LayoutVisibility.Always;
                    dtEndDate.EditValue = currentDateTime.Date.AddHours(23).AddMinutes(59).ToShortDateString();
                    dtStartDate.Properties.ShowDropDown = ShowDropDown.SingleClick;
                    break;
                case "Date Range":
                    dtStartDate.EditValue = null;
                    dtEndDate.EditValue = null;
                    dtStartDate.Properties.ReadOnly = false;
                    dtEndDate.Properties.ReadOnly = false;
                    lciEndDate.Visibility = LayoutVisibility.Always;
                    lciStartDate.Visibility = LayoutVisibility.Always;
                    dtStartDate.Properties.ShowDropDown = ShowDropDown.SingleClick;
                    dtEndDate.Properties.ShowDropDown = ShowDropDown.SingleClick;
                    break;
            }
        }

        private void UCReport_Load(object sender, EventArgs e)
        {
        }

        public static void DisplayReportFromExternalSource(string StartDate, string EndDate, string Employee, String ReportType, Sale SaleView = null)
        {  
            switch(ReportType)
            {
                case "Specific Sales":
                    if (SaleView != null)
                    {
                        SalesDetailedReport rep = new SalesDetailedReport(new List<Sale> { SaleView });
                        //ShowReport(rep);
                        rep.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "Sales Report":
                 List<Sale>   SalesList = AyatProcessManager.AyatProcessManager.GetSalesListbyFilterCriteria(null, null, null, StartDate, EndDate, null, null, null, null, null, null, null, null, "True", null, null);
                    if (SalesList != null && SalesList.Count > 0)
                    {
                        SalesDetailedReport rep = new SalesDetailedReport(SalesList);
                        //ShowReport(rep);
                        rep.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "Payment Report":

                    List<spGetPaymentListByDateRange_Result> PaymentList = AyatProcessManager.AyatProcessManager.GetSalesPaymentWithRange(StartDate, EndDate);
                    if (PaymentList != null && PaymentList.Count > 0)
                    {
                        PaymentReport rep = new PaymentReport(PaymentList);
                        //ShowReport(rep);
                        rep.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "Employee Commission Report":
                    SalesList = AyatProcessManager.AyatProcessManager.GetSalesByCommissionEmployeeId(Employee);
                     if (SalesList != null && SalesList.Count > 0)
                    {
                        EmployeeCommissionReport rep = new EmployeeCommissionReport(SalesList, Employee);
                        //ShowReport(rep);
                        rep.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "Employee Report":
                    if (DataBuffer.EmployeeList != null && DataBuffer.EmployeeList.Count > 0)
                    {
                        EmployeeReport rep = new EmployeeReport();
                        //ShowReport(rep);
                        rep.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show("There is no Report with this Criteria !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
            }

        }
    }
    public class ReportDTO
    {
        public bool select { get; set; }
        public int code { get; set; }
        public string reportname { get; set; }
        public int? parent { get; set; }
    }
    public class HouseTypeDTO
    {
        public int id { get; set; }
        public string HouseType { get; set; }
    }
}
