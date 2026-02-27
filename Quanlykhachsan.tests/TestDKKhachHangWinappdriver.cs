using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
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

        [TestMethod]
        public void DangKy_KhachBoTrongTen_KhongThanhCong()
        {
            Test_DangNhap_Va_MoFormDangKy();

            // ===== NHẬP DATA =====

            // bỏ trống tên khách
            session.FindElementByAccessibilityId("txtName").Clear();

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



            // ===== CLICK ĐĂNG KÝ =====
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();

            Thread.Sleep(1500);



            // ===== VERIFY KHÔNG THÀNH CÔNG =====

            // nếu hệ thống báo lỗi dạng popup
            var dialog = session.WindowHandles;

            if (dialog.Count > 1)
            {
                session.SwitchTo().Window(dialog.Last());

                var msg = session.FindElementByName("Vui lòng nhập tên khách hàng");
                Assert.IsNotNull(msg);

                session.FindElementByName("OK").Click();
            }



            // kiểm tra form vẫn còn mở (chưa đóng → chưa lưu)
            var txtName = session.FindElementByAccessibilityId("txtName");
            Assert.IsTrue(txtName.Displayed);
        }
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void DangKy_NhapSoVaoTen_KhongThanhCong()
        {
            Test_DangNhap_Va_MoFormDangKy();

            session.FindElementByAccessibilityId("txtName").SendKeys("12345");
            session.FindElementByAccessibilityId("txtContact").SendKeys("0912345678");
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();

            bool popupFound = false;
            string msg = "";

            for (int i = 0; i < 5; i++)
            {
                var handles = session.WindowHandles;

                if (handles.Count > 1)
                {
                    session.SwitchTo().Window(handles.Last());

                    msg = session.PageSource;
                    popupFound = true;
                    break;
                }

                Thread.Sleep(1000);
            }

            // ===== ASSERT popup xuất hiện =====
            Assert.IsTrue(popupFound, "BUG: Hệ thống không chặn tên chứa số");

            TestContext.WriteLine("Popup text = " + msg);

            // ===== Đóng popup =====
            try
            {
                session.FindElementByName("OK").Click();
            }
            catch { }

            // ===== verify form chưa submit =====
            Assert.IsTrue(
                session.FindElementByAccessibilityId("txtName").Displayed,
                "BUG: Dữ liệu sai vẫn được lưu"
            );
        }

        [TestMethod]
        public void DangKy_NhapChuVaoSDT_KhongThanhCong()
        {
            Test_DangNhap_Va_MoFormDangKy();

            // ===== NHẬP DATA =====
            session.FindElementByAccessibilityId("txtName").SendKeys("Nguyen Van A");
            session.FindElementByAccessibilityId("txtContact").SendKeys("ABCXYZ"); // sai SĐT
            session.FindElementByAccessibilityId("txtQuocTich").SendKeys("Viet Nam");
            session.FindElementByAccessibilityId("txtGioiTinh").SendKeys("Nam");
            session.FindElementByAccessibilityId("txtMaID").SendKeys("123456789012");
            session.FindElementByAccessibilityId("txtAddress").SendKeys("Ha Noi");
            session.FindElementByAccessibilityId("txtRoomNo").SendKeys("101");
            session.FindElementByAccessibilityId("txtSoDem").SendKeys("2");
            session.FindElementByAccessibilityId("txtBed_Type").SendKeys("Đơn");
            session.FindElementByAccessibilityId("txtRoom_type").SendKeys("Vip");

            // ===== CLICK ADD =====
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();


            // ===== TẠO ROOT SESSION =====
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");

            var desktop = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723"),
                options
            );


            // ===== CHỜ POPUP =====
            WindowsElement popup = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    popup = desktop.FindElementByClassName("#32770");
                    if (popup != null)
                        break;
                }
                catch { }

                Thread.Sleep(1000);
            }


            // ===== ASSERT POPUP PHẢI CÓ =====
            Assert.IsNotNull(popup, "BUG: Không xuất hiện popup lỗi SĐT");


            // ===== LẤY TEXT POPUP =====
            string msg = popup.Text.ToLower();


            // ===== ASSERT NỘI DUNG =====
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg),
     "Popup xuất hiện nhưng không có nội dung");


            // ===== CLICK OK =====
            try
            {
                popup.FindElement(By.Name("OK")).Click();
            }
            catch { }


            // ===== FORM PHẢI CÒN =====
            Assert.IsTrue(
                session.FindElementByAccessibilityId("txtName").Displayed,
                "BUG: Form đã đóng → dữ liệu sai vẫn lưu"
            );
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
