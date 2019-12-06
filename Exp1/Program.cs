// Тело программы откуда все вызывается и где всё обьеденяется

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Exp1.Classes;
using NetPatterns;
using System.IO.Ports;
using System.Globalization;
using System.Threading;
using System.Timers;


namespace Exp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;//<<<-----

            // Массив значений входных.
            // Массив правильных значений.
            // Массив весов для входных значений.
            // Нормализация.
            // Расчет функции.
            // Расчет ошибки.
            // Корректировка веса.

            int test = 0;
            int normaltobase = 0;
            int debug = 0;
            int learn = 0;

            //------------------------------------------------------------------------------------------------
            // Инициализация.
            var InCount = 9; // Количество входных параметров.
            var Epoch = 1 ;
            var AnswerCount = 4;
            var Layer1 = 9;
            var Layer2 = 9;
            var Layer3 = 1;
            var LayerLast = 1;
            var LearnRate = 0.03;           
            
            double[] In = new double[InCount];
            double[] Fx1 = new double[Layer1];
            double[] Fx2 = new double[Layer2];
            double[] FxLast = new double[LayerLast];
            double[,] InWeights1 = new double[InCount,Layer1];
            double[,] InWeights2 = new double[Fx1.Length, Layer2];
            double[,] InWeights3 = new double[Fx2.Length, Layer3];
            double[] InWeightsLast = new double[Fx2.Length];

            double[] Answers = new double[AnswerCount]; // Массив правильных значений.
            double[] Errors = new double[Epoch]; // Массив ошибок.

           

            //------------------------------------------------------------------------------------------------
            // Получение значений

            // ------------------ ComPort
            int round = 115200;
            int time = 1000;

            int timeround = 115200;
            SerialPort serialPort1 = new SerialPort();
            serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.Two;
            serialPort1.Parity = Parity.None;
            serialPort1.ReadTimeout = 1000;

            string datastring;

            string name = "";
            string[] data = new string[timeround];

            string[] names = new string[10];

            //string[] comportname = new string[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10"};
            string[] comportname = new string[] { "COM3" };
            //Console.WriteLine("comportname = " + comportname.Length);

            // перебор рабочих портов

            for (int i = 0; i < comportname.Length; i++)
            {
                serialPort1.PortName = comportname[i];
                try
                {
                    serialPort1.Open();
                    Console.WriteLine("port {0} opened", comportname[i]);
                    names[i] = comportname[i];

                    //serialPort1.Close();

                    //bool stat = serialPort1.CDHolding;
                    //Console.WriteLine("stat = " + stat);
                }
                catch
                {
                    Console.WriteLine("port {0} closed", comportname[i]);
                }
                serialPort1.Close();
            }

            foreach (var item in names)
            {
                // Определение имени рабочего порта                
                string str = "";
                Console.WriteLine(item);
                if (item != null)
                {
                    //Console.WriteLine(item);

                    try
                    {
                        serialPort1.PortName = item;
                        serialPort1.Open();
                        System.Threading.Thread.Sleep(time); // just wait a lot
                        for (int a = 0; a < timeround; a++)
                        {
                            try
                            {

                                str = serialPort1.ReadExisting();
                                if (str != "")
                                {
                                    //Console.WriteLine(str);                                   
                                    data[a] = str;
                                    //Console.WriteLine("Length = {0}", str.Length);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Порт закрыт");
                            }
                        }
                        serialPort1.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Порт закрыт");
                    }
                }

            }

            // Собрать строки массива в одну строку

            datastring = string.Join(null, data);

            // Инициализация массивов значений Axel Magnet Gyro
            double[] Axel = new double[data.Length * 3];
            double[] Magn = new double[data.Length * 3];
            double[] Gyro = new double[data.Length * 3];

            Console.WriteLine(datastring);

            ArrayList axelarr = new ArrayList();
            ArrayList magnarr = new ArrayList();
            ArrayList gyroarr = new ArrayList();

            ParseValues NewVal = new ParseValues();

            NewVal.ParcePort(datastring, out axelarr, out magnarr, out gyroarr);

            foreach (var item in axelarr) { Console.WriteLine("axel = {0} ", item); }
            foreach (var item in magnarr) { Console.WriteLine("magne = {0} ", item); }
            foreach (var item in gyroarr) { Console.WriteLine("gyro = {0} ", item); }

            Console.WriteLine("axel={0}, gyro={1}, magne={2}", axelarr.Count, gyroarr.Count, magnarr.Count);

            // ComPort запись в базу            
            // Запись в базу
            //------------------------------------------------------------------------------------------------
            // База данных проверка работы

            BaseWork bs = new BaseWork();

            double Xa, Ya, Za, Xg, Yg, Zg, Xm, Ym, Zm;
            
            string tableA = "VvodA";
            string tableG = "VvodG";
            string tableM = "VvodM";
            string tableAns = "Answer";
            //string column = "Xa,Ya,Za,Xg,Yg,Zg,Xm,Ym,Zm";
            string columnA = "Xa,Ya,Za";
            string columnG = "Xg,Yg,Zg";
            string columnM = "Xm,Ym,Zm";

            string valuesA, valuesG, valuesM = "";

            for (int i = 0; i < axelarr.Count; i=i+3)
            {
                Xa = Convert.ToDouble(axelarr[i]);
                Ya = Convert.ToDouble(axelarr[i+1]);
                Za = Convert.ToDouble(axelarr[i+2]);

                //Console.WriteLine("xa = {0} ya = {1} za = {2}",Xa,Ya,Za);

                Xg = Convert.ToDouble(gyroarr[i]);
                Yg = Convert.ToDouble(gyroarr[i + 1]);
                Zg = Convert.ToDouble(gyroarr[i + 2]);

                //Console.WriteLine("xg = {0} yg = {1} zg = {2}", Xg, Yg, Zg);

                Xm = Convert.ToDouble(magnarr[i]);
                Ym = Convert.ToDouble(magnarr[i + 1]);
                Zm = Convert.ToDouble(magnarr[i + 2]);

                //Console.WriteLine("xm = {0} ym = {1} zm = {2}", Xm, Ym, Zm);

                valuesA = String.Format("{0},{1},{2}", Xa, Ya, Za);
                valuesG = String.Format("{0},{1},{2}", Xg, Yg, Zg);
                valuesM = String.Format("{0},{1},{2}", Xm, Ym, Zm);

                if (test != 0)
                {
                    Console.WriteLine( "Записываем в базу данных");
                    bs.AddSqlData(tableA, columnA, valuesA);
                    bs.AddSqlData(tableG, columnG, valuesG);
                    bs.AddSqlData(tableM, columnM, valuesM);
                }
                else
                {
                    Console.WriteLine("Тестовый режим читаем из базы");
                }

            }

            //  --------------- База данных проверка работы

            //bs.PrintSqlData(tableA);
            //bs.PrintSqlData(tableG);
            //bs.PrintSqlData(tableM);

            //bs.DeleteSqlData(tableA);
            //bs.DeleteSqlData(tableG);
            //bs.DeleteSqlData(tableM);

            //bs.PrintSqlData(tableA);
            //bs.PrintSqlData(tableG);
            //bs.PrintSqlData(tableM);

            //------------------------------------------------------------------------------------------------
            // Чтение из базы
            // Читаем из базы исходные значения ( для тестового режима )

            ArrayList axelarrX, axelarrY, axelarrZ = new ArrayList();
            ArrayList gyroarrX, gyroarrY, gyroarrZ = new ArrayList();
            ArrayList magnarrX, magnarrY, magnarrZ = new ArrayList();
            ArrayList answerArr = new ArrayList();

            bs.GetSqlData(tableA, "Xa", out axelarrX);
            bs.GetSqlData(tableA, "Ya", out axelarrY);
            bs.GetSqlData(tableA, "Za", out axelarrZ);

            bs.GetSqlData(tableG, "Xg", out gyroarrX);
            bs.GetSqlData(tableG, "Yg", out gyroarrY);
            bs.GetSqlData(tableG, "Zg", out gyroarrZ);

            bs.GetSqlData(tableM, "Xm", out magnarrX);
            bs.GetSqlData(tableM, "Ym", out magnarrY);
            bs.GetSqlData(tableM, "Zm", out magnarrZ);

            bs.GetSqlData(tableAns, "Ans", out answerArr);

            //------------------------------------------------------------------------------------------------
            // Нормализация
            // Получаем нормализованные значения X Y Z для входов

            Neyron N0 = new Neyron();

            double[] axelarrXn = N0.Normalize(axelarrX.Count, axelarrX);
            double[] axelarrYn = N0.Normalize(axelarrX.Count, axelarrY);
            double[] axelarrZn = N0.Normalize(axelarrX.Count, axelarrZ);

            double[] gyroarrXn = N0.Normalize(axelarrX.Count, gyroarrX);
            double[] gyroarrYn = N0.Normalize(axelarrX.Count, gyroarrY);
            double[] gyroarrZn = N0.Normalize(axelarrX.Count, gyroarrZ);

            double[] magnarrXn = N0.Normalize(axelarrX.Count, magnarrX);
            double[] magnarrYn = N0.Normalize(axelarrX.Count, magnarrY);
            double[] magnarrZn = N0.Normalize(axelarrX.Count, magnarrZ);

            double Xan, Yan, Zan, Xgn, Ygn, Zgn, Xmn, Ymn, Zmn;
            string columns = "Xan,Yan,Zan,Xgn,Ygn,Zgn,Xmn,Ymn,Zmn";
            
            for (int i = 0; i < axelarrX.Count; i++)
            {
                Xan = Convert.ToDouble(axelarrXn[i]);
                Yan = Convert.ToDouble(axelarrYn[i]);
                Zan = Convert.ToDouble(axelarrZn[i]);

                //Console.WriteLine("xa = {0} ya = {1} za = {2}",Xa,Ya,Za);

                Xgn = Convert.ToDouble(gyroarrXn[i]);
                Ygn = Convert.ToDouble(gyroarrYn[i]);
                Zgn = Convert.ToDouble(gyroarrZn[i]);

                //Console.WriteLine("xg = {0} yg = {1} zg = {2}", Xg, Yg, Zg);

                Xmn = Convert.ToDouble(magnarrXn[i]);
                Ymn = Convert.ToDouble(magnarrYn[i]);
                Zmn = Convert.ToDouble(magnarrZn[i]);

                //Console.WriteLine("xm = {0} ym = {1} zm = {2}", Xm, Ym, Zm);

                string values = String.Format("{0},{1},{2},{3}, {4}, {5}, {6}, {7}, {8}", Xan, Yan, Zan, Xgn, Ygn, Zgn, Xmn, Ymn, Zmn);

                if (normaltobase != 0)
                {
                    Console.WriteLine("Записываем нормализованные в базу данных");
                    bs.AddSqlData("Normal", columns, values);
                }
                else
                {
                    //Console.WriteLine("Тестовый режим читаем из базы");
                }
     
            }


            // ----------------------------------------------------------------------------
            // Проверка сети
            // Загрузить ответы
            //Answers[0] = 0;
            //Answers[1] = 0;
            //Answers[2] = 1;
            //Answers[3] = 1;
            // Загрузить входные
            //int[] In1 = new int[] { 0, 0, 1, 1, 0, 1, 1, 0 };
            //---------------------------------------------------------------------------------

            // Поместить входные нормализованные значения в нейросеть

            // Поместить ответы в нейросеть
            

            //------------------------------------------------------------------------------------------------           
            // Массив правильных значений.

            Answers = (Double[])answerArr.ToArray(typeof(double));
            AnswerCount = Answers.Length;

            double[] Last = new double[AnswerCount];


            if (learn !=0)
            {
                // Массив весов для входных значений.
                Neyron Mass = new Neyron();
                InWeights1 = Mass.Weights(InCount, Layer1);
                // Массив весов для входных значениq из слоя 1 для слоя 2.
                InWeights2 = Mass.Weights(Layer1, Layer2);
                // Массив весов для входных значениq из слоя 2 для слоя 3.
                InWeights3 = Mass.Weights(Layer2, Layer3);
                // Массив весов для входных значений из слоя 3 для слоя Last.
                InWeightsLast = Mass.WeightsLast(Layer2);
            }
            else
            {
                ArrayList InW1, InW2, InW3, InWL = new ArrayList();
                bs.GetSqlData("InWeights1", "W", out InW1);
                bs.GetSqlData("InWeights2", "W", out InW2);
                bs.GetSqlData("InWeights3", "W", out InW3);
                bs.GetSqlData("InWeightsLast", "W", out InWL);
                int p = 0;
                for (int i = 0; i <InCount; i++){
                    for (int j = 0; j < Layer1; j++){
                        InWeights1[i, j] = Convert.ToDouble(InW1[p]);
                        p++;
                    }
                }
                p = 0;
                for (int i = 0; i < Layer1; i++){
                    for (int j = 0; j < Layer2; j++){
                        InWeights2[i, j] = Convert.ToDouble(InW2[p]);
                        p++;
                    }
                }
                p = 0;
                for (int i = 0; i < Layer2; i++){
                    for (int j = 0; j < Layer3; j++){
                        InWeights3[i, j] = Convert.ToDouble(InW2[p]);
                        p++;
                    }
                }
                for (int i = 0; i < Layer2; i++){
                        InWeightsLast[i] = Convert.ToDouble(InW2[i]);
                        p++;
                    }
            }

            // Массив ошибок
            double[] Ers = new double[AnswerCount];


//-------------------------------------------------------------------------------------------------
            // Цикл повторов.
            for (var k = 0; k < Epoch; k++)
            {
                //Console.Write("++++++++++++++++++++++++++++++++++++Epoch = "+ k);
                for (var a = 0; a <AnswerCount; a++)
                {
                    //Тест сети

                    //In[0] = In1[a];
                    //In[1] = In1[a + 1];

                    // Инициализация входного массива из 9ти параметров должна быть здесь, из нормализованного общего массива запихиваются 9 значений в In, 
                    // И так повторяется столько сколько есть ответов
                    // Потом посторяется столько раз весь цикл сколько хочешь

                    In[0] = Convert.ToDouble(axelarrXn[a]);
                    In[1] = Convert.ToDouble(axelarrYn[a]);
                    In[2] = Convert.ToDouble(axelarrZn[a]);
                    In[3] = Convert.ToDouble(gyroarrXn[a]);
                    In[4] = Convert.ToDouble(gyroarrYn[a]);
                    In[5] = Convert.ToDouble(gyroarrZn[a]);
                    In[6] = Convert.ToDouble(magnarrXn[a]);
                    In[7] = Convert.ToDouble(magnarrYn[a]);
                    In[8] = Convert.ToDouble(magnarrZn[a]);

//------------------------------------------------------------------------------------------------------------------------
                    // Прямое распространение
                    // Расчет функции активации слой1.                    
                    Neyron N1 = new Neyron();
                    Fx1 = N1.Calculate(Layer1, In, ref InWeights1);
                    
                    // Расчет функции активации слой2.                
                    Neyron N2 = new Neyron();
                    Fx2 = N2.Calculate(Layer2, Fx1, ref InWeights2);                
                
                    // Расчет функции активации последний слой.
                    Neyron N3 = new Neyron();
                    FxLast = N3.CalculateLast(LayerLast, Fx2, ref InWeightsLast);
                    Last[a] = FxLast[0];
                    //------------------------------------------------------------------------------------------------------------------------
                    // Обратное распространение
                    // Расчет ошибки.
                    double ers = Answers[a] - FxLast[0];
                    Ers[a] = Math.Pow(ers, 2);                   
                   // Console.WriteLine("--------------FxLast= {0}, Answers[a]= {1} ers2 = {2}------------------", FxLast[0], Answers[a], ers);
                                          
                    double sigm = (Answers[a] - FxLast[0])*N1.dFunc(FxLast[0]);                    
                    double[] sigm2 = new double[Fx2.Length]; // сколько нейронов в слое перед последним столько и сигм
                    double[] sigm1 = new double[Fx1.Length]; // сколько нейронов в слое перед предпоследним столько и сигм

                    // меняем веса которые идут на последний слой
                    N1.ReturnL(ref InWeightsLast, sigm, Fx2, LearnRate, 0);

                    // симгма слой 2 
                    for (var i = 0; i < Fx2.Length; i++)
                    {                        
                        sigm2[i] = (N1.dFunc(Fx2[i])) * (InWeightsLast[i] * sigm);                        
                    }

                    // меняем веса которые идут на последний слой
                    N1.Return1(ref InWeights2, sigm2, sigm, Fx1, LearnRate, 0, Fx2.Length);

                    // сигма слой 1
                    for (var i = 0; i < Fx1.Length; i++)
                    {
                        for (var j = 0; j < Fx2.Length; j++)
                        {                            
                            sigm1[i] = (N1.dFunc(Fx1[i]) * Fx1[i]) * (InWeights2[i, j] * sigm);                         
                        }
                    }
                    N1.Return1(ref InWeights1, sigm1, sigm, In, LearnRate, 0, Fx1.Length);
                }

                double sum = 0;
                for (int i = 0; i < AnswerCount; i++){                    
                    sum = Ers[i] + sum;                    
                }
                Errors[k] = sum / AnswerCount;
                N0.ShowProgress(k,Epoch);
            }

            //Запись весов для повтора

            if (learn != 0)
            {
                string[] tablesW = {"InWeights1", "InWeights2", "InWeights3", "InWeightsLast"};
                foreach (var item in tablesW) { bs.DeleteSqlData(item); }
                
                string valuesW;
                Console.WriteLine("Записываем веса 1 в базу данных");
                foreach (var item in InWeights1)
                {
                    valuesW = String.Format("{0}", item);
                    bs.AddSqlData("InWeights1", "W", valuesW);
                }
                Console.WriteLine("Записываем веса 2 в базу данных");
                foreach (var item in InWeights2)
                {
                    valuesW = String.Format("{0}", item);
                    bs.AddSqlData("InWeights2", "W", valuesW);
                }
                Console.WriteLine("Записываем веса Last в базу данных");
                foreach (var item in InWeightsLast)
                {
                    valuesW = String.Format("{0}", item);
                    bs.AddSqlData("InWeightsLast", "W", valuesW);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Timer t = new Timer(); 
            FormIn f1 = new FormIn(t);
            FormTimer f2 = new FormTimer();
            t.TimeChangedEvent += f1.TimerChangedHandler;
            //t.TimeChangedEvent += f2.TimerChangedHandler;
            f1.Show();
            f2.Show();
            Application.Run(new Form1(Errors));
            Application.Run(new Form1(Last));
        }
    }
}


// UNDONE: Закончил на вводе данных в базу данных, настройке форм и задания паттернов наблюдатель и вывод в 3ю форму. 

// TODO: 1. Создать таблицы 

// TODO: 2. Записать данные с ком порта в базу 
// TODO: 3. Нормализовать данные 
// TODO: 3. Прочитать данные из базы, запихать в сеть
// TODO: 3. Получить ошибку и записать результаты весов в базу
// TODO: 3. Прочитать данные весов из базы, должно быть два режима тест и рабочий

// TODO: 4. Нарисовать паттерн наблюдатель 
// TODO: 5. Записать в базу результаты 
// TODO: 6. Вывести результаты на форму   
// TODO: 7. Написать скелет по которым строятся сети разных типов

// TODO: 8. Написать методы нормлизации чтобы можно было запихивать любые входные значения
// TODO: 9. Написать методы для того чтобы можно было запихивать любые ответы

// TODO: 10. Написать метод разбиения расчетов на потоки и процессоры автоматически

// TODO: 11. Написать конечную функцию Если ошибка такая то это что
// TODO: 12. Написать метод задания своей функции

// TODO: 13. Написать разновременный запуск форм       




