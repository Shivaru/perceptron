// Timer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetPatterns;

namespace Exp1
{
        // .NET вариант - используем делегаты и события...
        public delegate void TimerChangedEventHandler(Object src, EventArgs e);

        public class Timer
        {
            int curValue;

            public event TimerChangedEventHandler TimeChangedEvent;

            public int Seconds
            {
                get
                {
                    return curValue;
                }
            }

            public void Tick()
            {
                DateTime dt = DateTime.Now;
                TimeSpan ts = DateTime.Now - dt;
                int sec = ts.Seconds;
                while (ts.Seconds < 10)
                {
                    ts = DateTime.Now - dt;
                    if (sec != ts.Seconds)
                    {
                        sec = ts.Seconds;
                        curValue = sec;
                        TimeChangedEvent(this, EventArgs.Empty);
                    }
                }
            }
        }


}
