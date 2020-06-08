using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Blog.Console
{
    class TestModel
    {
        string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<TestModel> nums_1 = new List<TestModel>();
            List<TestModel> nums_2 = new List<TestModel>();
            System.Console.WriteLine("第一种方法开始:");
            TestModel model = null;
            for (int i = 0; i < 1000000; i++)
            {
                nums_1.Add(model);
            }
            sw.Stop();
            System.Console.WriteLine("程序结束，用时:" + (sw.ElapsedMilliseconds));
            sw.Restart();
            System.Console.WriteLine("第二种方法开始:");
            for (int i = 0; i < 1000000; i++)
            {
                TestModel t = new TestModel();
                nums_2.Add(t);
            }
            sw.Stop();
            //System.out.println(nums_2);
            System.Console.WriteLine("程序结束，用时:" + (sw.ElapsedMilliseconds));
        }
    }
}
