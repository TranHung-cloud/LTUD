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
    public partial class FormTrangchu : System.Windows.Forms.Form
    {
        string maNguoiDung;
        string maGiaDinh;
        string vaiTro;
        private readonly FormDangNhap formDangNhap;
        private bool returnLogin = false;

        public FormTrangchu(string maND, string maGD = "", string vTro = "VT02", FormDangNhap formDangNhap = null)
        {
            InitializeComponent();
            maNguoiDung = maND;
            maGiaDinh = maGD;
            vaiTro = vTro;
            this.formDangNhap = formDangNhap;
        }

        private void FormTrangchu_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maGiaDinh))
            {
                btnTaisangiadinh.Enabled = false;
                btnThongtinbaotri.Enabled = false;
                btnQuanlygiadinh.Enabled = false;
                taoGiaDinhToolStripMenuItem.Visible = true;
            }
            else
            {
                btnTaisangiadinh.Enabled = true;
                btnThongtinbaotri.Enabled = true;
                btnQuanlygiadinh.Enabled = true;
                taoGiaDinhToolStripMenuItem.Visible = false;
            }
        }

        private void taoGiaDinhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTaoGiaDinh f = new FormTaoGiaDinh(maNguoiDung);
            this.Hide();
            f.FormClosed += (s, ev) => 
            {
                try
                {
                    DataTable dtUser = DatabaseConnection.GetData($"SELECT MAGIADINH, MAVAITRO FROM NGUOIDUNG WHERE MANGUOIDUNG = '{maNguoiDung}'");
                    if (dtUser.Rows.Count > 0)
                    {
                        maGiaDinh = dtUser.Rows[0]["MAGIADINH"].ToString().Trim();
                        vaiTro = dtUser.Rows[0]["MAVAITRO"].ToString().Trim();
                    }
                }
                catch { }
                FormTrangchu_Load(null, null);
                this.Show();
            };
            f.Show();
        }

        private void thôngTinGiaĐìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormThongtingiadinh f = new FormThongtin(maNguoiDung);
            //f.Show();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyThongTinCaNhanCuaNguoiDung quanLyThongTinCaNhanCuaNguoiDung = new QuanLyThongTinCaNhanCuaNguoiDung(maNguoiDung, this);
            this.Hide();
            quanLyThongTinCaNhanCuaNguoiDung.Show();
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formDangNhap.Show();
            returnLogin = true;
            this.Close();

        }
        private void btnTaisancanhan_Click(object sender, EventArgs e)
        {
            FormQLTS_CN f = new FormQLTS_CN(maNguoiDung, this);
            this.Hide();
            f.Show();
        }

        private void btnTaisangiadinh_Click(object sender, EventArgs e)
        {
            QLTaiSanGiaDinh f = new QLTaiSanGiaDinh(maNguoiDung);
            //this.Hide();
            f.Show();
        }

        private void btnThongtinbaotri_Click(object sender, EventArgs e)
        {
            FormQLBaoTri frm = new FormQLBaoTri(maGiaDinh, maNguoiDung, vaiTro);
            this.Hide();
            frm.FormClosed += (s, args) => this.Show();
            frm.Show();
        }

        private void btnQuanlygiadinh_Click(object sender, EventArgs e)
        {
            FormQLGiaDinh f = new FormQLGiaDinh(maGiaDinh, vaiTro);
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void FormTrangchu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!returnLogin)
            {
                formDangNhap.Show();
            }
        }
    }
}
