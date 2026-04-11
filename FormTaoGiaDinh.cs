using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;

namespace LTUD
{
    public partial class FormTaoGiaDinh : Form
    {
        private string maNguoiDung = "";

        public FormTaoGiaDinh(string maND)
        {
            InitializeComponent();
            maNguoiDung = maND;
            this.Load += FormTaoGiaDinh_Load;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void FormTaoGiaDinh_Load(object sender, EventArgs e)
        {
            txtMaGiaDinh.Text = TaoMaGiaDinhTuTang();
            txtMaGiaDinh.Enabled = false;
        }
        
        private string TaoMaGiaDinhTuTang()
        {
            DataTable dt = DatabaseConnection.GetData("SELECT MAGIADINH FROM GIADINH WHERE MAGIADINH LIKE 'GD%'");
            if (dt.Rows.Count == 0) return "GD01";

            int maxNumber = 0;
            foreach (DataRow row in dt.Rows)
            {
                string currentID = row["MAGIADINH"].ToString().Trim();
                string numPart = new string(currentID.SkipWhile(c => !char.IsDigit(c)).ToArray());

                if (int.TryParse(numPart, out int currentNum))
                {
                    if (currentNum > maxNumber)
                    {
                        maxNumber = currentNum;
                    }
                }
            }

            int nextNumber = maxNumber + 1;
            return "GD" + nextNumber.ToString("D2");
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (txtTenGiaDinh.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên gia đình!");
                return;
            }
            
            string maGD = txtMaGiaDinh.Text;
            string tenGD = txtTenGiaDinh.Text.Trim();
            
            try
            {
                // Thêm gia đình mới
                DatabaseConnection.ExecuteQuery($"INSERT INTO GIADINH (MAGIADINH, TENGIADINH) VALUES ('{maGD}', N'{tenGD}')");
                
                // Cập nhật người dùng thành chủ gia đình (VT03) và gán mã gia đình
                DatabaseConnection.ExecuteQuery($"UPDATE NGUOIDUNG SET MAGIADINH = '{maGD}', MAVAITRO = 'VT03' WHERE MANGUOIDUNG = '{maNguoiDung}'");
                
                MessageBox.Show("Tạo gia đình thành công. Bạn đã trở thành chủ gia đình!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo gia đình: " + ex.Message);
            }
        }
    }
}