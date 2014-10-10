using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
代理模式意图

为其他对象提供一种代理以控制对这个对象的访问。

Provide a surrogate or placeholder for another object to control access to it.

结构

运行时一种可能的 Proxy 结构的对象图：

参与者

Proxy

    保存一个引用使得代理可以访问实体。若 RealSubject 和 Subject 的接口相同，Proxy 会引用 Subject。
    提供一个与 Subject 的接口相同的接口，这样 Proxy 就可以用来代替实体。
    控制对实体的存取，并可能负责创建和删除它。
    其他功能依赖于 Proxy 的类型：
        远程代理（Remote Proxy）负责对请求及其参数进行编码，并向不同地址空间中的实体发送已编码的请求。
        虚拟代理（Virtual Proxy）可以缓存实体的附加信息，以便延迟对它的访问。
        保护代理（Protection Proxy）检查调用者是否具有实现一个请求所必须的访问权限。

Subject

    定义 RealSubject 和 Proxy 的共用接口，这样就在任何使用 RealSubject 的地方都可以使用 Proxy。

RealSubject

    定义 Proxy 所代表的实体。

适用性

下面是一些使用 Proxy 模式的常见情况：

    远程代理（Remote Proxy）为一个对象在不同的地址空间提供局部代表。
    虚拟代理（Virtual Proxy）根据需要创建开销很大的对象。
    保护代理（Protection Proxy）控制对原始对象的访问。
    智能代理（Smart Proxy）在访问对象时执行一些附件操作。

效果

    Proxy 模式在访问对象时引入了一定程度的间接性。
    Proxy 模式可以对用户隐藏 Copy-On-Write 优化方式。用 Proxy 延迟对象拷贝过程，仅当这个对象被修改时才进行真正的拷贝，用以大幅度降低拷贝大实体的开销。

相关模式

    Adapter 为它所适配的对象提供了一个不同的接口。Proxy 提供了与它的实体相同的接口
    Decorator 的实现与 Proxy 相似，但目的不一样。 Decorator 为对象添加功能，Proxy 则控制对对象的访问。

实现

实现方式（一）：使用相同 Subject 接口实现 Proxy。

对一个对象进行访问控制的一个原因是为了只有在我们确实需要这个对象时才对它进行创建和初始化。
 */

namespace DesignPatterns.Business.Proxy
{

    public abstract class Subject
    {
        public abstract string Name { get; }
        public abstract void Request();
    }

    public class ConcreteSubject : Subject
    {
        private readonly string _name;

        public ConcreteSubject(string name)
        {
            _name = name;
        }

        public override string Name
        {
            get { return _name; }
        }

        public override void Request()
        {
            Console.WriteLine("do something  " + Name);
            // do something
        }
    }

    public class Proxy : Subject
    {
        private Subject _realSubject;
        private readonly string _name;

        public Proxy(string name)
        {
            _name = name;
        }

        public override string Name
        {
            get { return _name; }
        }

        public override void Request()
        {
            if (_realSubject == null)
            {
                LoadRealSubject();
            }

            _realSubject.Request();
        }

        private void LoadRealSubject()
        {
            // do some heavy things
            _realSubject = new ConcreteSubject(_name);
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            Subject subject = new Proxy("SubjectName");
            var subjectName = subject.Name;
            Console.WriteLine(subjectName);
            subject.Request();
        }
    }

}


/*
 结构型模式涉及到如何组合类和对象以获得更大的结构。

    结构型类模式采用继承机制来组合接口实现。
    结构型对象模式不是对接口和实现进行组合，而是描述了如何对一些对象进行组合，从而实现新功能的一些方法。

因为可以在运行时改变对象组合关系，所以对象组合方式具有更大的灵活性，而这种机制用静态组合是不可能实现的。

    Adapter（适配器）
        将一个类的接口转换成客户希望的另外一个接口。
        Adapter 模式使得原本由于接口不兼容而不能一起工作的那些类可以一起工作。
        Convert the interface of a class into another interface clients expect.
        Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.
    Bridge（桥接）
        将抽象部分与它的实现部分分离，使它们都可以独立地变化。
        Decouple an abstraction from its implementation so that the two can vary independently.
    Composite（组合）
        将对象组合成树形结构以表示 “部分-整体” 的层次结构。
        Composite 使得用户对于单个对象和组合对象的使用具有一致性。
        Compose objects into tree structures to represent part-whole hierarchies.
        Composite lets clients treat individual objects and compositions of objects uniformly.
    Decorator（装饰）
        动态地给一个对象添加一些额外的职责。
        就增加功能来说，Decorator 模式相比生成子类更为灵活。
        Attach additional responsibilities to an object dynamically.
        Decorators provide a flexible alternative to subclassing for extending functionality.
    Facade（外观）
        为子系统中的一组接口提供一个一致的界面。
        Facade 模式定义了一个高层接口，这个接口使得这一子系统更加容易使用。
        Provide a unified interface to a set of interfaces in a subsystem.
        Facade defines a higher-level interface that makes the subsystem easier to use.
    Flyweight（享元）
        运用共享技术有效地支持大量细粒度的对象。
        Use sharing to support large numbers of fine-grained objects efficiently.
    Proxy（代理）
        为其他对象提供一种代理以控制对这个对象的访问。
        Provide a surrogate or placeholder for another object to control access to it.

结构型模式之间存在很多相似性，尤其是它们的参与者和协作之间的相似性。

这是因为结构型模式依赖于同一个很小的语言机制集合构造代码和对象：

    单继承和多重继承机制用于基于类的模式。
    对象组合机制用于对象式模式。

Adapter 和 Bridge 的相似性

Adapter 模式和 Bridge 模式具有一些共同的特征。它们之间的不同之处主要在于它们各自的用途。

Adapter 模式主要是为了解决两个已有接口之间不匹配的问题。它不考虑这些接口时怎么实现的，也不考虑它们各自可能会如何演化。

Bridge 模式则对抽象接口和它的实现部分进行桥接。它为用户提供了一个稳定的接口。

Adapter 模式和 Bridge 模式通常被用于软件生命周期的不同阶段。

当你发现两个不兼容的类必须同时工作时，就有必要使用 Adapter 模式，以避免代码重复。此处耦合不可见。

相反，Bridge 的使用者必须事先知道：一个抽象将有多个实现部分，并且抽象和实现两者是独立演化得。

Composite 和 Decorator 的相似性

Composite 和 Decorator 模式具有类似的结构图，这说明它们都基于递归组合来组织可变数目的对象。

Decorator 旨在使你能够不需要生成子类即可给对象添加职责。这就避免了静态实现所有功能组合，从而导致子类急剧增加。

Composite 则有不同的目的，它旨在构造类，使多个相关的对象能够以统一的方式处理，而多重对象可以被当作一个对象来处理。它重点不在于修饰，而在于表示。

Decorator 和 Proxy 的相似性

Decorator 和 Proxy 模式描述了怎样为对象提供一定程度上的间接引用。

Decorator 和 Proxy 对象的实现部分都保留了指向另一个对象的指针，它们向这个对象发送请求。

同样，它们也具有不同的设计目的。

Proxy 不能动态地添加和分离性质，它也不是为递归组合而设计的。它的目的是，当直接访问一个实体不方便或不符合需要时，为这个实体提供一个替代者。

在 Proxy 中，实体定义了关键功能，而 Proxy 提供对它的访问。

在 Decorator 中，组件仅提供了部分功能，而 Decorator 负责完成其他功能。
 
 */