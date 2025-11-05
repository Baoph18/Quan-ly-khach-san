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
    public partial class uc_HoaDon: UserControl
    {
        Function fn = new Function();
        string query;
        public uc_HoaDon()
        {
            InitializeComponent();
           
        }

        public void LoadHoaDon()
        {
            string query = @"
        SELECT 
            'HD' + RIGHT('000' + CAST(MAHD AS VARCHAR), 3) AS [Mã HĐ],
            KH.TENKH AS [Khách hàng],
            NV.TENNV AS [Nhân viên],
            P.SOPHONG AS [Phòng],
            HD.NGAYTHANHTOAN AS [Ngày thanh toán],
            HD.TONGTIEN AS [Tổng tiền],
            HD.PHUONGTHUCTT AS [Phương thức]
        FROM HOADON HD
        LEFT JOIN KHACHHANG KH ON HD.MAKH = KH.MAKH
        LEFT JOIN NHANVIEN NV ON HD.MANV = NV.MANV
        LEFT JOIN PHONG P ON HD.MAPHONG = P.MAPHONG
    ";

            DataSet ds = fn.getdata(query);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
               

            }
            else
            {
                dataGridView1.DataSource = null; // hoặc dùng DataTable rỗng
                MessageBox.Show("Không có dữ liệu hóa đơn để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void uc_HoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
        }

       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Lấy dữ liệu từ cột
                string mahd = row.Cells["Mã HĐ"].Value?.ToString();
                string tongtien = row.Cells["Tổng tiền"].Value?.ToString();

                // Gán lên label
                labelMaHD.Text =  mahd;
                labelTongTien.Text =  tongtien + " VNĐ";
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ngay = dtNgayThanhToan.Value.Date;

                string query = $@"
            SELECT 
                'HD' + RIGHT('000' + CAST(MAHD AS VARCHAR), 3) AS [Mã HĐ],
                KH.TENKH AS [Khách hàng],
                NV.TENNV AS [Nhân viên],
                P.SOPHONG AS [Phòng],
                HD.NGAYTHANHTOAN AS [Ngày thanh toán],
                HD.TONGTIEN AS [Tổng tiền],
                HD.PHUONGTHUCTT AS [Phương thức]
            FROM HOADON HD
            JOIN KHACHHANG KH ON HD.MAKH = KH.MAKH
            JOIN NHANVIEN NV ON HD.MANV = NV.MANV
            JOIN PHONG P ON HD.MAPHONG = P.MAPHONG
            WHERE CAST(HD.NGAYTHANHTOAN AS DATE) = '{ngay:yyyy-MM-dd}'
        ";

                DataSet ds = fn.getdata(query);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc hóa đơn theo ngày: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

       

        private void labelTongTien_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ThongKe tk = new ThongKe();
            tk.Show();
        }

        
    }
}
