using System;
using System.Data;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormAdminHome : Form
    {
        private string maGiaDinh = "";
        private string maNguoiDung = "";
        private string vaiTro = "";

        public FormAdminHome(string maGD = "GD01", string maND = "", string vTro = "")
        {
            InitializeComponent();
            maGiaDinh = maGD;
            maNguoiDung = maND;
            vaiTro = vTro;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                DataTable dtGiaDinh = DatabaseConnection.GetData($"SELECT TENGIADINH FROM GIADINH WHERE MAGIADINH = '{maGiaDinh}'");
                if (dtGiaDinh.Rows.Count > 0)
                    lblTongGiaDinh.Text = "Gia đình quản lý: " + dtGiaDinh.Rows[0][0].ToString();
                else
                    lblTongGiaDinh.Text = "Vui lòng tạo gia đình mới!";

                DataTable dtTaiSan = DatabaseConnection.GetData($@"
                    SELECT COUNT(*) FROM TAISAN 
                    WHERE MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{maGiaDinh}')");
                if (dtTaiSan.Rows.Count > 0)
                    lblTongTaiSan.Text = "Tổng tài sản sở hữu: " + dtTaiSan.Rows[0][0].ToString();

                DataTable dtNguoiDung = DatabaseConnection.GetData($"SELECT COUNT(*) FROM NGUOIDUNG WHERE MAGIADINH = '{maGiaDinh}'");
                if (dtNguoiDung.Rows.Count > 0)
                    lblTongThanhVien.Text = "Số lượng thành viên: " + dtNguoiDung.Rows[0][0].ToString();

                DataTable dtBaoTri = DatabaseConnection.GetData($@"
                    SELECT COUNT(*) FROM THONGTINBAOTRI 
                    WHERE TRANGTHAI != N'Hoàn thành' 
                    AND MATAISAN IN (SELECT MATAISAN FROM TAISAN WHERE MANGUOIDUNG IN (SELECT MANGUOIDUNG FROM NGUOIDUNG WHERE MAGIADINH = '{maGiaDinh}'))");
                if (dtBaoTri.Rows.Count > 0)
                    lblBaoTri.Text = "Tài sản sắp/đang bảo trì: " + dtBaoTri.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Dashboard: " + ex.Message);
            }
        }

        private void btnQLGiaDinh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maGiaDinh))
            {
                FormTaoGiaDinh frmTao = new FormTaoGiaDinh(maNguoiDung);
                if (frmTao.ShowDialog() == DialogResult.OK)
                {
                    // Lấy mã gia đình mới
                    DataTable dt = DatabaseConnection.GetData($"SELECT MAGIADINH, MAVAITRO FROM NGUOIDUNG WHERE MANGUOIDUNG = '{maNguoiDung}'");
                    if (dt.Rows.Count > 0)
                    {
                        maGiaDinh = dt.Rows[0]["MAGIADINH"].ToString().Trim();
                        // Quan trọng: Cập nhật lại vaiTro thành VT03 để truyền sang FormQLGiaDinh không bị ẩn nút
                        vaiTro = dt.Rows[0]["MAVAITRO"].ToString().Trim();
                    }
                    LoadDashboardData();
                    SetupUI();

                    // Chuyển luôn sang Form Quản lý gia đình sau khi tạo thành công
                    FormQLGiaDinh frm = new FormQLGiaDinh(maGiaDinh, vaiTro);
                    frm.FormClosed += (s, args) => LoadDashboardData();
                    frm.Show();
                }
            }
            else
            {
                FormQLGiaDinh frm = new FormQLGiaDinh(maGiaDinh, vaiTro);
                frm.FormClosed += (s, args) => LoadDashboardData();
                frm.Show();
            }
        }

        private void btnQLBaoTri_Click(object sender, EventArgs e)
        {
            FormQLBaoTri frm = new FormQLBaoTri(maGiaDinh, maNguoiDung, vaiTro);
            frm.FormClosed += (s, args) => LoadDashboardData();
            frm.Show();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            FormDangNhap frm = new FormDangNhap();
            this.Hide();
            frm.FormClosed += (s, args) => this.Close();
            frm.Show();
        }

        private void FormAdminHome_Load(object sender, EventArgs e)
        {
            SetupUI();
        }

        private void SetupUI()
        {
            if (string.IsNullOrEmpty(maGiaDinh))
            {
                btnQLGiaDinh.Text = "Tạo Gia Đình Mới";
            }
            else
            {
                btnQLGiaDinh.Text = "Quản lý Gia đình";
            }
        }
    }
}