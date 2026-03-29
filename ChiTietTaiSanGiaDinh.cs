using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTUD
{
    public partial class ChiTietTaiSanGiaDinh : Form
    {
        string connectString = @"Server=Hung\SQLEXPRESS; Database=QLTaiSan_LTUD; Integrated Security=True";
        SqlConnection conn;
        String maTaiSan = "";
        String pathHinh = "";
        public ChiTietTaiSanGiaDinh(String ma)
        {
            InitializeComponent();
            maTaiSan = ma;
        }

        private void ChiTietTaiSanGiaDinh_Load(object sender, EventArgs e)
        {
            loadData();
        }

        void connectData()
        {
            conn = new SqlConnection(connectString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        void loadData()
        {
            try
            {
                connectData();
                String lblSql = @"SELECT * FROM TAISAN ts 
                                JOIN LOAITAISAN lts ON ts.MALOAI = lts.MALOAI
                                JOIN PHUONGPHAPKHAUHAO ppkh ON ppkh.MAPP = lts.MAPP
                                JOIN NGUOIDUNG nd ON ts.MANGUOIDUNG = nd.MANGUOIDUNG
                                JOIN GIADINH gd ON gd.MAGIADINH = nd.MAGIADINH
                                WHERE MATAISAN = @ma";
                SqlCommand cmd = new SqlCommand(lblSql, conn);
                cmd.Parameters.AddWithValue("@ma", maTaiSan);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblMaTaiSan.Text = reader["MATAISAN"].ToString();
                    lblTenTaiSan.Text = reader["TENTAISAN"].ToString();
                    lblLoaiTaiSan.Text = reader["TENLOAI"].ToString();
                    lblGiaDinh.Text = reader["TENGIADINH"].ToString();
                    lblSoHuu.Text = reader["HOTEN"].ToString();
                    lblNgayMua.Text = reader["NGAYMUA"].ToString();
                    lblNguyenGia.Text = reader["NGUYENGIA"].ToString();
                    lblTinhTrang.Text = reader["TINHTRANG"].ToString();
                    lblPhuongPhap.Text = reader["TENPHUONGPHAP"].ToString();
                    pathHinh = reader["HINHANH"].ToString();
                    if (!String.IsNullOrEmpty(pathHinh)) 
                    { 
                        pic.Image = Image.FromFile(pathHinh);
                    }
                    reader.Close();
                }

                String dgvSql = @"SELECT NAMKHAUHAO, GIATRIKHAUHAO, GIATRICONLAISAUKHAUHAO 
                                FROM COKHAUHAO WHERE MATAISAN = @ma
                                ORDER BY NAMKHAUHAO";
                SqlDataAdapter da = new SqlDataAdapter(dgvSql, conn);
                da.SelectCommand.Parameters.AddWithValue("@ma", maTaiSan);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvKhauHao.DataSource = dt;

                dgvKhauHao.Columns[0].HeaderText = "NĂM KHẤU HAO";
                dgvKhauHao.Columns[1].HeaderText = "GIÁ TRỊ KHẤU HAO";
                dgvKhauHao.Columns[2].HeaderText = "GIÁ TRỊ CÒN LẠI SAU KHẤU HAO";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }
    }
}
