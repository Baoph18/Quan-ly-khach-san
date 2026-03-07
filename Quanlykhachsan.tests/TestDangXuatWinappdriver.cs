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
    /// Summary description for TestDangXuatWinappdriver
    /// </summary>
    [TestClass]
    public class TestDangXuatWinappdriver
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
            string path = @"D:\DangXuat_test.txt";

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
        public void Test_DangNhap_Va_MoFormDangXuat()
        {
            
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();
            

            
            Thread.Sleep(3000);

            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

            session.FindElementByAccessibilityId("btnInformation").Click();
            
            Thread.Sleep(2000);

            handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
        }

        [TestMethod]
        public void DangXuat_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoFormDangXuat();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thông tin cá nhân");
            // Click nút đăng xuất
            session.FindElementByAccessibilityId("btnDangxuat").Click();
            logSteps.Add("Nhấn nút đăng xuất");
            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var message = waitPopup.Until(d =>
                d.FindElement(By.Name("Bạn có chắc muốn đăng xuất?"))
            );
            Assert.AreEqual(
                "Bạn có chắc muốn đăng xuất?",
                message.Text.Trim()
            );
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("Yes"))
            );

            btnOK1.Click();
            logSteps.Add("Hiển thị thông báo:Bạn có muốn đăng xuất không");
            logSteps.Add("Nhấn Yes");
            WriteLogBlock("TEST ĐĂNG XUẤT THÀNH CÔNG", logSteps, "PASS");
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
