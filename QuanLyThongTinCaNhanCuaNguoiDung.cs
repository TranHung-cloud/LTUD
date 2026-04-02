using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Thêm thư viện này để kết nối SQL Server

namespace LTUD
{
    public partial class QuanLyThongTinCaNhanCuaNguoiDung : Form
    {
        // 1. CHUỖI KẾT NỐI (Bắt buộc: Hãy sửa lại tên Server cho khớp với máy bạn đang mượn)
        // Thay đổi Database từ QuanLyTaiSan thành QLTaiSanCaNhan
        string connectionString = @"Server=(localdb)\MSSQLLocalDB; Database=QLTaiSanCaNhan; Integrated Security=True;"; SqlConnection conn;

        // 2. GIẢ LẬP NGƯỜI DÙNG ĐĂNG NHẬP 
        // (Tạm thời gán bằng ND01, sau này làm chức năng Đăng nhập thì thay bằng biến động)
        string loggedInUserId = "ND01";

        public QuanLyThongTinCaNhanCuaNguoiDung()
        {
            InitializeComponent();
        }

        // 3. HÀM ĐỔI MÀU GIAO DIỆN (Sử dụng đúng bảng màu nhóm bạn yêu cầu)
        private void ApplyButtonColors(Button btn)
        {
            btn.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btn.ForeColor = ColorTranslator.FromHtml("#006A71");
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }

        // 4. SỰ KIỆN KHI MỞ FORM LÊN (Chạy đầu tiên)
        private void QuanLyThongTinCaNhanCuaNguoiDung_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#9ACBD0"); // Tô màu nền Form

            // Đổi màu 2 nút bấm (Lưu ý: Tên nút phải khớp với thuộc tính Name bên Designer)
            ApplyButtonColors(btnLuuThongTin);
            ApplyButtonColors(btnDoiMatKhau);

            // Gọi hàm lấy dữ liệu đổ lên màn hình
            LoadThongTinCaNhan();
        }

        // 5. HÀM TẢI THÔNG TIN CÁ NHÂN LÊN GIAO DIỆN
        private void LoadThongTinCaNhan()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // Lấy thông tin cá nhân và JOIN với bảng Phụ để lấy Tên Vai trò, Tên Gia đình
                string query = @"
                    SELECT N.MANGUOIDUNG, N.HOTEN, N.NGAYSINH, N.SODIENTHOAI, N.EMAIL, N.TRANGTHAI,
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
                    // Đổ dữ liệu vào các TextBox / DateTimePicker
                    txtMaND.Text = reader["MANGUOIDUNG"].ToString();
                    txtHoTen.Text = reader["HOTEN"].ToString();
                    dtpNgaySinh.Value = Convert.ToDateTime(reader["NGAYSINH"]);

                    // Dùng kiểm tra DBNull.Value để tránh lỗi khi người dùng chưa nhập SĐT hoặc Email
                    txtSoDienThoai.Text = reader["SODIENTHOAI"] != DBNull.Value ? reader["SODIENTHOAI"].ToString() : "";
                    txtEmail.Text = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : "";

                    // Đổ dữ liệu vào các ô CHỈ XEM (Vai trò, Gia đình, Trạng thái)
                    txtVaiTro.Text = reader["TENVAITRO"] != DBNull.Value ? reader["TENVAITRO"].ToString() : "Chưa cấp quyền";
                    txtGiaDinh.Text = reader["TENGIADINH"] != DBNull.Value ? reader["TENGIADINH"].ToString() : "Cá nhân độc lập";
                    txtTrangThai.Text = reader["TRANGTHAI"] != DBNull.Value ? reader["TRANGTHAI"].ToString() : "";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message);
            }
        }

        // 6. XỬ LÝ SỰ KIỆN KHI BẤM NÚT "LƯU THÔNG TIN"
        private void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            // Bắt lỗi không cho phép để trống Họ Tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!");
                return;
            }

            try
            {
                conn.Open();
                // Chỉ cho phép update 4 trường: Họ Tên, Ngày Sinh, SĐT, Email
                string query = "UPDATE NGUOIDUNG SET HOTEN = @HoTen, NGAYSINH = @NgaySinh, SODIENTHOAI = @SoDienThoai, EMAIL = @Email WHERE MANGUOIDUNG = @MaND";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Lấy dữ liệu từ giao diện truyền vào câu SQL
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@MaND", loggedInUserId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thông tin cá nhân thành công!");
                conn.Close();

                // Load lại để hiển thị dữ liệu mới nhất

                LoadThongTinCaNhan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật thông tin: " + ex.Message);
                conn.Close();
            }
        }

        // 7. XỬ LÝ SỰ KIỆN KHI BẤM NÚT "ĐỔI MẬT KHẨU"
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra xem có ô mật khẩu nào bị bỏ trống không
            if (string.IsNullOrWhiteSpace(txtMatKhauCu.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhauMoi.Text) ||
                string.IsNullOrWhiteSpace(txtXacNhanMK.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin mật khẩu!");
                return;
            }

            // Bước 2: Kiểm tra mật khẩu mới và xác nhận có khớp nhau không
            if (txtMatKhauMoi.Text != txtXacNhanMK.Text)
            {
                MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận không trùng khớp!");
                return;
            }

            try
            {
                conn.Open();

                // Bước 3: Lấy mật khẩu cũ từ Database lên để so sánh
                string checkQuery = "SELECT MATKHAU FROM NGUOIDUNG WHERE MANGUOIDUNG = @MaND";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@MaND", loggedInUserId);
                string currentPasswordInDb = checkCmd.ExecuteScalar()?.ToString();

                // Nếu người dùng nhập sai mật khẩu cũ thì chặn lại
                if (txtMatKhauCu.Text != currentPasswordInDb)
                {
                    MessageBox.Show("Mật khẩu hiện tại không chính xác!");
                    conn.Close();
                    return;
                }

                // Bước 4: Nếu mọi thứ đúng hết thì tiến hành update mật khẩu mới
                string updateQuery = "UPDATE NGUOIDUNG SET MATKHAU = @MatKhauMoi WHERE MANGUOIDUNG = @MaND";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@MatKhauMoi", txtMatKhauMoi.Text);
                updateCmd.Parameters.AddWithValue("@MaND", loggedInUserId);

                updateCmd.ExecuteNonQuery();
                MessageBox.Show("Đổi mật khẩu thành công!");

                // Thành công thì xóa trắng các ô nhập mật khẩu cho an toàn
                txtMatKhauCu.Clear();
                txtMatKhauMoi.Clear();
                txtXacNhanMK.Clear();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi mật khẩu: " + ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();

            }
        }

        
    }
}