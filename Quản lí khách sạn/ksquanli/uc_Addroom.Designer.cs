namespace Quản_lí_khách_sạn.ksquanli
{
    partial class uc_Addroom
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnAddRoom = new Guna.UI2.WinForms.Guna2Button();
            this.txtLoaigiuong = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGiatien = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSophong = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Datagridview = new System.Windows.Forms.DataGridView();
            this.btnRepair = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.txtLoaiphong = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnexporttoexel = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.Datagridview)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddRoom
            // 
            this.btnAddRoom.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRoom.BorderRadius = 10;
            this.btnAddRoom.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddRoom.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddRoom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddRoom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddRoom.FillColor = System.Drawing.Color.Lime;
            this.btnAddRoom.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnAddRoom.ForeColor = System.Drawing.Color.Black;
            this.btnAddRoom.HoverState.FillColor = System.Drawing.Color.Lime;
            this.btnAddRoom.HoverState.ForeColor = System.Drawing.Color.Black;
            this.btnAddRoom.Location = new System.Drawing.Point(929, 603);
            this.btnAddRoom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddRoom.Name = "btnAddRoom";
            this.btnAddRoom.Size = new System.Drawing.Size(421, 82);
            this.btnAddRoom.TabIndex = 32;
            this.btnAddRoom.Text = "Thêm Phòng";
            this.btnAddRoom.Click += new System.EventHandler(this.btnAddRoom_Click);
            // 
            // txtLoaigiuong
            // 
            this.txtLoaigiuong.BackColor = System.Drawing.Color.Transparent;
            this.txtLoaigiuong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtLoaigiuong.BorderRadius = 10;
            this.txtLoaigiuong.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtLoaigiuong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtLoaigiuong.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoaigiuong.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoaigiuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLoaigiuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.txtLoaigiuong.ItemHeight = 42;
            this.txtLoaigiuong.Items.AddRange(new object[] {
            "Đơn",
            "Đôi"});
            this.txtLoaigiuong.Location = new System.Drawing.Point(69, 833);
            this.txtLoaigiuong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLoaigiuong.Name = "txtLoaigiuong";
            this.txtLoaigiuong.Size = new System.Drawing.Size(361, 48);
            this.txtLoaigiuong.TabIndex = 31;
            this.txtLoaigiuong.SelectedIndexChanged += new System.EventHandler(this.txtLoaigiuong_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(64, 783);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 31);
            this.label5.TabIndex = 30;
            this.label5.Text = "Loại Giường";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(521, 646);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 31);
            this.label4.TabIndex = 28;
            this.label4.Text = "Loại Phòng";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtGiatien
            // 
            this.txtGiatien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtGiatien.BorderRadius = 10;
            this.txtGiatien.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGiatien.DefaultText = "";
            this.txtGiatien.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtGiatien.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtGiatien.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGiatien.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGiatien.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGiatien.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiatien.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGiatien.Location = new System.Drawing.Point(535, 833);
            this.txtGiatien.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGiatien.Name = "txtGiatien";
            this.txtGiatien.PlaceholderText = "";
            this.txtGiatien.SelectedText = "";
            this.txtGiatien.Size = new System.Drawing.Size(361, 60);
            this.txtGiatien.TabIndex = 27;
            this.txtGiatien.TextChanged += new System.EventHandler(this.txtGiatien_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(523, 796);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 31);
            this.label3.TabIndex = 26;
            this.label3.Text = "Giá Tiền";
            // 
            // txtSophong
            // 
            this.txtSophong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtSophong.BorderRadius = 10;
            this.txtSophong.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSophong.DefaultText = "";
            this.txtSophong.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSophong.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSophong.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSophong.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSophong.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSophong.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.txtSophong.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSophong.Location = new System.Drawing.Point(71, 688);
            this.txtSophong.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSophong.Name = "txtSophong";
            this.txtSophong.PlaceholderText = "";
            this.txtSophong.SelectedText = "";
            this.txtSophong.Size = new System.Drawing.Size(367, 60);
            this.txtSophong.TabIndex = 25;
            this.txtSophong.TextChanged += new System.EventHandler(this.txtSophong_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(76, 646);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 31);
            this.label2.TabIndex = 24;
            this.label2.Text = "Số Phòng";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Text", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(511, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 80);
            this.label1.TabIndex = 22;
            this.label1.Text = "Thêm Phòng";
            // 
            // Datagridview
            // 
            this.Datagridview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Datagridview.BackgroundColor = System.Drawing.Color.White;
            this.Datagridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Datagridview.GridColor = System.Drawing.Color.White;
            this.Datagridview.Location = new System.Drawing.Point(69, 186);
            this.Datagridview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Datagridview.Name = "Datagridview";
            this.Datagridview.RowHeadersWidth = 51;
            this.Datagridview.RowTemplate.Height = 24;
            this.Datagridview.Size = new System.Drawing.Size(1281, 399);
            this.Datagridview.TabIndex = 33;
            this.Datagridview.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Datagridview1_CellContentClick);
            this.Datagridview.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Datagridview1_RowHeaderMouseClick);
            this.Datagridview.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.Datagridview_RowPostPaint);
            // 
            // btnRepair
            // 
            this.btnRepair.BackColor = System.Drawing.Color.Transparent;
            this.btnRepair.BorderRadius = 10;
            this.btnRepair.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRepair.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRepair.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRepair.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRepair.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnRepair.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRepair.ForeColor = System.Drawing.Color.White;
            this.btnRepair.Location = new System.Drawing.Point(929, 707);
            this.btnRepair.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(421, 86);
            this.btnRepair.TabIndex = 36;
            this.btnRepair.Text = "Chỉnh Sửa";
            this.btnRepair.Click += new System.EventHandler(this.btnRepair_Click);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.SteelBlue;
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1421, 143);
            this.guna2Panel1.TabIndex = 38;
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 30;
            this.guna2Elipse2.TargetControl = this.Datagridview;
            // 
            // txtLoaiphong
            // 
            this.txtLoaiphong.BackColor = System.Drawing.Color.Transparent;
            this.txtLoaiphong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtLoaiphong.BorderRadius = 10;
            this.txtLoaiphong.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtLoaiphong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtLoaiphong.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoaiphong.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLoaiphong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLoaiphong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.txtLoaiphong.ItemHeight = 42;
            this.txtLoaiphong.Items.AddRange(new object[] {
            "Thường ",
            "Đôi",
            "Vip"});
            this.txtLoaiphong.Location = new System.Drawing.Point(529, 689);
            this.txtLoaiphong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLoaiphong.Name = "txtLoaiphong";
            this.txtLoaiphong.Size = new System.Drawing.Size(365, 48);
            this.txtLoaiphong.TabIndex = 40;
            this.txtLoaiphong.SelectedIndexChanged += new System.EventHandler(this.guna2ComboBox1_SelectedIndexChanged);
            // 
            // btnexporttoexel
            // 
            this.btnexporttoexel.BackColor = System.Drawing.Color.Transparent;
            this.btnexporttoexel.BorderRadius = 10;
            this.btnexporttoexel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnexporttoexel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnexporttoexel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnexporttoexel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnexporttoexel.FillColor = System.Drawing.Color.SteelBlue;
            this.btnexporttoexel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnexporttoexel.ForeColor = System.Drawing.Color.White;
            this.btnexporttoexel.HoverState.FillColor = System.Drawing.Color.Lime;
            this.btnexporttoexel.HoverState.ForeColor = System.Drawing.Color.Black;
            this.btnexporttoexel.Location = new System.Drawing.Point(929, 816);
            this.btnexporttoexel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnexporttoexel.Name = "btnexporttoexel";
            this.btnexporttoexel.Size = new System.Drawing.Size(421, 90);
            this.btnexporttoexel.TabIndex = 41;
            this.btnexporttoexel.Text = "Lưu File Exel";
            this.btnexporttoexel.Click += new System.EventHandler(this.btnexporttoexel_Click);
            // 
            // uc_Addroom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.Controls.Add(this.btnexporttoexel);
            this.Controls.Add(this.txtLoaiphong);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.btnRepair);
            this.Controls.Add(this.Datagridview);
            this.Controls.Add(this.btnAddRoom);
            this.Controls.Add(this.txtLoaigiuong);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtGiatien);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSophong);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "uc_Addroom";
            this.Size = new System.Drawing.Size(1421, 1013);
            this.Load += new System.EventHandler(this.uc_Addroom_Load);
            this.Enter += new System.EventHandler(this.uc_Addroom_Enter);
            this.Leave += new System.EventHandler(this.uc_Addroom_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.Datagridview)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnAddRoom;
        private Guna.UI2.WinForms.Guna2ComboBox txtLoaigiuong;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox txtGiatien;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtSophong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Datagridview;
        private Guna.UI2.WinForms.Guna2Button btnRepair;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
        private Guna.UI2.WinForms.Guna2ComboBox txtLoaiphong;
        private Guna.UI2.WinForms.Guna2Button btnexporttoexel;
    }
}
