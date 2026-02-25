using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Quanlykhachsan.tests
{
    [TestClass]
    public class ĐăngNhậpTests
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
        [TestMethod]
        public void Test_DangNhap()
        {
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            Thread.Sleep(3000);


          
         
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
