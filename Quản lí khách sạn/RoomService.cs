using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class RoomService
    {
      
        public bool SuaPhong(int maPhong, string soPhongMoi, string loaiPhongMoi, string giuongMoi, long giaMoi)
        {
           
            if (maPhong <= 0 || string.IsNullOrWhiteSpace(soPhongMoi) ||
                string.IsNullOrWhiteSpace(loaiPhongMoi) || string.IsNullOrWhiteSpace(giuongMoi) || giaMoi <= 0)
                return false;

            
            return true;

        }
    }
}

