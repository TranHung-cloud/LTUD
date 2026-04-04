namespace LTUD
{
    partial class FormQLTS_CN
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelQLTS = new System.Windows.Forms.Label();
            this.btnToMain = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnChiTiet = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.groupBoxTS = new System.Windows.Forms.GroupBox();
            this.txtGiaThucTe = new System.Windows.Forms.TextBox();
            this.lblGiaThucTe = new System.Windows.Forms.Label();
            this.txtTinhTrang = new System.Windows.Forms.TextBox();
            this.lblTinhTrang = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnThemAnh = new System.Windows.Forms.Button();
            this.lblHinhAnh = new System.Windows.Forms.Label();
            this.txtNguyenGia = new System.Windows.Forms.TextBox();
            this.lblNguyenGia = new System.Windows.Forms.Label();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.lblNgayMua = new System.Windows.Forms.Label();
            this.cbLoaiTaiSan = new System.Windows.Forms.ComboBox();
            this.lblLoaiTaiSan = new System.Windows.Forms.Label();
            this.txtTenTaiSan = new System.Windows.Forms.TextBox();
            this.lblTenTaiSan = new System.Windows.Forms.Label();
            this.txtMaTaiSan = new System.Windows.Forms.TextBox();
            this.labelMaTS = new System.Windows.Forms.Label();
            this.btnXemRP = new System.Windows.Forms.Button();
            this.btnDatLai = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBoxTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // labelQLTS
            // 
            this.labelQLTS.AutoSize = true;
            this.labelQLTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQLTS.Location = new System.Drawing.Point(220, 27);
            this.labelQLTS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelQLTS.Name = "labelQLTS";
            this.labelQLTS.Size = new System.Drawing.Size(399, 31);
            this.labelQLTS.TabIndex = 0;
            this.labelQLTS.Text = "QUẢN LÝ TÀI SẢN CÁ NHÂN";
            // 
            // btnToMain
            // 
            this.btnToMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToMain.Location = new System.Drawing.Point(698, 20);
            this.btnToMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnToMain.Name = "btnToMain";
            this.btnToMain.Size = new System.Drawing.Size(117, 39);
            this.btnToMain.TabIndex = 1;
            this.btnToMain.Text = "Về trang chủ";
            this.btnToMain.UseVisualStyleBackColor = true;
            this.btnToMain.Click += new System.EventHandler(this.btnToMain_Click);
            // 
            // dgv
            // 
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(50, 128);
            this.dgv.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 51;
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(752, 257);
            this.dgv.TabIndex = 2;
            this.dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellContentClick);
            // 
            // btnThem
            // 
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.Location = new System.Drawing.Point(50, 402);
            this.btnThem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(112, 39);
            this.btnThem.TabIndex = 3;
            this.btnThem.Text = "Thêm tài sản";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.Location = new System.Drawing.Point(183, 402);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(106, 39);
            this.btnXoa.TabIndex = 4;
            this.btnXoa.Text = "Xóa tài sản";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.Location = new System.Drawing.Point(310, 402);
            this.btnSua.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(100, 39);
            this.btnSua.TabIndex = 5;
            this.btnSua.Text = "Sửa tài sản";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnChiTiet
            // 
            this.btnChiTiet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChiTiet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChiTiet.Location = new System.Drawing.Point(427, 402);
            this.btnChiTiet.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnChiTiet.Name = "btnChiTiet";
            this.btnChiTiet.Size = new System.Drawing.Size(110, 39);
            this.btnChiTiet.TabIndex = 6;
            this.btnChiTiet.Text = "Xem khấu hao";
            this.btnChiTiet.UseVisualStyleBackColor = true;
            this.btnChiTiet.Click += new System.EventHandler(this.btnChiTiet_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.Location = new System.Drawing.Point(706, 88);
            this.btnTimKiem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(96, 33);
            this.btnTimKiem.TabIndex = 9;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.Location = new System.Drawing.Point(516, 88);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTimKiem.Multiline = true;
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(186, 34);
            this.txtTimKiem.TabIndex = 10;
            // 
            // groupBoxTS
            // 
            this.groupBoxTS.Controls.Add(this.txtGiaThucTe);
            this.groupBoxTS.Controls.Add(this.lblGiaThucTe);
            this.groupBoxTS.Controls.Add(this.txtTinhTrang);
            this.groupBoxTS.Controls.Add(this.lblTinhTrang);
            this.groupBoxTS.Controls.Add(this.pictureBox);
            this.groupBoxTS.Controls.Add(this.btnThemAnh);
            this.groupBoxTS.Controls.Add(this.lblHinhAnh);
            this.groupBoxTS.Controls.Add(this.txtNguyenGia);
            this.groupBoxTS.Controls.Add(this.lblNguyenGia);
            this.groupBoxTS.Controls.Add(this.dtp);
            this.groupBoxTS.Controls.Add(this.lblNgayMua);
            this.groupBoxTS.Controls.Add(this.cbLoaiTaiSan);
            this.groupBoxTS.Controls.Add(this.lblLoaiTaiSan);
            this.groupBoxTS.Controls.Add(this.txtTenTaiSan);
            this.groupBoxTS.Controls.Add(this.lblTenTaiSan);
            this.groupBoxTS.Controls.Add(this.txtMaTaiSan);
            this.groupBoxTS.Controls.Add(this.labelMaTS);
            this.groupBoxTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTS.Location = new System.Drawing.Point(50, 473);
            this.groupBoxTS.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxTS.Name = "groupBoxTS";
            this.groupBoxTS.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxTS.Size = new System.Drawing.Size(752, 239);
            this.groupBoxTS.TabIndex = 13;
            this.groupBoxTS.TabStop = false;
            this.groupBoxTS.Text = "Thông tin tài sản";
            // 
            // txtGiaThucTe
            // 
            this.txtGiaThucTe.Enabled = false;
            this.txtGiaThucTe.Location = new System.Drawing.Point(359, 192);
            this.txtGiaThucTe.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGiaThucTe.Name = "txtGiaThucTe";
            this.txtGiaThucTe.Size = new System.Drawing.Size(192, 24);
            this.txtGiaThucTe.TabIndex = 16;
            // 
            // lblGiaThucTe
            // 
            this.lblGiaThucTe.AutoSize = true;
            this.lblGiaThucTe.Location = new System.Drawing.Point(272, 194);
            this.lblGiaThucTe.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGiaThucTe.Name = "lblGiaThucTe";
            this.lblGiaThucTe.Size = new System.Drawing.Size(81, 18);
            this.lblGiaThucTe.TabIndex = 15;
            this.lblGiaThucTe.Text = "Giá hiện tại";
            // 
            // txtTinhTrang
            // 
            this.txtTinhTrang.Enabled = false;
            this.txtTinhTrang.Location = new System.Drawing.Point(105, 194);
            this.txtTinhTrang.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTinhTrang.Name = "txtTinhTrang";
            this.txtTinhTrang.Size = new System.Drawing.Size(142, 24);
            this.txtTinhTrang.TabIndex = 14;
            // 
            // lblTinhTrang
            // 
            this.lblTinhTrang.AutoSize = true;
            this.lblTinhTrang.Location = new System.Drawing.Point(17, 194);
            this.lblTinhTrang.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTinhTrang.Name = "lblTinhTrang";
            this.lblTinhTrang.Size = new System.Drawing.Size(73, 18);
            this.lblTinhTrang.TabIndex = 13;
            this.lblTinhTrang.Text = "Tình trạng";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(578, 24);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 206);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 12;
            this.pictureBox.TabStop = false;
            // 
            // btnThemAnh
            // 
            this.btnThemAnh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemAnh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemAnh.Location = new System.Drawing.Point(359, 134);
            this.btnThemAnh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnThemAnh.Name = "btnThemAnh";
            this.btnThemAnh.Size = new System.Drawing.Size(117, 39);
            this.btnThemAnh.TabIndex = 11;
            this.btnThemAnh.Text = "Thêm ảnh";
            this.btnThemAnh.UseVisualStyleBackColor = true;
            this.btnThemAnh.Click += new System.EventHandler(this.btnThemAnh_Click);
            // 
            // lblHinhAnh
            // 
            this.lblHinhAnh.AutoSize = true;
            this.lblHinhAnh.Location = new System.Drawing.Point(272, 142);
            this.lblHinhAnh.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHinhAnh.Name = "lblHinhAnh";
            this.lblHinhAnh.Size = new System.Drawing.Size(66, 18);
            this.lblHinhAnh.TabIndex = 10;
            this.lblHinhAnh.Text = "Hình ảnh";
            // 
            // txtNguyenGia
            // 
            this.txtNguyenGia.Location = new System.Drawing.Point(359, 89);
            this.txtNguyenGia.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtNguyenGia.Name = "txtNguyenGia";
            this.txtNguyenGia.Size = new System.Drawing.Size(192, 24);
            this.txtNguyenGia.TabIndex = 9;
            // 
            // lblNguyenGia
            // 
            this.lblNguyenGia.AutoSize = true;
            this.lblNguyenGia.Location = new System.Drawing.Point(272, 91);
            this.lblNguyenGia.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNguyenGia.Name = "lblNguyenGia";
            this.lblNguyenGia.Size = new System.Drawing.Size(81, 18);
            this.lblNguyenGia.TabIndex = 8;
            this.lblNguyenGia.Text = "Nguyên giá";
            // 
            // dtp
            // 
            this.dtp.Location = new System.Drawing.Point(359, 40);
            this.dtp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtp.Name = "dtp";
            this.dtp.Size = new System.Drawing.Size(192, 24);
            this.dtp.TabIndex = 7;
            // 
            // lblNgayMua
            // 
            this.lblNgayMua.AutoSize = true;
            this.lblNgayMua.Location = new System.Drawing.Point(272, 40);
            this.lblNgayMua.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNgayMua.Name = "lblNgayMua";
            this.lblNgayMua.Size = new System.Drawing.Size(75, 18);
            this.lblNgayMua.TabIndex = 6;
            this.lblNgayMua.Text = "Ngày mua";
            // 
            // cbLoaiTaiSan
            // 
            this.cbLoaiTaiSan.FormattingEnabled = true;
            this.cbLoaiTaiSan.Location = new System.Drawing.Point(105, 136);
            this.cbLoaiTaiSan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbLoaiTaiSan.Name = "cbLoaiTaiSan";
            this.cbLoaiTaiSan.Size = new System.Drawing.Size(142, 25);
            this.cbLoaiTaiSan.TabIndex = 5;
            // 
            // lblLoaiTaiSan
            // 
            this.lblLoaiTaiSan.AutoSize = true;
            this.lblLoaiTaiSan.Location = new System.Drawing.Point(17, 142);
            this.lblLoaiTaiSan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLoaiTaiSan.Name = "lblLoaiTaiSan";
            this.lblLoaiTaiSan.Size = new System.Drawing.Size(83, 18);
            this.lblLoaiTaiSan.TabIndex = 4;
            this.lblLoaiTaiSan.Text = "Loại tài sản";
            // 
            // txtTenTaiSan
            // 
            this.txtTenTaiSan.Location = new System.Drawing.Point(105, 87);
            this.txtTenTaiSan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTenTaiSan.Name = "txtTenTaiSan";
            this.txtTenTaiSan.Size = new System.Drawing.Size(142, 24);
            this.txtTenTaiSan.TabIndex = 3;
            // 
            // lblTenTaiSan
            // 
            this.lblTenTaiSan.AutoSize = true;
            this.lblTenTaiSan.Location = new System.Drawing.Point(17, 87);
            this.lblTenTaiSan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTenTaiSan.Name = "lblTenTaiSan";
            this.lblTenTaiSan.Size = new System.Drawing.Size(80, 18);
            this.lblTenTaiSan.TabIndex = 2;
            this.lblTenTaiSan.Text = "Tên tài sản";
            // 
            // txtMaTaiSan
            // 
            this.txtMaTaiSan.Enabled = false;
            this.txtMaTaiSan.Location = new System.Drawing.Point(105, 40);
            this.txtMaTaiSan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMaTaiSan.Name = "txtMaTaiSan";
            this.txtMaTaiSan.Size = new System.Drawing.Size(142, 24);
            this.txtMaTaiSan.TabIndex = 1;
            // 
            // labelMaTS
            // 
            this.labelMaTS.AutoSize = true;
            this.labelMaTS.Location = new System.Drawing.Point(17, 40);
            this.labelMaTS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMaTS.Name = "labelMaTS";
            this.labelMaTS.Size = new System.Drawing.Size(76, 18);
            this.labelMaTS.TabIndex = 0;
            this.labelMaTS.Text = "Mã tài sản";
            // 
            // btnXemRP
            // 
            this.btnXemRP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemRP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemRP.Location = new System.Drawing.Point(650, 402);
            this.btnXemRP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnXemRP.Name = "btnXemRP";
            this.btnXemRP.Size = new System.Drawing.Size(152, 39);
            this.btnXemRP.TabIndex = 14;
            this.btnXemRP.Text = "Xem báo cáo tài sản";
            this.btnXemRP.UseVisualStyleBackColor = true;
            this.btnXemRP.Click += new System.EventHandler(this.btnXemRP_Click);
            // 
            // btnDatLai
            // 
            this.btnDatLai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatLai.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDatLai.Location = new System.Drawing.Point(551, 402);
            this.btnDatLai.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDatLai.Name = "btnDatLai";
            this.btnDatLai.Size = new System.Drawing.Size(86, 39);
            this.btnDatLai.TabIndex = 15;
            this.btnDatLai.Text = "Đặt lại";
            this.btnDatLai.UseVisualStyleBackColor = true;
            this.btnDatLai.Click += new System.EventHandler(this.btnDatLai_Click);
            // 
            // FormQLTS_CN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(848, 736);
            this.Controls.Add(this.btnDatLai);
            this.Controls.Add(this.btnXemRP);
            this.Controls.Add(this.groupBoxTS);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.btnChiTiet);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnToMain);
            this.Controls.Add(this.labelQLTS);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximumSize = new System.Drawing.Size(866, 782);
            this.Name = "FormQLTS_CN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormQLTS_CN";
            this.Load += new System.EventHandler(this.FormQLTS_CN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBoxTS.ResumeLayout(false);
            this.groupBoxTS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelQLTS;
        private System.Windows.Forms.Button btnToMain;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnChiTiet;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.GroupBox groupBoxTS;
        private System.Windows.Forms.Label labelMaTS;
        private System.Windows.Forms.TextBox txtTenTaiSan;
        private System.Windows.Forms.Label lblTenTaiSan;
        private System.Windows.Forms.TextBox txtMaTaiSan;
        private System.Windows.Forms.Label lblLoaiTaiSan;
        private System.Windows.Forms.ComboBox cbLoaiTaiSan;
        private System.Windows.Forms.Label lblNgayMua;
        private System.Windows.Forms.DateTimePicker dtp;
        private System.Windows.Forms.Label lblNguyenGia;
        private System.Windows.Forms.TextBox txtNguyenGia;
        private System.Windows.Forms.Button btnThemAnh;
        private System.Windows.Forms.Label lblHinhAnh;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox txtTinhTrang;
        private System.Windows.Forms.Label lblTinhTrang;
        private System.Windows.Forms.Button btnXemRP;
        private System.Windows.Forms.TextBox txtGiaThucTe;
        private System.Windows.Forms.Label lblGiaThucTe;
        private System.Windows.Forms.Button btnDatLai;
    }
}