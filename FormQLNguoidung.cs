using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormQLNguoidung : System.Windows.Forms.Form
    {
        SqlConnection conn;

        void KetNoi()
        {
            conn = new SqlConnection(
              @"Server =.\SQLEXPRESS; Database = QLTaiSan_LTUD; Integrated Security = True; TrustServerCertificate=True");
            //conn = new SqlConnection(
            //  @"Data Source=.\SQLEXPRESS;Initial Catalog=QLTaiSan_LTUD;Integrated Security=True;TrustServerCertificate=True");
            conn.Open();
        }
        public FormQLNguoidung()
        {
            InitializeComponent();
        }

        private void FormQLNguoidung_Load(object sender, EventArgs e)
        {
            KetNoi();
            LoadGrid();
            LoadGiaDinh();
            LoadVaiTro();
            LoadTrangThai();
            txtMaND.Text = TaoMaTuTang();
            dtpNgaysinh.MaxDate = DateTime.Now;
        }

        void LoadGrid()
        {
            string sql = @"
        SELECT ND.MANGUOIDUNG, ND.HOTEN, GD.TENGIADINH, 
               VT.TENVAITRO, ND.NGAYSINH, ND.TRANGTHAI
        FROM NGUOIDUNG ND
        LEFT JOIN GIADINH GD ON ND.MAGIADINH = GD.MAGIADINH
        LEFT JOIN VAITRO VT ON ND.MAVAITRO = VT.MAVAITRO";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["MANGUOIDUNG"].HeaderText = "Mã người dùng";
            dataGridView1.Columns["HOTEN"].HeaderText = "Họ tên";
            dataGridView1.Columns["TENGIADINH"].HeaderText = "Gia đình";
            dataGridView1.Columns["TENVAITRO"].HeaderText = "Vai trò";
            dataGridView1.Columns["NGAYSINH"].HeaderText = "Ngày sinh";
            dataGridView1.Columns["TRANGTHAI"].HeaderText = "Trạng thái";


        }
        void LoadVaiTro()
        {
            string sql = "SELECT * FROM VAITRO";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cboVaitro.DataSource = dt;
            cboVaitro.DisplayMember = "TENVAITRO";
            cboVaitro.ValueMember = "MAVAITRO";

            cboVaitro.SelectedValue = "VT02";
        }
        void LoadGiaDinh()
        {
            string sql = "SELECT * FROM GIADINH";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cboGiadinh.DataSource = dt;
            cboGiadinh.DisplayMember = "TENGIADINH";
            cboGiadinh.ValueMember = "MAGIADINH";
            cboGiadinh.SelectedIndex = -1;
        }
        void LoadTrangThai()
        {
            cboTrangthai.Items.Add("Hoạt động");
            cboTrangthai.Items.Add("Bị khóa");
            cboTrangthai.SelectedIndex = -1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < 0) return;

            txtMaND.Text = dataGridView1.Rows[i].Cells["MANGUOIDUNG"].Value.ToString();
            txtHoten.Text = dataGridView1.Rows[i].Cells["HOTEN"].Value.ToString();
            cboGiadinh.Text = dataGridView1.Rows[i].Cells["TENGIADINH"].Value.ToString();
            cboVaitro.Text = dataGridView1.Rows[i].Cells["TENVAITRO"].Value.ToString();
            dtpNgaysinh.Value = Convert.ToDateTime(dataGridView1.Rows[i].Cells["NGAYSINH"].Value);
            cboTrangthai.Text = dataGridView1.Rows[i].Cells["TRANGTHAI"].Value.ToString();
        }
        string TaoMaTuTang()
        {
            string sql = "SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MANGUOIDUNG LIKE 'ND%'";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            int maxNumber = 0;
            foreach (DataRow row in dt.Rows)
            {
                string currentID = row["MANGUOIDUNG"].ToString().Trim();
                string numPart = new string(currentID.SkipWhile(c => !char.IsDigit(c)).ToArray());

                if (int.TryParse(numPart, out int currentNum))
                {
                    if (currentNum > maxNumber)
                    {
                        maxNumber = currentNum;
                    }
                }
            }

            int nextNumber = maxNumber + 1;
            return "ND" + nextNumber.ToString("D2");
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(txtHoten.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }

            // 2. Kiểm tra vai trò
            if (cboVaitro.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!");
                return;
            }

            // Lấy mã vai trò và chuẩn hóa (viết hoa, xóa khoảng trắng thừa)
            string vaitro = cboVaitro.SelectedValue.ToString().Trim().ToUpper();

            // 3. KIỂM TRA CHẶT CHẼ: Nếu là Chủ gia đình
            // Bạn hãy kiểm tra lại trong bảng VAITRO xem mã chính xác là gì (Ví dụ: VT03)
            if (vaitro == "VT03")
            {
                if (cboGiadinh.SelectedValue == null ||
                    string.IsNullOrEmpty(cboGiadinh.SelectedValue.ToString()) ||
                    cboGiadinh.SelectedIndex == -1)
                {
                    MessageBox.Show("LỖI: Vai trò Chủ gia đình bắt buộc phải chọn một gia đình cụ thể!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboGiadinh.Focus();
                    return; // Dừng hàm ngay lập tức, không cho chạy xuống lệnh INSERT
                }
            }

            // 4. Nếu vượt qua các kiểm tra trên mới thực hiện thêm vào DB
            try
            {
                string ma = TaoMaTuTang();
                string sql = @"INSERT INTO NGUOIDUNG (MANGUOIDUNG, MAVAITRO, MAGIADINH, HOTEN, NGAYSINH, MATKHAU, TRANGTHAI) 
                       VALUES (@ma, @vt, @gd, @ten, @ns, @mk, @tt)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@vt", vaitro);

                // Gán giá trị Gia đình hoặc NULL nếu không chọn
                cmd.Parameters.AddWithValue("@gd", cboGiadinh.SelectedValue ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ten", txtHoten.Text);
                cmd.Parameters.AddWithValue("@ns", dtpNgaysinh.Value);
                cmd.Parameters.AddWithValue("@mk", "123");
                cmd.Parameters.AddWithValue("@tt", cboTrangthai.Text ?? "Hoạt động");

                cmd.ExecuteNonQuery();
                LoadGrid();
                MessageBox.Show("Thêm thành công!");
                btnDatlai_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void btnDatlai_Click(object sender, EventArgs e)
        {
            txtMaND.Text = TaoMaTuTang();
            txtHoten.Clear();

            cboGiadinh.SelectedIndex = -1;

            // 🔥 mặc định lại vai trò
            cboVaitro.SelectedValue = "VT02";

            cboTrangthai.SelectedIndex = -1;
            dtpNgaysinh.Value = DateTime.Now.Date;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            // Xác nhận
            DialogResult kq = MessageBox.Show(
                "Bạn có chắc muốn xóa người dùng này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (kq == DialogResult.No)
                return;

            // Kiểm tra tài sản
            string checkSql = "SELECT COUNT(*) FROM TAISAN WHERE MANGUOIDUNG = @ma";
            SqlCommand checkCmd = new SqlCommand(checkSql, conn);
            checkCmd.Parameters.AddWithValue("@ma", txtMaND.Text);

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                MessageBox.Show("Người dùng đang sở hữu tài sản, không thể xóa");
                return;
            }

            // Xóa
            string deleteSql = "DELETE FROM NGUOIDUNG WHERE MANGUOIDUNG = @ma";
            SqlCommand deleteCmd = new SqlCommand(deleteSql, conn);
            deleteCmd.Parameters.AddWithValue("@ma", txtMaND.Text);

            int rows = deleteCmd.ExecuteNonQuery(); // 🔥 quan trọng

            if (rows > 0)
            {
                MessageBox.Show("Xóa thành công!");
                LoadGrid();
                btnDatlai_Click(null, null);
            }
            else
            {
                MessageBox.Show("Người dùng không tồn tại");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra mã người dùng (phải có mã mới sửa được)
            if (string.IsNullOrEmpty(txtMaND.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa từ danh sách!");
                return;
            }

            // 2. Kiểm tra họ tên trống
            if (string.IsNullOrWhiteSpace(txtHoten.Text))
            {
                MessageBox.Show("Họ tên không được để trống!");
                return;
            }

            // 3. Lấy giá trị Vai trò và chuẩn hóa
            // Ép kiểu an toàn và xóa khoảng trắng thừa
            string maVaitro = cboVaitro.SelectedValue != null ? cboVaitro.SelectedValue.ToString().Trim() : "";
            string tenVaitro = cboVaitro.Text.Trim();

            // 4. KIỂM TRA LOGIC CHỦ GIA ĐÌNH (Sửa tại đây)
            // Kiểm tra cả mã (VT03) HOẶC tên hiển thị (Chủ gia đình) để tránh sai sót
            if (maVaitro == "VT03" || tenVaitro == "Chủ gia đình")
            {
                if (cboGiadinh.SelectedValue == null || cboGiadinh.SelectedIndex == -1)
                {
                    MessageBox.Show("LỖI: Khi thay đổi vai trò thành 'Chủ gia đình', bạn bắt buộc phải chọn Gia đình cho họ!",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboGiadinh.Focus();
                    return; // NGĂN CHẶN việc chạy xuống lệnh UPDATE phía dưới
                }
            }

            // 5. Thực hiện cập nhật nếu vượt qua kiểm tra
            try
            {
                string sql = @"
            UPDATE NGUOIDUNG
            SET HOTEN = @ten,
                MAVAITRO = @vt,
                MAGIADINH = @gd,
                NGAYSINH = @ns,
                TRANGTHAI = @tt
            WHERE MANGUOIDUNG = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ma", txtMaND.Text);
                cmd.Parameters.AddWithValue("@ten", txtHoten.Text);
                cmd.Parameters.AddWithValue("@vt", maVaitro);

                // Nếu không chọn gia đình (cho các vai trò khác) thì lưu NULL vào DB
                cmd.Parameters.AddWithValue("@gd", cboGiadinh.SelectedValue ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ns", dtpNgaysinh.Value);
                cmd.Parameters.AddWithValue("@tt", cboTrangthai.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    LoadGrid();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng để cập nhật!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message);
            }
        }

        private void btnXemtaisan_Click(object sender, EventArgs e)
        {
            FormXemtaisan f = new FormXemtaisan(txtMaND.Text);
            f.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
