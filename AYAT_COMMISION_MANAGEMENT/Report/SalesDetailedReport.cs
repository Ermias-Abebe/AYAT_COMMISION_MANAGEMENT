using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using AyatDataAccess;
using System.Linq;
using AyatProcessManager;

namespace AYAT_COMMISION_MANAGEMENT.Report
{
    public partial class SalesDetailedReport : DevExpress.XtraReports.UI.XtraReport
    {
            List<SalesSummaryDTO> datalist { get; set; }
        public SalesDetailedReport(List<Sale> SalesList)
        {
            InitializeComponent();
          
            datalist = PopulateSalesSummarydata(SalesList); 

            GroupField groupField = new GroupField("code");
            GroupHeader2.GroupFields.Add(groupField);
            objectDataSource1.DataSource = datalist;
        }

        private List<SalesSummaryDTO> PopulateSalesSummarydata(List<Sale> SalesList)
        {
            List<SalesSummaryDTO> datalist = new List<SalesSummaryDTO>();
            SalesSummaryDTO data = new  SalesSummaryDTO();
            string EmployeeName = "";

            foreach (Sale x in SalesList)
            {
                int count = 1;
                List<PaymentPlan> paymentplanlist = AyatProcessManager.AyatProcessManager.GetPaymentPlanBySalesID(x.ID);
                foreach (PaymentPlan y in paymentplanlist)
                {
                    data = new SalesSummaryDTO()
                    {
                        SN = count,
                        ID = x.ID,
                        code = x.code,
                        Type = x.Type,
                        Date = x.Date.ToShortDateString(),
                        Total = x.Total,
                        VAT = x.VAT,
                        BeforeVAT = x.Total - x.VAT,
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
                        HouseType = x.HouseType == (int)Custom_Classes.House_Type.Semi_Finished ? "Semi Furnished" : "Fully Furnished",
                        Area = x.Area.ToString("N"),
                        BedRoom = x.BedRoom,
                        isActive = x.isActive,
                        userID = x.PrepareuserID,
                        userName = GetUserName(x.PrepareuserID, ref EmployeeName),
                        userEmployeeName = EmployeeName,
                        deviceID = x.PreparedeviceID,
                        deviceName = GetDeviceName(x.PreparedeviceID),
                        remark = x.remark,
                        PaymentPlanDesc = y.description,
                        PaymentPlanPayRate = y.payRate.ToString("N"),
                        PaymentPlanPayAmount = y.payAmount.ToString("N"),
                        PaymentPlanPaidAmount = y.paidAmount.ToString("N"),
                        PaymentPlanRemainingAmount = y.RemainingAmount.ToString("N"),
                        PaymentPlanStatus = y.payStatus.ToString(),
                        PaymentPlanStatusDesc = y.payStatus == 0 ? Convert.ToInt32(y.paidAmount)>0?"Partially Paid": "Fully UnPaid" : "Fully Paid",
                        PaymentPlanCommissionRate = y.commRate.ToString("N"),
                        PaymentPlanCommissionAmount = y.commAmount.ToString()
                    };
                    datalist.Add(data);
                    count++;
                }
            }

            datalist = datalist.OrderBy(x=> x.ID).ToList();
            return datalist;

        }

        private string GetDeviceName(int DeviceId)
        {
            Device Dev = DataBuffer.DeviceList.FirstOrDefault(x => x.ID == DeviceId);

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


        public void ShowDialog()
        {
            ReportPrintTool rptTool = new ReportPrintTool(this)
            { AutoShowParametersPanel = false };
            rptTool.ShowPreviewDialog();
        }
    }
}
