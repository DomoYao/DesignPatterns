using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
意图
   将一个类的接口转换成客户希望的另外一个接口。
参与者

Target

    定义 Client 使用的与特定领域相关的接口。

Client

    与符合 Target 接口的对象协同。

Adaptee

    定义一个已经存在的接口，这个接口需要适配。

Adapter

    对 Adaptee 的接口与 Target 接口进行适配。

适用性

在以下情况下可以使用 Adapter 模式：

    你想使用一个已经存在的类，而它的接口不符合你的需求。
    你想创建一个可以复用的类，该类可以与其他不相关的类或不可预见的类协同工作。
    你想使用一些已经存在的类，但是不可能对每一个都进行子类化匹配它们的接口。对象适配器可以适配它的父类的接口。

效果

    允许一个 Adapter 与多个 Adaptee （Adaptee 本身及它的子类）同时协同。Adapter 也可以一次给所有的 Adaptee 添加功能。
    使得重定义 Adaptee 的行为比较困难。这就需要生成 Adaptee 的子类并且使得 Adapter 引用这个子类而不是引用 Adaptee 自身。

相关模式

    Bridge 模式的结构与对象 Adapter 模式类似，但是 Bridge 模式的出发点不同：Bridge 目的是将接口部分和实现部分分离，从而对它们可以较为容易也相对独立的加以改变。而 Adapter 则意味着改变一个已有对象的接口。
    Decorator 模式增强了其他对象的功能而同时又不改变它的接口。因此 Decorator 对应用程序的透明性比 Adapter 要好。结果是 Decorator 支持递归组合，而 Adapter 无法实现这一点。
    Proxy 模式在不改变它的接口的条件下，为另一个对象定义了一个代理。

 */

namespace DesignPatterns.Business.Adapter
{
    /// <summary>
    /// 简单直接的对象适配器
    /// </summary>
    public class ParentAdaptee
    {
        public virtual string SpecificRequest()
        {
            return "ParentAdaptee";
        }
    }

    public class ChildAdaptee : ParentAdaptee
    {
        public override string SpecificRequest()
        {
            return "ChildAdaptee";
        }
    }

    public class Target
    {
        public virtual string Request()
        {
            return "Target";
        }
    }

    public class Adapter : Target
    {
        private readonly ParentAdaptee _adaptee;

        public Adapter(ParentAdaptee adaptee)
        {
            _adaptee = adaptee;
        }

        public override string Request()
        {
            return _adaptee.SpecificRequest();
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            ParentAdaptee adaptee = new ChildAdaptee();
            Target target = new Adapter(adaptee);
            var result = target.Request();
            Console.WriteLine(result);
        }
    }

}
