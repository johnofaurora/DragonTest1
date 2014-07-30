using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DragonTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        //comment

        string placeholder = "***";

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        String line = sr.ReadToEnd();
                        textBox1.Text = line;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("File could not be read");
                    return;
                }
                nextToolStripMenuItem_Click(sender, e);
            }
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cursPos = textBox1.SelectionStart;
            int nextIndex = textBox1.Text.IndexOf(placeholder, cursPos); //if curently at a ***, won't move past
            if (nextIndex < 0) 
            {
                cursPos = 0;
                nextIndex = textBox1.Text.IndexOf(placeholder, cursPos);
                if (nextIndex < 0)
                {
                    nextIndex = 0;
                    MessageBox.Show("End of template reached. \rNo \"" + placeholder +"\" detected.");
                }
            }
            //MessageBox.Show(cursPos.ToString() + "   " + nextIndex.ToString()); 
            textBox1.SelectionStart = nextIndex;
            textBox1.SelectionLength = placeholder.Length;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyData.ToString());
            if (e.KeyData.ToString() == "F1")
            {
                loadTemplateToolStripMenuItem_Click(sender, e);
            }
            if (e.KeyData.ToString() == "F2")
            {
                nextToolStripMenuItem_Click(sender, e);
            }
            if (e.KeyData.ToString() == "F3")
            {
                copyToCernerToolStripMenuItem_Click(sender, e);
            }

        }

        private void copyToCernerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var prc = Process.GetProcessesByName("hostex32");

            if (prc.Length > 0)
            {
                //MessageBox.Show("found something");
                SetForegroundWindow(prc[0].MainWindowHandle);
                SendKeys.Send(textBox1.Text);
            }
            else
            {
                MessageBox.Show("Could not find Cerner");
            }
        }


    }
}
