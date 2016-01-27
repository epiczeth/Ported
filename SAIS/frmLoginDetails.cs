using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SAIS
{
    public partial class frmLoginDetails : Form
    {
        private String cs = "Data Source=(local);Initial Catalog=sais;Integrated Security=True";
        public frmLoginDetails()
        {
            InitializeComponent();
        }

        private SqlConnection Connection
        {
            get
            {
                var ConnectionToFetch = new SqlConnection(cs);
                ConnectionToFetch.Open();
                return ConnectionToFetch;
            }
        }
        public DataView GetData()
        {
            var SelectQry = (dynamic )"SELECT (Username) as [User Name],(user_Password) as [Password] FROM users ";
            var SampleSource = new DataSet();
            DataView TableView = null;
            try
            {
                var SampleCommand = new SqlCommand();
                dynamic SampleDataAdapter = new SqlDataAdapter();
                SampleCommand.CommandText = SelectQry;
                SampleCommand.Connection = Connection;
                SampleDataAdapter.SelectCommand = SampleCommand;
                SampleDataAdapter.Fill(SampleSource);
                TableView = SampleSource.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return TableView;
        }
        private void frmLoginDetails_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetData();
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
    }
}
