using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quản_lí_khách_sạn
{
    [TestClass]
    public class Test_Chucnang 
    {
        [TestMethod]
        public void Test_Dangnhap()
        {
            // Arrange
            var service = new AccountService();
            string username = "b";
            string password = "123";

            // Act
            bool result = service.Login(username, password);

            // Assert
            Assert.IsTrue(result,"đăng nhập thành công");
        }

        [TestMethod]
        public void Test_Suaphong()
        {
            // Arrange
            var roomService = new RoomService();
            int maPhong = 1; 
            string soPhongMoi = "101";
            string loaiPhongMoi = "VIP";
            string giuongMoi = "Giường đôi";
            long giaMoi = 800000;

            // Act
            bool result = roomService.SuaPhong(maPhong, soPhongMoi, loaiPhongMoi, giuongMoi, giaMoi);

            // Assert
            Assert.IsTrue(result, "Sửa phòng thành công");
        }

        [TestMethod]
        public void Test_Thanhtoan()
        {
            // Arrange
            var payment = new PaymentService();
            int maHoaDon = 1;
            decimal tongTien = 500000;
            string phuongThuc = "Tiền mặt";

            // Act
            bool result = payment.ThanhToan(maHoaDon, tongTien, phuongThuc);

            // Assert
            Assert.IsTrue(result, "Thanh toán hợp lệ → thành công");
        }

        [TestMethod]
        public void Test_Datphong()
        {
            // Arrange
            var booking = new BookingService();
            int maPhong = 1;
            int maKhach = 5;
            int soDem = 3;
            string ngayNhan = "2025-11-10";
            string ngayTra = "2025-11-13";

            // Act
            bool result = booking.DatPhong(maPhong, maKhach, soDem, ngayNhan, ngayTra);

            // Assert
            Assert.IsTrue(result, "Đặt phòng hợp lệ → thành công");
        }
    }
}
