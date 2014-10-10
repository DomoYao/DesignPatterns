﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 实现方式（一）：Interpreter 模式结构样式代码。

TerminalExpression：实现解释 Terminal Symbols 的语法。

NonTerminalExpression：聚合一到多个 Expression，Expression 可以是 TerminalExpression，也可以是 NonTerminalExpression。。
 */
namespace DesignPatterns.Business.Interpreter1
{
    public class Context
    {
        public Context(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public abstract class ExpressionBase
    {
        public abstract void Interpret(Context context);
    }

    public class TerminalExpression : ExpressionBase
    {
        public override void Interpret(Context context)
        {
            Console.WriteLine("Terminal Symbol {0}.", context.Name);
        }
    }

    public class NonTerminalExpression : ExpressionBase
    {
        public ExpressionBase Expression1 { get; set; }
        public ExpressionBase Expression2 { get; set; }

        public override void Interpret(Context context)
        {
            Console.WriteLine("Non Terminal Symbol {0}.", context.Name);
            Expression1.Interpret(context);
            Expression2.Interpret(context);
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            var context = new Context("Hello World");
            var root = new NonTerminalExpression
                {
                    Expression1 = new TerminalExpression(),
                    Expression2 = new TerminalExpression()
                };

            root.Interpret(context);
        }
    }
}