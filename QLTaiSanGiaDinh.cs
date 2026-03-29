using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace LTUD
{
    public partial class QLTaiSanGiaDinh : Form
    {
        string connectString = @"Server=Hung\SQLEXPRESS; Database=QLTaiSan_LTUD; Integrated Security=True";
        SqlConnection conn;
        string maNguoiDung = "ND01";
        string maGiaDinh = "";
        string pathHinh = "";

        //public QLTaiSanGiaDinh(string ma)
        public QLTaiSanGiaDinh()
        {
            InitializeComponent();
            //maNguoiDung = ma;
        }
        private void QLTaiSanGiaDinh_Load(object sender, EventArgs e)
        {
            checkGiaDinh();
            loadData();
            loadCbo();
            cboLoaiTaiSan.SelectedIndex = -1;
        }

        void connectData()
        {
            conn = new SqlConnection(connectString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        void loadData()
        {
            connectData();
            string sql = @"SELECT MATAISAN, HOTEN, TENTAISAN, ts.MALOAI, TENLOAI, NGAYMUA, NGUYENGIA, TINHTRANG, HINHANH 
                        FROM TAISAN ts JOIN NGUOIDUNG nd ON ts.MANGUOIDUNG = nd.MANGUOIDUNG 
                        JOIN LOAITAISAN lts ON ts.MALOAI = lts.MALOAI 
                        WHERE nd.MAGIADINH = @ma AND PHAMVI = N'Gia đình'";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@ma", maGiaDinh);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            dgvTSGD.DataSource = dt;

            dgvTSGD.Columns[0].HeaderText = "Mã Tài Sản";
            dgvTSGD.Columns[1].HeaderText = "Đại diện Sở Hữu";
            dgvTSGD.Columns[2].HeaderText = "Tên Tài Sản";
            dgvTSGD.Columns[3].Visible = false;
            dgvTSGD.Columns[4].HeaderText = "Loại Tài Sản";
            dgvTSGD.Columns[5].HeaderText = "Ngày Mua";
            dgvTSGD.Columns[6].HeaderText = "Nguyên Giá";
            dgvTSGD.Columns[7].HeaderText = "Tình Trạng";
            dgvTSGD.Columns[8].Visible = false;
        }

        void loadCbo()
        {
            String sql = "SELECT * FROM LOAITAISAN";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cboLoaiTaiSan.DataSource = dt;
            cboLoaiTaiSan.ValueMember = "MALOAI";
            cboLoaiTaiSan.DisplayMember = "TENLOAI";
        }

        void checkGiaDinh()
        {
            try
            {
                connectData();
                string sql = "SELECT MAGIADINH FROM NGUOIDUNG WHERE MANGUOIDUNG = @ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", maNguoiDung);
                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    MessageBox.Show("Bạn đang không thuộc gia đình nào");
                    this.Close();
                    return;
                }

                maGiaDinh = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pathHinh = open.FileName;
                pic.Image = Image.FromFile(pathHinh);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connectData();
                string sql = "INSERT INTO TAISAN VALUES(@mts, @mnd, @ml, @tts, @nm, @ng, @tt, @pv, @ha)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mts", MaTaiSan.Text);
                cmd.Parameters.AddWithValue("@mnd", maNguoiDung);
                cmd.Parameters.AddWithValue("@ml", cboLoaiTaiSan.SelectedValue);
                cmd.Parameters.AddWithValue("@tts", TenTaiSan.Text);
                cmd.Parameters.AddWithValue("@nm", NgayMua.Value);
                cmd.Parameters.AddWithValue("@ng", Convert.ToDecimal(NguyenGia.Text));
                cmd.Parameters.AddWithValue("@tt", "Tốt");
                cmd.Parameters.AddWithValue("@pv", "Gia đình");
                cmd.Parameters.AddWithValue("@ha", pathHinh);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm tài sản thành công");
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                connectData();
                String checkSql = "SELECT MANGUOIDUNG FROM TAISAN WHERE MATAISAN = @ma";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@ma", MaTaiSan.Text);
                object result = checkCmd.ExecuteScalar();
                if (result.ToString().Trim() != maNguoiDung.Trim())
                {
                    MessageBox.Show("Bạn không thể xóa tài sản của người khác!");
                    return;
                }
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa tài sản này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    String delSql = "DELETE FROM TAISAN WHERE MATAISAN = @ma";
                    SqlCommand delCmd = new SqlCommand(delSql, conn);
                    delCmd.Parameters.AddWithValue("@ma", MaTaiSan.Text);
                    delCmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xóa tài sản thành công");
                    loadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void dgvTSGD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTSGD.Rows[e.RowIndex];
                MaTaiSan.Text = row.Cells["MATAISAN"].Value.ToString();
                cboLoaiTaiSan.SelectedValue = row.Cells["MALOAI"].Value;
                TenTaiSan.Text = row.Cells["TENTAISAN"].Value.ToString();
                NgayMua.Value = Convert.ToDateTime(row.Cells["NGAYMUA"].Value);
                NguyenGia.Text = row.Cells["NGUYENGIA"].Value.ToString();
                pathHinh = row.Cells["HINHANH"].Value.ToString();
                if (!string.IsNullOrEmpty(pathHinh))
                {
                    pic.Image = Image.FromFile(pathHinh);
                }
                else
                {
                    pic.Image = null;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MaTaiSan.Clear();
            cboLoaiTaiSan.SelectedIndex = -1;
            TenTaiSan.Clear();
            NgayMua.Value = DateTime.Now;
            NguyenGia.Clear();
            pathHinh = "";
            pic.Image = null;
            MaTaiSan.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(MaTaiSan.Text))
            {
                MessageBox.Show("Vui lòng chọn tài sản muốn xem chi tiết");
                return;
            }
            ChiTietTaiSanGiaDinh frm = new ChiTietTaiSanGiaDinh(MaTaiSan.Text);
            frm.ShowDialog(this);
        }

        private void SearchMTS_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(SearchMTS.Text))
            {
                loadData();
                return;
            }
            try
            {
                connectData();
                String sql = @"SELECT MATAISAN, HOTEN, TENTAISAN, ts.MALOAI, TENLOAI, NGAYMUA, NGUYENGIA, TINHTRANG, HINHANH 
                            FROM TAISAN ts JOIN NGUOIDUNG nd ON ts.MANGUOIDUNG = nd.MANGUOIDUNG 
                            JOIN LOAITAISAN lts ON ts.MALOAI = lts.MALOAI 
                            WHERE MATAISAN LIKE @mts AND nd.MAGIADINH = @mgd AND PHAMVI = N'Gia đình'";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@mts", "%" + SearchMTS.Text + "%");
                da.SelectCommand.Parameters.AddWithValue("@mgd", maGiaDinh);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTSGD.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }
    }
}
