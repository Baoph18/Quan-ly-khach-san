using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
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
            Thread.Sleep(3000);
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
            Test_DangNhap_Va_MoForm();

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1")); // kiểm tra lại AutomationId

            // Click vào grid
            grid.Click();
            Thread.Sleep(500);

            

            


            // Nhập lại thông tin
            var txtSoPhong = session.FindElementByAccessibilityId("txtTENKH");
            txtSoPhong.Clear();
            txtSoPhong.SendKeys("Tam lo");

            var txtLoaiPhong = session.FindElementByAccessibilityId("txtSDT");
            txtLoaiPhong.Clear();
            txtLoaiPhong.SendKeys("363636363636");

            var txtLoaiGiuong = session.FindElementByAccessibilityId("txtQUOCTICH");
            txtLoaiGiuong.Clear();
            txtLoaiGiuong.SendKeys("Rau Má");

            var txtGiaTien = session.FindElementByAccessibilityId("cboGIOITINH");
            txtGiaTien.Clear();
            txtGiaTien.SendKeys("Nữ");

            var txtMaDD = session.FindElementByAccessibilityId("txtMADD");
            txtMaDD.Clear();
            txtMaDD.SendKeys("3636");

            var txtDiaChi = session.FindElementByAccessibilityId("txtDIACHI");
            txtDiaChi.Clear();
            txtDiaChi.SendKeys("36 Thanh Hóa");

            var txtSoDem = session.FindElementByAccessibilityId("txtSoDem");
            txtSoDem.Clear();
            txtSoDem.SendKeys("3");

            session.FindElementByAccessibilityId("btnRepair").Click();
            session.FindElementByAccessibilityId("btnRepair").Click();
            Thread.Sleep(1500);

            // Bắt dialog OK
            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                if (session.PageSource.Contains("OK"))
                    break;
            }

            session.FindElementByName("OK").Click();
        }

        [TestMethod]
        public void SuaTTKhachHang_KhongThanhCong()
        {
            Test_DangNhap_Va_MoForm();

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ DataGrid xuất hiện
            var grid = wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            grid.Click();
            Thread.Sleep(500);



            // ===== NHẬP DATA SAI =====

            var txtTen = session.FindElementByAccessibilityId("txtTENKH");
            txtTen.Clear();
            txtTen.SendKeys("Test Fail");

            var txtSDT = session.FindElementByAccessibilityId("txtSDT");
            txtSDT.Clear();
            txtSDT.SendKeys("abcxyz"); // ❌ sai định dạng số điện thoại

            var txtQT = session.FindElementByAccessibilityId("txtQUOCTICH");
            txtQT.Clear();
            txtQT.SendKeys("VN");

            var cboGT = session.FindElementByAccessibilityId("cboGIOITINH");
            cboGT.Clear();
            cboGT.SendKeys("Nam");

            var txtMaDD = session.FindElementByAccessibilityId("txtMADD");
            txtMaDD.Clear();
            txtMaDD.SendKeys("111");

            var txtDC = session.FindElementByAccessibilityId("txtDIACHI");
            txtDC.Clear();
            txtDC.SendKeys("HCM");

            var txtSoDem = session.FindElementByAccessibilityId("txtSoDem");
            txtSoDem.Clear();
            txtSoDem.SendKeys("2");



            // ===== CLICK SỬA =====
            session.FindElementByAccessibilityId("btnRepair").Click();
            Thread.Sleep(1500);



            // ===== VERIFY KHÔNG THÀNH CÔNG =====

            bool errorFound = false;

            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);

                if (session.PageSource.Contains("không hợp lệ") ||
                    session.PageSource.Contains("lỗi") ||
                    session.PageSource.Contains("sai"))
                {
                    errorFound = true;
                    break;
                }
            }




            // Form vẫn còn mở → chứng tỏ chưa lưu
            var stillOpen = session.FindElementByAccessibilityId("txtTENKH");
            Assert.IsTrue(stillOpen.Displayed);
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
