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

//using HEOleAutLib;

namespace DragonTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RegexDictionary = new Dictionary<string, string>(3);
            RegexDictionary.Add(@"\*\*\*", "blank");
            RegexDictionary.Add(@"s\[[^\]]+\]", "choice");
            RegexDictionary.Add(@"m\[[^\]]+\]", "multichoice");
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
                        richTextBox1.Text = line;

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
            int cursPos = richTextBox1.SelectionStart + richTextBox1.SelectionLength;
            Tuple<Match, string> result = regexFind(richTextBox1.Text, RegexDictionary, cursPos);
            if (result.Item2 == "no match")
            {
                cursPos = 0;
                result = regexFind(richTextBox1.Text, RegexDictionary, cursPos);
                if (result.Item2 == "no match")
                {
                    MessageBox.Show("End of template reached. \rNo placeholders detected.");
                    return;
                }
            }
            else if (result.Item2 == "blank")
            {
                richTextBox1.SelectionStart = result.Item1.Index;
                richTextBox1.SelectionLength = result.Item1.Length;
            }
            else if (result.Item2 == "choice")
            {
                //MessageBox.Show("select things here");

                richTextBox1.SelectionStart = result.Item1.Index;
                richTextBox1.SelectionLength = result.Item1.Length;

                

                string[] choices = result.Item1.Value.Substring(Math.Min(2,result.Item1.Value.Length ) , Math.Max(result.Item1.Value.Length -3,0)).Split(new Char[] {'|'});
                SelectionForm sf = new SelectionForm(choices);
                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    richTextBox1.SelectedText = sf.selection;

                    if (sf.selection.Contains("***"))
                    {
                        richTextBox1.SelectionStart = Math.Max(result.Item1.Index - 1, 0);
                        nextToolStripMenuItem_Click(sender, e);
                    }
                }
            }
            else if (result.Item2 == "multichoice")
            {
                richTextBox1.SelectionStart = result.Item1.Index;
                richTextBox1.SelectionLength = result.Item1.Length;

                
                string[] choices = result.Item1.Value.Substring(Math.Min(2,result.Item1.Value.Length ) , Math.Max(result.Item1.Value.Length -3,0)).Split(new Char[] {'|'});
                MultiSelectForm msf = new MultiSelectForm(choices);
     

                if (msf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string output;
                    if (msf.selections.Count > 0) 
                    {
                        output = msf.selections.OfType<string>().Aggregate((a, b) => a + "\n" + b);
                    }
                    else
                    {
                        output = "";
                    }


                    richTextBox1.SelectedText = output;

                    if (output.Contains("***"))
                    {
                        richTextBox1.SelectionStart = Math.Max(result.Item1.Index - 1, 0);
                        nextToolStripMenuItem_Click(sender, e);
                    }
                }
            
            }

            richTextBox1.ScrollToCaret();
  

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
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
            if (e.KeyData.ToString() == "F4")
            {
                scanReqToolStripMenuItem_Click(sender, e);
            }

        }

        private void copyToCernerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Console.CapsLock && MessageBox.Show("CapsLock is on.  Proceed?", "CapsLock Alert!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            var prc = Process.GetProcessesByName("hostex32");

            if (prc.Length > 0)
            {
                //MessageBox.Show("found something");
                SetForegroundWindow(prc[0].MainWindowHandle);
                string[] lines = cleanString(richTextBox1.Text).Split(new char[] {'\n'});
                foreach(string line in lines)
                {
                    //SendKeys.Send(line.Trim());
                    SendKeys.Send(line);
                    SendKeys.Send("{ENTER}");

                }

                //SendKeys.Send(richTextBox1.Text);
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

        private string cleanString(string text)
        {
            //text = text.Trim();
            text = Regex.Replace(text, @"{[^\{]+\}\n", "effeleven");
            text = Regex.Replace(text, @"{[^\{]+\}", "effeleven");
            text = text.Replace("effeleven", "{F11}");
            if (text.Substring(0, 5) == "{F11}")
            {
                text = text.Substring(5);
            }
            text = text.Replace("+", "{+}");
            text = text.Replace("^", "{^}");
            text = text.Replace("%", "{%}");
            text = text.Replace("~", "{~}");
            text = text.Replace("(", "{(}");
            text = text.Replace(")", "{)}");
            text = text.Replace("[", "{[}");
            text = text.Replace("]", "{]}");
            return text;
        }

        private void scanReqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanForm sf = new ScanForm();
            sf.Text = "Scan Requisition";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var prc = Process.GetProcessesByName("hostex32");

                if (prc.Length > 0)
                {
                    //MessageBox.Show("found something");
                    SetForegroundWindow(prc[0].MainWindowHandle);

                    SendKeys.Send("{MULTIPLY}");
                    SendKeys.Send("{MULTIPLY}");
                    SendKeys.Send("ATR{ENTER}");
                    SendKeys.Send("SPF{ENTER}");
                    SendKeys.Send("S{ENTER}");
                    SendKeys.Send("{UP}");
                    SendKeys.Send(sf.ScannedMRN);
                    SendKeys.Send("{ENTER}");


                    HEOleAutLib.HostExSessions ss = new HEOleAutLib.HostExSessions();
                    HEOleAutLib.HostExSession s = ss.Item(0);
                    HEOleAutLib.HostExScreen sc = s.Screen;
                    System.Threading.Thread.Sleep(300);
                    System.Speech.Synthesis.SpeechSynthesizer speeker = new System.Speech.Synthesis.SpeechSynthesizer();
                    speeker.SpeakAsync(sc.GetString(5, 53, 27));

                    SendKeys.Send("1{ENTER}");
                    
                }
                else
                {
                    MessageBox.Show("Could not find Cerner");
                }
            }
            
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
            }
        }

    }
}
