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
        // 1. Chuỗi kết nối
        string connectionString = @"Server=(localdb)\MSSQLLocalDB; Database=QLTaiSanCaNhan; Integrated Security=True;";

        // 2. ID người dùng đăng nhập
        string loggedInUserId = "ND01";

        public QuanLyThongTinCaNhanCuaNguoiDung()
        {
            InitializeComponent();
        }

        // 3. Hàm định dạng giao diện
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

            LoadThongTinCaNhan();
        }

        // 5. Hàm tải dữ liệu từ SQL (Đã xóa Email và SĐT)
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
                MessageBox.Show("Lỗi hiển thị: " + ex.Message);
            }
        }

        // 6. SỰ KIỆN LƯU THÔNG TIN (Đã cập nhật SQL chỉ lưu Họ tên & Ngày sinh)
        private void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Sửa câu SQL: Bỏ cột SODIENTHOAI và EMAIL
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
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        // 7. SỰ KIỆN ĐỔI MẬT KHẨU
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhauCu.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhauMoi.Text) ||
                string.IsNullOrWhiteSpace(txtXacNhanMK.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các ô mật khẩu!");
                return;
            }

            if (txtMatKhauMoi.Text != txtXacNhanMK.Text)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận không khớp!");
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
                        MessageBox.Show("Mật khẩu cũ không chính xác!");
                        return;
                    }

                    string updateSql = "UPDATE NGUOIDUNG SET MATKHAU = @NewPass WHERE MANGUOIDUNG = @MaND";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@NewPass", txtMatKhauMoi.Text);
                    updateCmd.Parameters.AddWithValue("@MaND", loggedInUserId);

                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Đổi mật khẩu thành công!");

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
    }
}