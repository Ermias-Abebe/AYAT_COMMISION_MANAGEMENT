using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AYAT_COMMISION_MANAGEMENT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.XtraEditors.WindowsFormsSettings.SetDPIAware();
            DevExpress.XtraEditors.WindowsFormsSettings.EnableFormSkins();
            DevExpress.XtraEditors.WindowsFormsSettings.ForceDirectXPaint();
            DevExpress.XtraEditors.WindowsFormsSettings.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.True;
            DevExpress.XtraEditors.WindowsFormsSettings.ScrollUIMode = DevExpress.XtraEditors.ScrollUIMode.Touch;
            float fontSize = 11f;
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = new Font("Segoe UI", fontSize);
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultMenuFont = new Font("Segoe UI", fontSize);
            DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
            if (!AyatProcessManager.DataBuffer.CheckTrialDateExpiry())
                return;
                AyatProcessManager.DataBuffer.GetBufferData();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AyatProcessManager.DataBuffer.thisDevice = AyatProcessManager.AyatProcessManager.GetDeviceByName(SystemInformation.ComputerName).FirstOrDefault();
            if (AyatProcessManager.DataBuffer.thisDevice != null)
            {
                LoginPage login = new LoginPage();
                login.ShowDialog();
                if (login.Authenticated)
                    Application.Run(new MainForm());
            }
            else
            {
                XtraMessageBox.Show("Please Register Your Computer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
