using System;

namespace Blog.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var s1=SingletonTest.GetSingletonTest();
            var s2 = SingletonTest.GetSingletonTest();
            //SingletonTest s4 = new SingletonTest();
            //SingletonTest s5 = new SingletonTest();
            System.Console.WriteLine(s1 == s2);
            //System.Console.WriteLine(s5 == s4);
            System.Console.ReadKey();
        }
    }
}
