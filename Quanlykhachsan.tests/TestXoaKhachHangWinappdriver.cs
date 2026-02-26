using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Collections.Generic;
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
            @"E:\Kiểm thử phần mềm\file khachsan du phong\Quản lí khách sạn du phong\Quản lí khách sạn\bin\x64\Debug\Quản lí khách sạn.exe";

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
            Test_DangNhap_Va_MoForm();

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1")); // kiểm tra lại AutomationId

            // Click vào grid
            grid.Click();
            Thread.Sleep(500);



            Thread.Sleep(500);


            

            session.FindElementByAccessibilityId("btnDelete").Click();

            // Chờ dialog xuất hiện
            Thread.Sleep(1000);

            // Switch sang cửa sổ dialog
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);

                if (session.Title.Contains("Xác nhận"))
                    break;
            }

            // Click nút Yes
            session.FindElementByName("Yes").Click();
            // Chờ MessageBox OK xuất hiện
            var ok = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var okButton = ok.Until(d =>
                session.FindElementByName("OK"));

            okButton.Click();

            // Chờ MessageBox OK xuất hiện
            var ok1 = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var okButton1 = ok1.Until(d =>
                session.FindElementByName("OK"));

            okButton1.Click();

        }

        [TestMethod]
        public void XoaKhachHang_KhongChonDong_ThatBai()
        {
            Test_DangNhap_Va_MoForm();

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid load
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            // KHÔNG click grid → không chọn khách

            // Click nút Xóa luôn
            session.FindElementByAccessibilityId("btnDelete").Click();

            Thread.Sleep(1500);

            // ===== BẮT MESSAGEBOX LỖI =====
            var handles = session.WindowHandles;

            if (handles.Count > 1)
            {
                session.SwitchTo().Window(handles.Last());

                string msg = session.PageSource;

                // Kiểm tra thông báo yêu cầu chọn khách
                Assert.IsTrue(
                    msg.Contains("chọn") ||
                    msg.Contains("Select") ||
                    msg.Contains("vui lòng"),
                    "Không xuất hiện cảnh báo khi chưa chọn khách"
                );

                session.FindElementByName("OK").Click();
            }
          
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
