using System;
using System.Collections.Generic;
using System.Text;
//using System.Collections;

namespace NetPatterns
{
    public interface IObserver
    {
        void Update();
    }

    public abstract class Subject
    {
        protected List<IObserver> observers;

        public void Add(IObserver o)
        {
            observers.Add(o);
        }

        public void Remove(IObserver o)
        {
            if (observers.Contains(o))
                observers.Remove(o);
        }

        public void Notify()
        {
            foreach (IObserver o in observers)
                o.Update();
        }
    }

    public class defaultObserver : IObserver
    {
        public void Update()
        {
        }
    }


    public class ConcreteSubject : Subject
    {
        int curValue;

        public ConcreteSubject()
        {
            observers = new List<IObserver>();
        }

        public int SubjectState
        {
            get
            {
                return curValue;
            }
        }

        public void Timer()
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
                    Notify();
                }
            }
        }
    }

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
