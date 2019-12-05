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
    public partial class Form2 : Form, IObserver
    {
        ConcreteSubject timer;

        public Form2(ConcreteSubject s)
        {
            InitializeComponent();
            timer = s;
        }

        public void Update()
        {
            progressBar1.Value = timer.SubjectState;
        }

    }
}