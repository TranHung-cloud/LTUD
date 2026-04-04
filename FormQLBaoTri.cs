using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormQLBaoTri : Form
    {
        private string adminMaGD = "";
        private string maNguoiDung = "";
        private string vaiTro = "";

        public FormQLBaoTri(string maGD = "GD01", string mND = "", string vTro = "")
        {
            InitializeComponent();
            adminMaGD = maGD;
            maNguoiDung = mND;
            vaiTro = vTro;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
        }

        private void FormQLBaoTri_Load(object sender, EventArgs e)
        {
            LoadComboBoxPhamVi();
        }

        private void LoadComboBoxPhamVi()
        {
            try
            {
                cbPhamViFilter.Items.Clear();
                cbPhamViFilter.Items.AddRange(new string[] { "Tất cả", "Gia đình", "Cá nhân" });
                cbPhamViFilter.SelectedIndex = 0;
                cbPhamViFilter.SelectedIndexChanged += (s, ev) => LoadComboBoxTaiSanFilter();
                LoadComboBoxTaiSanFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phạm vi: " + ex.Message);
            }
        }

        private void LoadComboBoxTaiSanFilter()
        {
            if (cbPhamViFilter.SelectedItem == null) return;
            string selectedPV = cbPhamViFilter.SelectedItem.ToString();

            try
            {
                string query = $@"
                    SELECT MATAISAN, TENTAISAN + ' (' + MATAISAN + ')' AS TENDAYDU 
                    FROM TAISAN 
                    WHERE MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{adminMaGD}')
                    AND (PHAMVI = N'Gia đình' OR (PHAMVI = N'Cá nhân' AND MANGUOIDUNG = '{maNguoiDung}'))";

                if (selectedPV != "Tất cả")
                {
                    query += $" AND PHAMVI = N'{selectedPV}'";
                }

                DataTable dtTS = DatabaseConnection.GetData(query);
                DataRow rowAll = dtTS.NewRow();
                rowAll["MATAISAN"] = "ALL";
                rowAll["TENDAYDU"] = "-- Tất cả Tài sản --";
                dtTS.Rows.InsertAt(rowAll, 0);

                // Unregister event before updating DataSource to prevent firing prematurely
                cbTaiSanFilter.SelectedIndexChanged -= CbTaiSanFilter_SelectedIndexChanged;
                cbTaiSanFilter.DataSource = dtTS;
                cbTaiSanFilter.DisplayMember = "TENDAYDU";
                cbTaiSanFilter.ValueMember = "MATAISAN";
                cbTaiSanFilter.SelectedIndexChanged += CbTaiSanFilter_SelectedIndexChanged;

                LoadBaoTri();
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Lỗi tải danh sách tài sản: " + ex.Message);
            }
        }

        private void CbTaiSanFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBaoTri();
        }

        private void LoadBaoTri()
        {
            if (cbPhamViFilter.SelectedItem == null || cbTaiSanFilter.SelectedValue == null) return;
            string selectedPV = cbPhamViFilter.SelectedItem.ToString();
            string selectedTS = cbTaiSanFilter.SelectedValue.ToString();

            try
            {
                string query = $@"
                    SELECT 
                        B.MALICH AS [Mã Lịch],
                        T.TENTAISAN AS [Tên Tài Sản],
                        T.MATAISAN AS [Mã Tài Sản],
                        T.PHAMVI AS [Phạm Vi],
                        B.NGAYBAOTRI AS [Ngày Bảo Trì],
                        B.TRANGTHAI AS [Trạng Thái],
                        B.CHIPHI AS [Chi Phí],
                        B.NOIDUNGBAOTRI AS [Nội Dung]
                    FROM THONGTINBAOTRI B
                    INNER JOIN TAISAN T ON B.MATAISAN = T.MATAISAN
                    WHERE T.MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{adminMaGD}')
                    AND (T.PHAMVI = N'Gia đình' OR (T.PHAMVI = N'Cá nhân' AND T.MANGUOIDUNG = '{maNguoiDung}'))";

                if (selectedPV != "Tất cả")
                {
                    query += $" AND T.PHAMVI = N'{selectedPV}'";
                }

                if (selectedTS != "ALL")
                {
                    query += $" AND T.MATAISAN = '{selectedTS}'";
                }

                query += " ORDER BY B.MALICH ASC";
                dgvBaoTri.DataSource = DatabaseConnection.GetData(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Bảo trì: " + ex.Message);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ShowBaoTriDialog(true, "", "", DateTime.Now, "Đang xử lý", "0", "");
        }

        private bool KiemTraQuyenThucHien(string maLich = "", string maTS_Input = "")
        {
            if (vaiTro == "VT01") return true; // Cấp quyền cao nhất (nếu có)

            string phamVi = "";
            string nguoiSoHuu = "";

            if (!string.IsNullOrEmpty(maLich))
            {
                DataTable dt = DatabaseConnection.GetData($"SELECT T.PHAMVI, T.MANGUOIDUNG FROM THONGTINBAOTRI B JOIN TAISAN T ON B.MATAISAN = T.MATAISAN WHERE B.MALICH = '{maLich}'");
                if (dt.Rows.Count > 0)
                {
                    phamVi = dt.Rows[0]["PHAMVI"].ToString().Trim();
                    nguoiSoHuu = dt.Rows[0]["MANGUOIDUNG"].ToString().Trim();
                }
            }
            else if (!string.IsNullOrEmpty(maTS_Input))
            {
                DataTable dt = DatabaseConnection.GetData($"SELECT PHAMVI, MANGUOIDUNG FROM TAISAN WHERE MATAISAN = '{maTS_Input}'");
                if (dt.Rows.Count > 0)
                {
                    phamVi = dt.Rows[0]["PHAMVI"].ToString().Trim();
                    nguoiSoHuu = dt.Rows[0]["MANGUOIDUNG"].ToString().Trim();
                }
            }
            else
            {
                return true; // Thêm mới, quyền sẽ check ở lúc chọn tài sản (loadTS)
            }

            if (vaiTro == "VT03") // Chủ gia đình
            {
                bool isGiaDinh = phamVi.Equals("Gia đình", StringComparison.OrdinalIgnoreCase);
                bool isCaNhan = phamVi.Equals("Cá nhân", StringComparison.OrdinalIgnoreCase);
                bool isOwner = nguoiSoHuu.Equals(maNguoiDung, StringComparison.OrdinalIgnoreCase);

                if (!isGiaDinh && !(isCaNhan && isOwner))
                {
                    MessageBox.Show("Chủ gia đình có quyền sửa lịch bảo trì tài sản Gia đình hoặc tài sản Cá nhân của chính mình!", "Quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else if (vaiTro == "VT02") // Người dùng
            {
                bool isCaNhan = phamVi.Equals("Cá nhân", StringComparison.OrdinalIgnoreCase);
                bool isOwner = nguoiSoHuu.Equals(maNguoiDung, StringComparison.OrdinalIgnoreCase);

                if (!isCaNhan || !isOwner)
                {
                    MessageBox.Show("Người dùng chỉ có quyền sửa/xóa các lịch bảo trì của tài sản thuộc phạm vi Cá Nhân và do chính mình sở hữu!", "Quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvBaoTri.CurrentRow == null) return;
            string maLich = dgvBaoTri.CurrentRow.Cells[0].Value.ToString();

            if (!KiemTraQuyenThucHien(maLich)) return;

            string maTS = dgvBaoTri.CurrentRow.Cells[2].Value.ToString();
            DateTime ngay = Convert.ToDateTime(dgvBaoTri.CurrentRow.Cells[4].Value);
            string tt = dgvBaoTri.CurrentRow.Cells[5].Value.ToString();
            string chiphi = dgvBaoTri.CurrentRow.Cells[6].Value.ToString();
            string nDung = dgvBaoTri.CurrentRow.Cells[7].Value.ToString();
            ShowBaoTriDialog(false, maLich, maTS, ngay, tt, chiphi, nDung);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvBaoTri.CurrentRow == null) return;
            string maLich = dgvBaoTri.CurrentRow.Cells[0].Value.ToString();

            if (!KiemTraQuyenThucHien(maLich)) return;

            if (MessageBox.Show("Xác nhận xóa lịch bảo trì này?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseConnection.ExecuteQuery($"DELETE FROM THONGTINBAOTRI WHERE MALICH = '{maLich}'");
                LoadBaoTri();
            }
        }

        private void ShowBaoTriDialog(bool isAdd, string mLich, string mTS, DateTime nBaoTri, string trThai, string cPhi, string nnDung)
        {
            Form f = new Form { Width = 500, Height = 420, Text = isAdd ? "Thêm Bảo Trì" : "Sửa Bảo Trì", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
            f.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Label lblLich = new Label { Left = 20, Top = 23, Text = "Mã Lịch:", AutoSize = true }; TextBox txtLich = new TextBox { Left = 120, Top = 20, Width = 330, Text = mLich, Enabled = isAdd };

            Label lblPV = new Label { Left = 20, Top = 63, Text = "Phạm Vi:", AutoSize = true }; ComboBox cbPV = new ComboBox { Left = 120, Top = 60, DropDownStyle = ComboBoxStyle.DropDownList, Width = 330 };
            if (vaiTro == "VT03") cbPV.Items.AddRange(new string[] { "Gia đình", "Cá nhân" });
            else cbPV.Items.AddRange(new string[] { "Cá nhân" });

            Label lblTS = new Label { Left = 20, Top = 103, Text = "Tài Sản:", AutoSize = true }; ComboBox cbTS = new ComboBox { Left = 120, Top = 100, DropDownStyle = ComboBoxStyle.DropDownList, Width = 330 };
            Label lblNgay = new Label { Left = 20, Top = 143, Text = "Ngày BT:", AutoSize = true }; DateTimePicker dtpNgay = new DateTimePicker { Left = 120, Top = 140, Width = 330, Format = DateTimePickerFormat.Short, Value = nBaoTri };
            Label lblTT = new Label { Left = 20, Top = 183, Text = "Trạng Thái:", AutoSize = true }; ComboBox cbTT = new ComboBox { Left = 120, Top = 180, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTT.Items.AddRange(new string[] { "Hoàn thành", "Đang xử lý", "Chưa xử lý" });
            cbTT.SelectedItem = string.IsNullOrEmpty(trThai) ? "Đang xử lý" : trThai;
            Label lblCP = new Label { Left = 20, Top = 223, Text = "Chi Phí:", AutoSize = true }; TextBox txtCP = new TextBox { Left = 120, Top = 220, Width = 330, Text = cPhi };
            Label lblND = new Label { Left = 20, Top = 263, Text = "Nội Dung:", AutoSize = true }; TextBox txtND = new TextBox { Left = 120, Top = 260, Width = 330, Text = nnDung };
            Button btnSave = new Button { Left = 200, Top = 320, Width = 100, Text = "Lưu", BackColor = Color.FromArgb(242, 239, 231), ForeColor = Color.FromArgb(0, 106, 113), FlatStyle = FlatStyle.Flat };

            Action loadTS = () => {
                string pv = cbPV.SelectedItem?.ToString();
                string query = $@"
                    SELECT MATAISAN, TENTAISAN + ' (' + MATAISAN + ')' AS TENDAYDU 
                    FROM TAISAN 
                    WHERE PHAMVI = N'{pv}' AND MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{adminMaGD}')";

                if (vaiTro == "VT03") // Chủ gia đình 
                {
                    if (pv != null && pv.Equals("Cá nhân", StringComparison.OrdinalIgnoreCase))
                    {
                        query += $" AND MANGUOIDUNG = '{maNguoiDung}'";
                    }
                }
                else if (vaiTro == "VT02") // Người dùng
                {
                    query += $" AND MANGUOIDUNG = '{maNguoiDung}'";
                }

                cbTS.DataSource = DatabaseConnection.GetData(query);
                cbTS.DisplayMember = "TENDAYDU"; cbTS.ValueMember = "MATAISAN";
            };

            cbPV.SelectedIndexChanged += (s, e) => loadTS();

            if (!isAdd && !string.IsNullOrEmpty(mTS))
            {
                DataTable dtTSInfo = DatabaseConnection.GetData($"SELECT PHAMVI FROM TAISAN WHERE MATAISAN = '{mTS}'");
                if (dtTSInfo.Rows.Count > 0) cbPV.SelectedItem = dtTSInfo.Rows[0]["PHAMVI"].ToString();
                else cbPV.SelectedIndex = 0;
                loadTS();
                cbTS.SelectedValue = mTS;
            }
            else
            {
                cbPV.SelectedIndex = 0;
            }

            btnSave.Click += (s, ev) =>
            {
                if(cbTS.SelectedValue == null) { MessageBox.Show("Vui lòng chọn Tài sản!"); return; }

                // Kiểm tra bảo mật lần cuối trước khi lưu
                string ts = cbTS.SelectedValue.ToString();
                DataTable dtCheck = DatabaseConnection.GetData($"SELECT PHAMVI, MANGUOIDUNG FROM TAISAN WHERE MATAISAN = '{ts}'");
                if (dtCheck.Rows.Count > 0)
                {
                    string pvCheck = dtCheck.Rows[0]["PHAMVI"].ToString().Trim();
                    string ownerCheck = dtCheck.Rows[0]["MANGUOIDUNG"].ToString().Trim();
                    if (vaiTro == "VT02" && (pvCheck == "Gia đình" || ownerCheck != maNguoiDung))
                    {
                        MessageBox.Show("Người dùng chỉ có quyền thêm lịch bảo trì cho tài sản thuộc phạm vi Cá Nhân và do chính mình sở hữu!", "Quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                try
                {
                    string tt = cbTT.SelectedItem.ToString();
                    string dateStr = dtpNgay.Value.ToString("yyyy-MM-dd");
                    if (isAdd)
                        DatabaseConnection.ExecuteQuery($"INSERT INTO THONGTINBAOTRI VALUES ('{txtLich.Text}', '{ts}', '{dateStr}', N'{tt}', {txtCP.Text}, N'{txtND.Text}')");
                    else
                        DatabaseConnection.ExecuteQuery($"UPDATE THONGTINBAOTRI SET MATAISAN='{ts}', NGAYBAOTRI='{dateStr}', TRANGTHAI=N'{tt}', CHIPHI={txtCP.Text}, NOIDUNGBAOTRI=N'{txtND.Text}' WHERE MALICH='{txtLich.Text}'");
                    f.DialogResult = DialogResult.OK;
                    f.Close();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            };

            f.Controls.AddRange(new Control[] { lblLich, txtLich, lblPV, cbPV, lblTS, cbTS, lblNgay, dtpNgay, lblTT, cbTT, lblCP, txtCP, lblND, txtND, btnSave });
            if (f.ShowDialog() == DialogResult.OK) LoadBaoTri();
        }

        private void dgvBaoTri_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}