using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Quản_lí_khách_sạn
{
    public partial class ĐăngNhập: Form
    {
        Function fn = new Function();
        string query;
        public ĐăngNhập()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
           
        }

        private void bntExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        bool isPasswordVisible = false; // biến toàn cục trong class

       

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1️⃣ Đăng nhập bằng tài khoản trong CSDL (JOIN TAIKHOAN + NHANVIEN)
            string query = $@"
        SELECT nv.MANV, tk.TENTK, nv.TENNV, nv.CHUCVU
        FROM TAIKHOAN tk
        INNER JOIN NHANVIEN nv ON tk.MANV = nv.MANV
        WHERE tk.TENTK = '{username}' AND tk.MATKHAU = '{password}'";

            DataSet ds = fn.getdata(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                CurrentUser.Id = Convert.ToInt32(row["MANV"]);
                CurrentUser.UserName = row["TENTK"].ToString();
                CurrentUser.Display = row["TENNV"].ToString();
                CurrentUser.Role = row["CHUCVU"].ToString().ToLower().Trim();

                LabelError.Visible = false;
                new TrangChủ().Show();
                this.Hide();
                return;
            }

            // 2️⃣ Tài khoản admin dự phòng (chưa lưu trong DB hoặc xóa mất)
            if (username.ToLower() == "b" && password == "123")
            {
                string tentk = "admin";
                string matkhau = "123";
                string email = "admin@ks.vn";
                string chucvu = "quanly";

                CurrentUser.UserName = tentk;
                CurrentUser.Display = "Quản trị viên";
                CurrentUser.Role = chucvu;

                // 🟡 Kiểm tra tài khoản admin đã có trong DB chưa
                string checkQuery = $"SELECT nv.MANV FROM TAIKHOAN tk JOIN NHANVIEN nv ON tk.MANV = nv.MANV WHERE tk.TENTK = '{tentk}' AND tk.MATKHAU = '{matkhau}'";
                DataSet dt = fn.getdata(checkQuery);

                int manv;

                if (dt.Tables[0].Rows.Count == 0)
                {
                    // 🛠 Chưa có admin → thêm vào NHANVIEN
                    string insertNV = @"
                INSERT INTO NHANVIEN (TENNV, SDTNV, GIOITINHNV, EMAILNV, CHUCVU)
                VALUES (N'Quản trị viên', 0, N'Không rõ', 'admin@ks.vn', 'quanly');
                SELECT SCOPE_IDENTITY() AS NewID;
            ";
                    DataSet newId = fn.getdata(insertNV);
                    manv = Convert.ToInt32(newId.Tables[0].Rows[0]["NewID"]);

                    // ➕ Thêm vào TAIKHOAN
                    string insertTK = $"INSERT INTO TAIKHOAN (TENTK, MATKHAU, MANV) VALUES ('{tentk}', '{matkhau}', {manv})";
                    fn.setdata(insertTK, "Tài khoản admin mặc định đã được tạo.");
                }
                else
                {
                    manv = Convert.ToInt32(dt.Tables[0].Rows[0]["MANV"]);
                }

                CurrentUser.Id = manv;

                LabelError.Visible = false;
                new TrangChủ().Show();
                this.Hide();
                return;
            }

            // ❌ Sai tài khoản/mật khẩu
            LabelError.Visible = true;
            txtPassword.Clear();
        }

        private void bntExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
