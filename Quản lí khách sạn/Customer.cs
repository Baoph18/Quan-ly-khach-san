using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Quản_lí_khách_sạn.ksquanli.uc_CustomerDetails;

namespace Quản_lí_khách_sạn
{
    public class Customer
    {
        private List<KhachHangUpdateInfo> danhSachKH = new List<KhachHangUpdateInfo>();

        public bool AddKhachHang(KhachHangUpdateInfo kh)
        {
            if (kh == null || kh.MaKH <= 0)
                return false;

            if (danhSachKH.Any(x => x.MaKH == kh.MaKH))
                return false;

            danhSachKH.Add(kh);
            return true;
        }

        public bool DeleteKhachHang(int maKH)
        {
            var kh = danhSachKH.FirstOrDefault(x => x.MaKH == maKH);

            if (kh == null)
                return false;

            danhSachKH.Remove(kh);
            return true;
        }

        public bool UpdateKhachHang(KhachHangUpdateInfo khMoi)
        {
            if (khMoi == null || khMoi.MaKH <= 0)
                return false;

            // Kiểm tra SDT chỉ chứa số
            if (!string.IsNullOrEmpty(khMoi.SDT))
            {
                if (!Regex.IsMatch(khMoi.SDT, @"^\d+$"))
                    return false;
            }

            var khCu = danhSachKH.FirstOrDefault(x => x.MaKH == khMoi.MaKH);

            if (khCu == null)
                return false;

            khCu.Ten = khMoi.Ten;
            khCu.SDT = khMoi.SDT;
            khCu.QuocTich = khMoi.QuocTich;
            khCu.GioiTinh = khMoi.GioiTinh;
            khCu.MaDD = khMoi.MaDD;
            khCu.DiaChi = khMoi.DiaChi;
            khCu.SoDem = khMoi.SoDem;

            return true;
        }
    }
}
