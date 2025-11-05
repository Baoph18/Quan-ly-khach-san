using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    internal class CurrentUser
    {
        // Mã nhân viên đang đăng nhập
        public static int Id { get; set; }

        // Tên đăng nhập (TENTK)
        public static string UserName { get; set; }

        // Họ tên nhân viên (TENNV)
        public static string Display { get; set; }

        public static string Role { get; set; }  // Thêm dòng này để phân quyền
    }
}
