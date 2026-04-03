using Microsoft.Data.SqlClient;
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
    public partial class FormXemtaisan : Form
    {
        string maND; 
        SqlConnection conn;

        void KetNoi()
        {
            conn = new SqlConnection(
              @"Server =TuanThong\SQLEXPRESS; Database = QLTS_LTUD ; Integrated Security = True; TrustServerCertificate=True");
            conn.Open();
        }
        public FormXemtaisan(string ma)
        {
            InitializeComponent();
            KetNoi();
            maND = ma;
        }

        private void FormXemtaisan_Load(object sender, EventArgs e)
        {
            KetNoi();
            LoadTaiSan();

        }
        void LoadTaiSan()
{
    string sql = @"
    SELECT 
        MANGUOIDUNG AS [Mã người dùng],
        TENTAISAN AS [Tên tài sản],
        NGAYMUA AS [Ngày mua],
        TINHTRANG AS [Tình trạng],
        PHAMVI AS [Phạm vi]
    FROM TAISAN 
    WHERE MANGUOIDUNG = @ma";

    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
    da.SelectCommand.Parameters.AddWithValue("@ma", maND);

    DataTable dt = new DataTable();
    da.Fill(dt);

    dataGridView1.DataSource = dt;

    if (dt.Rows.Count == 0)
    {
        MessageBox.Show("Người dùng này chưa có tài sản!");
    }
}
    }
}
