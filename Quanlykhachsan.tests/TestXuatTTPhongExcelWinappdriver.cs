using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestXuatTTPhongExcelWinappdriver
    /// </summary>
    [TestClass]
    public class TestXuatTTPhongExcelWinappdriver
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
            string path = @"D:\Lưuexcel_test.txt";

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
        public void LuuPhongExcel_ThanhCong()
        {
            var logSteps = new List<string>();
            Test_DangNhap_Va_MoForm();
            logSteps.Add("Đăng nhập thành công");
            logSteps.Add("Mở form Thêm phòng");
            

            // Bấm đăng ký
            session.FindElementByAccessibilityId("btnexporttoexel").Click();
            Thread.Sleep(3000);
            logSteps.Add("Nhấn nút Lưu file excel");

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");

            var rootSession = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723"),
                options
            );
            WebDriverWait wait = new WebDriverWait(rootSession, TimeSpan.FromSeconds(10));

            var saveWindow = wait.Until(d =>
                d.FindElement(By.Name("Save As"))
            );

            var fileNameBox = session.FindElementByAccessibilityId("1001");
            fileNameBox.Clear();
            fileNameBox.SendKeys(@"D:\DanhSachPhong.xlsx");

            var btnSave = saveWindow.FindElement(By.Name("Save"));
            btnSave.Click();

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));
            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());

           
            var msgBox = waitPopup.Until(d =>
    d.FindElement(By.ClassName("#32770"))
);
            logSteps.Add("Hiển thị thông báo:Lưu file thành công");
            var btnOK = msgBox.FindElement(By.Name("OK"));
            btnOK.Click();
            logSteps.Add("Nhấn ok");
            
            WriteLogBlock("TEST LƯU THÔNG TIN PHONG VÀO EXCEL THÀNH CÔNG", logSteps, "PASS");
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
