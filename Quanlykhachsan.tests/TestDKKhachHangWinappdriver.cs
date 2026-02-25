using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestDKKhachHangWinappdriver
    /// </summary>
    [TestClass]
    public class TestDKKhachHangWinappdriver
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

        public void Test_DangNhap_Va_MoFormDangKy()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("btnDKKhachHang").Click();

            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        [TestMethod]
        public void DangKy_KhachHopLe_ThanhCong()
        {
            Test_DangNhap_Va_MoFormDangKy();
            // Nhập thông tin khách
            session.FindElementByAccessibilityId("txtName")
                   .SendKeys("Nguyen Van A");

            session.FindElementByAccessibilityId("txtContact")
                   .SendKeys("0912345678");

            session.FindElementByAccessibilityId("txtQuocTich")
                   .SendKeys("Viet Nam");

            session.FindElementByAccessibilityId("txtGioiTinh")
                   .SendKeys("Nam");

            session.FindElementByAccessibilityId("txtMaID")
                   .SendKeys("123456789012");

            session.FindElementByAccessibilityId("txtAddress")
                   .SendKeys("Ha Noi");

            session.FindElementByAccessibilityId("txtRoomNo")
                   .SendKeys("101");

            session.FindElementByAccessibilityId("txtSoDem")
                   .SendKeys("2");
            session.FindElementByAccessibilityId("txtBed_Type")
                   .SendKeys("Đơn");
            session.FindElementByAccessibilityId("txtRoom_type")
                   .SendKeys("Vip");
            session.FindElementByAccessibilityId("txtRoomNo")
                   .SendKeys("1");

            // Bấm đăng ký
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();

            // Đợi dialog hiện
            Thread.Sleep(1500);

            // Lấy tất cả window handle
            var handles = session.WindowHandles;

            // Switch sang window mới nhất (dialog)
            session.SwitchTo().Window(handles.Last());

            // Click OK
            session.FindElementByName("OK").Click();
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
