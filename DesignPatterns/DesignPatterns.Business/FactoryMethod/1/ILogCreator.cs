using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._1
{
    public interface ILogCreator
    {
        ILog Create(LogCategory  logCategory);
    }

    public enum LogCategory
    {
        File,
        DB,
    }
}
