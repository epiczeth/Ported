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
    public partial class frmConfigRecord1 : Form
    {
        private SqlDataReader rdr = null;
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";
        public frmConfigRecord1()
        {
            InitializeComponent();
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT * from Config order by Productname", con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmConfigRecord_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void txtProductname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("SELECT * from Config where productname like '" + txtProductname.Text + "%' order by Productname", con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4]);
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



        private void frmConfigRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            var frm = new frmConfig();
            frm.Show();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var dr = dataGridView1.SelectedRows[0];
                Hide();
                var obj = new frmConfig();


                obj.Show();
                obj.txtConfigID.Text = dr.Cells[0].Value.ToString();
                obj.cmbProductName.Text = dr.Cells[1].Value.ToString();
                obj.txtFeatures.Text = dr.Cells[2].Value.ToString();
                obj.txtPrice.Text = dr.Cells[3].Value.ToString();
                var data = (byte[])dr.Cells[4].Value;
                var ms = new MemoryStream(data);
                obj.pictureBox1.Image = Image.FromStream(ms);
                obj.btnUpdate.Enabled = true;
                obj.btnDelete.Enabled = true;
                obj.btnSave.Enabled = false;
                obj.cmbProductName.Focus();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var dr = dataGridView1.SelectedRows[0];
                Hide();
                var obj = new frmConfig();


                obj.Show();
                obj.txtConfigID.Text = dr.Cells[0].Value.ToString();
                obj.cmbProductName.Text = dr.Cells[1].Value.ToString();
                obj.txtFeatures.Text = dr.Cells[2].Value.ToString();
                obj.txtPrice.Text = dr.Cells[3].Value.ToString();
                var data = (byte[])dr.Cells[4].Value;
                var ms = new MemoryStream(data);
                obj.pictureBox1.Image = Image.FromStream(ms);
                obj.btnUpdate.Enabled = true;
                obj.btnDelete.Enabled = true;
                obj.btnSave.Enabled = false;
                obj.cmbProductName.Focus();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
