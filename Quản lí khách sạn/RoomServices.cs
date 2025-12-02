using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class Room
    {
        public string SoPhong { get; set; }
        public string LoaiPhong { get; set; }
        public string Giuong { get; set; }
        public decimal Gia { get; set; }
        public string TrangThai { get; set; } = "Trống";
    }

    public class RoomServices
    {
        public bool IsTesting { get; set; } = false;
        private readonly Function fn = new Function();

        public bool AddRoom(Room room)
        {
            // Kiểm tra dữ liệu
            if (IsInvalid(room))
                return false;

            // Bỏ qua DB nếu đang test
            if (!IsTesting && IsDuplicate(room.SoPhong))
                return false;

            return Insert(room);
        }

        private bool IsInvalid(Room r)
        {
            return string.IsNullOrWhiteSpace(r.SoPhong) ||
                   string.IsNullOrWhiteSpace(r.LoaiPhong) ||
                   string.IsNullOrWhiteSpace(r.Giuong) ||
                   r.Gia <= 0;
        }

        private bool IsDuplicate(string soPhong)
        {
            if (IsTesting)
                return false;   // Test: luôn không trùng

            string q = $"SELECT COUNT(*) FROM PHONG WHERE SOPHONG = '{soPhong}'";
            DataSet ds = fn.getdata(q);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        private bool Insert(Room r)
        {
            if (IsTesting)
                return true;   // Test: luôn thành công

            try
            {
                string q = $"INSERT INTO PHONG (SOPHONG, LOAIPHONG, GIUONG, GIA, DATPHONG) " +
                           $"VALUES ('{r.SoPhong}', N'{r.LoaiPhong}', N'{r.Giuong}', {r.Gia}, N'{r.TrangThai}')";

                fn.setdata(q, "Đã thêm phòng");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
