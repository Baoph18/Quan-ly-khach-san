using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using Quản_lí_khách_sạn;
using System;
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
            Thread.Sleep(3000); // đợi app load
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
            Test_DangNhap_Va_MoFormThanhToan();

            // Đợi form load xong và tìm DataGridView theo AccessibilityId
            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var grid = wait.Until(d =>
    session.FindElementByAccessibilityId("dataGridView1"));

            // Lấy tất cả dòng
            var rows = grid.FindElementsByClassName("DataGridRow");

            Assert.IsTrue(rows.Count > 0, "Không có dữ liệu trong DataGrid");

            var firstRow = rows[0];

            // Click chọn dòng
            firstRow.Click();

            Thread.Sleep(500);

            // Click nút Thanh Toán
            var btnThanhToan = wait.Until(d =>
                session.FindElementByAccessibilityId("btnThanhToan"));

            btnThanhToan.Click();

            Thread.Sleep(1000);

            // Nếu có MessageBox thì xử lý
            var handles = session.WindowHandles;
            if (handles.Count > 1)
            {
                session.SwitchTo().Window(handles.Last());
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
