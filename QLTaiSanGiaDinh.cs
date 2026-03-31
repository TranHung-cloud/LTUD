using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
        byte[] imageData = null;

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
            string sql = @"SELECT MATAISAN, HOTEN, TENTAISAN, ts.MALOAI, TENLOAI, CONVERT(DATE, NGAYMUA) AS NGAYMUA, NGUYENGIA, TINHTRANG, HINHANH 
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
                imageData = System.IO.File.ReadAllBytes(open.FileName);
                try
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pic.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    imageData = null;
                    pic.Image = null;
                }
            }
        }

        private byte[] ImageToByteArray(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if(NgayMua.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày mua không thể lớn hơn ngày hiện tại!");
                return;
            }
            if (String.IsNullOrEmpty(TenTaiSan.Text) || cboLoaiTaiSan.SelectedIndex == -1 || String.IsNullOrEmpty(NguyenGia.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            if (!decimal.TryParse(NguyenGia.Text, out decimal value))
            {
                MessageBox.Show("Vui lòng chỉ nhập số vào ô Nguyên giá!");
                NguyenGia.Focus();
                return;
            }

            if (value <= 0)
            {
                MessageBox.Show("Nguyên giá phải là số dương lớn hơn 0!");
                NguyenGia.Focus();
                return;
            }

            try
            {
                connectData();
                if (imageData == null && pic.Image != null)
                {
                    imageData = ImageToByteArray(pic.Image);
                }

                string maSql = "SELECT MAX(MATAISAN) FROM TAISAN";
                SqlCommand maCmd = new SqlCommand(maSql, conn);
                object maObj = maCmd.ExecuteScalar();
                string nextId;
                if (maObj == null || maObj == DBNull.Value)
                {
                    nextId = "TS001";
                }
                else
                {
                    string maxId = maObj.ToString();
                    string prefix = new string(maxId.TakeWhile(c => !char.IsDigit(c)).ToArray());
                    string numPart = new string(maxId.SkipWhile(c => !char.IsDigit(c)).ToArray());
                    int num = 0;
                    if (!int.TryParse(numPart, out num)) num = 0;
                    num += 1;
                    nextId = prefix + num.ToString("D2");
                }

                string sql = "INSERT INTO TAISAN(MATAISAN, MANGUOIDUNG, MALOAI, TENTAISAN, NGAYMUA, NGUYENGIA, TINHTRANG, PHAMVI, HINHANH) VALUES(@mts, @mnd, @ml, @tts, @nm, @ng, @tt, @pv, @ha)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mts", nextId);
                cmd.Parameters.AddWithValue("@mnd", maNguoiDung);
                cmd.Parameters.AddWithValue("@ml", cboLoaiTaiSan.SelectedValue);
                cmd.Parameters.AddWithValue("@tts", TenTaiSan.Text);
                cmd.Parameters.AddWithValue("@nm", NgayMua.Value);
                cmd.Parameters.AddWithValue("@ng", Convert.ToDecimal(NguyenGia.Text));
                cmd.Parameters.AddWithValue("@tt", "Tốt");
                cmd.Parameters.AddWithValue("@pv", "Gia đình");
                string base64 = null;
                if (imageData != null)
                {
                    base64 = Convert.ToBase64String(imageData);
                }

                var p = new SqlParameter("@ha", SqlDbType.NVarChar, -1) { Value = (object)base64 ?? DBNull.Value };
                cmd.Parameters.Add(p);
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
                MaTaiSan.Text = row.Cells["MATAISAN"].Value?.ToString();
                cboLoaiTaiSan.SelectedValue = row.Cells["MALOAI"].Value;
                TenTaiSan.Text = row.Cells["TENTAISAN"].Value?.ToString();
                NgayMua.Value = Convert.ToDateTime(row.Cells["NGAYMUA"].Value);
                NguyenGia.Text = row.Cells["NGUYENGIA"].Value?.ToString();

                var cellVal = row.Cells["HINHANH"].Value;
                // default clear
                imageData = null;
                pic.Image = null;

                if (cellVal == DBNull.Value || cellVal == null)
                {
                    // no image stored
                }
                else if (cellVal is byte[])
                {
                    imageData = (byte[])cellVal;
                    try
                    {
                        using (var ms = new MemoryStream(imageData))
                        {
                            pic.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        pic.Image = null;
                    }
                }
                else if (cellVal is string)
                {
                    string s = cellVal.ToString();
                    try
                    {
                        var bytes = Convert.FromBase64String(s);
                        imageData = bytes;
                        using (var ms = new MemoryStream(bytes))
                        {
                            pic.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        try
                        {
                            if (File.Exists(s))
                            {
                                pic.Image = Image.FromFile(s);
                            }
                        }
                        catch
                        {
                            pic.Image = null;
                        }
                    }
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
            imageData = null;
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                connectData();
                string famSql = "SELECT TENGIADINH FROM GIADINH WHERE MAGIADINH = @ma";
                SqlCommand famCmd = new SqlCommand(famSql, conn);
                famCmd.Parameters.AddWithValue("@ma", maGiaDinh);
                string tenGD = famCmd.ExecuteScalar().ToString();

                ReportParameter[] parameters = new ReportParameter[]
                {
                    new ReportParameter("TenGiaDinh", tenGD)
                };

                string sql = @"  SELECT ts.MATAISAN, TENTAISAN, HOTEN AS DAIDIENSOHUU, TENLOAI AS LOAITAISAN, CONVERT(DATE, NGAYMUA) AS NGAYMUA, NGUYENGIA, MIN(GIATRICONLAISAUKHAUHAO) AS GIATRIHIENTAI
                                 FROM TAISAN ts
                                 JOIN NGUOIDUNG nd ON ts.MANGUOIDUNG = nd.MANGUOIDUNG
                                 JOIN LOAITAISAN lts ON ts.MALOAI = lts.MALOAI
                                 JOIN COKHAUHAO ckh ON ckh.MATAISAN = ts.MATAISAN
                                 WHERE nd.MAGIADINH = @ma AND PHAMVI = N'Gia đình'
                                 GROUP BY ts.MATAISAN, TENTAISAN, HOTEN, TENLOAI, NGAYMUA, NGUYENGIA, ts.MALOAI";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@ma", maGiaDinh);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Form repForm = new Form();
                repForm.Text = "Báo cáo tài sản gia đình";
                repForm.WindowState = FormWindowState.Maximized;

                ReportViewer rv = new ReportViewer();
                rv.ProcessingMode = ProcessingMode.Local;
                rv.Dock = DockStyle.Fill;

                string reportPath = Path.Combine(Application.StartupPath, "ThongKeTaiSanGiaDinh.rdlc");
                rv.LocalReport.ReportPath = reportPath;

                var rds = new ReportDataSource("ThongTinTaiSan", dt);
                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(rds);
                rv.LocalReport.SetParameters(parameters);

                rv.RefreshReport();

                repForm.Controls.Add(rv);
                repForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo báo cáo: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (NgayMua.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày mua không thể lớn hơn ngày hiện tại!");
                return;
            }
            if (String.IsNullOrEmpty(TenTaiSan.Text) || cboLoaiTaiSan.SelectedIndex == -1 || String.IsNullOrEmpty(NguyenGia.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            if (!decimal.TryParse(NguyenGia.Text, out decimal value))
            {
                MessageBox.Show("Vui lòng chỉ nhập số vào ô Nguyên giá!");
                NguyenGia.Focus();
                return;
            }

            if (value <= 0)
            {
                MessageBox.Show("Nguyên giá phải là số dương lớn hơn 0!");
                NguyenGia.Focus();
                return;
            }


            try
            {
                connectData();
                string searchDepSql = "SELECT COUNT(*) FROM COKHAUHAO WHERE MATAISAN = @ma";
                SqlCommand searchDepCmd = new SqlCommand(searchDepSql, conn);
                searchDepCmd.Parameters.AddWithValue("@ma", MaTaiSan.Text.Trim());
                bool hasDepreciation = Convert.ToInt32(searchDepCmd.ExecuteScalar()) > 0;

                string currentMaLoai = "", currentMaNguoiDung = "";
                DateTime currentNgayMua = DateTime.MinValue;
                decimal currentNguyenGia = 0;

                string searchSql = @"SELECT MANGUOIDUNG, MALOAI, NGAYMUA, NGUYENGIA FROM TAISAN WHERE MATAISAN = @ma";
                SqlCommand cmd = new SqlCommand(searchSql, conn);
                cmd.Parameters.AddWithValue("@ma", MaTaiSan.Text.Trim());

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentMaNguoiDung = reader["MANGUOIDUNG"].ToString().Trim();
                        currentMaLoai = reader["MALOAI"].ToString().Trim();
                        currentNgayMua = Convert.ToDateTime(reader["NGAYMUA"]);
                        currentNguyenGia = Convert.ToDecimal(reader["NGUYENGIA"]);
                    }
                }


                if (currentMaNguoiDung != maNguoiDung.Trim())
                {
                    MessageBox.Show("Bạn không có quyền sửa tài sản này!");
                    return;
                }

                string updateSql = "";
                if (hasDepreciation)
                {
                    if (currentMaLoai != cboLoaiTaiSan.SelectedValue.ToString().Trim())
                    {
                        MessageBox.Show("Tài sản đã có khấu hao, không thể đổi loại!"); return;
                    }
                    if (currentNgayMua.Date != NgayMua.Value.Date)
                    {
                        MessageBox.Show("Tài sản đã có khấu hao, không thể đổi ngày mua!"); return;
                    }
                    if (currentNguyenGia != Convert.ToDecimal(NguyenGia.Text))
                    {
                        MessageBox.Show("Tài sản đã có khấu hao, không thể đổi nguyên giá!"); return;
                    }

                    updateSql = "UPDATE TAISAN SET TENTAISAN = @tts, HINHANH = @ha WHERE MATAISAN = @ma";
                }
                else
                {
                    updateSql = "UPDATE TAISAN SET TENTAISAN = @tts, MALOAI = @ml, NGUYENGIA = @ng, NGAYMUA = @nm, HINHANH = @ha WHERE MATAISAN = @ma";
                }

                SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                updateCmd.Parameters.AddWithValue("@tts", TenTaiSan.Text);
                updateCmd.Parameters.AddWithValue("@ma", MaTaiSan.Text.Trim());

                string base64 = (imageData != null) ? Convert.ToBase64String(imageData) : null;
                updateCmd.Parameters.Add(new SqlParameter("@ha", SqlDbType.NVarChar, -1) { Value = (object)base64 ?? DBNull.Value });

                if (!hasDepreciation)
                {
                    updateCmd.Parameters.AddWithValue("@ml", cboLoaiTaiSan.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@ng", Convert.ToDecimal(NguyenGia.Text));
                    updateCmd.Parameters.AddWithValue("@nm", NgayMua.Value);
                }

                updateCmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật tài sản thành công");
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
