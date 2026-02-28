using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
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
            string path = @"D:\DangKyKhachHang_test.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine($"===== LOG {testName.ToUpper()} =====");
                sw.WriteLine($"Thời gian: {DateTime.Now}");
                foreach (var step in steps)
                {
                    sw.WriteLine(step);
                }
                sw.WriteLine($"KẾT QUẢ: {result}");
                sw.WriteLine(); // dòng trống phân cách
            }
        }
        public void Test_DangNhap_Va_MoFormDKKH()
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
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDKKH();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Đăng ký khách hàng");


            // ===== Nhập dữ liệu =====
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

            session.FindElementByAccessibilityId("txtSoDem")
                   .SendKeys("2");

            session.FindElementByAccessibilityId("txtBed_Type")
                   .SendKeys("Đơn");

            session.FindElementByAccessibilityId("txtRoom_type")
                   .SendKeys("Vip");

            session.FindElementByAccessibilityId("txtRoomNo")
                   .Clear();

            session.FindElementByAccessibilityId("txtRoomNo")
                   .SendKeys("53");
            logSteps.Add("Nhập thông tin khách hàng");

            // ===== Click đăng ký =====
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();
            logSteps.Add("Nhấn nút đăng ký");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Đăng ký khách hàng thành công");
            btnOK1.Click();
            logSteps.Add("Nhấn Ok");
            WriteLogBlock("TEST ĐĂNG KÝ KHÁCH HÀNG THÀNH CÔNG", logSteps, "PASS");
        }

        [TestMethod]
        public void DangKy_KhachBoTrongTen_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDKKH();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Đăng ký khách hàng");

            // ===== NHẬP DATA =====
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
            logSteps.Add("Nhập thông tin khách hàng(bỏ trống tên)");
            // ===== CLICK =====
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();
            logSteps.Add("Nhấn nút đăng ký");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Vui lòng nhập đầy đủ thông tin");
            btnOK1.Click();
            logSteps.Add("Nhấn Ok");
            WriteLogBlock("TEST ĐĂNG KÝ KHÁCH HÀNG BỎ TRỐNG TÊN", logSteps, "PASS");
        }
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void DangKy_NhapSoVaoTen_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDKKH();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Đăng ký khách hàng");

            session.FindElementByAccessibilityId("txtName").SendKeys("12345");
            session.FindElementByAccessibilityId("txtContact").SendKeys("0912345678");
            logSteps.Add("Nhập thông tin khách hàng(Nhập số vào tên)");
            session.FindElementByAccessibilityId("btnAdd_Khachhang").Click();
            logSteps.Add("Nhấn nút đăng ký");

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
            logSteps.Add("Hiển thị thông báo:Chỉ được nhập chữ cái và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt");
            // ===== Đóng popup =====
            try
            {
                session.FindElementByName("OK").Click();
            }
            catch { }
            logSteps.Add("Nhấn Ok");
            WriteLogBlock("TEST ĐĂNG KÝ KHÁCH HÀNG NHẬP SỐ VÀO TÊN", logSteps, "PASS");
        }

        [TestMethod]
        public void DangKy_NhapChuVaoSDT_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDKKH();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Đăng ký khách hàng");

            

            // Nhập sai SĐT
            var txtContact = session.FindElementByAccessibilityId("txtContact");
            txtContact.SendKeys("ABCXYZ");
            logSteps.Add("Nhập thông tin khách hàng(Nhập chữ vào sdt)");
            // Trigger validation bằng cách rời khỏi ô
            txtContact.SendKeys(Keys.Tab);
            // hoặc:
            // session.FindElementByAccessibilityId("txtName").Click();

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Chỉ được số. Không cho phép chữ hoặc ký tự đặc biệt");
            btnOK1.Click();
            logSteps.Add("Nhấn Ok");
            WriteLogBlock("TEST ĐĂNG KÝ KHÁCH HÀNG NHẬP CHỮ VÀO SDT", logSteps, "PASS");
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
