using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignPatterns.Business.AbstractFactory.AbstractFactoryPattern.Implementation3;
using DesignPatterns.Business.FactoryMethod._1;
using DesignPatterns.Business.FactoryMethod._2;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            // 工厂方法
            //FactoryMethodTest.Test();
            // 泛型工厂方法
            //FactoryMethod2Test.Test();

            // 抽象工厂
            //AbstractFactoryTest.Test();

            // 泛型抽象工厂
            //AbstractFactoryTest2.Test();

            // 单列模式
            //Business.Bulider.Client.TestCase1();
            //Business.Bulider2.Client.TestCase2();
            
            // 原型模式
            //DesignPatterns.Business.Prototype.Client.TestCase1();
            //DesignPatterns.Business.Prototype2.Client.TestCase2();
            //DesignPatterns.Business.Prototype3.Client.TestCase3();
            //DesignPatterns.Business.Prototype4.Client.TestCase4();

            // 组合模式
            //DesignPatterns.Business.Composite.Client.TestCase1();

            // 桥接模式
            // DesignPatterns.Business.Bridge.Client.TestCase1();

            // 适配器模式
            //DesignPatterns.Business.Adapter.Client.TestCase1();
            //DesignPatterns.Business.Adapter2.Client.TestCase2();

            // 装饰模式
            //DesignPatterns.Business.Decorator.Client.TestCase1();
            //DesignPatterns.Business.Decorator2.Client.TestCase1();

            //外观模式
            //DesignPatterns.Business.Facade.Client.TestCase1();

            // Flyweight（享元）
            //DesignPatterns.Business.Flyweight.Client.TestCase1();
            // 代理模式
            //DesignPatterns.Business.Proxy.Client.TestCase1();

            // 职责链
            // DesignPatterns.Business.ChainOfResponsibilityPattern.Client.TestCase1();

            // Command（命令）
            // DesignPatterns.Business.Command1.Client.TestCase1();
            // DesignPatterns.Business.Command2.Client.TestCase2();
            //DesignPatterns.Business.Command3.Client.TestCase3();
            //DesignPatterns.Business.Command4.Client.TestCase4();
            // DesignPatterns.Business.Command5.Client.TestCase5();
            // DesignPatterns.Business.Command6.Client.TestCase6();
            // DesignPatterns.Business.Command.Client.TestCase7();

            //Interpreter（解释器）
            // DesignPatterns.Business.Interpreter1.Client.TestCase1();
            // DesignPatterns.Business.Interpreter2.Client.TestCase2();

            // Iterator（迭代器）
            //DesignPatterns.Business.Iterator1.Client.TestCase1();

            // Mediator（中介者）
           // DesignPatterns.Business.Mediator.Client.TestCase1();

            // Memento（备忘录）
           // DesignPatterns.Business.Memento.Client.TestCase1();

            // Observer（观察者）
            //DesignPatterns.Business.Observer.Client.TestCase1();

            // State（状态）
            //DesignPatterns.Business.State.Client.TestCase1();

            //Strategy（策略）
            //DesignPatterns.Business.Strategy.Client.TestCase1();

            //Template Method（模板方法）
            //DesignPatterns.Business.TemplateMethod.Client.TestCase1();

            //Visitor（访问者）
            //DesignPatterns.Business.Visitor1.Client.TestCase1();
            //DesignPatterns.Business.Visitor2.Client.TestCase2();
            //DesignPatterns.Business.Visitor3.Client.TestCase3();

            //Object Pool（对象池）
            DesignPatterns.Business.ObjectPool1.Client.TestCase1();

            //随机数
            //Console.WriteLine(DesignPatterns.UniqueId.UniqueId.Generate());

            // 快速排序
            //DesignPatterns.Sort.QuickSort.Client();
            Console.ReadLine();
        }
    }
}
