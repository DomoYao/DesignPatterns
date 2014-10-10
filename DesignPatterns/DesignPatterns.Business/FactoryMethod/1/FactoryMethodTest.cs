using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._1
{
    public class FactoryMethodTest
    {
        public static void Test()
        {
            var creator=new LogCreator();
            ILog log = creator.Create(LogCategory.DB);
            log.WriteLog();

            var creator2 = new LogCreator();
            ILog log2 = creator2.Create(LogCategory.File);
            log2.WriteLog();
        }
    }
}
