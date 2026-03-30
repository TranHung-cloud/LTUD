namespace LTUD
{
    partial class FormQLBaoTri
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
            this.dgvBaoTri = new System.Windows.Forms.DataGridView();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.lblPhamViFilter = new System.Windows.Forms.Label();
            this.cbPhamViFilter = new System.Windows.Forms.ComboBox();
            this.lblTaiSanFilter = new System.Windows.Forms.Label();
            this.cbTaiSanFilter = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoTri)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(230, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(351, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản Lý Thông Tin Bảo Trì";
            // 
            // dgvBaoTri
            // 
            this.dgvBaoTri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBaoTri.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(203)))), ((int)(((byte)(208)))));
            this.dgvBaoTri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaoTri.Location = new System.Drawing.Point(50, 110);
            this.dgvBaoTri.Name = "dgvBaoTri";
            this.dgvBaoTri.Size = new System.Drawing.Size(700, 270);
            this.dgvBaoTri.TabIndex = 1;
            this.dgvBaoTri.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBaoTri_CellContentClick);
            // 
            // lblPhamViFilter
            // 
            this.lblPhamViFilter.AutoSize = true;
            this.lblPhamViFilter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPhamViFilter.Location = new System.Drawing.Point(50, 75);
            this.lblPhamViFilter.Name = "lblPhamViFilter";
            this.lblPhamViFilter.Size = new System.Drawing.Size(127, 21);
            this.lblPhamViFilter.TabIndex = 5;
            this.lblPhamViFilter.Text = "Chọn Phạm vi:";
            // 
            // cbPhamViFilter
            // 
            this.cbPhamViFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhamViFilter.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbPhamViFilter.FormattingEnabled = true;
            this.cbPhamViFilter.Location = new System.Drawing.Point(180, 72);
            this.cbPhamViFilter.Name = "cbPhamViFilter";
            this.cbPhamViFilter.Size = new System.Drawing.Size(150, 29);
            this.cbPhamViFilter.TabIndex = 6;
            // 
            // lblTaiSanFilter
            // 
            this.lblTaiSanFilter.AutoSize = true;
            this.lblTaiSanFilter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTaiSanFilter.Location = new System.Drawing.Point(340, 75);
            this.lblTaiSanFilter.Name = "lblTaiSanFilter";
            this.lblTaiSanFilter.Size = new System.Drawing.Size(71, 21);
            this.lblTaiSanFilter.TabIndex = 7;
            this.lblTaiSanFilter.Text = "Tài sản:";
            // 
            // cbTaiSanFilter
            // 
            this.cbTaiSanFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTaiSanFilter.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cbTaiSanFilter.FormattingEnabled = true;
            this.cbTaiSanFilter.Location = new System.Drawing.Point(420, 72);
            this.cbTaiSanFilter.Name = "cbTaiSanFilter";
            this.cbTaiSanFilter.Size = new System.Drawing.Size(330, 29);
            this.cbTaiSanFilter.TabIndex = 8;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnThem.Location = new System.Drawing.Point(50, 400);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(100, 40);
            this.btnThem.TabIndex = 2;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnSua.Location = new System.Drawing.Point(180, 400);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(100, 40);
            this.btnSua.TabIndex = 3;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXoa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnXoa.Location = new System.Drawing.Point(310, 400);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 40);
            this.btnXoa.TabIndex = 4;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            // 
            // FormQLBaoTri
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(203)))), ((int)(((byte)(208)))));
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.cbTaiSanFilter);
            this.Controls.Add(this.lblTaiSanFilter);
            this.Controls.Add(this.cbPhamViFilter);
            this.Controls.Add(this.lblPhamViFilter);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.dgvBaoTri);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormQLBaoTri";
            this.Text = "Quản lý Bảo trì";
            this.Load += new System.EventHandler(this.FormQLBaoTri_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoTri)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvBaoTri;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.ComboBox cbPhamViFilter;
        private System.Windows.Forms.Label lblPhamViFilter;
        private System.Windows.Forms.ComboBox cbTaiSanFilter;
        private System.Windows.Forms.Label lblTaiSanFilter;
    }
}