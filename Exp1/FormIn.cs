using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetPatterns;


namespace Exp1
{
    public partial class FormIn : Form
    {
        Timer timer;

        public FormIn(Timer t)
        {
            InitializeComponent();
            timer = t;
        }

        public void TimerChangedHandler(Object sender, EventArgs e)
        {
            label0.Text = timer.Seconds.ToString();
            label0.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Tick();
        }


    }


}
