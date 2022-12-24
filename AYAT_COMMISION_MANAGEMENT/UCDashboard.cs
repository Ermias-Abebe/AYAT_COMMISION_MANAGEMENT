using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors; 
using AyatDataAccess;
using static AyatProcessManager.Custom_Classes;
using DevExpress.XtraCharts;
using AyatProcessManager;
using DevExpress.XtraEditors.Controls;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class UCDashboard : XtraUserControl
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        int groupby { get; set; }

        ViewType ControlViewType = ViewType.Bar;

        List<DashboardGroupResult> DashboardsalesData { get; set; }
        List<DashboardGroupResult> DashboardPaymentData { get; set; }
        public UCDashboard()
        {
            InitializeComponent();
        }

        public void Initalize_Dashboard()
        {
            sleEmployee.Properties.DataSource = DataBuffer.EmployeeList;
            sleEmployee.Properties.DisplayMember = "fullName";
            sleEmployee.Properties.ValueMember = "ID";
            sleEmployee.EditValue = null;

            xtcDashbordTab.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            List<spGetDashboardStats_Result> bottomdata = AyatProcessManager.AyatProcessManager.GetDashboardBottomData();
            if (bottomdata != null && bottomdata.Count > 0)
            {
                lblTotalCustomer.Text = bottomdata.FirstOrDefault().TotalCustomer.ToString();
                lblTotalEmployee.Text = bottomdata.FirstOrDefault().TotalEmployee.ToString();
                lblTotalUser.Text = bottomdata.FirstOrDefault().TotalUsers.ToString();
                lblTotalSales.Text = bottomdata.FirstOrDefault().TotalSales.ToString("N");
                lblTotalPayment.Text = bottomdata.FirstOrDefault().TotalPayment.ToString("N");
                lblTotalCommision.Text = bottomdata.FirstOrDefault().TotalCommission.ToString("N");
            }
            cmbDateCriteria.EditValue = "This Month";
            cmbGroupBy.EditValue = "Days";

            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate = DateTime.Now;
            groupby = 0;
            PopulateSalesDashboardData();
            PopulateEmployeeDashboard();
            PopulateEmployeeCommission(null);

            spSalesSpitcontainer.PanelVisibility = SplitPanelVisibility.Panel1;
        }

        private void PopulateSalesDashboardData()
        {
            DashboardsalesData = AyatProcessManager.AyatProcessManager.GetDashboardSalesData(groupby, StartDate, EndDate);
            DashboardPaymentData = AyatProcessManager.AyatProcessManager.GetDashboardPaymentData(groupby, StartDate, EndDate);
            DrawChart(DashboardsalesData, DashboardPaymentData, ControlViewType); 
        }

        public void PopulateEmployeeDashboard()
        {
            var val = from E in DataBuffer.EmployeeList
                      orderby E.role
                      group E by E.role into grp
                      select new { key = grp.Key, cnt = grp.Count() };

            ccEmployeeChart.Series.Clear();
       

            Series EmployeePoints = new Series("Total Data", ViewType.Pie);
            EmployeePoints.LegendTextPattern = "{A}  ({V})";
            foreach (var v in val)
            {
                if (v.key != null)
                {
                    Role r = DataBuffer.RoleList.FirstOrDefault(x => x.ID == v.key);
                    if (r != null)
                    {

                        EmployeePoints.Points.Add(new SeriesPoint(r.description, v.cnt));
                    }
                    else
                    {
                        EmployeePoints.Points.Add(new SeriesPoint("No Role", v.cnt));
                    }
                }
                else
                {
                    EmployeePoints.Points.Add(new SeriesPoint("No Role", v.cnt));
                }
            }

            ccEmployeeChart.Series.Add(EmployeePoints);
        }

        

        private void DrawChart( List<DashboardGroupResult> DashboardsalesData, List<DashboardGroupResult> DashboardPaymentData, ViewType view)
        {
            ccSalesPayemetChart.Series.Clear();


            if (view != ViewType.Pie)
            {
                Series SalesPoints = new Series("Sales", view); 

                List<SeriesPoint> points = GetGraphSeriesPointCollection(DashboardsalesData);

                foreach (var value in points)
                {
                    //  DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(val.Key.ToString(), new double[] { Convert.ToDouble(value.Total.Value) });
                    SalesPoints.Points.Add(value);
                }
                ccSalesPayemetChart.Series.Add(SalesPoints);



                Series PaymentPoints = new Series("Payments", view); 

                points = GetGraphSeriesPointCollection(DashboardPaymentData);
                foreach (var value in points)
                {
                    //  DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(val.Key.ToString(), new double[] { Convert.ToDouble(value.Total.Value) });
                    PaymentPoints.Points.Add(value);
                }
                ccSalesPayemetChart.Series.Add(PaymentPoints);

                SetChartMarigins(ccSalesPayemetChart);
            }
            else
            {
                Series SalesPaymentPoints = new Series("Total Data", view);
                SalesPaymentPoints.LegendTextPattern = "{A}  ({V})";

                SalesPaymentPoints.Points.Add(new SeriesPoint("Sales", DashboardsalesData.Sum(x => x.Total)));
                SalesPaymentPoints.Points.Add(new SeriesPoint("Payments", DashboardPaymentData.Sum(x => x.Total))); 

                ccSalesPayemetChart.Series.Add(SalesPaymentPoints);

            }

        }
      

        private List<SeriesPoint> GetGraphSeriesPointCollection(List<DashboardGroupResult> list)
        {
            List<SeriesPoint> pointPairList = new List<SeriesPoint>();
            DateTime value = StartDate;
            DateTime t = EndDate;
            int groupBy = groupby;

            switch (groupBy)
            {
                case 0:
                    {


                        DateTime dateTime = new DateTime(value.Ticks);
                        while (dateTime <= t)
                        {
                            DashboardGroupResult dashboardGroupResult = null;
                            foreach (DashboardGroupResult result in list)
                            {
                                TimeSpan timeSpan = dateTime.Subtract(value);
                                if (result.GroupNo == timeSpan.Days)
                                {
                                    pointPairList.Add(new SeriesPoint(dateTime, (double)(result.Total ?? 0.0m)));
                                    dashboardGroupResult = result;
                                }


                            }
                            if (dashboardGroupResult != null)
                            {
                                list.Remove(dashboardGroupResult);
                            }
                            else
                            {
                                pointPairList.Add(new SeriesPoint(dateTime, 0.0));
                            }
                            dateTime = dateTime.AddDays(1.0);
                        }
                        break;
                    }
                case 1:
                    {

                        // diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
                        DateTime weekStart = value.AddDays((-1) * (int)value.DayOfWeek);
                        DateTime weekEnd = t.AddDays((-1) * (int)t.DayOfWeek + 6);
                        //diagram.AxisX.WholeRange.SetMinMaxValues(weekStart.AddDays(-7), weekEnd);
                        DateTime dateTime = weekStart;
                        while (dateTime <= weekEnd)
                        {
                            DashboardGroupResult dashboardGroupResult = null;
                            foreach (DashboardGroupResult result in list)
                            {
                                TimeSpan timeSpan = dateTime.Subtract(weekStart);
                                int? groupNo = result.GroupNo;
                                if ((groupNo.HasValue ? new double?((double)groupNo.Value) : null) == Math.Floor((double)timeSpan.Days / 7.0))
                                {
                                    pointPairList.Add(new SeriesPoint(dateTime, (double)(result.Total ?? 0m)));
                                    dashboardGroupResult = result;
                                }
                            }
                            if (dashboardGroupResult != null)
                            {
                                list.Remove(dashboardGroupResult);
                            }
                            else
                            {
                                pointPairList.Add(new SeriesPoint(dateTime, 0.0));
                            }
                            dateTime = dateTime.AddDays(7.0);
                        }
                        break;
                    }
                case 2:
                    {

                        //diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Month;
                        DateTime monthStart = value.AddDays((double)(-1 * (value.Day - 1)));
                        DateTime monthEnd = t.AddDays((double)(-1 * (t.Day - 1))).AddMonths(1).AddDays(-1);
                        // diagram.AxisX.WholeRange.SetMinMaxValues(monthStart.AddMonths(-1), monthEnd.AddDays((double)(-1*(monthEnd.Day -1))).AddDays(1));

                        DateTime dateTime = monthStart;
                        while (dateTime <= monthEnd)
                        {
                            DashboardGroupResult dashboardGroupResult = null;
                            foreach (DashboardGroupResult result in list)
                            {

                                if (result.GroupNo == dateTime.Year * 12 + dateTime.Month - (monthStart.Year * 12 + monthStart.Month))
                                {
                                    pointPairList.Add(new SeriesPoint(dateTime, (double)(result.Total ?? 0m)));
                                    dashboardGroupResult = result;
                                }
                            }
                            if (dashboardGroupResult != null)
                            {
                                list.Remove(dashboardGroupResult);
                            }
                            else
                            {
                                pointPairList.Add(new SeriesPoint(dateTime, 0.0));
                            }
                            dateTime = dateTime.AddMonths(1);
                        }
                        break;
                    }
                case 3:
                    {
                        //  diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Quarter;
                        DateTime quarterStart = value.AddDays((double)(-1 * (value.Day - 1)));
                        DateTime quarterEnd = t.AddDays((double)(-1 * (t.Day - 1)));
                        DateTime quarteStartMod = quarterStart.AddMonths(-1 * ((quarterStart.Month - 1) % 3));
                        DateTime quarterEndMod = quarterEnd.AddMonths(-1 * ((quarterEnd.Month - 1) % 3) + 3).AddDays(-1.0);
                        //diagram.AxisX.WholeRange.SetMinMaxValues(quarteStartMod.AddMonths(-3), quarterEnd.AddMonths(-1 * ((quarterStart.Month - 1) % 3) + 6).AddDays(-1.0));
                        DateTime dateTime = quarteStartMod;
                        while (dateTime <= quarterEndMod)
                        {
                            DashboardGroupResult dashboardGroupResult = null;
                            foreach (DashboardGroupResult result in list)
                            {

                                if (result.GroupNo == dateTime.Year * 4 + (dateTime.Month - 1) / 3 - (quarteStartMod.Year * 4 + (quarteStartMod.Month - 1) / 3))
                                {
                                    pointPairList.Add(new SeriesPoint(dateTime, (double)(result.Total ?? 0m)));
                                    dashboardGroupResult = result;
                                }
                            }
                            if (dashboardGroupResult != null)
                            {
                                list.Remove(dashboardGroupResult);
                            }
                            else
                            {
                                pointPairList.Add(new SeriesPoint(dateTime, 0.0));
                            }
                            dateTime = dateTime.AddMonths(3);
                        }
                        break;
                    }
                case 4:
                    {
                        //  diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Year;
                        DateTime yearStart = value.AddDays((double)(-1 * (value.DayOfYear - 1)));
                        DateTime yearEnd = t.AddDays((double)(-1 * (t.DayOfYear - 1))).AddYears(1).AddDays(-1.0);
                        // diagram.AxisX.WholeRange.SetMinMaxValues(yearStart.AddYears(-1), yearEnd.AddDays((double)(-1 * (yearEnd.DayOfYear - 1))).AddYears(1));
                        DateTime dateTime = yearStart;
                        while (dateTime <= yearEnd)
                        {
                            DashboardGroupResult dashboardGroupResult = null;
                            foreach (DashboardGroupResult result in list)
                            {

                                if (result.GroupNo == dateTime.Year - yearStart.Year)
                                {
                                    pointPairList.Add(new SeriesPoint(dateTime, (double)(result.Total ?? 0m)));
                                    dashboardGroupResult = result;
                                }
                            }
                            if (dashboardGroupResult != null)
                            {
                                list.Remove(dashboardGroupResult);
                            }
                            else
                            {
                                pointPairList.Add(new SeriesPoint(dateTime, 0.0));
                            }
                            dateTime = dateTime.AddYears(1);
                        }
                        break;
                    }

            }
            return pointPairList;
        }

        private void SetChartMarigins(ChartControl control)
        {
            try
            {
                DateTime value = StartDate;
                DateTime t = EndDate;
                int groupBy = groupby;
                XYDiagram diagram = (XYDiagram)control.Diagram;
                switch (groupBy)
                {
                    case 0:
                        {

                            diagram.AxisX.WholeRange.SetMinMaxValues(value.AddDays(-1.0), t.AddDays(1.0));
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                            break;
                        }
                    case 1:
                        {

                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
                            DateTime weekStart = value.AddDays((-1) * (int)value.DayOfWeek);
                            DateTime weekEnd = t.AddDays((-1) * (int)t.DayOfWeek + 6);
                            diagram.AxisX.WholeRange.SetMinMaxValues(weekStart.AddDays(-7), weekEnd);
                            break;
                        }
                    case 2:
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Month;
                            DateTime monthStart = value.AddDays((double)(-1 * (value.Day - 1)));
                            DateTime monthEnd = t.AddDays((double)(-1 * (t.Day - 1))).AddMonths(1).AddDays(-1);
                            diagram.AxisX.WholeRange.SetMinMaxValues(monthStart.AddMonths(-1), monthEnd.AddDays((double)(-1 * (monthEnd.Day - 1))).AddDays(1));

                            break;
                        }
                    case 3:
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Year;
                            DateTime yearStart = value.AddDays((double)(-1 * (value.DayOfYear - 1)));
                            DateTime yearEnd = t.AddDays((double)(-1 * (t.DayOfYear - 1))).AddYears(1).AddDays(-1.0);
                            diagram.AxisX.WholeRange.SetMinMaxValues(yearStart.AddYears(-1), yearEnd.AddDays((double)(-1 * (yearEnd.DayOfYear - 1))).AddYears(1));

                            break;
                        }
                    case 5:
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Quarter;
                            DateTime quarterStart = value.AddDays((double)(-1 * (value.Day - 1)));
                            DateTime quarterEnd = t.AddDays((double)(-1 * (t.Day - 1)));
                            DateTime quarteStartMod = quarterStart.AddMonths(-1 * ((quarterStart.Month - 1) % 3));
                            DateTime quarterEndMod = quarterEnd.AddMonths(-1 * ((quarterEnd.Month - 1) % 3) + 3).AddDays(-1.0);
                            diagram.AxisX.WholeRange.SetMinMaxValues(quarteStartMod.AddMonths(-3), quarterEnd.AddMonths(-1 * ((quarterStart.Month - 1) % 3) + 6).AddDays(-1.0));

                            break;
                        };
                }
            }
            catch (Exception) { }
        }

        private void cmbDateCriteria_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbDateCriteria.EditValue != null && !string.IsNullOrEmpty(cmbDateCriteria.EditValue.ToString()))
            {
                switch (cmbDateCriteria.EditValue.ToString())
                {
                    case "Today":
                        StartDate = DateTime.Now.Date;
                        break;
                    case "This Week":
                        StartDate = StartOfThisWeek();
                        break;
                    case "This Month":
                        StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        break;
                    case "This Year":
                        StartDate = new DateTime(DateTime.Today.Year, 1, 1);
                        break;
                }
                PopulateSalesDashboardData();
            }
        }
        private static DateTime StartOfThisWeek()
        {
            DayOfWeek firstDay = DayOfWeek.Monday;
            int num = DateTime.Today.DayOfWeek - firstDay;
            if (num < 0)
            {
                num += 7;
            }
            return DateTime.Today.AddDays(-num);
        }
        private void cmbGroupBy_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbGroupBy.EditValue != null && !string.IsNullOrEmpty(cmbGroupBy.EditValue.ToString()))
            {
                switch (cmbGroupBy.EditValue.ToString())
                {
                    case "Days":
                        groupby = 0;
                        break;
                    case "Weeks":
                        groupby = 1;
                        break;
                    case "Months":
                        groupby = 2;
                        break;
                    case "Years":
                        groupby = 3;
                        break;

                }
                PopulateSalesDashboardData();

            }
        }
          
        private void btnSalesBar_Click(object sender, EventArgs e)
        {
            ControlViewType = ViewType.Bar;
            PopulateSalesDashboardData();
        }

        private void btnSalesLine_Click(object sender, EventArgs e)
        {
            ControlViewType = ViewType.Line;
            PopulateSalesDashboardData();
        }

        private void cbLine_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblSalesDashboard_Click(object sender, EventArgs e)
        {
            xtcDashbordTab.SelectedTabPage = xtpSalesDashboard;
        }

        private void lblEmployeeDashboard_Click(object sender, EventArgs e)
        {

            xtcDashbordTab.SelectedTabPage = xtpEmployeeDashboard;
        }

        private void btnSalesPie_Click(object sender, EventArgs e)
        {
            ControlViewType = ViewType.Pie;
            PopulateSalesDashboardData();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            UCReport.DisplayReportFromExternalSource(StartDate.Date.ToString(), EndDate.Date.AddHours(23).AddMinutes(59).ToString(), null,"Sales Report");
        }

        private void btnPaymentReport_Click(object sender, EventArgs e)
        {
            UCReport.DisplayReportFromExternalSource(StartDate.Date.ToString(), EndDate.Date.AddHours(23).AddMinutes(59).ToString(), null, "Payment Report");
        }

        private void btnEmployeeReport_Click(object sender, EventArgs e)
        {
            UCReport.DisplayReportFromExternalSource(StartDate.Date.ToString(), EndDate.Date.AddHours(23).AddMinutes(59).ToString(), null, "Employee Report");
        }

        private void sleEmployee_EditValueChanged(object sender, EventArgs e)
        {
            if (sleEmployee.EditValue != null && !string.IsNullOrEmpty(sleEmployee.EditValue.ToString()))
                PopulateEmployeeCommission(Convert.ToInt32(sleEmployee.EditValue.ToString()));
            else
                PopulateEmployeeCommission(null);
        }

        public void PopulateEmployeeCommission(int? Employee)
        {
            if (Employee == null)
            {
                ccEmployeeCommissionChart.Series.Clear();
                Series EmployeePoints = new Series("Total Data", ViewType.Pie);
                EmployeePoints.LegendTextPattern = "{A}  ({V})";
                List<Commission> AllCommissionList = AyatProcessManager.AyatProcessManager.CommissionSelectAll();
                foreach (Employee emp in DataBuffer.EmployeeList)
                {
                    List<Commission> EmpCommissionList = AllCommissionList.Where(x => x.empID == emp.ID).ToList();
                    EmployeePoints.Points.Add(new SeriesPoint(emp.fullName, EmpCommissionList == null ? 0 : EmpCommissionList.Sum(x => x.amount)));
                }
                ccEmployeeCommissionChart.Series.Add(EmployeePoints);
            }
            else
            { 
                ccEmployeeCommissionChart.Series.Clear();
                Series EmployeePoints = new Series("Total Data", ViewType.Pie);
                EmployeePoints.LegendTextPattern = "{A}  ({V})";

                Employee Emp = DataBuffer.EmployeeList.FirstOrDefault(x=> x.ID == Employee.Value);
                List<Commission> AllEmployeeCommissionList = AyatProcessManager.AyatProcessManager.GetCommissionByEmployeeId(Employee.Value);

                List<Commission> UnavailableCommissionList = AllEmployeeCommissionList.Where(x => x.status == 0).ToList();
                List<Commission> AvailableCommissionList = AllEmployeeCommissionList.Where(x => x.status == 1).ToList();
                List<Commission> PaidCommissionList = AllEmployeeCommissionList.Where(x => x.status == 2).ToList();

                EmployeePoints.Points.Add(new SeriesPoint("Unavailable Commission", UnavailableCommissionList == null ? 0 : UnavailableCommissionList.Sum(x => x.amount)));
                EmployeePoints.Points.Add(new SeriesPoint("Available Commission", AvailableCommissionList == null ? 0 : AvailableCommissionList.Sum(x => x.amount)));
                EmployeePoints.Points.Add(new SeriesPoint("Paid Commission", PaidCommissionList == null ? 0 : PaidCommissionList.Sum(x => x.amount)));

                ccEmployeeCommissionChart.Series.Add(EmployeePoints);
            }

        }

        private void sleEmployee_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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

        private void btnEmployeeCommissionReport_Click(object sender, EventArgs e)
        {

            if (sleEmployee.EditValue != null && !string.IsNullOrEmpty(sleEmployee.EditValue.ToString()))
                UCReport.DisplayReportFromExternalSource(null, null, sleEmployee.EditValue.ToString(), "Employee Commission Report");
            else 
            UCReport.DisplayReportFromExternalSource(null, null, null, "Employee Commission Report");
        }
    }
}
