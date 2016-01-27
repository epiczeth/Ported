using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAIS
{
    public partial class frmInvoiceReport : Form
    {
        public frmInvoiceReport()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
