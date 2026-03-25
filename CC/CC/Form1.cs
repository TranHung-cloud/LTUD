using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ApplyHoverEffectToButtons();
        }
        private void ApplyHoverEffectToButtons()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    button.UseVisualStyleBackColor = false;
                    Color defaultColor = button.BackColor;
                    button.MouseEnter += (s, e) => button.BackColor = Color.Red;
                    button.MouseLeave += (s, e) => button.BackColor = defaultColor;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
