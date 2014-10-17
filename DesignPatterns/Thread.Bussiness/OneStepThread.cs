using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threads.Bussiness
{
    /// <summary>
    /// 线程池中的工作者线程
    /// </summary>
    public class OneStepThread
    {
        /// <summary>
        /// 创建工作者线程的方法
        /// </summary>
        /// public static bool QueueUserWorkItem (WaitCallback callBack);
        /// public static bool QueueUserWorkItem(WaitCallback callback, Object state);
        /// 这两个方法向线程池的队列添加一个工作项（work item）以及一个可选的状态数据。然后，这两个方法就会立即返回。
        /// 工作项其实就是由callback参数标识的一个方法，该方法将由线程池线程执行。同时写的回调方法必须匹配System.Threading.WaitCallback委托类型，定义为：
        /// public delegate void WaitCallback(Object state);
        public static void Client1()
        {
            // 设置线程池中处于活动的线程的最大数目
            // 设置线程池中工作者线程数量为1000，I/O线程数量为1000
            ThreadPool.SetMaxThreads(1000, 1000);
            Console.WriteLine("主线程: queue an asynchronous method");
            PrintMessage("主线程开始...");

            // 把工作项添加到队列中，此时线程池会用工作者线程去执行回调方法
            ThreadPool.QueueUserWorkItem(AsyncMethod);
            Console.Read();
        }

        // 方法必须匹配WaitCallback委托
        private static void AsyncMethod(object state)
        {
            Thread.Sleep(1000);
            PrintMessage("子线程开始...");
            Console.WriteLine("子线程 has worked ");
        }

        // 打印线程池信息
        private static void PrintMessage(String data)
        {
            int workthreadnumber;
            int iothreadnumber;

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);

            Console.WriteLine("{0}\n CurrentThreadId is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
                data,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.IsBackground.ToString(),
                workthreadnumber.ToString(),
                iothreadnumber.ToString());
        }

        /// <summary>
        /// 取消操作 为了取消一个操作，首先必须创建一个System.Threading.CancellationTokenSource对象。
        /// 下面代码演示了协作式取消的使用，主要实现当用户在控制台敲下回车键后就停止数数方法。
        /// </summary>
        public static void Client2()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            Console.WriteLine("Main thread run");
            PrintMessage("Start");
            Run();
            Console.ReadKey();
        }

        private static void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            // 这里用Lambda表达式的方式和使用委托的效果一样的，只是用了Lambda后可以少定义一个方法。
            // 这在这里就是让大家明白怎么lambda表达式如何由委托转变的
            ////ThreadPool.QueueUserWorkItem(o => Count(cts.Token, 1000));

            ThreadPool.QueueUserWorkItem(Callback, cts.Token);

            Console.WriteLine("Press Enter key to cancel the operation\n");
            Console.ReadLine();

            // 传达取消请求
            cts.Cancel();
        }

        private static void Callback(object state)
        {
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method Start");
            CancellationToken token = (CancellationToken)state;
            Count(token, 1000);
        }

        // 执行的操作，当受到取消请求时停止数数
        private static void Count(CancellationToken token, int countto)
        {
            for (int i = 0; i < countto; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Count is canceled");
                    break;
                }

                Console.WriteLine(i);
                Thread.Sleep(300);
            }

            Console.WriteLine("Cout has done");
        }


        private delegate string MyTestdelegate(string state);
        /// <summary>
        /// 使用委托实现多线程
        /// </summary>
        public static void Client3()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            PrintMessage("Main Thread Start");

            //实例化委托
            MyTestdelegate testdelegate = new MyTestdelegate(AsyncMethod);

            // 异步调用委托
            IAsyncResult result = testdelegate.BeginInvoke("Hi,",null, null);

            // 获取结果并打印出来
            string returndata = testdelegate.EndInvoke(result);
            Console.WriteLine(returndata);

            Console.ReadLine();
        }

        private static string AsyncMethod(string state)
        {
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method");
            return state+"Method has completed";
        }

        /// <summary>
        /// 使用任务来实现异步
        /// </summary>
        public static void Client4()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            PrintMessage("Main Thread Start");
            // 调用构造函数创建Task对象,
            Task<int> task = new Task<int>(n => AsyncMethod((int)n), 10);

            // 启动任务 
            task.Start();
            // 等待任务完成
            task.Wait();
            Console.WriteLine("The Method result is: " + task.Result);

            Console.ReadLine();
        }

        private static int AsyncMethod(int n)
        {
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method");

            int sum = 0;
            for (int i = 1; i < n; i++)
            {
                // 如果n太大，使用checked使下面代码抛出异常
                checked
                {
                    sum += i;
                }
            }

            return sum;
        }

        /// <summary>
        /// 取消任务
        /// 如果要取消任务，同样可以使用一个CancellationTokenSource对象来取消一个Task.
        /// </summary>
        public static void Client5()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            PrintMessage("Main Thread Start");
            CancellationTokenSource cts = new CancellationTokenSource();

            // 调用构造函数创建Task对象,将一个CancellationToken传给Task构造器从而使Task和CancellationToken关联起来
            Task<int> task = new Task<int>(n => AsyncMethod(cts.Token, (int)n), 10);

            // 启动任务 
            task.Start();

            // 延迟取消任务
            Thread.Sleep(3000);

            // 取消任务
            cts.Cancel();
            Console.WriteLine("The Method result is: " + task.Result);
            Console.ReadLine();
        }

        private static int AsyncMethod(CancellationToken ct, int n)
        {
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method");

            int sum = 0;
            try
            {
                for (int i = 1; i < n; i++)
                {
                    // 当CancellationTokenSource对象调用Cancel方法时，
                    // 就会引起OperationCanceledException异常
                    // 通过调用CancellationToken的ThrowIfCancellationRequested方法来定时检查操作是否已经取消，
                    // 这个方法和CancellationToken的IsCancellationRequested属性类似
                    ct.ThrowIfCancellationRequested();
                    Thread.Sleep(500);
                    // 如果n太大，使用checked使下面代码抛出异常
                    checked
                    {
                        sum += i;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception is:" + e.GetType().Name);
                Console.WriteLine("Operation is Canceled");
            }

            return sum;
        }

        /// <summary>
        /// 任务工厂,同样可以通过任务工厂TaskFactory类型来实现异步操作。
        /// </summary>
        public static void Client6()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            Task.Factory.StartNew(() => PrintMessage("Main Thread"));
            Console.Read();
        }
    }
}
