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
    public partial class frmStockRecord1 : Form
    {
        private SqlDataReader rdr = null;
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";
        public frmStockRecord1()
        {
            InitializeComponent();
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var sql = "SELECT StockID,Config.ConfigID,ProductName,Features,Price,Quantity,Totalprice,Stockdate from Stock,Config where Stock.ConfigID=Config.ConfigID order by ProductName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6], rdr[7]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmStockRecord_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void txtProductname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var sql = "SELECT StockID,Config.ConfigID,ProductName,Features,Price,Quantity,Totalprice,Stockdate from Stock,Config where Stock.ConfigID=Config.ConfigID and productname like '" + txtProductname.Text + "%' order by ProductName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6], rdr[7]);
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
                Hide();
                var frm = new frmStock();
                frm.Show();
                frm.txtStockID.Text = dr.Cells[0].Value.ToString();
                frm.txtConfigID.Text = dr.Cells[1].Value.ToString();
                frm.txtProductname.Text = dr.Cells[2].Value.ToString();
                frm.txtFeatures.Text = dr.Cells[3].Value.ToString();
                frm.txtPrice.Text = dr.Cells[4].Value.ToString();
                frm.txtQty.Text = dr.Cells[5].Value.ToString();
                frm.txtTotalPrice.Text = dr.Cells[6].Value.ToString();
                frm.dtpStockDate.Text = dr.Cells[7].Value.ToString();
                frm.btnUpdate.Enabled = true;
                frm.btnDelete.Enabled = true;
                frm.btnSave.Enabled = false;
                frm.label8.Text = label1.Text;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var sql = "SELECT StockID,Config.ConfigID,ProductName,Features,Price,Quantity,Totalprice,Stockdate from Stock,Config where Stock.ConfigID=Config.ConfigID and Quantity <= 0 order by ProductName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6], rdr[7]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmStockRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            var frm = new frmStock();
            frm.label8.Text = label1.Text;
            frm.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
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

                rowsTotal = dataGridView1.RowCount - 1;
                colsTotal = dataGridView1.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGridView1.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = dataGridView1.Rows[I].Cells[j].Value;
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

        private void button2_Click(object sender, EventArgs e)
        {
            txtProductname.Text = string.Empty;
            GetData();
        }
    }
}
