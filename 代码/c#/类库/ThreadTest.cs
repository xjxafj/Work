using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using MagicKit.BLL;

namespace 线程同步
{
    class Program
    {
        static int num = 1;
        static void Main(string[] args)
        {

            var tt = RegistryHelper.GetProductCode("SDL Trados Studio 2019");
            //test1();
            //List<TestClass> list = new List<TestClass>();
            //list.Add(new TestClass() { Name="fff"});
            //for (int i = 0; i < 100; i++)
            //{
            //    Test4(list);
            //    Thread.Sleep(1000);
            //}
            
            //Console.ReadLine();
        }



        /// <summary>
        /// Invoke方式一 action
        /// </summary>
        public static void Test4(List<TestClass> list)
        {
            Stopwatch stopWatch = new Stopwatch();

            Console.WriteLine("主线程：{0}线程ID ： {1}；开始", "Client1", Thread.CurrentThread.ManagedThreadId);
            stopWatch.Start();

            try
            {
                Parallel.Invoke(() => Task1(list), () => Task2(list), () => Task3(list), delegate ()
                {
                    //throw new Exception("我这里发送了异常");
                });
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                    Console.WriteLine(ex.Message);
            }

            stopWatch.Stop();
            Console.WriteLine("主线程：{0}线程ID ： {1}；结束,共用时{2}ms", "Client1", Thread.CurrentThread.ManagedThreadId, stopWatch.ElapsedMilliseconds);
        }



        //线程顺序执行方法1

        static void test1()
        {
            Action act = () =>
            {
                num++;
                Console.WriteLine(num);
            };

            Action act1 = () =>
            {
                Console.WriteLine("this is 1");

            };


            Action act2 = () =>
            {

                Console.WriteLine("this is 2");
            };


            Action act3 = () =>
            {
               
                Console.WriteLine("this is 3");
                Thread.Sleep(10000);
            };


            Action<string> act4 = new Action<string>(Book);
            //{
            //    Console.WriteLine("this is 3");
            //};



            Task.Factory.StartNew(act3).ContinueWith(o=> act2()).ContinueWith(o => act2());

            Console.ReadLine();
        }

        private static void Book(string obj)
        {
            throw new NotImplementedException();
        }


        public static void Test3()
        {

            Stopwatch stopWatch = new Stopwatch();

            Console.WriteLine("主线程：{0}线程ID ： {1}；开始", "Client1", Thread.CurrentThread.ManagedThreadId);
            stopWatch.Start();
            //Parallel.Invoke(() => Task1("task1"), () => Task2("task2"), () => Task3("task3"));
            stopWatch.Stop();
            Console.WriteLine("主线程：{0}线程ID ： {1}；结束,共用时{2}ms", "Client1", Thread.CurrentThread.ManagedThreadId, stopWatch.ElapsedMilliseconds);
            Console.WriteLine("tet");
            Console.ReadLine();
        }

        



        private static void Task1(List<TestClass> list)
        {
            //Thread.Sleep(5000);
            for (int i = 0; i < 100; i++)
            {
                foreach (var item in list)
                {
                    item.Name="task1";
                }
            }
            Console.WriteLine("任务名：{0}线程ID ： {1}", "task1", Thread.CurrentThread.ManagedThreadId);
        }

        private static void Task2(List<TestClass> list)
        {
            for (int i = 0; i < 100; i++)
            {
                foreach (var item in list)
                {
                    item.Name = "task2";
                }
            }
            Console.WriteLine("任务名：{0}线程ID ： {1}", "task2", Thread.CurrentThread.ManagedThreadId);
        }

        private static void Task3(List<TestClass> list)
        {
            for (int i = 0; i < 100; i++)
            {
                foreach (var item in list)
                {
                    item.Name = "task3";
                }
            }
            Console.WriteLine("任务名：{0}线程ID ： {1}", "task3", Thread.CurrentThread.ManagedThreadId);
        }






        //线程顺序执行方法2
        static void test2()
        {
            Action act1 = () =>
            {
                Console.WriteLine("this is 1");

            };


            Action act2 = () =>
            {

                Console.WriteLine("this is 2");
            };


            Action act3 = () =>
            {

                Console.WriteLine("this is 3");
            };

            for (int i = 0; i < 100; i++)
            {
                Thread t1 = new Thread(new ThreadStart(act1));
                Thread t2 = new Thread(new ThreadStart(act2));
                Thread t3 = new Thread(new ThreadStart(act3));
                t1.IsBackground = true;
                t1.Priority = ThreadPriority.Lowest;
                t1.Start();
                t3.Start();
                t2.Start();
                t1.Join();
                Thread.Sleep(1000);
            }
           


           
           
            //t1.Join();
            //t1.Join();
            //t3.Join(); //主线程停止,执行t3
            //t2.Join(); //主线程停止,执行t2
            Console.ReadLine();
        }

    }

    public  class TestClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }


}
