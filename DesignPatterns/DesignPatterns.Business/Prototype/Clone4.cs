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

namespace DesignPatterns.Business.Prototype4
{
    /// <summary>
    /// 实现方式（四）：初始化克隆对象。
    /// 客户可能会希望使用一些值来初始化该对象的内部状态。
    /// 但在 Clone 操作中传递参数会破坏克隆接口的统一性。
    /// 原型的类可以在 Clone 操作之后，调用包含初始化参数的 Initialize 方法来设定对象内部状态。
    /// </summary>
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

    public class ConcreteDeepCopyPrototypeProductB: AbstractOrInterfaceOfPrototypeProduct
    {
        public ConcreteDeepCopyPrototypeProductB()
        {
            this.ReferenceProperty2 = new ReferencedClass()
            {
                ReferencedClassProperty1 = 222
            };
        }

        public void Initialize(int propertyValue)
        {
            this.ValueProperty1 = propertyValue;
            this.ReferenceProperty2.ReferencedClassProperty1 = propertyValue;
        }

        public override AbstractOrInterfaceOfPrototypeProduct Clone()
        {
            return new ConcreteDeepCopyPrototypeProductB()
                {
                    ValueProperty1 = this.ValueProperty1,
                    ReferenceProperty2 = new ReferencedClass()
                        {
                            ReferencedClassProperty1 =this.ReferenceProperty2.ReferencedClassProperty1
                        }
                };
        }
    }

    public class Client
    {
        public static void TestCase4()
        {
            AbstractOrInterfaceOfPrototypeProduct prototypeProduct2 = new ConcreteDeepCopyPrototypeProductB();
            var clonedProduct2 =(ConcreteDeepCopyPrototypeProductB) prototypeProduct2.Clone();

            clonedProduct2.Initialize(123);
            Console.WriteLine(clonedProduct2.ReferenceProperty2.ReferencedClassProperty1);
        }
    }


}
