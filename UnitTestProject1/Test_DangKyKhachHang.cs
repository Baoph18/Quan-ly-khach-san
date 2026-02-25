using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    [TestClass]
    public class DangKyKhachHangTests
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        private const string AppId =
            @"D:\Quản lí khách sạn\bin\Debug\Quản lí khách sạn.exe";

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

        [TestMethod]
        public void DangKy_KhachHopLe_ThanhCong()
        {
            // Nhập thông tin khách
            session.FindElementByAccessibilityId("txtTen")
                   .SendKeys("Nguyen Van A");

            session.FindElementByAccessibilityId("txtSDT")
                   .SendKeys("0912345678");

            session.FindElementByAccessibilityId("txtQuocTich")
                   .SendKeys("Viet Nam");

            session.FindElementByAccessibilityId("cboGioiTinh")
                   .SendKeys("Nam");

            session.FindElementByAccessibilityId("txtMaDD")
                   .SendKeys("123456789012");

            session.FindElementByAccessibilityId("txtDiaChi")
                   .SendKeys("Ha Noi");

            session.FindElementByAccessibilityId("txtSoPhong")
                   .SendKeys("101");

            session.FindElementByAccessibilityId("txtSoDem")
                   .SendKeys("2");

            // Bấm đăng ký
            session.FindElementByAccessibilityId("btnDangKy").Click();

            Thread.Sleep(1500);

            // Kiểm tra MessageBox thành công
            var messageBox = session.FindElementByName("Thông báo");
            Assert.IsNotNull(messageBox);

            session.FindElementByName("OK").Click();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            session?.Quit();
        }
    }
}