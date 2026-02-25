using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestThemNhanVienWinappdriver
    /// </summary>
    [TestClass]
    public class TestThemNhanVienWinappdriver
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        private const string AppId =
            @"D:\Quản lí khách sạn\Quản lí khách sạn\bin\x64\Debug\Quản lí khách sạn.exe";

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
        public void Test_DangNhap_Va_MoFormNhanVien()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("btnEmplyee").Click();

            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }
        [TestMethod]
        public void UI_AddNhanVien_HopLe_ThanhCong()
        {
            Test_DangNhap_Va_MoFormNhanVien();   // nếu có login



            // Nhập dữ liệu

            session.FindElementByAccessibilityId("txtName").SendKeys("Nguyen Van B");
            session.FindElementByAccessibilityId("txtMobile").SendKeys("0909999999");
            session.FindElementByAccessibilityId("txtEmail").SendKeys("danguyen@gmail.com");
            session.FindElementByAccessibilityId("txtUserName").SendKeys("ad");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("5232532");
            var cbo = session.FindElementByAccessibilityId("txtGender");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            session.FindElementByAccessibilityId("txtEmail").SendKeys("test2@gmail.com");

            // Bấm Thêm
            session.FindElementByAccessibilityId("btnDangKy").Click();

            Thread.Sleep(1000);

            // Nếu có MessageBox
            var handles = session.WindowHandles;
            if (handles.Count > 1)
            {
                session.SwitchTo().Window(handles.Last());
                var message = session.PageSource;
                session.FindElementByName("OK").Click();

                Assert.IsTrue(message.Contains("Thành công"));
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
