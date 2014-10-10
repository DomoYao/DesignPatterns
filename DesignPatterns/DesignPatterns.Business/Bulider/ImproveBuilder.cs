using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Builder 逐步的构造产品，所以其接口必须足够的普遍。
namespace DesignPatterns.Business.Bulider2
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

        public virtual string BuildValueDependOnWeatherPart(string weather)
        {
            // could do nothing by default
            ComplexProduct.ValueDependOnWeather = weather;
            return ComplexProduct.ValueDependOnWeather;
        }

        public virtual string BuildValueDependOnFortunePart(string luck, string combinedWithWeather)
        {
            // could do nothing by default
            ComplexProduct.ValueDependOnFortune = luck + combinedWithWeather;
            return ComplexProduct.ValueDependOnFortune;
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

        public override string BuildValueDependOnWeatherPart(string weather)
        {
            // something customized
            ComplexProduct.ValueDependOnWeather = _dayOfWeek + " is " + weather;
            return ComplexProduct.ValueDependOnWeather;
        }

        public override string BuildValueDependOnFortunePart(string luck, string combinedWithWeather)
        {
            // something customized
            if (_luckyNumber == 8)
                ComplexProduct.ValueDependOnFortune = "Supper" + luck + combinedWithWeather;
            else
                ComplexProduct.ValueDependOnFortune = "Just so so" + luck + combinedWithWeather;
            return ComplexProduct.ValueDependOnFortune;
        }
    }

    /// <summary>
    /// 如果构造过程中需要访问前面已经构造了的产品构件，则 Builder 将构件返回给 Director，由 Director 将构件传递给 Builder 中的下一个步骤
    /// </summary>
    public class GoodWeatherAndGoodLuckDirector
    {
        public void ConstructWithGoodWeatherAndGoodLuck(AbstractComplexProductBuilder builder)
        {
            string weather = builder.BuildValueDependOnWeatherPart(@"PM2.5 < 50");
            builder.BuildValueDependOnFortunePart(@"Good Luck", weather);
        }

        public void ConstructWithBadWeatherAndBadLuck(AbstractComplexProductBuilder builder)
        {
            string weather = builder.BuildValueDependOnWeatherPart(@"PM2.5 > 500");
            builder.BuildValueDependOnFortunePart(@"Bad Luck", weather);
        }
    }

    public class Client
    {
        public static void TestCase2()
        {
            AbstractComplexProductBuilder builder = new ConcreteProductBuilderA("Sunday", 9);
            GoodWeatherAndGoodLuckDirector director = new GoodWeatherAndGoodLuckDirector();

            builder.BeginBuild();
            director.ConstructWithGoodWeatherAndGoodLuck(builder);
            ComplexProduct productWithGoodLuck = builder.EndBuild();
            Console.WriteLine(productWithGoodLuck.ValueDependOnFortune + ":::" + productWithGoodLuck.ValueDependOnWeather);

            builder.BeginBuild();
            director.ConstructWithBadWeatherAndBadLuck(builder);
            ComplexProduct productWithBadLuck = builder.EndBuild();
            Console.WriteLine(productWithBadLuck.ValueDependOnFortune + "---" + productWithBadLuck.ValueDependOnWeather);
        }
    }
}

