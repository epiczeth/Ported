using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace SAIS
{
    public partial class frmStock : Form
    {
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private SqlDataReader rdr;
        private String cs = string.Format("Data Source={0};Initial Catalog=sais;User ID={1};Password={2};", Config.datasource, Config.userid, Config.password);
        public frmStock()
        {
            InitializeComponent();
        }

        private void frmStock_Load(object sender, EventArgs e)
        {
            GetData();
        }
        public static string GetUniqueKey(int maxSize)
        {
            var chars = new char[62];
            chars =
            "123456789".ToCharArray();
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
        private void auto()
        {
            txtStockID.Text = "S-" + GetUniqueKey(6);
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT StockId as [Stock ID], (productName) as [Product Name],Features,sum(Quantity) as [Quantity],Price,sum(TotalPrice) as [Total Price] from Config,Stock where Config.ConfigID=Stock.ConfigID group by Stockid, productname,features,price having (sum(Quantity) > 0)  order by Productname", con);
                var myDA = new SqlDataAdapter(cmd);
                var myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Config");
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Config"].DefaultView;
                con.Close();
                var header = new string[]
                { "รหัส", "ชื่อสินค้า", "รายละเอียด", "จำนวน", "ราคา", "รวม"
                };
                for (var i = 0; i < header.Length; i++)
                {
                    dataGridView1.Columns[i].HeaderText = header[i];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmConfigRecord();
            frm.label1.Text = label8.Text;
            frm.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductname.Text == string.Empty)
            {
                MessageBox.Show("กรุณาเลือกชื่อสินค้า", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductname.Focus();
                return;
            }
            if (txtQty.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุจำนวน", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQty.Focus();
                return;
            }

            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select ConfigID  from stock where ConfigID=" + txtConfigID.Text + string.Empty;
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                if (rdr.Read() == true)
                {
                    MessageBox.Show("มีข้อมูลนี้อยู่แล้ว" + "\n" + "กรุณาอัพเดทฐานข้อมูล", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }
                auto();
                con = new SqlConnection(cs);
                con.Open();
                var cb = "insert into Stock(StockID,ConfigID,Quantity,Totalprice,StockDate) VALUES (@a,@b,@c,@d,@e)";

                cmd = new SqlCommand(cb);
                cmd.Parameters.AddWithValue("a", txtStockID.Text);
                cmd.Parameters.AddWithValue("b", txtConfigID.Text);
                cmd.Parameters.AddWithValue("c", txtQty.Text);
                cmd.Parameters.AddWithValue("d", txtTotalPrice.Text);
                cmd.Parameters.AddWithValue("e", dtpStockDate.Value);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("บันทึกข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                GetData();
                var frm = new frmMainMenu();
                frm.GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            var val1 = 0;
            var val2 = 0;
            int.TryParse(txtPrice.Text, out val1);
            int.TryParse(txtQty.Text, out val2);
            var I = (val1 * val2);
            txtTotalPrice.Text = I.ToString();
        }
        private void Reset()
        {
            txtPrice.Text = string.Empty;
            txtFeatures.Text = string.Empty;
            txtProductname.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtTotalPrice.Text = string.Empty;
            txtStockID.Text = string.Empty;
            dtpStockDate.Text = DateTime.Today.ToString();
            txtProduct.Text = string.Empty;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
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
                var cq = "delete from Stock where StockID='" + txtStockID.Text + "'";
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

        private void frmStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            var frm = new frmMainMenu();
            frm.lblUser.Text = label8.Text;
            frm.Show();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmStockRecord1();
            frm.label1.Text = label8.Text;
            frm.Show();
            frm.GetData();
        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT StockId as [Stock ID], (productName) as [Product Name],Features,sum(Quantity) as [Quantity],Price,sum(TotalPrice) as [Total Price] from Config,Stock where Config.ConfigID=Stock.ConfigID and productname like '" + txtProduct.Text + "%' group by Stockid, productname,features,price having sum(quantity > 0)  order by Productname", con);
                var myDA = new SqlDataAdapter(cmd);
                var myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Config");
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Config"].DefaultView;
                con.Close();
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
                var cb = "Update Stock set ConfigID=@a,Quantity=@b,Totalprice=@c,StockDate=@d where StockID=@e";

                cmd = new SqlCommand(cb);
                cmd.Parameters.AddWithValue("a",  txtConfigID.Text);
                cmd.Parameters.AddWithValue("b", txtQty.Text);
                cmd.Parameters.AddWithValue("c", txtTotalPrice.Text);
                cmd.Parameters.AddWithValue("d", dtpStockDate.Value);
                cmd.Parameters.AddWithValue("e", txtStockID.Text);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("ปรับปรุงข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
                GetData();
                var frm = new frmMainMenu();
                frm.GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmStockRecord1();
            frm.label1.Text = label8.Text;
            frm.Show();
            frm.GetData();
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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
