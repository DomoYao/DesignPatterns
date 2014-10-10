using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._2
{
    public class FactoryMethod2Test
    {
        public static void Test()
        {
            var creator = new LogCreator<DBLog>();
            var creator2 = new LogCreator<FileLog>();
            ILog log = creator.Create();
            ILog log2 = creator2.Create();
            log.WriteLog();
            log2.WriteLog();
        }
    }
}
