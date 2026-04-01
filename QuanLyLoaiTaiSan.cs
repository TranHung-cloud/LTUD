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

            LoadDataLoaiTaiSan();
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

        private void dgvLoaiTaiSan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLoaiTaiSan.Rows[e.RowIndex];
                txtMaLoai.Text = row.Cells["MALOAI"].Value.ToString();
                txtMaPP.Text = row.Cells["MAPP"].Value.ToString();
                txtTenLoai.Text = row.Cells["TENLOAI"].Value.ToString();
                txtSoNam.Text = row.Cells["SONAMSUDUNG"].Value.ToString();
                txtChuKy.Text = row.Cells["CHUKYBAOTRI"].Value.ToString();

                txtMaLoai.Enabled = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaLoai.Clear();
            txtMaPP.Clear();
            txtTenLoai.Clear();
            txtSoNam.Clear();
            txtChuKy.Clear();
            txtMaLoai.Enabled = true;
            txtMaLoai.Focus();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO LOAITAISAN (MALOAI, MAPP, TENLOAI, SONAMSUDUNG, CHUKYBAOTRI) VALUES (@MaLoai, @MaPP, @TenLoai, @SoNam, @ChuKy)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLoai", txtMaLoai.Text);
                cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);
                cmd.Parameters.AddWithValue("@TenLoai", txtTenLoai.Text);
                cmd.Parameters.AddWithValue("@SoNam", int.Parse(txtSoNam.Text));
                cmd.Parameters.AddWithValue("@ChuKy", int.Parse(txtChuKy.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!");
                conn.Close();
                LoadDataLoaiTaiSan();
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
                string query = "UPDATE LOAITAISAN SET MAPP = @MaPP, TENLOAI = @TenLoai, SONAMSUDUNG = @SoNam, CHUKYBAOTRI = @ChuKy WHERE MALOAI = @MaLoai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLoai", txtMaLoai.Text);
                cmd.Parameters.AddWithValue("@MaPP", txtMaPP.Text);
                cmd.Parameters.AddWithValue("@TenLoai", txtTenLoai.Text);
                cmd.Parameters.AddWithValue("@SoNam", int.Parse(txtSoNam.Text));
                cmd.Parameters.AddWithValue("@ChuKy", int.Parse(txtChuKy.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!");
                conn.Close();
                LoadDataLoaiTaiSan();
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
                    MessageBox.Show("Lỗi xóa dữ liệu (Có thể do đang có Tài sản thuộc Loại này): " + ex.Message);
                    conn.Close();
                }
            }
        }
    }
}