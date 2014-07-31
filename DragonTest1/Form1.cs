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
using System.Text.RegularExpressions;

namespace DragonTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RegexDictionary = new Dictionary<string, string>(3);
            RegexDictionary.Add(@"\*\*\*", "blank");
            RegexDictionary.Add(@"\[.+\]", "choice");
            RegexDictionary.Add(@"\{\*.+\*\}", "comment");
        }

        Dictionary<string, string> RegexDictionary;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        //comment

        //string placeholder = "***";

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
                //nextToolStripMenuItem_Click(sender, e);
            }
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int cursPos = textBox1.SelectionStart;
            Tuple<Match, string> result = regexFind(textBox1.Text, RegexDictionary, cursPos);
            if (result.Item2 == "no match")
            {
                cursPos = 0;
                result = regexFind(textBox1.Text, RegexDictionary, cursPos);
                if (result.Item2 == "no match")
                {
                    MessageBox.Show("End of template reached. \rNo placeholders detected.");
                    return;
                }
            }
            textBox1.SelectionStart = result.Item1.Index;
            textBox1.SelectionLength = result.Item1.Length;
            textBox1.ScrollToCaret();

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


        private Tuple<Match, string> regexFind(string text, Dictionary<string, string> regexDictionary, int startPosition)
        {
            Match bestMatch = Regex.Match("", "");
            int bestIndex = int.MaxValue;
            string bestType = "no match";

            foreach (string k in regexDictionary.Keys)
            {
                Match m = Regex.Match(text, k);
                while (m.Success)
                {
                    if (m.Index < bestIndex && m.Index > startPosition)
                    {
                        bestIndex = m.Index;
                        bestMatch = m;
                        bestType = regexDictionary[k];
                    }
                    m = m.NextMatch();
                }

            }

            return new Tuple<Match, string>(bestMatch, bestType);

        }

    }
}
