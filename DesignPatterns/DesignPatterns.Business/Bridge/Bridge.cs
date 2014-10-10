using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
参与者

Abstraction

    定义抽象类的接口。
    维护一个指向 Implementor 类型对象的指针。

RefinedAbstraction

    扩充由 Abstraction 定义的接口。

Implementor

    定义实现类的接口，该接口不一定要与 Abstraction 接口完全一致，甚至可以完全不同。
    Implementor 接口仅提供基本操作，Abstraction 则定义了基于这些基本操作的较高层次的操作。

ConcreteImplementor

    实现 Implementor 接口并定义它的具体实现。
桥接模式的特点

              桥接模式的主要目的是将一个对象的变化因素抽象出来，不是通过类继承的方式来满足这个因素的变化，而是通过对象组合的方式来依赖因素的抽象，这样当依赖的因素

    的具体实现发生变化后，而我们的具体的引用却不用发生改变，因为我们的对象是依赖于抽象的，而不是具体的实现。

              而且，通过这样的依赖抽象，我们在多个对象共享这样的因素的时候，就成为可能，如果我们使用的是具体的因素的共享，当我们改变这个变化因素的时候，我们必须把

    使用这个因素的所有的对象，都进行相应的修改，而如果所有的引用这个变化因素的对象都依赖于抽象而不是具体的依赖呢？这也为我们的共享的提供了变化性。

适用性

在以下情况下可以使用 Bridge 模式：

    你不希望在抽象和它的实现部分之间有一个固定的绑定关系。比如需要在程序运行时刻实现部分应可以被选择或者切换。
    类的抽象以及它的实现都应该可以通过生成子类的方法加以扩充。
    对一个抽象的实现部分的修改应对客户不产生影响，即客户的代码不必重新编译。
    你想对客户完全隐藏抽象的实现部分。
    类的层次需要将一个对象分解成两个部分。
    你想在多个对象间共享实现，但同时要求客户并不知道这一点。
    1、当一个对象有多个变化因素的时候，通过抽象这些变化因素，将依赖具体实现，修改为依赖抽象。
    2、当某个变化因素在多个对象中共享时。我们可以抽象出这个变化因素，然后实现这些不同的变化因素。
    3、当我们期望一个对象的多个变化因素可以动态的变化，而且不影响客户的程序的使用时。

效果

    分离接口及其实现部分。
    提高可扩充性。
    实现细节对客户透明。

相关模式

    Abstract Factory 模式可以用来创建和配置一个特定的 Bridge 模式。
    Adaptor 模式用来帮助无关的类协同工作，它通常在系统设计完成后才会被使用。Bridge 模式则是在系统开始时就被使用，它使得抽象接口和实现部分可以独立进行改变。
    Bridge 模式的结构与对象 Adapter 模式类似，但是 Bridge 模式的出发点不同：Bridge 目的是将接口部分和实现部分分离，从而对它们可以较为容易也相对独立的加以改变。而 Adapter 则意味着改变一个已有对象的接口。

实现

实现方式（一）：使用 Bridge 模式分离抽象部分和实现部分。

当一个抽象可能有多个实现时，通常用继承来协调它们。抽象类定义对该抽象的接口，而具体的子类则用不同方式加以实现。

但是此方法有时不够灵活。继承机制将抽象部分与它的实现部分固定在一起，使得难以对抽象部分和实现部分独立地进行修改、扩充和重用。

使用 Bridge 模式，它在抽象类与它的实现直接起到了桥梁作用，使它们可以独立地变化。
 */

namespace DesignPatterns.Business.Bridge
{

    public class Abstraction
    {
        protected IImplementor Implementor;

        public Abstraction(IImplementor implementor)
        {
            Implementor = implementor;
        }

        /// <summary>
        /// 经常变化的因子
        /// </summary>
        public virtual void Operation()
        {
            Implementor.OperationImp1();
        }
    }

    /// <summary>
    /// 实现者接口
    /// </summary>
    public interface IImplementor
    {
        void OperationImp1();
    }

    public class ConcreteImplementorA : IImplementor
    {
        private const string Name = "Sqlserver 数据库操作";
        public void OperationImp1()
        {
            Console.WriteLine(Name);
            // do something
        }
    }

    public class ConcreteImplementorB : IImplementor
    {
        private const string Name = "Oracle 数据库的操作";

        public void OperationImp1()
        {
            Console.WriteLine(Name);
            // do something
            
        }
    }

    public class ChildAbstraction : Abstraction
    {
        public ChildAbstraction(IImplementor implementor)
            : base(implementor)
        {
        }

        public override void Operation()
        {
            base.Operation();
            Console.WriteLine("桥接模式，不管你是什么数据库，我都能实现");
            // do some others
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            IImplementor implementor1 = new ConcreteImplementorA();
            IImplementor implementor2 = new ConcreteImplementorB();

            Abstraction abstraction0 = new Abstraction(implementor1);
            // 一个对象的多个变化因素可以动态的变化，而且不影响客户的程序的使用时
            Abstraction abstraction1 = new ChildAbstraction(implementor1);
            Abstraction abstraction2 = new ChildAbstraction(implementor2);

            abstraction0.Operation();
            abstraction1.Operation();
            abstraction2.Operation();
        }
    }

}
