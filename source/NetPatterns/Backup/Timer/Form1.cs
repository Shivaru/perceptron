using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetPatterns;

namespace Timer
{
    public partial class Form1 : Form, IObserver 
    {
        ConcreteSubject timer;

        public Form1(ConcreteSubject s)
        {
            InitializeComponent();
            timer = s;
        }

        public void Update()
        {
            label1.Text = timer.SubjectState.ToString();
            label1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Timer();
        }
    }
}