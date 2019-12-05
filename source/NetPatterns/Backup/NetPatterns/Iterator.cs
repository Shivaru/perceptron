using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace NetPatterns
{
    public interface Iterator
    {
        void FirstItem();
        void NextItem();
        bool isDone();
        int currentItem();
    }

    public interface IAggregate
    {
        Iterator CreateIterator();
    }

    public class MyAggregate : IAggregate
    {
        int[] arr;

        public MyAggregate()
        {
            arr = new int[3] { 1, 5, 7 };
        }

        public int getItem(int idx)
        {
            return arr[idx];
        }

        public Iterator CreateIterator()
        {
            return new MyIterator(this);
        }
    }

    public class MyIterator : Iterator
    {
        int idx;
        MyAggregate agg;

        public MyIterator(MyAggregate a)
        {
            agg = a;
        }

        public void FirstItem()
        {
            idx = 0;
        }

        public void NextItem()
        {
            if (!isDone()) idx++;
        }

        public bool isDone()
        {
            return idx > 2;
        }

        public int currentItem()
        {
            if (!isDone())
                return agg.getItem(idx);
            else
                return 0;
        }

    }

    public class AggregateClient
    {
        IAggregate ia;

        public AggregateClient(IAggregate a)
        {
            ia = a;
        }

        public int getTotal()
        {
            int res = 0;
            Iterator it = ia.CreateIterator();
            for (it.FirstItem(); !it.isDone(); it.NextItem())
            {
                res += it.currentItem();
            }
            return res;
        }
    }

    // А теперь - альтернатива :

    public class MyEnumAggregate : IEnumerable
    {
        int[] arr;

        public MyEnumAggregate()
        {
            arr = new int[3] { 1, 5, 7 };
        }

        public int getItem(int idx)
        {
            return arr[idx];
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MyEnumerator(this);
        }

        #endregion
    }


    public class MyEnumerator : IEnumerator
    {
        int idx;
        MyEnumAggregate agg;

        public MyEnumerator(MyEnumAggregate a)
        {
            agg = a;
            Reset();
        }

        public bool isDone()
        {
            return idx > 2;
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get
            {
                if (!isDone())
                    return agg.getItem(idx);
                else
                    return 0;
            }
        }

        public bool MoveNext()
        {
           if (!isDone()) 
               idx++;
           return !isDone();
        }

        public void Reset()
        {
            idx = -1;
        }

        #endregion
    }


    public class EnumerableClient
    {
        IEnumerable ie;

        public EnumerableClient(IEnumerable e)
        {
            ie = e;
        }

        public int getTotal()
        {
            int res = 0;
            foreach (int i in ie)
                res += i;
            return res;
        }

    }

}
