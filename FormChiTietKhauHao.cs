using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormChiTietKhauHao : Form
    {
        private readonly string maTaiSan;
        private readonly SqlConnection conn;

        public FormChiTietKhauHao(string maTaiSan, SqlConnection conn)
        {
            InitializeComponent();
            this.maTaiSan = maTaiSan;
            this.conn = conn;
        }

        private void FormChiTietKhauHao_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#9ACBD0");
            string sql = "select tentaisan, nguyengia from taisan where mataisan = @maTaiSan";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@maTaiSan", maTaiSan);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            var culture = new System.Globalization.CultureInfo("vi-VN");
            if (reader.Read())
            {
                decimal nguyenGia = Convert.ToDecimal(reader["nguyengia"]);
                lblTenTaiSan.Text = "Tên tài sản: " + reader["tentaisan"].ToString();
                lblNguyenGia.Text = "Nguyên giá: " + nguyenGia.ToString("C0", culture);
            }
            reader.Close();
            sql = "select * from cokhauhao where mataisan = @maTaiSan";
            cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@maTaiSan", maTaiSan);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            dgv.DataSource = dt;
            dgv.Columns["mataisan"].Visible = false;
            dgv.Columns["giatrikhauhao"].HeaderText = "Giá trị khấu hao";
            dgv.Columns["giatriconlaisaukhauhao"].HeaderText = "Giá trị còn lại";
            dgv.Columns["namkhauhao"].HeaderText = "Năm khấu hao";
            dgv.Columns["giatrikhauhao"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
            dgv.Columns["giatrikhauhao"].DefaultCellStyle.Format = "C0";
            dgv.Columns["giatriconlaisaukhauhao"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
            dgv.Columns["giatriconlaisaukhauhao"].DefaultCellStyle.Format = "C0";
            decimal giaTriConLai = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1]["giatriconlaisaukhauhao"]);
            lblGiaTriConLai.Text = "Giá trị còn lại: " + giaTriConLai.ToString("C0", culture);
            conn.Close();
        }

        private void FormChiTietKhauHao_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
