using System;
using System.Threading;

namespace Blog.Console
{
    class Program
    {
        static bool flag = false;
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                System.Console.WriteLine("start....");
                System.Console.WriteLine("子线程ID:" + Thread.CurrentThread.ManagedThreadId.ToString());
                Thread.Sleep(1000);
                flag = true;
                System.Console.WriteLine("end....flag="+flag);
            }).Start();

            while (true)
            {
                if (flag)
                {
                    System.Console.WriteLine("主线程ID:" + Thread.CurrentThread.ManagedThreadId.ToString());
                }
            }


        }

    }
}
