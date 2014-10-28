using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threads.Bussiness
{
    public class SemaphoreAndMutex
    {
        public static Semaphore semaphore = new Semaphore(0, 10);
        public static int time = 0;

        /// <summary>
        /// 信号量（Semaphore）是由内核对象维护的int变量，当信号量为0时，在信号量上等待的线程会堵塞，信号量大于0时，就解除堵塞。当在一个信号量上等待的线程解除堵塞时，内核自动会将信号量的计数减1。在.net 下通过Semaphore类来实现信号量同步。
        /// Semaphore类限制可同时访问某一资源或资源池的线程数。线程通过调用 WaitOne方法将信号量减1，并通过调用 Release方法把信号量加1。
        /// </summary>
        public static void Client1()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread test = new Thread(new ParameterizedThreadStart(TestMethod));

                // 开始线程，并传递参数
                test.Start(i);
            }

            // 等待1秒让所有线程开始并阻塞在信号量上
            Thread.Sleep(500);

            // 信号量计数加4
            // 最后可以看到输出结果次数为4次
            semaphore.Release(4);
            Console.Read();
        }

        public static void TestMethod(object number)
        {
            // 设置一个时间间隔让输出有顺序
            int span = Interlocked.Add(ref time, 100);
            Thread.Sleep(1000 + span);

            //信号量计数减1
            semaphore.WaitOne();

            Console.WriteLine("Thread {0} run ", number);
        }


        // 初始信号量计数为4，最大计数为10
        public static Semaphore semaphore2 = new Semaphore(4, 10, "My");
        public static int time2 = 0;
        public static void Client2()
        {
            for (int i = 0; i < 3; i++)
            {
                Thread test = new Thread(new ParameterizedThreadStart(TestMethod2));

                // 开始线程，并传递参数
                test.Start(i);
            }

            // 等待1秒让所有线程开始并阻塞在信号量上
            Thread.Sleep(1000);

            Console.Read();
        }

        public static void TestMethod2(object number)
        {
            // 设置一个时间间隔让输出有顺序
            int span = Interlocked.Add(ref time2, 500);
            Thread.Sleep(1000 + span);

            //信号量计数减1
            semaphore2.WaitOne();

            Console.WriteLine("Thread {0} run ", number);
        }


        public static Mutex mutex = new Mutex();
        public static int count;
        /// <summary>
        /// 同样互斥体也是同样可以实现线程之间的同步和不同进程中线程的同步的
        /// </summary>
        public static void Client3()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread test = new Thread(TestMethod3);

                // 开始线程，并传递参数
                test.Start();
            }

            Console.Read();
        }

        public static void TestMethod3()
        {
            mutex.WaitOne();
            Thread.Sleep(500);
            count++;
            Console.WriteLine("Current Cout Number is {0}", count);
            mutex.ReleaseMutex();
        }


        public static Mutex mutex2 = new Mutex(false, "My4");
        //public static Mutex mutex2 = new Mutex();
        public static void Client4()
        {
            Console.WriteLine("Main start at : " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(TestMethod4);
            t.Start();

            Console.Read();
        }

        public static void TestMethod4()
        {
            mutex2.WaitOne();
            Thread.Sleep(5000);
            Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());
            mutex2.ReleaseMutex();
        }
    }
}
