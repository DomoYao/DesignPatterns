using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.Singleton3
{
    /// <summary>
    /// 可以使用 Reset 操作来将已创建的实例销毁掉
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

        public void Reset()
        {
            _instance = null;
        }
    }


}
