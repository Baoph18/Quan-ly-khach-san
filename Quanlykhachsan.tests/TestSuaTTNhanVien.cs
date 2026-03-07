using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for TestSuaTTNhanVien
    /// </summary>
    [TestClass]
    public class TestSuaTTNhanVien
    {
        private NhanVienServices service;

        [TestInitialize]
        public void Setup()
        {
            service = new NhanVienServices();
        }

        private NhanVien TaoNhanVienHopLe()
        {
            return new NhanVien
            {
                MaNV = 1,
                Ten = "Tran Van B",
                SDT = "0912345678",
                GioiTinh = "Nam",
                Email = "test@gmail.com"
            };
        }

        // 1. Hợp lệ
        [TestMethod]
        public void SuaNhanVien_HopLe_ThanhCong()
        {
            Assert.AreEqual("Sửa thành công", service.SuaNhanVien(TaoNhanVienHopLe()));
        }

        // 2. Mã NV sai
        [TestMethod]
        public void SuaNhanVien_MaSai_TraVeLoi()
        {
            var nv = TaoNhanVienHopLe();
            nv.MaNV = 0;

            Assert.AreEqual("Mã nhân viên không hợp lệ", service.SuaNhanVien(nv));
        }

        // 3. Tên rỗng
        [TestMethod]
        public void SuaNhanVien_TenRong_TraVeLoi()
        {
            var nv = TaoNhanVienHopLe();
            nv.Ten = "";

            Assert.AreEqual("Tên không hợp lệ", service.SuaNhanVien(nv));
        }

        // 4. SĐT sai
        [TestMethod]
        public void SuaNhanVien_SDTSai_TraVeLoi()
        {
            var nv = TaoNhanVienHopLe();
            nv.SDT = "123";

            Assert.AreEqual("SDT không hợp lệ", service.SuaNhanVien(nv));
        }

        // 5. Giới tính sai
        [TestMethod]
        public void SuaNhanVien_GioiTinhSai_TraVeLoi()
        {
            var nv = TaoNhanVienHopLe();
            nv.GioiTinh = "Khac";

            Assert.AreEqual("Giới tính không hợp lệ", service.SuaNhanVien(nv));
        }

        // 6. Email sai
        [TestMethod]
        public void SuaNhanVien_EmailSai_TraVeLoi()
        {
            var nv = TaoNhanVienHopLe();
            nv.Email = "abc.com";

            Assert.AreEqual("Email không hợp lệ", service.SuaNhanVien(nv));
        }
    }
}
