using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quanlykhachsan.tests
{
    public class NhanVienServices
    {
        private List<NhanVien> danhSachNhanVien = new List<NhanVien>();
        public string AddNhanVien(NhanVien nv)
        {
            if (nv.MaNV <= 0 ||
                string.IsNullOrWhiteSpace(nv.Ten) ||
                string.IsNullOrWhiteSpace(nv.SDT) ||
                string.IsNullOrWhiteSpace(nv.GioiTinh) ||
                string.IsNullOrWhiteSpace(nv.Email))
                return "Thiếu thông tin";

            if (!Regex.IsMatch(nv.SDT, @"^0\d{9}$"))
                return "SDT không hợp lệ";

            if (nv.GioiTinh != "Nam" && nv.GioiTinh != "Nữ")
                return "Giới tính không hợp lệ";

            if (!Regex.IsMatch(nv.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Email không hợp lệ";
            danhSachNhanVien.Add(nv);
            return "Thành công";
        }

        public string SuaNhanVien(NhanVien nv)
        {
            if (nv.MaNV <= 0)
                return "Mã nhân viên không hợp lệ";

            if (string.IsNullOrWhiteSpace(nv.Ten))
                return "Tên không hợp lệ";

            if (!Regex.IsMatch(nv.SDT, @"^0\d{9}$"))
                return "SDT không hợp lệ";

            if (nv.GioiTinh != "Nam" && nv.GioiTinh != "Nữ")
                return "Giới tính không hợp lệ";

            if (!Regex.IsMatch(nv.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Email không hợp lệ";

            return "Sửa thành công";
        }
        
        public string XoaNhanVien(int maNV)
        {
            if (maNV <= 0)
                return "Mã nhân viên không hợp lệ";

            var nv = danhSachNhanVien.FirstOrDefault(x => x.MaNV == maNV);

            if (nv == null)
                return "Nhân viên không tồn tại";

            danhSachNhanVien.Remove(nv);

            return "Xóa thành công";
        }
    }
}
