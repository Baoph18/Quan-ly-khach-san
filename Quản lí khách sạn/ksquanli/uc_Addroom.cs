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
    
    public partial class uc_Addroom : UserControl
    {
        Function fn = new Function();
        string query;
        public uc_Addroom()
        {
            InitializeComponent();
            XmlConfigurator.Configure(new FileInfo("Log4net.config"));
        }
        
        private void uc_Addroom_Load(object sender, EventArgs e)
        {
            query = "select MAPHONG AS [Mã Phòng], SOPHONG AS [Số Phòng],LOAIPHONG AS [Loại Phòng], GIUONG AS [Giường], GIA AS [Gía], DATPHONG AS [Trạng thái đặt phòng] from PHONG";
            DataSet ds = fn.getdata(query);
            Datagridview.DataSource = ds.Tables[0];
           

        }

        private readonly Function _function = new Function();
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRoomInputValid())
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sophong = txtSophong.Text.Trim();
                string loaiphong = txtLoaiphong.Text.Trim();
                string loaigiuong = txtLoaigiuong.Text.Trim();

                // chuyển giá tiền → có thể gây FormatException
                long giatien = long.Parse(txtGiatien.Text.Trim());

                // kiểm tra trùng phòng → lỗi nghiệp vụ
                if (IsDuplicateRoomNumber(sophong))
                {
                    throw new ApplicationException("Số phòng đã tồn tại! Vui lòng nhập số khác.");
                }

                string insertQuery = $"INSERT INTO PHONG (SOPHONG, LOAIPHONG, GIUONG, GIA) " +
                                     $"VALUES ('{sophong}', N'{loaiphong}', N'{loaigiuong}', '{giatien}')";

                _function.setdata(insertQuery, "Đã thêm phòng thành công!");

                uc_Addroom_Load(this, null);
            }
            catch (FormatException ex)  // nhập sai kiểu số
            {
                MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message,
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ApplicationException ex)  // lỗi nghiệp vụ do mình throw
            {
                MessageBox.Show(ex.Message,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)  // lỗi hệ thống
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                clearAll(); // luôn chạy
            }
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = Datagridview.Rows[e.RowIndex];

                if (!row.IsNewRow && row.Cells[0].Value != DBNull.Value)
                {
                    selectedRoomId = Convert.ToInt32(row.Cells[0].Value); // MAPHONG
                    txtSophong.Text = row.Cells[1].Value?.ToString();
                    txtLoaiphong.Text = row.Cells[2].Value?.ToString();
                    txtLoaigiuong.Text = row.Cells[3].Value?.ToString();
                    txtGiatien.Text = row.Cells[4].Value?.ToString();
                }
            }
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraDaChonPhong())
                    throw new ApplicationException("Chưa chọn phòng để sửa.");

                if (!KiemTraDuLieuNhap())
                    throw new ApplicationException("Dữ liệu nhập không hợp lệ.");

                CapNhatPhong();
                LamMoiSauKhiSua();
            }
            catch (FormatException ex)   // Lỗi định dạng số (giá tiền)
            {
                
                MessageBox.Show("Lỗi định dạng số: " + ex.Message,
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ApplicationException ex)  // lỗi nghiệp vụ
            {
                
                MessageBox.Show(ex.Message,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)  // lỗi hệ thống khác
            {
                
                MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // luôn chạy dù có lỗi hay không
                clearAll();
                selectedRoomId = -1;
            }
        }

        private bool KiemTraDaChonPhong()
        {
            if (selectedRoomId == -1)
            {
                
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


            string query =
                $"UPDATE PHONG SET SOPHONG='{sophong}', LOAIPHONG='{loaiphong}', GIUONG='{loaigiuong}', GIA={gia} WHERE MAPHONG={selectedRoomId}";


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
            for (int i = 0; i < Datagridview.Rows.Count - 1; i++)
            {
                worksheet.Cells[i + 2, 1] = Datagridview.Rows[i].Cells[0].Value?.ToString();
                worksheet.Cells[i + 2, 2] = Datagridview.Rows[i].Cells[1].Value?.ToString();
                worksheet.Cells[i + 2, 3] = Datagridview.Rows[i].Cells[2].Value?.ToString();
                worksheet.Cells[i + 2, 4] = Datagridview.Rows[i].Cells[3].Value?.ToString();
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
            {     // Lấy giá trị người dùng nhập
                  // Nếu không đúng định dạng, tự tạo một lỗi để đưa xuống catch xử lý
                string input = txtSophong.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    // throw dùng để ném lỗi một cách chủ động
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                // catch sẽ bắt lỗi được ném từ throw hoặc lỗi hệ thống
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSophong.Text = ""; // Sau khi báo lỗi, xóa dữ liệu sai để người dùng nhập lại
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

        private void Datagridview_SelectionChanged(object sender, EventArgs e)
        {
            if (Datagridview.CurrentRow != null && !Datagridview.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = Datagridview.CurrentRow;

                if (row.Cells[0].Value != null)
                {
                    selectedRoomId = Convert.ToInt32(row.Cells[0].Value);
                }

                txtSophong.Text = row.Cells[1].Value?.ToString();
            }
        }
    }
}
