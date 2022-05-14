using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Apollo_Justice_Script_Editor
{
    public partial class Form1 : Form
    {
        MessageDrawer md;
        Graphics gr;
        string filePath;
        bool loaded = false;
        int numoffound = 0;
        bool infinding = false;
        int[] fs = null;
        public Form1()
        {
            InitializeComponent();
            textBox1.Click += On_Click;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            textBox1.Enabled = false;
            groupBox1.Enabled = false;
        }

        public void Open()
        {
            string scpath = "";
            loaded = false;
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) //Getting file's path
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open script file";
                openFileDialog.Filter = "Text file (*.txt)|*.txt|Any file (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                DialogResult dr = openFileDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    scpath = openFileDialog.FileName;
                }
                else if (dr == DialogResult.Cancel)
                {
                    loaded = true;
                    return;
                }
            }
            textBox1.Lines = File.ReadAllLines(scpath);
            md = new MessageDrawer();
            gr = pictureBox1.CreateGraphics();
            loaded = true;
            filePath = scpath;
            saveToolStripMenuItem.Enabled = true; //updating all UI elements
            saveAsToolStripMenuItem.Enabled = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Enabled = true;
            groupBox1.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void On_Click(object sender, EventArgs e)
        {
            if (!loaded)
                return;
            new Thread(new ThreadStart(ShowMessage)).Start();
        }

        async void ShowMessage()
        {
            string dial = ScriptHelper.GetMessage(textBox1.Lines, textBox1.SelectionStart);
            if (dial == null)
                return;
            short[][] shorts = ScriptHelper.GetMessageShorts(dial);
            if (shorts == null)
                return;
            md.DrawMessage(shorts, gr);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!loaded)
                return;
            if (infinding)
            {
                ResetFinding();
            }
            new Thread(new ThreadStart(ShowMessage)).Start();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) //saving into last file
        {
            File.WriteAllLines(filePath, textBox1.Lines);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scpath = "";
            using (SaveFileDialog sfd = new SaveFileDialog()) //getting file's path
            {
                sfd.RestoreDirectory = true;
                sfd.Title = "Save script file as";
                sfd.Filter = "Text file (*.txt)|*.txt|Any file (*.*)|*.*";
                sfd.FilterIndex = 1;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    scpath = sfd.FileName;
                }
                else if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            filePath = scpath;
            File.WriteAllLines(filePath, textBox1.Lines); //writing lines in file
        }

        public int[] Find(string seek) //finding text in file
        {
            infinding = true;
            List<int> sels = new List<int>();
            string[] text = textBox1.Lines;
            int len = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].ToUpper().Contains(seek.ToUpper()))
                {
                    sels.Add(len + text[i].IndexOf(seek));
                }
                len += text[i].Length + 2;
            }
            return sels.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] sels = Find(textBox2.Text);
            if (sels == null || sels.Length == 0) //if text was not found, show the message
            {
                MessageBox.Show("Matches not found");
                return;
            }
            for (int i = 0; i < sels.Length; i++)
            {
                if (sels[i] >= textBox1.SelectionStart)
                {
                    GotoFound(sels, i);
                    break;
                }
            }
            fs = sels;
            button4.Enabled = true;
        }

        void GotoFound(int[] sels, int ind) //replacing cursor to a found text
        {
            textBox1.SelectionStart = sels[ind];
            numoffound = ind;
            if (numoffound >= sels.Length - 1 && numoffound != 0)
            {
                button3.Enabled = true;
                button2.Enabled = false;
            }
            else if (numoffound >= sels.Length - 1 && numoffound == 0)
            {
                button3.Enabled = false;
                button2.Enabled = false;
            }
            else if (numoffound == 0)
            {
                button2.Enabled = true;
                button3.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            textBox1.Focus();
            textBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e) //Next button
        {
            numoffound++;
            GotoFound(fs, numoffound);
        }

        private void button3_Click(object sender, EventArgs e) //Previous button
        {
            numoffound--;
            GotoFound(fs, numoffound);
        }

        void ResetFinding() //Reseting finding
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            numoffound = 0;
            fs = null;
            infinding = false;
        }

        private void button4_Click(object sender, EventArgs e) //Replace button
        {
            int cur = -1;
            if (!fs.Contains(textBox1.SelectionStart))
            {
                button1_Click(sender, e);
            }
            else
            {
                string[] text = textBox1.Lines;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Contains(textBox2.Text))
                    {
                        cur++;
                        if (cur == numoffound)
                        {
                            int t = text[i].IndexOf(textBox2.Text);
                            string temp = text[i].Remove(t, textBox2.Text.Length);
                            text[i] = temp.Insert(t, textBox3.Text);
                            loaded = false;
                            textBox1.Lines = text;
                            loaded = true;
                            textBox1.SelectionStart = fs[numoffound];
                            button4.Enabled = false;
                            button1_Click(sender, e);
                            break;
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) //Replace all button
        {
            int selc = textBox1.SelectionStart;
            string[] text = textBox1.Lines;
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = text[i].Replace(textBox2.Text, textBox3.Text);
            }
            loaded = false;
            textBox1.Lines = text;
            loaded = true;
            textBox1.SelectionStart = selc;
            textBox1.Focus();
            textBox1.ScrollToCaret();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //updating Ui elements after writing something in find textbox
        {
            button1.Enabled = textBox2.Text != null || textBox2.Text != "";
            button5.Enabled = textBox2.Text != null || textBox2.Text != "";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox2.Text != null || textBox2.Text != "";
            button5.Enabled = textBox2.Text != null || textBox2.Text != "";
        }
    }
}
