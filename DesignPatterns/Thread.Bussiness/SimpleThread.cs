using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threads.Bussiness
{
    /// <summary>
    /// 简单线程的使用
    /// </summary>
    public class SimpleThread
    {
        private static void Worker(object msg)
        {
            // 模拟做5秒
            Thread.Sleep(5000);


            // 下面语句，只有由一个前台线程执行时，才会显示出来
            Console.WriteLine(msg+"Return from Worker Thread");
        }

        /// <summary>
        /// 前后台线程
        /// </summary>
        public static void Client1()
        {
            // 创建一个新线程（默认为前台线程）
            Thread backthread = new Thread(Worker);

            // 使线程成为一个后台线程
            //backthread.IsBackground = true; //线程为后台线程，当主线程（执行Main方法的线程，主线程当然也是前台线程了）结束运行后.索引看不到输出结果.
            backthread.IsBackground = false;

            // 通过Start方法启动线程
            backthread.Start("Hi,");

            // 如果backthread是前台线程，则应用程序大约5秒后才终止
            // 如果backthread是后台线程，则应用程序立即终止
            Console.WriteLine("Return from Main Thread");
        }

        /// <summary>
        /// Join()方法能保证主线程（前台线程）在异步线程thread（后台线程）运行结束后才会运行
        /// </summary>
        public static void Client2()
        {
            // 创建一个新线程（默认为前台线程）
            Thread backthread = new Thread(Worker);
            // 使线程成为一个后台线程
            backthread.IsBackground = true;

            // 通过Start方法启动线程
            backthread.Start("Hello,");
            backthread.Join();

            // 模拟主线程的输出
            Thread.Sleep(2000);

            Console.WriteLine("Return from Main Thread");
            Console.Read();
        }

        /// <summary>
        /// 以ParameterizedThreadStart委托的方式来实现多线程
        /// </summary>
        public static void Client3()
        {
            // 创建一个新线程（默认为前台线程）
            Thread backthread = new Thread(new ParameterizedThreadStart(Worker));

            // 通过Start方法启动线程
            backthread.Start("你好,");

            
            Console.WriteLine("Return from Main Thread");
            Console.Read();
        }

        /// <summary>
        /// 线程Abort
        /// Abort方法和Interrupt都是用来终止线程的，但是两者还是有区别的。
        /// 1、他们抛出的异常不一样，Abort 方法抛出的异常是ThreadAbortException， Interrupt抛出的异常为ThreadInterruptedException
        /// 2、调用interrupt方法的线程之后可以被唤醒，然而调用Abort方法的线程就直接被终止不能被唤醒的。
        /// </summary>
        public static void Client4()
        {
            Thread abortThread = new Thread(AbortMethod);
            abortThread.Name = "Abort Thread";
            abortThread.Start();
            Thread.Sleep(1000);
            try
            {
                abortThread.Abort();
            }
            catch
            {
                Console.WriteLine("{0} Exception happen in Main Thread", Thread.CurrentThread.Name);
                Console.WriteLine("{0} Status is:{1} In Main Thread ", Thread.CurrentThread.Name, Thread.CurrentThread.ThreadState);
            }
            finally
            {
                Console.WriteLine("{0} Status is:{1} In Main Thread ", abortThread.Name, abortThread.ThreadState);
            }

            abortThread.Join();
            Console.WriteLine("{0} Status is:{1} ", abortThread.Name, abortThread.ThreadState);
            Console.Read();

        }

        /// <summary>
        /// 线程Interrupt
        /// Abort方法和Interrupt都是用来终止线程的，但是两者还是有区别的。
        /// 1、他们抛出的异常不一样，Abort 方法抛出的异常是ThreadAbortException， Interrupt抛出的异常为ThreadInterruptedException
        /// 2、调用interrupt方法的线程之后可以被唤醒，然而调用Abort方法的线程就直接被终止不能被唤醒的。
        /// </summary>
        public static void Client5()
        {
            Thread interruptThread = new Thread(AbortMethod);
            interruptThread.Name = "Interrupt Thread";
            interruptThread.Start();
            interruptThread.Interrupt();

            interruptThread.Join();
            Console.WriteLine("{0} Status is:{1} ", interruptThread.Name, interruptThread.ThreadState);
            Console.Read(); 

        }

        private static void AbortMethod()
        {
            //在线程执行的方法中运用循环，如果线程可以唤醒，则输出结果中就一定会有循环的部分，然而调用Abort方法线程就直接终止，就不会有循环的部分
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("Thread is Running");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetType().Name);
                    Console.WriteLine("{0} Exception happen In Abort Thread", Thread.CurrentThread.Name);
                    Console.WriteLine("{0} Status is:{1} In Abort Thread ", Thread.CurrentThread.Name,
                                      Thread.CurrentThread.ThreadState);
                }
                finally
                {
                    Console.WriteLine("{0} Status is:{1} In Abort Thread", Thread.CurrentThread.Name,
                                      Thread.CurrentThread.ThreadState);
                }
            }
        }
    }
}
