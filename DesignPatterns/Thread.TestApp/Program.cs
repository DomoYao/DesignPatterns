using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            #region 线程同步

            #endregion

           
        }

    }
}
