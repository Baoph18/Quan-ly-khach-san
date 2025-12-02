using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lí_khách_sạn.ksquanli
{
    //siu
    public partial class ThongTinCaNhan: UserControl
    {
        Function fn = new Function();
        public ThongTinCaNhan()
        {
            InitializeComponent();
          }

      private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private readonly Function _function = new Function();
        private void ThongTinCaNhan_Load(object sender, EventArgs e)
        {
            LoadEmployeeInfo();
        }
        //su
        private void LoadEmployeeInfo()
        {
            try
            {
                if (CurrentUser.Id <= 0)
                {
                    MessageBox.Show("Không xác định được người dùng hiện tại.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = $@"
            SELECT nv.TENNV, nv.SDTNV, nv.GIOITINHNV, nv.EMAILNV,
                   tk.TENTK, tk.MATKHAU
            FROM NHANVIEN nv
            JOIN TAIKHOAN tk ON nv.MANV = tk.MANV
            WHERE nv.MANV = {CurrentUser.Id}";

                DataSet dataSet = _function.getdata(query);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin nhân viên.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DisplayEmployeeInfo(dataSet.Tables[0].Rows[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayEmployeeInfo(DataRow row)
        {
            txtName.Text = row["TENNV"]?.ToString();
            txtMobile.Text = row["SDTNV"]?.ToString();
            txtGender.Text = row["GIOITINHNV"]?.ToString();
            txtEmail.Text = row["EMAILNV"]?.ToString();
            txtUserName.Text = row["TENTK"]?.ToString();
            txtPassword.Text = row["MATKHAU"]?.ToString();
        }




        private void btnDangxuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.OpenForms["TrangChủ"]?.Hide(); // Ẩn form chính nếu có
                new ĐăngNhập().Show();  // Mở lại form đăng nhập
            }
        }
    }
}
