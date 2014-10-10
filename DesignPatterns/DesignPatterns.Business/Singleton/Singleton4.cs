using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.Singleton4
{
    /// <summary>
    /// Singleton 的实现如果需要保证线程安全性，则可以使用 Double-Check Locking 技术。
    /// </summary>
    public class Singleton
    {
        private static Singleton _instance;
        private static readonly object SyncRoot = new object();

        // the constructor should be protected or private
        protected Singleton()
        {
        }

        public static Singleton Instance()
        {
            // double-check locking
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        // use lazy initialization
                        _instance = new Singleton();
                    }
                }
            }

            return _instance;
        }
    }


}
