using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using log4net;
using log4net.Config;

namespace Quản_lí_khách_sạn
{
    static class Program
    {
        // Khai báo một logger cho Program.cs 
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Yêu cầu Log4net đọc file config 
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            // Ghi một dòng log test ngay khi app khởi động
            log.Info("--- UNG DUNG BAT DAU---");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ĐăngNhập());
        }
    }
}
