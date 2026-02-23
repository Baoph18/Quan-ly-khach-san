using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_DangKyKhachHang
    /// </summary>
    [TestClass]
    public class Test_DangKyKhachHang
    {
        private BookingService service;

        [TestInitialize]
        public void Setup()
        {
            service = new BookingService();
        }

        private KhachHang TaoKhachHopLe()
        {
            return new KhachHang
            {
                Ten = "Nguyen Van A",
                SDT = 9123456789, // sửa ở đây
                QuocTich = "Viet Nam",
                GioiTinh = "Nam",
                MaDD = "123456789012",
                DiaChi = "Ha Noi",
                NgayCheckin = DateTime.Now.Date,
                SoPhong = "101",
                SoDem = "2",
                MaPhong = 1
            };
        }

        // 1. Hợp lệ
        [TestMethod]
        public void DangKy_HopLe_ThanhCong()
        {
            Assert.AreEqual("Đăng ký thành công", service.DangKy(TaoKhachHopLe()));
        }

        // 2. Tên rỗng
        [TestMethod]
        public void DangKy_TenRong_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.Ten = "";
            Assert.AreEqual("Tên không hợp lệ", service.DangKy(kh));
        }

        // 3. SDT sai
        [TestMethod]
        public void DangKy_SDTSai_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.SDT = 123;
            Assert.AreEqual("Số điện thoại không hợp lệ", service.DangKy(kh));
        }

        // 4. Quốc tịch rỗng
        [TestMethod]
        public void DangKy_QuocTichRong_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.QuocTich = "";
            Assert.AreEqual("Quốc tịch không hợp lệ", service.DangKy(kh));
        }

        // 5. Giới tính sai
        [TestMethod]
        public void DangKy_GioiTinhSai_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.GioiTinh = "Khac";
            Assert.AreEqual("Giới tính không hợp lệ", service.DangKy(kh));
        }

        // 6. Ngày checkin quá khứ
        [TestMethod]
        public void DangKy_NgayQuaKhu_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.NgayCheckin = DateTime.Now.AddDays(-1);
            Assert.AreEqual("Ngày checkin không hợp lệ", service.DangKy(kh));
        }

        // 7. Số đêm = 0
        [TestMethod]
        public void DangKy_SoDemBang0_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.SoDem = "0";
            Assert.AreEqual("Số đêm không hợp lệ", service.DangKy(kh));
        }

        // 8. Mã phòng không hợp lệ
        [TestMethod]
        public void DangKy_MaPhongKhongHopLe_TraVeLoi()
        {
            var kh = TaoKhachHopLe();
            kh.MaPhong = 0;
            Assert.AreEqual("Mã phòng không hợp lệ", service.DangKy(kh));
        }
    }
}
