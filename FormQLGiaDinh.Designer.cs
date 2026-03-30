namespace LTUD
{
    partial class FormQLGiaDinh
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
            this.dgvNguoiDung = new System.Windows.Forms.DataGridView();
            this.lblGiaDinh = new System.Windows.Forms.Label();
            this.lblMaGiaDinh = new System.Windows.Forms.Label();
            this.lblTenGiaDinh = new System.Windows.Forms.Label();
            this.lblThanhVien = new System.Windows.Forms.Label();
            this.btnThemTV = new System.Windows.Forms.Button();
            this.btnSuaTV = new System.Windows.Forms.Button();
            this.btnXoaTV = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoiDung)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.lblTitle.Location = new System.Drawing.Point(260, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản Lý Gia Đình";
            // 
            // lblGiaDinh
            // 
            this.lblGiaDinh.AutoSize = true;
            this.lblGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGiaDinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.lblGiaDinh.Location = new System.Drawing.Point(30, 60);
            this.lblGiaDinh.Name = "lblGiaDinh";
            this.lblGiaDinh.Size = new System.Drawing.Size(161, 21);
            this.lblGiaDinh.TabIndex = 3;
            this.lblGiaDinh.Text = "Thông tin Gia đình:";
            // 
            // lblMaGiaDinh
            // 
            this.lblMaGiaDinh.AutoSize = true;
            this.lblMaGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMaGiaDinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.lblMaGiaDinh.Location = new System.Drawing.Point(30, 90);
            this.lblMaGiaDinh.Name = "lblMaGiaDinh";
            this.lblMaGiaDinh.Size = new System.Drawing.Size(117, 21);
            this.lblMaGiaDinh.TabIndex = 5;
            this.lblMaGiaDinh.Text = "Mã Gia Đình: ...";
            // 
            // lblTenGiaDinh
            // 
            this.lblTenGiaDinh.AutoSize = true;
            this.lblTenGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTenGiaDinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.lblTenGiaDinh.Location = new System.Drawing.Point(30, 120);
            this.lblTenGiaDinh.Name = "lblTenGiaDinh";
            this.lblTenGiaDinh.Size = new System.Drawing.Size(118, 21);
            this.lblTenGiaDinh.TabIndex = 6;
            this.lblTenGiaDinh.Text = "Tên Gia Đình: ...";
            // 
            // lblThanhVien
            // 
            this.lblThanhVien.AutoSize = true;
            this.lblThanhVien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblThanhVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.lblThanhVien.Location = new System.Drawing.Point(300, 60);
            this.lblThanhVien.Name = "lblThanhVien";
            this.lblThanhVien.Size = new System.Drawing.Size(183, 21);
            this.lblThanhVien.TabIndex = 4;
            this.lblThanhVien.Text = "Danh sách Thành viên";
            // 
            // dgvNguoiDung
            // 
            this.dgvNguoiDung.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.dgvNguoiDung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNguoiDung.Location = new System.Drawing.Point(300, 90);
            this.dgvNguoiDung.Name = "dgvNguoiDung";
            this.dgvNguoiDung.Size = new System.Drawing.Size(520, 300);
            this.dgvNguoiDung.TabIndex = 2;
            // 
            // btnThemTV
            // 
            this.btnThemTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(166)))), ((int)(((byte)(167)))));
            this.btnThemTV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemTV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThemTV.ForeColor = System.Drawing.Color.White;
            this.btnThemTV.Location = new System.Drawing.Point(300, 410);
            this.btnThemTV.Name = "btnThemTV";
            this.btnThemTV.Size = new System.Drawing.Size(120, 40);
            this.btnThemTV.TabIndex = 7;
            this.btnThemTV.Text = "Thêm";
            this.btnThemTV.UseVisualStyleBackColor = false;
            // 
            // btnSuaTV
            // 
            this.btnSuaTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(166)))), ((int)(((byte)(167)))));
            this.btnSuaTV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaTV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSuaTV.ForeColor = System.Drawing.Color.White;
            this.btnSuaTV.Location = new System.Drawing.Point(440, 410);
            this.btnSuaTV.Name = "btnSuaTV";
            this.btnSuaTV.Size = new System.Drawing.Size(120, 40);
            this.btnSuaTV.TabIndex = 8;
            this.btnSuaTV.Text = "Sửa";
            this.btnSuaTV.UseVisualStyleBackColor = false;
            // 
            // btnXoaTV
            // 
            this.btnXoaTV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnXoaTV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaTV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXoaTV.ForeColor = System.Drawing.Color.White;
            this.btnXoaTV.Location = new System.Drawing.Point(580, 410);
            this.btnXoaTV.Name = "btnXoaTV";
            this.btnXoaTV.Size = new System.Drawing.Size(120, 40);
            this.btnXoaTV.TabIndex = 9;
            this.btnXoaTV.Text = "Vô hiệu hóa";
            this.btnXoaTV.UseVisualStyleBackColor = false;
            // FormQLGiaDinh
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(203)))), ((int)(((byte)(208)))));
            this.ClientSize = new System.Drawing.Size(850, 480);
            this.Controls.Add(this.btnXoaTV);
            this.Controls.Add(this.btnSuaTV);
            this.Controls.Add(this.btnThemTV);
            this.Controls.Add(this.lblThanhVien);
            this.Controls.Add(this.lblGiaDinh);
            this.Controls.Add(this.lblMaGiaDinh);
            this.Controls.Add(this.lblTenGiaDinh);
            this.Controls.Add(this.dgvNguoiDung);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormQLGiaDinh";
            this.Text = "Quản lý Gia đình";
            this.Load += new System.EventHandler(this.FormQLGiaDinh_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoiDung)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMaGiaDinh;
        private System.Windows.Forms.Label lblTenGiaDinh;
        private System.Windows.Forms.DataGridView dgvNguoiDung;
        private System.Windows.Forms.Label lblGiaDinh;
        private System.Windows.Forms.Label lblThanhVien;
        private System.Windows.Forms.Button btnThemTV;
        private System.Windows.Forms.Button btnSuaTV;
        private System.Windows.Forms.Button btnXoaTV;
    }
}