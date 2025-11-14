using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class BookingService
    {
        public bool DatPhong(int maPhong, int maKhach, int soDem, string ngayNhan, string ngayTra)
        {
            
            if (maPhong <= 0 || maKhach <= 0 || soDem <= 0 ||
                string.IsNullOrWhiteSpace(ngayNhan) || string.IsNullOrWhiteSpace(ngayTra))
                return false;

            return true;
        }
    }
}
