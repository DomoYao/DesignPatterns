using System;
using System.Threading;
using Threads.Bussiness;

namespace Threads.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1、线程的初步认识
            //前后台线程测试
            //SimpleThread.Client1();

            //利用Join().主线程在后台线程执行完结束
            //SimpleThread.Client2();

            //利用ParameterizedThreadStart创建线程
            //SimpleThread.Client3();

            // 线程Abort，终止不能唤醒
            //SimpleThread.Client4();

            // 线程Interrupt，终止可以重新唤醒
            //SimpleThread.Client5();
            #endregion

            #region 2.线程池中的工作者线程

            // 创建工作者线程的方法
            //OneStepThread.Client1();

            // 协作式取消
            //OneStepThread.Client2();

            // 利用委托创建线程
            //OneStepThread.Client3();

            // 使用任务来(Task)实现异步
            // OneStepThread.Client4();

            // CancellationTokenSource对象来取消一个Task.
            // OneStepThread.Client5();

            // 任务工厂
            //OneStepThread.Client6();
            #endregion

            #region 3、线程池中的I/O线程
            // 异步写入文件
            //AsyncFile.Client1();

            // 异步读取文件
            //AsyncFile.Client2();

            // 异步请求
            //AsyncFile.Client3();
            #endregion

            #region 4、线程同步
            // 使用锁的性能影响
            //Synchronization.Client1();

            // 不使用锁，数据不是我们想要的
            //Synchronization.Client2();

            // 使用锁
            //Synchronization.Client3();

            // Monitor实现线程同步
            //Synchronization.Client4();

            // ReaderWriterLock实现线程同步
            //Synchronization.Client5();
           
            #endregion

            #region 5、线程同步——事件构造
            //AutoResetEvent （自动重置事件） 
            //ThreadOfEvent.Client1();
            //ThreadOfEvent.Client2();

            // ManualResetEvent(手动重置事件)
            //ThreadOfEvent.Client3();

            // 跨进程之间同步
            //ThreadOfEvent.Client4();
            #endregion

            #region 6、线程同步——信号量和互斥体
            // 信号量（Semaphore）
            //SemaphoreAndMutex.Client1();
            //SemaphoreAndMutex.Client2();

            // 互斥体（Mutex）
            //SemaphoreAndMutex.Client3();
            SemaphoreAndMutex.Client4();
            #endregion

        }

       
    }
}
