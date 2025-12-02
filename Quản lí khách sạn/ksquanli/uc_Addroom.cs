using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using log4net.Repository.Hierarchy;
using log4net.Config;

namespace Quản_lí_khách_sạn.ksquanli
{
    //ègdgdf
    public static class Logger
    {
        private static readonly string path = "log.txt";

        private static void Write(string level, string message)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string log = $"{time} [{level}] {message}";
            File.AppendAllText(path, log + Environment.NewLine);
        }

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Warn(string message)
        {
            Write("WARN", message);
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }
    }
    public partial class uc_Addroom : UserControl
    {
        Function fn = new Function();
        string query;
        public uc_Addroom()
        {
            InitializeComponent();
        }
        //dương gay LGBT 
        private void uc_Addroom_Load(object sender, EventArgs e)
        {
            query = "select MAPHONG AS [Mã Phòng], SOPHONG AS [Số Phòng],LOAIPHONG AS [Loại Phòng], GIUONG AS [Giường], GIA AS [Gía], DATPHONG AS [Trạng thái đặt phòng] from PHONG";
            DataSet ds = fn.getdata(query);
            Datagridview.DataSource = ds.Tables[0];
           

        }

        private readonly Function _function = new Function();
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if (!IsRoomInputValid())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sophong = txtSophong.Text;
            string loaiphong = txtLoaiphong.Text;
            string loaigiuong = txtLoaigiuong.Text;
            long giatien = long.Parse(txtGiatien.Text);

            if (IsDuplicateRoomNumber(sophong))
            {
                MessageBox.Show("Số phòng đã tồn tại! Vui lòng nhập số khác.", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSophong.Focus();
                return;
            }

            string insertQuery = $"INSERT INTO PHONG (SOPHONG, LOAIPHONG, GIUONG, GIA) " +
                                 $"VALUES ('{sophong}', N'{loaiphong}', '{loaigiuong}', '{giatien}')";
            _function.setdata(insertQuery, "Đã thêm phòng thành công!");

            uc_Addroom_Load(this, null);
            clearAll();

        }

        private bool IsRoomInputValid()
        {
            return !string.IsNullOrWhiteSpace(txtSophong.Text)
                && !string.IsNullOrWhiteSpace(txtLoaiphong.Text)
                && !string.IsNullOrWhiteSpace(txtLoaigiuong.Text)
                && !string.IsNullOrWhiteSpace(txtGiatien.Text);
        }

        private bool IsDuplicateRoomNumber(string sophong)
        {
            string checkQuery = $"SELECT COUNT(*) FROM PHONG WHERE SOPHONG = '{sophong}'";
            DataSet dsCheck = _function.getdata(checkQuery);
            int count = Convert.ToInt32(dsCheck.Tables[0].Rows[0][0]);
            return count > 0;
        }

        public void clearAll()
        {
            txtSophong.Clear();
            txtLoaiphong.SelectedIndex = -1;
            txtLoaigiuong.SelectedIndex = -1;
            txtGiatien.Clear();
            selectedRoomId = -1;

        }

        private void uc_Addroom_Leave(object sender, EventArgs e) 
        {
            clearAll();
        }

        private void uc_Addroom_Enter(object sender, EventArgs e)
        {
            uc_Addroom_Load(this, null);
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

       
        int selectedRoomId = -1; // MAPHONG của phòng đang được chọn
      

        private void Datagridview1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && Datagridview.Rows[e.RowIndex].Cells[0].Value != null)
            {
                // lấy dữ liệu từ dòng đc chọn gán vào biến row
                DataGridViewRow row = Datagridview.Rows[e.RowIndex];

                selectedRoomId = Convert.ToInt32(row.Cells[0].Value); // MAPHONG
                txtSophong.Text = row.Cells[1].Value.ToString();      // SOPHONG
                txtLoaiphong.Text = row.Cells[2].Value.ToString();    // LOAIPHONG
                txtLoaigiuong.Text = row.Cells[3].Value.ToString();   // GIUONG
                txtGiatien.Text = row.Cells[4].Value.ToString();      // GIA
            }
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            try
            {
                
                XmlConfigurator.Configure(new FileInfo("Log4net.config"));

                if (!KiemTraDaChonPhong()) return;
                if (!KiemTraDuLieuNhap()) return;

                CapNhatPhong();
                LamMoiSauKhiSua();
            }
            catch (Exception ex)
            {
                XuLyLoi(ex);
            }
        }

        private bool KiemTraDaChonPhong()
        {
            if (selectedRoomId == -1)
            {
                Logger.Error("Người dùng nhấn sửa nhưng chưa chọn phòng.");
                MessageBox.Show("Vui lòng chọn phòng cần chỉnh sửa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool KiemTraDuLieuNhap()
        {
            if (txtSophong.Text == "" || txtLoaiphong.Text == "" ||
                txtLoaigiuong.Text == "" || txtGiatien.Text == "")
            {
                Logger.Warn("Người dùng nhập thiếu dữ liệu khi sửa phòng.");
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void CapNhatPhong()
        {
            string sophong = txtSophong.Text;
            string loaiphong = txtLoaiphong.Text;
            string loaigiuong = txtLoaigiuong.Text;
            long gia = long.Parse(txtGiatien.Text);

            Logger.Info($"Dữ liệu nhập: SOPHONG={sophong}, LOAIPHONG={loaiphong}, LOAIGIUONG={loaigiuong}, GIA={gia}");

            string query =
                $"UPDATE PHONG SET SOPHONG='{sophong}', LOAIPHONG='{loaiphong}', GIUONG='{loaigiuong}', GIA={gia} WHERE MAPHONG={selectedRoomId}";

            Logger.Info("Sửa thông tin phòng thành công: " + query);

            fn.setdata(query, "Cập nhật thông tin phòng thành công!");
        }

        private void LamMoiSauKhiSua()
        {
            uc_Addroom_Load(this, null);
            clearAll();
            selectedRoomId = -1;

           
        }

        private void XuLyLoi(Exception ex)
        {
            Logger.Error("Lỗi khi sửa phòng: " + ex.Message);
            Logger.Error("StackTrace: " + ex.StackTrace);

            MessageBox.Show("Đã xảy ra lỗi khi cập nhật phòng!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void txtLoaigiuong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtLoaiphong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Datagridview1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
        
        private void Datagridview_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

        }

        private void btnexporttoexel_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu không có dữ liệu
            if (Datagridview.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo ứng dụng Excel
            Excel.Application excelApp = new Excel.Application();
            if (excelApp == null)
            {
                MessageBox.Show("Excel chưa được cài đặt trên máy tính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // tạo mới , lấy workship đầu tiên
            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "DanhSachPhong";

            // Đặt tiêu đề cột
            worksheet.Cells[1, 1] = "Mã phòng";
            worksheet.Cells[1, 2] = "Loại phòng";
            
            worksheet.Cells[1, 3] = "Loại giường";
            worksheet.Cells[1, 4] = "Giá tiền";

            // Xuất dữ liệu từ DataGridView vào Excel
            for (int i = 0; i < Datagridview.Rows.Count; i++)
            {
                worksheet.Cells[i + 2, 1] = Datagridview.Rows[i].Cells["MAPHONG"].Value?.ToString();
                worksheet.Cells[i + 2, 2] = Datagridview.Rows[i].Cells["LOAIPHONG"].Value?.ToString();
                
                worksheet.Cells[i + 2, 4] = Datagridview.Rows[i].Cells["GIUONG"].Value?.ToString();
                worksheet.Cells[i + 2, 5] = Datagridview.Rows[i].Cells["GIA"].Value?.ToString();
            }

            // Hộp thoại lưu file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "DanhSachPhong.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(saveFileDialog.FileName);
                workbook.Close();
                excelApp.Quit();

                // Giải phóng bộ nhớ thư viện excel chạy ngầm 
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);

                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtSophong_TextChanged(object sender, EventArgs e)
        {
            try
            {

                string input = txtSophong.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    XmlConfigurator.Configure(new FileInfo("Log4net.config"));
                    Logger.Warn("Sai định dạng.Yêu cầu nhập số,không được nhập chữ");
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSophong.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtGiatien_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtGiatien.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiatien.Text = ""; // Xóa dữ liệu sai
            }
        }
        public void load()
        {
            query = "select MAPHONG AS [Mã Phòng], SOPHONG AS [Số Phòng],LOAIPHONG AS [Loại Phòng], GIUONG AS [Giường], GIA AS [Gía], DATPHONG AS [Trạng thái đặt phòng] from PHONG";
            DataSet ds = fn.getdata(query);
            Datagridview.DataSource = ds.Tables[0];
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
