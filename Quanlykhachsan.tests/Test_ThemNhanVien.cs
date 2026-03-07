using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_ThemNhanVien
    /// </summary>
    [TestClass]
    public class Test_ThemNhanVien
    {
        private NhanVienServices service;

        [TestInitialize]
        public void Setup()
        {
            service = new NhanVienServices();
        }

        // 1. Hợp lệ
        [TestMethod]
        public void AddNhanVien_HopLe_ThanhCong()
        {
            var nv = new NhanVien
            {
                MaNV = 1,
                Ten = "Nguyen Van A",
                SDT = "0901234567",
                GioiTinh = "Nam",
                Email = "test@gmail.com"
            };

            var result = service.AddNhanVien(nv);

            Assert.AreEqual("Thành công", result);
        }

        // 2. Thiếu tên
        [TestMethod]
        public void AddNhanVien_TenRong_TraVeLoi()
        {
            var nv = new NhanVien
            {
                MaNV = 2,
                Ten = "",
                SDT = "0901234567",
                GioiTinh = "Nam",
                Email = "test@gmail.com"
            };

            var result = service.AddNhanVien(nv);

            Assert.AreEqual("Thiếu thông tin", result);
        }

        // 3. SDT sai định dạng
        [TestMethod]
        public void AddNhanVien_SDTSai_TraVeLoi()
        {
            var nv = new NhanVien
            {
                MaNV = 3,
                Ten = "Test",
                SDT = "abc",
                GioiTinh = "Nam",
                Email = "test@gmail.com"
            };

            var result = service.AddNhanVien(nv);

            Assert.AreEqual("SDT không hợp lệ", result);
        }

        // 4. Email sai
        [TestMethod]
        public void AddNhanVien_EmailSai_TraVeLoi()
        {
            var nv = new NhanVien
            {
                MaNV = 4,
                Ten = "Test",
                SDT = "0901234567",
                GioiTinh = "Nam",
                Email = "abc.com"
            };

            var result = service.AddNhanVien(nv);

            Assert.AreEqual("Email không hợp lệ", result);
        }

        // 5. Giới tính sai
        [TestMethod]
        public void AddNhanVien_GioiTinhSai_TraVeLoi()
        {
            var nv = new NhanVien
            {
                MaNV = 5,
                Ten = "Test",
                SDT = "0901234567",
                GioiTinh = "Khac",
                Email = "test@gmail.com"
            };

            var result = service.AddNhanVien(nv);

            Assert.AreEqual("Giới tính không hợp lệ", result);
        }
    }
}
