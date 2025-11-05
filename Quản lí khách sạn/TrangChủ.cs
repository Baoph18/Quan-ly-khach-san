using Quản_lí_khách_sạn.ksquanli;
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
    public partial class TrangChủ: Form
    {
        public TrangChủ()
        {
            InitializeComponent();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void uc_Addroom1_Load(object sender, EventArgs e)
        {

        }
        private void TrangChủ_Load(object sender, EventArgs e)
        {


            // Lấy vai trò hiện tại
            string role = (CurrentUser.Role ?? "").ToLower().Trim();

            // Nếu không phải là quản lý, ẩn nút Nhân viên
            if (role != "quanly")
            {
               
                btnEmplyee.Enabled = false;
                bntThongKe.Enabled = false;
            }
            else
            {
               
                btnEmplyee.Enabled = true;
                bntThongKe.Enabled = true;
            }

            // Ẩn các UserControl ban đầu
            uc_Addroom2.Visible = false;
            uc_CustomerReg2.Visible = false;
            uc_Checkout2.Visible = false;
            uc_CustomerDetails2.Visible = false;
            uc_Employee1.Visible = false;
            thongTinCaNhan1.Visible = false;

            // Hiển thị giao diện mặc định
            btnthphong.PerformClick();
        }

        private void btnthphong_Click(object sender, EventArgs e)
        {
            //di chuyển một Panel tên là PanelMoving theo (chiều ngang) tọa độ x + 8 pixel 
            //PanelMoving.Left = btnthphong.Left + 8;
            uc_Addroom2.Visible = true;
            uc_Addroom2.BringToFront();
            uc_Addroom2.load();
        }

        private void btnDKKhachHang_Click(object sender, EventArgs e)
        {
            //PanelMoving.Left = btnDKKhachHang.Left + 13;
            uc_CustomerReg2.Visible = true;
            uc_CustomerReg2.BringToFront();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            
            //PanelMoving.Left = btnCheckout.Left + 8;
            uc_Checkout2.Visible = true;
            uc_Checkout2.BringToFront();
            uc_Checkout2.Load();
           
        }

        private void btnCustomerDetails_Click(object sender, EventArgs e)
        {
            //PanelMoving.Left = btnCustomerDetails.Left + 8;
            uc_CustomerDetails2.Visible = true;
            uc_CustomerDetails2.BringToFront();
            uc_CustomerDetails2.load();
            
            
        }

        private void btnEmplyee_Click(object sender, EventArgs e)
        {
            //PanelMoving.Left = btnEmplyee.Left + 8;
            uc_Employee1.Visible = true;
            uc_Employee1.BringToFront();
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            //PanelMoving.Left = btnHoaDon.Left + 8;
            uc_HoaDon1.Visible = true;
            uc_HoaDon1.BringToFront();
            uc_HoaDon1.LoadHoaDon();
        }

        private void btnInformation_Click(object sender, EventArgs e)
        {
            thongTinCaNhan1.Visible = true;
            thongTinCaNhan1.BringToFront();
            
        }


        private void thongTinCaNhan1_Load(object sender, EventArgs e)
        {

        }

        private void thongTinCaNhan1_Load_1(object sender, EventArgs e)
        {

        }

        private void PanelMoving_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bntThongKe_Click(object sender, EventArgs e)
        {
            ThongKe tk = new ThongKe();
            tk.Show();
        }
    }
}
