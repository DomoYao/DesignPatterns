using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.Singleton
{
    /// <summary>
    /// 使用 Static 变量初始化 Singleton。在类加载时即创建实例。缺点是无论使用与否实例均被创建
    /// </summary>
    public class Singleton
    {
        private static readonly Singleton _instance = new Singleton();

        // the constructor should be protected or private
        protected Singleton()
        {
        }

        public static Singleton Instance()
        {
            return _instance;
        }
    }
}
