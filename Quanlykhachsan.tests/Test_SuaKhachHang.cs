using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;
using System;
using System.Collections.Generic;
using System.Text;
using static Quản_lí_khách_sạn.ksquanli.uc_CustomerDetails;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_SuaKhachHang
    /// </summary>
    [TestClass]
    public class Test_SuaKhachHang
    {
        private Customer service;

        [TestInitialize]
        public void Setup()
        {
            service = new Customer();
        }

        // 1️⃣ Sửa thành công
        [TestMethod]
        public void SuaKhachHang_TonTai_TraVeTrue()
        {
            KhachHangUpdateInfo kh = new KhachHangUpdateInfo
            {
                MaKH = 1,
                Ten = "Nguyen Van A"
            };

            service.AddKhachHang(kh);

            KhachHangUpdateInfo khMoi = new KhachHangUpdateInfo
            {
                MaKH = 1,
                Ten = "Nguyen Van B",
                SDT = "0123456789"
            };

            bool ketQua = service.UpdateKhachHang(khMoi);

            Assert.IsTrue(ketQua);
        }

        // 2️⃣ Sửa khách không tồn tại
        [TestMethod]
        public void SuaKhachHang_KhongTonTai_TraVeFalse()
        {
            KhachHangUpdateInfo kh = new KhachHangUpdateInfo
            {
                MaKH = 99,
                Ten = "Khong ton tai"
            };

            bool ketQua = service.UpdateKhachHang(kh);

            Assert.IsFalse(ketQua);
        }

        // 3️⃣ Mã không hợp lệ
        [TestMethod]
        public void SuaKhachHang_MaKhongHopLe_TraVeFalse()
        {
            KhachHangUpdateInfo kh = new KhachHangUpdateInfo
            {
                MaKH = 0,
                Ten = "Sai ma"
            };

            bool ketQua = service.UpdateKhachHang(kh);

            Assert.IsFalse(ketQua);
        }

        [TestMethod]
        public void SuaKhachHang_SDT_ChuaChu_TraVeFalse()
        {
            KhachHangUpdateInfo kh = new KhachHangUpdateInfo
            {
                MaKH = 1,
                Ten = "Nguyen Van A",
                SDT = "0123456789"
            };

            service.AddKhachHang(kh);

            KhachHangUpdateInfo khMoi = new KhachHangUpdateInfo
            {
                MaKH = 1,
                Ten = "Nguyen Van B",
                SDT = "abc123"
            };

            bool ketQua = service.UpdateKhachHang(khMoi);

            Assert.IsFalse(ketQua);
        }
    }
}
