using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragonTest1
{
    public partial class SelectionForm : Form
    {
        //public SelectionForm()
        //{
        //    InitializeComponent();
        //}

        public SelectionForm(string[] choices)
        {
            InitializeComponent();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(choices);
            listBox1.SelectedIndex = 0;
        }

        public string selection = "";


        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            selection = (string) listBox1.SelectedItem;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }



        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && listBox1.SelectedIndex >= 0)
            {
                listBox1_DoubleClick(sender, e);
            }
            else
            {
                switch (e.KeyCode)
                {

                    case Keys.D1:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1 , 0);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad1:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 0);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D2:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 1);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad2:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 1);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D3:
                        
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 2);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad3:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 2);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D4:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 3);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.NumPad4:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 3);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D5:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 4);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad5:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 4);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D6:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 5);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad6:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 5);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D7:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 6);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad7:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 6);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D8:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 7);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad8:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 7);
                        listBox1_DoubleClick(sender, e);
                        break;

                    case Keys.D9:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 8);
                        listBox1_DoubleClick(sender, e);
                        break;
                    case Keys.NumPad9:
                        listBox1.SelectedIndex = Math.Min(listBox1.Items.Count - 1, 8);
                        listBox1_DoubleClick(sender, e);
                        break; 
       
                    default:
                        break;
                }
            }
        }


    }
}
