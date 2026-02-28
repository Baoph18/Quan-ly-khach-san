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
    /// Summary description for TestThemNhanVienWinappdriver
    /// </summary>
    [TestClass]
    public class TestThemNhanVienWinappdriver
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
        private void WriteLogBlock(string testName, List<string> steps, string result)
        {
            string path = @"D:\ThemNhanVien_test.txt";

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
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void UI_AddNhanVien_HopLe_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");
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
            logSteps.Add("Nhập thông tin nhân viên");
            // ===== CLICK ĐĂNG KÝ =====
            session.FindElementByAccessibilityId("btnDangKy").Click();
            logSteps.Add("Nhấn nút đăng ký");

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiện thị thông báo:Đăng ký nhân viên thành công");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");

            Thread.Sleep(500); // cho popup 2 kịp xuất hiện

            // ===== POPUP 1 =====
            var btnOK2 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Tạo tài khoản thành công");
            btnOK2.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM NHÂN VIÊN THÀNH CÔNG", logSteps, "PASS");
        }

        [TestMethod]
        public void UI_AddNhanVien_KhongHopLe_ThatBai()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");

            // ===== NHẬP DỮ LIỆU SAI =====

            // Bỏ trống tên
            var txtName = session.FindElementByAccessibilityId("txtName");
            txtName.Clear();
            txtName.SendKeys("sf");
            // SĐT nhập chữ → sai định dạng
            var txtMobile = session.FindElementByAccessibilityId("txtMobile");
            txtMobile.Clear();
            txtMobile.SendKeys("ABCXYZ");
            logSteps.Add("Nhập thông tin nhân viên(nhập chữ vào sdt");




            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Chỉ được số. Không cho phép chữ hoặc ký tự đặc biệt");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM NHÂN VIÊN NHẬP CHỮ VÀO SDT", logSteps, "PASS");

        }

        [TestMethod]
        public void UI_AddNhanVien_BoTrongBatBuoc_HienThongBaoDungNoiDung()
        {
            // 1. Login & mở form nhân viên
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");

            // 2. Nhập dữ liệu sai/bỏ trống
            session.FindElementByAccessibilityId("txtName").Clear(); // bỏ trống tên
            session.FindElementByAccessibilityId("txtMobile").SendKeys("4354675863");
            session.FindElementByAccessibilityId("txtEmail").SendKeys("abc@gmail.com");
            session.FindElementByAccessibilityId("txtUserName").SendKeys("utt");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");

            // chọn giới tính
            var cbo = session.FindElementByAccessibilityId("txtGender");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);
            logSteps.Add("Nhập thông tin nhân viên(bỏ trống tên)");
            // 3. Click Thêm
            session.FindElementByAccessibilityId("btnDangKy").Click();
            logSteps.Add("Nhấn nút đăng ký");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Vui lòng nhập thông tin đầy đủ");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM NHÂN VIÊN BỎ TRỐNG TÊN", logSteps, "PASS");
        }
        [TestMethod]
        public void UI_AddNhanVien_SaiEmail_HienThongBaoDungNoiDung()
        {
            // 1. Login & mở form nhân viên
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");

            // 2. Nhập dữ liệu sai/bỏ trống
            session.FindElementByAccessibilityId("txtName").SendKeys("bi"); // bỏ trống tên
            session.FindElementByAccessibilityId("txtMobile").SendKeys("4354675863");
            session.FindElementByAccessibilityId("txtEmail").SendKeys("abcgmail.com");
            session.FindElementByAccessibilityId("txtUserName").SendKeys("utt");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");

            // chọn giới tính
            var cbo = session.FindElementByAccessibilityId("txtGender");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);
            logSteps.Add("Nhập thông tin nhân viên(nhập sai email)");
            // 3. Click Thêm
            session.FindElementByAccessibilityId("btnDangKy").Click();
            logSteps.Add("Nhấn nút đăng ký");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông bảo:Email phải chứa '@");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");

            
            WriteLogBlock("TEST THÊM NHÂN VIÊN NHẬP SAI EMAIL", logSteps, "PASS");
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
