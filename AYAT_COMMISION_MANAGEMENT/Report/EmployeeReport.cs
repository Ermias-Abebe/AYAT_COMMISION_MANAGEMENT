using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using AyatProcessManager;
using System.Linq;
using AyatDataAccess;

namespace AYAT_COMMISION_MANAGEMENT.Report
{
    public partial class EmployeeReport : DevExpress.XtraReports.UI.XtraReport
    {
        public EmployeeReport()
        {
            InitializeComponent();
            List<EmployeeReportDTO> EmployeeReportlist = DataBuffer.EmployeeList.Select(x =>
             new EmployeeReportDTO()
             {
                 id = x.ID,
                 role = x.role == null ? null : x.role.Value.ToString(),
                 roleDesc = GetRoleDec(x.role),
                 email = x.email,
                 fullname = x.fullName,
                 gender = x.gender,
                 hiredate = x.hiredate.Value.ToShortDateString(),
                 phonenumber = x.mobilephone,
                 status = x.status,
                 supervisor = x.supervisor == null ? null : x.supervisor.Value.ToString(),
                 supervisorname = GetSupervisorName(x.supervisor) 
             }).ToList();


            GroupField groupField = new GroupField("roleDesc");
            GroupHeader1.GroupFields.Add(groupField);
            objectDataSource1.DataSource = EmployeeReportlist;
        }

        private string GetRoleDec(int? role)
        {
            string returnval = "Role";
            if (role != null)
            {
                Role Employerole = DataBuffer.RoleList.FirstOrDefault(x => x.ID == role.Value);
                if (role != null)
                {
                    returnval = Employerole.description;
                }
            } 
            return returnval;
        }
        private string GetSupervisorName(int? Super)
        {
            string returnval = "";
            if (Super != null)
            {
                Employee Supervisor = DataBuffer.EmployeeList.FirstOrDefault(x => x.ID == Super.Value);
                if (Super != null)
                {
                    returnval = Supervisor.fullName;
                }
            }
            return returnval;
        }


        public void ShowDialog()
        {
            ReportPrintTool rptTool = new ReportPrintTool(this)
            { AutoShowParametersPanel = false };
            rptTool.ShowPreviewDialog();
        }
    }

    public class EmployeeReportDTO
    {
        public int id { get; set; }
        public string role { get; set; }
        public string roleDesc { get; set; }
        public string fullname { get; set; }
        public string status { get; set; }
        public string gender { get; set; }
        public string hiredate { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public string supervisor { get; set; }
        public string supervisorname { get; set; }
    }
}
