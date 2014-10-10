using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._1
{
    /// <summary>
    /// 接口也可以是抽象类，让子类决定实例化哪一个目标类
    /// </summary>
    public interface ILog
    {
        void WriteLog();
    }
}
