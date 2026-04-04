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
        public TrangChuAdmin()
        {
            InitializeComponent();
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
