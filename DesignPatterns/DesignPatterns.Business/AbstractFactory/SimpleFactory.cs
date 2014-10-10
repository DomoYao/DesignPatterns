using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.AbstractFactory
{
    namespace AbstractFactoryPattern.Implementation3
    {
        public enum LogCategory
        {
            File,
            DB,
        }

        public interface IFactory
        {
           object CreateLog(LogCategory category);
        }

        public interface  IFileLog
        {
            void WriteToFile();
        }

        public interface  IDbLog
        {
            void WriteToDb();
        }

        public class LogFactory : IFactory
        {
            public object CreateLog(LogCategory category)
            {
                switch (category)
                {
                    case LogCategory.File:
                        return new FileLog();
                    case LogCategory.DB:
                        return new DbLog();
                    default:
                        throw new NotSupportedException();
                }
            }
        }


        public class FileLog : IFileLog
        {
            public void WriteToFile()
            {
                Console.WriteLine("日志写入文件");
            }
        }

        public class DbLog : IDbLog
        {
            public void WriteToDb()
            {
                Console.WriteLine("日志写入数据库");
            }
        }

        public class AbstractFactoryTest
        {
            public static void Test()
            {
                IFactory kit = new LogFactory();
                IFileLog fileLog = (IFileLog)kit.CreateLog(LogCategory.File);
                IDbLog dbLog = (IDbLog)kit.CreateLog(LogCategory.DB);

                fileLog.WriteToFile();
                dbLog.WriteToDb();
            }
        }
    }
}
