using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetPatterns;

namespace Cmd
{
    public class AboutCmd : ICommand
    {
        public void Execute()
        {
            MessageBox.Show("Copyright (c) 2007, Hello Corp.", "About");
        }
    }

    public class HelloCmd : ICommand
    {
        public void Execute()
        {
            if (MessageBox.Show("Hello ! Are You OK ?",
                   "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                MessageBox.Show("It's fine !");
            else
                MessageBox.Show("Don't warry, be happy !");

        }
    }

   
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
            Invoker inv = new Invoker();
            inv.Add("Hello", new HelloCmd());
            inv.Add("About", new AboutCmd());
            Application.Run(new Form1(inv));
        }
    }
}