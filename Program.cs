using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTUD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new QLTaiSanGiaDinh());
            //Application.Run(new FormQLNguoidung());
            //Application.Run(new FormReportTSCN());
            //Application.Run(new FormQLNguoidung());
            // Giả lập Admin thuộc gia đình GD01 đang đăng nhập
            Application.Run(new FormDangNhap());
            //Application.Run(new FormAdminHome("GD01"));

        }
    }
}
