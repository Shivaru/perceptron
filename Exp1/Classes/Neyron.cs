using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;

// Функции активации
// Рассчет

namespace Exp1
{
    public class Neyron
    {
        private int console = 1;

        // Функции активации.

        //Сигмойд активации.
        //public double Func(double Sum) // TODO: изменить в методах на private
        //{
        //    double x = Sum;
        //    double Fx;
        //    Fx = 1 / (1 + Math.Exp((-1) * x));
        //    return Fx;
        //}
        //public double dFunc(double Sum) // TODO: изменить в методах на private
        //{
        //    double x = Sum;
        //    double dFx;
        //    dFx = (1 / (1 + Math.Exp((-1) * x))) * (1 - (1 / (1 + Math.Exp((-1) * x))));
        //    return dFx;
        //}

        // Гиперболический тангенс.

        //public double Func(double Sum) // TODO: изменить в методах на private
        //{
        //    double x = Sum;
        //    var Fx = 0.0;
        //    Fx = (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x));
        //    return Fx;
        //}
        //public double dFunc(double Sum) // TODO: изменить в методах на private
        //{
        //    double x = Sum;
        //    var dFx = 0.0;
        //    dFx = 4 / ((Math.Exp(x) + Math.Exp(-x)) * (Math.Exp(x) + Math.Exp(-x))); ;
        //    return dFx;
        //}

        //public double Func(double x) // TODO: изменить в методах на private
        //{
        //    return Math.Max(0, x);
        //}
        //public double dFunc(double x) // TODO: изменить в методах на private
        //{
        //    return Math.Max(0, 1);// x < 0 ? 0 : x;
        //}

        //Сигмойд активации.
        public double Func(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }
        public double dFunc(double x)
        {
            return Func(x) * (1 - Func(x));
        }

        // Расчет слоя.
        public double[] Calculate(int Length, double[] Xs, ref double[,] Ws)
        {            
            double[] Xns = new double[Length];
            for (var i = 0; i < Length; i++)
            {
                double Mult = 0;
                double Summ = 0;
                
                for (var j = 0; j < Xs.Length; j++)
                {
                    double X = Convert.ToDouble(Xs[j]);
                    double W = Convert.ToDouble(Ws[j, i]);
                    Mult = X * W;
                    Summ = Summ + Mult;                    
                }
                Xns[i] = Func(Summ);                
            }            
            return Xns;
        }
        // Расчет последнего слоя.
        public double[] CalculateLast(int Length, double[] Xs, ref double[] Ws)
        {  
            double[] Xns = new double[Length];
            for (var i = 0; i < Length; i++)
            {
                double Mult = 0;
                double Summ = 0;
                for (var j = 0; j < Xs.Length; j++)
                {
                    double X = Convert.ToDouble(Xs[j]);
                    double W = Convert.ToDouble(Ws[j]);
                    Mult = X * W;
                    Summ = Summ + Mult;                    
                }

                Xns[i] = Func(Summ);                
            }         
        return Xns;
        }

// Веса
        // Изменение весов. Последний слой.
        //public void ReturnLast(int Length, double Answer, double Learn, double[] Xs, double[] X0s, ref double[] Ws)
        //{
        //    if (console != 0)
        //    {
        //        Console.WriteLine("=__________________обратно______________= ");
        //        Console.WriteLine("Lenght = " + Length);
        //        Console.WriteLine("Answer = " + Answer);
        //        Console.WriteLine("LernRate = " + Learn);

        //        int k = 0;
        //        foreach (var item in Xs)
        //        {
        //            Console.WriteLine("input обратно [{1}]={0}", item, k);
        //            k++;
        //        }

        //        int t = 0;
        //        foreach (var item in X0s)
        //        {
        //            Console.WriteLine("Значения предыдущего слоя [{1}]={0}", item, t);
        //            t++;
        //        }


        //        int a = 0;
        //        foreach (var item in Ws)
        //        {
        //            Console.WriteLine("input Вес [{1}]={0}", item, a);
        //            a++;
        //        }
        //    }

        //// создаем для вего переменные кроме массива весов который изменяется
        //    int LayerLast = Length;
        //    double[] FxLast = new double[LayerLast];
        //    FxLast = Xs;
        //    double[] Fx2 = new double[X0s.Length];
        //    Fx2 = X0s;
        //    double Error = 0.0;
        //    double LearnRate = Learn;

        //    for (var i = 0; i < LayerLast; i++)
        //    {
        //        Error = (Answer - FxLast[i]) * (FxLast[i] * (1 - FxLast[i]));
        //        if (console != 0){Console.WriteLine("Error = " + Error);}
        //        for (var j = 0; j < Fx2.Length; j++)
        //        {
        //            var W = Convert.ToDouble(Ws[j]);
        //            var Wnew = 0.0;
        //            Wnew = W + (LearnRate * Error * Fx2[j]);// Значение предыдущего выхода.
        //            Ws[j] = Wnew;
        //        }
        //    }

        //    if (console != 0)
        //    {
        //        int y = 0;
        //        foreach (var item in Ws)
        //        {
        //            Console.WriteLine("Output Вес [{1}]={0}", item, y);
        //            y++;
        //        }
        //    }


        //}

        public void ReturnLast(int Length, double Answer, double Learn, double sigm, double[] Xs, double[] X0s, ref double[] Ws, out double[] sigm1)
        {            
            // создаем для вего переменные кроме массива весов который изменяется
            int LayerLast = Length;
            double[] Fx2 = new double[X0s.Length];
            double[] ssigm = new double[Fx2.Length];
            Fx2 = X0s;
            double Error = 0.0;

            double[] FxLast = new double[LayerLast];
            FxLast = Xs;
            Fx2 = X0s;
            double LearnRate = Learn;

            double epsil = Learn;
            double alfa = 0.3;
            double gradW;
            double deltaW;
            double Wnew2;

            for (var i = 0; i < LayerLast; i++)
            {
                Error = (Answer - FxLast[i]) * (FxLast[i] * (1 - FxLast[i]));
                for (var j = 0; j < Fx2.Length; j++)
                {
                    var W = Convert.ToDouble(Ws[j]);
                    var Wnew = 0.0;
                    gradW = Fx2[j] * sigm;
                    deltaW = epsil * gradW + alfa * 0;

                    Wnew2 = W + (LearnRate * Error * Fx2[j]);// Значение предыдущего выхода.
                    Console.WriteLine("Вес второй способ ="+Wnew2);
                    Wnew = W + deltaW;
                    Ws[j] = Wnew;

                    ssigm[j] = ((Answer - Fx2[j]) * Fx2[j]) * (W * sigm);
                    Console.WriteLine("Сигма нейрона {0}={1}",j,ssigm[j]);
                }
            }
            sigm1 = ssigm;
       }
        
        
        // Обратное распространение дельты весов
        public void ReturnL(ref double[] InWeightsLast, double sigm, double[] Fx2, double LearnRate, int deltaA)
        {
            double A = 0.3;            
            for (int i = 0; i <Fx2.Length; i++)
            {                
                double grad = Fx2[i] * sigm;
                double deltaW = LearnRate * grad + deltaA * A;
                double W = InWeightsLast[i] + deltaW;
                InWeightsLast[i] = W;                
            }
        }
        public void Return1(ref double[,] InWeights2, double[] sigm2, double sigm, double[] Fx1, double LearnRate, int deltaA, int lengthJ)
        {
            double A = 0.3;
            for (int i = 0; i < Fx1.Length; i++)
            {                
                for (var j = 0; j < lengthJ; j++)
                {
                    double grad = Fx1[i] * sigm2[j];   
                    double deltaW = LearnRate * grad + deltaA * A;
                    double W = InWeights2[i,j] + deltaW;
                    InWeights2[i,j] = W; 
                }
            }
        }

        // Инициализация массива весов.
        // Массив весов для входных значений.
        public double[,] Weights(int inCount, int layerLength)
        {
            double[,] inWeights = new double[inCount, layerLength];
            Random Wran = new Random();
            for (var i = 0; i < inCount; i++)
            {
                for (var j = 0; j < layerLength; j++)
                {
                    double wr = Wran.NextDouble() * (-0.5 - .5) + .5;
                    inWeights[i, j] = wr;
                }                
            }
            return inWeights;
        }
        // Массив весов для входных значений для слоя Last.
        public double[] WeightsLast(int layerLastLength)
        {
            double[] inWeightsLast = new double[layerLastLength];
            Random Wran = new Random();
            for (var i = 0; i < layerLastLength; i++)
            {
                double wr = Wran.NextDouble() * (-0.5 - .5) + .5;
                inWeightsLast[i] = wr;
            }
            return inWeightsLast;
        }

        //public double[] sigm(double[,] Answer, double[,] Weights, double[] sigm)
        //{
        //}


        // Расчет ошибки.
        public double Error(double answers, double layerLast)
        {
            double Error = Math.Pow((answers - layerLast), 2) / 1; //; второй способ 
            //double Error = (answers - layerLast) * (layerLast * (1 - layerLast));
            return Error;
            
        }        
        // Нормализация
        public double[] Normalize (int Length, ArrayList Xss)
        {
            double[] Xns = new double[Length];
            double[] Xs = (Double[])Xss.ToArray(typeof(double));
            double min = Xs.Min();
            double max = Xs.Max();                       
            for (int i = 0; i < Length; i++)
            {
                Xns[i] = (((Xs[i] - min) * 2) / (max - min)) - 1;                
            }
            return Xns;
        }
        public double[] Normalize2 (int Length, ArrayList Xss)
        {
            double[] Xns = new double[Length];
            double[] Xs = (Double[])Xss.ToArray(typeof(double));
            double min = Xs.Min();
            double max = Xs.Max();
            double xc = (min + max) / 2;
            double a = 1.0;
            for (int i = 0; i < Length; i++)
            {
                Xns[i] = (Math.Exp(a*(Xs[i]-xc))-1)/ (Math.Exp(a * (Xs[i] - xc)) + 1);
            }
            return Xns;
        }

        public void ShowProgress(int value, int max)
        {
            float w = (float)value / max;
            int l1 = string.Format("{0:p}", w).Length;
            int l = (int)((Console.BufferWidth - l1) * (value / (float)max));
            Console.Write("\r{0}{1:p}", new string('\u2591', l), w);
        }

    }
}






// TODO:  1. Neyron - Сделать еще 2 распространенные функции 
// TODO:  2. Neyron - Добавить методы для другой схемы сети  

