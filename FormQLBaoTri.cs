using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormQLBaoTri : Form
    {
        private string adminMaGD = "";

        public FormQLBaoTri(string maGD = "GD01")
        {
            InitializeComponent();
            adminMaGD = maGD;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
        }

        private void FormQLBaoTri_Load(object sender, EventArgs e)
        {
            LoadBaoTri();
        }

        private void LoadBaoTri()
        {
            try
            {
                string query = $@"
                    SELECT 
                        MALICH AS [Mã Lịch],
                        MATAISAN AS [Mã Tài Sản],
                        NGAYBAOTRI AS [Ngày Bảo Trì],
                        TRANGTHAI AS [Trạng Thái],
                        CHIPHI AS [Chi Phí],
                        NOIDUNGBAOTRI AS [Nội Dung]
                    FROM THONGTINBAOTRI
                    WHERE MATAISAN IN (SELECT MATAISAN FROM TAISAN WHERE MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{adminMaGD}'))
                    ORDER BY MALICH ASC";
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

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvBaoTri.CurrentRow == null) return;
            string maLich = dgvBaoTri.CurrentRow.Cells[0].Value.ToString();
            string maTS = dgvBaoTri.CurrentRow.Cells[1].Value.ToString();
            DateTime ngay = Convert.ToDateTime(dgvBaoTri.CurrentRow.Cells[2].Value);
            string tt = dgvBaoTri.CurrentRow.Cells[3].Value.ToString();
            string chiphi = dgvBaoTri.CurrentRow.Cells[4].Value.ToString();
            string nDung = dgvBaoTri.CurrentRow.Cells[5].Value.ToString();
            ShowBaoTriDialog(false, maLich, maTS, ngay, tt, chiphi, nDung);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvBaoTri.CurrentRow == null) return;
            string maLich = dgvBaoTri.CurrentRow.Cells[0].Value.ToString();
            if (MessageBox.Show("Xác nhận xóa lịch bảo trì này?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseConnection.ExecuteQuery($"DELETE FROM THONGTINBAOTRI WHERE MALICH = '{maLich}'");
                LoadBaoTri();
            }
        }

        private void ShowBaoTriDialog(bool isAdd, string mLich, string mTS, DateTime nBaoTri, string trThai, string cPhi, string nnDung)
        {
            Form f = new Form { Width = 500, Height = 380, Text = isAdd ? "Thêm Bảo Trì" : "Sửa Bảo Trì", StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(154, 203, 208) };
            Label lblLich = new Label { Left = 20, Top = 23, Text = "Mã Lịch:", AutoSize = true }; TextBox txtLich = new TextBox { Left = 120, Top = 20, Width = 330, Text = mLich, Enabled = isAdd };
            Label lblTS = new Label { Left = 20, Top = 63, Text = "Tài Sản:", AutoSize = true }; ComboBox cbTS = new ComboBox { Left = 120, Top = 60, DropDownStyle = ComboBoxStyle.DropDownList, Width = 330 };
            Label lblNgay = new Label { Left = 20, Top = 103, Text = "Ngày BT:", AutoSize = true }; DateTimePicker dtpNgay = new DateTimePicker { Left = 120, Top = 100, Width = 330, Format = DateTimePickerFormat.Short, Value = nBaoTri };
            Label lblTT = new Label { Left = 20, Top = 143, Text = "Trạng Thái:", AutoSize = true }; ComboBox cbTT = new ComboBox { Left = 120, Top = 140, Width = 330, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTT.Items.AddRange(new string[] { "Hoàn thành", "Đang xử lý", "Chưa xử lý" });
            cbTT.SelectedItem = string.IsNullOrEmpty(trThai) ? "Đang xử lý" : trThai;
            Label lblCP = new Label { Left = 20, Top = 183, Text = "Chi Phí:", AutoSize = true }; TextBox txtCP = new TextBox { Left = 120, Top = 180, Width = 330, Text = cPhi };
            Label lblND = new Label { Left = 20, Top = 223, Text = "Nội Dung:", AutoSize = true }; TextBox txtND = new TextBox { Left = 120, Top = 220, Width = 330, Text = nnDung };
            Button btnSave = new Button { Left = 200, Top = 280, Width = 100, Text = "Lưu", BackColor = Color.FromArgb(72, 166, 167), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            // Load DS Tài Sản theo gia đình
            cbTS.DataSource = DatabaseConnection.GetData($@"
                SELECT MATAISAN, TENTAISAN + ' (' + MATAISAN + ')' AS TENDAYDU 
                FROM TAISAN 
                WHERE MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{adminMaGD}')");
            cbTS.DisplayMember = "TENDAYDU"; cbTS.ValueMember = "MATAISAN";
            if (!isAdd) cbTS.SelectedValue = mTS;

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    string ts = cbTS.SelectedValue.ToString();
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

            f.Controls.AddRange(new Control[] { lblLich, txtLich, lblTS, cbTS, lblNgay, dtpNgay, lblTT, cbTT, lblCP, txtCP, lblND, txtND, btnSave });
            if (f.ShowDialog() == DialogResult.OK) LoadBaoTri();
        }

        private void dgvBaoTri_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}