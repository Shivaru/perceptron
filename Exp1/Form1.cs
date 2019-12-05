using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace Exp1
{
    public partial class Form1 : Form
    {
        public ArrayList YChart1 = new ArrayList();
        //double[] YChart1 = new double[];
        public double[] errors;
        
        public Form1(double[] arr1)
        {
            foreach (var item in arr1)
            {
                YChart1.Add(item);
            }

            //YChart1 = arr1;
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

            int count1 = YChart1.Count;
            for (int i = 0; i < count1; i++)
            {
                double Y = Convert.ToDouble(YChart1[i]);
                this.chart1.Series["Error"].Points.AddY(Y);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // данная строка кода позволяет загрузить данные в таблицу "bxGDataSet2.Vvod". При необходимости она может быть перемещена или удалена.
            //this.vvodTableAdapter.Fill(this.bxGDataSet2.Vvod);
            // данная строка кода позволяет загрузить данные в таблицу "bxGDataSet1.Error". При необходимости она может быть перемещена или удалена.
            //this.errorTableAdapter.Fill(this.bxGDataSet1.Error);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            //errorTableAdapter.Update(bxGDataSet1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //vvodTableAdapter.Update(bxGDataSet2);
        }


    }
}

