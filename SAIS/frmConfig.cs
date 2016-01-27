using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace SAIS
{
    public partial class frmConfig : Form
    {
        private SqlDataReader rdr = null;
        private SqlConnection con = null;
        private SqlCommand cmd = null;

        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";
        public frmConfig()
        {
            InitializeComponent();
        }
        public void FillCombo()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select RTRIM(Productname) from product order by ProductName";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmbProductName.Items.Add(rdr[0]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmConfig_Load(object sender, EventArgs e)
        {
            FillCombo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbProductName.Text == string.Empty)
            {
                MessageBox.Show("กรุณาเลือกสินค้า", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbProductName.Focus();
                return;
            }
            if (txtPrice.Text == string.Empty)
            {
                MessageBox.Show("กรุณารุบะราคา", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrice.Focus();
                return;
            }

            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var cb = string.Format("insert into Config(ProductName,Features,Price,Picture) VALUES ('{0}','{1}',{2},@d1)", cmbProductName.Text, txtFeatures.Text, txtPrice.Text);
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                var ms = new MemoryStream();
                var bmpImage = new Bitmap(pictureBox1.Image);
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var data = ms.GetBuffer();
                var p = new SqlParameter("@d1", SqlDbType.VarBinary);
                p.Value = data;
                cmd.Parameters.Add(p);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("บันทึกข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            txtPrice.Text = string.Empty;
            txtFeatures.Text = string.Empty;
            cmbProductName.Text = string.Empty;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
            pictureBox1.Image = null;
            pictureBox1.Image = Properties.Resources._12;
            cmbProductName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("คุณต้องการลบข้อมูลนี้จริงหรือไม่?", "ยืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
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
                var cq = "delete from Config where ConfigID=" + txtConfigID.Text + string.Empty;
                cmd = new SqlCommand(cq);
                cmd.Connection = con;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();

                var cb = "update Config set productName='" + cmbProductName.Text + "',Features='" + txtFeatures.Text + "',Price=" + txtPrice.Text + ",Picture=@d1 where ConfigID=" + txtConfigID.Text + string.Empty;
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                var ms = new MemoryStream();
                var bmpImage = new Bitmap(pictureBox1.Image);
                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var data = ms.GetBuffer();
                var p = new SqlParameter("@d1", SqlDbType.VarBinary);
                p.Value = data;
                cmd.Parameters.Add(p);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("ปรับปรุงข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnGetData_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmConfigRecord1();
            frm.Show();
            frm.GetData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var _with1 = openFileDialog1;

                _with1.Filter = ("Image Files |*.png; *.bmp; *.jpg;*.jpeg; *.gif;");
                _with1.FilterIndex = 4;

                openFileDialog1.FileName = string.Empty;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
