using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;

namespace SAIS
{
    public partial class frmRecoveryPassword : Form
    {
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";

        public frmRecoveryPassword()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == string.Empty)
            {
                MessageBox.Show("ระบุอีเมล์", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                var ds = new DataSet();
                var con = new SqlConnection(cs);
                con.Open();
                var cmd = new SqlCommand("SELECT User_Password FROM Registration Where Email = '" + txtEmail.Text + "'", con);

                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var Msg = new MailMessage();

                    Msg.From = new MailAddress("abcd@gmail.com");

                    Msg.To.Add(txtEmail.Text);
                    Msg.Subject = "ข้อมูลรหัสผ่านของคุณ";
                    Msg.Body = "รหัสผ่านของคุณ: " + Convert.ToString(ds.Tables[0].Rows[0]["user_Password"]) + string.Empty;
                    Msg.IsBodyHtml = true;

                    var smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("abcd@gmail.com", "abcd");
                    smtp.EnableSsl = true;
                    smtp.Send(Msg);
                    MessageBox.Show(("รหัสผ่านได้ถูกส่งไปที่อีเมล์คุณแล้ว " + ("\r\n" + "กรุณาตรวจสอบอีเมล์ของท่าน")), "ขอบคุณ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                    var LoginForm1 = new frmLogin();
                    LoginForm1.Show();
                    LoginForm1.txtUserName.Text = string.Empty;
                    LoginForm1.txtPassword.Text = string.Empty;
                    LoginForm1.ProgressBar1.Visible = false;
                    LoginForm1.txtUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecoveryPassword_Load(object sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        private void RecoveryPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            var frm = new frmLogin();
            frm.txtUserName.Text = string.Empty;
            frm.txtPassword.Text = string.Empty;
            frm.txtUserName.Focus();
            frm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            timer1.Enabled = false;
        }
    }
}
