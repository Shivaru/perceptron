using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exp1;


namespace Exp1Test
{
    [TestClass]
    public class NeyronTest
    {
        [TestMethod]
        public void FuncTest()
        {
            double first = 0.7; //задать расчетные значения
            double answer = 0.668187772168166; //задать расчетные значения
                 
            Neyron ntest = new Neyron();
            var metanswer = ntest.Func(first);
       
            Assert.AreEqual(answer,metanswer);   
        }
        
        [TestMethod]
        public void dFuncTest()
        {
            double first = 0.7; //задать расчетные значения
            double answer = 0.221712873293109; //задать расчетные значения

            Neyron ntest1 = new Neyron();
            var metanswer = ntest1.dFunc(first);

            Assert.AreEqual(answer, metanswer); 

        }


    }
}
