using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // Thêm thư viện này
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTUD
{
    public partial class FormReportTSCN : Form
    {
        private readonly FormQLTS_CN formQLTS_CN;
        private readonly string userId;

        public FormReportTSCN(FormQLTS_CN formQLTS_CN, string userId)
        {
            InitializeComponent();
            this.formQLTS_CN = formQLTS_CN;
            this.userId = userId;
        }
        private void FormReportTSCN_Load(object sender, EventArgs e)
        {
            // Nhận dữ liệu làm nguồn cho Report 
            string sql = @"SELECT NGUOIDUNG.HOTEN, NGUOIDUNG.MANGUOIDUNG, TAISAN.MATAISAN, 
                                        TAISAN.MALOAI, TAISAN.TENTAISAN, TAISAN.NGAYMUA, TAISAN.NGUYENGIA, 
                                        TAISAN.TINHTRANG, TAISAN.PHAMVI, COKHAUHAO.NAMKHAUHAO, 
                                        COKHAUHAO.GIATRIKHAUHAO, COKHAUHAO.GIATRICONLAISAUKHAUHAO, 
                                        LOAITAISAN.TENLOAI, LOAITAISAN.SONAMSUDUNG
                                 FROM   COKHAUHAO 
                                 RIGHT JOIN TAISAN ON COKHAUHAO.MATAISAN = TAISAN.MATAISAN 
                                 INNER JOIN NGUOIDUNG ON TAISAN.MANGUOIDUNG = NGUOIDUNG.MANGUOIDUNG 
                                 INNER JOIN LOAITAISAN ON TAISAN.MALOAI = LOAITAISAN.MALOAI
                                 WHERE NGUOIDUNG.MANGUOIDUNG = @userId"
                                 ; 
            DataTable dt = new DataTable();          
            // Thay thế hàm MyPublics.OpenData bằng kết nối trực tiếp đến DataBase
            string connectionString = @"Server =.\SQLEXPRESS; Database = QLTaiSan_LTUD; Integrated Security = True; TrustServerCertificate=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    try
                    {
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                this.Text = "Báo cáo Tài sản Cá nhân";
                this.WindowState = FormWindowState.Normal;

                ReportViewer rpvReport = new ReportViewer();
                this.Controls.Add(rpvReport);
                rpvReport.Dock = DockStyle.Fill;
                rpvReport.ProcessingMode = ProcessingMode.Local;

                // Cập nhật đường dẫn tới file rdlc hiện tại của bạn
                rpvReport.LocalReport.ReportEmbeddedResource = "LTUD.rptTSCN.rdlc";

                // Tạo nguồn dữ liệu cho báo cáo 
                ReportDataSource rdsTaiSan = new ReportDataSource();
                
                // Thay thế bằng tên DataSet mà bạn khai báo trong file report
                rdsTaiSan.Name = "datasetTSCN"; 
                rdsTaiSan.Value = dt;

                // Xóa dữ liệu của báo cáo cũ và thêm dữ liệu mới
                rpvReport.LocalReport.DataSources.Clear();
                rpvReport.LocalReport.DataSources.Add(rdsTaiSan);

                // Refresh lại báo cáo 
                rpvReport.RefreshReport();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu làm nguồn cho Report!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                this.Close();
            }
        }

        private void FormReportTSCN_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            formQLTS_CN.Show();
        }
    }
}
