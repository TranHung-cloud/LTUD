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
                string query = $@"
                    SELECT 
                        N.MANGUOIDUNG AS [Mã ND], 
                        N.MAVAITRO AS [Mã Vai Trò], 
                        V.TENVAITRO AS [Vai Trò], 
                        N.HOTEN AS [Họ Tên], 
                        N.NGAYSINH AS [Ngày Sinh], 
                        N.TRANGTHAI AS [Trạng Thái] 
                    FROM NGUOIDUNG N
                    LEFT JOIN VAITRO V ON N.MAVAITRO = V.MAVAITRO
                    WHERE N.MAGIADINH = '{maGD}' 
                    ORDER BY N.MANGUOIDUNG ASC";
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

            Form f = new Form { Width = 500, Height = 320, Text = "Thêm Thành Viên Vào Gia Đình", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
            f.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            Label lblInput = new Label { Left = 20, Top = 23, Text = "Nhập Mã ND:", AutoSize = true };
            TextBox txtSearch = new TextBox { Left = 130, Top = 20, Width = 200 };

            Label lblResMa = new Label { Left = 20, Top = 70, Text = "Mã ND: ", AutoSize = true, Font = new Font("Segoe UI", 9.75F, FontStyle.Bold) };
            Label lblResTen = new Label { Left = 20, Top = 100, Text = "Họ Tên: ", AutoSize = true, Font = new Font("Segoe UI", 9.75F, FontStyle.Bold) };
            Label lblResNgay = new Label { Left = 20, Top = 130, Text = "Ngày Sinh: ", AutoSize = true, Font = new Font("Segoe UI", 9.75F, FontStyle.Bold) };
            Label lblResStatus = new Label { Left = 20, Top = 160, Text = "", AutoSize = true, Font = new Font("Segoe UI", 9.75F, FontStyle.Italic), ForeColor = Color.Red };

            Button btnSave = new Button { Left = 200, Top = 210, Width = 100, Text = "Thêm vào", BackColor = Color.FromArgb(242, 239, 231), ForeColor = Color.FromArgb(0, 106, 113), FlatStyle = FlatStyle.Flat, Enabled = false };

            string targetMaND = "";

            txtSearch.TextChanged += (s, ev) =>
            {
                string searchMa = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchMa))
                {
                    lblResMa.Text = "Mã ND: ";
                    lblResTen.Text = "Họ Tên: ";
                    lblResNgay.Text = "Ngày Sinh: ";
                    lblResStatus.Text = "";
                    btnSave.Enabled = false;
                    targetMaND = "";
                    return;
                }

                try
                {
                    DataTable dt = DatabaseConnection.GetData($"SELECT MANGUOIDUNG, HOTEN, NGAYSINH, MAGIADINH, MAVAITRO FROM NGUOIDUNG WHERE MANGUOIDUNG = '{searchMa}'");
                    if (dt.Rows.Count > 0)
                    {
                        string maND = dt.Rows[0]["MANGUOIDUNG"].ToString();
                        string hoTen = dt.Rows[0]["HOTEN"].ToString();
                        string ngaySinh = "";
                        if (dt.Rows[0]["NGAYSINH"] != DBNull.Value)
                        {
                            ngaySinh = Convert.ToDateTime(dt.Rows[0]["NGAYSINH"]).ToString("dd/MM/yyyy");
                        }
                        string maGD = dt.Rows[0]["MAGIADINH"].ToString().Trim();
                        string vaiTro = dt.Rows[0]["MAVAITRO"].ToString().Trim();

                        lblResMa.Text = "Mã ND: " + maND;
                        lblResTen.Text = "Họ Tên: " + hoTen;
                        lblResNgay.Text = "Ngày Sinh: " + ngaySinh;

                        if (vaiTro == "VT01")
                        {
                            lblResStatus.Text = "Không thể thêm Quản trị viên vào gia đình!";
                            lblResStatus.ForeColor = Color.Red;
                            btnSave.Enabled = false;
                        }
                        else if (!string.IsNullOrEmpty(maGD))
                        {
                            lblResStatus.Text = "Người dùng này đã thuộc một gia đình!";
                            lblResStatus.ForeColor = Color.Red;
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            lblResStatus.Text = "Hợp lệ. Có thể thêm vào gia đình.";
                            lblResStatus.ForeColor = Color.Green;
                            targetMaND = maND;
                            btnSave.Enabled = true;
                        }
                    }
                    else
                    {
                        lblResMa.Text = "Mã ND: ";
                        lblResTen.Text = "Họ Tên: ";
                        lblResNgay.Text = "Ngày Sinh: ";
                        lblResStatus.Text = "Không tìm thấy người dùng!";
                        lblResStatus.ForeColor = Color.Red;
                        btnSave.Enabled = false;
                        targetMaND = "";
                    }
                }
                catch (Exception ex)
                {
                    lblResStatus.Text = "Lỗi: " + ex.Message;
                }
            };

            btnSave.Click += (s, ev) =>
            {
                if (!string.IsNullOrEmpty(targetMaND))
                {
                    try
                    {
                        DatabaseConnection.ExecuteQuery($"UPDATE NGUOIDUNG SET MAGIADINH = '{currentMaGD}', MAVAITRO = 'VT02' WHERE MANGUOIDUNG = '{targetMaND}'");
                        MessageBox.Show("Thêm thành viên vào gia đình thành công!");
                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            };

            f.Controls.AddRange(new Control[] { lblInput, txtSearch, lblResMa, lblResTen, lblResNgay, lblResStatus, btnSave });

            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadThanhVien(currentMaGD);
            }
        }

        private void BtnSuaTV_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.CurrentRow == null) return;
            string ma = dgvNguoiDung.CurrentRow.Cells[0].Value.ToString();

            // Sửa lại Index các cột vì số cột hiển thị đã thay đổi (Cột MAVAITRO vẫn ở vị trí 1, biến cột 2 là Vai Trò, 3 là Họ Tên,...)
            string vaitro = dgvNguoiDung.CurrentRow.Cells["Mã Vai Trò"].Value.ToString();
            string hoten = dgvNguoiDung.CurrentRow.Cells["Họ Tên"].Value.ToString();
            DateTime ngay = Convert.ToDateTime(dgvNguoiDung.CurrentRow.Cells["Ngày Sinh"].Value);
            string tt = dgvNguoiDung.CurrentRow.Cells["Trạng Thái"].Value.ToString();
            ShowThanhVienDialog(false, ma, vaitro, hoten, ngay, tt);
        }

        private void BtnXoaTV_Click(object sender, EventArgs e)
        {
            if (dgvNguoiDung.CurrentRow == null) return;
            string ma = dgvNguoiDung.CurrentRow.Cells["Mã ND"].Value.ToString().Trim();

            // Nhận diện vai trò của người chuẩn bị xóa qua cột Mã Vai Trò
            string vaiTroCuaNguoiCanXoa = dgvNguoiDung.CurrentRow.Cells["Mã Vai Trò"].Value.ToString().Trim();
            if (vaiTroCuaNguoiCanXoa == "VT03")
            {
                MessageBox.Show("Không thể xóa Chủ Gia Đình ra khỏi gia đình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable checkTS = DatabaseConnection.GetData($"SELECT * FROM TAISAN WHERE MANGUOIDUNG = '{ma}' AND PHAMVI = N'Gia đình'");
            if (checkTS.Rows.Count > 0)
            {
                MessageBox.Show("Không thể xóa người dùng này vì họ đang quản lý tài sản thuộc phạm vi Gia đình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa thành viên này khỏi gia đình?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Cập nhật MAGIADINH về NULL thay vì xóa hẳn khỏi hệ thống
                DatabaseConnection.ExecuteQuery($"UPDATE NGUOIDUNG SET MAGIADINH = NULL WHERE MANGUOIDUNG = '{ma}'");
                LoadThanhVien(currentMaGD);
            }
        }

        private void ShowThanhVienDialog(bool isAdd, string maND, string maVT, string hoTen, DateTime ngaySinh, string trangThai)
        {
            Form f = new Form { Width = 600, Height = 530, Text = "Sửa Thành Viên", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
            f.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label lblMa = new Label { Left = 20, Top = 23, Text = "Mã ND:", AutoSize = true }; TextBox txtMa = new TextBox { Left = 120, Top = 20, Width = 430, Text = maND, Enabled = false };
            Label lblTen = new Label { Left = 20, Top = 63, Text = "Họ Tên:", AutoSize = true }; TextBox txtTen = new TextBox { Left = 120, Top = 60, Width = 430, Text = hoTen, Enabled = false };
            Label lblNgay = new Label { Left = 20, Top = 103, Text = "Ngày Sinh:", AutoSize = true }; DateTimePicker dtpNgay = new DateTimePicker { Left = 120, Top = 100, Width = 430, Format = DateTimePickerFormat.Short, Value = ngaySinh, Enabled = false };
            Label lblVT = new Label { Left = 20, Top = 143, Text = "Vai Trò:", AutoSize = true }; ComboBox cbVT = new ComboBox { Left = 120, Top = 140, Width = 430, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblGrid = new Label { Left = 20, Top = 193, Text = "Tài sản Gia đình đang đại diện:", AutoSize = true, Font = new Font("Segoe UI", 9.75F, FontStyle.Bold) };
            DataGridView dgvTS = new DataGridView { Left = 20, Top = 220, Width = 530, Height = 180, AllowUserToAddRows = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = Color.White };

            Button btnSave = new Button { Left = 250, Top = 430, Width = 100, Text = "Lưu", BackColor = Color.FromArgb(242, 239, 231), ForeColor = Color.FromArgb(0, 106, 113), FlatStyle = FlatStyle.Flat };

            // Load Combobox VaiTro: Chỉ lấy Chủ gia đình và Người dùng
            cbVT.DataSource = DatabaseConnection.GetData("SELECT MAVAITRO, TENVAITRO FROM VAITRO WHERE MAVAITRO IN ('VT02', 'VT03')");
            cbVT.DisplayMember = "TENVAITRO"; cbVT.ValueMember = "MAVAITRO"; cbVT.SelectedValue = maVT;

            // Nếu người cần sửa là VT03 (Chủ gia đình) thì không cho phép đổi vai trò sang người dùng thường để tránh gia đình vô chủ
            if (maVT == "VT03")
            {
                cbVT.Enabled = false;
            }

            Action loadGridAndCombo = () => {
                dgvTS.DataSource = DatabaseConnection.GetData($"SELECT MATAISAN AS [Mã TS], TENTAISAN AS [Tên Tài Sản] FROM TAISAN WHERE MANGUOIDUNG = '{maND}' AND PHAMVI = N'Gia đình'");
                if (!dgvTS.Columns.Contains("Cập nhật"))
                {
                    DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                    btnCol.Name = "Cập nhật";
                    btnCol.HeaderText = "";
                    btnCol.Text = "Cập nhật";
                    btnCol.UseColumnTextForButtonValue = true;
                    btnCol.Width = 80;
                    dgvTS.Columns.Add(btnCol);
                }
            };
            loadGridAndCombo();

            dgvTS.CellClick += (senderGrid, eGrid) => {
                if (eGrid.RowIndex >= 0 && dgvTS.Columns[eGrid.ColumnIndex].Name == "Cập nhật")
                {
                    string maTS = dgvTS.Rows[eGrid.RowIndex].Cells["Mã TS"].Value.ToString();

                    Form fSelect = new Form { Width = 400, Height = 200, Text = "Chọn thành viên đại diện mới", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
                    Label lblUser = new Label { Left = 20, Top = 33, Text = "Chọn thành viên:", AutoSize = true };
                    ComboBox cbAssign = new ComboBox { Left = 150, Top = 30, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

                    DataTable dtMembers = DatabaseConnection.GetData($"SELECT MANGUOIDUNG, HOTEN + ' (' + MANGUOIDUNG + ')' AS TENDAYDU FROM NGUOIDUNG WHERE MAGIADINH = '{currentMaGD}' AND MANGUOIDUNG != '{maND}'");
                    if (dtMembers.Rows.Count == 0)
                    {
                        MessageBox.Show("Gia đình không có thành viên nào khác để nhận đại diện tài sản này!");
                        return;
                    }

                    cbAssign.DataSource = dtMembers;
                    cbAssign.DisplayMember = "TENDAYDU";
                    cbAssign.ValueMember = "MANGUOIDUNG";

                    Button btnConfirm = new Button { Left = 150, Top = 100, Width = 100, Text = "Xác nhận", BackColor = Color.FromArgb(242, 239, 231), FlatStyle = FlatStyle.Flat };
                    btnConfirm.Click += (sC, eC) => {
                        if (cbAssign.SelectedValue != null)
                        {
                            string newOwner = cbAssign.SelectedValue.ToString();
                            DatabaseConnection.ExecuteQuery($"UPDATE TAISAN SET MANGUOIDUNG = '{newOwner}' WHERE MATAISAN = '{maTS}'");
                            MessageBox.Show("Cập nhật đại diện tài sản thành công!");
                            fSelect.DialogResult = DialogResult.OK;
                            fSelect.Close();
                            loadGridAndCombo();
                        }
                    };

                    fSelect.Controls.AddRange(new Control[] { lblUser, cbAssign, btnConfirm });
                    fSelect.ShowDialog();
                }
            };

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    string vt = cbVT.SelectedValue.ToString();
                    string dateStr = dtpNgay.Value.ToString("yyyy-MM-dd");

                    DatabaseConnection.ExecuteQuery($"UPDATE NGUOIDUNG SET MAVAITRO='{vt}', HOTEN=N'{txtTen.Text}', NGAYSINH='{dateStr}' WHERE MANGUOIDUNG='{txtMa.Text}'");

                    f.DialogResult = DialogResult.OK;
                    f.Close();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            };
            f.Controls.AddRange(new Control[] { lblMa, txtMa, lblTen, txtTen, lblNgay, dtpNgay, lblVT, cbVT, lblGrid, dgvTS, btnSave });
            if (f.ShowDialog() == DialogResult.OK) LoadThanhVien(currentMaGD);
        }
    }
}
