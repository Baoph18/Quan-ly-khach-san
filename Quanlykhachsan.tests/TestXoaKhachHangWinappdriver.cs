using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestXoaKhachHangWinappdriver
    /// </summary>
    [TestClass]
    public class TestXoaKhachHangWinappdriver
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
            Thread.Sleep(3000);
        }
        private void WriteLogBlock(string testName, List<string> steps, string result)
        {
            string path = @"D:\XoaKhachHang_test.txt";

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
        public void Test_DangNhap_Va_MoForm()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();
            Thread.Sleep(3000);

            // Switch đúng window chính sau login
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                if (session.Title.Contains("Quản"))
                    break;
            }

            session.FindElementByAccessibilityId("btnCustomerDetails").Click();
            Thread.Sleep(2000);

            // Switch đúng form phòng
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                if (session.Title.Contains("Thông Tin Khách Hàng"))
                    break;
            }
        }

        [TestMethod]
        public void XoaKhachHang_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thông tin khách hàng");
            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1")); // kiểm tra lại AutomationId
            logSteps.Add("Chọn khách hàng cần xóa");
            // Click vào grid
            grid.Click();
            Thread.Sleep(500);

            session.FindElementByAccessibilityId("btnDelete").Click();
            logSteps.Add("Nhấn xóa");
            // Chờ dialog xuất hiện
            Thread.Sleep(1000);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("Yes"))
            );
            logSteps.Add("Hiện thị thông báo:Bạn có muốn xóa khách hàng này không");
            btnOK1.Click();
            logSteps.Add("Nhấn yes");


            Thread.Sleep(1500);

            // ===== POPUP 2 =====
            var btnOK2 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiện thị thông báo:Giải phóng phòng");
            btnOK2.Click();
            logSteps.Add("Nhấn ok");
            Thread.Sleep(1500);

            var btnOK3 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiện thị thông báo:Xóa khách hàng thành công");
            btnOK3.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST XÓA KHÁCH HÀNG THÀNH CÔNG", logSteps, "PASS");
        }

        //[TestMethod]
        //public void XoaKhachHang_KhongChonDong_ThatBai()
        //{
        //    Test_DangNhap_Va_MoForm();

        //    WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

        //    // Chờ DataGrid load
        //    var grid = wait.Until(d =>
        //        session.FindElementByAccessibilityId("dataGridView1"));

        //    // KHÔNG click grid → không chọn khách

        //    // Click nút Xóa luôn
        //    session.FindElementByAccessibilityId("btnDelete").Click();

        //    Thread.Sleep(1500);
        //    WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

        //    // ===== POPUP 1 =====
        //    var btnOK1 = waitPopup.Until(d =>
        //        d.FindElement(By.Name("OK"))
        //    );

        //    btnOK1.Click();

        //}

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
