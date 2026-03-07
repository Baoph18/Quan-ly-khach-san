using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quanlykhachsan.tests
{
    /// <summary>
    /// Summary description for Test_SuaPhong
    /// </summary>
    [TestClass]
    public class Test_SuaPhong
    {
        private RoomService service;

        [TestInitialize]
        public void KhoiTao()
        {
            service = new RoomService();
        }
        // 1️⃣ Sửa phòng hợp lệ
        [TestMethod]
        public void SuaPhong_HopLe_TraVeTrue()
        {
            Room r = new Room
            {
                SoPhong = "201",
                LoaiPhong = "Standard",
                Giuong = "Đơn",
                Gia = 200000
            };

            service.AddRoom(r);

            Room rMoi = new Room
            {
                SoPhong = "201",
                LoaiPhong = "Deluxe",
                Giuong = "Đôi",
                Gia = 500000,
                TrangThai = "Đã đặt"
            };

            bool ketQua = service.UpdateRoom(rMoi);

            Assert.IsTrue(ketQua);
        }

        // 2️⃣ Sửa phòng không tồn tại
        [TestMethod]
        public void SuaPhong_KhongTonTai_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "999",
                LoaiPhong = "Deluxe",
                Giuong = "Đôi",
                Gia = 500000
            };

            bool ketQua = service.UpdateRoom(r);

            Assert.IsFalse(ketQua);
        }

        // 3️⃣ Sửa với dữ liệu không hợp lệ
        [TestMethod]
        public void SuaPhong_GiaAm_TraVeFalse()
        {
            Room r = new Room
            {
                SoPhong = "202",
                LoaiPhong = "Standard",
                Giuong = "Đơn",
                Gia = 200000
            };

            service.AddRoom(r);

            Room rMoi = new Room
            {
                SoPhong = "202",
                LoaiPhong = "VIP",
                Giuong = "Đôi",
                Gia = -100
            };

            bool ketQua = service.UpdateRoom(rMoi);

            Assert.IsFalse(ketQua);
        }
    }
}
