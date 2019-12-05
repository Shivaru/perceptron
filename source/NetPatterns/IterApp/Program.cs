using System;
using System.Collections.Generic;
using System.Text;
using NetPatterns;

namespace IterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new AggregateClient(new MyAggregate()).getTotal());
            Console.WriteLine(new EnumerableClient(new MyEnumAggregate()).getTotal());
        }
    }
}
