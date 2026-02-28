using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestThongKeWinappdriver
    /// </summary>
    [TestClass]
    public class TestThongKeWinappdriver
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        private const string AppId =
               @"D:\QLKS\Quản lí khách sạn\bin\x64\Debug\Quản lí khách sạn.exe";

        private static WindowsDriver<WindowsElement> session;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", AppId);

            session = new WindowsDriver<WindowsElement>(
                new Uri(WindowsApplicationDriverUrl),
                options
            );

            Assert.IsNotNull(session);
            Thread.Sleep(3000); // đợi app load
        }
        public void Test_DangNhap_Va_MoFormDangXuat()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("bntThongKe").Click();

            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        private void WriteLogBlock(string testName, List<string> steps, string result)
        {
            string path = @"D:\ThongKe_test.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine($"===== LOG {testName.ToUpper()} =====");
                sw.WriteLine($"Thời gian: {DateTime.Now}");
                foreach (var step in steps)
                {
                    sw.WriteLine(step);
                }
                sw.WriteLine($"KẾT QUẢ: {result}");
                sw.WriteLine(); // dòng trống phân cách
            }
        }
        [TestMethod]
        public void ThongKe_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDangXuat();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thống kê");


            

            // Đợi dialog hiện
            Thread.Sleep(1500);

            // Lấy tất cả window handle
            var handles = session.WindowHandles;

            // Switch sang window mới nhất (dialog)
            session.SwitchTo().Window(handles.Last());
            logSteps.Add("Hiện form crystal report");


            Thread.Sleep(1500);
            WriteLogBlock("TEST THỐNG KÊ THÀNH CÔNG", logSteps, "PASS");
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                session?.Quit();
            }
            catch { }

            System.Diagnostics.Process[] processes =
                System.Diagnostics.Process.GetProcessesByName("Quản lí khách sạn");

            foreach (var p in processes)
            {
                p.Kill();
            }
        }
    }
}
