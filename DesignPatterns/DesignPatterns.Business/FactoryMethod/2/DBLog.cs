using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._2
{
    public class DBLog:ILog
    {
    
        public void WriteLog()
        {
            Console.WriteLine("DBLog....");
        }
    }
}
