using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
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
            Test_DangNhap_Va_MoFormNhanVien();

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(15));

            // Chờ grid xuất hiện
            wait.Until(d =>
                session.FindElementByAccessibilityId("dataGridView1"));

            Thread.Sleep(800);

            // ---- SỬA DỮ LIỆU ----

            var txtTenNV = session.FindElementByAccessibilityId("txtTenNV");
            txtTenNV.Clear();
            txtTenNV.SendKeys("Duy Tân");

            var txtSDTNV = session.FindElementByAccessibilityId("txtSDTNV");
            txtSDTNV.Clear();
            txtSDTNV.SendKeys("5345645747467");

            var cboGioiTinh = session.FindElementByAccessibilityId("cboGioiTinh");
            cboGioiTinh.Click();
            cboGioiTinh.SendKeys("Nam");

            var txtEmailr = session.FindElementByAccessibilityId("txtEmailr");
            txtEmailr.Clear();
            txtEmailr.SendKeys("duytan@gmail.com");

            // ---- CLICK NÚT SỬA ----

            var btnSua = wait.Until(d =>
                session.FindElementByName("Sửa"));

            

            btnSua.Click();
            btnSua.Click();
            Thread.Sleep(1000);

            // ---- BẮT MESSAGEBOX SUCCESS ----

            foreach (var handle in session.WindowHandles)
            {
                session.SwitchTo().Window(handle);
                try
                {
                    var okBtn = session.FindElementByName("OK");
                    okBtn.Click();
                    break;
                }
                catch { }
            }

            



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
