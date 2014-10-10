using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
// 使用弱引用代替对 Receiver 的强引用。
namespace DesignPatterns.Business.Command5
{
    public class WeakAction
    {
        public WeakAction(Action action)
        {
            Method = action.Method;
            Reference = new WeakReference(action.Target);
        }

        protected MethodInfo Method { get; private set; }
        protected WeakReference Reference { get; private set; }

        public bool IsAlive
        {
            get { return Reference.IsAlive; }
        }

        public object Target
        {
            get { return Reference.Target; }
        }

        public void Invoke()
        {
            if (Method != null && IsAlive)
            {
                Method.Invoke(Target, null);
            }
        }
    }

    public abstract class Command
    {
        public abstract void Execute();
    }

    public class ConcreteCommand : Command
    {
        private readonly WeakAction _action;

        public ConcreteCommand(Action action)
        {
            _action = new WeakAction(action);
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
            Console.WriteLine("do something");
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
        public static void TestCase5()
        {
            var receiver = new Receiver();
            Command cmd = new ConcreteCommand(receiver.Action);

            var invoker = new Invoker();
            invoker.StoreCommand(cmd);

            invoker.Invoke();
        }
    }
}