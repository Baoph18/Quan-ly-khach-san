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
using System.Data.SqlClient;

using log4net;
using log4net.Config;
using System.IO;

namespace Quản_lí_khách_sạn.ksquanli
{
    //Log bth5
    public partial class uc_Employee: UserControl
    {
        //// Khai báo một logger cho Program.cs 
        private static readonly ILog log = LogManager.GetLogger(typeof(uc_Employee));
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
            try
            {
                if (tabEmployee.SelectedIndex == 1)
                {
                    if (dataGridView1 != null)
                        SetEmployee(dataGridView1);
                }
                else if (tabEmployee.SelectedIndex == 2)
                {
                    if (dataGridView2 != null)
                        SetEmployee(dataGridView2);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Một bảng dữ liệu chưa được khởi tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chuyển tab: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // TẠO CLASS THAM SỐ 
        public class NhanVienDeleteInfo
        {
            public int MaNV { get; set; }
        }

        // TÁCH HÀM 
        private bool LayVaXacNhanMaNhanVien(out NhanVienDeleteInfo info)
        {
            info = null;

            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtID.Text.Trim(), out int maNV))
            {
                MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            info = new NhanVienDeleteInfo { MaNV = maNV };
            return true;
        }

        private bool XacNhanXoaNhanVien()
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return confirm == DialogResult.Yes;
        }

        private void XoaNhanVien(NhanVienDeleteInfo info)
        {
            try
            {
                string query = $@"
            DELETE FROM TAIKHOAN WHERE MANV = {info.MaNV};
            DELETE FROM NHANVIEN WHERE MANV = {info.MaNV};
        ";

                fn.setdata(query, "Đã xóa nhân viên và tài khoản thành công!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Không thể xóa nhân viên vì có ràng buộc dữ liệu.\nChi tiết: " + ex.Message,
                    "Lỗi SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(
                    "Kết nối cơ sở dữ liệu gặp lỗi hoặc bị đóng!",
                    "Lỗi CSDL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(
                    "Dữ liệu nhân viên để xóa đang null!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi không xác định: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }//fđf
        }
//jkjkjk

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!LayVaXacNhanMaNhanVien(out NhanVienDeleteInfo info))
                return;

            if (!XacNhanXoaNhanVien())
                return;

            try
            {
                XoaNhanVien(info);

                // Làm mới DataGridView (Giả định rằng hàm này có sẵn)
                tabEmployee_SelectedIndexChanged(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhân viên:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtID.Text))
        //    {
        //        MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    // kiểm tra nhập vào có pk số nguyên hay ko
        //    if (!int.TryParse(txtID.Text.Trim(), out int manv))
        //    {
        //        MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận",
        //                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //    if (confirm == DialogResult.Yes)
        //    {
        //        try
        //        {
        //            // 1️⃣ Nếu khóa ngoại trong HOADON KHÔNG dùng ON DELETE SET NULL
        //            // thì bạn phải xóa HOADON thủ công:
        //            // fn.setdata($"DELETE FROM HOADON WHERE MANV = {manv}", "");

        //            string query = $@"
        //            DELETE FROM TAIKHOAN WHERE MANV = {manv};
        //            DELETE FROM NHANVIEN WHERE MANV = {manv};";

        //            fn.setdata(query, "Đã xóa nhân viên và tài khoản thành công!");

        //            // 4️⃣ Làm mới DataGridView
        //            tabEmployee_SelectedIndexChanged(this, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Lỗi khi xóa nhân viên:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

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

        // TẠO CLASS THAM SỐ 
        public class NhanVienUpdateInfo
        {
            public int MaNV { get; set; }
            public string Ten { get; set; }
            public string SDT { get; set; }
            public string GioiTinh { get; set; }
            public string Email { get; set; }
        }

        // TÁCH HÀM 
        private bool LayVaXacNhanThongTinCapNhat(out NhanVienUpdateInfo info)
        {
            info = null;

            if (string.IsNullOrWhiteSpace(txtIDNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!KiemTraEmail(txtEmailr.Text))
            {
                MessageBox.Show("Email vừa nhập không hợp lệ!!!", "Thông báo",
                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmailr.Focus(); // Sử dụng txtEmailr thay vì txtEmail (theo code gốc)
                return false;
            }

            // Đảm bảo MaNV là số nguyên hợp lệ (giả định đã kiểm tra khi tải)
            if (!int.TryParse(txtIDNV.Text.Trim(), out int maNv))
            {
                MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Gán dữ liệu, xử lý SQL Injection (thay ' bằng '')
            info = new NhanVienUpdateInfo
            {
                MaNV = maNv,
                Ten = txtTenNV.Text.Trim().Replace("'", "''"),
                SDT = txtSDTNV.Text.Trim(),
                GioiTinh = cboGioiTinh.Text.Trim().Replace("'", "''"),
                Email = txtEmailr.Text.Trim().Replace("'", "''")
            };

            return true;
        }

        private void CapNhatNhanVien(NhanVienUpdateInfo info)
        {
            string query = $@"
        UPDATE NHANVIEN SET 
        TENNV = N'{info.Ten}', 
        SDTNV = '{info.SDT}', 
        GIOITINHNV = N'{info.GioiTinh}', 
        EMAILNV = N'{info.Email}' 
        WHERE MANV = {info.MaNV};"; // Sử dụng info.MaNV

            fn.setdata(query, "Cập nhật thông tin nhân viên thành công!");
        }
        
        private void btnRepair_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu đầu vào và gom thành object
            if (!LayVaXacNhanThongTinCapNhat(out NhanVienUpdateInfo info))
                return;

            try
            {
                // 2. Thực hiện cập nhật vào DB
                CapNhatNhanVien(info);
                // Yêu cầu Log4net đọc file config 
                XmlConfigurator.Configure(new FileInfo("log4net.config"));
                // Ghi log INFO khi sửa nhân viên thành công
                log.Info(
                    $"Sua thong tin nhan vien thanh cong: MaNV={info.MaNV}, Ten='{info.Ten}', " +
                    $"SDT='{info.SDT}', GioiTinh='{info.GioiTinh}', Email='{info.Email}'"
                );
                // 3. Làm mới UI
                SetEmployee(dataGridView1); // làm mới danh sách
                Clear1();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }










        //private void btnRepair_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtIDNV.Text))
        //    {
        //        MessageBox.Show("Vui lòng chọn nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    if (!checkEmail(txtEmailr.Text))
        //    {
        //        MessageBox.Show("Email vừa nhập không hợp lệ!!!", "Thông báo",
        //         MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        txtEmail.Focus();
        //    }
        //    else
        //    {
        //        try
        //        {
        //            string manv = txtIDNV.Text.Trim();
        //            string ten = txtTenNV.Text.Trim().Replace("'", "''");
        //            string sdt = txtSDTNV.Text.Trim();
        //            string gioitinh = cboGioiTinh.Text.Trim().Replace("'", "''");
        //            string email = txtEmailr.Text.Trim().Replace("'", "''");


        //            query = $"UPDATE NHANVIEN SET " +
        //            $"TENNV = N'{ten}', " +
        //            $"SDTNV = '{sdt}', " +
        //            $"GIOITINHNV = N'{gioitinh}', " +
        //            $"EMAILNV = N'{email}' " +  // ✅ không có dấu phẩy
        //            $"WHERE MANV = {manv}";

        //            fn.setdata(query, "Cập nhật thông tin nhân viên thành công!");

        //            SetEmployee(dataGridView1);  // làm mới danh sách
        //            Clear1();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }


        //}

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
            XmlConfigurator.Configure(new FileInfo("Log4net.config"));

            if (!NhapDuLieuDayDu())
            {
                Logger.Warn("Thiếu dữ liệu đăng ký.");
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!KiemTraEmail(txtEmail.Text))
                return;

            try
            {
                DangKyNhanVien();
            }
            catch (Exception ex)
            {
                XuLyLoi(ex);
            }


        }

        private bool NhapDuLieuDayDu()
        {
            return txtName.Text != "" && txtMobile.Text != "" && txtGender.Text != "" &&
                   txtEmail.Text != "" && txtUserName.Text != "" && txtPassword.Text != "";
        }

        // 2️⃣ Kiểm tra email hợp lệ và chưa tồn tại
        

        // 3️⃣ Thực hiện đăng ký nhân viên và tạo tài khoản
        private void DangKyNhanVien()
        {
            string ten = txtName.Text.Trim().Replace("'", "''");
            string sdt = txtMobile.Text.Trim();
            string gioiTinh = txtGender.Text.Trim().Replace("'", "''");
            string email = txtEmail.Text.Trim().Replace("'", "''");
            string taiKhoan = txtUserName.Text.Trim().Replace("'", "''");
            string matKhau = txtPassword.Text.Trim().Replace("'", "''");
            string chucVu = "nhanvien";

            Logger.Info($"Dữ liệu nhập: TENNV={ten}, SDTNV={sdt}, GIOITINHNV={gioiTinh}, EMAILNV={email}, TENTK={taiKhoan}");

            // Thêm nhân viên mới
            string query = $"INSERT INTO NHANVIEN (TENNV, SDTNV, GIOITINHNV, EMAILNV, CHUCVU) " +
                           $"VALUES (N'{ten}', '{sdt}', N'{gioiTinh}', '{email}', '{chucVu}')";
            fn.setdata(query, "Đăng ký nhân viên thành công!");
            Logger.Info("Đăng ký nhân viên thành công: " + ten);

            // Lấy MANV mới nhất
            int maNV = LayMaNhanVienMoiNhat();

            // Thêm tài khoản
            query = $"INSERT INTO TAIKHOAN (TENTK, MATKHAU, MANV) VALUES ('{taiKhoan}', '{matKhau}', {maNV})";
            fn.setdata(query, "Tạo tài khoản thành công!");
            Logger.Info("Tạo tài khoản thành công cho MANV=" + maNV);

            clearAll();
            getMaxID();
        }
        private void XuLyLoi(Exception ex)
        {
            Logger.Error("Lỗi khi đăng ký nhân viên: " + ex.Message);
            Logger.Error("StackTrace: " + ex.StackTrace);

            MessageBox.Show("Đã xảy ra lỗi khi Thêm nhân viên!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private int LayMaNhanVienMoiNhat()
        {
            string getIdQuery = "SELECT MAX(MANV) FROM NHANVIEN";
            DataSet ds = fn.getdata(getIdQuery);
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
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
        private bool KiemTraEmail(string email)
        {
            if (!email.Contains("@"))
            {
                Logger.Error("Email sai: thiếu @");
                MessageBox.Show("Email phải chứa '@'!", "Sai email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!email.EndsWith(".com"))
            {
                Logger.Error("Email sai: thiếu .com");
                MessageBox.Show("Email phải kết thúc bằng '.com'!", "Sai email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int index1 = email.IndexOf("@");
            int index2 = email.IndexOf(".com");

            string domain = email.Substring(index1 + 1, index2 - index1 - 1);

            if (domain != "gmail" && domain != "hotmail")
            {
                Logger.Error("Email sai: domain không phải gmail hoặc hotmail");
                MessageBox.Show("Email phải là gmail hoặc hotmail!", "Sai email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
            string input = txtTenNV.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    // Yêu cầu Log4net đọc file config 
                    XmlConfigurator.Configure(new FileInfo("log4net.config"));
                    //// Ghi log WARN
                    //log.Warn($"Nhap sai ten nhan vien: '{input}'. Chi duoc nhap chu cai va khoang trang.");

                    // CHUYỂN SANG GHI LOG ERROR
                    log.Error($"Nhap sai ten nhan vien: '{input}'. Chi duoc nhap chu cai va khoang trang.");

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
