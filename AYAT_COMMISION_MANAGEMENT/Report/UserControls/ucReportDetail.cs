using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraReports.UI;
using System.Globalization;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;

namespace AYAT_COMMISION_MANAGEMENT
{
    public partial class ucReportDetail : UserControl
    {
        public ucReportDetail(XtraReport xtraReport)
        {
            InitializeComponent();

            xtraReport.RequestParameters = false;
            xtraReport.CreateDocument();
            documentViewer1.DocumentSource = xtraReport;
        }
        
        

    }
}
