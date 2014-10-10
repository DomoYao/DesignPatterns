using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._2
{
    public class FileLog:ILog
    {
        public void WriteLog()
        {
            Console.WriteLine("FileLog...");
        }
    }
}
