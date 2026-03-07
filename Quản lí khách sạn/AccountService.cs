using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class AccountService
    {
        // Giả lập dữ liệu có sẵn
        private Dictionary<string, string> danhSachTaiKhoan =
            new Dictionary<string, string>()
            {
            { "admin", "123456" },
            { "nhanvien", "111111" }
            };

        public string DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) ||
                string.IsNullOrWhiteSpace(matKhau))
                return "Thiếu thông tin";

            if (!danhSachTaiKhoan.ContainsKey(tenDangNhap))
                return "Tài khoản không tồn tại";

            if (danhSachTaiKhoan[tenDangNhap] != matKhau)
                return "Sai mật khẩu";

            return "Thành công";
        }
    }
}
