namespace LTUD
{
    partial class FormAdminHome
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnQLGiaDinh = new System.Windows.Forms.Button();
            this.btnQLBaoTri = new System.Windows.Forms.Button();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.pnlDashboard = new System.Windows.Forms.Panel();
            this.lblTongGiaDinh = new System.Windows.Forms.Label();
            this.lblBaoTri = new System.Windows.Forms.Label();
            this.lblTongThanhVien = new System.Windows.Forms.Label();
            this.lblTongTaiSan = new System.Windows.Forms.Label();
            this.pnlDashboard.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(230, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(313, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Dashboard Quản Lý";
            // 
            // btnQLGiaDinh
            // 
            this.btnQLGiaDinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnQLGiaDinh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQLGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnQLGiaDinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnQLGiaDinh.Location = new System.Drawing.Point(50, 400);
            this.btnQLGiaDinh.Name = "btnQLGiaDinh";
            this.btnQLGiaDinh.Size = new System.Drawing.Size(200, 50);
            this.btnQLGiaDinh.TabIndex = 1;
            this.btnQLGiaDinh.Text = "Quản lý Gia đình";
            this.btnQLGiaDinh.UseVisualStyleBackColor = false;
            this.btnQLGiaDinh.Click += new System.EventHandler(this.btnQLGiaDinh_Click);
            // 
            // btnQLBaoTri
            // 
            this.btnQLBaoTri.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnQLBaoTri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQLBaoTri.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnQLBaoTri.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnQLBaoTri.Location = new System.Drawing.Point(300, 400);
            this.btnQLBaoTri.Name = "btnQLBaoTri";
            this.btnQLBaoTri.Size = new System.Drawing.Size(200, 50);
            this.btnQLBaoTri.TabIndex = 2;
            this.btnQLBaoTri.Text = "Thông tin Bảo trì";
            this.btnQLBaoTri.UseVisualStyleBackColor = false;
            this.btnQLBaoTri.Click += new System.EventHandler(this.btnQLBaoTri_Click);
            // 
            // pnlDashboard
            // 
            this.pnlDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(203)))), ((int)(((byte)(208)))));
            this.pnlDashboard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDashboard.Controls.Add(this.lblTongGiaDinh);
            this.pnlDashboard.Controls.Add(this.lblBaoTri);
            this.pnlDashboard.Controls.Add(this.lblTongThanhVien);
            this.pnlDashboard.Controls.Add(this.lblTongTaiSan);
            this.pnlDashboard.ForeColor = System.Drawing.Color.Black;
            this.pnlDashboard.Location = new System.Drawing.Point(50, 100);
            this.pnlDashboard.Name = "pnlDashboard";
            this.pnlDashboard.Size = new System.Drawing.Size(700, 250);
            this.pnlDashboard.TabIndex = 3;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDangXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnDangXuat.Location = new System.Drawing.Point(550, 400);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(200, 50);
            this.btnDangXuat.TabIndex = 5;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // lblTongGiaDinh
            // 
            this.lblTongGiaDinh.AutoSize = true;
            this.lblTongGiaDinh.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblTongGiaDinh.ForeColor = System.Drawing.Color.Black;
            this.lblTongGiaDinh.Location = new System.Drawing.Point(30, 30);
            this.lblTongGiaDinh.Name = "lblTongGiaDinh";
            this.lblTongGiaDinh.Size = new System.Drawing.Size(180, 25);
            this.lblTongGiaDinh.TabIndex = 4;
            this.lblTongGiaDinh.Text = "Số lượng gia đình: 0";
            // 
            // lblBaoTri
            // 
            this.lblBaoTri.AutoSize = true;
            this.lblBaoTri.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblBaoTri.ForeColor = System.Drawing.Color.Black;
            this.lblBaoTri.Location = new System.Drawing.Point(30, 180);
            this.lblBaoTri.Name = "lblBaoTri";
            this.lblBaoTri.Size = new System.Drawing.Size(256, 25);
            this.lblBaoTri.TabIndex = 2;
            this.lblBaoTri.Text = "Tài sản sắp đến hạn bảo trì: 0";
            // 
            // lblTongThanhVien
            // 
            this.lblTongThanhVien.AutoSize = true;
            this.lblTongThanhVien.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblTongThanhVien.ForeColor = System.Drawing.Color.Black;
            this.lblTongThanhVien.Location = new System.Drawing.Point(30, 130);
            this.lblTongThanhVien.Name = "lblTongThanhVien";
            this.lblTongThanhVien.Size = new System.Drawing.Size(200, 25);
            this.lblTongThanhVien.TabIndex = 1;
            this.lblTongThanhVien.Text = "Số lượng thành viên: 0";
            // 
            // lblTongTaiSan
            // 
            this.lblTongTaiSan.AutoSize = true;
            this.lblTongTaiSan.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblTongTaiSan.ForeColor = System.Drawing.Color.Black;
            this.lblTongTaiSan.Location = new System.Drawing.Point(30, 80);
            this.lblTongTaiSan.Name = "lblTongTaiSan";
            this.lblTongTaiSan.Size = new System.Drawing.Size(196, 25);
            this.lblTongTaiSan.TabIndex = 0;
            this.lblTongTaiSan.Text = "Tổng tài sản sổ hữu: 0";
            // 
            // FormAdminHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(203)))), ((int)(((byte)(208)))));
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnDangXuat);
            this.Controls.Add(this.pnlDashboard);
            this.Controls.Add(this.btnQLBaoTri);
            this.Controls.Add(this.btnQLGiaDinh);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormAdminHome";
            this.Text = "Trang chủ Admin";
            this.Load += new System.EventHandler(this.FormAdminHome_Load);
            this.pnlDashboard.ResumeLayout(false);
            this.pnlDashboard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnQLGiaDinh;
        private System.Windows.Forms.Button btnQLBaoTri;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Panel pnlDashboard;
        private System.Windows.Forms.Label lblTongTaiSan;
        private System.Windows.Forms.Label lblBaoTri;
        private System.Windows.Forms.Label lblTongThanhVien;
        private System.Windows.Forms.Label lblTongGiaDinh;
    }
}