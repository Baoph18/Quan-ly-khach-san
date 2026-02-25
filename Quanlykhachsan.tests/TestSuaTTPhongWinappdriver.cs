using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
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
            @"D:\Quản lí khách sạn\Quản lí khách sạn\bin\x64\Debug\Quản lí khách sạn.exe";

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
        
        public void ThemPhong_ThanhCong()
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
            var txtSoPhong = session.FindElementByAccessibilityId("txtSophong");
            txtSoPhong.Clear();
            txtSoPhong.SendKeys("1");

            var txtLoaiPhong = session.FindElementByAccessibilityId("txtLoaiphong");
            txtLoaiPhong.Clear();
            txtLoaiPhong.SendKeys("Thường");

            var txtLoaiGiuong = session.FindElementByAccessibilityId("txtLoaigiuong");
            txtLoaiGiuong.Clear();
            txtLoaiGiuong.SendKeys("Đơn");

            var txtGiaTien = session.FindElementByAccessibilityId("txtGiatien");
            txtGiaTien.Clear();
            txtGiaTien.SendKeys("1");

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
