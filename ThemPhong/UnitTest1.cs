using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quản_lí_khách_sạn;

namespace ThemPhong
{
    [TestClass]
    public class RoomServiceTests
    {
        private RoomServices roomService;

        [TestInitialize]
        public void Setup()
        {
            roomService = new RoomServices();
            roomService.IsTesting = true;
        }

        [TestMethod]
        public void ThemPhong_Test()
        {
            var room = new Room
            {
                SoPhong = "302",
                LoaiPhong = "VIP",
                Giuong = "Đôi",
                Gia = 1000000,
                TrangThai = "Trống"
            };

            bool result = roomService.AddRoom(room);

            Assert.IsTrue(result, "Phòng hợp lệ phải được thêm thành công.");
        }
    }
}
