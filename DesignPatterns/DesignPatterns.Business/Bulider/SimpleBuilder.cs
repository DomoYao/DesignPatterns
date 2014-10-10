using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Builder

//    为创建一个 Product 对象的各个部件指定抽象接口。

//ConcreteBuilder

//    实现 Builder 的接口以构造和装配该产品的各个部件。
//    定义并明确它所创建的表示。
//    提供一个检索产品的接口。

//Director

//    构造一个使用 Builder 接口的对象。

//Product

//    表示被构造的复杂对象。ConcreteBuilder 创建该产品的内部表示并定义它的装配过程。
//    包含定义组成部件的类，包括将这些部件装配成最终产品的接口。

//适用性

//在以下情况下可以使用 Builder 模式：

//    当创建复杂对象的算法应该独立于该对象的组成部分以及它们的装配方式时。
//    当构造过程必须允许被构造的对象有不同的表示时。

//效果

//    它使你可以改变一个产品的内部表示。在改变该产品的内部表示时所要做的只是定义一个新的 ConcreteBuilder。
//    它将构造代码和表示代码分开，提高了对象的模块性。客户不需要知道定义产品内部结构的类的所有信息。
//    它使你可以对构造过程进行更精细的控制。对象是在 Director 的控制下一步一步构造的，仅当产品构造完成时 Director 才从 Builder 中取回它。

//相关模式

//    Abstract Factory 和 Builder 相似，因为它也可以创建复杂对象。区别是 Builder 着重于一步步构造一个复杂对象。而 Abstract Factory 着重于多个系列的产品对象（或简单或复杂）。Builder 是在最后一步返回产品，Abstract Factory 是立即返回。
//    Composite 通常是用 Builder 生成的。

namespace DesignPatterns.Business.Bulider
{

    public class ComplexProduct
    {
        public string ValueDependOnWeather { get; set; }
        public string ValueDependOnFortune { get; set; }
    }

    public abstract class AbstractComplexProductBuilder
    {
        protected ComplexProduct ComplexProduct;

        public void BeginBuild(ComplexProduct existingComplexProduct = null)
        {
            if (existingComplexProduct == null)
                ComplexProduct = new ComplexProduct();
            else
                ComplexProduct = existingComplexProduct;
        }

        public virtual void BuildValueDependOnWeatherPart(string weather)
        {
            // could do nothing by default
            ComplexProduct.ValueDependOnWeather = weather;
        }

        public virtual void BuildValueDependOnFortunePart(string luck)
        {
            // could do nothing by default
            ComplexProduct.ValueDependOnFortune = luck;
        }

        public ComplexProduct EndBuild()
        {
            return this.ComplexProduct;
        }
    }

    public class ConcreteProductBuilderA : AbstractComplexProductBuilder
    {
        private readonly string _dayOfWeek;
        private readonly int _luckyNumber;

        public ConcreteProductBuilderA(string dayOfWeek, int luckyNumber)
        {
            _dayOfWeek = dayOfWeek;
            _luckyNumber = luckyNumber;
        }

        public override void BuildValueDependOnWeatherPart(string weather)
        {
            // something customized
            ComplexProduct.ValueDependOnWeather = _dayOfWeek + " is " + weather;
        }

        public override void BuildValueDependOnFortunePart(string luck)
        {
            // something customized
            if (_luckyNumber == 8)
                ComplexProduct.ValueDependOnFortune = "Supper" + luck;
            else
                ComplexProduct.ValueDependOnFortune = "Just so so" + luck;
        }
    }

    public class GoodWeatherAndGoodLuckDirector
    {
        public void ConstructWithGoodWeatherAndGoodLuck(AbstractComplexProductBuilder builder)
        {
            builder.BuildValueDependOnWeatherPart(@"PM2.5 < 50");
            builder.BuildValueDependOnFortunePart(@"Good Luck");
        }

        public void ConstructWithBadWeatherAndBadLuck(AbstractComplexProductBuilder builder)
        {
            builder.BuildValueDependOnWeatherPart(@"PM2.5 > 500");
            builder.BuildValueDependOnFortunePart(@"Bad Luck");
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            AbstractComplexProductBuilder builder = new ConcreteProductBuilderA("Sunday", 9);
            GoodWeatherAndGoodLuckDirector director = new GoodWeatherAndGoodLuckDirector();

            builder.BeginBuild();
            director.ConstructWithGoodWeatherAndGoodLuck(builder);
            ComplexProduct productWithGoodLuck = builder.EndBuild();

            Console.WriteLine(productWithGoodLuck.ValueDependOnFortune+":::"+productWithGoodLuck.ValueDependOnWeather);

            builder.BeginBuild();
            director.ConstructWithBadWeatherAndBadLuck(builder);
            ComplexProduct productWithBadLuck = builder.EndBuild();

            Console.WriteLine(productWithBadLuck.ValueDependOnFortune + "---" + productWithBadLuck.ValueDependOnWeather);
        }
    }
}

