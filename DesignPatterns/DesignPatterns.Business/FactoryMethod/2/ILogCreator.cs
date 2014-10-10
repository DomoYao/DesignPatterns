using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns.Business.FactoryMethod._2
{
    public interface ILogCreator
    {
        ILog Create();
    }
}
