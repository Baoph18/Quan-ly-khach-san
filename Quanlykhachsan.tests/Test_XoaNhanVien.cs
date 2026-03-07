using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_XoaNhanVien
    /// </summary>
    [TestClass]
    public class Test_XoaNhanVien
    {
        private NhanVienServices service;

        [TestInitialize]
        public void Setup()
        {
            service = new NhanVienServices();
        }

        // Hàm tạo nhân viên hợp lệ
        private NhanVien TaoNhanVienHopLe()
        {
            return new NhanVien
            {
                MaNV = 1,
                Ten = "Nguyen Van A",
                SDT = "0901234567",
                GioiTinh = "Nam",
                Email = "test@gmail.com"
            };
        }

        // 1. Xóa hợp lệ
        [TestMethod]
        public void XoaNhanVien_HopLe_ThanhCong()
        {
            var nv = TaoNhanVienHopLe();
            service.AddNhanVien(nv);

            var result = service.XoaNhanVien(1);

            Assert.AreEqual("Xóa thành công", result);
        }

        // 2. Mã <= 0
        [TestMethod]
        public void XoaNhanVien_MaKhongHopLe_TraVeLoi()
        {
            var result = service.XoaNhanVien(0);

            Assert.AreEqual("Mã nhân viên không hợp lệ", result);
        }

        // 3. Không tồn tại
        [TestMethod]
        public void XoaNhanVien_KhongTonTai_TraVeLoi()
        {
            var result = service.XoaNhanVien(999);

            Assert.AreEqual("Nhân viên không tồn tại", result);
        }

        // 4. Xóa 2 lần
        [TestMethod]
        public void XoaNhanVien_XoaHaiLan_TraVeKhongTonTai()
        {
            var nv = TaoNhanVienHopLe();
            service.AddNhanVien(nv);

            service.XoaNhanVien(1);
            var result = service.XoaNhanVien(1);

            Assert.AreEqual("Nhân viên không tồn tại", result);
        }
    }
}
