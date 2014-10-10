using System;
using System.Collections.Generic;

/*
 参与者

Prototype

    声明一个克隆自身的接口。

ConcretePrototype

    实现一个克隆自身的操作。

Client

    让一个原型克隆自身从而创建一个新的对象。

适用性

在以下情况下可以使用 Prototype 模式：

    一个系统要独立于它的产品的创建、构成和表示时。
    当要实例化的类是在运行时刻指定时，例如：通过动态装载。
    为了避免创建一个与产品类层次平行的工厂类层次时。
    当一个类的实例只能有几个不同状态组合中的一种时。建立相应数目的原型并克隆它们可能比每次用合适的状态手工实例化该类更方便一些。

缺点

    每一个 Prototype 子类都必须实现 Clone 操作。当内部包括一些不支持拷贝或有循环引用的对象时，实现克隆可能也会很困难。

效果

    它对客户隐藏了具体的产品类，因此减少了客户知道的名字的数目。
    使客户无需改变即可使用与特定应用相关的类。
    运行时刻增加和删除产品。
    改变值以指定新对象。
    改变结构以指定新对象。
    减少子类的构造。
    用类动态配置应用。

 */

namespace DesignPatterns.Business.Prototype2
{
    /// 实现方式（二）：使用浅拷贝实现克隆（Clone）操作。
    ///Prototype 模式最困难的部分在于正确的实现 Clone 操作。
    ///浅拷贝（Shallow Copy）在拷贝时只复制对象所有字段的值。如果字段是值类型，则复制其值；如果字段是引用类型，则复制引用指针。
    public class ReferencedClass
    {
        public int ReferencedClassProperty1 { get; set; }
    }

    public abstract class AbstractOrInterfaceOfPrototypeProduct
    {
        public int ValueProperty1 { get; set; }
        public ReferencedClass ReferenceProperty2 { get; set; }

        public abstract AbstractOrInterfaceOfPrototypeProduct Clone();
    }

    public class ConcreteShallowCopyPrototypeProductA: AbstractOrInterfaceOfPrototypeProduct
    {
        public ConcreteShallowCopyPrototypeProductA()
        {
            this.ReferenceProperty2 = new ReferencedClass()
                {
                    ReferencedClassProperty1 = 111
                };
        }

        public override AbstractOrInterfaceOfPrototypeProduct Clone()
        {
            return new ConcreteShallowCopyPrototypeProductA()
                {
                    ValueProperty1 = this.ValueProperty1,
                    ReferenceProperty2 = this.ReferenceProperty2,
                };
        }
    }

    public class Client
    {
        public static void TestCase2()
        {
            AbstractOrInterfaceOfPrototypeProduct prototypeProduct1 = new ConcreteShallowCopyPrototypeProductA();
            AbstractOrInterfaceOfPrototypeProduct clonedProduct1 = prototypeProduct1.Clone();
            bool areEqual1 = object.ReferenceEquals(prototypeProduct1.ReferenceProperty2,clonedProduct1.ReferenceProperty2);
            Console.WriteLine(areEqual1);
        }
    }


}
