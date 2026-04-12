using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace LTUD
{
    public partial class FormQLNguoidung : System.Windows.Forms.Form
    {
        SqlConnection conn;
        string currentUser;
        void KetNoi()
        {
            conn = new SqlConnection(
              @"Server =.\SQLEXPRESS; Database = QLTaiSan_LTUD; Integrated Security = True; TrustServerCertificate=True");
            conn.Open();
        }
        public FormQLNguoidung(string maND)
        {
            InitializeComponent();
            currentUser = maND;
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

            txtHoten.ReadOnly = true;       
            dtpNgaysinh.Enabled = false;    
            cboGiadinh.Enabled = false;     
            txtMaND.ReadOnly = true;        
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
            if (string.IsNullOrWhiteSpace(txtHoten.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }

            if (cboVaitro.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!");
                return;
            }

            string vaitro = cboVaitro.SelectedValue.ToString().Trim().ToUpper();
            if (vaitro == "VT03")
            {
                if (cboGiadinh.SelectedValue == null ||
                    string.IsNullOrEmpty(cboGiadinh.SelectedValue.ToString()) ||
                    cboGiadinh.SelectedIndex == -1)
                {
                    MessageBox.Show("LỖI: Vai trò Chủ gia đình bắt buộc phải chọn một gia đình cụ thể!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboGiadinh.Focus();
                    return;
                }
            }
            try
            {
                string ma = TaoMaTuTang();
                string sql = @"INSERT INTO NGUOIDUNG (MANGUOIDUNG, MAVAITRO, MAGIADINH, HOTEN, NGAYSINH, MATKHAU, TRANGTHAI) 
                       VALUES (@ma, @vt, @gd, @ten, @ns, @mk, @tt)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@vt", vaitro);
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
            cboVaitro.SelectedValue = "VT02";
            cboTrangthai.SelectedIndex = -1;
            dtpNgaysinh.Value = DateTime.Now.Date;
            txtHoten.ReadOnly = false;
            dtpNgaysinh.Enabled = true;
            cboGiadinh.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaND.Text.Trim().ToUpper() == currentUser.Trim().ToUpper())
            {
                MessageBox.Show("Bạn không thể tự xóa chính mình!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult kq = MessageBox.Show(
                "Bạn có chắc muốn xóa người dùng này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (kq == DialogResult.No)
                return;
            string checkSql = "SELECT COUNT(*) FROM TAISAN WHERE MANGUOIDUNG = @ma";
            SqlCommand checkCmd = new SqlCommand(checkSql, conn);
            checkCmd.Parameters.AddWithValue("@ma", txtMaND.Text);

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                MessageBox.Show("Người dùng đang sở hữu tài sản, không thể xóa");
                return;
            }
            string deleteSql = "DELETE FROM NGUOIDUNG WHERE MANGUOIDUNG = @ma";
            SqlCommand deleteCmd = new SqlCommand(deleteSql, conn);
            deleteCmd.Parameters.AddWithValue("@ma", txtMaND.Text);

            int rows = deleteCmd.ExecuteNonQuery(); 

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
            if (string.IsNullOrEmpty(txtMaND.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa từ danh sách!");
                return;
            }
            string vaitroMoi = cboVaitro.SelectedValue?.ToString().Trim().ToUpper();
            string trangthaiMoi = cboTrangthai.Text.Trim();
            string vaitroCu = "";
            using (SqlCommand cmdCheck = new SqlCommand("SELECT MAVAITRO FROM NGUOIDUNG WHERE MANGUOIDUNG = @ma", conn))
            {
                cmdCheck.Parameters.AddWithValue("@ma", txtMaND.Text);
                object result = cmdCheck.ExecuteScalar();
                vaitroCu = result?.ToString().Trim().ToUpper() ?? "";
            }
            if (vaitroCu != "VT03" && vaitroMoi == "VT03")
            {
                MessageBox.Show("LỖI: Hệ thống không cho phép chuyển người dùng thường thành Chủ gia đình tại đây!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cboVaitro.SelectedValue = vaitroCu;
                return;
            }
            if (vaitroCu == "VT03" && vaitroMoi == "VT01")
            {
                MessageBox.Show("LỖI: Không được phép thay đổi vai trò từ Chủ gia đình sang Quản trị viên!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cboVaitro.SelectedValue = "VT03";
                return;
            }
            if (txtMaND.Text.Trim().ToUpper() == currentUser.Trim().ToUpper())
            {
                MessageBox.Show("Bạn không được phép tự sửa thông tin của chính mình!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {         
                string sql = @"UPDATE NGUOIDUNG 
                       SET MAVAITRO = @vt, 
                           TRANGTHAI = @tt 
                       WHERE MANGUOIDUNG = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", txtMaND.Text);
                cmd.Parameters.AddWithValue("@vt", vaitroMoi);
                cmd.Parameters.AddWithValue("@tt", trangthaiMoi);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    LoadGrid();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
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
