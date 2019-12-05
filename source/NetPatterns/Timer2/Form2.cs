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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void TimerChangedHandler(Object sender, EventArgs e)
        {
            progressBar1.Value = ((NetPatterns.Timer)sender).Seconds;
        }
    }
}