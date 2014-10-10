using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._2
{
    /// <summary>
    /// 泛型实现工厂方法
    /// </summary>
    public class LogCreator<T> : ILogCreator where T : ILog, new()
    {
        public virtual ILog Create()
        {
            return new T();
        }
    }

   
}
