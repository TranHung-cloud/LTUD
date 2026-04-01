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
            txtMaPP.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO PHUONGPHAPKHAUHAO (MAPP, TENPHUONGPHAP, TYLE) VALUES (@MaPP, @TenPP, @TyLe)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);
                cmd.Parameters.AddWithValue("@TenPP", txtTenPP.Text);

                if (string.IsNullOrWhiteSpace(txtTyLe.Text))
                    cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@TyLe", float.Parse(txtTyLe.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!");
                conn.Close();
                LoadDataPPKhauHao();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dữ liệu: " + ex.Message);
                conn.Close();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "UPDATE PHUONGPHAPKHAUHAO SET TENPHUONGPHAP = @TenPP, TYLE = @TyLe WHERE MAPP = @MaPP";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);
                cmd.Parameters.AddWithValue("@TenPP", txtTenPP.Text);

                if (string.IsNullOrWhiteSpace(txtTyLe.Text))
                    cmd.Parameters.AddWithValue("@TyLe", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@TyLe", float.Parse(txtTyLe.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!");
                conn.Close();
                LoadDataPPKhauHao();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa dữ liệu: " + ex.Message);
                conn.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
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
                    conn.Close();
                }
            }
        }
    }
}