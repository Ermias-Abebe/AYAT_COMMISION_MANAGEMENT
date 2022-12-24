using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Linq;
using AyatDataAccess;

namespace AYAT_COMMISION_MANAGEMENT.Report
{
    public partial class PaymentReport : DevExpress.XtraReports.UI.XtraReport
    {
        public PaymentReport(List<spGetPaymentListByDateRange_Result> paymentdata)
        {
            InitializeComponent();

            List<PaymentDTO> EmployeeReportlist = paymentdata.Select(x =>
           new PaymentDTO()
           {
               id = x.ID,
               salesid = x.salesID,
               salescode = x.SalesCode,
               salescustomer = x.FAName,
               salestotal = x.Total,
               salesdate = x.salesdate.ToShortDateString(),
               paymentreciptno = x.recieptNo,
               paymentdate = x.paymentdate.ToShortDateString(),
               paymentamount = x.amount
           }).ToList();


            GroupField groupField = new GroupField("salesid");
            GroupHeader1.GroupFields.Add(groupField);
            objectDataSource1.DataSource = EmployeeReportlist;
        }
        public void ShowDialog()
        {
            ReportPrintTool rptTool = new ReportPrintTool(this)
            { AutoShowParametersPanel = false };
            rptTool.ShowPreviewDialog();
        }
    }
    public class PaymentDTO
    {
        public int id { get; set; }
        public int salesid { get; set; }
        public string salescode { get; set; }
        public string salescustomer { get; set; }
        public decimal salestotal { get; set; }
        public string salesdate { get; set; }
        public string paymentreciptno { get; set; }
        public string paymentdate { get; set; }
        public decimal paymentamount { get; set; }
    }
}
