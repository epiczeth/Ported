using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace SAIS
{
    public partial class frmSalesRecord1 : Form
    {
        private DataTable dTable;
        private SqlConnection con = null;
        private SqlDataAdapter adp;
        private DataSet ds;
        private SqlCommand cmd = null;
        private SqlDataReader rdr;
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";
        public frmSalesRecord1()
        {
            InitializeComponent();
        }

        private void frmSalesRecord_Load(object sender, EventArgs e)
        {
            FillCombo();
            TabControl1.TabPages.Remove(TabPage1);
            TabControl1.TabPages.Remove(TabPage2);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (DataGridView1.DataSource == null)
            {
                MessageBox.Show("ไม่มีข้อมูลในการสร้างไฟล์ Excel", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var rowsTotal = 0;
            var colsTotal = 0;
            var I = 0;
            var j = 0;
            var iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            var xlApp = new Excel.Application();

            try
            {
                var excelBook = xlApp.Workbooks.Add();
                var excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = DataGridView1.RowCount - 1;
                colsTotal = DataGridView1.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridView1.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = DataGridView1.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (DataGridView2.DataSource == null)
            {
                MessageBox.Show("ไม่มีข้อมูลในการสร้างไฟล์ Excel", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var rowsTotal = 0;
            var colsTotal = 0;
            var I = 0;
            var j = 0;
            var iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            var xlApp = new Excel.Application();

            try
            {
                var excelBook = xlApp.Workbooks.Add();
                var excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = DataGridView2.RowCount - 1;
                colsTotal = DataGridView2.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridView2.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = DataGridView2.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (DataGridView3.DataSource == null)
            {
                MessageBox.Show("ไม่มีข้อมูลในการสร้างไฟล์ Excel", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var rowsTotal = 0;
            var colsTotal = 0;
            var I = 0;
            var j = 0;
            var iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            var xlApp = new Excel.Application();

            try
            {
                var excelBook = xlApp.Workbooks.Add();
                var excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = DataGridView3.RowCount - 1;
                colsTotal = DataGridView3.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridView3.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = DataGridView3.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            DataGridView3.DataSource = null;
            cmbCustomerName.Text = string.Empty;
            GroupBox4.Visible = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DataGridView1.DataSource = null;
            dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            dtpInvoiceDateTo.Text = DateTime.Today.ToString();
            GroupBox3.Visible = false;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            DateTimePicker1.Text = DateTime.Today.ToString();
            DateTimePicker2.Text = DateTime.Today.ToString();
            DataGridView2.DataSource = null;
            GroupBox10.Visible = false;
        }
        public void FillCombo()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("SELECT distinct CustomerName FROM Sales,Customer where Sales.CustomerID=Customer.CustomerID", con);
                ds = new DataSet("ds");
                adp.Fill(ds);
                dTable = ds.Tables[0];
                cmbCustomerName.Items.Clear();
                foreach (DataRow drow in dTable.Rows)
                {
                    cmbCustomerName.Items.Add(drow[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                GroupBox3.Visible = true;
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT (invoiceNo) as [Invoice No],(InvoiceDate) as [Invoice Date],(Sales.CustomerID) as [Customer ID],(CustomerName) as [Customer Name],SubTotal as [SubTotal],VATPercentage as [Vat+ST %],VATAmount as [VAT+ST Amount],(GrandTotal) as [Grand Total],(TotalPayment) as [Total Payment],(PaymentDue) as [Payment Due],Remarks from Sales,Customer where Sales.CustomerID=Customer.CustomerID and InvoiceDate between #" + dtpInvoiceDateFrom.Text + "# And #" + dtpInvoiceDateTo.Text + "# order by InvoiceDate desc", con);
                var myDA = new SqlDataAdapter(cmd);
                var myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Sales");
                myDA.Fill(myDataSet, "Customer");
                DataGridView1.DataSource = myDataSet.Tables["Customer"].DefaultView;
                DataGridView1.DataSource = myDataSet.Tables["Sales"].DefaultView;
                Int64 sum = 0;
                Int64 sum1 = 0;
                Int64 sum2 = 0;

                foreach (DataGridViewRow r in DataGridView1.Rows)
                {
                    var i = Convert.ToInt64(r.Cells[7].Value);
                    var j = Convert.ToInt64(r.Cells[8].Value);
                    var k = Convert.ToInt64(r.Cells[9].Value);
                    sum = sum + i;
                    sum1 = sum1 + j;
                    sum2 = sum2 + k;
                }
                TextBox1.Text = sum.ToString();
                TextBox2.Text = sum1.ToString();
                TextBox3.Text = sum2.ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GroupBox4.Visible = true;
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT (invoiceNo) as [Invoice No],(InvoiceDate)" +
                " as [Invoice Date],(Sales.CustomerID) as [Customer ID],(CustomerName)" +
                " as [Customer Name],SubTotal as [SubTotal],VATPercentage as [Vat+ST %]," +
                "VATAmount as [VAT+ST Amount],(GrandTotal) as [Grand Total],(TotalPayment)" +
                " as [Total Payment],(PaymentDue) as [Payment Due],Remarks from Sales,Customer" +
                " where Sales.CustomerID=Customer.CustomerID and Customername='" + cmbCustomerName.Text + "'" +
                " order by CustomerName,InvoiceDate", con);
                var myDA = new SqlDataAdapter(cmd);
                var myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Sales");
                myDA.Fill(myDataSet, "Customer");
                DataGridView3.DataSource = myDataSet.Tables["Customer"].DefaultView;
                DataGridView3.DataSource = myDataSet.Tables["Sales"].DefaultView;
                Int64 sum = 0;
                Int64 sum1 = 0;
                Int64 sum2 = 0;

                var header = new string[11]
                { "รหัสสั่งซื้อ", "วันที่สั่งซื้อ", "รหัสลูกค้า", "ชื่อลูกค้า", "ราคาสุทธิ", "ภาษี", "จำนวนภาษี", "ราคารวมภาษี", "รวมเป็นเงิน", "ส่วนลด", "หมายเหตุ"
                };
                for (var i = 0; i < header.Length; i++)
                {
                    DataGridView3.Columns[i].HeaderText = header[i];
                }

                foreach (DataGridViewRow r in DataGridView3.Rows)
                {
                    var i = Convert.ToInt64(r.Cells[7].Value);
                    var j = Convert.ToInt64(r.Cells[8].Value);
                    var k = Convert.ToInt64(r.Cells[9].Value);
                    sum = sum + i;
                    sum1 = sum1 + j;
                    sum2 = sum2 + k;
                }
                TextBox6.Text = sum.ToString();
                TextBox5.Text = sum1.ToString();
                TextBox4.Text = sum2.ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                GroupBox10.Visible = true;
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT (invoiceNo) as [Invoice No],(InvoiceDate) as [Invoice Date],(Sales.CustomerID) as [Customer ID],(CustomerName) as [Customer Name],SubTotal as [SubTotal],VATPercentage as [Vat+ST %],VATAmount as [VAT+ST Amount],(GrandTotal) as [Grand Total],(TotalPayment) as [Total Payment],(PaymentDue) as [Payment Due],Remarks from Sales,Customer where Sales.CustomerID=Customer.CustomerID and InvoiceDate between #" + DateTimePicker2.Text + "# And #" + DateTimePicker1.Text + "# and PaymentDue > 0 order by InvoiceDate desc", con);
                var myDA = new SqlDataAdapter(cmd);
                var myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Sales");
                myDA.Fill(myDataSet, "Customer");
                DataGridView2.DataSource = myDataSet.Tables["Customer"].DefaultView;
                DataGridView2.DataSource = myDataSet.Tables["Sales"].DefaultView;
                Int64 sum = 0;
                Int64 sum1 = 0;
                Int64 sum2 = 0;

                foreach (DataGridViewRow r in DataGridView2.Rows)
                {
                    var i = Convert.ToInt64(r.Cells[7].Value);
                    var j = Convert.ToInt64(r.Cells[8].Value);
                    var k = Convert.ToInt64(r.Cells[9].Value);
                    sum = sum + i;
                    sum1 = sum1 + j;
                    sum2 = sum2 + k;
                }
                TextBox12.Text = sum.ToString();
                TextBox11.Text = sum1.ToString();
                TextBox10.Text = sum2.ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmSalesRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            var frm = new frmInvoice();
            frm.label6.Text = label9.Text;
            frm.Show();
        }

        private void DataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var dr = DataGridView1.SelectedRows[0];
                Hide();
                var frmSales = new frmInvoice();


                frmSales.Show();
                frmSales.txtInvoiceNo.Text = dr.Cells[0].Value.ToString();
                frmSales.dtpInvoiceDate.Text = dr.Cells[1].Value.ToString();
                frmSales.txtCustomerID.Text = dr.Cells[2].Value.ToString();
                frmSales.txtCustomerName.Text = dr.Cells[3].Value.ToString();
                frmSales.txtSubTotal.Text = dr.Cells[4].Value.ToString();
                frmSales.txtTaxPer.Text = dr.Cells[5].Value.ToString();
                frmSales.txtTaxAmt.Text = dr.Cells[6].Value.ToString();
                frmSales.txtTotal.Text = dr.Cells[7].Value.ToString();
                frmSales.txtTotalPayment.Text = dr.Cells[8].Value.ToString();
                frmSales.txtPaymentDue.Text = dr.Cells[9].Value.ToString();
                frmSales.txtRemarks.Text = dr.Cells[10].Value.ToString();
                frmSales.btnUpdate.Enabled = true;
                frmSales.Delete.Enabled = true;
                frmSales.btnPrint.Enabled = true;
                frmSales.Save.Enabled = false;
                frmSales.label6.Text = label9.Text;
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT Config.ConfigID,Config.Productname,ProductSold.Price,ProductSold.Quantity,ProductSold.TotalAmount from Sales,ProductSold,Config where Sales.InvoiceNo=ProductSold.InvoiceNo and Config.ConfigID=ProductSold.ConfigID and Sales.InvoiceNo='" + dr.Cells[0].Value.ToString() + "'", con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read() == true)
                {
                    var lst = new ListViewItem();
                    lst.SubItems.Add(rdr[0].ToString().Trim());
                    lst.SubItems.Add(rdr[1].ToString().Trim());
                    lst.SubItems.Add(rdr[2].ToString().Trim());
                    lst.SubItems.Add(rdr[3].ToString().Trim());
                    lst.SubItems.Add(rdr[4].ToString().Trim());
                    frmSales.ListView1.Items.Add(lst);
                }
                frmSales.ListView1.Enabled = false;
                frmSales.Button7.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var strRowNumber = (e.RowIndex + 1).ToString();
            var size = e.Graphics.MeasureString(strRowNumber, Font);
            if (DataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                DataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            var b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void DataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var dr = DataGridView3.SelectedRows[0];
                Hide();
                var frmSales = new frmInvoice();


                frmSales.Show();
                frmSales.txtInvoiceNo.Text = dr.Cells[0].Value.ToString();
                frmSales.dtpInvoiceDate.Text = dr.Cells[1].Value.ToString();
                frmSales.txtCustomerID.Text = dr.Cells[2].Value.ToString();
                frmSales.txtCustomerName.Text = dr.Cells[3].Value.ToString();
                frmSales.txtSubTotal.Text = dr.Cells[4].Value.ToString();
                frmSales.txtTaxPer.Text = dr.Cells[5].Value.ToString();
                frmSales.txtTaxAmt.Text = dr.Cells[6].Value.ToString();
                frmSales.txtTotal.Text = dr.Cells[7].Value.ToString();
                frmSales.txtTotalPayment.Text = dr.Cells[8].Value.ToString();
                frmSales.txtPaymentDue.Text = dr.Cells[9].Value.ToString();
                frmSales.txtRemarks.Text = dr.Cells[10].Value.ToString();
                frmSales.btnUpdate.Enabled = true;
                frmSales.Delete.Enabled = true;
                frmSales.btnPrint.Enabled = true;
                frmSales.Save.Enabled = false;
                frmSales.label6.Text = label9.Text;
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT Config.ConfigID,Config.Productname,ProductSold.Price,ProductSold.Quantity,ProductSold.TotalAmount from Sales,ProductSold,Config where Sales.InvoiceNo=ProductSold.InvoiceNo and Config.ConfigID=ProductSold.ConfigID and Sales.InvoiceNo='" + dr.Cells[0].Value.ToString() + "'" , con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var lst = new ListViewItem();
                    lst.SubItems.Add(rdr[0].ToString().Trim());
                    lst.SubItems.Add(rdr[1].ToString().Trim());
                    lst.SubItems.Add(rdr[2].ToString().Trim());
                    lst.SubItems.Add(rdr[3].ToString().Trim());
                    lst.SubItems.Add(rdr[4].ToString().Trim());
                    frmSales.ListView1.Items.Add(lst);
                }
                frmSales.ListView1.Enabled = false;
                frmSales.Button7.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var strRowNumber = (e.RowIndex + 1).ToString();
            var size = e.Graphics.MeasureString(strRowNumber, Font);
            if (DataGridView3.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                DataGridView3.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            var b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void DataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var strRowNumber = (e.RowIndex + 1).ToString();
            var size = e.Graphics.MeasureString(strRowNumber, Font);
            if (DataGridView2.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                DataGridView2.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            var b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void DataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var dr = DataGridView2.SelectedRows[0];
                Hide();
                var frmSales = new frmInvoice();


                frmSales.Show();
                frmSales.txtInvoiceNo.Text = dr.Cells[0].Value.ToString();
                frmSales.dtpInvoiceDate.Text = dr.Cells[1].Value.ToString();
                frmSales.txtCustomerID.Text = dr.Cells[2].Value.ToString();
                frmSales.txtCustomerName.Text = dr.Cells[3].Value.ToString();
                frmSales.txtSubTotal.Text = dr.Cells[4].Value.ToString();
                frmSales.txtTaxPer.Text = dr.Cells[5].Value.ToString();
                frmSales.txtTaxAmt.Text = dr.Cells[6].Value.ToString();
                frmSales.txtTotal.Text = dr.Cells[7].Value.ToString();
                frmSales.txtTotalPayment.Text = dr.Cells[8].Value.ToString();
                frmSales.txtPaymentDue.Text = dr.Cells[9].Value.ToString();
                frmSales.txtRemarks.Text = dr.Cells[10].Value.ToString();
                frmSales.btnUpdate.Enabled = true;
                frmSales.Delete.Enabled = true;
                frmSales.btnPrint.Enabled = true;
                frmSales.Save.Enabled = false;
                frmSales.label6.Text = label9.Text;
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT Config.ConfigID,Config.Productname,ProductSold.Price,ProductSold.Quantity,ProductSold.TotalAmount from Sales,ProductSold,Config where Sales.InvoiceNo=ProductSold.InvoiceNo and Config.ConfigID=ProductSold.ConfigID and Sales.InvoiceNo='" + dr.Cells[0].Value.ToString() + "'", con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var lst = new ListViewItem();
                    lst.SubItems.Add(rdr[0].ToString().Trim());
                    lst.SubItems.Add(rdr[1].ToString().Trim());
                    lst.SubItems.Add(rdr[2].ToString().Trim());
                    lst.SubItems.Add(rdr[3].ToString().Trim());
                    lst.SubItems.Add(rdr[4].ToString().Trim());
                    frmSales.ListView1.Items.Add(lst);
                }
                frmSales.ListView1.Enabled = false;
                frmSales.Button7.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TabControl1_Click(object sender, EventArgs e)
        {
            DataGridView1.DataSource = null;
            dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            dtpInvoiceDateTo.Text = DateTime.Today.ToString();
            GroupBox3.Visible = false;
            DataGridView3.DataSource = null;
            cmbCustomerName.Text = string.Empty;
            GroupBox4.Visible = false;
            DateTimePicker1.Text = DateTime.Today.ToString();
            DateTimePicker2.Text = DateTime.Today.ToString();
            DataGridView2.DataSource = null;
            GroupBox10.Visible = false;
        }
    }
}
