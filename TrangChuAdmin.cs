using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTUD
{
    public partial class TrangChuAdmin : Form
    {
        private string maNguoiDung;

        public TrangChuAdmin(string maND = "")
        {
            InitializeComponent();
            maNguoiDung = maND;
            this.Load += TrangChuAdmin_Load;
        }

        private void TrangChuAdmin_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(maNguoiDung))
            {
                try
                {
                    DataTable dtUser = DatabaseConnection.GetData($"SELECT HOTEN FROM NGUOIDUNG WHERE MANGUOIDUNG = '{maNguoiDung}'");
                    if (dtUser.Rows.Count > 0)
                    {
                        string hoTen = dtUser.Rows[0]["HOTEN"].ToString();
                        this.Text = $"Trang Chủ Admin - Xin chào: {hoTen}";
                    }
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xin chào, đây là phần mềm đầu tiên của tôi!");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegisterForm formDangKy = new RegisterForm();
            formDangKy.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đăng nhập thành công! Chào mừng bạn đến với phần mềm Quản lý tài sản.");
        }
    }
}
