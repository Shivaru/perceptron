using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetPatterns;

namespace Timer2
{
    public partial class Form1 : Form
    {
        NetPatterns.Timer timer;

        public Form1(NetPatterns.Timer t)
        {
            InitializeComponent();
            timer = t;
        }

        public void TimerChangedHandler(Object sender, EventArgs e)
        {
            label1.Text = timer.Seconds.ToString();
            label1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Tick();
        }

    }
}
