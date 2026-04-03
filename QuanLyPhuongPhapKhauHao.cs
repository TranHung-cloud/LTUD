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
    public partial class QuanLyPhuongPhapKhauHao : Form
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB; Database=QuanLyTaiSan; Integrated Security=True;";
        SqlConnection conn;

        public QuanLyPhuongPhapKhauHao()
        {
            InitializeComponent();
            // Khóa ô mã phương pháp để hệ thống tự sinh
            txtMaPP.ReadOnly = true;
            txtMaPP.BackColor = Color.LightGray;
        }

        private void ApplyButtonColors(Button btn)
        {
            btn.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btn.ForeColor = ColorTranslator.FromHtml("#006A71");
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }

        private void QuanLyPhuongPhapKhauHao_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#9ACBD0");

            ApplyButtonColors(btnThem);
            ApplyButtonColors(btnSua);
            ApplyButtonColors(btnXoa);
            ApplyButtonColors(btnLamMoi);

            LoadDataPPKhauHao();
        }

        private void LoadDataPPKhauHao()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "SELECT * FROM PHUONGPHAPKHAUHAO";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvPhuongPhap.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // --- HÀM TỰ SINH MÃ PHƯƠNG PHÁP
        private string TuSinhMaPP()
        {
            string maMoi = "PP01";
            try
            {
                using (SqlConnection tempConn = new SqlConnection(connectionString))
                {
                    tempConn.Open();
                    string query = "SELECT MAPP FROM PHUONGPHAPKHAUHAO";
                    SqlCommand cmd = new SqlCommand(query, tempConn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int max = 0;
                    while (reader.Read())
                    {
                        string s = reader["MAPP"].ToString();

                        // Chỉ kiểm tra chuỗi bắt đầu bằng "PP" và có độ dài lớn hơn 2
                        if (s.StartsWith("PP") && s.Length > 2)
                        {
                            // Cắt bỏ 2 ký tự đầu ("PP"), lấy phần số phía sau để ép kiểu
                            if (int.TryParse(s.Substring(2), out int num))
                            {
                                if (num > max) max = num;
                            }
                        }
                    }

                    // max + 1 là số tiếp theo. 
                    // .ToString("D2") sẽ định dạng: 1 -> "01", 9 -> "09", 10 -> "10", 99 -> "99"
                    maMoi = "PP" + (max + 1).ToString("D2");
                }
            }
            catch
            {
                maMoi = "PP01";
            }
            return maMoi;
        }

        // --- HÀM MỚI: KIỂM TRA XEM PHƯƠNG PHÁP NÀY ĐÃ ĐƯỢC GÁN CHO LOẠI TÀI SẢN NÀO CHƯA ---
        private bool KiemTraPPDangSuDung(string maPP)
        {
            bool dangSuDung = false;
            try
            {
                using (SqlConnection tempConn = new SqlConnection(connectionString))
                {
                    tempConn.Open();
                    // Đếm xem có Loại tài sản nào đang dùng MAPP này không
                    string query = "SELECT COUNT(*) FROM LOAITAISAN WHERE MAPP = @MaPP";
                    SqlCommand cmd = new SqlCommand(query, tempConn);
                    cmd.Parameters.AddWithValue("@MaPP", maPP);

                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        dangSuDung = true;
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi kết nối
            }
            return dangSuDung;
        }

        private void dgvPhuongPhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPhuongPhap.Rows[e.RowIndex];
                txtMaPP.Text = row.Cells["MAPP"].Value.ToString();
                txtTenPP.Text = row.Cells["TENPHUONGPHAP"].Value.ToString();
                txtTyLe.Text = row.Cells["TYLE"].Value.ToString();

                txtMaPP.Enabled = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaPP.Clear();
            txtTenPP.Clear();
            txtTyLe.Clear();
            txtMaPP.Enabled = true;
            txtTenPP.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenPP.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phương pháp!");
                return;
            }

            // --- CHỈNH SỬA: KIỂM TRA NGƯỜI DÙNG NHẬP CHỮ VÀO Ô TỶ LỆ ---
            float? tyLe = null; // Dùng kiểu nullable float để xử lý trường hợp cho phép rỗng
            if (!string.IsNullOrWhiteSpace(txtTyLe.Text))
            {
                if (!float.TryParse(txtTyLe.Text, out float parsedTyLe) || parsedTyLe < 0)
                {
                    MessageBox.Show("Lỗi: 'Tỷ lệ khấu hao' phải là một số không âm (Không được nhập chữ)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTyLe.Focus();
                    return;
                }
                tyLe = parsedTyLe;
            }

            try
            {
                string maPPAuto = TuSinhMaPP();

                conn.Open();
                string query = "INSERT INTO PHUONGPHAPKHAUHAO (MAPP, TENPHUONGPHAP, TYLE) VALUES (@MaPP, @TenPP, @TyLe)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaPP", maPPAuto);
                cmd.Parameters.AddWithValue("@TenPP", txtTenPP.Text.Trim());

                // Gán tỷ lệ hoặc gán NULL nếu để trống
                if (tyLe.HasValue)
                    cmd.Parameters.AddWithValue("@TyLe", tyLe.Value);
                else
                    cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công! Mã phương pháp mới là: " + maPPAuto);
                conn.Close();
                LoadDataPPKhauHao();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dữ liệu: " + ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPP.Text))
            {
                MessageBox.Show("Vui lòng chọn phương pháp cần sửa!");
                return;
            }

            // --- CHỈNH SỬA: RÀNG BUỘC KHÔNG CHO SỬA NẾU ĐÃ ĐƯỢC ÁP DỤNG ---
            if (KiemTraPPDangSuDung(txtMaPP.Text))
            {
                MessageBox.Show("Từ chối: Không thể sửa Phương pháp này vì nó đã được áp dụng cho các Loại tài sản trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- CHỈNH SỬA: KIỂM TRA NGƯỜI DÙNG NHẬP CHỮ VÀO Ô TỶ LỆ ---
            float? tyLe = null;
            if (!string.IsNullOrWhiteSpace(txtTyLe.Text))
            {
                if (!float.TryParse(txtTyLe.Text, out float parsedTyLe) || parsedTyLe < 0)
                {
                    MessageBox.Show("Lỗi: 'Tỷ lệ khấu hao' phải là một số không âm (Không được nhập chữ)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTyLe.Focus();
                    return;
                }
                tyLe = parsedTyLe;
            }

            try
            {
                conn.Open();
                string query = "UPDATE PHUONGPHAPKHAUHAO SET TENPHUONGPHAP = @TenPP, TYLE = @TyLe WHERE MAPP = @MaPP";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);
                cmd.Parameters.AddWithValue("@TenPP", txtTenPP.Text.Trim());

                // Gán tỷ lệ hoặc gán NULL nếu để trống
                if (tyLe.HasValue)
                    cmd.Parameters.AddWithValue("@TyLe", tyLe.Value);
                else
                    cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!");
                conn.Close();
                LoadDataPPKhauHao();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa dữ liệu: " + ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPP.Text)) return;

            // --- CHỈNH SỬA: RÀNG BUỘC KHÔNG CHO XÓA NẾU ĐÃ ĐƯỢC ÁP DỤNG ---
            if (KiemTraPPDangSuDung(txtMaPP.Text))
            {
                MessageBox.Show("Từ chối: Không thể xóa Phương pháp này vì nó đang được liên kết với các Loại tài sản trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa phương pháp này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM PHUONGPHAPKHAUHAO WHERE MAPP = @MaPP";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công!");
                    conn.Close();
                    LoadDataPPKhauHao();
                    btnLamMoi_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa do phương pháp này đang được sử dụng!\nLỗi chi tiết: " + ex.Message);
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }
    }
}