using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quản_lí_khách_sạn
{
    [TestClass]
    public class Test_Chucnang 
    {
        private AccountService service;

        [TestInitialize]
        public void KhoiTao()
        {
            service = new AccountService();
        }

        // 1️⃣ Đăng nhập thành công
        [TestMethod]
        public void DangNhap_HopLe_TraVeThanhCong()
        {
            var ketQua = service.DangNhap("admin", "123456");

            Assert.AreEqual("Thành công", ketQua);
        }

        // 2️⃣ Sai mật khẩu
        [TestMethod]
        public void DangNhap_SaiMatKhau_TraVeSaiMatKhau()
        {
            var ketQua = service.DangNhap("admin", "abc");

            Assert.AreEqual("Sai mật khẩu", ketQua);
        }

        // 3️⃣ Tài khoản không tồn tại
        [TestMethod]
        public void DangNhap_TaiKhoanKhongTonTai_TraVeLoi()
        {
            var ketQua = service.DangNhap("abcxyz", "123456");

            Assert.AreEqual("Tài khoản không tồn tại", ketQua);
        }

        // 4️⃣ Thiếu tên đăng nhập
        [TestMethod]
        public void DangNhap_ThieuTenDangNhap_TraVeLoi()
        {
            var ketQua = service.DangNhap("", "123456");

            Assert.AreEqual("Thiếu thông tin", ketQua);
        }

        // 5️⃣ Thiếu mật khẩu
        [TestMethod]
        public void DangNhap_ThieuMatKhau_TraVeLoi()
        {
            var ketQua = service.DangNhap("admin", "");

            Assert.AreEqual("Thiếu thông tin", ketQua);
        }

        [TestMethod]
        public void DangNhap_KhongNhapDuLieu_TraVeLoi()
        {
            var ketQua = service.DangNhap("", "");

            Assert.AreEqual("Thiếu thông tin", ketQua);
        }
    }
}
