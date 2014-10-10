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
namespace DesignPatterns.Business.Prototype
{
    /// <summary>
    /// 实现方式（一）：使用一个原型管理器。
    ///当一个系统中原型数目不固定时，可以保持一个可用原型的注册表，用以存储和检索原型。我们称这个注册表为原型管理器（Prototype Manager）。
    ///客户在克隆一个原型前会先向注册表请求该原型。
    /// </summary>
    public abstract class AbstractOrInterfaceOfPrototypeProduct
    {
        public int ValueProperty1 { get; set; }

        public abstract AbstractOrInterfaceOfPrototypeProduct Clone();
    }

    public class ConcretePrototypeProductA : AbstractOrInterfaceOfPrototypeProduct
    {
        public override AbstractOrInterfaceOfPrototypeProduct Clone()
        {
            return new ConcretePrototypeProductA
                {
                    ValueProperty1 = this.ValueProperty1,
                };
        }
    }

    public class ConcretePrototypeProductB : AbstractOrInterfaceOfPrototypeProduct
    {
        public override AbstractOrInterfaceOfPrototypeProduct Clone()
        {
            return new ConcretePrototypeProductB
                {
                    ValueProperty1 = this.ValueProperty1,
                };
        }
    }

    public class ProductPrototypeManager
    {
        private readonly Dictionary<string, AbstractOrInterfaceOfPrototypeProduct> _registry= new Dictionary<string, AbstractOrInterfaceOfPrototypeProduct>();

        public void Register(string name,AbstractOrInterfaceOfPrototypeProduct prototypeProduct)
        {
            _registry[name] = prototypeProduct;
        }

        public void Unregister(string name)
        {
            _registry.Remove(name);
        }

        public AbstractOrInterfaceOfPrototypeProduct Retrieve(string name)
        {
            return _registry[name];
        }

        public bool IsRegisterd(string name)
        {
            return _registry.ContainsKey(name);
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            AbstractOrInterfaceOfPrototypeProduct prototypeProduct1 = new ConcretePrototypeProductA{ValueProperty1 = 1};
            AbstractOrInterfaceOfPrototypeProduct prototypeProduct2 = new ConcretePrototypeProductB{ValueProperty1 = 2};

            var manager = new ProductPrototypeManager();
            manager.Register("PrototypeProduct1", prototypeProduct1);
            manager.Register("PrototypeProduct2", prototypeProduct2);

            AbstractOrInterfaceOfPrototypeProduct clonedProduct1 = manager.Retrieve("PrototypeProduct1").Clone();

            Console.WriteLine(clonedProduct1.ValueProperty1);

            if (manager.IsRegisterd("PrototypeProduct2"))
            {
                AbstractOrInterfaceOfPrototypeProduct clonedProduct2 = manager.Retrieve("PrototypeProduct2").Clone();
                Console.WriteLine(clonedProduct2.ValueProperty1);
            }
        }
    }

}
