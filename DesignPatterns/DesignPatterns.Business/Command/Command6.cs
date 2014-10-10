using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
实现方式（六）：使 Command 支持 Undo 和 Redo。

如果 Command 提供方法逆转操作，例如 Undo 操作，就可以取消执行的效果。为达到这个目的，ConcreteCommand 类可能需要存储额外的状态信息。

这个状态包括：

接收者对象，它真正执行处理该请求的各操作。

接收者上执行操作的参数。

如果处理请求的操作会改变接收者对象中的某些值，那么这些值也必须先存储起来。接收者还必须提供一些操作，以使该命令可将接收者恢复到它先前的状态。
 */

namespace DesignPatterns.Business.Command6
{

    public abstract class Command
    {
        public abstract void Execute();
        public abstract void Unexecute();
        public abstract void Reexecute();
    }

    public class ConcreteCommand : Command
    {
        private readonly Receiver _receiver;
        private readonly string _state;
        private string _lastState;

        public ConcreteCommand(Receiver receiver, string state)
        {
            _receiver = receiver;
            _state = state;
        }

        public override void Execute()
        {
            _lastState = _receiver.Name;
            _receiver.ChangeName(_state);
        }

        public override void Unexecute()
        {
            _receiver.ChangeName(_lastState);
            _lastState = string.Empty;
        }

        public override void Reexecute()
        {
            Unexecute();
            Execute();
        }
    }

    public class Receiver
    {
        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Console.WriteLine(name);
            // do something
            Name = name;
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

        public void UndoInvoke()
        {
            if (_cmd != null)
            {
                _cmd.Unexecute();
            }
        }
    }

    public class Client
    {
        public static void TestCase6()
        {
            var receiver = new Receiver();
            Command cmd = new ConcreteCommand(receiver, "Hello World");

            var invoker = new Invoker();
            invoker.StoreCommand(cmd);

            invoker.Invoke();
            invoker.UndoInvoke();
        }
    }
}