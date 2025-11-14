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
        private Function fn = new Function(); // class Function bạn đã có sẵn

        public bool Login(string username, string password)
        {
            // Không nhập gì
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            // Truy vấn tài khoản trong CSDL
            string query = $@"
                SELECT * FROM TAIKHOAN tk
                JOIN NHANVIEN nv ON tk.MANV = nv.MANV
                WHERE tk.TENTK = '{username}' AND tk.MATKHAU = '{password}'";

            DataSet ds = fn.getdata(query);
            if (ds.Tables[0].Rows.Count > 0)
                return true;

            // Tài khoản admin mặc định
            if (username.ToLower() == "b" && password == "123")
                return true;

            return false;
        }
    }
}
