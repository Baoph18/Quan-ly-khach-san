using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

    public class RoomService
    {
        // Danh sách phòng lưu tạm trong RAM
        private List<Room> danhSachPhong = new List<Room>();

        public bool AddRoom(Room room)
        {
            if (IsInvalid(room))
                return false;

            if (IsDuplicate(room.SoPhong))
                return false;

            danhSachPhong.Add(room);
            return true;
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
            return danhSachPhong.Any(p => p.SoPhong == soPhong);
        }

        public bool UpdateRoom(Room room)
        {
            if (IsInvalid(room))
                return false;

            var phong = danhSachPhong.FirstOrDefault(p => p.SoPhong == room.SoPhong);

            if (phong == null)
                return false;

            phong.LoaiPhong = room.LoaiPhong;
            phong.Giuong = room.Giuong;
            phong.Gia = room.Gia;
            phong.TrangThai = room.TrangThai;

            return true;
        }
    }



}

