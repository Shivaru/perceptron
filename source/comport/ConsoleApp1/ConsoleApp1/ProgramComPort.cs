using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Timers;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows;

namespace ComPortT
{
    // Класс работы с ComPort
    public class ProgramComPort
    {

        //  Наследуем наш клас от SerialPort для более красивого кода
        public class MySerialPort : SerialPort
        {
            private const int DataSize = 54;    // UNDONE: так и не понял, какой размер данных нужен. Укажите правильное число в байтах
            //private readonly string[] _bufer = new string[DataSize];
            private readonly byte[] _bufer = new byte[DataSize];
            private int _stepIndex;
            private bool _startRead;

            public MySerialPort(string port) : base()
            {
                //  все папаметры вы должны указать в соответствии с вашим устройством
                //base.PortName = "COM3";
                base.BaudRate = 115200;
                base.DataBits = 8;
                base.StopBits = StopBits.Two;
                base.Parity = Parity.None;
                base.ReadTimeout = 1000;
                Open(port);

                //  тут подписываемся на событие прихода данных в порт
                //  для вашей задачи это должно подойти идеально
                base.DataReceived += SerialPort_DataReceived;
            }

            //  открываем порт передав туда имя
            public void Open(string portName)
            {
                if (base.IsOpen)
                {
                    base.Close();
                }
                base.PortName = portName;
                base.Open();
                Console.WriteLine("Open port {0}", _bufer);
            }

            //  эта функция вызвется каждый раз, когда в порт чтото будет передано от вашего устройства
            
            //void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
            //{
            //    var port = (SerialPort)sender;
            //    try
            //    {
            //        //  узнаем сколько байт пришло
            //        int buferSize = port.BytesToRead;
            //        for (int i = 0; i < buferSize; ++i)
            //        {
            //            //  читаем по одному байту
            //            byte bt = (byte)port.ReadByte();
            //            //  если встретили начало кадра (0xFF) - начинаем запись в _bufer
            //            if (0xFF == bt)
            //            {
            //                _stepIndex = 0;
            //                _startRead = true;
            //                //  раскоментировать если надо сохранять этот байт
            //                //_bufer[_stepIndex] = bt;
            //                //++_stepIndex;
            //            }
            //            //  дописываем в буфер все остальное
            //            if (_startRead)
            //            {
            //                _bufer[_stepIndex] = bt;
            //                ++_stepIndex;
            //            }
            //            //  когда буфер наполнлся данными
            //            if (_stepIndex == DataSize && _startRead)
            //            {
            //                //  по идее тут должны быть все ваши данные.
                            
            //                //  .. что то делаем ...
            //                //  var item = _bufer[7];

            //                _startRead = false;
            //            }
            //        }
            //    }
            //    catch { }
            //}
            
            void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                var port = (SerialPort)sender;
                try
                {
                    //  узнаем сколько байт пришло
                    int buferSize = port.BytesToRead;
                    for (int i = 0; i < buferSize; ++i)
                    {
                        //  читаем по одному байту
                        byte bt = (byte)port.ReadByte();
                        //  если встретили начало кадра (0xFF) - начинаем запись в _bufer
                        if (0xFF == bt)
                        {
                            _stepIndex = 0;
                            _startRead = true;
                            //  раскоментировать если надо сохранять этот байт
                            //_bufer[_stepIndex] = bt;
                            //++_stepIndex;
                        }
                        //  дописываем в буфер все остальное
                        if (_startRead)
                        {
                            _bufer[_stepIndex] = bt;
                            ++_stepIndex;
                        }
                        //  когда буфер наполнлся данными
                        if (_stepIndex == DataSize && _startRead)
                        {
                            //  по идее тут должны быть все ваши данные.

                            //  .. что то делаем ...
                            //  var item = _bufer[7];

                            _startRead = false;
                        }
                    }
                }
                catch { }
            }




        }

        // Класс парсинга данных
        public class Values
        {
            // Метод парсинга строки
            public void ParcePort(string datastring, out ArrayList axel, out ArrayList magn, out ArrayList gyro)
            {
                Char delimitersharp = '#';
                String[] substrings = datastring.Split(delimitersharp);

                double[] valuemass = new double[3];

                //ArrayList valuemass = new ArrayList();
                ArrayList tempaxelarr = new ArrayList();
                ArrayList tempmagnarr = new ArrayList();
                ArrayList tempgyroarr = new ArrayList();

                ArrayList axelarr = new ArrayList();
                ArrayList magnarr = new ArrayList();
                ArrayList gyroarr = new ArrayList();

                ArrayList emptyarr = new ArrayList();

                foreach (var substring in substrings)
                {
                    string[] patterns = new string[] { "A-C=", "M-C=", "G-C=" };

                    int last = 0;
                    int start = 0;
                    string temp = "";

                    for (int i = 0; i < patterns.Length; i++)
                    {
                        if (substring.Contains(patterns[i]) == true)
                        {
                            last = substring.LastIndexOf(patterns[i]);
                            temp = substring.Substring(last + 4);
                            string[] separator = new string[] { "," };
                            string[] temp2 = temp.Split(separator, StringSplitOptions.None);

                            if (temp2.Length == 3)
                            {
                                valuemass[0] = Double.Parse(temp2[0], CultureInfo.InvariantCulture);
                                valuemass[1] = Double.Parse(temp2[1], CultureInfo.InvariantCulture);
                                valuemass[2] = Double.Parse(temp2[2], CultureInfo.InvariantCulture);
                            }

                            if (patterns[i] == "A-C=") { axelarr.AddRange(valuemass); }
                            else if (patterns[i] == "M-C=") { magnarr.AddRange(valuemass); }
                            else { gyroarr.AddRange(valuemass); }


                            //foreach (var item in temp2)
                            //{
                            //    double val = Double.Parse(item, CultureInfo.InvariantCulture);
                            //    valuemass.Add(val);
                            //}

                            //foreach (var item in valuemass)
                            //{
                            //   // Console.WriteLine(item);
                            //}

                            //// UNDONE: Незаписываются аррай лист в аррей лист

                            //if (patterns[i] == "A-C=") { tempaxelarr.AddRange(valuemass); }
                            //else if (patterns[i] == "M-C=") { tempmagnarr.AddRange(valuemass); }
                            //else { tempgyroarr.AddRange(valuemass); }

                            //if (tempaxelarr.Count == 3 && tempmagnarr.Count == 3 /*&& tempgyroarr.Count == 3*/)
                            //{
                            //    axelarr.AddRange(tempaxelarr);
                            //    magnarr.AddRange(tempmagnarr);
                            //    gyroarr.AddRange(tempgyroarr);
                            //}

                        }
                    }
                }

                axel = axelarr;
                magn = magnarr;
                gyro = gyroarr;



            }
        }

        // Класс получения данных с Bluetooth


        public class Port 
        {
            System.Timers.Timer aTimer;
            SerialPort currentPort = new SerialPort();

            public SerialPort CurrentPort
            {
                get => currentPort;
                set => currentPort = value;
            }

            public bool ArduinoDetected()
            {
                try
                {
                    currentPort.Open();
                    System.Threading.Thread.Sleep(1000); // just wait a lot

                    string returnMessage = currentPort.ReadLine();
                    currentPort.Close();

                    // in arduino sketch should be Serial.println("Info from Arduino") inside  void loop()
                    if (returnMessage.Contains(""))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            private void Window_Loaded(object sender, EventArgs e)
            {
                bool ArduinoPortFound = false;

                try
                {
                    string[] ports = SerialPort.GetPortNames();
                    foreach (string port in ports)
                    {
                        currentPort = new SerialPort(port, 9600);
                        if (ArduinoDetected())
                        {
                            ArduinoPortFound = true;
                            break;
                        }
                        else
                        {
                            ArduinoPortFound = false;
                        }
                    }
                }
                catch { }

                if (ArduinoPortFound == false) return;

                System.Threading.Thread.Sleep(500); // wait a lot after closing

                currentPort.BaudRate = 9600;
                currentPort.DtrEnable = true;
                currentPort.ReadTimeout = 1000;
                try
                {
                    currentPort.Open();
                }
                catch { }

                aTimer = new System.Timers.Timer(1000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;









            }

            private void OnTimedEvent(object sender, ElapsedEventArgs e)
            {
                if (!currentPort.IsOpen) return;
                try
                {
                    currentPort.DiscardInBuffer();  // remove old information from buffer
                    string strFromPort = currentPort.ReadLine();  // read last value
             
                }
                catch
                {
             
                }
            }

            private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                aTimer.Enabled = false;
                currentPort.Close();
            }



        }


        // Тело программы.
        static void Main(string[] args)
        {
            int timeround = 115200;
            SerialPort serialPort1 = new SerialPort();
            //ProgramComPort.MySerialPort sport = new ProgramComPort.MySerialPort("Com4");
            

            serialPort1.BaudRate = 115200;
            //serialPort1.ReadTimeout = 20;
            string datastring;

            string name = "";
            string[] data = new string[timeround];

            string[] names = new string[10];

            // Массив имен портов

            string[] comportname = new string[10] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10"};

            // перебор рабочих портов

            //serialPort1.PortName = "COM4";
            //FieldInfo fi = typeof(SerialPort).GetField("CtsHolding");
            //object fieldValue = fi.GetValue(serialPort1);      
            
            for (int i = 0; i < 10; i++)
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

                if (item != null)
                {
                    Console.WriteLine(item);
                    
                    try
                    {
                        serialPort1.PortName = item;
                        serialPort1.Open();
                        System.Threading.Thread.Sleep(1000); // just wait a lot
                        for (int a = 0; a < timeround; a++)
                        {
                            try
                            {

                                str = serialPort1.ReadExisting();
                                if ( str != "")
                                {
                                   Console.WriteLine(str);                                   
                                    data[a] = str;
                                    Console.WriteLine("Length = {0}", str.Length);
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

            // Тестируем задавая свою строку
            // Первое и последнюю строку надо отсекать потому что они могут быть неполными или обрывочными                                                                                
     
            //datastring = "#A-C=-12.29,13.31,245.76 #M-C=-86.33,-64.50,63.50 #G-C=-42.00,60.00,-3.00 #A-C=-11.26,14.34,246.78 #M-C=-86.17,-64.33,64.17 #G-C=-43.00,60.00,-2.00";
            //datastring = "#A-C=-12.29,13.31,245.76 #M-C=-86.33,-64.50,63.50 #G-C=-42.00 #A-C=-11.26,14.34,246.78 #M-C=-86.17,-64.33,64.17 #G-C=-43.00";
            //datastring = "#A-C=-12.29,13.31,245.76 #M-C=-86.33,-64.50 #G-C=-42.00,60.00,-3.15";
            //datastring = "#A-C=1,2,3 #M-C=4,5,89 #G-C=7,8,9";

            Console.WriteLine(datastring);

            ArrayList axelarr = new ArrayList();
            ArrayList magnarr = new ArrayList();
            ArrayList gyroarr = new ArrayList();

            Values NewVal = new Values();

            NewVal.ParcePort(datastring,out axelarr, out magnarr, out gyroarr);

            foreach (var item in axelarr) { Console.WriteLine("axel = {0} ", item);}
            foreach (var item in magnarr) { Console.WriteLine("magne = {0} ", item); }
            foreach (var item in gyroarr) { Console.WriteLine("gyro = {0} ", item); }


            // Тест

            string[] portnames = SerialPort.GetPortNames();

            string[] dataBytes = new String[256];


            SerialPort newPort = new SerialPort("COM3");
            newPort.Open();
            dataBytes[1]=newPort.ReadLine();


            foreach (var bt in dataBytes)
            {
                Console.WriteLine(bt);
            }





            Console.ReadLine();


        }
    }


}

// TODO: 1  Метод разбора строки ввода 
// TODO: 2. Определить метод засекащий время считывания 
// TODO: 3. Метод запиывающий разобранные значения в массив для дступа из другого класса 
// TODO: 4. Обработать исключение отсутствие данных в ком порту


/*
            //Char delimitersharp = '#';            
            //String[] substrings = datastring.Split(delimitersharp);
            //Console.WriteLine("---------- Отделяем подстроки ----------");

            //double[] axelmass  = new double[3];
            //double[] magnmass = new double[3];
            //double[] gyromass = new double[3];
            //double[] valuemass = new double[3];


            foreach (var substring in substrings)
            {
                string[] patterns = new string[] { "A-C=", "M-C=", "G-C=" };

                int last = 0;
                int start = 0;
                string temp = "";

                for (int i = 0; i < patterns.Length; i++)
                {
                    if (substring.Contains(patterns[i]) == true)
                    {
                        last = substring.LastIndexOf(patterns[i]);
                        Console.WriteLine("Pattern {1} = {0}", substring.Substring(last + 4), patterns[i]);

                        temp = substring.Substring(last + 4);
                        string[] separator = new string[] { "," };
                        string[] temp2 = temp.Split(separator, StringSplitOptions.None);

                        if (temp2.Length == 3)
                        {
                            Console.WriteLine("X {1} = {0}", temp2[0], patterns[i]);
                            Console.WriteLine("Y {1} = {0}", temp2[1], patterns[i]);
                            Console.WriteLine("Z {1} = {0}", temp2[2], patterns[i]);
                            valuemass[0] = Double.Parse(temp2[0], CultureInfo.InvariantCulture);
                            valuemass[1] = Double.Parse(temp2[1], CultureInfo.InvariantCulture);
                            valuemass[2] = Double.Parse(temp2[2], CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            Console.WriteLine("Битая строка");
                        }

                        if (patterns[i] == "A-C=") { axelarr.AddRange(valuemass); }
                        else if (patterns[i] == "M-C=") { magnarr.AddRange(valuemass); }
                        else { gyroarr.AddRange(valuemass); }

                        //if (patterns[i] == "A-C=") { axelmass = valuemass; }
                        //else if (patterns[i] == "M-C=") { magnmass = valuemass; }
                        //else { gyromass = valuemass; }
                    }
                }
            }
 
     
     
     
     
     
     */

