namespace LTUD
{
    partial class FormTaoGiaDinh
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
            this.lblMaGiaDinh = new System.Windows.Forms.Label();
            this.txtMaGiaDinh = new System.Windows.Forms.TextBox();
            this.lblTenGiaDinh = new System.Windows.Forms.Label();
            this.txtTenGiaDinh = new System.Windows.Forms.TextBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(120, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(260, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tạo Gia Đình";
            // 
            // lblMaGiaDinh
            // 
            this.lblMaGiaDinh.AutoSize = true;
            this.lblMaGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMaGiaDinh.Location = new System.Drawing.Point(50, 100);
            this.lblMaGiaDinh.Name = "lblMaGiaDinh";
            this.lblMaGiaDinh.Size = new System.Drawing.Size(100, 21);
            this.lblMaGiaDinh.TabIndex = 1;
            this.lblMaGiaDinh.Text = "Mã Gia Đình:";
            // 
            // txtMaGiaDinh
            // 
            this.txtMaGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtMaGiaDinh.Location = new System.Drawing.Point(180, 97);
            this.txtMaGiaDinh.Name = "txtMaGiaDinh";
            this.txtMaGiaDinh.Size = new System.Drawing.Size(250, 29);
            this.txtMaGiaDinh.TabIndex = 2;
            // 
            // lblTenGiaDinh
            // 
            this.lblTenGiaDinh.AutoSize = true;
            this.lblTenGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTenGiaDinh.Location = new System.Drawing.Point(50, 160);
            this.lblTenGiaDinh.Name = "lblTenGiaDinh";
            this.lblTenGiaDinh.Size = new System.Drawing.Size(102, 21);
            this.lblTenGiaDinh.TabIndex = 3;
            this.lblTenGiaDinh.Text = "Tên Gia Đình:";
            // 
            // txtTenGiaDinh
            // 
            this.txtTenGiaDinh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTenGiaDinh.Location = new System.Drawing.Point(180, 157);
            this.txtTenGiaDinh.Name = "txtTenGiaDinh";
            this.txtTenGiaDinh.Size = new System.Drawing.Size(250, 29);
            this.txtTenGiaDinh.TabIndex = 4;
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnLuu.Location = new System.Drawing.Point(120, 230);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(120, 40);
            this.btnLuu.TabIndex = 5;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(239)))), ((int)(((byte)(231)))));
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(113)))));
            this.btnHuy.Location = new System.Drawing.Point(260, 230);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(120, 40);
            this.btnHuy.TabIndex = 6;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // FormTaoGiaDinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(203)))), ((int)(((byte)(208)))));
            this.ClientSize = new System.Drawing.Size(500, 320);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.txtTenGiaDinh);
            this.Controls.Add(this.lblTenGiaDinh);
            this.Controls.Add(this.txtMaGiaDinh);
            this.Controls.Add(this.lblMaGiaDinh);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormTaoGiaDinh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tạo Gia Đình Mới";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMaGiaDinh;
        private System.Windows.Forms.TextBox txtMaGiaDinh;
        private System.Windows.Forms.Label lblTenGiaDinh;
        private System.Windows.Forms.TextBox txtTenGiaDinh;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}