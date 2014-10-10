using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 实现方式（二）：注入 Receiver 的指定方法，Command 仅能调用该方法。

namespace DesignPatterns.Business.Command2
{

    public abstract class Command
    {
        public abstract void Execute();
    }

    public class ConcreteCommand : Command
    {
        private readonly Action _action;

        public ConcreteCommand(Action action)
        {
            _action = action;
        }

        public override void Execute()
        {
            _action.Invoke();
        }
    }

    public class Receiver
    {
        public void Action()
        {
            Console.WriteLine("do something....");
            // do something
        }

        public void Action2()
        {
            Console.WriteLine("do other something....");
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
        public static void TestCase2()
        {
            var receiver = new Receiver();

            Command cmd = new ConcreteCommand(receiver.Action);
            var invoker = new Invoker();
            invoker.StoreCommand(cmd);
            invoker.Invoke();

            Command cmd2 = new ConcreteCommand(receiver.Action2);
            var invoker2 = new Invoker();
            invoker2.StoreCommand(cmd2);
            invoker2.Invoke();
        }
    }

}
