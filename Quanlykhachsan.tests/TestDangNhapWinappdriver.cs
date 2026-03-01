using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    [TestClass]
    public class ĐăngNhậpTests
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
            string path = @"D:\DangNhap_test.txt";

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

        [TestMethod]
        public void Test_DangNhap_ThanhCong()
        {
            var logSteps = new List<string>();

            logSteps.Add("Mở form đăng nhập");
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            logSteps.Add("Nhập username(b) và password(123)");
            
            session.FindElementByAccessibilityId("btnLogin").Click();
            logSteps.Add("Nhấn nút đăng nhập");
            // Đợi form Trang Chủ load (có thể lâu hơn tùy kết nối DB)
            Thread.Sleep(3000);

            // switch sang window mới
            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
            // Kiểm tra: Cửa sổ Trang Chủ xuất hiện
            // TODO: Sửa "Trang Chủ" thành tiêu đề (Text) thực tế của form TrangChủ
            logSteps.Add("Nhấn nút đăng nhập vô trang chủ");
            var mainForm = session.FindElementByName("TrangChủ");
            WriteLogBlock("TEST ĐĂNG NHẬP THÀNH CÔNG", logSteps, "PASS");
            Assert.IsNotNull(mainForm);
           
        }


        [TestMethod]
        public void Test_DangNhapSaiThongTin_HienThongBaoLoi()
        {
            var logSteps = new List<string>();

            logSteps.Add("Mở form đăng nhập");
            session.FindElementByAccessibilityId("txtUserName").Clear();
            session.FindElementByAccessibilityId("txtUserName").SendKeys("saiuser");

            session.FindElementByAccessibilityId("txtPassword").Clear();
            session.FindElementByAccessibilityId("txtPassword").SendKeys("saimatkhau");
            logSteps.Add("Nhập username(saiuser) và password(saimatkhau)");

            session.FindElementByAccessibilityId("btnLogin").Click();
            logSteps.Add("Nhấn nút đăng nhập");

            WebDriverWait wait = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            var errorMessage = wait.Until(d =>
    session.FindElementByAccessibilityId("LabelError")
);
            logSteps.Add("Hiển thị lỗi:Bạn đăng nhập không đúng hoặc mật khẩu sai");
            Assert.IsTrue(errorMessage.Displayed);

            Assert.AreEqual(
                "Bạn đăng nhập không đúng hoặc mật khẩu sai",
                errorMessage.Text.Trim()
            );
            
            WriteLogBlock("TEST ĐĂNG NHẬP SAI THÔNG TIN", logSteps, "FAIL");

        }

        [TestMethod]
        public void Test_DangNhap_KhongNhapMatKhau_HienThongBaoTrenButton()
        {
            var logSteps = new List<string>();

            logSteps.Add("Mở form đăng nhập");
            // nhập username
            session.FindElementByAccessibilityId("txtUserName").Clear();
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");

            // không nhập password
            session.FindElementByAccessibilityId("txtPassword").Clear();
            logSteps.Add("Nhập username(b) và password()");

            session.FindElementByAccessibilityId("btnLogin").Click();
            session.FindElementByAccessibilityId("btnLogin").Click();
            logSteps.Add("Nhấn nút đăng nhập");

            // đợi UI cập nhật text
            Thread.Sleep(1500);

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));
            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );
            logSteps.Add("Hiện thông báo lỗi:Vui lòng nhập đầy đủ thông tin");
            logSteps.Add("Nhấn Ok");
            btnOK1.Click();
            WriteLogBlock("TEST ĐĂNG NHẬP KHÔNG NHẬP MẬT KHẨU", logSteps, "FAIL");
        }

        [TestMethod]
        public void DangNhap_BoTrongUserVaPass_ThatBai()
        {
            var logSteps = new List<string>();

            logSteps.Add("Mở form đăng nhập");
            // Không nhập gì cả
            logSteps.Add("Không nhập username và password");
            // Click nút đăng nhập
            session.FindElementByAccessibilityId("btnLogin").Click();
            logSteps.Add("Nhấn nút đăng nhập");
            Thread.Sleep(1500);


            

            WebDriverWait waitPopup = new WebDriverWait(session, TimeSpan.FromSeconds(10));

            // ===== POPUP 1 =====
            var btnOK1 = waitPopup.Until(d =>
                d.FindElement(By.Name("OK"))
            );

            btnOK1.Click();
            logSteps.Add("Hiện thông báo lỗi:Vui lòng nhập đầy đủ thông tin");
            logSteps.Add("Nhấn Ok");
            WriteLogBlock("TEST ĐĂNG NHẬP KHÔNG NHẬP USER VÀ PASS", logSteps, "FAIL");
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

//        [TestInitialize]
//        public void ClearFields()
//        {
//            // Xóa dữ liệu cũ trước mỗi test case
//            session.FindElementByAccessibilityId("txtUserName").Clear();
//            session.FindElementByAccessibilityId("txtPassword").Clear();
//        }

//        //[TestMethod]
//        //public void Test_BoTrongThongTin_HienThiCanhBao()
//        //{
//        //    // Hành động: Bấm nút đăng nhập mà không nhập gì
//        //    session.FindElementByAccessibilityId("btnLogin").Click();
//        //    Thread.Sleep(500); // Đợi MessageBox hiện lên

//        //    // Kiểm tra: MessageBox "Cảnh báo" có xuất hiện không
//        //    var warningDialog = session.FindElementByName("Cảnh báo");
//        //    Assert.IsNotNull(warningDialog);

//        //    // Bấm OK để đóng MessageBox
//        //    session.FindElementByName("OK").Click();
//        //}

//        //[TestMethod]
//        //public void Test_DangNhapSai_HienThiLabelError()
//        //{
//        //    // Hành động: Nhập sai tài khoản và mật khẩu
//        //    session.FindElementByAccessibilityId("txtUserName").SendKeys("saikhoan");
//        //    session.FindElementByAccessibilityId("txtPassword").SendKeys("saimatkhau");
//        //    session.FindElementByAccessibilityId("btnLogin").Click();
//        //    Thread.Sleep(500);

//        //    // Kiểm tra: LabelError hiển thị (nếu Label có text là "Tên đăng nhập hoặc mật khẩu không đúng", bạn có thể dùng FindElementByName)
//        //    var labelError = session.FindElementByAccessibilityId("LabelError");
//        //    Assert.IsTrue(labelError.Displayed);
//        //}


//    }
//}
