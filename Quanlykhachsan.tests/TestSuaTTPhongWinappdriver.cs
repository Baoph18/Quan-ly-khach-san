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
    /// Summary description for TestSuaTTPhongWinappdriver
    /// </summary>
    [TestClass]
    public class TestSuaTTPhongWinappdriver
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
            Thread.Sleep(3000);
        }
        private void WriteLogBlock(string testName, List<string> steps, string result)
        {
            string path = @"D:\SuaTTPhong_test.txt";

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

            // Switch đúng window chính sau login
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                if (session.Title.Contains("Quản"))
                    break;
            }

            session.FindElementByAccessibilityId("btnthphong").Click();
            Thread.Sleep(2000);

            // Switch đúng form phòng
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                if (session.Title.Contains("Phòng"))
                    break;
            }
        }

        [TestMethod]
        
        public void SuaPhong_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");
            
            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1")); // kiểm tra lại AutomationId
            logSteps.Add("Chọn phòng cần sửa");
            // Click vào grid
            grid.Click();
            Thread.Sleep(500);

            // Nhập lại thông tin
            var txtSoPhong = session.FindElementByAccessibilityId("txtSophong");
            txtSoPhong.Clear();
            txtSoPhong.SendKeys("1");

            var cbo = session.FindElementByAccessibilityId("txtLoaiphong");
            cbo.Click();
            cbo.SendKeys("Thường");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);

            var cbo1 = session.FindElementByAccessibilityId("txtLoaigiuong");
            cbo1.Click();
            cbo1.SendKeys("Đơn");
            cbo1.SendKeys(OpenQA.Selenium.Keys.Enter);

            var txtGiaTien = session.FindElementByAccessibilityId("txtGiatien");
            txtGiaTien.Clear();
            txtGiaTien.SendKeys("1");
            logSteps.Add("Nhập thông tin phòng");

            session.FindElementByAccessibilityId("btnRepair").Click();
            session.FindElementByAccessibilityId("btnRepair").Click();
            logSteps.Add("Nhấn nút sửa");

            Thread.Sleep(1500);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Cập nhật thông tin phòng thành công!"))
            );
            Assert.AreEqual(
                "Cập nhật thông tin phòng thành công!",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Cập nhật thông tin phòng thành công");

            btnOK1.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST SỬA THÔNG TIN PHÒNG THÀNH CÔNG", logSteps, "PASS");
        }


        [TestMethod]
        public void SuaPhong_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));
            logSteps.Add("Chọn phòng cần sửa");
            grid.Click();
            Thread.Sleep(500);

            // Nhập giá âm (trigger validate nếu có sự kiện TextChanged/Leave)
            var txtGiaTien = session.FindElementByAccessibilityId("txtGiatien");
            txtGiaTien.Clear();
            txtGiaTien.SendKeys("-100");
            logSteps.Add("Nhập số tiền âm");
            // Click ra ngoài để kích hoạt validation
            grid.Click();
            
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
            WriteLogBlock("TEST SỬA THÔNG TIN PHÒNG NHẬP SỐ TIỀN ÂM", logSteps, "FAIL");
        }


        [TestMethod]
        public void SuaPhong_BoTrongDuLieu_ThatBai()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));
            logSteps.Add("Chọn phòng cần sửa");
            grid.Click();

            // Xóa dữ liệu
            session.FindElementByAccessibilityId("txtSophong").Clear();
            session.FindElementByAccessibilityId("txtLoaiphong").Clear();
            session.FindElementByAccessibilityId("txtLoaigiuong").Clear();
            session.FindElementByAccessibilityId("txtGiatien").Clear();
            logSteps.Add("Xóa hết dữ liệu ô nhập");
            // Click sửa
            session.FindElementByAccessibilityId("btnRepair").Click();
            logSteps.Add("Nhấn nút Sửa");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Vui lòng nhập đầy đủ thông tin");
            btnOK1.Click();
            logSteps.Add("Nhấn ok");

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Dữ liệu nhập không hợp lệ."))
            );
            Assert.AreEqual(
                "Dữ liệu nhập không hợp lệ.",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK2 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Dữ liệu nhập không hợp lệ");
            btnOK2.Click();
            logSteps.Add("Nhấn ok");
            WriteLogBlock("TEST SỬA THÔNG TIN PHÒNG BỎ TRỐNG DỮ LIỆU", logSteps, "FAIL");
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
