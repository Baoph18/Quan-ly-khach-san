using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

using System.Threading;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestSuaTTKhachHangWinappdriver
    /// </summary>
    [TestClass]
    public class TestSuaTTKhachHangWinappdriver
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
            string path = @"D:\SuaTTKhachHang_test.txt";

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

            session.FindElementByAccessibilityId("btnCustomerDetails").Click();
            Thread.Sleep(2000);

            // Switch đúng form phòng
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                if (session.Title.Contains("Thông Tin Khách Hàng"))
                    break;
            }
        }

        [TestMethod]
        public void SuaTTKhachHang_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thông tin khách hàng");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));
            // Chờ DataGrid xuất hiện  WebDriverWait wait = new WebDriv
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1")); // kiểm tra lại AutomationId

            // Click vào grid
            grid.Click();
            logSteps.Add("Chọn khách hàng cần sửa");
            Thread.Sleep(500);
            // Nhập lại thông tin
            var txtSoPhong = session.FindElementByAccessibilityId("txtTENKH");
            txtSoPhong.Clear();
            txtSoPhong.SendKeys("Tâm lê");

            var txtLoaiPhong = session.FindElementByAccessibilityId("txtSDT");
            txtLoaiPhong.Clear();
            txtLoaiPhong.SendKeys("0336708057");

            var txtLoaiGiuong = session.FindElementByAccessibilityId("txtQUOCTICH");
            txtLoaiGiuong.Clear();
            txtLoaiGiuong.SendKeys("Việt Nam");

            var cbo = session.FindElementByAccessibilityId("cboGIOITINH");
            cbo.Click();
            cbo.SendKeys("Nam");
            cbo.SendKeys(OpenQA.Selenium.Keys.Enter);
            

            var txtMaDD = session.FindElementByAccessibilityId("txtMADD");
            txtMaDD.Clear();
            txtMaDD.SendKeys("2711");

            var txtDiaChi = session.FindElementByAccessibilityId("txtDIACHI");
            txtDiaChi.Clear();
            txtDiaChi.SendKeys("72 Bà Rịa Vũng Tàu");

            var txtSoDem = session.FindElementByAccessibilityId("txtSoDem");
            txtSoDem.Clear();
            txtSoDem.SendKeys("3");
            logSteps.Add("Nhập thông tin khách hàng");
            session.FindElementByAccessibilityId("btnRepair").Click();
            session.FindElementByAccessibilityId("btnRepair").Click();
            logSteps.Add("Nhấn nút sửa");
            Thread.Sleep(1500);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Thông tin khách hàng đã được cập nhật!"))
            );
            Assert.AreEqual(
                "Thông tin khách hàng đã được cập nhật!",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiển thị thông báo:Thông tin khách hàng đã được cập nhật");
            btnOK1.Click();
            logSteps.Add("Nhấn Ok");
            WriteLogBlock("TEST SỬA THÔNG TIN KHÁCH HÀNG THÀNH CÔNG", logSteps, "PASS");
        }

        [TestMethod]
        public void SuaTTKhachHang_NhapChuVaoSDT_KhongThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thông tin khách hàng");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            grid.Click();
            logSteps.Add("Chọn khách hàng cần sửa");
            Thread.Sleep(500);



            // ===== NHẬP DATA SAI =====

            var txtTen = session.FindElementByAccessibilityId("txtTENKH");
            txtTen.Clear();
            txtTen.SendKeys("Test Fail");

            var txtSDT = session.FindElementByAccessibilityId("txtSDT");
            txtSDT.Clear();
            txtSDT.SendKeys("abcxyz"); // ❌ sai định dạng số điện thoại

            
            logSteps.Add("Nhập thông tin khách hàng(Nhập chữ vào sdt)");

            
            logSteps.Add("Hiển thị thông báo:Chỉ được số. Không cho phép chữ hoặc ký tự đặc biệt");
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
            btnOK1.Click();
            logSteps.Add("Nhấn ok");


            

            WriteLogBlock("TEST SỬA THÔNG TIN KHÁCH HÀNG NHẬP CHỮ VÀO SDT", logSteps, "FAIL");
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
