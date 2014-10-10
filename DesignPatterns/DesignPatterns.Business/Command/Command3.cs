using System;

//参数化 Command 构造。
namespace DesignPatterns.Business.Command3
{

    public abstract class Command
    {
        public abstract void Execute();
    }

    public class ConcreteCommand : Command
    {
        private readonly Action<string> _action;
        private readonly string _state;

        public ConcreteCommand(Action<string> action, string state)
        {
            _action = action;
            _state = state;
        }

        public override void Execute()
        {
            _action.Invoke(_state);
        }
    }

    public class Receiver
    {
        public void Action(string state)
        {
            Console.WriteLine("do something" + state);
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
        public static void TestCase3()
        {
            var receiver = new Receiver();
            Command cmd = new ConcreteCommand(receiver.Action, "Hello World");

            var invoker = new Invoker();
            invoker.StoreCommand(cmd);

            invoker.Invoke();
        }
    }
}
