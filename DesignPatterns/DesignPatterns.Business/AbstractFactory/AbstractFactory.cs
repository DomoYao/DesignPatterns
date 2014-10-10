using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.AbstractFactory
{
    namespace AbstractFactoryPattern.Implementation3
    {

        public interface ILogFactory2
        {
            IFileLog CreateFileLog();
            IDbLog CreateDbLog();
            IOtherLog CreateOtherLog<TC>() where TC : IOtherLog, new();
        }



        public class LogFactory2<TA, TB> : ILogFactory2
            where TA : IFileLog, new()
            where TB : IDbLog, new()
        {
            public IFileLog CreateFileLog()
            {
                return new FileLog();
            }

            public IDbLog CreateDbLog()
            {
               return new DbLog();
            }

            public IOtherLog CreateOtherLog<TC>() where TC : IOtherLog, new()
            {
                return new TC();
            }
        }


        public interface IOtherLog
        {
            void WriteToOther();
        }

        public class OtherLog : IOtherLog
        {
            public void WriteToOther()
            {
                Console.WriteLine("其他日志方式");
            }
        }

       

        public class AbstractFactoryTest2
        {
            public static void Test()
            {
                ILogFactory2 kit = new LogFactory2<FileLog,DbLog>();
                IFileLog fileLog = kit.CreateFileLog();
                IDbLog dbLog = kit.CreateDbLog();

                fileLog.WriteToFile();
                dbLog.WriteToDb();

                IOtherLog otherLog = kit.CreateOtherLog<OtherLog>();
                otherLog.WriteToOther();
            }
        }
    }
}
