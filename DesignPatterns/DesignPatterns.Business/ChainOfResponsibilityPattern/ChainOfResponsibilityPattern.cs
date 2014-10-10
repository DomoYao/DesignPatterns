/*
 
设计模式之美：Chain of Responsibility（职责链）

意图

使多个对象都有机会处理请求，从而避免请求的发送者和接收者之间的耦合关系。

将这些对象连成一条链，并沿着这条链传递该请求，直到有一个对象处理它位置。

Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request.

Chain the receiving objects and pass the request along the chain until an object handles it.
 * 
参与者

Handler

    定义一个处理请求的接口。
    实现后继链

ConcreteHandler

    处理它所负责的请求。
    可访问它的后继者。
    如果可处理该请求，就处理；否则将该请求转发给它的后继者。

Client

    向链上的具体处理者对象提交请求。

适用性

在以下情况下可以使用 Chain of Responsibility 模式：

    有多个对象可以处理一个请求，哪个对象处理该请求运行时自动确定。
    你想在不明确指定接收者的情况下，向多个对象中的一个提交一个请求。
    可处理一个请求的对象集合应被动态指定。

效果

    降低耦合度。对象无需知道哪个一个对象处理其请求，仅需知道对象被处理。
    增强了给对象指派职责的灵活性。可以运行时对该链进行动态增加或修改。

相关模式

    Chain of Resposibility 常与 Composite 一起使用。一个构件的父构件可作为它的后继。

 */

using System;

namespace DesignPatterns.Business.ChainOfResponsibilityPattern
{
    public enum RequestCategory
    {
        Category1,
        Category2,
    }

    public abstract class Request
    {
        public abstract RequestCategory Category { get; }
        public bool IsHandled { get; set; }
    }

    public class ConcreteRequest1 : Request
    {
        public override RequestCategory Category
        {
            get { return RequestCategory.Category1; }
        }
    }

    public class ConcreteRequest2 : Request
    {
        public override RequestCategory Category
        {
            get { return RequestCategory.Category2; }
        }
    }

    public abstract class Handler
    {
        private readonly Handler _successor;

        protected Handler()
        {
        }

        protected Handler(Handler successor)
        {
            _successor = successor;
        }

        public void Handle(Request request)
        {
            OnHandle(request);

            if (!request.IsHandled)
            {
                if (_successor != null)
                {
                    // pass request to successor
                    _successor.Handle(request);
                }
            }
        }

        protected abstract void OnHandle(Request request);
    }

    public class ConcreteHandler1 : Handler
    {
        public ConcreteHandler1()
        {
        }

        public ConcreteHandler1(Handler successor)
            : base(successor)
        {
        }

        protected override void OnHandle(Request request)
        {
            if (request.Category == RequestCategory.Category1)
            {
                // handle the request which category is Category1
                Console.WriteLine("ConcreteHandler1");
                request.IsHandled = true;
            }
        }
    }

    public class ConcreteHandler2 : Handler
    {
        public ConcreteHandler2()
        {
        }

        public ConcreteHandler2(Handler successor)
            : base(successor)
        {
        }

        protected override void OnHandle(Request request)
        {
            if (request.Category == RequestCategory.Category2)
            {
                // handle the request which category is Category2
                Console.WriteLine("ConcreteHandler2");
                request.IsHandled = true;
            }
        }
    }

    public class Client
    {
        public static void TestCase1()
        {
            Request request1 = new ConcreteRequest1();
            Request request2 = new ConcreteRequest2();

            Handler handler2 = new ConcreteHandler2();
            Handler handler1 = new ConcreteHandler1(handler2);

            handler1.Handle(request1);
            handler1.Handle(request2);
        }
    }
}

