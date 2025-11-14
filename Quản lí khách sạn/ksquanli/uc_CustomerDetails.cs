using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Quản_lí_khách_sạn.ksquanli
{
    public partial class uc_CustomerDetails: UserControl
    {
        Function fn = new Function();
        string query;
        public uc_CustomerDetails()
        {
            InitializeComponent();
        }

       
        // gửi query lấy dữ liệu và gán dữ liệu lên datagridview 
        private void getrecord(String query)
        {
            DataSet ds = fn.getdata(query);
            dataGridView1.DataSource = ds.Tables[0];
           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                // Lấy MAKH và MAPHONG từ dòng được chọn
                int makh = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Mã Khách Hàng"].Value.ToString());
                string soPhong = dataGridView1.SelectedRows[0].Cells["Số Phòng"].Value.ToString();

                // Tìm MAPHONG từ SOPHONG
                string getMaphongQuery = $"SELECT MAPHONG FROM PHONG WHERE SOPHONG = '{soPhong}'";
                DataSet dsPhong = fn.getdata(getMaphongQuery);

                if (dsPhong.Tables[0].Rows.Count > 0)
                {
                    int maPhong = Convert.ToInt32(dsPhong.Tables[0].Rows[0]["MAPHONG"]);

                    // 1️⃣ Cập nhật lại trạng thái phòng
                    string updatePhong = $"UPDATE PHONG SET DATPHONG = 'NO' WHERE MAPHONG = {maPhong}";
                    fn.setdata(updatePhong, "Đã giải phóng phòng.");
                }

                // 2️⃣ Xóa khách hàng
                string deleteQuery = $"DELETE FROM KHACHHANG WHERE MAKH = {makh}";
                fn.setdata(deleteQuery, "Đã xóa khách hàng thành công!");
                load();
               


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtMAKH.Text = row.Cells[0].Value.ToString(); // Mã Khách Hàng
                txtTENKH.Text = row.Cells[1].Value.ToString(); // Tên Khách Hàng
                txtSDT.Text = row.Cells[2].Value.ToString(); // Số điện thoại
                txtQUOCTICH.Text = row.Cells[3].Value.ToString(); // Nước
                cboGIOITINH.Text = row.Cells[4].Value.ToString(); // Giới Tính
                txtMADD.Text = row.Cells[5].Value.ToString(); // Mã Định Danh
                txtDIACHI.Text = row.Cells[6].Value.ToString(); // Địa Chỉ
                txtSoDem.Text = row.Cells[7].Value.ToString(); // Số Đêm
            }
        }
        // TẠO CLASS THAM SỐ 
        public class KhachHangUpdateInfo
        {
            public int MaKH { get; set; }
            public string Ten { get; set; }
            public string SDT { get; set; }
            public string QuocTich { get; set; }
            public string GioiTinh { get; set; }
            public string MaDD { get; set; }
            public string DiaChi { get; set; }
            public string SoDem { get; set; }
        }

        //TÁCH HÀM (Extract Method)
        private bool XacNhanSuaKhachHang()
        {
            if (string.IsNullOrWhiteSpace(txtMAKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private KhachHangUpdateInfo LayThongTinKhachHang()
        {
            return new KhachHangUpdateInfo
            {
                MaKH = Convert.ToInt32(txtMAKH.Text),
                Ten = txtTENKH.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                QuocTich = txtQUOCTICH.Text.Trim(),
                GioiTinh = cboGIOITINH.Text.Trim(),
                MaDD = txtMADD.Text.Trim(),
                DiaChi = txtDIACHI.Text.Trim(),
                SoDem = txtSoDem.Text.Trim()
            };
        }

        private void CapNhatKhachHang(KhachHangUpdateInfo info)
        {
            string query = $@"
                UPDATE KHACHHANG SET
                    TENKH = N'{info.Ten}',
                    SDT = '{info.SDT}',
                    NUOC = N'{info.QuocTich}',
                    GIOITINH = N'{info.GioiTinh}',
                    MADD = N'{info.MaDD}',
                    DIACHI = N'{info.DiaChi}',
                    SODEM = '{info.SoDem}'
                WHERE MAKH = {info.MaKH}
            ";

            fn.setdata(query, "Thông tin khách hàng đã được cập nhật!");
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            if (!XacNhanSuaKhachHang())
                return;

            try
            {
                KhachHangUpdateInfo info = LayThongTinKhachHang();
                CapNhatKhachHang(info);

                load();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //private void btnRepair_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtMAKH.Text))
        //    {
        //        MessageBox.Show("Vui lòng chọn khách hàng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    try
        //    {
        //        int makh = Convert.ToInt32(txtMAKH.Text);
        //        string ten = txtTENKH.Text;
        //        string sdt = txtSDT.Text;
        //        string quoctich = txtQUOCTICH.Text;
        //        string gioitinh = cboGIOITINH.Text;
        //        string madd = txtMADD.Text;
        //        string diachi = txtDIACHI.Text;
        //        string sodem = txtSoDem.Text;

        //        string updateQuery = $"UPDATE KHACHHANG SET " +
        //             $"TENKH = N'{ten}', " +
        //             $"SDT = '{sdt}', " +
        //             $"NUOC = N'{quoctich}', " +
        //             $"GIOITINH = N'{gioitinh}', " +
        //             $"MADD = N'{madd}', " +
        //             $"DIACHI = N'{diachi}', " + // ✅ Đã thêm dấu ,
        //             $"SODEM = '{sodem}' " +
        //             $"WHERE MAKH = {makh}";

        //        fn.setdata(updateQuery, "Thông tin khách hàng đã được cập nhật!");

        //        // Cập nhật lại bảng
        //        load();
        //        ClearInputs();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void ClearInputs()
        {
            txtMAKH.Clear();
            txtTENKH.Clear();
            txtSDT.Clear();
            txtQUOCTICH.Clear();
            cboGIOITINH.SelectedIndex = -1;
            txtMADD.Clear();
            txtDIACHI.Clear();
        }

        private void txtTENKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMADD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtMADD.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMADD.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uc_CustomerDetails_Load(object sender, EventArgs e)
        {
            query = "SELECT KHACHHANG.MAKH AS[Mã Khách Hàng], KHACHHANG.TENKH AS [Tên Khách Hàng], KHACHHANG.SDT AS [Số điện thoại], KHACHHANG.NUOC AS[Nước], KHACHHANG.GIOITINH AS[Giới Tính], KHACHHANG.MADD AS[Mã Định Danh], KHACHHANG.DIACHI AS[Địa Chỉ],KHACHHANG.SODEM AS[Số Đêm], KHACHHANG.CHECKIN AS[Ngày đặt phòng], KHACHHANG.CHECKOU AS[Ngày Thanh Toán], KHACHHANG.TONGTIEN AS[Tổng Tiền], PHONG.SOPHONG AS[Số Phòng], PHONG.LOAIPHONG AS[Loại Phòng], PHONG.GIUONG AS[Giường], PHONG.GIA AS[Gía] FROM KHACHHANG INNER JOIN PHONG ON KHACHHANG.MAPHONG = PHONG.MAPHONG WHERE CHECKOU IS NULL";
            getrecord(query);
        }

        public void load()
        {
            query = "SELECT KHACHHANG.MAKH AS[Mã Khách Hàng], KHACHHANG.TENKH AS [Tên Khách Hàng], KHACHHANG.SDT AS [Số điện thoại], KHACHHANG.NUOC AS[Nước], KHACHHANG.GIOITINH AS[Giới Tính], KHACHHANG.MADD AS[Mã Định Danh], KHACHHANG.DIACHI AS[Địa Chỉ],KHACHHANG.SODEM AS[Số Đêm], KHACHHANG.CHECKIN AS[Ngày đặt phòng], KHACHHANG.CHECKOU AS[Ngày Thanh Toán], KHACHHANG.TONGTIEN AS[Tổng Tiền], PHONG.SOPHONG AS[Số Phòng], PHONG.LOAIPHONG AS[Loại Phòng], PHONG.GIUONG AS[Giường], PHONG.GIA AS[Gía] FROM KHACHHANG INNER JOIN PHONG ON KHACHHANG.MAPHONG = PHONG.MAPHONG WHERE CHECKOU IS NULL";
            getrecord(query);
        }

        private void txtMAKH_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtMAKH.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMAKH.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtQUOCTICH_TextChanged(object sender, EventArgs e)
        {
            string input = txtQUOCTICH.Text;

            foreach (char c in input)
            {
                // CHỈ cho nhập chữ cái (có dấu) và khoảng trắng
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái tiếng Việt và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQUOCTICH.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }

        private void txtTENKH_TextChanged_1(object sender, EventArgs e)
        {
            string input = txtQUOCTICH.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQUOCTICH.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtSDT.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtDIACHI_TextChanged(object sender, EventArgs e)
        {
            string input = txtQUOCTICH.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQUOCTICH.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }

        private void txtSoDem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtSDT.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Text = ""; // Xóa dữ liệu sai
            }
        }
      
    }
  
}
