using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threads.Bussiness
{
    public class ThreadOfEvent
    {

        // 初始化自动重置事件，并把状态设置为非终止状态
        // 如果这里把初始状态设置为True时，
        // 当调用WaitOne方法时就不会阻塞线程,看到的输出结果的时间就是一样的了
        // 因为设置为True时，表示此时已经为终止状态了。       
        public static AutoResetEvent autoEvent = new AutoResetEvent(false);

        /// <summary>
        /// 线程通过调用AutoResetEvent的WaitOne方法来等待信号，如果AutoResetEvent对象为非终止状态，则线程被阻止，等到线程调用Set方法来恢复线程执行。
        /// 如果AutoResetEvent为终止状态时，则线程不会被阻止，此时AutoResetEvent将立即释放线程并返回为非终止状态（指出有线程在使用资源的一种状态）。
        /// </summary>
        public static void Client1()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(TestMethod);
            t.Start();

            // 阻塞主线程3秒后
            // 调用 Set方法释放线程，使线程t可以运行
            Thread.Sleep(3000);

            // Set 方法就是把事件状态设置为终止状态。
            autoEvent.Set();
            Console.Read();
        }

        public static void TestMethod()
        {
            //没有带参数的WaitOne方法，该方法表示无限制阻塞线程，直到收到一个事件为止（通过Set方法来发送一个信号）
            autoEvent.WaitOne();

            // 3秒后线程可以运行，所以此时显示的时间应该和主线程显示的时间相差3秒
            Console.WriteLine("Method Restart run at: " + DateTime.Now.ToLongTimeString());
        }


        /// <summary>
        /// 我们也可以设置堵塞线程的事件，当超时时，线程将不阻塞直接运行（尽管此时没有通过Set来发送一个信号，线程照样运行，只是WaitOne方法返回的的值不一样）。
        ///bool WaitOne(int millisecondsTimeout) 收到信号时返回为True,没收到信号返回为false。
        /// </summary>
        public static void Client2()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(TestMethod2);
            t.Start();

            // 阻塞主线程1秒后
            // 调用 Set方法释放线程，使线程t可以运行
            //Thread.Sleep(3000);
            Thread.Sleep(1000); //修改成这个看看

            // Set 方法就是把事件状态设置为终止状态。
            autoEvent.Set();
            Console.Read();
        }

        public static void TestMethod2()
        {
            if (autoEvent.WaitOne(2000))
            {
                Console.WriteLine("Get Singal to Work");
                // 3秒后线程可以运行，所以此时显示的时间应该和主线程显示的时间相差一秒
                Console.WriteLine("Method Restart run at: " + DateTime.Now.ToLongTimeString());
            }
            else
            {
                Console.WriteLine("Time Out to work");
                Console.WriteLine("Method Restart run at: " + DateTime.Now.ToLongTimeString());
            }
        }


        //ManualResetEvent的使用和AutoResetEvent的使用很类似，因为他们都是从EventWaitHandle类派生的，不过他们还是有点区别:
        //AutoResetEvent 为终止状态时线程调用 WaitOne，则线程不会被阻止。AutoResetEvent 将立即释放线程并返回到非终止状态,当再次调用WaitOne状态时线程会被阻止
        //这里请注意如果AutoResetEvent初始为非终止状态时， 调用WaitOne(int millisecondsTimeout)方法后并不会把状态返回为终止状态，此时还是非终止的，调用WaitOne方法自动改变状态只针对初始状态为终止状态时有效。
        //然而ManualResetEvent初始状态为终止状态时时调用WaitOne，则线程同样不会被阻止，但是ManualResetEvent的状态不会发生改变（当我再次调用WaitOne方法是一样不会阻止线程），需要我们手动终止()
        // 初始化自动重置事件，并把状态设置为终止状态
        //public static AutoResetEvent autoEvent2 = new AutoResetEvent(true); // 比较ManualResetEvent区别

        public static ManualResetEvent autoEvent2 = new ManualResetEvent(true); 
        
        public static void Client3()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(TestMethod3);
            t.Start();
            Console.Read();
        }

        public static void TestMethod3()
        {
            // 初始状态为终止状态，则第一次调用WaitOne方法不会堵塞线程
            // 此时运行的时间间隔应该为0秒，但是因为是AutoResetEvent对象
            // 调用WaitOne方法后立即把状态返回为非终止状态。
            autoEvent2.WaitOne();
            Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());

            // 因为此时AutoRestEvent为非终止状态，所以调用WaitOne方法后将阻塞线程1秒，这里设置了超时时间
            // 所以下面语句的和主线程中语句的时间间隔为1秒
            // 当时 ManualResetEvent对象时，因为不会自动重置状态
            // 所以调用完第一次WaitOne方法后状态仍然为非终止状态,所以再次调用不会阻塞线程，所以此时的时间间隔也为0
            // 如果没有设置超时时间的话，下面这行语句将不会执行
            autoEvent2.WaitOne(3000);
            Console.WriteLine("方法 开始 : " + DateTime.Now.ToLongTimeString());


        }

        //public static EventWaitHandle autoEvent3 = new EventWaitHandle(true, EventResetMode.AutoReset, "My");
        public static EventWaitHandle autoEvent3 = new EventWaitHandle(false, EventResetMode.AutoReset, "My");
        public static void Client4()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(TestMethod4);

            // 为了有时间启动另外一个线程
            Thread.Sleep(2000);
            t.Start();
            Console.Read();
        }

        public static void TestMethod4()
        {
            // 进程一：显示的时间间隔为2秒
            // 进程二中显示的时间间隔为3秒
            // 因为进程二中AutoResetEvent的初始状态为非终止的
            // 因为在进程一中通过WaitOne方法的调用已经把AutoResetEvent的初始状态返回为非终止状态了
            autoEvent3.WaitOne(1000);
            Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());
        }
    }
}
