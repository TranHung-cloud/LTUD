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

        public FormTrangchu(string maND, string maGD = "", string vTro = "VT02")
        {
            InitializeComponent();
            maNguoiDung = maND;
            maGiaDinh = maGD;
            vaiTro = vTro;
        }

        private void FormTrangchu_Load(object sender, EventArgs e)
        {

        }

        private void thôngTinGiaĐìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormThongtingiadinh f = new FormThongtin(maNguoiDung);
            //f.Show();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormThongtincanhan f = new FormThongtincanhan(maNguoiDung);
            //f.Show();
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDangNhap f = new FormDangNhap();
            this.Hide();
            f.Show();
        }
        private void btnTaisancanhan_Click(object sender, EventArgs e)
        {
            FormQLTS_CN f = new FormQLTS_CN(maNguoiDung);
            //this.Hide();
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
            //this.Hide();
            frm.Show();
        }

        private void btnQuanlygiadinh_Click(object sender, EventArgs e)
        {
            FormQLGiaDinh f = new FormQLGiaDinh(maGiaDinh, vaiTro);
            f.Show();
        }
    }
}
