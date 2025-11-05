using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Quản_lí_khách_sạn.ksquanli
{
    
    public partial class uc_Employee: UserControl
    {
        Function fn = new Function();
        string query;
        public uc_Employee()
        {
            InitializeComponent();
        }

        private void uc_Employee_Load(object sender, EventArgs e)
        {
            getMaxID();
        }

        public void getMaxID()
        {
            query = "select max(MANV) from NHANVIEN";
            DataSet ds = fn.getdata(query);

            if (ds.Tables[0].Rows[0][0].ToString() != "")
            {
                Int64 num = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
                labelToSet.Text = (num + 1).ToString();
            }
        }

       
        public void clearAll()
        {
            txtName.Clear();
            txtMobile.Clear();
            txtGender.SelectedIndex = -1;
            txtEmail.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
        }

        
        private void tabEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (tabEmployee.SelectedIndex == 1)
           {
                SetEmployee(dataGridView1);
           }
           else if(tabEmployee.SelectedIndex == 2)
           {
                SetEmployee(dataGridView2);
           }
        }

        public void SetEmployee(DataGridView dgv)
        {
            query = @"
        SELECT nv.MANV AS [Mã Nhân Viên], nv.TENNV AS [Tên Nhân Viên], nv.SDTNV AS [Số điện thoại], nv.GIOITINHNV AS [Giới Tính], nv.EMAILNV AS [Email], nv.CHUCVU AS [Chức Vụ],
               tk.TENTK AS [Tên Tài Khoản], tk.MATKHAU AS [Mật Khẩu]
        FROM NHANVIEN nv
        LEFT JOIN TAIKHOAN tk ON nv.MANV = tk.MANV";

            DataSet ds = fn.getdata(query);
            dgv.DataSource = ds.Tables[0];
           

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // kiểm tra nhập vào có pk số nguyên hay ko
            if (!int.TryParse(txtID.Text.Trim(), out int manv))
            {
                MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // 1️⃣ Nếu khóa ngoại trong HOADON KHÔNG dùng ON DELETE SET NULL
                    // thì bạn phải xóa HOADON thủ công:
                    // fn.setdata($"DELETE FROM HOADON WHERE MANV = {manv}", "");

                    string query = $@"
    DELETE FROM TAIKHOAN WHERE MANV = {manv};
    DELETE FROM NHANVIEN WHERE MANV = {manv};";

                    fn.setdata(query, "Đã xóa nhân viên và tài khoản thành công!");

                    // 4️⃣ Làm mới DataGridView
                    tabEmployee_SelectedIndexChanged(this, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void uc_Employee_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        private void txtGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtIDNV.Text = row.Cells[0].Value.ToString();
                txtTenNV.Text = row.Cells[1].Value.ToString();
                txtSDTNV.Text = row.Cells[2].Value.ToString();
                cboGioiTinh.Text = row.Cells[3].Value.ToString();
                txtEmailr.Text = row.Cells[4].Value.ToString();
                
            }
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!checkEmail(txtEmailr.Text))
            {
                MessageBox.Show("Email vừa nhập không hợp lệ!!!", "Thông báo",
                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
            }
            else
            {
                try
                {
                    string manv = txtIDNV.Text.Trim();
                    string ten = txtTenNV.Text.Trim().Replace("'", "''");
                    string sdt = txtSDTNV.Text.Trim();
                    string gioitinh = cboGioiTinh.Text.Trim().Replace("'", "''");
                    string email = txtEmailr.Text.Trim().Replace("'", "''");


                    query = $"UPDATE NHANVIEN SET " +
            $"TENNV = N'{ten}', " +
            $"SDTNV = '{sdt}', " +
            $"GIOITINHNV = N'{gioitinh}', " +
            $"EMAILNV = N'{email}' " +  // ✅ không có dấu phẩy
            $"WHERE MANV = {manv}";

                    fn.setdata(query, "Cập nhật thông tin nhân viên thành công!");

                    SetEmployee(dataGridView1);  // làm mới danh sách
                    Clear1();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
           
        }

        private void Clear1()
        {
            txtIDNV.Clear();
            txtTenNV.Clear();
            txtSDTNV.Clear();
            txtEmailr.Clear();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnDangKy_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtMobile.Text != "" && txtGender.Text != "" &&
        txtEmail.Text != "" && txtUserName.Text != "" && txtPassword.Text != "")
            {

                if (!checkEmail(txtEmail.Text))
                {    
                    MessageBox.Show("Email vừa nhập không hợp lệ!!!", "Thông báo",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                }
                else
                {
                    try
                    {

                        string name = txtName.Text.Trim().Replace("'", "''");
                        string mobile = txtMobile.Text.Trim();
                        string gender = txtGender.Text.Trim().Replace("'", "''");
                        string email = txtEmail.Text.Trim().Replace("'", "''");
                        string username = txtUserName.Text.Trim().Replace("'", "''");
                        string pass = txtPassword.Text.Trim().Replace("'", "''");
                        string chucvu = "nhanvien";

                        string checkEmailQuery = $"SELECT COUNT(*) FROM NHANVIEN WHERE EMAILNV = '{email}'";
                        DataSet checkDs = fn.getdata(checkEmailQuery);
                        int count = Convert.ToInt32(checkDs.Tables[0].Rows[0][0]);

                        if (count > 0)
                        {
                            MessageBox.Show("Email này đã tồn tại. Vui lòng nhập email khác!", "Trùng Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtEmail.Focus();
                            return;
                        }
                        // 1️⃣ Thêm nhân viên mới (chưa có tài khoản)
                        query = $"INSERT INTO NHANVIEN (TENNV, SDTNV, GIOITINHNV, EMAILNV, CHUCVU) " +
                                $"VALUES (N'{name}', '{mobile}', N'{gender}', '{email}', '{chucvu}')";
                        fn.setdata(query, "Đăng ký nhân viên thành công!");

                        // 2️⃣ Lấy MANV mới nhất
                        string getIdQuery = "SELECT MAX(MANV) FROM NHANVIEN";
                        DataSet ds = fn.getdata(getIdQuery);

                        // SỬA chỗ này: dùng long (Int64) thay vì int
                        int manv = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                        // 3️⃣ Thêm vào bảng tài khoản
                        query = $"INSERT INTO TAIKHOAN (TENTK, MATKHAU, MANV) VALUES ('{username}', '{pass}', {manv})";
                        fn.setdata(query, "Tạo tài khoản thành công!");

                        clearAll();
                        getMaxID();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }    
                
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string input = txtName.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }

        private void txtMobile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtMobile.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobile.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtUserName.Text;

                // Regex: chỉ cho chữ cái a-z và A-Z, có thể thêm khoảng trắng nếu cần
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]*$"))
                {
                    throw new Exception("Chỉ được nhập chữ cái. Không cho phép số hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
        private bool checkEmail(string email)
        {
            //Kiểm tra có chứa @ không?
            if (!email.Contains("@"))
            {
                return false;
            }
            //Nếu không chứa ".com" -> Sai
            if (!email.Contains(".com"))
            {
                return false;
            }
            //Tìm vị trí @ trong chuỗi email
            int index1 = email.IndexOf("@");
            int index2 = email.IndexOf(".com");
            //Lấy ra tên miền nếu có của email
            string domain = email.Substring(index1 + 1, index2 - index1 - 1);
            //Tại đây bạn có thể thêm miền nào muốn vào đây
            //Nếu không nằm trong đây trả về false
            if (domain != "gmail" && domain != "hotmail")
            {
                return false;
            }
            return true;
        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
            string input = txtName.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }

        private void txtIDNV_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtMobile.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobile.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtSDTNV_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtMobile.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobile.Text = ""; // Xóa dữ liệu sai
            }
        }
    }
}
