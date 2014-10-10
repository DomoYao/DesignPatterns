using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 解释波兰表达式（Polish Notation）。

中缀表达式

中缀表达式中，二元运算符总是置于与之相关的两个运算对象之间，根据运算符间的优先关系来确定运算的次序，同时考虑括号规则。
比如： 2 + 3 * (5 - 1)

前缀表达式

波兰逻辑学家 J.Lukasiewicz 于 1929 年提出了一种不需要括号的表示法，将运算符写在运算对象之前，也就是前缀表达式，即波兰式（Polish Notation, PN）。
比如：2 + 3 * (5 - 1) 这个表达式的前缀表达式为 + 2 * 3 - 5 1。

后缀表达式

后缀表达式也称为逆波兰式（Reverse Polish Notation, RPN），和前缀表达式相反，是将运算符号放置于运算对象之后。
比如：2 + 3 * (5 - 1) 用逆波兰式来表示则是：2 3 5 1 - * +。
 */
namespace DesignPatterns.Business.Interpreter2
{

    public interface IExpression
    {
        int Evaluate();
    }

    public class IntegerTerminalExpression : IExpression
    {
        private readonly int _value;

        public IntegerTerminalExpression(int value)
        {
            _value = value;
        }

        public int Evaluate()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class AdditionNonterminalExpression : IExpression
    {
        private readonly IExpression _expr1;
        private readonly IExpression _expr2;

        public AdditionNonterminalExpression(IExpression expr1,IExpression expr2)
        {
            _expr1 = expr1;
            _expr2 = expr2;
        }

        public int Evaluate()
        {
            int value1 = _expr1.Evaluate();
            int value2 = _expr2.Evaluate();
            return value1 + value2;
        }

        public override string ToString()
        {
            return string.Format("({0} + {1})", _expr1, _expr2);
        }
    }

    public class SubtractionNonterminalExpression : IExpression
    {
        private readonly IExpression _expr1;
        private readonly IExpression _expr2;

        public SubtractionNonterminalExpression(
            IExpression expr1,
            IExpression expr2)
        {
            _expr1 = expr1;
            _expr2 = expr2;
        }

        public int Evaluate()
        {
            int value1 = _expr1.Evaluate();
            int value2 = _expr2.Evaluate();
            return value1 - value2;
        }

        public override string ToString()
        {
            return string.Format("({0} - {1})", _expr1, _expr2);
        }
    }

    public interface IParser
    {
        IExpression Parse(string polish);
    }

    public class Parser : IParser
    {
        public IExpression Parse(string polish)
        {
            var symbols = new List<string>(polish.Split(' '));
            return ParseNextExpression(symbols);
        }

        private IExpression ParseNextExpression(List<string> symbols)
        {
            int value;
            if (int.TryParse(symbols[0], out value))
            {
                symbols.RemoveAt(0);
                return new IntegerTerminalExpression(value);
            }
            return ParseNonTerminalExpression(symbols);
        }

        private IExpression ParseNonTerminalExpression(List<string> symbols)
        {
            var symbol = symbols[0];
            symbols.RemoveAt(0);

            var expr1 = ParseNextExpression(symbols);
            var expr2 = ParseNextExpression(symbols);

            switch (symbol)
            {
                case "+":
                    return new AdditionNonterminalExpression(expr1, expr2);
                case "-":
                    return new SubtractionNonterminalExpression(expr1, expr2);
                default:
                    {
                        string message = string.Format("Invalid Symbol ({0})", symbol);
                        throw new InvalidOperationException(message);
                    }
            }
        }
    }

    public class Client
    {
        public static void TestCase2()
        {
            IParser parser = new Parser();

            var commands =
                new string[]
                    {
                        "+ 1 2",
                        "- 3 4",
                        "+ - 5 6 7",
                        "+ 8 - 9 1",
                        "+ - + - - 2 3 4 + - -5 6 + -7 8 9 0"
                    };

            foreach (var command in commands)
            {
                IExpression expression = parser.Parse(command);
                Console.WriteLine("{0} = {1}", expression, expression.Evaluate());
            }

            // Results:
            // (1 + 2) = 3
            // (3 - 4) = -1
            // ((5 - 6) + 7) = 6
            // (8 + (9 - 1)) = 16
            // (((((2 - 3) - 4) + ((-5 - 6) + (-7 + 8))) - 9) + 0) = -24
        }
    }
}