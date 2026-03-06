using Castle.Core.Internal;
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
    /// Summary description for TestSuaTTNhanVienWinappdriver
    /// </summary>
    [TestClass]
    public class TestSuaTTNhanVienWinappdriver
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
            string path = @"D:\SuaTTNhanVien_test.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("=================================================");
                sw.WriteLine($"TEST CASE : {testName}");
                sw.WriteLine($"TIME      : {DateTime.Now}");
                sw.WriteLine("STEPS     :");

                foreach (var step in steps)
                {
                    sw.WriteLine($"  - {step}");
                }

                sw.WriteLine($"RESULT    : {result}");
                sw.WriteLine("=================================================\n");
            }
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

            // ===== TÌM TAB XÓA THEO XPATH TOÀN BỘ UI =====
            var allElements = session.FindElementsByXPath("//*");

            WindowsElement tabXoa = null;

            foreach (var el in allElements)
            {
                if (!string.IsNullOrEmpty(el.Text) && el.Text.Contains("Thông"))
                {
                    tabXoa = el;
                    break;
                }
            }

            Assert.IsNotNull(tabXoa, "Không tìm thấy tab Thông tin Nhân Viên");

            // Click trực tiếp, không cần Actions
            tabXoa.Click();

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void UI_SuaTTNhanVien_HopLe_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");
            logSteps.Add("Mở tab Thông tin nhân viên");
            
            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ grid xuất hiện
            wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            logSteps.Add("Chọn nhân viên cần sửa");
            Thread.Sleep(800);

            // ---- SỬA DỮ LIỆU ----

            var txtTenNV = session.FindElementByAccessibilityId("txtTenNV");
            txtTenNV.Clear();
            txtTenNV.SendKeys("Duy Tân");

            var txtSDTNV = session.FindElementByAccessibilityId("txtSDTNV");
            txtSDTNV.Clear();
            txtSDTNV.SendKeys("53456457");

            var cboGioiTinh = session.FindElementByAccessibilityId("cboGioiTinh");
            cboGioiTinh.Click();
            cboGioiTinh.SendKeys("Nam");

            var txtEmailr = session.FindElementByAccessibilityId("txtEmailr");
            txtEmailr.Clear();
            txtEmailr.SendKeys("duytan@gmail.com");
            logSteps.Add("Nhập lại thông tin nhân viên");
            // ---- CLICK NÚT SỬA ----

            var btnSua = wait.Until(d =>
                session.FindElementByName("Sửa"));

            

            btnSua.Click();
            btnSua.Click();
            logSteps.Add("Nhấn nút sửa");
            Thread.Sleep(1000);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Cập nhật thông tin nhân viên thành công");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST SỬA THÔNG TIN NHÂN VIÊN THÀNH CÔNG", logSteps, "PASS");
        }

        [TestMethod]
        public void UI_SuaTTNhanVien_BotrongTen_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");
            logSteps.Add("Mở tab Thông tin nhân viên");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ grid load
            wait.Until(d => session.FindElementByAccessibilityId("dataGridView1"));
            Thread.Sleep(800);
            logSteps.Add("Chọn nhân viên cần sửa");
            // ===== NHẬP DỮ LIỆU SAI =====

            var txtTenNV = session.FindElementByAccessibilityId("txtTenNV");

            txtTenNV.Click();
            txtTenNV.SendKeys(OpenQA.Selenium.Keys.Control + "a");
            txtTenNV.SendKeys(OpenQA.Selenium.Keys.Delete);

            var txtSDTNV = session.FindElementByAccessibilityId("txtSDTNV");
            txtSDTNV.Clear();
            txtSDTNV.SendKeys("0338982058"); // sai định dạng

            var cboGioiTinh = session.FindElementByAccessibilityId("cboGioiTinh");
            cboGioiTinh.Click();
            cboGioiTinh.SendKeys("Nam");

            var txtEmailr = session.FindElementByAccessibilityId("txtEmailr");
            txtEmailr.Clear();
            txtEmailr.SendKeys("duynguyen@gmail.com"); // sai format
            logSteps.Add("Nhập lại thông tin nhân viên(bỏ trống tên)");
            // ===== CLICK SỬA =====
            var btnSua = wait.Until(d => session.FindElementByName("Sửa"));
            
            btnSua.Click();
            logSteps.Add("Nhấn nút sửa");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("HIển thị thông báo:Vui lòng nhập đầy đủ thông tin");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST SỬA THÔNG TIN NHÂN VIÊN BỎ TRỐNG TÊN", logSteps, "FAIL");
        }

        [TestMethod]
        public void UI_SuaTTNhanVien_VietSaiEmail_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");
            logSteps.Add("Mở tab Thông tin nhân viên");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ grid load
            wait.Until(d => session.FindElementByAccessibilityId("dataGridView1"));
            Thread.Sleep(800);
            logSteps.Add("Chọn nhân viên cần sửa");
            // ===== NHẬP DỮ LIỆU SAI =====

            var txtTenNV = session.FindElementByAccessibilityId("txtTenNV");
            txtTenNV.Clear(); // bỏ trống tên
            txtTenNV.SendKeys("duy");

            var txtSDTNV = session.FindElementByAccessibilityId("txtSDTNV");
            txtSDTNV.Clear();
            txtSDTNV.SendKeys("0338982058"); // sai định dạng

            var cboGioiTinh = session.FindElementByAccessibilityId("cboGioiTinh");
            cboGioiTinh.Click();
            cboGioiTinh.SendKeys("Nam");

            var txtEmailr = session.FindElementByAccessibilityId("txtEmailr");
            txtEmailr.Clear();
            txtEmailr.SendKeys("duynguyen"); // sai format
            logSteps.Add("Nhập lại thông tin nhân viên(bỏ trống tên)");
            // ===== CLICK SỬA =====
            var btnSua = wait.Until(d => session.FindElementByName("Sửa"));

            btnSua.Click();
            btnSua.Click();
            logSteps.Add("Nhấn nút sửa");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông bảo:Email phải chứa '@");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");

            var btnOK2 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Email vừa nhập không hợp lệ");
            btnOK2.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST SỬA THÔNG TIN NHÂN VIÊN NHẬP SAI EMAIL", logSteps, "FAIL");
        }
        [ClassCleanup]
        public static void Cleanup()
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
