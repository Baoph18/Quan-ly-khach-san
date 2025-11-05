using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lí_khách_sạn
{
    public partial class ThongKe : Form
    {
        Function fn = new Function();
        public ThongKe()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            
        }


        private void LoadReport()
        {
            try
            {
                string query = "SELECT * FROM V_HOADON_VIEW"; // hoặc view bạn đang dùng
                DataSet ds = fn.getdata(query);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CrystalReport1 rpt = new CrystalReport1();  // đây là file .rpt đã thiết kế
                    rpt.SetDataSource(ds.Tables[0]);          // gán dữ liệu từ DataSet
                    crystalReportViewer1.ReportSource = rpt;  // gán vào Viewer để hiển thị
                    crystalReportViewer1.Refresh();           // làm mới để hiển thị ngay
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load báo cáo: " + ex.Message);
            }
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {
            // ✅ Đặt trước thời gian lọc mặc định
            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtTo.Value = DateTime.Now;

            LoadReport(); // ✅ Gọi hàm riêng để load báo cáo
        }
        private void btnLoc_Click_1(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate = dtFrom.Value.Date;
                DateTime toDate = dtTo.Value.Date;

                if (fromDate > toDate)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = $@"
            SELECT 'HD' + RIGHT('000' + CAST(MAHD AS VARCHAR), 3) AS MAHD_HIEN,
                    KH.TENKH, P.SOPHONG, NV.TENNV AS TENNHANVIEN,
                   HD.NGAYTHANHTOAN, HD.TONGTIEN, HD.PHUONGTHUCTT
            FROM HOADON HD
            JOIN KHACHHANG KH ON HD.MAKH = KH.MAKH
            JOIN PHONG P ON HD.MAPHONG = P.MAPHONG
            JOIN NHANVIEN NV ON HD.MANV = NV.MANV
            WHERE HD.NGAYTHANHTOAN BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";

                DataSet ds = fn.getdata(query);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CrystalReport1 rpt = new CrystalReport1(); // báo cáo đã thiết kế
                    rpt.SetDataSource(ds.Tables[0]);
                    crystalReportViewer1.ReportSource = rpt;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void crystalReportViewer1_Load_1(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_Load_2(object sender, EventArgs e)
        {

        }
    }
}
