using System;
using System.Collections;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComPort;

namespace ComPorttest
{
    [TestClass]
    public class ComPortParcetest
    {
        [TestMethod]
        public void Parcetest()
        {
            string first =  "#A-C=-10.24,12.29,244.74, #M-C=-86.50,-64.00,64.00, #G-C=-42.00,59.00,-3.00";

            //string[] first = {"#A-C=-10.24,12.29,244.74", "#M-C=-86.50,-64.00,64.00", "#G-C=-42.00,59.00,-3.00"};
            string[] first2 = {"#A-C=-13.31,14.34,243.71", "#M-C=-85.83,-64.17,64.00", "#G-C=-45.00,60."};
            string[] first3 = { "14.34,243.71", "#M-C=-85.83,-64.17,64.00", "#G-C=-42.00,59.00,-3.00" };
            string[] first4 = { "#A-C=-13.31,14.34,243.71", "#M-C=-85.83,-64.17,64.00", "#G-C=-11.75,15.59,200.80", "#A-C=-10.24,12.29,244.74", "#M-C=-86.50,-64.00,64.00", "#G-C=-22.00,79.00,-5.00" };
            string[] first5 = { "#A-C=-13.31,14.34,243.71", "#M-C=-85.83,-64.17,64.00", "#G-C=-10.24,12.29,244.74", "#A-C=-10.24,12.29,244.74", "#M-C=-86.50,-64.00,64.00", "#G-C=-42.00,59.00" };

            //double[,] answerA = new double[1, 3] { { 10.24, 12.29, 244.74 } };
            //double[,] answerM = new double[1, 3] { { -86.50, -64.00, 64.00 } };
            //double[,] answerG = new double[1, 3] { { -42.00, 59.00, -3.00 } };

            ArrayList answerA = new ArrayList();
            answerA.Add(10.24);
            answerA.Add(12.29);
            answerA.Add(244.74);

            //ArrayList answerM = new ArrayList();
            //answerA.Add(-86.50);
            //answerA.Add(-64.00);
            //answerA.Add(64.00);

            //ArrayList answerG = new ArrayList();
            //answerA.Add(-42.00);
            //answerA.Add(59.00);
            //answerA.Add(-3.00);


            ArrayList ansA = new ArrayList();
            ArrayList ansM = new ArrayList();
            ArrayList ansG = new ArrayList();


            //double[,] answerA4 = new double[2, 3] { { -13.31, 14.34, 243.71 }, { -10.24, 12.29, 244.74 } };
            //double[,] answerM4 = new double[2, 3] { { -85.83, -64.17, 64.00 }, { -86.50, -64.00, 64.00 } };
            //double[,] answerG4 = new double[2, 3] { { -11.75, 15.59, 200.80 }, { -22.00, 79.00, -5.00 } };            

            //int answer0 = 0;

            ProgramComPort.Values Cport = new ProgramComPort.Values();            
            
            Cport.ParcePort(first, out ansA, out ansM, out ansG);
            //var metanswer = Cport.ParcePort(first);
            //var metanswer2 = Cport.ParcePort(first2);
            //var metanswer3 = Cport.ParcePort(first3);
            //var metanswer4 = Cport.ParcePort(first4);
            //var metanswer5 = Cport.ParcePort(first5);

            double a1 = Convert.ToDouble(answerA[1]);
            double a2 = Convert.ToDouble(ansA[1]);
           
            Assert.AreEqual(answerA.Count, ansA.Count);
            Assert.AreEqual(a1, a2);
            //Assert.AreEqual(answer0, metanswer2);
            //Assert.AreEqual(answer0, metanswer3);
            //Assert.AreEqual(answerA4, answerM4, answerG4, metanswer4);
            //Assert.AreEqual(answer0, metanswer5);
        }


    }
}

