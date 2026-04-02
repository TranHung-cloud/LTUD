using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

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

        // --- HÀM TỰ SINH MÃ PHƯƠNG PHÁP (PP1, PP2,...) ---
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
                        // Tách phần số sau chữ 'PP'
                        if (s.StartsWith("PP0"))
                        {
                            if (int.TryParse(s.Substring(2), out int num))
                            {
                                if (num > max) max = num;
                            }
                        }
                    }
                    maMoi = "PP0" + (max + 1);
                }
            }
            catch { maMoi = "PP01"; }
            return maMoi;
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
            // Kiểm tra ràng buộc tên không được để trống
            if (string.IsNullOrWhiteSpace(txtTenPP.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phương pháp!");
                return;
            }

            try
            {
                // Gọi hàm tự sinh mã trước khi thêm
                string maPPAuto = TuSinhMaPP();

                conn.Open();
                string query = "INSERT INTO PHUONGPHAPKHAUHAO (MAPP, TENPHUONGPHAP, TYLE) VALUES (@MaPP, @TenPP, @TyLe)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaPP", maPPAuto); // Sử dụng mã tự sinh
                cmd.Parameters.AddWithValue("@TenPP", txtTenPP.Text.Trim());

                // Xử lý tỷ lệ khấu hao (cho phép null hoặc số thực)
                if (string.IsNullOrWhiteSpace(txtTyLe.Text))
                {
                    cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);
                }
                else
                {
                    if (float.TryParse(txtTyLe.Text, out float tyLe))
                        cmd.Parameters.AddWithValue("@TyLe", tyLe);
                    else
                        cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);
                }

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

            try
            {
                conn.Open();
                string query = "UPDATE PHUONGPHAPKHAUHAO SET TENPHUONGPHAP = @TenPP, TYLE = @TyLe WHERE MAPP = @MaPP";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);
                cmd.Parameters.AddWithValue("@TenPP", txtTenPP.Text.Trim());

                if (string.IsNullOrWhiteSpace(txtTyLe.Text))
                    cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);
                else
                {
                    float tyLe;
                    if (float.TryParse(txtTyLe.Text, out tyLe))
                        cmd.Parameters.AddWithValue("@TyLe", tyLe);
                    else
                        cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);
                }

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
                    MessageBox.Show("Không thể xóa do phương pháp này đang được sử dụng ở bảng LOẠI TÀI SẢN!\nLỗi chi tiết: " + ex.Message);
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }
    }
}