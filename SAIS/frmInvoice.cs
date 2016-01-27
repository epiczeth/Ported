using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace SAIS
{
    public partial class frmInvoice : Form
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader rdr;
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";

        public frmInvoice()
        {
            InitializeComponent();
        }
        private void auto()
        {
            txtInvoiceNo.Text = "INV-" + GetUniqueKey(8);
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

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerID.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาเลือกรหัสลูกค้า", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustomerID.Focus();
                    return;
                }

                if (txtTaxPer.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาระบุจำนวนภาษี", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTaxPer.Focus();
                    return;
                }

                if (txtTotalPayment.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาระบุจำนวนราคารวม", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalPayment.Focus();
                    return;
                }
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("ไม่มีสินค้า", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                auto();
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select invoiceno from Sales where invoiceno=@find";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@find", SqlDbType.VarChar, 20, "invoiceno"));
                cmd.Parameters["@find"].Value = txtInvoiceNo.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.Read() == true)
                {
                    MessageBox.Show("รหัสการสั่งซื้อนี้มีอยู่แล้ว", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }

                con = new SqlConnection(cs);
                con.Open();

                var cb = "insert Into Sales(InvoiceNo,InvoiceDate,CustomerID,SubTotal,VATPercentage,VATAmount,GrandTotal,TotalPayment,PaymentDue,Remarks) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j)";
                cmd = new SqlCommand(cb);
                cmd.Parameters.AddWithValue("@a", txtInvoiceNo.Text);
                cmd.Parameters.AddWithValue("@b", dtpInvoiceDate.Value);
                cmd.Parameters.AddWithValue("@c", txtCustomerID.Text);
                cmd.Parameters.AddWithValue("@d", txtSubTotal.Text);
                cmd.Parameters.AddWithValue("@e", Convert.ToInt32(txtTaxPer.Text));
                cmd.Parameters.AddWithValue("@f", txtTaxAmt.Text);
                cmd.Parameters.AddWithValue("@g", txtTotal.Text);
                cmd.Parameters.AddWithValue("@h", txtTotalPayment.Text);
                cmd.Parameters.AddWithValue("@i", txtPaymentDue.Text);
                cmd.Parameters.AddWithValue("@j", txtRemarks.Text);
                cmd.Connection = con;
                cmd.ExecuteReader();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Close();


                for (var i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs);

                    var cd = "insert Into ProductSold(InvoiceNo,ConfigID,Quantity,Price,TotalAmount) VALUES (@InvoiceNo,@ConfigID,@Quantity,@Price,@Totalamount)";
                    cmd = new SqlCommand(cd);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("InvoiceNo", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("ConfigID", ListView1.Items[i].SubItems[1].Text);
                    cmd.Parameters.AddWithValue("Quantity", ListView1.Items[i].SubItems[4].Text);
                    cmd.Parameters.AddWithValue("Price", ListView1.Items[i].SubItems[3].Text);
                    cmd.Parameters.AddWithValue("TotalAmount", ListView1.Items[i].SubItems[5].Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (var i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs);
                    con.Open();
                    var cb1 = "update stock set Quantity = Quantity - " + ListView1.Items[i].SubItems[4].Text + " where ConfigID= " + ListView1.Items[i].SubItems[1].Text + string.Empty;
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (var i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs);
                    con.Open();

                    var cb2 = "update stock set TotalPrice = Totalprice - '" + ListView1.Items[i].SubItems[5].Text + "' where ConfigID= " + ListView1.Items[i].SubItems[1].Text + string.Empty;
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();
                }

                Save.Enabled = false;
                btnPrint.Enabled = true;
                GetData();
                MessageBox.Show("บันทึกข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmInvoice_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmCustomersRecord1();
            frm.label1.Text = label6.Text;
            frm.Visible = true;
        }


        private void txtSaleQty_TextChanged(object sender, EventArgs e)
        {
            var val1 = 0;
            var val2 = 0;
            int.TryParse(txtPrice.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            var I = (val1 * val2);
            txtTotalAmount.Text = I.ToString();
        }

        public double subtot()
        {
            var i = 0;
            var j = 0;
            var k = 0;
            i = 0;
            j = 0;
            k = 0;


            try
            {
                j = ListView1.Items.Count;
                for (i = 0; i <= j - 1; i++)
                {
                    k = k + Convert.ToInt32(ListView1.Items[i].SubItems[5].Text);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return k;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerID.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาเลือกรหัสลูกค้า", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustomerID.Focus();
                    return;
                }

                if (txtProductName.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาเลือกชื่อสินค้า", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtSaleQty.Text == string.Empty)
                {
                    MessageBox.Show("กรุณาระบุจำนวนขาย", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }
                var SaleQty = Convert.ToInt32(txtSaleQty.Text);
                if (SaleQty == 0)
                {
                    MessageBox.Show("กรุณาระบุจำนวนขายต้องมากกว่า 0", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }

                if (ListView1.Items.Count == 0)
                {
                    var lst = new ListViewItem();
                    lst.SubItems.Add(txtConfigID.Text);
                    lst.SubItems.Add(txtProductName.Text);
                    lst.SubItems.Add(txtPrice.Text);
                    lst.SubItems.Add(txtSaleQty.Text);
                    lst.SubItems.Add(txtTotalAmount.Text);
                    ListView1.Items.Add(lst);
                    txtSubTotal.Text = subtot().ToString();
                    if (txtTaxPer.Text != string.Empty)
                    {
                        txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                        txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                    }
                    var val1 = 0;
                    var val2 = 0;
                    int.TryParse(txtTotal.Text, out val1);
                    int.TryParse(txtTotalPayment.Text, out val2);
                    var I = (val1 - val2);
                    txtPaymentDue.Text = I.ToString();
                    txtProductName.Text = string.Empty;
                    txtConfigID.Text = string.Empty;
                    txtPrice.Text = string.Empty;
                    txtAvailableQty.Text = string.Empty;
                    txtSaleQty.Text = string.Empty;
                    txtTotalAmount.Text = string.Empty;
                    textBox1.Text = string.Empty;
                    return;
                }

                for (var j = 0; j <= ListView1.Items.Count - 1; j++)
                {
                    if (ListView1.Items[j].SubItems[1].Text == txtConfigID.Text)
                    {
                        ListView1.Items[j].SubItems[1].Text = txtConfigID.Text;
                        ListView1.Items[j].SubItems[2].Text = txtProductName.Text;
                        ListView1.Items[j].SubItems[3].Text = txtPrice.Text;
                        ListView1.Items[j].SubItems[4].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[4].Text) + Convert.ToInt32(txtSaleQty.Text)).ToString();
                        ListView1.Items[j].SubItems[5].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[5].Text) + Convert.ToInt32(txtTotalAmount.Text)).ToString();
                        txtSubTotal.Text = subtot().ToString();
                        if (txtTaxPer.Text != string.Empty)
                        {
                            txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                            txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                        }
                        var val1 = 0;
                        var val2 = 0;
                        int.TryParse(txtTotal.Text, out val1);
                        int.TryParse(txtTotalPayment.Text, out val2);
                        var I = (val1 - val2);
                        txtPaymentDue.Text = I.ToString();
                        txtProductName.Text = string.Empty;
                        txtConfigID.Text = string.Empty;
                        txtPrice.Text = string.Empty;
                        txtAvailableQty.Text = string.Empty;
                        txtSaleQty.Text = string.Empty;
                        txtTotalAmount.Text = string.Empty;
                        return;
                    }
                }

                var lst1 = new ListViewItem();

                lst1.SubItems.Add(txtConfigID.Text);
                lst1.SubItems.Add(txtProductName.Text);
                lst1.SubItems.Add(txtPrice.Text);
                lst1.SubItems.Add(txtSaleQty.Text);
                lst1.SubItems.Add(txtTotalAmount.Text);
                ListView1.Items.Add(lst1);
                txtSubTotal.Text = subtot().ToString();
                if (txtTaxPer.Text != string.Empty)
                {
                    txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                    txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                }
                var val3 = 0;
                var val4 = 0;
                int.TryParse(txtTotal.Text, out val3);
                int.TryParse(txtTotalPayment.Text, out val4);
                var I1 = (val3 - val4);
                txtPaymentDue.Text = I1.ToString();
                txtProductName.Text = string.Empty;
                txtConfigID.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtAvailableQty.Text = string.Empty;
                txtSaleQty.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("ไม่มีข้อมูลให้ลบ", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var itmCnt = 0;
                    var i = 0;
                    var t = 0;

                    ListView1.FocusedItem.Remove();
                    itmCnt = ListView1.Items.Count;
                    t = 1;

                    for (i = 1; i <= itmCnt + 1; i++)
                    {
                        t = t + 1;
                    }
                    txtSubTotal.Text = subtot().ToString();
                    if (txtTaxPer.Text != string.Empty)
                    {
                        txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                        txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                    }
                    var val1 = 0;
                    var val2 = 0;
                    int.TryParse(txtTotal.Text, out val1);
                    int.TryParse(txtTotalPayment.Text, out val2);
                    var I = (val1 - val2);
                    txtPaymentDue.Text = I.ToString();
                }

                btnRemove.Enabled = false;
                if (ListView1.Items.Count == 0)
                {
                    txtSubTotal.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTaxPer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTaxPer.Text))
                {
                    txtTaxAmt.Text = string.Empty;
                    txtTotal.Text = string.Empty;
                    return;
                }
                txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString() ;
                txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                txtTotalPayment.Text = txtTotal.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var sql = "SELECT StockID,Config.ConfigID,ProductName,Features,Price,sum(Quantity) from Stock,Config where Stock.ConfigID=Config.ConfigID and Productname like '" + textBox1.Text + "%' group by StockID,productname,Price,Features,Config.ConfigID having sum(quantity > 0) order by ProductName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var strRowNumber = (e.RowIndex + 1).ToString();
            var size = e.Graphics.MeasureString(strRowNumber, Font);
            if (dataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                dataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            var b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var dr = dataGridView1.SelectedRows[0];
                txtConfigID.Text = dr.Cells[1].Value.ToString();
                txtProductName.Text = dr.Cells[2].Value.ToString();
                txtPrice.Text = dr.Cells[4].Value.ToString();
                txtAvailableQty.Text = dr.Cells[5].Value.ToString();
                txtSaleQty.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var sql = "SELECT StockID,Config.ConfigID,ProductName,Features,Price,sum(Quantity) from Stock,Config where Stock.ConfigID=Config.ConfigID group by StockID,productname,Price,Features,Config.ConfigID having (sum(quantity) > 0) order by ProductName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            txtInvoiceNo.Text = string.Empty;
            dtpInvoiceDate.Text = DateTime.Today.ToString();
            txtCustomerID.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtConfigID.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtAvailableQty.Text = string.Empty;
            txtSaleQty.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
            ListView1.Items.Clear();
            txtSubTotal.Text = string.Empty;
            txtTaxPer.Text = string.Empty;
            txtTaxAmt.Text = string.Empty;
            txtTotal.Text = string.Empty;
            txtTotalPayment.Text = string.Empty;
            txtPaymentDue.Text = string.Empty;
            textBox1.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            Save.Enabled = true;
            Delete.Enabled = false;
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;
            btnPrint.Enabled = false;
            ListView1.Enabled = true;
            Button7.Enabled = true;
        }

        private void NewRecord_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Delete_Click(object sender, EventArgs e)
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
                var cq1 = "delete from productSold where InvoiceNo='" + txtInvoiceNo.Text + "'";
                cmd = new SqlCommand(cq1);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                con = new SqlConnection(cs);
                con.Open();
                var cq = "delete from Sales where InvoiceNo='" + txtInvoiceNo.Text + "'";
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

        private void frmInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            var frm = new frmMainMenu();
            frm.lblUser.Text = label6.Text;
            frm.Show();
        }

        private void txtTotalPayment_TextChanged(object sender, EventArgs e)
        {
            var val1 = 0;
            var val2 = 0;
            int.TryParse(txtTotal.Text, out val1);
            int.TryParse(txtTotalPayment.Text, out val2);
            var I = (val1 - val2);
            txtPaymentDue.Text = I.ToString();
        }

        private void txtTotalPayment_Validating(object sender, CancelEventArgs e)
        {
            var val1 = 0;
            var val2 = 0;
            int.TryParse(txtTotal.Text, out val1);
            int.TryParse(txtTotalPayment.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("ราคารวมต้องไม่มากกว่าราคารวมทั้งหมด", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTotalPayment.Text = string.Empty;
                txtPaymentDue.Text = string.Empty;
                txtTotalPayment.Focus();
                return;
            }
        }

        private void txtSaleQty_Validating(object sender, CancelEventArgs e)
        {
            var val1 = 0;
            var val2 = 0;
            int.TryParse(txtAvailableQty.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("จำนวนขายมีมากว่าจำนวนคงคลังไม่ได้", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSaleQty.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
                txtSaleQty.Focus();
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;

                var rpt = new rptInvoice();

                cmd = new SqlCommand();
                var myDA = new SqlDataAdapter();
                var myDS = new DataSet();

                con = new SqlConnection(cs);
                cmd.Connection = con;
                cmd.CommandText = "SELECT Config.ConfigID, Config.ProductName, Config.Features, Config.Price, Sales.InvoiceNo, Sales.InvoiceDate, Sales.CustomerID, Sales.SubTotal,Sales.VATPercentage, Sales.VATAmount, Sales.GrandTotal, Sales.TotalPayment, Sales.PaymentDue, Sales.Remarks, ProductSold.ID,ProductSold.InvoiceNo AS Expr1, ProductSold.ConfigID AS Expr2, ProductSold.Quantity, ProductSold.Price AS Expr3, ProductSold.TotalAmount,Customer.CustomerID AS Expr4, Customer.CustomerName, Customer.Address, Customer.Landmark, Customer.City, Customer.State, Customer.ZipCode,Customer.Phone, Customer.MobileNo, Customer.FaxNo, Customer.Email, Customer.Notes FROM (((Customer INNER JOIN Sales ON Customer.CustomerID = Sales.CustomerID) INNER JOIN ProductSold ON Sales.InvoiceNo = ProductSold.InvoiceNo) INNER JOIN Config ON ProductSold.ConfigID = Config.ConfigID) where Sales.invoiceNo='" + txtInvoiceNo.Text + "'";
                cmd.CommandType = CommandType.Text;
                myDA.SelectCommand = cmd;
                myDA.Fill(myDS, "Config");
                myDA.Fill(myDS, "Sales");
                myDA.Fill(myDS, "ProductSold");
                myDA.Fill(myDS, "Customer");
                rpt.SetDataSource(myDS);
                var frm = new frmInvoiceReport();
                frm.crystalReportViewer1.ReportSource = rpt;
                frm.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            timer1.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var cb = "update Sales set GrandTotal= " + txtTotal.Text + ",TotalPayment= " + txtTotalPayment.Text + ",PaymentDue= " + txtPaymentDue.Text + ",Remarks='" + txtRemarks.Text + "' where Invoiceno= '" + txtInvoiceNo.Text + "'";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                for (var i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs);
                    var cd = "update ProductSold set Quantity=" + ListView1.Items[i].SubItems[4].Text + ",Price= " + ListView1.Items[i].SubItems[3].Text + ",TotalAmount= " + ListView1.Items[i].SubItems[5].Text + " where InvoiceNo='" + txtInvoiceNo.Text + "' and ConfigID= " + ListView1.Items[i].SubItems[1].Text + string.Empty;
                    cmd = new SqlCommand(cd);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (var i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs);
                    con.Open();
                    var cb1 = "update stock set Quantity = Quantity - " + ListView1.Items[i].SubItems[4].Text + " where ConfigID= " + ListView1.Items[i].SubItems[1].Text + string.Empty;
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (var i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs);
                    con.Open();

                    var cb2 = "update stock set TotalPrice = Totalprice - '" + ListView1.Items[i].SubItems[5].Text + "' where ConfigID= " + ListView1.Items[i].SubItems[1].Text + string.Empty;
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();
                }
                GetData();
                btnUpdate.Enabled = false;
                MessageBox.Show("ปรับปรุงข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmSalesRecord1();
            frm.DataGridView1.DataSource = null;
            frm.dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            frm.dtpInvoiceDateTo.Text = DateTime.Today.ToString();
            frm.GroupBox3.Visible = false;
            frm.DataGridView3.DataSource = null;
            frm.cmbCustomerName.Text = string.Empty;
            frm.GroupBox4.Visible = false;
            frm.DateTimePicker1.Text = DateTime.Today.ToString();
            frm.DateTimePicker2.Text = DateTime.Today.ToString();
            frm.DataGridView2.DataSource = null;
            frm.GroupBox10.Visible = false;
            frm.FillCombo();
            frm.label9.Text = label6.Text;
            frm.Show();
        }

        private void txtSaleQty_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTotalPayment_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTaxPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
