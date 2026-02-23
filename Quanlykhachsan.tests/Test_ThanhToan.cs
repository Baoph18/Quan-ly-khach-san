using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;
using System;

namespace Quanlykhachsan.tests
{
    [TestClass]
    public class Test_ThanhToan
    {
        private PaymentService service;

        [TestInitialize]
        public void Setup()
        {
            service = new PaymentService();
        }

        // 1. Hợp lệ
        [TestMethod]
        public void ThanhToan_HopLe_TraVeThanhCong()
        {
            var tt = TaoHoaDonHopLe();
            Assert.AreEqual("Thanh toán thành công", service.ThanhToan(tt));
        }

        // 2. MaKhachHang = 0
        [TestMethod]
        public void ThanhToan_MaKhachHangBang0_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.MaKhachHang = 0;
            Assert.AreEqual("Khách hàng không hợp lệ", service.ThanhToan(tt));
        }

        // 3. MaNhanVien = 0
        [TestMethod]
        public void ThanhToan_MaNhanVienBang0_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.MaNhanVien = 0;
            Assert.AreEqual("Nhân viên không hợp lệ", service.ThanhToan(tt));
        }

        // 4. SoPhong rỗng
        [TestMethod]
        public void ThanhToan_SoPhongRong_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.SoPhong = "";
            Assert.AreEqual("Số phòng không hợp lệ", service.ThanhToan(tt));
        }

        // 5. PhuongThuc rỗng
        [TestMethod]
        public void ThanhToan_PhuongThucRong_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.PhuongThuc = "";
            Assert.AreEqual("Phương thức thanh toán không hợp lệ", service.ThanhToan(tt));
        }

        // 6. TongTien = 0
        [TestMethod]
        public void ThanhToan_TongTienBang0_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.TongTien = 0;
            Assert.AreEqual("Tổng tiền không hợp lệ", service.ThanhToan(tt));
        }

        // 7. TongTien âm
        [TestMethod]
        public void ThanhToan_TongTienAm_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.TongTien = -500000;
            Assert.AreEqual("Tổng tiền không hợp lệ", service.ThanhToan(tt));
        }

        // 8. Ngày checkout quá khứ
        [TestMethod]
        public void ThanhToan_NgayQuaKhu_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.NgayCheckout = DateTime.Now.AddDays(-1);
            Assert.AreEqual("Ngày checkout không hợp lệ", service.ThanhToan(tt));
        }

        // 9. Ngày checkout hôm nay
        [TestMethod]
        public void ThanhToan_NgayHomNay_HopLe()
        {
            var tt = TaoHoaDonHopLe();
            tt.NgayCheckout = DateTime.Now.Date;
            Assert.AreEqual("Thanh toán thành công", service.ThanhToan(tt));
        }

        // 10. Ngày checkout tương lai
        [TestMethod]
        public void ThanhToan_NgayTuongLai_HopLe()
        {
            var tt = TaoHoaDonHopLe();
            tt.NgayCheckout = DateTime.Now.AddDays(2);
            Assert.AreEqual("Thanh toán thành công", service.ThanhToan(tt));
        }

        // 11. TongTien = 1 (Boundary)
        [TestMethod]
        public void ThanhToan_TongTienBang1_HopLe()
        {
            var tt = TaoHoaDonHopLe();
            tt.TongTien = 1;
            Assert.AreEqual("Thanh toán thành công", service.ThanhToan(tt));
        }

        // 12. SoPhong null
        [TestMethod]
        public void ThanhToan_SoPhongNull_TraVeLoi()
        {
            var tt = TaoHoaDonHopLe();
            tt.SoPhong = null;
            Assert.AreEqual("Số phòng không hợp lệ", service.ThanhToan(tt));
        }

        // Hàm tạo dữ liệu hợp lệ
        private HoaDon TaoHoaDonHopLe()
        {
            return new HoaDon
            {
                MaKhachHang = 1,
                MaNhanVien = 1,
                SoPhong = "101",
                PhuongThuc = "Tiền mặt",
                NgayCheckout = DateTime.Now.Date,
                TongTien = 1000000
            };
        }
    }
}
