using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace SAIS
{
    public partial class frmCategory : Form
    {
        private SqlDataReader rdr = null;
        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";


        public frmCategory()
        {
            InitializeComponent();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            Autocomplete();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text == string.Empty)
            {
                MessageBox.Show("กรุณาระบุชื่อประเภทสินค้า", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCategoryName.Focus();
                return;
            }


            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var ct = "select CategoryName from Category where CategoryName='" + txtCategoryName.Text + "'";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("Category Name Already Exists", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCategoryName.Text = string.Empty;
                    txtCategoryName.Focus();


                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }

                con = new SqlConnection(cs);
                con.Open();

                var cb = string.Format("insert into Category(CategoryName) VALUES ('{0}')", txtCategoryName.Text);

                cmd = new SqlCommand(cb) { Connection = con };
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("บันทึกข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                var cq = "delete from Category where Categoryname='" + txtCategoryName.Text + "'";
                cmd = new SqlCommand(cq);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    MessageBox.Show("ลบข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                    Autocomplete();
                }
                else
                {
                    MessageBox.Show("ข้อมูลดังกล่าวไม่มีอยู่จริง", "ขออภัย", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void Autocomplete()
        {
            try
            {
                con = new SqlConnection(cs);
                con.Open();
                var cmd = new SqlCommand("SELECT distinct Categoryname FROM Category", con);
                var ds = new DataSet();
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Category");
                var col = new AutoCompleteStringCollection();
                var i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    col.Add(ds.Tables[0].Rows[i]["Categoryname"].ToString());
                }
                txtCategoryName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtCategoryName.AutoCompleteCustomSource = col;
                txtCategoryName.AutoCompleteMode = AutoCompleteMode.Suggest;

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

                var cb = "update Category set CategoryName='" + txtCategoryName.Text + "' where Categoryname='" + textBox1.Text + "'";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("ปรับปรุงข้อมูลสำเร็จ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                btnUpdate.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            txtCategoryName.Text = string.Empty;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtCategoryName.Focus();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new frmCategoryRecord();
            frm.Show();
            frm.GetData();
        }
    }
}
