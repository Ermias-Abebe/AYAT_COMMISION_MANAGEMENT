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
    public partial class EmployeeCommissionReport : DevExpress.XtraReports.UI.XtraReport
    {
        string specificEmployee { get; set; }

        public EmployeeCommissionReport(List<Sale> SalesList, string Employee)
        {
            InitializeComponent();
            specificEmployee = Employee;
            List<EmployeeCommissionDTO> commissionDTOList = PopulateCommissionSummarydata(SalesList);

            GroupField groupField = new GroupField("EmployeeName");
            GroupHeader1.GroupFields.Add(groupField);
            objectDataSource1.DataSource = commissionDTOList;
        }


        private List<EmployeeCommissionDTO> PopulateCommissionSummarydata(List<Sale> SalesList)
        {
            List<EmployeeCommissionDTO> datalist = new List<EmployeeCommissionDTO>();
            List<int> SalesIdList = SalesList.Select(x => x.ID).Distinct().ToList();
            List<Commission> CommissionList = new List<Commission>();
            List<PaymentPlan> paymentplanlist = new List<PaymentPlan>();
            foreach (int salesid in SalesIdList)
            {
                Sale Sales = SalesList.FirstOrDefault(x => x.ID == salesid);
                List<Commission> Commission = AyatProcessManager.AyatProcessManager.GetCommissionBySalesId(salesid);
                List<PaymentPlan> paymentplan = AyatProcessManager.AyatProcessManager.GetPaymentPlanBySalesID(salesid);
                CommissionList.AddRange(Commission);
                paymentplanlist.AddRange(paymentplan);
            }

            List<int?> EmploayeeIdList = CommissionList.Where(x => x.empID != null).Select(x => x.empID).Distinct().ToList();

            if(!string.IsNullOrEmpty(specificEmployee))
            {
                EmploayeeIdList = new List<int?>() { Convert.ToInt32( specificEmployee) };

            }

            foreach (int Empid in EmploayeeIdList)
            {
                List<Commission> EmployeeCommissionList = CommissionList.Where(x => x.empID == Empid).ToList() ;

                List<int> EmpSalesIdList = EmployeeCommissionList.Select(x => x.salesID).Distinct().ToList();

                List<Sale> EmpSalesList = SalesList.Where(x=> EmpSalesIdList.Contains( x.ID)).ToList();
                 
              

                foreach (Sale sal in EmpSalesList)
                {
                    List<int> CommissionRoleList = EmployeeCommissionList.Where(s=> s.salesID == sal.ID).Select(x => x.role ).Distinct().ToList().ToList(); 

                    Employee roleemp = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == Empid);
                    foreach (int Empcomrole in CommissionRoleList)
                    {
                        List<Commission> EmployeeRoleCommissionList = EmployeeCommissionList.Where(x=> x.role == Empcomrole).ToList();
                        List<Commission> UnavailableEmployeeRoleCommissionList = EmployeeRoleCommissionList.Where(x => x.status == 0).ToList();
                        List<Commission> AvailableEmployeeRoleCommissionList = EmployeeRoleCommissionList.Where(x => x.status == 1).ToList();
                        List<Commission> PaidEmployeeRoleCommissionList = EmployeeRoleCommissionList.Where(x => x.status == 2).ToList();
                        Role emprole = DataBuffer.RoleList.FirstOrDefault(x=> x.ID == Empcomrole);

                        EmployeeCommissionDTO data = new EmployeeCommissionDTO()
                        {
                            SalesId = sal.code,
                            EmployeeName = roleemp.fullName,
                            CommissionRole = emprole.description,
                            CommissionAvailablemount = AvailableEmployeeRoleCommissionList == null ? "0" : AvailableEmployeeRoleCommissionList.Sum(x => x.amount).ToString("N"),
                            CommissionPaidAmount = PaidEmployeeRoleCommissionList == null ? "0" : PaidEmployeeRoleCommissionList.Sum(x => x.amount).ToString("N"),
                            Commissionrate = EmployeeRoleCommissionList.Select(x=> x.rate).First().ToString("N"),
                            CommissionUnAvailablemount = UnavailableEmployeeRoleCommissionList == null ? "0" : UnavailableEmployeeRoleCommissionList.Sum(x => x.amount).ToString("N"),
                            CommissionTotalAmount = EmployeeRoleCommissionList == null ? "0" : EmployeeRoleCommissionList.Sum(x => x.amount).ToString("N"),
                            SalesTotalAmount = sal.Total.ToString("N")
                        };
                        datalist.Add(data);

                    }

                      

                }

            }



            return datalist;

            //CommissionReport rep = new CommissionReport(datalist);

            //ReportPrintTool rptTool = new ReportPrintTool(rep) { AutoShowParametersPanel = false };
            //rptTool.ShowPreviewDialog();
        }


        public void ShowDialog()
        {
            ReportPrintTool rptTool = new ReportPrintTool(this)
            { AutoShowParametersPanel = false };
            rptTool.ShowPreviewDialog();
        }

    }

    public class EmployeeCommissionDTO
    {
        public int SN { get; set; }
        public string SalesId { get; set; } 
        public string SalesTotalAmount { get; set; }   
        public string EmployeeName { get; set; }
        public string CommissionRole { get; set; }
        public string Commissionrate { get; set; }
        public string CommissionTotalAmount { get; set; }
        public string CommissionAvailablemount { get; set; }
        public string CommissionPaidAmount { get; set; }
        public string CommissionUnAvailablemount { get; set; }

    }
}
