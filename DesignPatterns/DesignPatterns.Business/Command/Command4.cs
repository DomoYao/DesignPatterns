using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 使用泛型减少 Command 子类。
namespace DesignPatterns.Business.Command4
{

    public abstract class Command
    {
        public abstract void Execute();
    }

    public class ConcreteCommand<T, TS> : Command
    {
        private readonly Action<T, TS> _action;
        private readonly T _state1;
        private readonly TS _state2;

        public ConcreteCommand(Action<T, TS> action, T state1, TS state2)
        {
            _action = action;
            _state1 = state1;
            _state2 = state2;
        }

        public override void Execute()
        {
            _action.Invoke(_state1, _state2);
        }
    }

    public class Receiver
    {
        public void Action(string state1, int state2)
        {
            Console.WriteLine("do something "+state1+"   "+state2);
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
        public static void TestCase4()
        {
            var receiver = new Receiver();
            Command cmd = new ConcreteCommand<string, int>(receiver.Action, "Hello World", 250);

            var invoker = new Invoker();
            invoker.StoreCommand(cmd);

            invoker.Invoke();
        }
    }
}
