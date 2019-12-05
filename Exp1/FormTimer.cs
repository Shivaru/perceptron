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
    public partial class FormTimer : Form
    {
        public FormTimer()
        {
            InitializeComponent();

            timer1.Interval = 500; // 500 миллисекунд
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;


        }

        void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
        }


        //public void TimerChangedHandler(Object sender, EventArgs e)
        //{
        //    progressBar1.Value = ((Timer)sender).Seconds;
        //}



  
    }

}


