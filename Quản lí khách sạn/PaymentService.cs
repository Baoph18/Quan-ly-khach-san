using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quản_lí_khách_sạn
{
    public class PaymentService
    {
       
        public bool ThanhToan(int maHoaDon, decimal tongTien, string phuongThuc)
        {
          
            if (maHoaDon <= 0 || tongTien <= 0 || string.IsNullOrWhiteSpace(phuongThuc))
                return false;

         
            return true;
        }
    }
}
