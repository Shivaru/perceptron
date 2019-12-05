using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetPatterns;

namespace Timer
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
            ConcreteSubject timer = new ConcreteSubject();
            Form1 f1 = new Form1(timer);
            Form2 f2 = new Form2(timer);
            timer.Add(f1);
            timer.Add(f2);
            f2.Show();
            Application.Run(f1);
        }
    }
}