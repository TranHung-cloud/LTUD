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

namespace LTUD
{
    public partial class FormQLTS_CN : Form
    {
        private SqlConnection conn;
        private string userId ;
        public FormQLTS_CN(string userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void FormQLTS_CN_Load(object sender, EventArgs e)
        {
            InitDesign();
            string connectionString = @"Server =.\SQLEXPRESS; Database = QLTaiSan_LTUD ; Integrated Security = True; TrustServerCertificate=True";
            conn = new SqlConnection(connectionString);
            UpdateDepreciation();
            ShowData();
        }
        private void InitDesign()
        {
            this.BackColor = ColorTranslator.FromHtml("#9ACBD0");
            btnToMain.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnToMain.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnThem.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnThem.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnXoa.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnXoa.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnSua.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnSua.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnChiTiet.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnChiTiet.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnTimKiem.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnTimKiem.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnThemAnh.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnThemAnh.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnXemRP.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnXemRP.ForeColor = ColorTranslator.FromHtml("#006A71");
            btnDatLai.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            btnDatLai.ForeColor = ColorTranslator.FromHtml("#006A71");
            //txtTimKiem.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            //txtMaTaiSan.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            //txtTenTaiSan.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            //cbLoaiTaiSan.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            //dtp.CalendarMonthBackground = ColorTranslator.FromHtml("#F2EFE7");
            //txtNguyenGia.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            //txtTinhTrang.BackColor = ColorTranslator.FromHtml("#F2EFE7");
            //txtGiaThucTe.BackColor = ColorTranslator.FromHtml("#F2EFE7");
        }
        private void UpdateDepreciation()
        {
            string sql = @"
                            insert into cokhauhao 
                                (mataisan, namkhauhao, giatrikhauhao, giatriconlaisaukhauhao)
                            select 
                                t.mataisan,
                                year(t.ngaymua) + 1 + n.number as namkhauhao,
                                t.nguyengia * 1.0 / l.sonamsudung as giatrikhauhao,
                                case 
                                    when t.nguyengia - ((n.number + 1) * (t.nguyengia * 1.0 / l.sonamsudung)) < 0 
                                    then 0
                                    else t.nguyengia - ((n.number + 1) * (t.nguyengia * 1.0 / l.sonamsudung))
                                end as giatriconlaisaukhauhao
                            from taisan t
                            join loaitaisan l 
                                on t.maloai = l.maloai
                            join master..spt_values n 
                                on n.type = 'p' 
                                and n.number < l.sonamsudung
                            where 
                                dateadd(year, n.number + 1, t.ngaymua) <= getdate()
                                and not exists (
                                    select 1 
                                    from cokhauhao c 
                                    where c.mataisan = t.mataisan 
                                    and c.namkhauhao = year(t.ngaymua) + 1 + n.number
                                )
                                and t.manguoidung = @userId
                            ";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            sql = @"
                   update taisan set tinhtrang = N'Hết khấu hao'
                   where  manguoidung = @userId and tinhtrang != N'Hết khấu hao' and
                          mataisan in (
                          select mataisan from cokhauhao 
                          where giatriconlaisaukhauhao = 0
                   )
                   ";
            cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void ShowData()
        {
            string sql = @"select *, isnull(c.giatriconlaisaukhauhao, t.nguyengia) as giatriconlai " +
                "from taisan t join loaitaisan l on t.maloai = l.maloai " +
                "left join cokhauhao c on c.mataisan = t.mataisan and c.namkhauhao = " +
                                                                                   "(select max(c2.namkhauhao) from cokhauhao c2 where c2.mataisan = t.mataisan)" +
                " where t.manguoidung = @userId and t.phamvi='Cá nhân'";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            dgv.DataSource = dt;
            dgv.Columns["mataisan"].Visible = false;
            dgv.Columns["maloai"].Visible = false;
            dgv.Columns["maloai1"].Visible = false;
            dgv.Columns["manguoidung"].Visible = false;
            dgv.Columns["phamvi"].Visible = false;
            dgv.Columns["hinhanh"].Visible = false;
            dgv.Columns["mapp"].Visible = false;
            dgv.Columns["mataisan1"].Visible = false;
            dgv.Columns["namkhauhao"].Visible = false;
            dgv.Columns["giatrikhauhao"].Visible = false;
            dgv.Columns["giatriconlaisaukhauhao"].Visible = false;
            dgv.Columns["tenloai"].HeaderText = "Loại tài sản";
            dgv.Columns["tentaisan"].HeaderText = "Tên tài sản";
            dgv.Columns["ngaymua"].HeaderText = "Ngày mua";
            dgv.Columns["nguyengia"].HeaderText = "Nguyên giá";
            dgv.Columns["tinhtrang"].HeaderText = "Tình trạng";
            dgv.Columns["sonamsudung"].HeaderText = "Số năm sử dụng";
            dgv.Columns["chukybaotri"].HeaderText = "Chu kỳ bảo trì";
            dgv.Columns["giatriconlai"].HeaderText = "Giá trị thực";
            dgv.Columns["giatriconlai"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
            dgv.Columns["giatriconlai"].DefaultCellStyle.Format = "C0";
            dgv.Columns["nguyengia"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
            dgv.Columns["nguyengia"].DefaultCellStyle.Format = "C0";
            CreateId();
            sql = "select * from loaitaisan";
            da = new SqlDataAdapter(sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            cbLoaiTaiSan.DataSource = dt;
            cbLoaiTaiSan.DisplayMember = "tenloai";
            cbLoaiTaiSan.ValueMember = "maloai";
            cbLoaiTaiSan.SelectedIndex = -1;
        }
        private void CreateId()
        {
            string sql = @"select mataisan from taisan where mataisan like 'TS%' order by mataisan desc";
            var cmd = new SqlCommand(sql, conn);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string lastId = reader["mataisan"].ToString();
                int numberPart = int.Parse(lastId.Substring(2));
                txtMaTaiSan.Text = "TS" + (numberPart + 1).ToString("D2");
            }
            else
            {
                txtMaTaiSan.Text = "TS01";
            }
            reader.Close();
            conn.Close();
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if(txtTimKiem.Text.Trim() == "")
            {
                ShowData();
                return;
            }
            else
            {
                string sql = @"select *, isnull(c.giatriconlaisaukhauhao, t.nguyengia) as giatriconlai " +
                "from taisan t join loaitaisan l on t.maloai = l.maloai " +
                "left join cokhauhao c on c.mataisan = t.mataisan and c.namkhauhao = " +
                                                                                   "(select max(c2.namkhauhao) from cokhauhao c2 where c2.mataisan = t.mataisan)" +
                " where t.manguoidung = @userId and (tentaisan like @keyword or tenloai like @keyword)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@keyword", "%" + txtTimKiem.Text.Trim() + "%");
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgv.Rows[e.RowIndex];
                txtMaTaiSan.Text = selectedRow.Cells["mataisan"].Value.ToString();
                txtTenTaiSan.Text = selectedRow.Cells["tentaisan"].Value.ToString();
                cbLoaiTaiSan.Text = selectedRow.Cells["tenloai"].Value.ToString();
                dtp.Value = Convert.ToDateTime(selectedRow.Cells["ngaymua"].Value);
                txtTinhTrang.Text = selectedRow.Cells["tinhtrang"].Value.ToString();
                txtGiaThucTe.Text = selectedRow.Cells["giatriconlai"].Value.ToString();
                txtNguyenGia.Text = selectedRow.Cells["nguyengia"].Value.ToString();
                string imageName = selectedRow.Cells["hinhanh"].Value.ToString();
                if (imageName != "")
                {
                    try
                    {
                        string imagePath = System.IO.Path.Combine(Application.StartupPath, "Images", imageName);
                        if (System.IO.File.Exists(imagePath))
                        {
                            using (Image img = Image.FromFile(imagePath))
                            {
                                pictureBox.Image = new Bitmap(img);
                            }
                            // Khối using tự động gọi img.Dispose(), tức là mở file xong -> copy -> đóng file ngay lập tức.
                        }
                        else
                        {
                            pictureBox.Image = null;
                            MessageBox.Show("Không tìm thấy hình ảnh: " + imageName);
                        }
                    }
                    catch (Exception ex)
                    {
                        pictureBox.Image = null;
                        MessageBox.Show("Lỗi khi tải hình ảnh: " + ex.Message);
                    }
                }
                else
                {
                    pictureBox.Image = null;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaTaiSan.Text.Trim() == "" || txtTenTaiSan.Text.Trim() == "" || cbLoaiTaiSan.Text.Trim() == "" || txtNguyenGia.Text.Trim() == "" || dtp.Value == null || pictureBox.Image == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }
            else
            {
                var tryParseResult = decimal.TryParse(txtNguyenGia.Text.Trim(), out decimal nguyenGia);
                if (!tryParseResult || nguyenGia < 0)
                {
                    MessageBox.Show("Sai định dạng nguyên giá");
                    return;
                }
                var dateDiff = (DateTime.Now - dtp.Value).TotalDays;
                if (dateDiff < 0)
                {
                    MessageBox.Show("Ngày mua không thể lớn hơn ngày hiện tại");
                    return;
                }
                string sql = "insert into taisan (mataisan, tentaisan, maloai, ngaymua, nguyengia, tinhtrang, manguoidung, hinhanh, phamvi) " +
                    "values (@mataisan, @tentaisan, @maloai, @ngaymua, @nguyengia, N'Tốt', @manguoidung, @hinhanh, N'Cá nhân')";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mataisan", txtMaTaiSan.Text.Trim());
                cmd.Parameters.AddWithValue("@tentaisan", txtTenTaiSan.Text.Trim());
                cmd.Parameters.AddWithValue("@maloai", cbLoaiTaiSan.SelectedValue);
                cmd.Parameters.AddWithValue("@ngaymua", dtp.Value.Date);
                cmd.Parameters.AddWithValue("@nguyengia", decimal.Parse(txtNguyenGia.Text.Trim()));
                cmd.Parameters.AddWithValue("@manguoidung", userId);
                string sourceImagePath = pictureBox.ImageLocation; 
                string imageName = txtMaTaiSan.Text.Trim() + System.IO.Path.GetExtension(sourceImagePath);
                cmd.Parameters.AddWithValue("@hinhanh", imageName);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    string targetImagePath = System.IO.Path.Combine(Application.StartupPath, "Images", imageName);
                    System.IO.File.Copy(sourceImagePath, targetImagePath, true);
                    MessageBox.Show("Thêm tài sản thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm tài sản: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                UpdateDepreciation();
                ShowData();
                btnDatLai_Click(sender, e);
            }
        }

        private void btnThemAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.ImageLocation = openFileDialog.FileName;
            }
        }
        private void btnDatLai_Click(object sender, EventArgs e)
        {
            txtTenTaiSan.Text = "";
            cbLoaiTaiSan.SelectedIndex = -1;
            dtp.Value = DateTime.Now;
            txtNguyenGia.Text = "";
            txtTinhTrang.Text = "";
            txtGiaThucTe.Text = "";
            pictureBox.Image = null;
            pictureBox.ImageLocation = null;
            CreateId();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaTaiSan.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn tài sản cần xóa.");
                return;
            }
            string imageName = "";
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["mataisan"].Value != null && 
                    row.Cells["mataisan"].Value.ToString().Trim() == txtMaTaiSan.Text.Trim())
                {
                    if (row.Cells["hinhanh"].Value != null)
                    {
                        imageName = row.Cells["hinhanh"].Value.ToString();
                    }
                    break;
                }
            }
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
            pictureBox.ImageLocation = null;
            string sql = "delete from cokhauhao where mataisan = @mataisan; delete from taisan where mataisan = @mataisan";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mataisan", txtMaTaiSan.Text.Trim());
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
                if (!string.IsNullOrEmpty(imageName))
                {
                    string imagePath = System.IO.Path.Combine(Application.StartupPath, "Images", imageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        try 
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        catch (Exception fileEx)
                        {
                            MessageBox.Show("Đã xóa trên Hệ thống, nhưng không thể xóa file vật lý do bị khóa: " + fileEx.Message);
                        }
                    }
                }            
                MessageBox.Show("Xóa tài sản thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài sản: " + ex.Message);
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                    conn.Close();
            }          
            ShowData();
            btnDatLai_Click(sender, e);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTaiSan.Text == "" || txtTenTaiSan.Text == "" || cbLoaiTaiSan.Text == "" || txtNguyenGia.Text == "" || dtp.Value == null || pictureBox.Image == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }
            string sql = "select 1 from cokhauhao where mataisan = @mataisan";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mataisan", txtMaTaiSan.Text.Trim());
            if(conn.State == ConnectionState.Closed)
                conn.Open();
            var reader = cmd.ExecuteReader();
            var hasDepreciation = reader.Read();
            reader.Close();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["mataisan"].Value != null &&
                    row.Cells["mataisan"].Value.ToString().Trim() == txtMaTaiSan.Text.Trim())
                {
                    if (hasDepreciation)
                    {
                        if (cbLoaiTaiSan.SelectedValue.ToString() != row.Cells["maloai"].Value.ToString())
                        {
                            MessageBox.Show("Không thể thay đổi loại tài sản đã có khấu hao");
                            return;
                        }
                        if (dtp.Value.Date != Convert.ToDateTime(row.Cells["ngaymua"].Value).Date)
                        {
                            MessageBox.Show("Không thể thay đổi ngày mua của tài sản đã có khấu hao");
                            return;
                        }
                        if (decimal.Parse(txtNguyenGia.Text.Trim()) != Convert.ToDecimal(row.Cells["nguyengia"].Value))
                        {
                            MessageBox.Show("Không thể thay đổi nguyên giá của tài sản đã có khấu hao");
                            return;
                        }
                        sql = "update taisan set tentaisan = @tentaisan, hinhanh = @hinhanh where mataisan = @mataisan";
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@tentaisan", txtTenTaiSan.Text.Trim());
                        cmd.Parameters.AddWithValue("@mataisan", txtMaTaiSan.Text.Trim());
                        
                    }
                    else
                    {
                        var tryParseResult = decimal.TryParse(txtNguyenGia.Text.Trim(), out decimal nguyenGia);
                        if (!tryParseResult || nguyenGia < 0)
                        {
                            MessageBox.Show("Sai định dạng nguyên giá");
                            return;
                        }
                        var dateDiff = (DateTime.Now - dtp.Value).TotalDays;
                        if (dateDiff < 0)
                        {
                            MessageBox.Show("Ngày mua không thể lớn hơn ngày hiện tại");
                            return;
                        }
                        sql = "update taisan set tentaisan = @tentaisan, maloai = @maloai, ngaymua = @ngaymua, nguyengia = @nguyengia, hinhanh = @hinhanh where mataisan = @mataisan";
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@tentaisan", txtTenTaiSan.Text.Trim());
                        cmd.Parameters.AddWithValue("@maloai", cbLoaiTaiSan.SelectedValue);
                        cmd.Parameters.AddWithValue("@ngaymua", dtp.Value.Date);
                        cmd.Parameters.AddWithValue("@nguyengia", decimal.Parse(txtNguyenGia.Text.Trim()));
                        cmd.Parameters.AddWithValue("@mataisan", txtMaTaiSan.Text.Trim());
                    }
                    if (string.IsNullOrEmpty(pictureBox.ImageLocation))
                    {
                        cmd.Parameters.AddWithValue("@hinhanh", row.Cells["hinhanh"].Value.ToString());
                    }
                    else
                    {
                        string sourceNewImagePath = pictureBox.ImageLocation;
                        string newImageName = txtMaTaiSan.Text.Trim() + System.IO.Path.GetExtension(sourceNewImagePath);
                        cmd.Parameters.AddWithValue("@hinhanh", newImageName);
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        if (!string.IsNullOrEmpty(pictureBox.ImageLocation))
                        {
                            string oldImageName = row.Cells["hinhanh"].Value.ToString();
                            string sourceNewImagePath = pictureBox.ImageLocation;
                            string newImageName = txtMaTaiSan.Text.Trim() + System.IO.Path.GetExtension(sourceNewImagePath);
                            string targetImagePath = System.IO.Path.Combine(Application.StartupPath, "Images", newImageName);
                            System.IO.File.Copy(sourceNewImagePath, targetImagePath, true);

                            if (oldImageName != newImageName)
                            {
                                string oldImagePath = System.IO.Path.Combine(Application.StartupPath, "Images", oldImageName);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                    catch (Exception fileEx)
                                    {
                                        MessageBox.Show("Đã cập nhật trên Hệ thống, nhưng không thể xóa file cũ do bị khóa: " + fileEx.Message);
                                    }
                                }
                            }
                        }
                        MessageBox.Show("Cập nhật tài sản thành công.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật tài sản: " + ex.Message);
                    }
                    finally
                    {
                        UpdateDepreciation();
                        ShowData();
                        btnDatLai_Click(sender, e);
                        conn.Close();
                    }
                }
            }
        }

        private void btnXemRP_Click(object sender, EventArgs e)
        {
            FormReportTSCN formReport = new FormReportTSCN(this, userId);
            this.Hide();
            formReport.ShowDialog();
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (txtMaTaiSan.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn tài sản cần xem chi tiết.");
                return;
            }
            string sql = "select 1 from cokhauhao where mataisan = @mataisan";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mataisan", txtMaTaiSan.Text.Trim());
            conn.Open();
            var reader = cmd.ExecuteReader();
            var hasDepreciation = reader.Read();
            reader.Close();
            conn.Close();
            if (!hasDepreciation)
            {
                MessageBox.Show("Tài sản này chưa có khấu hao nên không có chi tiết khấu hao để hiển thị.");
                return;
            }
            FormChiTietKhauHao formChiTiet = new FormChiTietKhauHao(txtMaTaiSan.Text.Trim(), conn, this);
            this.Hide();
            formChiTiet.ShowDialog();
        }
    }
}
