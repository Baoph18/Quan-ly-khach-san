using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using System.Threading;

namespace Quanlykhachsan.tests
{
    [TestClass]
    public class ĐăngNhậpTests
    {
        // Đường dẫn mặc định của WinAppDriver
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        // TODO: Thay thế bằng đường dẫn tuyệt đối đến file .exe của phần mềm khách sạn của bạn
        private const string AppId = @"D:\Quản lí khách sạn\Quản lí khách sạn\bin\x64\Debug\Quản lí khách sạn.exe";

        private static WindowsDriver<WindowsElement> session;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Khởi tạo session mở ứng dụng
            if (session == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", AppId);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

                Assert.IsNotNull(session, "Không thể khởi tạo session WinAppDriver.");

                // Đợi ứng dụng load lên
                Thread.Sleep(2000);
            }
        }

        [TestMethod]
        public void Test_DangNhapTaiKhoanDuPhong_ThanhCong()
        {
            // Hành động: Nhập tài khoản admin dự phòng
            session.FindElementByAccessibilityId("txtUserName").SendKeys("b");
            session.FindElementByAccessibilityId("txtPassword").SendKeys("123");
            session.FindElementByAccessibilityId("btnLogin").Click();

            // Đợi form Trang Chủ load (có thể lâu hơn tùy kết nối DB)
            Thread.Sleep(5000);



            // switch sang window mới
            var handles = session.WindowHandles;
            session.SwitchTo().Window(handles.Last());
            // Kiểm tra: Cửa sổ Trang Chủ xuất hiện
            // TODO: Sửa "Trang Chủ" thành tiêu đề (Text) thực tế của form TrangChủ
            var mainForm = session.FindElementByName("TrangChủ");
            Assert.IsNotNull(mainForm);
        }
        [ClassCleanup]
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
