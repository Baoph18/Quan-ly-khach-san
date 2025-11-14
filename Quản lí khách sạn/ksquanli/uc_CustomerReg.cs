using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Quản_lí_khách_sạn.ksquanli
{
    public partial class uc_CustomerReg: UserControl
    {
        Function fn = new Function();
        string query;
        public uc_CustomerReg()
        {
            InitializeComponent();
        }
        // đổ dữ liệu query vào combobox
        public void SetComboBox(string query, ComboBox combo)
        {
            combo.Items.Clear();       // Xóa các mục cũ
            combo.Text = "";           // Đặt lại giá trị hiển thị

            SqlDataReader sdr = fn.getforcombo(query);

            while (sdr.Read())
            {
                string value = sdr[0].ToString();     // Lấy cột đầu tiên (SOPHONG)
                if (!combo.Items.Contains(value))     // Kiểm tra chưa có mới thêm
                {
                    combo.Items.Add(value);
                }
            }

            sdr.Close();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void uc_CustomerReg_Load(object sender, EventArgs e)
        {

        }

       

       

        // Khai báo một biến toàn cục
        int rid;

       

        // Khi bấm nút thêm khách hàng thì sẽ truyền dữ liệu vào database
       
            public void clearAll()
        {
            txtName.Clear();
            txtContact.Clear();
            txtQuocTich.Clear();
            txtGioiTinh.SelectedIndex = -1;
            txtMaID.Clear();
            txtAddress.Clear();
            txtCheckin.ResetText();
            txtBed_Type.SelectedIndex = -1;
            txtRoom_type.SelectedIndex = -1;
            txtRoomNo.Items.Clear();
            txtPrice.Clear();
            txtSoDem.Clear();

        }

        private void uc_CustomerReg_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtQuocTich_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMaID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        //TẠO CLASS THAM SỐ 
        public class KhachHangAddInfo
        {
            public string Ten { get; set; }
            public long SDT { get; set; }
            public string QuocTich { get; set; }
            public string GioiTinh { get; set; }
            public string MaDD { get; set; }
            public string DiaChi { get; set; }
            public DateTime NgayCheckin { get; set; }
            public string SoPhong { get; set; }
            public string SoDem { get; set; }
            public int MaPhong { get; set; } // rid
        }

        // TÁCH HÀM 
        private bool XacNhanThemKhachHang()
        {
            if (txtName.Text == "" || txtContact.Text == "" || txtQuocTich.Text == "" ||
                txtGioiTinh.Text == "" || txtMaID.Text == "" || txtAddress.Text == "" ||
                txtPrice.Text == "" || txtSoDem.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private KhachHangAddInfo LayThongTinKhachHangMoi()
        {
            return new KhachHangAddInfo
            {
                Ten = txtName.Text.Trim(),
                SDT = long.Parse(txtContact.Text.Trim()),
                QuocTich = txtQuocTich.Text.Trim(),
                GioiTinh = txtGioiTinh.Text.Trim(),
                MaDD = txtMaID.Text.Trim(),
                DiaChi = txtAddress.Text.Trim(),
                NgayCheckin = txtCheckin.Value,
                SoPhong = txtRoomNo.Text.Trim(),
                SoDem = txtSoDem.Text.Trim(),
                MaPhong = rid
            };
        }

        private void ThemKhachHang(KhachHangAddInfo info)
        {
            string checkinStr = info.NgayCheckin.ToString("MM/dd/yyyy");

            string query = $@"
                INSERT INTO KHACHHANG (TENKH, SDT, NUOC, GIOITINH, MADD, DIACHI, SODEM, CHECKIN, MAPHONG)
                VALUES (N'{info.Ten}', '{info.SDT}', N'{info.QuocTich}', N'{info.GioiTinh}', 
                        N'{info.MaDD}', N'{info.DiaChi}', '{info.SoDem}', '{checkinStr}', {info.MaPhong});

                UPDATE PHONG 
                SET DATPHONG = 'YES' 
                WHERE SOPHONG = '{info.SoPhong}';
            ";

            fn.setdata(query, $"Khách hàng đã được đăng ký và phòng {info.SoPhong} đã được đánh dấu là đã đặt!");
        }

        private void btnAdd_Khachhang_Click_1(object sender, EventArgs e)
        {
            if (!XacNhanThemKhachHang())
                return;

            try
            {
                KhachHangAddInfo info = LayThongTinKhachHangMoi();
                ThemKhachHang(info);

                clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //private void btnAdd_Khachhang_Click_1(object sender, EventArgs e)
        //{
        //    if (txtName.Text != "" && txtContact.Text != "" && txtQuocTich.Text != "" &&
        // txtGioiTinh.Text != "" && txtMaID.Text != "" && txtAddress.Text != "" &&
        // txtCheckin.Text != "" && txtPrice.Text != "" && txtSoDem.Text != "")
        //    {
        //        try
        //        {
        //            string name = txtName.Text.Trim();
        //            Int64 phone = Int64.Parse(txtContact.Text.Trim());
        //            string quoctich = txtQuocTich.Text.Trim();
        //            string gioitinh = txtGioiTinh.Text.Trim();
        //            string maid = txtMaID.Text.Trim();
        //            string address = txtAddress.Text.Trim();
        //            string checkin = txtCheckin.Value.ToString("MM/dd/yyyy");
        //            string sophong = txtRoomNo.Text.Trim();
        //            string sodem = txtSoDem.Text.Trim();
        //            string query = $@"
        //                INSERT INTO KHACHHANG (TENKH, SDT, NUOC, GIOITINH, MADD, DIACHI,SODEM, CHECKIN, MAPHONG)
        //                VALUES (N'{name}', '{phone}', N'{quoctich}', N'{gioitinh}', N'{maid}', N'{address}','{sodem}', '{checkin}', {rid});

        //                UPDATE PHONG 
        //                SET DATPHONG = 'YES' 
        //                WHERE SOPHONG = '{sophong}';
        //            ";

        //            fn.setdata(query, $"Khách hàng đã được đăng ký và phòng {sophong} đã được đánh dấu là đã đặt!");

        //            clearAll();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        private void txtQuocTich_TextChanged(object sender, EventArgs e)
        {
            string input = txtQuocTich.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái và khoảng trắng. Không cho phép số hoặc ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuocTich.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
        }
        private void LoadRoomNoOptions()
        {
            txtRoomNo.Items.Clear();
            txtRoomNo.Text = "";
            txtPrice.Clear();

            query = "SELECT SOPHONG FROM PHONG WHERE GIUONG = '" + txtBed_Type.Text + "' AND LOAIPHONG = N'" + txtRoom_type.Text + "' AND DATPHONG = 'NO'";
            SetComboBox(query, txtRoomNo);
        }


        private void txtRoomNo_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            string selectedRoom = txtRoomNo.Text;

            query = $"SELECT GIA, MAPHONG FROM PHONG WHERE SOPHONG = '{selectedRoom}'";
            SqlDataReader sdr = fn.getforcombo(query);

            if (sdr.Read())
            {
                txtPrice.Text = sdr["GIA"].ToString();
                rid = int.Parse(sdr["MAPHONG"].ToString()); // 👈 Gán giá trị MAPHONG tại đây
            }

            sdr.Close();

        }

        private void txtRoom_type_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadRoomNoOptions();
        }

        private void txtBed_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRoom_type.SelectedIndex = -1;
            txtRoomNo.Items.Clear();
            txtPrice.Clear();
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged_1(object sender, EventArgs e)
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

        private void txtContact_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string input = txtContact.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtMaID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = txtMaID.Text;

                // Kiểm tra nếu nhập không phải là số nguyên
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d*$"))
                {
                    throw new Exception("Chỉ được nhập số, không cho phép chữ hoặc ký tự đặc biệt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaID.Text = ""; // Xóa dữ liệu sai
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            string input = txtAddress.Text;

            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsDigit(c) && !char.IsWhiteSpace(c))
                {
                    MessageBox.Show("Chỉ được nhập chữ cái, số và khoảng trắng. Không cho phép ký tự đặc biệt.",
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAddress.Text = ""; // Xóa dữ liệu sai
                    return;
                }
            }
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

        private void txtBed_Type_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtCheckin_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
