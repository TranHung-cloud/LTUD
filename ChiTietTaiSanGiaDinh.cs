using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTUD
{
    public partial class ChiTietTaiSanGiaDinh : Form
    {
        string connectString = @"Server =TuanThong\SQLEXPRESS; Database = QLTS_LTUD ; Integrated Security = True; TrustServerCertificate=True";
        SqlConnection conn;
        String maTaiSan = "";
        public ChiTietTaiSanGiaDinh(String ma)
        {
            InitializeComponent();
            maTaiSan = ma;
        }

        private void ChiTietTaiSanGiaDinh_Load(object sender, EventArgs e)
        {
            depreciationUpdate();
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
                    lblNgayMua.Text = Convert.ToDateTime(reader["NGAYMUA"]).ToString("dd/MM/yyyy");
                    lblNguyenGia.Text = reader["NGUYENGIA"].ToString();
                    lblTinhTrang.Text = reader["TINHTRANG"].ToString();
                    lblPhuongPhap.Text = reader["TENPHUONGPHAP"].ToString();
                    string s = reader["HINHANH"] as string;
                    if (!string.IsNullOrEmpty(s))
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(s);
                            using (var ms = new MemoryStream(bytes))
                            {
                                pic.Image = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            pic.Image = null;
                        }
                    }
                    else
                    {
                        pic.Image = null;
                    }
                    reader.Close();
                }

                String dgvSql = @"SELECT NAMKHAUHAO, GIATRIKHAUHAO, GIATRICONLAISAUKHAUHAO 
                                FROM COKHAUHAO WHERE MATAISAN = @ma
                                ORDER BY NAMKHAUHAO DESC";
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

        void depreciationUpdate()
        {
            connectData();

            string sql = @"
        SELECT TOP 1 
            ts.NGAYMUA, 
            ts.NGUYENGIA,
            ISNULL(ckh.NAMKHAUHAO, YEAR(ts.NGAYMUA)) as MaxYear,
            ISNULL(ckh.GIATRICONLAISAUKHAUHAO, ts.NGUYENGIA) as CurrentRemain,
            (ts.NGUYENGIA / lts.SONAMSUDUNG) as YearlyDepreciation
        FROM TAISAN ts 
        JOIN LOAITAISAN lts ON ts.MALOAI = lts.MALOAI
        LEFT JOIN COKHAUHAO ckh ON TRIM(ts.MATAISAN) = TRIM(ckh.MATAISAN) 
        WHERE TRIM(ts.MATAISAN) = @ma
        ORDER BY ckh.NAMKHAUHAO DESC";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma", maTaiSan.Trim());

            DateTime purchaseDate = DateTime.Now;
            int lastYear = 0;
            double currentRemainValue = 0;
            double depreciationValue = 0;
            bool hasData = false;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    purchaseDate = Convert.ToDateTime(reader["NGAYMUA"]);
                    lastYear = Convert.ToInt32(reader["MaxYear"]);
                    currentRemainValue = Convert.ToDouble(reader["CurrentRemain"]);
                    depreciationValue = Convert.ToDouble(reader["YearlyDepreciation"]);
                    hasData = true;
                }
            }

            if (!hasData) return;

            DateTime today = DateTime.Now.Date;

            while (true)
            {
                int nextYear = lastYear + 1;

                DateTime nextDueDate = new DateTime(nextYear, purchaseDate.Month, purchaseDate.Day);

                if (nextYear <= today.Year && today >= nextDueDate)
                {
                    string checkSql = "SELECT COUNT(*) FROM THOIDIEMKHAUHAO WHERE NAMKHAUHAO = @nam";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@nam", nextYear);
                    if ((int)checkCmd.ExecuteScalar() == 0)
                    {
                        new SqlCommand("INSERT INTO THOIDIEMKHAUHAO (NAMKHAUHAO) VALUES (" + nextYear + ")", conn).ExecuteNonQuery();
                    }

                    currentRemainValue -= depreciationValue;
                    if (currentRemainValue < 0) currentRemainValue = 0;

                    string insSql = @"INSERT INTO COKHAUHAO (MATAISAN, NAMKHAUHAO, GIATRIKHAUHAO, GIATRICONLAISAUKHAUHAO) 
                             VALUES (@ma, @dy, @dv, @rv)";
                    using (SqlCommand insCmd = new SqlCommand(insSql, conn))
                    {
                        insCmd.Parameters.AddWithValue("@ma", maTaiSan.Trim());
                        insCmd.Parameters.AddWithValue("@dy", nextYear);
                        insCmd.Parameters.AddWithValue("@dv", depreciationValue);
                        insCmd.Parameters.AddWithValue("@rv", currentRemainValue);
                        insCmd.ExecuteNonQuery();
                    }

                    lastYear = nextYear;
                    if (currentRemainValue <= 0) break;
                }
                else
                {
                    break;
                }
            }
        }

    }
}
