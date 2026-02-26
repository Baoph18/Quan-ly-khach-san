using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestDangXuatWinappdriver
    /// </summary>
    [TestClass]
    public class TestDangXuatWinappdriver
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
        public void Test_DangNhap_Va_MoFormDangXuat()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("btnInformation").Click();

            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        [TestMethod]
        public void DangXuat_ThanhCong()
        {
            Test_DangNhap_Va_MoFormDangXuat();
            

            // Bấm đăng ký
            session.FindElementByAccessibilityId("btnDangxuat").Click();

            // Đợi dialog hiện
            Thread.Sleep(1500);

            // Lấy tất cả window handle
            var handles = session.WindowHandles;

            // Switch sang window mới nhất (dialog)
            session.SwitchTo().Window(handles.Last());

            // Switch sang cửa sổ dialog
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);

                if (session.Title.Contains("Xác nhận"))
                    break;
            }

            // Click nút Yes
            session.FindElementByName("Yes").Click();
        }

        [TestMethod]
        public void Test_DangNhap_ThanhCong()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            // Đợi form Trang Chủ load (có thể lâu hơn tùy kết nối DB)
            Thread.Sleep(3000);

            // switch sang window mới
            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
            // Kiểm tra: Cửa sổ Trang Chủ xuất hiện
            // TODO: Sửa "Trang Chủ" thành tiêu đề (Text) thực tế của form TrangChủ
            var mainForm = session.FindElementByName("TrangChủ");
            Assert.IsNotNull(mainForm);
        }


        [TestMethod]
        public void Test_DangNhapSaiThongTin_HienThongBaoLoi()
        {
            // 1. Nhập username hoặc password sai
            session.FindElementByAccessibilityId("txtUserName").Clear();
            session.FindElementByAccessibilityId("txtUserName").SendKeys("saiuser");

            session.FindElementByAccessibilityId("txtPassword").Clear();
            session.FindElementByAccessibilityId("txtPassword").SendKeys("saimatkhau");

            // 2. Nhấn nút đăng nhập
            session.FindElementByAccessibilityId("btnLogin").Click();

            // 3. Chờ hệ thống phản hồi
            Thread.Sleep(2000);

            // 4. Kiểm tra thông báo lỗi hiển thị
            var errorMessage = session.FindElementByAccessibilityId("LabelError");
            // sửa lại AccessibilityId nếu label thông báo của bạn tên khác

            Assert.IsNotNull(errorMessage);
            Assert.IsTrue(errorMessage.Displayed);

            //// 5. Kiểm tra nội dung thông báo (nếu cần)
            //Assert.AreEqual("Sai tên đăng nhập hoặc mật khẩu", errorMessage.Text);
        }

        [TestMethod]
        public void Test_DangNhap_KhongNhapMatKhau_HienThongBaoTrenButton()
        {
            // nhập username
            session.FindElementByAccessibilityId("txtUserName").Clear();
            session.FindElementByAccessibilityId("txtUserName").SendKeys("admin");

            // không nhập password
            session.FindElementByAccessibilityId("txtPassword").Clear();

            // click login
            var btnLogin = session.FindElementByAccessibilityId("btnLogin");
            btnLogin.Click();

            // đợi UI cập nhật text
            Thread.Sleep(1500);

            // kiểm tra text của button đổi thành thông báo lỗi
            string buttonText = btnLogin.Text;

          
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
