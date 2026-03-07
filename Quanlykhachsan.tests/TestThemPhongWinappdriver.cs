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
    /// Summary description for TestThemPhongWinappdriver
    /// </summary>
    [TestClass]
    public class TestThemPhongWinappdriver
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
            string path = @"D:\ThemPhong_test.txt";

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
        public void Test_DangNhap_Va_MoForm()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();
            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("btnthphong").Click();
            
            

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        [TestMethod]
        public void ThêmPhòng_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");
            // Nhập thông tin khách
            session.FindElementByAccessibilityId("txtSophong")
                   .SendKeys("99");

            var cbo = session.FindElementByAccessibilityId("txtLoaiphong");
            cbo.Click();
            cbo.SendKeys("Thường");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            var cbo1 = session.FindElementByAccessibilityId("txtLoaigiuong");
            cbo1.Click();
            cbo1.SendKeys("Đơn");
            cbo1.SendKeys(OpenQA.Selenium.Keys.Enter);
            

            session.FindElementByAccessibilityId("txtGiatien")
                   .SendKeys("260000");
            logSteps.Add("Nhập thông tin phòng");


            // Bấm đăng ký
            session.FindElementByAccessibilityId("btnAddRoom").Click();
            session.FindElementByAccessibilityId("btnAddRoom").Click();
            logSteps.Add("Nhấn nút Thêm phòng");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Đã thêm phòng thành công!"))
            );
            Assert.AreEqual(
                "Đã thêm phòng thành công!",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Đã thêm phòng thành công");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM PHÒNG THÀNH CÔNG", logSteps, "PASS");
        }

        [TestMethod]
        public void ThemPhong_TrungSoPhong_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");
            // Nhập thông tin khách
            session.FindElementByAccessibilityId("txtSophong")
                   .SendKeys("99");

            var cbo = session.FindElementByAccessibilityId("txtLoaiphong");
            cbo.Click();
            cbo.SendKeys("Thường");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            var cbo1 = session.FindElementByAccessibilityId("txtLoaigiuong");
            cbo1.Click();
            cbo1.SendKeys("Đơn");
            cbo1.SendKeys(OpenQA.Selenium.Keys.Enter);

            session.FindElementByAccessibilityId("txtGiatien")
                   .SendKeys("260000");
            logSteps.Add("Nhập thông tin phòng");


            // Bấm đăng ký
            session.FindElementByAccessibilityId("btnAddRoom").Click();
            session.FindElementByAccessibilityId("btnAddRoom").Click();
            logSteps.Add("Nhấn nút Thêm phòng");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Số phòng đã tồn tại! Vui lòng nhập số khác."))
            );
            Assert.AreEqual(
                "Số phòng đã tồn tại! Vui lòng nhập số khác.",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Số phòng đã tồn tại!Vui lòng nhập số khác");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM PHÒNG TRÙNG SỐ PHÒNG", logSteps, "FAIL");
        }

        [TestMethod]
        public void ThemPhong_NhapTienAm_KhongHopLe_ThatBai()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");

            // ===== NHẬP DỮ LIỆU SAI =====

            // Số phòng hợp lệ
            session.FindElementByAccessibilityId("txtSophong")
                   .SendKeys("100");


            var cbo = session.FindElementByAccessibilityId("txtLoaiphong");
            cbo.Click();
            cbo.SendKeys("Vip");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            var cbo1 = session.FindElementByAccessibilityId("txtLoaigiuong");
            cbo1.Click();
            cbo1.SendKeys("Đơn");
            cbo1.SendKeys(OpenQA.Selenium.Keys.Enter);

            // Giá âm → sai dữ liệu
            session.FindElementByAccessibilityId("txtGiatien")
                   .SendKeys("-500");

            logSteps.Add("Nhập thông tin phòng(giá tiền âm");
            


            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt."))
            );
            Assert.AreEqual(
                "Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Chỉ được số. Không cho phép chữ hoặc ký tự đặc biệt");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM PHÒNG NHẬP TIỀN ÂM", logSteps, "FAIL");
        }

        [TestMethod]
        public void ThemPhong_BoTrong_ThatBai()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");

            // ===== NHẬP DỮ LIỆU SAI =====

            // Số phòng hợp lệ
            session.FindElementByAccessibilityId("txtSophong")
                   .SendKeys("100");

            // Bỏ trống loại phòng
            var loaiphong = session.FindElementByAccessibilityId("txtLoaiphong");
            loaiphong.Clear();

            

            var cbo1 = session.FindElementByAccessibilityId("txtLoaigiuong");
            cbo1.Click();
            cbo1.SendKeys("Đơn");
            cbo1.SendKeys(OpenQA.Selenium.Keys.Enter);

            // Giá âm → sai dữ liệu
            session.FindElementByAccessibilityId("txtGiatien")
                   .SendKeys("500");
            logSteps.Add("Nhập thông tin phòng(bỏ trống loại phòng");
            // ===== CLICK THÊM =====
            session.FindElementByAccessibilityId("btnAddRoom").Click();
            session.FindElementByAccessibilityId("btnAddRoom").Click();
            logSteps.Add("Nhấn nút thêm phòng");


            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Vui lòng điền đầy đủ thông tin!"))
            );
            Assert.AreEqual(
                "Vui lòng điền đầy đủ thông tin!",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Vui lòng nhập đầy đủ thông tin");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM PHÒNG BỎ TRỐNG DỮ LIỆU", logSteps, "FAIL");

        }

        [TestMethod]
        public void ThemPhong_NhapChuVaoSoPhong_ThatBai()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");

            // ===== NHẬP DỮ LIỆU =====

            // Số phòng nhập chữ → sai
            session.FindElementByAccessibilityId("txtSophong")
                   .SendKeys("ABC");


            logSteps.Add("Nhập thông tin phòng(nhập chữ vào số phòng");


            Thread.Sleep(1500);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt."))
            );
            Assert.AreEqual(
                "Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Chỉ được số. Không cho phép chữ hoặc ký tự đặc biệt");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST THÊM PHÒNG NHẬP CHỮ VÀO SỐ PHÒNG", logSteps, "FAIL");
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
