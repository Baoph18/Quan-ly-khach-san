using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class HoaDon
    {
        public int MaKhachHang { get; set; }
        public int MaNhanVien { get; set; }
        public string SoPhong { get; set; }
        public string PhuongThuc { get; set; }
        public DateTime NgayCheckout { get; set; }
        public decimal TongTien { get; set; }
    }
    public class PaymentService
    {
        

        public string ThanhToan(HoaDon hd)
        {
            if (hd.MaKhachHang <= 0)
                return "Khách hàng không hợp lệ";

            if (hd.MaNhanVien <= 0)
                return "Nhân viên không hợp lệ";

            if (string.IsNullOrWhiteSpace(hd.SoPhong))
                return "Số phòng không hợp lệ";

            if (string.IsNullOrWhiteSpace(hd.PhuongThuc))
                return "Phương thức thanh toán không hợp lệ";

            if (hd.NgayCheckout < DateTime.Now.Date)
                return "Ngày checkout không hợp lệ";

            if (hd.TongTien <= 0)
                return "Tổng tiền không hợp lệ";

            return "Thanh toán thành công";
        }
    }
}
