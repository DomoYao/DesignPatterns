using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threads.Bussiness
{
    /// <summary>
    /// 线程同步
    /// </summary>
    public class Synchronization
    {
        /// <summary>
        /// 使用锁性能的影响
        /// 1. 它的使用比较繁琐。因为我们要用额外的代码把多个线程同时访问的数据包围起来，并获取和释放一个线程同步锁，如果我们在一个代码块忘记获取锁，就有可能造成数据损坏。
        /// 2. 使用线程同步会影响性能，获取和释放一个锁肯定是需要时间的吧，因为我们在决定哪个线程先获取锁时候， CPU必须进行协调，进行这些额外的工作就会对性能造成影响
        /// 3. 因为线程同步一次只允许一个线程访问资源，这样就会阻塞线程，阻塞线程会造成更多的线程被创建，这样CPU就有可能要调度更多的线程，同样也对性能造成了影响。
        /// </summary>
        public static void Client1()
        {
            int x = 0;
            // 迭代次数为500万
            const int iterationNumber = 5000000;
            // 不采用锁的情况
            // StartNew方法 对新的 Stopwatch 实例进行初始化，将运行时间属性设置为零，然后开始测量运行时间。
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterationNumber; i++)
            {
                x++;
            }

            Console.WriteLine("Use the all time is :{0} ms", sw.ElapsedMilliseconds);

            sw.Restart();
            // 使用锁的情况
            for (int i = 0; i < iterationNumber; i++)
            {
                Interlocked.Increment(ref x);
            }

            Console.WriteLine("Use the all time is :{0} ms", sw.ElapsedMilliseconds);
            Console.Read();
        }

        /// <summary>
        /// 不使用锁
        /// </summary>
        public static void Client2()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread testthread = new Thread(Add);
                testthread.Start();
            }

            Console.Read();
        }

        // 共享资源
        public static int Number = 0;

        public static void Add()
        {
            Thread.Sleep(500);
            Console.WriteLine("the current value of number is:{0}", ++Number);
        }

        public static int Number1 = 0;
        public static void Add2()
        {
            Thread.Sleep(500);
            Console.WriteLine("使用锁的值:{0}", Interlocked.Increment(ref Number1));
        }

        /// <summary>
        /// 使用锁
        /// </summary>
        public static void Client3()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread testthread = new Thread(Add2);
                testthread.Start();
            }

            Console.Read();
        }

        /// <summary>
        /// Monitor实现线程同步
        /// </summary>
        public static void Client4()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread testthread = new Thread(Add4);
                testthread.Start();
            }

            Console.Read();
        }

        // 共享资源
        public static int number = 1;
        private static object lockobj = new object();
        /// <summary>
        /// Monitor.Enter()Monitor.Exit() ==Lock()
        /// </summary>
        public static void Add4()
        {
            Thread.Sleep(1000);
            //获得排他锁
            Monitor.Enter(lockobj);

            Console.WriteLine("the current value of number is:{0}", number++);

            // 释放指定对象上的排他锁。
            Monitor.Exit(lockobj);
        }


        public static List<int> lists = new List<int>();
        // 创建一个对象
        public static ReaderWriterLock readerwritelock = new ReaderWriterLock();
        /// <summary>
        /// ReaderWriterLock实现线程同步
        /// </summary>
        public static void Client5()
        {
            //创建一个线程读取数据
            Thread t1 = new Thread(Write);
            t1.Start();
            // 创建10个线程读取数据
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(Read);
                t.Start();
            }

            Console.Read();

        }

        // 写入方法
        public static void Write()
        {
            // 获取写入锁，以10毫秒为超时。
            readerwritelock.AcquireWriterLock(10);
            Random ran = new Random();
            int count = ran.Next(1, 10);
            lists.Add(count);
            Console.WriteLine("Write the data is:" + count);
            // 释放写入锁
            readerwritelock.ReleaseWriterLock();
        }

        // 读取方法
        public static void Read()
        {
            // 获取读取锁
            readerwritelock.AcquireReaderLock(10);

            foreach (int li in lists)
            {
                // 输出读取的数据
                Console.WriteLine(li);
            }

            // 释放读取锁
            readerwritelock.ReleaseReaderLock();
        }
    }
}
