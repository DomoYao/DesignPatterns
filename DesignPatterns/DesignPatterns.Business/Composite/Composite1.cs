using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 组合模式
//参与者

//Component

//    为组合中的对象声明接口。
//    在适当的情况下，实现所有类共有接口的缺省行为
//    声明一个接口用于访问和管理 Component 的子组件。
//    在递归结构中定义一个接口，用于访问一个父部件，并在合适的情况下实现它。

//Leaf

//    在组合中表示叶节点对象，叶节点没有子节点。
//    在组合中定义图元对象的行为。

//Composite

//    定义有子部件的那些部件的行为。
//    在 Composite 接口中实现与子部件有关的操作。

//Client

//    通过 Component 接口操纵组合部件的对象。

//适用性

//在以下情况下可以使用 Composite 模式：

//    你想表示对象的 “部分-整体” 层次结构。
//    你希望用户忽略组合对象与单个对象的不同，用户将统一地使用组合结构中的所有对象。

//缺点

//    与类层次结构设计原则冲突

//Composite 模式的目的之一是使得用户不知道它们正在使用具体的 Leaf 和 Composite 类。

//为达到这一目的，Component 需要为 Leaf 和 Composite 定义一些公共操作，并提供缺省的实现，而 Leaf 和 Composite 子类可以对它们进行重定义。

//然而，这个目标会与类层次结构设计原则冲突，该原则规定：一个类只能定义那些对它的子类有意义的操作。

//效果

//    定义了包含基本对象和组合对象的类层次结构。
//    简化客户代码。
//    使得更容易增加新类型的组件。
//    使你的设计变得更加一般化。

//相关模式

//    Command 模式描述了如何用一个 MacroCommand Composite 类组成一些 Command 对象，并对它们进行排序。
//    通常 “部件-父部件” 连接用于 Responsibility of Chain 模式。
//    Decorator 模式经常与 Composite 模式一起使用。它们通常有一个公共的父类。
//    Flyweight 让你共享组件，但不再能引用它们的父部件。
//    Iterator 可以用来遍历 Composite。
//    Visitor 将本来应该分布在 Composite 和 Leaf 类中的操作和行为局部化。

namespace DesignPatterns.Business.Composite
{
    /// <summary>
    /// 实现方式（一）：在 Component 中定义公共接口以保持透明性但损失安全性。
    /// 在 Component 中定义 Add 和 Remove 操作需要考虑安全性和透明性。
    /// 在类层次结构的根部定义子节点管理接口的方法具有良好的透明性，但是这一方法是以安全性为代价的，因为客户有可能会做一些无意义的事情，例如在 Leaf 中 Add 对象等。
    /// 在 Composite 类中定义管理子部件的方法具有良好的安全性，但是这又损失了透明性，因为 Leaf 和 Composite 具有不同的接口。
    /// </summary>
    public abstract class Component
    {
        protected List<Component> Children = new List<Component>();

        public string Name { get; set; }

        public abstract void Operation();

        public virtual void Add(Component component)
        {
            Children.Add(component);
        }

        public virtual void Remove(Component component)
        {
            Children.Remove(component);
        }

        public virtual IEnumerable<Component> GetChildren()
        {
            return Children;
        }
    }

    public class Leaf : Component
    {
        public override void Operation()
        {
            foreach (var child in Children)
            {
                child.Operation();
            }

            // may do something
            Console.WriteLine(Name);
        }

        public override void Add(Component component)
        {
            Children.Add(component);
        }

        public override void Remove(Component component)
        {
            Children.Add(component);
        }

        public override IEnumerable<Component> GetChildren()
        {
            return Children;
        }
    }

    public class Composite : Component
    {
        public override void Operation()
        {
            foreach (var child in Children)
            {
                child.Operation();
            }

            // may do something
            Console.WriteLine(Name);
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            Component component1 = new Leaf(){Name = "Leaf"};
            Component component2 = new Composite() { Name = "Composite" };

            component2.Add(component1);

            component1.Operation();
            component2.Operation();
        }
    }

}
