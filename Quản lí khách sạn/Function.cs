using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lí_khách_sạn
{
    class Function
    {
        // kết nối sql
        protected SqlConnection getconnection()
        {
            //Tạo một đối tượng kết nối đến SQL Server tên là con.
            SqlConnection con = new SqlConnection();
            //Gán chuỗi kết nối (Connection String) cho đối tượng con.
            con.ConnectionString = "Data Source=LAPTOP-GHEUHM8V\\SQLEXPRESS01;Initial Catalog = QL_KS; Integrated Security = True"; 
            return con;
            
        }


        // LẤY DATA 
        public DataSet getdata(string query)
        {
            // tạo một đối tượng conn để kết nối vào sql bằng hàm getconnection 
            SqlConnection conn = getconnection();
            // tạo đối tượng cmd để tạo ra nhưng câu lệnh sql trống 
            SqlCommand cmd = new SqlCommand();
            // gán kết nối cho lệnh sql vừa tạo 
            cmd.Connection = conn;
            // đưa câu lệnh sql (select , update , delete ..) truyền vào cho lệnh sql vừa tạo 
            cmd.CommandText = query;
            //// ⑥ Adapter làm “cầu nối” để đổ dữ liệu vào DataSet
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // tạo đói tượng ds là một vùng nhớ rỗng để chưas dữ liệu 
            DataSet ds = new DataSet();
            // apdapter chạy lệnh sql và đổ dữ liệu vào dataset 
            da.Fill(ds);
            return ds;
        }

        // ĐẶT DATA luu data
        public void setdata(string query, string message)
        {
            SqlConnection conn = getconnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            // Gán câu SQL (INSERT / UPDATE / DELETE …)
            cmd.CommandText = query;
            // thực thi câu lệnh sql 
            // Khi câu lệnh không cần lấy dữ liệu ra (không SELECT), mà chỉ thay đổi dữ liệu hoặc cấu trúc DB 
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show(message, "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ĐỌC DATA
        // gửi query kết quả và đọc từng dòng 
        public SqlDataReader getforcombo(string query)
        {
            SqlConnection conn = getconnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            // tạo ra một  lệnh SQL đã gắn sẵn câu truy vấn và “đường dây” kết nối
            // . Từ đây bạn chỉ cần thêm tham số (nếu có), mở kết nối, rồi gọi Execute… là xong!
            cmd = new SqlCommand(query, conn);
            // gửi truy vấn sql tới sever và trả về  kết quả vào đối tượng sdr 
            // mục đích là dùng để đọc kết quả truy vấn từ CSDL theo từng dòng một; 
            SqlDataReader sdr = cmd.ExecuteReader();
            return sdr;
        }
    }
}
