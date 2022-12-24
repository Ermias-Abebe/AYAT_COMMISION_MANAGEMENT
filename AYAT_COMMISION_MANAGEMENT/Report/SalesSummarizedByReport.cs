using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using AyatDataAccess;
using System.Linq;
using AyatProcessManager;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class SalesSummarizedByReport : DevExpress.XtraReports.UI.XtraReport
    {

        List<SalesSummaryDTO> datalist { get; set; }
        public SalesSummarizedByReport(List<Sale> SalesList, string GroupColumnname, string ReportName, string GroupType, bool summary = true)
        {
            InitializeComponent();
            this.Name = ReportName;
            xlReportName.Text = ReportName;
            datalist = PopulateSalesSummarydata(SalesList);
            if (summary)
            {
                xlGroupbyName.Text = GroupType+" [" + GroupColumnname + "]";
                GroupField groupField = new GroupField(GroupColumnname);
                GroupHeader1.GroupFields.Add(groupField);
            }else
            {
                xlGroupbyName.Text = "InActive Sales";
            }

            objectDataSource2.DataSource = datalist;
        }
        public void ShowDialog()
        {
            ReportPrintTool rptTool = new ReportPrintTool(this)
            { AutoShowParametersPanel = false };
            rptTool.ShowPreviewDialog();
        }
        private List<SalesSummaryDTO> PopulateSalesSummarydata(List<Sale> SalesList)
        {
          
            List<SalesSummaryDTO> datalist = new List<SalesSummaryDTO>();
            string EmployeeName = "";
            datalist = SalesList.Select(x => new SalesSummaryDTO()
            {
                ID = x.ID,
                code = x.code,
                Type = x.Type,
                Date = x.Date.ToShortDateString(),
                Total = x.Total,
                VAT = x.VAT,
                Ad = x.Ad,
                FirstPayment = x.FirstPayment,
                RegFee = x.RegFee,
                FAName = x.FAName,
                SAName = x.SAName,
                HomePhone = x.HomePhone,
                MobilePhone = x.MobilePhone,
                Site = x.Site,
                BuildingNo = x.BuildingNo,
                FloorNo = x.FloorNo,
                HouseNo = x.HouseNo,
                HouseType = x.HouseType == 0 ? "Semi Furnished" : "Fully Furnished",
                Area = x.Area.ToString("N"),
                BedRoom = x.BedRoom,
                isActive = x.isActive,
                userID = x.PrepareuserID,
                userName = GetUserName(x.PrepareuserID, ref EmployeeName),
                userEmployeeName = EmployeeName,
                deviceID = x.PreparedeviceID,
                deviceName = GetDeviceName(x.PreparedeviceID),
                remark = x.remark
            }).ToList();

            


            return datalist;
             
        }

        private string GetDeviceName(int DeviceId)
        {
            Device Dev = DataBuffer.DeviceList.FirstOrDefault(x=> x.ID == DeviceId);

            return Dev == null ? "" : Dev.deviceName;
        }

        private string GetUserName(int userID, ref string employeeName)
        {
            UserName User = DataBuffer.UserNameList.FirstOrDefault(x => x.ID == userID);
            if (User != null)
            {
                Employee Emp = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == User.empID);
                if (Emp != null)
                {
                    employeeName = Emp.fullName;
                }
            }
            return User == null ? "" : User.name;
        }
    }
    public class SalesSummaryDTO
    {
        public int SN { get; set; }
        public int ID { get; set; }
        public string code { get; set; }
        public int Type { get; set; }
        public string Date { get; set; }
        public decimal Total { get; set; }
        public decimal VAT { get; set; }
        public decimal BeforeVAT { get; set; }
        public decimal? Ad { get; set; }
        public decimal? FirstPayment { get; set; }
        public decimal? RegFee { get; set; }
        public string FAName { get; set; }
        public string SAName { get; set; }  
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; } 
        public string Site { get; set; }
        public string BuildingNo { get; set; }
        public int FloorNo { get; set; }
        public string HouseNo { get; set; }
        public string HouseType { get; set; } 
        public string Area { get; set; }
        public int BedRoom { get; set; }
        public bool isActive { get; set; }
        public int userID { get; set; }
        public string userName { get; set; }
        public string userEmployeeName { get; set; }
        public int deviceID { get; set; }
        public string deviceName { get; set; }
        public string remark { get; set; }
        public string PaymentPlanDesc { get; set; }
        public string PaymentPlanPayRate { get; set; }
        public string PaymentPlanPayAmount { get; set; }
        public string PaymentPlanPaidAmount { get; set; }
        public string PaymentPlanRemainingAmount { get; set; }
        
        public string PaymentPlanStatus { get; set; }
        public string PaymentPlanStatusDesc { get; set; }
        public string PaymentPlanCommissionRate { get; set; }
        public string PaymentPlanCommissionAmount { get; set; }
    }


}


 