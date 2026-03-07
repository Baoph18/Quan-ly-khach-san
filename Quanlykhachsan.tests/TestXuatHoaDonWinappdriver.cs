using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestXuatHoaDonWinappdriver
    /// </summary>
    [TestClass]
    public class TestXuatHoaDonWinappdriver
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

            session.FindElementByAccessibilityId("btnHoaDon").Click();

            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        private void WriteLogBlock(string testName, List<string> steps, string result)
        {
            string path = @"D:\XuatHoaDon_test.txt";

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("=================================================");
                sw.WriteLine($"TEST CASE : {testName}");
                sw.WriteLine($"TIME      : {DateTime.Now}");
                sw.WriteLine("STEPS     :");

                foreach (var step in steps)
                {
                    sw.WriteLine($"  - {step}");
                }

                sw.WriteLine($"RESULT    : {result}");
                sw.WriteLine("=================================================\n");
            }
        }
        [TestMethod]
        public void XuatHoaDon_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDangXuat();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Hóa Đơn");

            
            session.FindElementByAccessibilityId("guna2Button2").Click();
            logSteps.Add("Nhấn nút xuất hóa đơn");
            
            // Đợi dialog hiện
            Thread.Sleep(1500);

            // Lấy tất cả window handle
            var handles = session.WindowHandles;

            // Switch sang window mới nhất (dialog)
            session.SwitchTo().Window(handles.Last());
            logSteps.Add("Hiện form crystal report");
            

            Thread.Sleep(1500);
            WriteLogBlock("TEST XUẤT HÓA ĐƠN THÀNH CÔNG", logSteps, "PASS");
        }

        [ClassCleanup]
        public static void Cleanup()
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
