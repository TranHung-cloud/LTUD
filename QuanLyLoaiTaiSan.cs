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
    public partial class QuanLyLoaiTaiSan : Form
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB; Database=QuanLyTaiSan; Integrated Security=True;";
        SqlConnection conn;

        public QuanLyLoaiTaiSan()
        {
            InitializeComponent();
            // CHỈNH SỬA: Khóa ô mã loại vì hệ thống sẽ tự sinh
            txtMaLoai.ReadOnly = true;
            txtMaLoai.BackColor = Color.LightGray;
        }

        private void ApplyButtonColors(Button btn)
        {
            btn.BackColor = System.Drawing.ColorTranslator.FromHtml("#F2EFE7");
            btn.ForeColor = System.Drawing.ColorTranslator.FromHtml("#006A71");
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }

        private void QuanLyLoaiTaiSan_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#9ACBD0");

            ApplyButtonColors(btnThem);
            ApplyButtonColors(btnSua);
            ApplyButtonColors(btnXoa);
            ApplyButtonColors(btnLamMoi);

            LoadComboBoxMaPP();
            LoadDataLoaiTaiSan();
        }

        private void LoadComboBoxMaPP()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "SELECT MAPP, TENPHUONGPHAP FROM PHUONGPHAPKHAUHAO";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboMaPP.DataSource = dt;
                cboMaPP.DisplayMember = "MAPP";
                cboMaPP.ValueMember = "MAPP";

                cboMaPP.SelectedIndex = -1;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phương pháp: " + ex.Message);
            }
        }

        private void LoadDataLoaiTaiSan()
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "SELECT * FROM LOAITAISAN";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvLoaiTaiSan.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        // --- HÀM MỚI: TỰ ĐỘNG LẤY MÃ LỚN NHẤT + 1 ---
        private string TuSinhMaLoai()
        {
            string maMoi = "L01";
            try
            {
                using (SqlConnection tempConn = new SqlConnection(connectionString))
                {
                    tempConn.Open();
                    string query = "SELECT MALOAI FROM LOAITAISAN";
                    SqlCommand cmd = new SqlCommand(query, tempConn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int max = 0;
                    while (reader.Read())
                    {
                        string s = reader["MALOAI"].ToString();
                        if (s.StartsWith("L0"))
                        {
                            int num = int.Parse(s.Substring(1));
                            if (num > max) max = num;
                        }
                    }
                    maMoi = "L0" + (max + 1);
                }
            }
            catch { maMoi = "L01"; }
            return maMoi;
        }

        private void dgvLoaiTaiSan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLoaiTaiSan.Rows[e.RowIndex];
                txtMaLoai.Text = row.Cells["MALOAI"].Value.ToString();
                cboMaPP.SelectedValue = row.Cells["MAPP"].Value.ToString();
                txtTenLoai.Text = row.Cells["TENLOAI"].Value.ToString();
                txtSoNam.Text = row.Cells["SONAMSUDUNG"].Value.ToString();
                txtChuKy.Text = row.Cells["CHUKYBAOTRI"].Value.ToString();

                txtMaLoai.Enabled = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaLoai.Clear();
            cboMaPP.SelectedIndex = -1;
            txtTenLoai.Clear();
            txtSoNam.Clear();
            txtChuKy.Clear();
            txtMaLoai.Enabled = true;
            txtMaLoai.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // CHỈNH SỬA: Kiểm tra tên loại trước khi thêm
            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại tài sản!");
                return;
            }

            if (cboMaPP.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Mã phương pháp!");
                return;
            }

            try
            {
                // CHỈNH SỬA: Gọi hàm tự sinh mã
                string maTuDong = TuSinhMaLoai();

                conn.Open();
                string query = "INSERT INTO LOAITAISAN (MALOAI, MAPP, TENLOAI, SONAMSUDUNG, CHUKYBAOTRI) VALUES (@MaLoai, @MaPP, @TenLoai, @SoNam, @ChuKy)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaLoai", maTuDong); // Dùng mã tự sinh
                cmd.Parameters.AddWithValue("@MaPP", cboMaPP.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@TenLoai", txtTenLoai.Text);

                // CHỈNH SỬA: Dùng TryParse để tránh lỗi nhập chữ vào ô số
                int soNam, chuKy;
                int.TryParse(txtSoNam.Text, out soNam);
                int.TryParse(txtChuKy.Text, out chuKy);
                cmd.Parameters.AddWithValue("@SoNam", soNam);
                cmd.Parameters.AddWithValue("@ChuKy", chuKy);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công! Mã mới là: " + maTuDong);
                conn.Close();
                LoadDataLoaiTaiSan();
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
            if (string.IsNullOrEmpty(txtMaLoai.Text))
            {
                MessageBox.Show("Vui lòng chọn loại tài sản cần sửa!");
                return;
            }

            try
            {
                conn.Open();
                string query = "UPDATE LOAITAISAN SET MAPP = @MaPP, TENLOAI = @TenLoai, SONAMSUDUNG = @SoNam, CHUKYBAOTRI = @ChuKy WHERE MALOAI = @MaLoai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLoai", txtMaLoai.Text);
                cmd.Parameters.AddWithValue("@MaPP", cboMaPP.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@TenLoai", txtTenLoai.Text);

                int soNam, chuKy;
                int.TryParse(txtSoNam.Text, out soNam);
                int.TryParse(txtChuKy.Text, out chuKy);
                cmd.Parameters.AddWithValue("@SoNam", soNam);
                cmd.Parameters.AddWithValue("@ChuKy", chuKy);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!");
                conn.Close();
                LoadDataLoaiTaiSan();
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
            if (string.IsNullOrEmpty(txtMaLoai.Text)) return;

            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa loại tài sản này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM LOAITAISAN WHERE MALOAI = @MaLoai";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaLoai", txtMaLoai.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công!");
                    conn.Close();
                    LoadDataLoaiTaiSan();
                    btnLamMoi_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: Có thể loại này đang được dùng cho một tài sản khác!");
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }
    }
}