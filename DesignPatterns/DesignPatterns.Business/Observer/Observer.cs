using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
设计模式之美：Observer（观察者）
别名

    Dependency
    Publish-Subscribe

意图

定义对象间的一种一对多的依赖关系，当一个对象的状态发生改变时，所有依赖于它的对象都得到通知并被自动更新。

Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.

结构

参与者

Subject

    Subject 知道它的 Observer。可以有任意多个 Observer 观察同一个 Subject。
    提供注册和删除 Observer 的接口。

Observer

    为那些在 Subject 发生改变时需要获得通知的对象定义一个 Update 接口。

ConcreteSubject

    将有关状态存储各个 ConcreteObserver 对象。
    当它的状态发生改变时，想它的各个 Observer 发出通知。

ConcreteObserver

    维护一个指向 ConcreteSubject 对象的引用。
    存储有关状态，这些状态应该与 ConcreteSubject 的状态保持一致。
    实现 Observer 的更新接口以使自身状态与 ConcreteSubject 状态保持一致。

适用性

在以下情况下可以使用 Observer 模式：

    当一个抽象模型有两个方面，其中一个方面依赖于另一个方面。将这二者封装在独立的对象中以使它们可以各自独立地改变和复用。
    当对一个对象的改变需要同时改变其他对象，而不知道具体有多少对象有待改变。
    当一个对象必须通知其他对象，而它又不能假定其他对象时谁。

效果

    目标与观察者间的抽象耦合。
    支持广播通信。
    意外的更新。因为 Observer 并不知道其他 Observer 的存在，所以对改变 Subject 的最终代价一无所知。

相关模式

    可以使用 Mediator 模式封装复杂的更新语义，充当 Subject 与 Observer 之间的中介者。

实现

下面的交互图描述 Subject 与 Observer 之间的协作:

实现方式（一）：Observer 模式结构样式代码。

推模式（Push Model）：Subject 向 Observer 发送关于改变的详细信息，而不管它们是否需要。

拉模式（Pull Model）：Subject 除最小通知外什么也不推送，由 Observer 显式地向 Subject 询问细节。
 */

namespace DesignPatterns.Business.Observer
{

    public abstract class Observer
    {
        public abstract void Update();
    }

    public abstract class Subject
    {
        private readonly List<Observer> _observers = new List<Observer>();

        public void Attach(Observer observer)
        {
            _observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }

    public class ConcreteSubject : Subject
    {
        private string _state;

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                Notify();
            }
        }
    }

    public class ConcreteObserver : Observer
    {
        private readonly ConcreteSubject _subject;

        public ConcreteObserver(string name, ConcreteSubject subject)
        {
            Name = name;
            _subject = subject;
        }

        public string Name { get; private set; }

        public override void Update()
        {
            string subjectState = _subject.State;
            Console.WriteLine(Name + ": " + subjectState);
        }
    }

    public class ConcreteObserver2 : Observer
    {
        private readonly ConcreteSubject _subject;

        public ConcreteObserver2(string name, ConcreteSubject subject)
        {
            Name = name;
            _subject = subject;
        }

        public string Name { get; private set; }

        public override void Update()
        {
            string subjectState = _subject.State;
            Console.WriteLine(Name + ": " + subjectState+"......");
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            var subject = new ConcreteSubject();
            subject.Attach(new ConcreteObserver("Observer 1", subject));
            subject.Attach(new ConcreteObserver("Observer 2", subject));
            subject.Attach(new ConcreteObserver("Observer 3", subject));
            subject.Attach(new ConcreteObserver2("Observer 4", subject));

            subject.State = "Hello World";
        }
    }
}