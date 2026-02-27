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

        public TestContext TestContext { get; set; }
        [TestMethod]
        public void UI_AddNhanVien_HopLe_ThanhCong()
        {
            Test_DangNhap_Va_MoFormNhanVien();

            // ===== NHẬP DỮ LIỆU =====
            session.FindElementByAccessibilityId("txtName").SendKeys("Nguyen Van u");
            session.FindElementByAccessibilityId("txtMobile").SendKeys("0909999999");
            session.FindElementByAccessibilityId("txtEmail").SendKeys("te@gmail.com");
            session.FindElementByAccessibilityId("txtUserName").SendKeys("ua");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("12346");

            var cbo = session.FindElementByAccessibilityId("txtGender");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            // ===== CLICK ĐĂNG KÝ =====
            session.FindElementByAccessibilityId("btnDangKy").Click();


            // ===== TẠO DESKTOP SESSION ĐỂ BẮT POPUP =====
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");

            var desktopSession = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723"),
                options
            );


            // ===== CHỜ POPUP XUẤT HIỆN =====
            WindowsElement popup = null;

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    popup = desktopSession.FindElementByXPath("//*[contains(@Name,'thành công')]");
                    if (popup != null)
                        break;
                }
                catch { }

                Thread.Sleep(500);
            }


            // ===== VERIFY POPUP =====
            Assert.IsNotNull(popup, "Không xuất hiện popup thành công");



            // ===== CLICK OK =====
            try
            {
                desktopSession.FindElementByName("OK").Click();
            }
            catch { }



            // ===== LOG PASS =====
            Console.WriteLine("✔ PASS: Thêm nhân viên thành công");
        }

        [TestMethod]
        public void UI_AddNhanVien_KhongHopLe_ThatBai()
        {
            Test_DangNhap_Va_MoFormNhanVien();

            // ===== NHẬP DỮ LIỆU SAI =====

            // Bỏ trống tên
            var txtName = session.FindElementByAccessibilityId("txtName");
            txtName.Clear();

            // SĐT nhập chữ → sai định dạng
            var txtMobile = session.FindElementByAccessibilityId("txtMobile");
            txtMobile.Clear();
            txtMobile.SendKeys("ABCXYZ");

            session.FindElementByAccessibilityId("txtEmail").SendKeys("saiemail");

            session.FindElementByAccessibilityId("txtUserName").SendKeys("userfail");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");

            var cbo = session.FindElementByAccessibilityId("txtGender");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            // ===== CLICK THÊM =====
            session.FindElementByAccessibilityId("btnDangKy").Click();

            Thread.Sleep(1500);

            // ===== KIỂM TRA MESSAGEBOX LỖI =====
            var handles = session.WindowHandles;

            if (handles.Count > 1)
            {
                session.SwitchTo().Window(handles.Last());

                string message = session.PageSource;

                // Kiểm tra có thông báo lỗi
                Assert.IsTrue(
                    message.Contains("lỗi") ||
                    message.Contains("không hợp lệ") ||
                    message.Contains("vui lòng"),
                    "Không xuất hiện thông báo lỗi khi nhập sai dữ liệu"
                );

                session.FindElementByName("OK").Click();
            }
        
        }

        [TestMethod]
        public void UI_AddNhanVien_BoTrongBatBuoc_HienThongBaoDungNoiDung()
        {
            // 1. Login & mở form nhân viên
            Test_DangNhap_Va_MoFormNhanVien();

            // 2. Nhập dữ liệu sai/bỏ trống
            session.FindElementByAccessibilityId("txtName").Clear(); // bỏ trống tên
            session.FindElementByAccessibilityId("txtMobile").SendKeys("4354675863"); // chữ -> sai
            session.FindElementByAccessibilityId("txtEmail").SendKeys("abc@gmail.com");
            session.FindElementByAccessibilityId("txtUserName").SendKeys("utt");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");

            // chọn giới tính
            var cbo = session.FindElementByAccessibilityId("txtGender");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            // 3. Click Thêm
            session.FindElementByAccessibilityId("btnDangKy").Click();

            Thread.Sleep(1500); // chờ popup hiện

            // ===== VERIFY KHÔNG THÀNH CÔNG =====

            // nếu hệ thống báo lỗi dạng popup
            var dialog = session.WindowHandles;

            if (dialog.Count > 1)
            {
                session.SwitchTo().Window(dialog.Last());

                var msg = session.FindElementByName("Vui lòng nhập tên nhân viên ");
                Assert.IsNotNull(msg);

                session.FindElementByName("OK").Click();
            }



            // kiểm tra form vẫn còn mở (chưa đóng → chưa lưu)
            var txtName = session.FindElementByAccessibilityId("txtName");
            Assert.IsTrue(txtName.Displayed);
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
