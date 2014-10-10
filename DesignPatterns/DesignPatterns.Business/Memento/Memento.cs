using System;

/*
设计模式之美：Memento（备忘录）意图

在不破坏封装性的前提下，捕获一个对象的内部状态，并在该对象之外保存这个状态。这样以后就可将该对象恢复到原先保存的状态。

Without violating encapsulation, capture and externalize an object's internal state so that the object can be restored to this state later.

结构

参与者

Memento

    Memento 存储 Originator 对象的内部状态。Originator 根据需要决定 Memento 存储那些内部状态。
    防止 Originator 以外的其他对象访问 Memento。Memento 可实现两个接口，Caretaker 只能看到 Memento 的窄接口，Originator 可以看到宽接口。

Originator

    Originator 创建一个 Memento，用以记录当前时刻的内部状态。
    使用 Memento 恢复内部状态。

Caretaker

    负责保存 Memento。
    不能对 Memento 的内容进行操作和检查。

适用性

在以下情况下可以使用 Memento 模式：

    必须保存一个对象在某一个时刻的状态，这样以后需要时它才能恢复到先前的状态。
    如果一个用接口来让其他对象直接得到的这些状态，将会暴露对象的实现细节并破坏对象的封装性。

效果

    保持封装边界。
    简化了 Originator。
    定义窄接口和宽接口。
    使用和维护 Memento 的潜在代价。

相关模式

    可以使用 Memento 存储 Command 的内部状态，以支持撤销操作。
    Memento 可以使用 Iterator 进行迭代。

实现

Caretaker 向 Originator 请求一个 Memento，保留一段时间后，将其送回 Originator。

实现方式（一）：Memento 模式结构样式代码。

Memento 有两个接口：一个为 Originator 所使用的宽接口，一个为其他对象所使用的窄接口。
 */
namespace DesignPatterns.Business.Memento
{

    public class Memento
    {
        private readonly string _state;

        public Memento(string state)
        {
            _state = state;
        }

        public string GetState()
        {
            return _state;
        }
    }

    public class Originator
    {
        public string State { get; set; }

        public Memento CreateMemento()
        {
            return (new Memento(State));
        }

        public void SetMemento(Memento memento)
        {
            State = memento.GetState();
        }
    }

    public class Caretaker
    {
        public Memento Memento { get; set; }
    }

    public class Client
    {
        public static void TestCase1()
        {
            var originator = new Originator {State = "State A"};
            Console.WriteLine(originator.State);

            var memento = originator.CreateMemento();
            var caretaker = new Caretaker {Memento = memento};

            originator.State = "State B";
            Console.WriteLine(originator.State);

            originator.SetMemento(caretaker.Memento);
            Console.WriteLine(originator.State);
        }
    }

}
