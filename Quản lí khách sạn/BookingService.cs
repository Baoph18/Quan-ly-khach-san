using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class KhachHang
    {
        public string Ten { get; set; }
        public long SDT { get; set; }
        public string QuocTich { get; set; }
        public string GioiTinh { get; set; }
        public string MaDD { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayCheckin { get; set; }
        public string SoPhong { get; set; }
        public string SoDem { get; set; }
        public int MaPhong { get; set; }
    }
    public class BookingService
    {
        public string DangKy(KhachHang kh)
        {
            if (string.IsNullOrWhiteSpace(kh.Ten))
                return "Tên không hợp lệ";

            if (kh.SDT < 1000000000 || kh.SDT > 9999999999)
                return "Số điện thoại không hợp lệ";

            if (string.IsNullOrWhiteSpace(kh.QuocTich))
                return "Quốc tịch không hợp lệ";

            if (kh.GioiTinh != "Nam" && kh.GioiTinh != "Nữ")
                return "Giới tính không hợp lệ";

            if (string.IsNullOrWhiteSpace(kh.MaDD))
                return "Mã định danh không hợp lệ";

            if (kh.NgayCheckin < DateTime.Now.Date)
                return "Ngày checkin không hợp lệ";

            if (string.IsNullOrWhiteSpace(kh.SoPhong))
                return "Số phòng không hợp lệ";

            if (!int.TryParse(kh.SoDem, out int soDem) || soDem <= 0)
                return "Số đêm không hợp lệ";

            if (kh.MaPhong <= 0)
                return "Mã phòng không hợp lệ";

            return "Đăng ký thành công";
        }
    }
}
