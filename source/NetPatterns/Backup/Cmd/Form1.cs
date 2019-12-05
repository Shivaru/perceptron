using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetPatterns;

namespace Cmd
{
    public partial class Form1 : Form
    {
        Invoker inv;
        public Form1(Invoker i)
        {
            InitializeComponent();
            inv = i;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inv.Execute(listBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in inv.getCommandNames())
            {
                listBox1.Items.Add(s);
            }
        }

    }
}