using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._1
{
    public class LogCreator:ILogCreator
    {
        /// <summary>
        /// 定义成虚方法，可以被子类继承改变被实例化的类.
        /// </summary>
        /// <param name="logCategory"></param>
        /// <returns></returns>
        public virtual ILog Create(LogCategory logCategory)
        {
            switch (logCategory)
            {
                case LogCategory.DB:
                    return new DBLog();
                case LogCategory.File:
                    return new FileLog();
                default:
                    throw new NotSupportedException();
            }
        }
    }

   
}
