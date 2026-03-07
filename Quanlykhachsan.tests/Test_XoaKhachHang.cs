using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;
using System;
using System.Collections.Generic;
using System.Text;
using static Quản_lí_khách_sạn.ksquanli.uc_CustomerDetails;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_XoaKhachHang
    /// </summary>
    [TestClass]
    public class Test_XoaKhachHang
    {
        private Customer service;

        [TestInitialize]
        public void KhoiTao()
        {
            service = new Customer();
        }

        // 1️⃣ Xóa thành công
        [TestMethod]
        public void XoaKhachHang_TonTai_TraVeTrue()
        {
            KhachHangUpdateInfo kh = new KhachHangUpdateInfo
            {
                MaKH = 1,
                Ten = "Nguyen Van A"
            };

            service.AddKhachHang(kh);

            bool ketQua = service.DeleteKhachHang(1);

            Assert.IsTrue(ketQua);
        }

        // 2️⃣ Xóa khách không tồn tại
        [TestMethod]
        public void XoaKhachHang_KhongTonTai_TraVeFalse()
        {
            bool ketQua = service.DeleteKhachHang(99);

            Assert.IsFalse(ketQua);
        }

        // 3️⃣ Xóa với mã không hợp lệ
        [TestMethod]
        public void XoaKhachHang_MaKhongHopLe_TraVeFalse()
        {
            bool ketQua = service.DeleteKhachHang(0);

            Assert.IsFalse(ketQua);
        }
    }
}
