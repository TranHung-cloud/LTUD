using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace LTUD
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
            btnDangNhap.Click += BtnDangNhap_Click;
            btnMoDangKy.Click += BtnMoDangKy_Click;
            this.Load += FormDangNhap_Load;
            this.Resize += FormDangNhap_Load;
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            string maND = txtMaNguoiDung.Text.Trim();
            string mk = txtMatKhau.Text;

            if (maND == "" || mk == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable dt = DatabaseConnection.GetData($"SELECT MAVAITRO, MAGIADINH, TRANGTHAI FROM NGUOIDUNG WHERE MANGUOIDUNG = '{maND}' AND MATKHAU = '{mk}'");
                if (dt.Rows.Count > 0)
                {
                    string trangThai = dt.Rows[0]["TRANGTHAI"].ToString().Trim();
                    if (trangThai.Equals("Bị khóa", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Tài khoản của bạn đã bị khóa! Vui lòng liên hệ quản trị viên.", "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string vaiTro = dt.Rows[0]["MAVAITRO"].ToString().Trim();
                    string maGD = dt.Rows[0]["MAGIADINH"].ToString().Trim();

                    if (vaiTro == "VT01")
                    {
                        TrangChuAdmin fAdminAll = new TrangChuAdmin(maND, this);
                        this.Hide();
                        //fAdminAll.FormClosed += (s, args) => this.Close();
                        fAdminAll.Show();
                    }
                    else
                    {
                        FormTrangchu fTrangChu = new FormTrangchu(maND, maGD, vaiTro, this);
                        this.Hide();
                        //fTrangChu.FormClosed += (s, args) => this.Close();
                        fTrangChu.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            int cx = this.ClientSize.Width / 2;
            int cy = this.ClientSize.Height / 2;

            int groupWidth = 320;
            int groupHeight = 200;
            int startX = cx - groupWidth / 2;
            int startY = cy - groupHeight / 2;

            lblTitle.Location = new Point(startX + 90, startY);
            lblMaNguoiDung.Location = new Point(startX - 50, startY + 60);
            txtMaNguoiDung.Location = new Point(startX + 100, startY + 57);
            lblMatKhau.Location = new Point(startX - 50, startY + 110);
            txtMatKhau.Location = new Point(startX + 100, startY + 107);
            btnDangNhap.Location = new Point(startX + 20, startY + 170);
            btnMoDangKy.Location = new Point(startX + 140, startY + 170);
        }

        private void BtnMoDangKy_Click(object sender, EventArgs e)
        {
            FormDangKy frm = new FormDangKy();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void FormDangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}


