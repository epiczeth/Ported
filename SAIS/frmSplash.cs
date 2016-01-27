using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAIS
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var frm = new frmLogin();
            progressBar1.Visible = true;

            progressBar1.Value = progressBar1.Value + 2;
            if (progressBar1.Value == 10)
            {
                label3.Text = "กำลังอ่านโมดูล..";
            }
            else
            {
                if (progressBar1.Value == 20)
                {
                    label3.Text = "กำลังเปิดโมดูล..";
                }
                else
                {
                    if (progressBar1.Value == 40)
                    {
                        label3.Text = "กำลังเริ่มการทำงานโมดูล..";
                    }
                    else
                    {
                        if (progressBar1.Value == 60)
                        {
                            label3.Text = "กำลังโหลดโมดูล..";
                        }
                        else
                        {
                            if (progressBar1.Value == 80)
                            {
                                label3.Text = "โหลดโมดูลสำเร็จ.";
                            }
                            else
                            {
                                if (progressBar1.Value == 100)
                                {
                                    frm.Show();
                                    timer1.Enabled = false;
                                    Hide();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            progressBar1.Width = Width;
        }
    }
}
