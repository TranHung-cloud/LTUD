namespace LTUD
{
    partial class FormChiTietKhauHao
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
            this.lblCTKH = new System.Windows.Forms.Label();
            this.lblTenTaiSan = new System.Windows.Forms.Label();
            this.lblNguyenGia = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lblGiaTriConLai = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCTKH
            // 
            this.lblCTKH.AutoSize = true;
            this.lblCTKH.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCTKH.Location = new System.Drawing.Point(230, 23);
            this.lblCTKH.Name = "lblCTKH";
            this.lblCTKH.Size = new System.Drawing.Size(315, 42);
            this.lblCTKH.TabIndex = 0;
            this.lblCTKH.Text = "Chi tiết khấu hao";
            // 
            // lblTenTaiSan
            // 
            this.lblTenTaiSan.AutoSize = true;
            this.lblTenTaiSan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenTaiSan.Location = new System.Drawing.Point(77, 122);
            this.lblTenTaiSan.Name = "lblTenTaiSan";
            this.lblTenTaiSan.Size = new System.Drawing.Size(120, 25);
            this.lblTenTaiSan.TabIndex = 1;
            this.lblTenTaiSan.Text = "Tên tài sản: ";
            // 
            // lblNguyenGia
            // 
            this.lblNguyenGia.AutoSize = true;
            this.lblNguyenGia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNguyenGia.Location = new System.Drawing.Point(77, 175);
            this.lblNguyenGia.Name = "lblNguyenGia";
            this.lblNguyenGia.Size = new System.Drawing.Size(122, 25);
            this.lblNguyenGia.TabIndex = 2;
            this.lblNguyenGia.Text = "Nguyên giá: ";
            // 
            // dgv
            // 
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(82, 237);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 51;
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(628, 243);
            this.dgv.TabIndex = 3;
            // 
            // lblGiaTriConLai
            // 
            this.lblGiaTriConLai.AutoSize = true;
            this.lblGiaTriConLai.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiaTriConLai.Location = new System.Drawing.Point(411, 175);
            this.lblGiaTriConLai.Name = "lblGiaTriConLai";
            this.lblGiaTriConLai.Size = new System.Drawing.Size(134, 25);
            this.lblGiaTriConLai.TabIndex = 4;
            this.lblGiaTriConLai.Text = "Giá trị còn lại: ";
            // 
            // FormChiTietKhauHao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 587);
            this.Controls.Add(this.lblGiaTriConLai);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.lblNguyenGia);
            this.Controls.Add(this.lblTenTaiSan);
            this.Controls.Add(this.lblCTKH);
            this.Name = "FormChiTietKhauHao";
            this.Text = "FormChiTietKhauHao";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormChiTietKhauHao_FormClosed);
            this.Load += new System.EventHandler(this.FormChiTietKhauHao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCTKH;
        private System.Windows.Forms.Label lblTenTaiSan;
        private System.Windows.Forms.Label lblNguyenGia;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblGiaTriConLai;
    }
}