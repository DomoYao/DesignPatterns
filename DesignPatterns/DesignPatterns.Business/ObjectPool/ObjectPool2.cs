using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//实现方式（二）：使用对象构造方法和预分配方式实现 ObjectPool 类。

namespace DesignPatterns.Business.ObjectPool
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public class ObjectPool<T> where T : class
    {
        private readonly Func<T> _objectFactory;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        /// <summary>
        /// 对象池
        /// </summary>
        /// <param name="objectFactory">构造缓存对象的函数</param>
        public ObjectPool(Func<T> objectFactory)
        {
            _objectFactory = objectFactory;
        }

        /// <summary>
        /// 构造指定数量的对象
        /// </summary>
        /// <param name="count">数量</param>
        public void Allocate(int count)
        {
            for (int i = 0; i < count; i++)
                _queue.Enqueue(_objectFactory());
        }

        /// <summary>
        /// 缓存一个对象
        /// </summary>
        /// <param name="obj">对象</param>
        public void Enqueue(T obj)
        {
            _queue.Enqueue(obj);
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <returns>对象</returns>
        public T Dequeue()
        {
            T obj;
            return !_queue.TryDequeue(out obj) ? _objectFactory() : obj;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var pool = new ObjectPool<byte[]>(() => new byte[65535]);
            pool.Allocate(1000);

            var buffer = pool.Dequeue();

            // .. do something here ..

            pool.Enqueue(buffer);
        }
    }
}
