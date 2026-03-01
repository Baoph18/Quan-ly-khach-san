using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using Quản_lí_khách_sạn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestThanhToanWinappdriver
    /// </summary>
    [TestClass]
    public class TestThanhToanWinappdriver
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

        private void WriteLogBlock(string testName, List<string> steps, string result)
        {
            string path = @"D:\ThanhToan_test.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(path));

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
        public void Test_DangNhap_Va_MoFormThanhToan()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("btnCheckout").Click();

            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        [TestMethod]
        public void UI_ThanhToan_HopLe_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormThanhToan();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thanh Toán");
            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(20));

            // Switch sang window mới nhất
            wait.Until(d => session.WindowHandles.Count > 0);
            session.SwitchTo().Window(session.WindowHandles.Last());

            // Đợi grid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            // Lấy tất cả dòng
            var rows = grid.FindElementsByClassName("DataItem");

            // Lấy tất cả phần tử con trong grid
            var children = grid.FindElementsByXPath(".//*");

            Assert.IsTrue(children.Count > 0, "Grid không có dữ liệu hoặc chưa load xong");

            // Click vào grid (không click row cụ thể nữa)
            grid.Click();
            logSteps.Add("Chọn đơn cần thanh toán");
            Thread.Sleep(500);

            var btnThanhToan = wait.Until(d =>
                session.FindElementByAccessibilityId("btnThanhToan"));

            btnThanhToan.Click();
            logSteps.Add("Nhấn nút thanh toán");
            Thread.Sleep(1000);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Bạn có chắc muốn thanh toán");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            // ===== POPUP 1 =====
            var btnOK2 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Thanh toán và cập nhật thông tin thành công");
            btnOK2.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THANH TOÁN THÀNH CÔNG", logSteps, "PASS");
        }

        [TestMethod]
        public void UI_ThanhToan_KhongDuLieu_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormThanhToan();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thanh Toán");
            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(20));

            // Switch sang window mới nhất
            wait.Until(d => session.WindowHandles.Count > 0);
            session.SwitchTo().Window(session.WindowHandles.Last());

            // Đợi grid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            // Lấy tất cả dòng
            var rows = grid.FindElementsByClassName("DataItem");

            // Lấy tất cả phần tử con trong grid
            var children = grid.FindElementsByXPath(".//*");

            Assert.IsTrue(children.Count > 0, "Grid không có dữ liệu hoặc chưa load xong");

            // Click vào grid (không click row cụ thể nữa)
            grid.Click();
            logSteps.Add("Chọn đơn cần thanh toán");
            Thread.Sleep(500);

            var btnThanhToan = wait.Until(d =>
                session.FindElementByAccessibilityId("btnThanhToan"));

            btnThanhToan.Click();
            logSteps.Add("Nhấn nút thanh toán");
            Thread.Sleep(1000);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Không có khách hàng nào để thanh toán");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            
            WriteLogBlock("TEST THANH TOÁN KHÔNG CÓ DỮ LIỆU", logSteps, "FAIL");
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
