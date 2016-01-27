using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SAIS
{
    public partial class frmUserRegistration : Form
    {
        private SqlDataReader rdr = null;
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";


        public frmUserRegistration()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Autocomplete();
        }
        private void Reset()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtContact_no.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail_Address.Text = string.Empty;
            btnRegister.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate_record.Enabled = false;
            txtUsername.Focus();
        }
        private void NewRecord_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุชื่อผู้ใช้", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุรหัสผ่าน", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            if (txtName.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุชื่อ", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }
            if (txtContact_no.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุเบอร์โทร.", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact_no.Focus();
                return;
            }
            if (txtEmail_Address.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุอีเมล์", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail_Address.Focus();
                return;
            }
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select Registration.UserName from Registration where Registration.UserName=@find";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@find", SqlDbType.VarChar, 30, "username"));
                cmd.Parameters["@find"].Value = txtUsername.Text;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("ไม่สามารถใช้ชื่อนี้ได้", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Text = string.Empty;
                    txtUsername.Focus();


                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }

                con = new SqlConnection(cs);
                con.Open();

                var cb = "insert into Registration(UserName,User_Password,ContactNo,Email,NameOfUser,JoiningDate) VALUES (@a,@b,@c,@d,@e,@f)";
                cmd = new SqlCommand(cb);
                cmd.Parameters.AddWithValue("a", txtUsername.Text);
                cmd.Parameters.AddWithValue("b", txtPassword.Text);
                cmd.Parameters.AddWithValue("c", txtContact_no.Text);
                cmd.Parameters.AddWithValue("d", txtEmail_Address.Text);
                cmd.Parameters.AddWithValue("e", txtName.Text);
                cmd.Parameters.AddWithValue("f", DateTime.Now);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                con = new SqlConnection(cs);
                con.Open();

                var cb1 = "insert into Users(Username,User_password) VALUES ('" + txtUsername.Text + "','" + txtPassword.Text + "')";
                cmd = new SqlCommand(cb1);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("ลงทะเบียนสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                btnRegister.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CheckAvailability_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาระบุชื่อผู้ใช้", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                    return;
                }
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select UserName from Registration where UserName='" + txtUsername.Text + "'";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("ไม่สามารถใช้ชื่อนี้ได้", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!rdr.Read())
                {
                    MessageBox.Show("สามารถใช้ชื่อนี้ได้", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.Focus();
                }
                if ((rdr != null))
                {
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Email_Address_Validating(object sender, CancelEventArgs e)
        {
            var rEMail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (txtEmail_Address.Text.Length > 0)
            {
                if (!rEMail.IsMatch(txtEmail_Address.Text))
                {
                    MessageBox.Show("รูปแบบอีเมล์ไม่ถูกต้อง", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail_Address.SelectAll();
                    e.Cancel = true;
                }
            }
        }

        private void Name_Of_User_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        private void Username_Validating(object sender, CancelEventArgs e)
        {
            var rEMail = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9_]");
            if (txtUsername.Text.Length > 0)
            {
                if (!rEMail.IsMatch(txtUsername.Text))
                {
                    MessageBox.Show("สามารถใช้ได้แค่ตัวอักษร a-z, 0-9 และ _ เท่านั้น", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.SelectAll();
                    e.Cancel = true;
                }
            }
        }

        private void GetDetails_Click(object sender, EventArgs e)
        {
            var frm = new frmRegisteredUsersDetails();
            frm.Show();
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
            btnUpdate_record.Enabled = true;
            try
            {
                txtUsername.Text = txtUsername.Text.TrimEnd();
                con = new SqlConnection(cs);

                con.Open();
                cmd = con.CreateCommand();

                cmd.CommandText = "SELECT UserName,User_Password,NameOfUser,ContactNo,Email FROM Registration WHERE UserName = '" + txtUsername.Text.Trim() + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    txtPassword.Text = (rdr.GetString(1).Trim());
                    txtName.Text = (rdr.GetString(2).Trim());
                    txtContact_no.Text = (rdr.GetString(3).Trim());
                    txtEmail_Address.Text = (rdr.GetString(4).Trim());
                }

                if ((rdr != null))
                {
                    rdr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Autocomplete()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var cmd = new SqlCommand("SELECT Registration.UserName FROM Registration", con);
                var ds = new DataSet();
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Registration");
                var col = new AutoCompleteStringCollection();
                var i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    col.Add(ds.Tables[0].Rows[i]["Username"].ToString());
                }
                txtUsername.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtUsername.AutoCompleteCustomSource = col;
                txtUsername.AutoCompleteMode = AutoCompleteMode.Suggest;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Update_record_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();

                var cb = "update Registration set User_Password='" + txtPassword.Text + "',ContactNo='" + txtContact_no.Text + "',Email='" + txtEmail_Address.Text + "',NameOfUser='" + txtName.Text + "' where UserName='" + txtUsername.Text + "'";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                con = new SqlConnection(cs);
                con.Open();
                var cb1 = "update Users set User_password='" + txtPassword.Text + "' where Username='" + txtUsername.Text + "'";
                cmd = new SqlCommand(cb1);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("ปรับปรุ่งข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                btnUpdate_record.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("คุณต้องการลบข้อมูลนี้ใช้หรือไม่?", "ลบข้อมูล", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }
        }
        private void delete_records()
        {
            try
            {
                var RowsAffected = 0;
                con = new SqlConnection(cs);
                con.Open();
                var ct = "delete from Users where Username='" + txtUsername.Text + "'";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con = new SqlConnection(cs);
                con.Open();
                var cq = "delete from Registration where Username='" + txtUsername.Text + "'";
                cmd = new SqlCommand(cq);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    MessageBox.Show("ลบข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Autocomplete();
                    Reset();
                }
                else
                {
                    MessageBox.Show("ไม่พบข้อมูลดังกล่าว", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                    Autocomplete();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtContact_no_Validating(object sender, CancelEventArgs e)
        {
            if (txtContact_no.TextLength > 10)
            {
                MessageBox.Show("สูงสุด 10 ตัวอักษรเท่านั้น", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact_no.Focus();
            }
        }

        private void txtContact_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
