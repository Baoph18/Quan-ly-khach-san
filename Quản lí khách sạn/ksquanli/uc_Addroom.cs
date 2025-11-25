using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace Quản_lí_khách_sạn.ksquanli
{
    public partial class uc_Addroom : UserControl
    {
        Function fn = new Function();
        string query;
        public uc_Addroom()
        {
            InitializeComponent();
        }
        
        private void uc_Addroom_Load(object sender, EventArgs e)
        {
            query = "select MAPHONG AS [Mã Phòng], SOPHONG AS [Số Phòng],LOAIPHONG AS [Loại Phòng], GIUONG AS [Giường], GIA AS [Gía], DATPHONG AS [Trạng thái đặt phòng] from PHONG";
            DataSet ds = fn.getdata(query);
            Datagridview.DataSource = ds.Tables[0];
           

        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {


            //if ( txtSophong.Text != "" && txtLoaiphong.Text != "" && txtLoaigiuong.Text != "" && txtGiatien.Text != "")
            //{         
            //    // gán vào biến sophong
            //    String sophong = txtSophong.Text;
            //    String loaiphong = txtLoaiphong.Text;
            //    String loaigiuong = txtLoaigiuong.Text;
            //    Int64 giatien = Int64.Parse(txtGiatien.Text);

            //    // 🔍 Kiểm tra trùng số phòng
            //    string checkQuery = $"SELECT COUNT(*) FROM PHONG WHERE SOPHONG = '{sophong}'";
            //    DataSet dsCheck = fn.getdata(checkQuery);
            //    int count = Convert.ToInt32(dsCheck.Tables[0].Rows[0][0]);

            //    if (count > 0)
            //    {
            //        MessageBox.Show("Số phòng đã tồn tại! Vui lòng nhập số phòng khác.", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        txtSophong.Focus();
            //        return;
            //    }
            //    query = "insert into PHONG (SOPHONG, LOAIPHONG, GIUONG, GIA) values ('" + sophong + "', N'" + loaiphong + "', '" + loaigiuong + "', '" + giatien + "')";
            //    fn.setdata(query, "Đã thêm phòng");

            //    uc_Addroom_Load(this, null);
            //    clearAll();

            //}
            //else
            //{
            //    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

            try // Thực thi logic thêm phòng
            {
                
                string sophong = txtSophong.Text.Trim();
                string loaiphong = txtLoaiphong.Text.Trim();
                string loaigiuong = txtLoaigiuong.Text.Trim();
                long giatien = ParseGiaTien(txtGiatien.Text); // có throw

                if (IsRoomExist(sophong))
                {
                    throw new ApplicationException("Số phòng đã tồn tại! Vui lòng nhập số phòng khác.");
                }

                AddRoomToDatabase(sophong, loaiphong, loaigiuong, giatien);

                MessageBox.Show("Thêm phòng thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                uc_Addroom_Load(this, null);
            }
            catch (FormatException ex)  // Bắt lỗi định dạng dữ liệu (nhập sai số, sai kiểu)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message,
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ApplicationException ex)  // Bắt lỗi nghiệp vụ do mình tự ném (ví dụ: số phòng đã tồn tại)
            {
                MessageBox.Show(ex.Message,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // Bắt tất cả các lỗi còn lại (lỗi hệ thống)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Luôn chạy dù có lỗi hay không
                clearAll();
            }
        }



        private long ParseGiaTien(string text)
        {
            if (!long.TryParse(text, out long giatien))
            {
                throw new FormatException("Giá tiền phải là số nguyên.");
            }
            return giatien;
        }

        private bool IsRoomExist(string sophong)
        {
            string query = $"SELECT COUNT(*) FROM PHONG WHERE SOPHONG = '{sophong}'";
            DataSet ds = fn.getdata(query);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }


        private bool IsInputValid()
        {
            return !string.IsNullOrWhiteSpace(txtSophong.Text) &&
                   !string.IsNullOrWhiteSpace(txtLoaiphong.Text) &&
                   !string.IsNullOrWhiteSpace(txtLoaigiuong.Text) &&
                   !string.IsNullOrWhiteSpace(txtGiatien.Text);
        }


        private void AddRoomToDatabase(string sophong, string loaiphong, string loaigiuong, long giatien)
        {
            string insertQuery =
                "INSERT INTO PHONG (SOPHONG, LOAIPHONG, GIUONG, GIA) VALUES " +
                $"('{sophong}', N'{loaiphong}', '{loaigiuong}', '{giatien}')";

            try
            {
                fn.setdata(insertQuery, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể thêm phòng vào cơ sở dữ liệu. Chi tiết: " + ex.Message);
            }
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
            //if (selectedRoomId == -1)
            //{
            //    MessageBox.Show("Vui lòng chọn phòng cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //if (txtSophong.Text != "" && txtLoaiphong.Text != "" && txtLoaigiuong.Text != "" && txtGiatien.Text != "")
            //{
            //    string sophong = txtSophong.Text;
            //    string loaiphong = txtLoaiphong.Text;
            //    string loaigiuong = txtLoaigiuong.Text;

            //    long gia = long.Parse(txtGiatien.Text);

            //    string query = $"UPDATE PHONG SET SOPHONG = '{sophong}', LOAIPHONG = '{loaiphong}', GIUONG = '{loaigiuong}', GIA = {gia} WHERE MAPHONG = {selectedRoomId}";
            //    fn.setdata(query, "Cập nhật thông tin phòng thành công!");

            //    uc_Addroom_Load(this, null);
            //    clearAll();
            //    selectedRoomId = -1; // reset lại

            //}
            //else
            //{
            //    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            try // Logic chính: kiểm tra phòng, cập nhật DB, reset form
            {
                if (!IsRoomSelected())
                    return;

                UpdateRoom();

                ResetForm();
            }
            catch (FormatException ex) // Bắt lỗi sai định dạng dữ liệu (nhập chữ thay vì số,...)
            {
                MessageBox.Show($"Lỗi định dạng dữ liệu: {ex.Message}",
                                "Lỗi nhập liệu",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (SqlException ex) // Bắt lỗi liên quan đến database (connection, câu lệnh SQL, khóa,...)
            {
                MessageBox.Show($"Lỗi SQL: {ex.Message}",
                                "Lỗi hệ thống",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)  // Bắt các lỗi còn lại (lỗi không xác định)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                // luôn chạy dù có lỗi hay không
                Console.WriteLine("btnRepair_Click đã kết thúc.");
            }
        }

        private bool IsRoomSelected()
        {
            if (selectedRoomId == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng cần chỉnh sửa.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtSophong.Text) ||
        string.IsNullOrWhiteSpace(txtLoaiphong.Text) ||
        string.IsNullOrWhiteSpace(txtLoaigiuong.Text) ||
        string.IsNullOrWhiteSpace(txtGiatien.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!long.TryParse(txtGiatien.Text, out _))
            {
                MessageBox.Show("Giá tiền phải là số hợp lệ!", "Lỗi",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        public void setDataWithParameters(string query, Dictionary<string, object> parameters, string message)
        {
            SqlConnection con = new SqlConnection("your_connection_string_here");
            SqlCommand cmd = null;

            try  // Mở kết nối, tạo command, thêm parameters và thực thi query
            {
                con.Open();
                cmd = new SqlCommand(query, con);

                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                cmd.ExecuteNonQuery();

                if (!string.IsNullOrEmpty(message))
                {
                    MessageBox.Show(message, "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex) // Bắt lỗi liên quan đến SQL: cú pháp, kết nối, trùng khóa,...
            {
                throw new Exception("Lỗi truy vấn SQL: " + ex.Message);
            }
            catch (Exception) // Bắt các lỗi còn lại và ném nguyên lỗi ra ngoài (giữ stacktrace)
            {
                throw; // giữ nguyên lỗi
            }
            finally  // Luôn chạy: giải phóng SqlCommand và đóng SqlConnection
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        private void UpdateRoom()
        {
            try
            {
                string query = @"UPDATE PHONG 
                         SET SOPHONG=@sophong, LOAIPHONG=@loaiphong, 
                             GIUONG=@giuong, GIA=@gia 
                         WHERE MAPHONG=@id";

                Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"@sophong", txtSophong.Text },
            {"@loaiphong", txtLoaiphong.Text },
            {"@giuong", txtLoaigiuong.Text },
            {"@gia", long.Parse(txtGiatien.Text) },
            {"@id", selectedRoomId }
        };

                fn.setDataWithParameters(query, parameters, "Cập nhật thông tin phòng thành công!");
            }
            catch
            {
                // ném lỗi lên cho btnRepair_Click xử lý
                throw;
            }
        }


        private void ResetForm()
        {
            uc_Addroom_Load(this, null);
            clearAll();
            selectedRoomId = -1;
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
            //// Kiểm tra nếu không có dữ liệu
            //if (Datagridview.Rows.Count == 0)
            //{
            //    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //// Tạo ứng dụng Excel
            //Excel.Application excelApp = new Excel.Application();
            //if (excelApp == null)
            //{
            //    MessageBox.Show("Excel chưa được cài đặt trên máy tính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //// tạo mới , lấy workship đầu tiên
            //Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            //Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
            //worksheet.Name = "DanhSachPhong";

            //// Đặt tiêu đề cột
            //worksheet.Cells[1, 1] = "Mã phòng";
            //worksheet.Cells[1, 2] = "Loại phòng";

            //worksheet.Cells[1, 3] = "Loại giường";
            //worksheet.Cells[1, 4] = "Giá tiền";

            //// Xuất dữ liệu từ DataGridView vào Excel
            //for (int i = 0; i < Datagridview.Rows.Count; i++)
            //{
            //    worksheet.Cells[i + 2, 1] = Datagridview.Rows[i].Cells["MAPHONG"].Value?.ToString();
            //    worksheet.Cells[i + 2, 2] = Datagridview.Rows[i].Cells["LOAIPHONG"].Value?.ToString();

            //    worksheet.Cells[i + 2, 4] = Datagridview.Rows[i].Cells["GIUONG"].Value?.ToString();
            //    worksheet.Cells[i + 2, 5] = Datagridview.Rows[i].Cells["GIA"].Value?.ToString();
            //}

            //// Hộp thoại lưu file
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            //saveFileDialog.FileName = "DanhSachPhong.xlsx";

            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    workbook.SaveAs(saveFileDialog.FileName);
            //    workbook.Close();
            //    excelApp.Quit();

            //    // Giải phóng bộ nhớ thư viện excel chạy ngầm 
            //    Marshal.ReleaseComObject(worksheet);
            //    Marshal.ReleaseComObject(workbook);
            //    Marshal.ReleaseComObject(excelApp);

            //    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            if (!HasDataToExport())
                return;

            Excel.Application excelApp = CreateExcelApp();
            if (excelApp == null)
                return;

            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet sheet = InitializeWorksheet(workbook, "DanhSachPhong"); 

            WriteHeader(sheet);
            WriteData(sheet);

            SaveWorkbook(workbook, excelApp, sheet);
        }


        private bool HasDataToExport()
        {
            if (Datagridview.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        private Excel.Application CreateExcelApp()
        {
            Excel.Application app = new Excel.Application();

            if (app == null)
            {
                MessageBox.Show("Excel chưa được cài đặt!",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return null;
            }

            return app;
        }


        private Excel.Worksheet InitializeWorksheet(Excel.Workbook workbook, string sheetName)
        {
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.ActiveSheet;
            sheet.Name = sheetName;
            return sheet;
        }
        private void WriteHeader(Excel.Worksheet sheet)
        {
            sheet.Cells[1, 1] = "Mã phòng";
            sheet.Cells[1, 2] = "Loại phòng";
            sheet.Cells[1, 3] = "Loại giường";
            sheet.Cells[1, 4] = "Giá tiền";
        }



        private void WriteData(Excel.Worksheet sheet)
        {
            for (int i = 0; i < Datagridview.Rows.Count; i++)
            {
                sheet.Cells[i + 2, 1] = Datagridview.Rows[i].Cells[0].Value?.ToString();
                sheet.Cells[i + 2, 2] = Datagridview.Rows[i].Cells[2].Value?.ToString();
                sheet.Cells[i + 2, 3] = Datagridview.Rows[i].Cells[3].Value?.ToString();
                sheet.Cells[i + 2, 4] = Datagridview.Rows[i].Cells[4].Value?.ToString();
            }
        }


        private void SaveWorkbook(Excel.Workbook workbook, Excel.Application app, Excel.Worksheet sheet)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            dialog.FileName = "DanhSachPhong.xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(dialog.FileName);

                workbook.Close();
                app.Quit();

                ReleaseComObject(sheet);
                ReleaseComObject(workbook);
                ReleaseComObject(app);

                MessageBox.Show("Xuất file Excel thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }


        private void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null)
                    Marshal.ReleaseComObject(obj);
            }
            catch { }
            finally
            {
                obj = null;
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
    }
}
