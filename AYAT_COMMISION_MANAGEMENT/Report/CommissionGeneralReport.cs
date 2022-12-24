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
    public partial class CommissionGeneralReport : DevExpress.XtraReports.UI.XtraReport
    {

        SalesCommissionDTO data = new SalesCommissionDTO(); 
        List<SalesCommissionDTO> datalist { get; set; }
        public CommissionGeneralReport(List<Sale> SalesList, int type)
        {
            InitializeComponent();
            if (type == 1)
            {
                this.Name = "Commission Report";
                xlReportName.Text = "Commission Report";
                datalist = PopulateCommissionSummarydata(SalesList);
            }
            else if (type == 2)
            {
                this.Name = "Commission Payment Plan Report";
                xlReportName.Text = "Commission Payment Plan Report";
                datalist = PopulateCommissionPaymentPlandata(SalesList);
            }
            GroupField groupField = new GroupField("SalesId");
            GroupHeader2.GroupFields.Add(groupField);

            groupField = new GroupField("CommissionType");
            GroupHeader1.GroupFields.Add(groupField);
            objectDataSource1.DataSource = datalist;
        }
        public void ShowDialog()
        {
            ReportPrintTool rptTool = new ReportPrintTool(this)
            { AutoShowParametersPanel = false };
            rptTool.ShowPreviewDialog();
        }
        private List<SalesCommissionDTO> PopulateCommissionSummarydata(List<Sale> SalesList)
        {
            List<SalesCommissionDTO> datalist = new List<SalesCommissionDTO>(); 
            List<int> SalesIdList = SalesList.Select(x => x.ID).Distinct().ToList();

            foreach (int salesid in SalesIdList)
            { 
                Sale Sales = SalesList.FirstOrDefault(x => x.ID == salesid);
                List<Commission> Commissionlist = AyatProcessManager.AyatProcessManager.GetCommissionBySalesId(salesid);
                List<PaymentPlan> paymentplanlist = AyatProcessManager.AyatProcessManager.GetPaymentPlanBySalesID(salesid);
                List<Payment> paymentlist = AyatProcessManager.AyatProcessManager.GetPaymentBySalesid(salesid);
                List<string> InvoicePaymentlist = paymentlist.Select(x => x.FsNo).ToList();
                string PaidAmount = "0.00";
                if (paymentplanlist != null && paymentplanlist.Count > 0)
                {
                    PaidAmount = paymentplanlist.Sum(x => x.paidAmount).ToString("N");
                }

                InvoicePaymentlist = InvoicePaymentlist.Distinct().ToList();
                string invoice = "";
                foreach (string P in InvoicePaymentlist)
                {
                    if (InvoicePaymentlist.IndexOf(P) == 0)
                    {
                        invoice += P;
                    }
                    else
                    {
                        invoice += " ," + P;
                    }
                }

                decimal CommissionTotal = Commissionlist.Sum(x => x.amount);
                decimal PaidCommissionTotal = Commissionlist.Where(x => x.status == 2).Sum(x => x.amount);

                decimal Availablepercent = paymentplanlist.Where(x => x.payStatus == 1).Sum(x => x.commRate);
                decimal paidpercent = paymentplanlist.Where(x => x.payStatus == 2).Sum(x => x.commRate);

                List<int> Rolelist = Commissionlist.Select(x => x.role).ToList();
                Rolelist = Rolelist.Distinct().ToList().OrderBy(x => x).ToList();
                UserName user = DataBuffer.UserNameList.FirstOrDefault(x => x.ID == Sales.PrepareuserID);
                Employee salesEmployee = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == user.empID);
                int count = 1;

                foreach (int role in Rolelist)
                {
                    List<Commission> CommissionRateList = Commissionlist.Where(x => x.role == role).ToList();
                    List<Commission> AvailablecomList = CommissionRateList.Where(x => x.status == 1).ToList();
                    List<Commission> PaidcomList = CommissionRateList.Where(x => x.status == 2).ToList();
                    List<Commission> BankpaymentpendingcomList = CommissionRateList.Where(x => x.status == 3).ToList();
                    List<Commission> NotPaidcomList = CommissionRateList.Where(x => x.status == 0).ToList();
                    Employee roleemp = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == CommissionRateList.FirstOrDefault().empID);
                    Role roleObj = DataBuffer.RoleList.FirstOrDefault(xlReportName => xlReportName.ID == role);

                    data = new SalesCommissionDTO()
                    {
                        SN = count,
                        SalesId = Sales.code,
                        SalesDate = Sales.Date.ToShortDateString(),
                        CustomerName = Sales.FAName,
                        SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                        SalesVatAmount = Sales.VAT.ToString("N"),
                        SalesTotalAmount = Sales.Total.ToString("N"),
                        SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                        SalesPaidAmount = PaidAmount,
                        CommissionType = "1. Total Commission",
                        EmployeeName = roleemp != null ? roleemp.fullName:null,
                        EmployeeRole = roleObj.description,
                        Commissionrate = CommissionRateList.FirstOrDefault().rate.ToString("N"),
                        CommissionAmount = CommissionRateList.Sum(x => x.amount).ToString("N"),
                        SalesPaidInvoiceNo = invoice,
                        PreparedBy = salesEmployee.fullName
                    };

                    datalist.Add(data);

                    if (NotPaidcomList != null && NotPaidcomList.Count > 0)
                    {
                        data = new SalesCommissionDTO()
                        {
                            SN = count,
                            SalesId = Sales.code,
                            SalesDate = Sales.Date.ToShortDateString(),
                            CustomerName = Sales.FAName,
                            SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                            SalesVatAmount = Sales.VAT.ToString("N"),
                            SalesTotalAmount = Sales.Total.ToString("N"),
                            SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                            SalesPaidAmount = PaidAmount,
                            CommissionType = "5. Unavailable Commission",
                            EmployeeName = roleemp != null ? roleemp.fullName : null,
                            EmployeeRole = roleObj.description,
                            Commissionrate = NotPaidcomList.FirstOrDefault().rate.ToString("N"),
                            CommissionAmount = NotPaidcomList.Sum(x => x.amount).ToString("N"),
                            SalesPaidInvoiceNo = invoice,
                            PreparedBy = salesEmployee.fullName
                        };
                        datalist.Add(data);

                    }
                    if (AvailablecomList != null && AvailablecomList.Count > 0)
                    {
                        data = new SalesCommissionDTO()
                        {
                            SN = count,
                            SalesId = Sales.code,
                            SalesDate = Sales.Date.ToShortDateString(),
                            CustomerName = Sales.FAName,
                            SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                            SalesVatAmount = Sales.VAT.ToString("N"),
                            SalesTotalAmount = Sales.Total.ToString("N"),
                            SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                            SalesPaidAmount = PaidAmount,
                            CommissionType = "4. Available Commission",
                            EmployeeName = roleemp != null ? roleemp.fullName : null,
                            EmployeeRole = roleObj.description,
                            Commissionrate = AvailablecomList.FirstOrDefault().rate.ToString("N"),
                            CommissionAmount = AvailablecomList.Sum(x => x.amount).ToString("N"),
                            SalesPaidInvoiceNo = invoice,
                            PreparedBy = salesEmployee.fullName
                        };
                        datalist.Add(data);

                    }
                    if (BankpaymentpendingcomList != null && BankpaymentpendingcomList.Count > 0)
                    {
                        data = new SalesCommissionDTO()
                        {
                            SN = count,
                            SalesId = Sales.code,
                            SalesDate = Sales.Date.ToShortDateString(),
                            CustomerName = Sales.FAName,
                            SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                            SalesVatAmount = Sales.VAT.ToString("N"),
                            SalesTotalAmount = Sales.Total.ToString("N"),
                            SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                            SalesPaidAmount = PaidAmount,
                            CommissionType = "3. Bank Payment Pending Commission",
                            EmployeeName = roleemp != null ? roleemp.fullName : null,
                            EmployeeRole = roleObj.description,
                            Commissionrate = BankpaymentpendingcomList.FirstOrDefault().rate.ToString("N"),
                            CommissionAmount = BankpaymentpendingcomList.Sum(x => x.amount).ToString("N"),
                            SalesPaidInvoiceNo = invoice,
                            PreparedBy = salesEmployee.fullName
                        };
                        datalist.Add(data);

                    }
                    if (PaidcomList != null && PaidcomList.Count > 0)
                    {
                        data = new SalesCommissionDTO()
                        {
                            SN = count,
                            SalesId = Sales.code,
                            SalesDate = Sales.Date.ToShortDateString(),
                            CustomerName = Sales.FAName,
                            SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                            SalesVatAmount = Sales.VAT.ToString("N"),
                            SalesTotalAmount = Sales.Total.ToString("N"),
                            SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                            SalesPaidAmount = PaidAmount,
                            CommissionType = "2. Paid Commission",
                            EmployeeName = roleemp != null ? roleemp.fullName : null,
                            EmployeeRole = roleObj.description,
                            Commissionrate = PaidcomList.FirstOrDefault().rate.ToString("N"),
                            CommissionAmount = PaidcomList.Sum(x => x.amount).ToString("N"),
                            SalesPaidInvoiceNo = invoice,
                            PreparedBy = salesEmployee.fullName
                        };
                        datalist.Add(data);

                    }
                    count++;

                }

            }



            return datalist;

            //CommissionReport rep = new CommissionReport(datalist);

            //ReportPrintTool rptTool = new ReportPrintTool(rep) { AutoShowParametersPanel = false };
            //rptTool.ShowPreviewDialog();
        }
         
        private List<SalesCommissionDTO> PopulateCommissionPaymentPlandata(List<Sale> SalesList)
        {
            List<SalesCommissionDTO> datalist = new List<SalesCommissionDTO>();
            List<int> SalesIdList = SalesList.Select(x => x.ID).Distinct().ToList();

            foreach (int salesid in SalesIdList)
            {
                Sale Sales = SalesList.FirstOrDefault(x => x.ID == salesid);
                List<Commission> Commissionlist = AyatProcessManager.AyatProcessManager.GetCommissionBySalesId(salesid);
                List<PaymentPlan> paymentplanlist = AyatProcessManager.AyatProcessManager.GetPaymentPlanBySalesID(salesid);
                List<Payment> paymentlist = AyatProcessManager.AyatProcessManager.GetPaymentBySalesid(salesid);
                List<string> InvoicePaymentlist = paymentlist.Select(x => x.FsNo).ToList();
                string PaidAmount = "0.00";
                if (paymentplanlist != null && paymentplanlist.Count > 0)
                {
                    PaidAmount = paymentplanlist.Sum(x => x.paidAmount).ToString("N");
                }

                InvoicePaymentlist = InvoicePaymentlist.Distinct().ToList();
                string invoice = "";
                foreach (string P in InvoicePaymentlist)
                {
                    if (InvoicePaymentlist.IndexOf(P) == 0)
                    {
                        invoice += P;
                    }
                    else
                    {
                        invoice += " ," + P;
                    }
                }

                decimal CommissionTotal = Commissionlist.Sum(x => x.amount);
                decimal PaidCommissionTotal = Commissionlist.Where(x => x.status == 2).Sum(x => x.amount);

                decimal Availablepercent = paymentplanlist.Where(x => x.payStatus == 1).Sum(x => x.commRate);
                decimal paidpercent = paymentplanlist.Where(x => x.payStatus == 2).Sum(x => x.commRate);

                List<int> Rolelist = Commissionlist.Select(x => x.role).ToList();
                Rolelist = Rolelist.Distinct().ToList().OrderBy(x => x).ToList();
                UserName user = DataBuffer.UserNameList.FirstOrDefault(x => x.ID == Sales.PrepareuserID);
                Employee salesEmployee = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == user.empID);
                int count = 1;

                foreach (int role in Rolelist)
                {
                    Role roleObj = DataBuffer.RoleList.FirstOrDefault(xlReportName => xlReportName.ID == role);
                    List<Commission> CommissionRateList = Commissionlist.Where(x => x.role == role).ToList();
                    Employee roleemp = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == CommissionRateList.FirstOrDefault().empID);
                    data = new SalesCommissionDTO()
                    {
                        SN = count,
                        SalesId = Sales.code,
                        SalesDate = Sales.Date.ToShortDateString(),
                        CustomerName = Sales.FAName,
                        SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                        SalesVatAmount = Sales.VAT.ToString("N"),
                        SalesTotalAmount = Sales.Total.ToString("N"),
                        SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                        SalesPaidAmount = PaidAmount,
                        CommissionType = "1. Total Commission (100%)",
                        EmployeeName = roleemp != null ? roleemp.fullName : null,
                        EmployeeRole = roleObj.description,
                        Commissionrate = CommissionRateList.FirstOrDefault().rate.ToString("N"),
                        CommissionAmount = CommissionRateList.Sum(x => x.amount).ToString("N"),
                        SalesPaidInvoiceNo = invoice,
                        PreparedBy = salesEmployee.fullName
                    };
                    datalist.Add(data);
                    foreach (var pay in paymentplanlist.Where(x => x.commRate > 0).ToList())
                    {
                        var payRoleCom = CommissionRateList.Where(x => x.payID == pay.ID).ToList();
                
                        data = new SalesCommissionDTO()
                        {
                            SN = count,
                            SalesId = Sales.code,
                            SalesDate = Sales.Date.ToShortDateString(),
                            CustomerName = Sales.FAName,
                            SalesBeforeVat = (Sales.Total - Sales.VAT).ToString("N"),
                            SalesVatAmount = Sales.VAT.ToString("N"),
                            SalesTotalAmount = Sales.Total.ToString("N"),
                            SalesAdvertAmount = Sales.Ad == null ? "0.00" : Sales.Ad.Value.ToString("N"),
                            SalesPaidAmount = PaidAmount,
                            CommissionType = (paymentplanlist.Where(x => x.commRate > 0).ToList().IndexOf(pay) + 2) + ". Commission (" + pay.commRate.ToString("N") + "%)",
                            EmployeeName = roleemp != null ? roleemp.fullName : null,
                            EmployeeRole = roleObj.description,
                            Commissionrate = payRoleCom.FirstOrDefault().rate.ToString("N"),
                            CommissionAmount = payRoleCom.Sum(x => x.amount).ToString("N"),
                            SalesPaidInvoiceNo = invoice,
                            PreparedBy = salesEmployee.fullName
                        };

                        datalist.Add(data);
                    }
                    count++;
                }
            }
            return datalist;
        }

    }

    public class SalesCommissionDTO
    {
        public string SalesId { get; set; }
        public string SalesDate { get; set; }
        public string CustomerName { get; set; }
        public string SalesBeforeVat { get; set; }
        public string SalesVatAmount { get; set; }
        public string SalesTotalAmount { get; set; }
        public string SalesPaidAmount { get; set; }
        public string SalesPaidInvoiceNo { get; set; }
        public string SalesAdvertAmount { get; set; }
        public int SN { get; set; }
        public string CommissionType { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRole { get; set; }
        public string Commissionrate { get; set; }
        public string CommissionAmount { get; set; }
        public string PreparedBy { get; set; }

    }

}


 