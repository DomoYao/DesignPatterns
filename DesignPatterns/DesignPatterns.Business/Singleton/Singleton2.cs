using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.Singleton2
{
    /// <summary>
    /// 使用 Lazy Initialization 来实现 Singleton。
    ///通常将创建类的唯一实例的操作隐藏在一个类操作后面，由它保证只有一个实例被创建。这个操作可以访问保存唯一实例的变量，保证在它的首次使用前被创建和初始化。
    /// </summary>
    public class Singleton
    {
        private static Singleton _instance;

        // the constructor should be protected or private
        protected Singleton()
        {
        }

        public static Singleton Instance()
        {
            if (_instance == null)
            {
                // use lazy initialization
                _instance = new Singleton();
            }

            return _instance;
        }
    }

}
