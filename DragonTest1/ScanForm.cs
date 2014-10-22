using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DragonTest1
{
    public partial class ScanForm : Form
    {
        public string ScannedMRN;

        public ScanForm()
        {
            InitializeComponent();
            ScannedMRN = "";
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.ScannedMRN = this.mrnBox.Text;
            this.Close();
        }

        private void mrnBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(mrnBox.Text.Length == 7)
            {
                OkButton_Click(sender, e);
            }
            if (e.KeyCode == Keys.Enter)
            {
                OkButton_Click(sender, e);
            }
        }
    }
}
