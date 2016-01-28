using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace SAIS
{
    public partial class frmCustomers : Form
    {
        private SqlDataReader rdr = null;
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private String cs = string.Format("Data Source={0};Initial Catalog=sais;User ID={1};Password={2};", Config.datasource, Config.userid, Config.password);

        public frmCustomers()
        {
            InitializeComponent();
        }
        private void Reset()
        {
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtFaxNo.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtLandmark.Text = string.Empty;
            txtMobileNo.Text = string.Empty;
            txtNotes.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtCustomerID.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            cmbState.Text = string.Empty;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtCustomerName.Focus();
        }
        private void auto()
        {
            txtCustomerID.Text = "C-" + GetUniqueKey(6);
        }
        public static string GetUniqueKey(int maxSize)
        {
            var chars = new char[62];
            chars = "123456789".ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        private void txtZipCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtZipCode.TextLength > 6)
            {
                MessageBox.Show("สามารถใช้ได้เพียง 6 ตัวอักษร", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtZipCode.Focus();
            }
        }

        private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (txtMobileNo.TextLength > 10)
            {
                MessageBox.Show("สามารถใช้ได้เพียง 10 ตัวอักษร", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobileNo.Focus();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุชื่อ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCustomerName.Focus();
                return;
            }

            if (txtAddress.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุที่อยู่", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return;
            }
            if (txtCity.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุเมือง", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCity.Focus();
                return;
            }
            if (cmbState.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุจังหวัด", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbState.Focus();
                return;
            }
            if (txtZipCode.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุรหัสไปรษณีย์", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtZipCode.Focus();
                return;
            }


            if (txtMobileNo.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุเบอร์โทร", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobileNo.Focus();
                return;
            }

            try
            {
                auto();
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select CustomerID from Customer where CustomerID=@find";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@find", SqlDbType.VarChar, 20, "CustomerID"));
                cmd.Parameters["@find"].Value = txtCustomerID.Text;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("รหัสลูกค้านี้มีอยู่แล้ว", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                }
                else
                {
                    con = new SqlConnection(cs);
                    con.Open();

                    var cb = "insert into Customer(CustomerID,Customername,address,landmark,city,state,zipcode,Phone,email,mobileno,faxno,notes) VALUES (@d1,@d2,@d4,@d5,@d6,@d7,@d8,@d9,@d10,@d11,@d12,@d13)";

                    cmd = new SqlCommand(cb);

                    cmd.Connection = con;

                    cmd.Parameters.Add(new SqlParameter("@d1", SqlDbType.VarChar, 20, "CustomerID"));
                    cmd.Parameters.Add(new SqlParameter("@d2", SqlDbType.VarChar, 100, "Customername"));
                    cmd.Parameters.Add(new SqlParameter("@d4", SqlDbType.VarChar, 250, "address"));
                    cmd.Parameters.Add(new SqlParameter("@d5", SqlDbType.VarChar, 250, "landmark"));

                    cmd.Parameters.Add(new SqlParameter("@d6", SqlDbType.VarChar, 50, "city"));

                    cmd.Parameters.Add(new SqlParameter("@d7", SqlDbType.VarChar, 50, "state"));

                    cmd.Parameters.Add(new SqlParameter("@d8", SqlDbType.VarChar, 10, "zipcode"));

                    cmd.Parameters.Add(new SqlParameter("@d9", SqlDbType.VarChar, 15, "phone"));

                    cmd.Parameters.Add(new SqlParameter("@d10", SqlDbType.VarChar, 150, "email"));

                    cmd.Parameters.Add(new SqlParameter("@d11", SqlDbType.VarChar, 15, "mobileno"));

                    cmd.Parameters.Add(new SqlParameter("@d12", SqlDbType.VarChar, 15, "faxno"));

                    cmd.Parameters.Add(new SqlParameter("@d13", SqlDbType.VarChar, 250, "notes"));


                    cmd.Parameters["@d1"].Value = txtCustomerID.Text;
                    cmd.Parameters["@d2"].Value = txtCustomerName.Text;
                    cmd.Parameters["@d4"].Value = txtAddress.Text;
                    cmd.Parameters["@d5"].Value = txtLandmark.Text;
                    cmd.Parameters["@d6"].Value = txtCity.Text;
                    cmd.Parameters["@d7"].Value = cmbState.Text;
                    cmd.Parameters["@d8"].Value = txtZipCode.Text;
                    cmd.Parameters["@d9"].Value = txtPhone.Text;
                    cmd.Parameters["@d10"].Value = txtEmail.Text;
                    cmd.Parameters["@d11"].Value = txtMobileNo.Text;
                    cmd.Parameters["@d12"].Value = txtFaxNo.Text;
                    cmd.Parameters["@d13"].Value = txtNotes.Text;

                    cmd.ExecuteReader();
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Customer Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void delete_records()
        {
            try
            {
                var RowsAffected = 0;
                con = new SqlConnection(cs);
                con.Open();
                var cq = "delete from Customer where CustomerID=@DELETE1;";
                cmd = new SqlCommand(cq);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@DELETE1", SqlDbType.VarChar, 20, "CustomerID"));
                cmd.Parameters["@DELETE1"].Value = txtCustomerID.Text;
                RowsAffected = cmd.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    MessageBox.Show("ลบข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("ข้อมูลดังกล่าวไม่มีอยู่จริง", "ขออภัย", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("คุณต้องการจะลบข้อมูลนี้ใช่หรือไม่?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    delete_records();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();

                var cb = "update Customer set Customername = '" + txtCustomerName.Text + "',address= '" + txtAddress.Text + "',landmark= '" + txtLandmark.Text + "',city= '" + txtCity.Text + "',state= '" + cmbState.Text + "',zipcode= '" + txtZipCode.Text + "',Phone= '" + txtPhone.Text + "',email= '" + txtEmail.Text + "',mobileno= '" + txtMobileNo.Text + "',faxno= '" + txtFaxNo.Text + "',notes= '" + txtNotes.Text + "' where CustomerID= '" + txtCustomerID.Text + "'";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                MessageBox.Show("ปรับปรุงข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFaxNo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Button2_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmCustomersRecord2();
            frm.Show();
            frm.GetData();
        }
    }
}
