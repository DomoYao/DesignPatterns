using System;
using System.Collections.Generic;

/*
Flyweight（享元）意图

运用共享技术有效地支持大量细粒度的对象。

Use sharing to support large numbers of fine-grained objects efficiently.

结构

下面的对象图说明了如何共享 Flyweight：

参与者

Flyweight

    描述一个接口，通过这个接口 Flyweight 可以接受并作用于外部状态。

ConcreteFlyweight

    实现 Flyweight 接口，并为内部状态增加存储空间。该对象必须是可共享的。它所存储的状态必须是内部的，即必须独立于对象的场景。

UnsharedConcreteFlyweight

    并非所有的 Flyweight 子类都需要被共享。Flyweight 接口使共享成为可能，但它并不强制共享。

FlyweightFactory

    创建并管理 Flyweight 对象。
    确保合理地共享 Flyweight。

Client

    维持一个对 Flyweight 的引用。
    计算或存储 Flyweight 的外部状态。

适用性

Flyweight 模式的有效性很大程度上取决于如何使用它以及在何处使用它。

当以下情况成立时可以使用 Flyweight 模式：

    一个应用程序使用了大量的对象。
    完全由于使用大量对象，造成很大的存储开销。
    对象的大多数状态都可变为外部状态。
    如果删除对象的外部状态，那么可以用相对较少的共享对象取代很多组对象。
    应用程序不依赖于对象标识。

效果

    存储空间上的节省抵消了传输、查找和计算外部状态时的开销。节约量随着共享状态的增多而增大。

相关模式

    Flyweight 模式通常和 Composite 模式结合起来，用共享叶节点的又向无环图实现一个逻辑上的层次结构。
    通常，最好用 Flyweight 实现 State 和 Strategy 对象。

实现

实现方式（一）：使用 FlyweightFactory 管理 Flyweight 对象。

Flyweight 模式的可用性在很大程度上取决于是否易识别外部状态并将它从共享对象中删除。

理想的状况是，外部状态可以由一个单独的对象结构计算得到，且该结构的存储要求非常小。

通常，因为 Flyweight 对象是共享的，用户不能直接对它进行实例化，因为 FlyweightFactory 可以帮助用户查找某个特定的 Flyweight 对象。

共享还意味着某种形式的引用计数和垃圾回收。
 */

namespace DesignPatterns.Business.Flyweight
{

    public abstract class Flyweight
    {
        public abstract string Identifier { get; }
        public abstract void Operation(string extrinsicState);
    }

    public class ConcreteFlyweight : Flyweight
    {
        public override string Identifier
        {
            get { return "hello"; }
        }

        public override void Operation(string extrinsicState)
        {
            // do something
            Console.WriteLine(Identifier+";;;;;" + extrinsicState);
        }
    }

   
    /// <summary>
    /// 采用一个共享来避免大量拥有相同内容对象的开销。
    ///这种开销中最常见、直观的就是内存的损耗。享元模式以共享的方式高效的支持大量的细粒度对象。
    ///在名字和定义中都体现出了共享这一个核心概念，
    ///那么怎么来实现共享呢？要知道每个事物都是不同的，但是又有一定的共性，
    ///如果只有完全相同的事物才能共享，那么享元模式可以说就是不可行的；因此我们应该尽量将事物的共性共享，
    ///而又保留它的个性。为了做到这点，享元模式中区分了内蕴状态和外蕴状态。内蕴状态就是共性，外蕴状态就是个性了。
    ///注：共享的对象必须是不可变的，不然一变则全变（如果有这种需求除外)
    ///内蕴状态存 储在享元内部，不会随环境的改变而有所不同，是可以共享的；外蕴状态是不可以共享的，
    ///它随环境的改变而改变的，因此外蕴状态是由客户端来保持（因为环境的 变化是由客户端引起的）。
    ///在每个具体的环境下，客户端将外蕴状态传递给享元，从而创建不同的对象出来。
    /// </summary>
    public class FlyweightFactory
    {
        private readonly Dictionary<string, Flyweight> _pool = new Dictionary<string, Flyweight>();

        public Flyweight CreateFlyweight(string identifier)
        {
            if (!_pool.ContainsKey(identifier))
            {
                Flyweight flyweight = new ConcreteFlyweight();
                _pool.Add(flyweight.Identifier, flyweight);
            }

            return _pool[identifier];
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            var factory = new FlyweightFactory();
            Flyweight flyweight1 = factory.CreateFlyweight("hello");
            Flyweight flyweight2 = factory.CreateFlyweight("hello");
            flyweight1.Operation("extrinsic state");
            flyweight2.Operation("extrinsic state");
        }
    }

}
