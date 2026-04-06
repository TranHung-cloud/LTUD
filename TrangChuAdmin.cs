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
        private readonly FormDangNhap formDangNhap;
        private bool isExit = true;

        public TrangChuAdmin(string maND = "", FormDangNhap formDangNhap = null)
        {
            InitializeComponent();
            maNguoiDung = maND;
            this.formDangNhap = formDangNhap;
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
            QuanLyLoaiTaiSan formLoaiTaiSan = new QuanLyLoaiTaiSan();
            this.Hide();
            formLoaiTaiSan.ShowDialog();
            this.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormQLNguoidung f = new FormQLNguoidung(); 
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            formDangNhap.Show();
            isExit = false;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QuanLyPhuongPhapKhauHao formKhauHao = new QuanLyPhuongPhapKhauHao();
            this.Hide();
            formKhauHao.ShowDialog();
            this.Show();
        }

        private void TrangChuAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isExit)
            {
                Application.Exit();
            }
        }
    }
}
