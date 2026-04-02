using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace LTUD
{
    public partial class FormDangKy : Form
    {
        public FormDangKy()
        {
            InitializeComponent();
            btnDangKy.Click += BtnDangKy_Click;
            btnHuy.Click += (s, e) => this.Close();
            this.Load += FormDangKy_Load;
            this.Resize += FormDangKy_Load;
        }

        private void FormDangKy_Load(object sender, EventArgs e)
        {
            int cx = this.ClientSize.Width / 2;
            int cy = this.ClientSize.Height / 2;

            int groupWidth = 340;
            int groupHeight = 350;
            int startX = cx - groupWidth / 2;
            int startY = cy - groupHeight / 2;

            lblTitle.Location = new Point(startX + 110, startY);

            lblMaNguoiDung.Location = new Point(startX, startY + 60);
            txtMaNguoiDung.Location = new Point(startX + 160, startY + 57);

            lblHoTen.Location = new Point(startX, startY + 110);
            txtHoTen.Location = new Point(startX + 160, startY + 107);

            lblNgaySinh.Location = new Point(startX, startY + 160);
            dtpNgaySinh.Location = new Point(startX + 160, startY + 157);

            lblMatKhau.Location = new Point(startX, startY + 210);
            txtMatKhau.Location = new Point(startX + 160, startY + 207);

            lblXacNhanMatKhau.Location = new Point(startX, startY + 260);
            txtXacNhanMatKhau.Location = new Point(startX + 160, startY + 257);

            btnDangKy.Location = new Point(startX + 60, startY + 310);
            btnHuy.Location = new Point(startX + 180, startY + 310);
        }

        private void BtnDangKy_Click(object sender, EventArgs e)
        {
            string maND = txtMaNguoiDung.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            DateTime ngaySinh = dtpNgaySinh.Value;
            string matKhau = txtMatKhau.Text;
            string xacNhanMK = txtXacNhanMatKhau.Text;

            if (maND.Length == 0 || hoTen.Length == 0 || matKhau.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!");
                return;
            }

            if (ngaySinh >= DateTime.Now)
            {
                MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (matKhau != xacNhanMK)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DataTable checkUser = DatabaseConnection.GetData($"SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MANGUOIDUNG = '{maND}'");
                if (checkUser.Rows.Count > 0)
                {
                    MessageBox.Show("Mã người dùng đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Default Vai tro (VT02 - User or set as needed) and Gia Dinh (Empty or default GD01)
                string defaultVT = "VT02"; 
                string defaultGD = "GD01"; 
                string status = "Hoạt động";
                string dateStr = ngaySinh.ToString("yyyy-MM-dd");

                DatabaseConnection.ExecuteQuery($"INSERT INTO NGUOIDUNG VALUES ('{maND}', '{defaultVT}', '{defaultGD}', N'{hoTen}', '{dateStr}', '{matKhau}', N'{status}')");
                
                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
