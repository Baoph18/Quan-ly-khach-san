using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
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
    /// Summary description for TestXoaNhanVienWinappdriver
    /// </summary>
    [TestClass]
    public class TestXoaNhanVienWinappdriver
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
            string path = @"D:\XoaNhanVien_test.txt";

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
                if (!string.IsNullOrEmpty(el.Text) && el.Text.Contains("Xoá"))
                {
                    tabXoa = el;
                    break;
                }
            }
            
            Assert.IsNotNull(tabXoa, "Không tìm thấy tab Xóa Nhân Viên");

            // Click trực tiếp, không cần Actions
            tabXoa.Click();

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void UI_XoaNhanVien_HopLe_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");
            logSteps.Add("Mở tab Xóa nhân viên");
            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            var grid = session.FindElementByAccessibilityId("dataGridView2");

            // Lấy tất cả Row thật (không lấy Top Row)
            var rows = grid.FindElementsByXPath(".//*[starts-with(@Name,'Row ')]");

            Assert.IsTrue(rows.Count > 0, "Không có dòng dữ liệu");

            // Lấy Row 0
            var row0 = rows[0];

            // Lấy cell đầu tiên bên trong Row 0
            var firstCell = row0.FindElementsByXPath(".//*")
                                .FirstOrDefault(e =>
                                    !string.IsNullOrWhiteSpace(e.Text) &&
                                    e.Text.Trim() != "Row 0");

            Assert.IsNotNull(firstCell, "Không tìm được ID cell");

            string id = firstCell.Text.Trim();

            Console.WriteLine("ID lấy được: " + id);

            Assert.IsFalse(string.IsNullOrEmpty(id), "Không đọc được ID");

            // Nhập ID vào textbox
            var txtID = session.FindElementByAccessibilityId("txtID"); // đổi lại nếu tên khác
            txtID.Clear();
            txtID.SendKeys(id);
            logSteps.Add("Nhập id vào textbox");
            Thread.Sleep(500);

            var all = session.FindElementsByXPath("//*");

            foreach (var el in all)
            {
                if (!string.IsNullOrEmpty(el.Text))
                    Console.WriteLine("Element thấy: " + el.Text);
            }
            // Click nút Xóa
            session.FindElementByAccessibilityId("btnDelete").Click();
            session.FindElementByAccessibilityId("btnDelete").Click();
            Thread.Sleep(1000);
            logSteps.Add("Nhấn nút xóa");

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("Yes"))
            );
            logSteps.Add("Hiện thị thông báo:Bạn có muốn xóa nhân viên này không");
            btnOK1.Click();
            logSteps.Add("Nhấn yes");


            
            Thread.Sleep(1500);

            var btnOK3 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiện thị thông báo:Xóa nhân viên thành công");
            btnOK3.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST XÓA NHÂN VIÊN THÀNH CÔNG", logSteps, "PASS");


        }

        [TestMethod]
        public void UI_XoaNhanVien_KhongNhapMa_ThatBai()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormNhanVien();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Nhân viên");
            logSteps.Add("Mở tab Xóa nhân viên");

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            // Chờ grid load
            var grid = session.FindElementByAccessibilityId("dataGridView2");

            Assert.IsNotNull(grid, "Không tìm thấy bảng nhân viên");

            // ===== KHÔNG CHỌN DÒNG =====

            // Click nút Xóa luôn
            session.FindElementByAccessibilityId("btnDelete").Click();
            session.FindElementByAccessibilityId("btnDelete").Click();
            logSteps.Add("Nhấn nút xóa");
            Thread.Sleep(1500);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Vui lòng nhập id nhân viên để xóa");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST XÓA NHÂN VIÊN CHƯA NHẬP ID", logSteps, "FAIL");

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
