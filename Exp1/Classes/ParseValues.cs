// Класс парсинга данных

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO.Ports;
using System.Globalization;
using System.Threading;

namespace Exp1.Classes
{

    // Класс парсинга данных
    class ParseValues
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
                string[] patterns = new string[] {"A-C=", "M-C=", "G-C="};

                int last = 0;
                int start = 0;
                string temp = "";

                for (int i = 0; i < patterns.Length; i++)
                {
                    if (substring.Contains(patterns[i]) == true)
                    {
                        last = substring.LastIndexOf(patterns[i]);
                        temp = substring.Substring(last + 4);
                        string[] separator = new string[] {","};
                        string[] temp2 = temp.Split(separator, StringSplitOptions.None);

                        if (temp2.Length == 3)
                        {
                            valuemass[0] = Double.Parse(temp2[0], CultureInfo.InvariantCulture);
                            valuemass[1] = Double.Parse(temp2[1], CultureInfo.InvariantCulture);
                            valuemass[2] = Double.Parse(temp2[2], CultureInfo.InvariantCulture);
                        }

                        if (patterns[i] == "A-C=")
                        {
                            axelarr.AddRange(valuemass);
                        }
                        else if (patterns[i] == "M-C=")
                        {
                            magnarr.AddRange(valuemass);
                        }
                        else
                        {
                            gyroarr.AddRange(valuemass);
                        }


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
}
