using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormAdminHome : Form
    {
        private string maGiaDinh = "";

        public FormAdminHome(string maGD = "GD01")
        {
            InitializeComponent();
            maGiaDinh = maGD;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                DataTable dtGiaDinh = DatabaseConnection.GetData($"SELECT TENGIADINH FROM GIADINH WHERE MAGIADINH = '{maGiaDinh}'");
                if (dtGiaDinh.Rows.Count > 0)
                    lblTongGiaDinh.Text = "Gia đình quản lý: " + dtGiaDinh.Rows[0][0].ToString();

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
            FormQLGiaDinh frm = new FormQLGiaDinh(maGiaDinh);
            frm.FormClosed += (s, args) => LoadDashboardData();
            frm.Show();
        }

        private void btnQLBaoTri_Click(object sender, EventArgs e)
        {
            FormQLBaoTri frm = new FormQLBaoTri(maGiaDinh);
            frm.FormClosed += (s, args) => LoadDashboardData();
            frm.Show();
        }

        private void FormAdminHome_Load(object sender, EventArgs e)
        {

        }
    }
}