using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormQLGiaDinh : Form
    {
        private string currentMaGD = "";
        private string adminMaGD = "";
        private string vaiTroAdmin = "";

        public FormQLGiaDinh(string maGD = "GD01", string vaiTro = "VT03")
        {
            InitializeComponent();
            adminMaGD = maGD;
            vaiTroAdmin = vaiTro;

            // Nếu là người dùng thường, không cho phép thêm sửa xóa thành viên
            if (vaiTroAdmin != "VT03")
            {
                btnThemTV.Visible = false;
                btnSuaTV.Visible = false;
                btnXoaTV.Visible = false;
            }
            else
            {
                btnThemTV.Click += BtnThemTV_Click;
                btnSuaTV.Click += BtnSuaTV_Click;
                btnXoaTV.Click += BtnXoaTV_Click;
            }
        }

        private void FormQLGiaDinh_Load(object sender, EventArgs e)
        {
            // Fix Schema Taisan cho phép NULL người dùng khi xóa
            try { DatabaseConnection.ExecuteQuery("ALTER TABLE TAISAN ALTER COLUMN MANGUOIDUNG CHAR(10) NULL"); } catch { }
            LoadGiaDinh();
        }

        private void LoadGiaDinh()
        {
            try
            {
                DataTable dt = DatabaseConnection.GetData($"SELECT MAGIADINH, TENGIADINH FROM GIADINH WHERE MAGIADINH = '{adminMaGD}'");
                if (dt.Rows.Count > 0)
                {
                    lblMaGiaDinh.Text = "Mã Gia Đình: " + dt.Rows[0]["MAGIADINH"].ToString();
                    lblTenGiaDinh.Text = "Tên Gia Đình: " + dt.Rows[0]["TENGIADINH"].ToString();
                    currentMaGD = dt.Rows[0]["MAGIADINH"].ToString();
                    LoadThanhVien(currentMaGD);
                }
                else
                {
                    lblMaGiaDinh.Text = "Mã Gia Đình: Không tìm thấy";
                    lblTenGiaDinh.Text = "Tên Gia Đình: Không tìm thấy";
                    currentMaGD = "";
                    dgvNguoiDung.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Gia đình: " + ex.Message);
            }
        }

        private void LoadThanhVien(string maGD)
        {
            try
            {
                string query = $"SELECT MANGUOIDUNG AS [Mã ND], MAVAITRO AS [Mã Vai Trò], HOTEN AS [Họ Tên], NGAYSINH AS [Ngày Sinh], TRANGTHAI AS [Trạng Thái] FROM NGUOIDUNG WHERE MAGIADINH = '{maGD}' ORDER BY MANGUOIDUNG ASC";
                dgvNguoiDung.DataSource = DatabaseConnection.GetData(query);

                if (dgvNguoiDung.Columns.Contains("Mã ND")) dgvNguoiDung.Columns["Mã ND"].Visible = false;
                if (dgvNguoiDung.Columns.Contains("Mã Vai Trò")) dgvNguoiDung.Columns["Mã Vai Trò"].Visible = false;
                if (dgvNguoiDung.Columns.Contains("Trạng Thái")) dgvNguoiDung.Columns["Trạng Thái"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Thành viên: " + ex.Message);
            }
        }

        // ======================= CRUD THÀNH VIÊN =======================
        private void BtnThemTV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentMaGD)) { MessageBox.Show("Vui lòng chọn 1 gia đình trước!"); return; }

            Form f = new Form { Width = 500, Height = 200, Text = "Thêm Thành Viên Vào Gia Đình", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
            f.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            Label lblUser = new Label { Left = 20, Top = 33, Text = "Người dùng chưa có GĐ:", AutoSize = true };
            ComboBox cbUser = new ComboBox { Left = 180, Top = 30, Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };

            DataTable dtUsers = DatabaseConnection.GetData("SELECT MANGUOIDUNG, HOTEN + ' (' + MANGUOIDUNG + ')' AS TENDAYDU FROM NGUOIDUNG WHERE MAGIADINH IS NULL OR LTRIM(RTRIM(MAGIADINH)) = ''");

            if (dtUsers.Rows.Count == 0)
            {
                MessageBox.Show("Không có người dùng nào chưa có gia đình để thêm!");
                return;
            }

            cbUser.DataSource = dtUsers;
            cbUser.DisplayMember = "TENDAYDU";
            cbUser.ValueMember = "MANGUOIDUNG";

            Button btnSave = new Button { Left = 200, Top = 100, Width = 100, Text = "Thêm vào", BackColor = Color.FromArgb(242, 239, 231), ForeColor = Color.FromArgb(0, 106, 113), FlatStyle = FlatStyle.Flat };

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    if (cbUser.SelectedValue != null)
                    {
                        string maUser = cbUser.SelectedValue.ToString();
                        DatabaseConnection.ExecuteQuery($"UPDATE NGUOIDUNG SET MAGIADINH = '{currentMaGD}' WHERE MANGUOIDUNG = '{maUser}'");
                        MessageBox.Show("Thêm thành viên vào gia đình thành công!");
                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            };
            
            f.Controls.AddRange(new Control[] { lblUser, cbUser, btnSave });
            
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadThanhVien(currentMaGD);
            }
        }

        private void BtnSuaTV_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.CurrentRow == null) return;
            string ma = dgvNguoiDung.CurrentRow.Cells[0].Value.ToString();
            string vaitro = dgvNguoiDung.CurrentRow.Cells[1].Value.ToString();
            string hoten = dgvNguoiDung.CurrentRow.Cells[2].Value.ToString();
            DateTime ngay = Convert.ToDateTime(dgvNguoiDung.CurrentRow.Cells[3].Value);
            string tt = dgvNguoiDung.CurrentRow.Cells[4].Value.ToString();
            ShowThanhVienDialog(false, ma, vaitro, hoten, ngay, tt);
        }

        private void BtnXoaTV_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.CurrentRow == null) return;
            string ma = dgvNguoiDung.CurrentRow.Cells[0].Value.ToString();

            DataTable checkTS = DatabaseConnection.GetData($"SELECT * FROM TAISAN WHERE MANGUOIDUNG = '{ma}' AND PHAMVI = N'Gia đình'");
            if (checkTS.Rows.Count > 0)
            {
                MessageBox.Show("Không thể xóa người dùng này vì họ đang quản lý tài sản thuộc phạm vi Gia đình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa thành viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseConnection.ExecuteQuery($"UPDATE TAISAN SET MANGUOIDUNG = NULL WHERE MANGUOIDUNG = '{ma}'");
                DatabaseConnection.ExecuteQuery($"DELETE FROM NGUOIDUNG WHERE MANGUOIDUNG = '{ma}'");
                LoadThanhVien(currentMaGD);
            }
        }

        private void ShowThanhVienDialog(bool isAdd, string maND, string maVT, string hoTen, DateTime ngaySinh, string trangThai)
        {
            Form f = new Form { Width = 500, Height = 400, Text = isAdd ? "Thêm TV" : "Sửa TV", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
            f.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label lblMa = new Label { Left = 20, Top = 23, Text = "Mã ND:", AutoSize = true }; TextBox txtMa = new TextBox { Left = 120, Top = 20, Width = 330, Text = maND, Enabled = isAdd };
            Label lblTen = new Label { Left = 20, Top = 63, Text = "Họ Tên:", AutoSize = true }; TextBox txtTen = new TextBox { Left = 120, Top = 60, Width = 330, Text = hoTen };
            Label lblNgay = new Label { Left = 20, Top = 103, Text = "Ngày Sinh:", AutoSize = true }; DateTimePicker dtpNgay = new DateTimePicker { Left = 120, Top = 100, Width = 330, Format = DateTimePickerFormat.Short, Value = ngaySinh };
            Label lblVT = new Label { Left = 20, Top = 143, Text = "Vai Trò:", AutoSize = true }; ComboBox cbVT = new ComboBox { Left = 120, Top = 140, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
            Label lblTT = new Label { Left = 20, Top = 183, Text = "Trạng Thái:", AutoSize = true }; ComboBox cbTT = new ComboBox { Left = 120, Top = 180, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTT.Items.AddRange(new string[] { "Hoạt động", "Vô hiệu hóa" });
            cbTT.SelectedItem = string.IsNullOrEmpty(trangThai) ? "Hoạt động" : trangThai;
            Label lblTS = new Label { Left = 20, Top = 223, Text = "Quản lý TS:", AutoSize = true }; ComboBox cbTS = new ComboBox { Left = 120, Top = 220, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };

            Button btnSave = new Button { Left = 200, Top = 280, Width = 100, Text = "Lưu", BackColor = Color.FromArgb(242, 239, 231), ForeColor = Color.FromArgb(0, 106, 113), FlatStyle = FlatStyle.Flat };

            // Load Combobox VaiTro
            cbVT.DataSource = DatabaseConnection.GetData("SELECT MAVAITRO, TENVAITRO FROM VAITRO");
            cbVT.DisplayMember = "TENVAITRO"; cbVT.ValueMember = "MAVAITRO"; cbVT.SelectedValue = maVT;

            // Load Combobox Tài Sản
            DataTable dtTS = DatabaseConnection.GetData($"SELECT MATAISAN, TENTAISAN + ' (' + MATAISAN + ')' AS TENDAYDU FROM TAISAN WHERE (MANGUOIDUNG IS NULL OR LTRIM(RTRIM(MANGUOIDUNG)) = '' OR MANGUOIDUNG = '{maND}') AND PHAMVI = N'Gia đình'");
            DataRow rowNone = dtTS.NewRow();
            rowNone["MATAISAN"] = "NONE";
            rowNone["TENDAYDU"] = "<Không quản lý tài sản nào>";
            dtTS.Rows.InsertAt(rowNone, 0);
            cbTS.DataSource = dtTS;
            cbTS.DisplayMember = "TENDAYDU";
            cbTS.ValueMember = "MATAISAN";

            if (!isAdd)
            {
                DataTable dtCurrentAsset = DatabaseConnection.GetData($"SELECT MATAISAN FROM TAISAN WHERE MANGUOIDUNG = '{maND}'");
                if (dtCurrentAsset.Rows.Count > 0)
                    cbTS.SelectedValue = dtCurrentAsset.Rows[0]["MATAISAN"];
                else
                    cbTS.SelectedValue = "NONE";
            }

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    string vt = cbVT.SelectedValue.ToString();
                    string tt = cbTT.SelectedItem.ToString();
                    string dateStr = dtpNgay.Value.ToString("yyyy-MM-dd");
                    string ts = cbTS.SelectedValue.ToString();

                    if (isAdd)
                        DatabaseConnection.ExecuteQuery($"INSERT INTO NGUOIDUNG VALUES ('{txtMa.Text}', '{vt}', '{currentMaGD}', N'{txtTen.Text}', '{dateStr}', '123', N'{tt}')");
                    else
                        DatabaseConnection.ExecuteQuery($"UPDATE NGUOIDUNG SET MAVAITRO='{vt}', HOTEN=N'{txtTen.Text}', NGAYSINH='{dateStr}', TRANGTHAI=N'{tt}' WHERE MANGUOIDUNG='{txtMa.Text}'");

                    // Cập nhật người quản lý tài sản
                    DatabaseConnection.ExecuteQuery($"UPDATE TAISAN SET MANGUOIDUNG = NULL WHERE MANGUOIDUNG = '{txtMa.Text}'");
                    if (ts != "NONE")
                    {
                        DatabaseConnection.ExecuteQuery($"UPDATE TAISAN SET MANGUOIDUNG = '{txtMa.Text}' WHERE MATAISAN = '{ts}'");
                    }

                    f.DialogResult = DialogResult.OK;
                    f.Close();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            };
            f.Controls.AddRange(new Control[] { lblMa, txtMa, lblTen, txtTen, lblNgay, dtpNgay, lblVT, cbVT, lblTT, cbTT, lblTS, cbTS, btnSave });
            if (f.ShowDialog() == DialogResult.OK) LoadThanhVien(currentMaGD);
        }
    }
}
