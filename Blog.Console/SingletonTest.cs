using System;
namespace Blog.Console
{
    public class SingletonTest
    {
        public static SingletonTest singletonTest = new SingletonTest();
        public static int Number = 0;
        private SingletonTest()
        {
            Number = 3;
        }
        public static int GetNum()
        {
            return Number;
        }
        public static SingletonTest GetSingletonTest()
        {
            return singletonTest;
        }
    }
}
