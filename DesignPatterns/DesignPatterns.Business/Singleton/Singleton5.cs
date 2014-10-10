using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.Singleton5
{
        /// <summary>
        /// 使用注册表机制创建和查询 Singleton 类的子类实例。
        ///如果系统中定义了多个 Singleton 的子类，可以实现一个注册表机制，用于存储子类的映射。
        /// </summary>
        public class Singleton
        {
            private static Dictionary<string, Singleton> _registry
              = new Dictionary<string, Singleton>();
            private static Singleton _instance;

            // the constructor should be protected or private
            protected Singleton()
            {
            }

            public static Singleton Instance(string name)
            {
                if (!_registry.ContainsKey(name))
                {
                    if (name == "Apple")
                    {
                        _registry.Add(name, new AppleSingleton());
                    }
                    else if (name == "Orange")
                    {
                        _registry.Add(name, new OrangeSingleton());
                    }
                }

                return _registry[name];
            }
        }

        public class AppleSingleton : Singleton
        {
        }

        public class OrangeSingleton : Singleton
        {
        }
    
}
