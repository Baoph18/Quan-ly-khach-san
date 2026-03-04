using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_ThemPhong
    /// </summary>
    [TestClass]
    public class Test_ThemPhong
    {
        private RoomService service;

        [TestInitialize]
        public void KhoiTao()
        {
            service = new RoomService();
        }

        // 1️⃣ Thêm phòng hợp lệ
        [TestMethod]
        public void ThemPhong_HopLe_TraVeTrue()
        {
            Room r = new Room
            {
                SoPhong = "101",
                LoaiPhong = "Deluxe",
                Giuong = "Đôi",
                Gia = 500000
            };

            bool ketQua = service.AddRoom(r);

            Assert.IsTrue(ketQua);
        }

        // 2️⃣ Trùng số phòng
        [TestMethod]
        public void ThemPhong_TrungSoPhong_TraVeFalse()
        {
            Room r1 = new Room
            {
                SoPhong = "101",
                LoaiPhong = "Deluxe",
                Giuong = "Đôi",
                Gia = 500000
            };

            Room r2 = new Room
            {
                SoPhong = "101",
                LoaiPhong = "Standard",
                Giuong = "Đơn",
                Gia = 300000
            };

            service.AddRoom(r1);
            bool ketQua = service.AddRoom(r2);

            Assert.IsFalse(ketQua);
        }

        // 3️⃣ Thiếu số phòng
        [TestMethod]
        public void ThemPhong_ThieuSoPhong_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "",
                LoaiPhong = "Standard",
                Giuong = "Đơn",
                Gia = 200000
            };

            bool ketQua = service.AddRoom(r);

            Assert.IsFalse(ketQua);
        }

        // 4️⃣ Thiếu loại phòng
        [TestMethod]
        public void ThemPhong_ThieuLoaiPhong_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "102",
                LoaiPhong = "",
                Giuong = "Đơn",
                Gia = 200000
            };

            bool ketQua = service.AddRoom(r);

            Assert.IsFalse(ketQua);
        }

        // 5️⃣ Thiếu giường
        [TestMethod]
        public void ThemPhong_ThieuGiuong_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "103",
                LoaiPhong = "Standard",
                Giuong = "",
                Gia = 200000
            };

            bool ketQua = service.AddRoom(r);

            Assert.IsFalse(ketQua);
        }

        // 6️⃣ Giá bằng 0
        [TestMethod]
        public void ThemPhong_GiaBang0_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "104",
                LoaiPhong = "Standard",
                Giuong = "Đơn",
                Gia = 0
            };

            bool ketQua = service.AddRoom(r);

            Assert.IsFalse(ketQua);
        }

        // 7️⃣ Giá âm
        [TestMethod]
        public void ThemPhong_GiaAm_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "105",
                LoaiPhong = "Standard",
                Giuong = "Đơn",
                Gia = -100
            };

            bool ketQua = service.AddRoom(r);

            Assert.IsFalse(ketQua);
        }
    }
}
