using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//实现方式（一）：直接注入 Receiver 对象，Command 决定调用哪个方法。

namespace DesignPatterns.Business.Command1
{

    public abstract class Command
    {
        public abstract void Execute();
    }

    public class ConcreteCommand : Command
    {
        private readonly Receiver _receiver;

        public ConcreteCommand(Receiver receiver)
        {
            _receiver = receiver;
        }

        public override void Execute()
        {
            _receiver.Action();
        }
    }

    public class Receiver
    {
        public void Action()
        {
            Console.WriteLine("do something....");
            // do something
        }
    }

    public class Invoker
    {
        private Command _cmd;

        public void StoreCommand(Command cmd)
        {
            _cmd = cmd;
        }

        public void Invoke()
        {
            if (_cmd != null)
            {
                _cmd.Execute();
            }
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            var receiver = new Receiver();
            Command cmd = new ConcreteCommand(receiver);

            var invoker = new Invoker();
            invoker.StoreCommand(cmd);

            invoker.Invoke();
        }
    }
}

