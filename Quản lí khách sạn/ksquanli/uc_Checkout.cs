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
    public partial class uc_Checkout: UserControl
    {
        Function fn = new Function();
        string query;

        public uc_Checkout()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void uc_Checkout_Load(object sender, EventArgs e)
        {
            query = "select KHACHHANG.MAKH AS [Mã khách hàng], KHACHHANG.TENKH AS [Tên khách hàng], KHACHHANG.SDT AS [Số điện thoại], KHACHHANG.NUOC AS [Nước], KHACHHANG.GIOITINH AS[Giới Tính], KHACHHANG.MADD AS [Mã Định Danh], KHACHHANG.DIACHI AS [Địa Chỉ], KHACHHANG.CHECKIN AS[Ngày Đặt Phòng], KHACHHANG.TONGTIEN AS[Tổng Tiền], PHONG.SOPHONG AS [Số Phòng], PHONG.LOAIPHONG AS[Loại Phòng], KHACHHANG.SODEM AS[Số Đêm], PHONG.GIUONG AS[Giường], PHONG.GIA AS[Gía] from KHACHHANG inner join PHONG on KHACHHANG.MAPHONG = PHONG.MAPHONG where CHEKOU = 'NO'";
            DataSet ds = fn.getdata(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

       

        int id;
        /// <summary>
        /// //////////////////////////////////////////////////////////
        /// </summary>
        public class ThanhToanInfo
        {
            public int MaKhachHang { get; set; }
            public int MaNhanVien { get; set; }
            public string SoPhong { get; set; }
            public string PhuongThuc { get; set; }
            public DateTime NgayCheckout { get; set; }
            public decimal TongTien { get; set; }
        }

        private bool XacNhanThanhToan()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Không có khách hàng để thanh toán!", "Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn thanh toán?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return false;

            return true;
        }

        private int LayMaPhongTheoSoPhong(string soPhong)
        {
            string query = $"SELECT MAPHONG FROM PHONG WHERE SOPHONG = '{soPhong}'";
            DataSet dsPhong = fn.getdata(query);

            if (dsPhong.Tables[0].Rows.Count == 0)
                throw new Exception($"Không tìm thấy phòng '{soPhong}'.");

            return Convert.ToInt32(dsPhong.Tables[0].Rows[0]["MAPHONG"]);
        }

        private void ThucHienThanhToan(ThanhToanInfo info, int maPhong)
        {
            string checkoutStr = info.NgayCheckout.ToString("yyyy-MM-dd");

            string query = $@"
            UPDATE KHACHHANG 
            SET CHEKOU = 'YES', CHECKOU = '{checkoutStr}', TONGTIEN = {info.TongTien}
            WHERE MAKH = {info.MaKhachHang};

            UPDATE PHONG 
            SET DATPHONG = 'NO' 
            WHERE MAPHONG = {maPhong};

            INSERT INTO HOADON (MAKH, MANV, MAPHONG, NGAYTHANHTOAN, TONGTIEN, PHUONGTHUCTT)
            VALUES ({info.MaKhachHang}, {info.MaNhanVien}, {maPhong}, '{checkoutStr}', {info.TongTien}, N'{info.PhuongThuc}');
              ";

            fn.setdata(query, "Thanh toán & cập nhật dữ liệu thành công!");
        }
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (!XacNhanThanhToan())
                return;

            try
            {
                // Gom tham số vào đối tượng ThanhToanInfo
                ThanhToanInfo info = new ThanhToanInfo
                {
                    MaKhachHang = id,
                    MaNhanVien = CurrentUser.Id,
                    SoPhong = txtRoomNo.Text.Trim(),
                    PhuongThuc = cboPhuongthuc.Text.Trim(),
                    NgayCheckout = txtCheckout.Value,
                    TongTien = decimal.Parse(txtTongSoTien.Text.Replace(" VNĐ", "").Replace(",", "").Trim())
                };

                int maPhong = LayMaPhongTheoSoPhong(info.SoPhong);
                ThucHienThanhToan(info, maPhong);

                // Làm mới giao diện
                uc_Checkout_Load(this, null);
                clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        //private void btnThanhToan_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtName.Text))
        //    {
        //        MessageBox.Show("Không có khách hàng để thanh toán!", "Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    if (MessageBox.Show("Bạn có chắc chắn muốn thanh toán?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
        //        return;

        //    try
        //    {
        //        int maKhachHang = id;
        //        int maNhanVien = CurrentUser.Id;
        //        string soPhong = txtRoomNo.Text.Trim();
        //        string phuongThuc = cboPhuongthuc.Text.Trim();
        //        string checkout = txtCheckout.Value.ToString("MM/dd/yyyy");
        //        decimal tongTien = decimal.Parse(txtTongSoTien.Text.Replace(" VNĐ", "").Replace(",", "").Trim());

        //         //🔍 Lấy MAPHONG từ SOPHONG
        //        string getMaphongQuery = $"SELECT MAPHONG FROM PHONG WHERE SOPHONG = '{soPhong}'";
        //        DataSet dsPhong = fn.getdata(getMaphongQuery);

        //        if (dsPhong.Tables[0].Rows.Count == 0)
        //        {
        //            MessageBox.Show("Không tìm thấy phòng '" + soPhong + "'.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        //số nguyên 32 bit có dấu
        //        int maPhong = Convert.ToInt32(dsPhong.Tables[0].Rows[0]["MAPHONG"]);

        //        // 🧾 Cập nhật khách hàng
        //        string query = $@"
        //        UPDATE KHACHHANG 
        //        SET CHEKOU = 'YES', CHECKOU = '{checkout:yyyy-MM-dd}', TONGTIEN = {tongTien} 
        //        WHERE MAKH = {maKhachHang};

        //        UPDATE PHONG 
        //        SET DATPHONG = 'NO' 
        //        WHERE MAPHONG = {maPhong};

        //        INSERT INTO HOADON (MAKH, MANV, MAPHONG, NGAYTHANHTOAN, TONGTIEN, PHUONGTHUCTT)
        //        VALUES ({maKhachHang}, {maNhanVien}, {maPhong}, '{checkout:yyyy-MM-dd}', {tongTien}, N'{phuongThuc}');
        //        ";

        //       //  ✅ Gọi setdata 1 lần duy nhất
        //        fn.setdata(query, "Thanh toán & cập nhật dữ liệu thành công!");

        //        // 🔁 Làm mới giao diện
        //        uc_Checkout_Load(this, null);
        //        clearAll();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi thanh toán:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void clearAll()
        {
            txtSearchName.Clear();
            txtRoomNo.Clear();
            txtName.Clear();
            txtSoDem.Clear();
            txtTongSoTien.Clear();
            txtCheckout.ResetText();
        }

        private void uc_Checkout_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
            {
                id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtRoomNo.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();

                int soDem = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString());
                decimal gia = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString());

                txtSoDem.Text = soDem.ToString();
                decimal tongTien = soDem * gia;
                txtTongSoTien.Text = tongTien.ToString("N0") + " VNĐ"; // định dạng tiền Việt

            }
        }

        private void txtCheckout_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSoDem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtSoDem.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDem.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string input = txtName.Text;

            foreach (char c in input)
            {
                // CHỈ cho nhập chữ cái (có dấu) và khoảng trắng
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái tiếng Việt và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }

        private void txtTongSoTien_TextChanged(object sender, EventArgs e)
        {

        }
        public void Load()
        {
            query = "select KHACHHANG.MAKH AS [Mã khách hàng], KHACHHANG.TENKH AS [Tên khách hàng], KHACHHANG.SDT AS [Số điện thoại], KHACHHANG.NUOC AS [Nước], KHACHHANG.GIOITINH AS[Giới Tính], KHACHHANG.MADD AS [Mã Định Danh], KHACHHANG.DIACHI AS [Địa Chỉ], KHACHHANG.CHECKIN AS[Ngày Đặt Phòng], KHACHHANG.TONGTIEN AS[Tổng Tiền], PHONG.SOPHONG AS [Số Phòng], PHONG.LOAIPHONG AS[Loại Phòng], KHACHHANG.SODEM AS[Số Đêm], PHONG.GIUONG AS[Giường], PHONG.GIA AS[Gía] from KHACHHANG inner join PHONG on KHACHHANG.MAPHONG = PHONG.MAPHONG where CHEKOU = 'NO'"; 
            DataSet ds = fn.getdata(query);
            dataGridView1.DataSource = ds.Tables[0];
          

        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            query = "select KHACHHANG.MAKH AS [Mã khách hàng], KHACHHANG.TENKH AS [Tên khách hàng], KHACHHANG.SDT AS [Số điện thoại], KHACHHANG.NUOC AS [Nước], KHACHHANG.GIOITINH AS[Giới Tính], KHACHHANG.MADD AS [Mã Định Danh], KHACHHANG.DIACHI AS [Địa Chỉ], KHACHHANG.CHECKIN AS[Ngày Đặt Phòng], KHACHHANG.TONGTIEN AS[Tổng Tiền], PHONG.SOPHONG AS [Số Phòng], PHONG.LOAIPHONG AS[Loại Phòng], KHACHHANG.SODEM AS[Số Đêm], PHONG.GIUONG AS[Giường], PHONG.GIA AS[Gía] from KHACHHANG inner join PHONG on KHACHHANG.MAPHONG = PHONG.MAPHONG where TENKH like '" + txtSearchName.Text + "%' and CHEKOU = 'NO' "; // % chuỗi đi cùng LIKE 
            DataSet ds = fn.getdata(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
