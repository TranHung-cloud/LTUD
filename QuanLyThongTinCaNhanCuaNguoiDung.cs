using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LTUD
{
    public partial class QuanLyThongTinCaNhanCuaNguoiDung : Form
    {
        // 1. Chuỗi kết nối đã cập nhật tên Database chính xác của Nhi
        string connectionString = @"Server =.\SQLEXPRESS; Database = QLTaiSan_LTUD; Integrated Security = True; TrustServerCertificate=True";

        // 2. ID người dùng đăng nhập (Mặc định ND01)
        string loggedInUserId;

        public QuanLyThongTinCaNhanCuaNguoiDung(string loggedInUserId)
        {
            InitializeComponent();
            this.loggedInUserId = loggedInUserId;
        }

        // 3. Hàm định dạng giao diện (Màu sắc pastel theo yêu cầu)
        private void ApplyButtonColors(Button btn)
        {
            btn.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btn.ForeColor = ColorTranslator.FromHtml("#006A71");
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }

        // 4. Sự kiện Load Form
        private void QuanLyThongTinCaNhanCuaNguoiDung_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#9ACBD0");

            ApplyButtonColors(btnLuuThongTin);
            ApplyButtonColors(btnDoiMatKhau);

            // Ràng buộc 1: Không cho phép chọn ngày sinh từ hôm nay trở đi ngay trên lịch
            dtpNgaySinh.MaxDate = DateTime.Now.AddDays(-1);

            // Tải dữ liệu ban đầu
            LoadThongTinCaNhan();
        }

        // 5. Hàm tải dữ liệu từ SQL (Bỏ Email và SĐT)
        private void LoadThongTinCaNhan()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT N.MANGUOIDUNG, N.HOTEN, N.NGAYSINH, N.TRANGTHAI,
                               V.TENVAITRO, G.TENGIADINH
                        FROM NGUOIDUNG N
                        LEFT JOIN VAITRO V ON N.MAVAITRO = V.MAVAITRO
                        LEFT JOIN GIADINH G ON N.MAGIADINH = G.MAGIADINH
                        WHERE N.MANGUOIDUNG = @MaND";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaND", loggedInUserId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtMaND.Text = reader["MANGUOIDUNG"].ToString();
                        txtHoTen.Text = reader["HOTEN"].ToString();

                        if (reader["NGAYSINH"] != DBNull.Value)
                            dtpNgaySinh.Value = Convert.ToDateTime(reader["NGAYSINH"]);

                        txtVaiTro.Text = reader["TENVAITRO"]?.ToString() ?? "Chưa cấp quyền";
                        txtGiaDinh.Text = reader["TENGIADINH"]?.ToString() ?? "Cá nhân";
                        txtTrangThai.Text = reader["TRANGTHAI"]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị dữ liệu: " + ex.Message);
            }
        }

        // 6. SỰ KIỆN LƯU THÔNG TIN (Có ràng buộc ngày sinh)
        private void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            // Kiểm tra họ tên trống
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!");
                return;
            }

            // Ràng buộc 2: Kiểm tra logic ngày sinh một lần nữa trước khi lưu
            if (dtpNgaySinh.Value.Date >= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh phải là một ngày trong quá khứ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE NGUOIDUNG 
                                   SET HOTEN = @HoTen, 
                                       NGAYSINH = @NgaySinh 
                                   WHERE MANGUOIDUNG = @MaND";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                    cmd.Parameters.AddWithValue("@MaND", loggedInUserId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                        LoadThongTinCaNhan();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy người dùng mã: " + loggedInUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật database: " + ex.Message);
            }
        }

        // 7. SỰ KIỆN ĐỔI MẬT KHẨU
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhauCu.Text) ||
              string.IsNullOrWhiteSpace(txtMatKhauMoi.Text) ||
              string.IsNullOrWhiteSpace(txtXacNhanMK.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các thông tin mật khẩu!");
                return;
            }

            if (txtMatKhauMoi.Text != txtXacNhanMK.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không trùng khớp với mật khẩu mới!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string checkSql = "SELECT MATKHAU FROM NGUOIDUNG WHERE MANGUOIDUNG = @MaND";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@MaND", loggedInUserId);
                    string passHienTai = checkCmd.ExecuteScalar()?.ToString();

                    if (txtMatKhauCu.Text != passHienTai)
                    {
                        MessageBox.Show("Mật khẩu hiện tại không chính xác!");
                        return;
                    }

                    string updateSql = "UPDATE NGUOIDUNG SET MATKHAU = @NewPass WHERE MANGUOIDUNG = @MaND";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@NewPass", txtMatKhauMoi.Text);
                    updateCmd.Parameters.AddWithValue("@MaND", loggedInUserId);

                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Đổi mật khẩu thành công!");

                    // Xóa trắng các ô nhập mật khẩu sau khi đổi xong
                    txtMatKhauCu.Clear();
                    txtMatKhauMoi.Clear();
                    txtXacNhanMK.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi mật khẩu: " + ex.Message);
            }
        }

        private void QuanLyThongTinCaNhanCuaNguoiDung_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}