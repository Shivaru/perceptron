using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetPatterns;

namespace Timer2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NetPatterns.Timer t = new NetPatterns.Timer();
            Form1 f1 = new Form1(t);
            Form2 f2 = new Form2();
            t.TimeChangedEvent += f1.TimerChangedHandler;
            t.TimeChangedEvent += f2.TimerChangedHandler;
            f2.Show();
            Application.Run(f1);
        }
    }
}