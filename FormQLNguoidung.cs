using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LTUD
{
    public partial class FormQLNguoidung : System.Windows.Forms.Form
    {
        SqlConnection conn;

        void KetNoi()
        {
            conn = new SqlConnection(
              "Server =FAKEDAT\\SQLEXPRESS; Database = QLTAISAN ; Integrated Security = True; TrustServerCertificate=True");
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
            cboVaitro.SelectedIndex = -1;
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
            string sql = "SELECT TOP 1 MANGUOIDUNG FROM NGUOIDUNG ORDER BY MANGUOIDUNG DESC";
            SqlCommand cmd = new SqlCommand(sql, conn);

            object result = cmd.ExecuteScalar();

            if (result == null || result.ToString() == "")
                return "ND01";

            string lastID = result.ToString(); // 

            int number = int.Parse(lastID.Substring(2)); // lấy số phía sau
            number++;

            return "ND" + number.ToString("D2"); // 
        }
        private void btnThem_Click(object sender, EventArgs e)
        {

            if (txtHoten.Text == "")
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }

            string ma = TaoMaTuTang();
            txtMaND.Text = ma;

            string sql = @"
                        INSERT INTO NGUOIDUNG
                        VALUES (@ma, @vt, @gd, @ten, @ns, @mk, @tt)";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ma", ma);
            cmd.Parameters.AddWithValue("@vt", cboVaitro.SelectedValue);
            cmd.Parameters.AddWithValue("@gd", cboGiadinh.SelectedValue);
            cmd.Parameters.AddWithValue("@ten", txtHoten.Text);
            cmd.Parameters.AddWithValue("@ns", dtpNgaysinh.Value);
            cmd.Parameters.AddWithValue("@mk", "123");
            cmd.Parameters.AddWithValue("@tt", cboTrangthai.Text);

            cmd.ExecuteNonQuery();

            LoadGrid();
            MessageBox.Show("Thêm thành công!");

            btnDatlai_Click(null, null); // reset form
        }

        private void btnDatlai_Click(object sender, EventArgs e)
        {
            txtMaND.Text = TaoMaTuTang(); // hiện mã mới luôn
            txtHoten.Clear();

            cboGiadinh.SelectedIndex = -1;
            cboVaitro.SelectedIndex = -1;
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
            cmd.Parameters.AddWithValue("@vt", cboVaitro.SelectedValue);
            cmd.Parameters.AddWithValue("@gd", cboGiadinh.SelectedValue);
            cmd.Parameters.AddWithValue("@ns", dtpNgaysinh.Value);
            cmd.Parameters.AddWithValue("@tt", cboTrangthai.Text);
            
            cmd.ExecuteNonQuery();

            LoadGrid();
            MessageBox.Show("Sửa thành công!");
        }

        private void btnXemtaisan_Click(object sender, EventArgs e)
        {
            FormXemtaisan f = new FormXemtaisan(txtMaND.Text);
            f.Show();
        }
    }
}
