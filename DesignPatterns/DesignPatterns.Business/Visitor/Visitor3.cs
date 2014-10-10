using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 实现方式（三）：使用 Acyclic Visitor 模式解构设计。

我们注意到实现方式（二） Employee 类依赖于 EmployeeVisitor 基类。而 EmployeeVisitor 类为每个 Employee 的子类都提供了一个 Visit 方法。

因此，这里形成了一个依赖关系的环。这导致 Visitor 在响应变化时变得复杂。

Visitor 模式在类继承关系不是经常变化时可以工作的很好，但在子类衍生频繁的情况下会增加复杂度。

此时，我们可以应用 Acyclic Visitor 模式，抽象出窄接口，以使 Employee 子类仅依赖于该窄接口。
 */
namespace DesignPatterns.Business.Visitor3
{
    public abstract class Employee
    {
        public abstract string Accept(EmployeeVisitor visitor);
    }

    public class HourlyEmployee : Employee
    {
        public override string Accept(EmployeeVisitor visitor)
        {
            try
            {
                var hourlyEmployeeVisitor = (IHourlyEmployeeVisitor) visitor;
                return hourlyEmployeeVisitor.Visit(this);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }
    }

    public class SalariedEmployee : Employee
    {
        public override string Accept(EmployeeVisitor visitor)
        {
            try
            {
                var salariedEmployeeVisitor = (ISalariedEmployeeVisitor) visitor;
                return salariedEmployeeVisitor.Visit(this);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }
    }

    public interface IHourlyEmployeeVisitor
    {
        string Visit(HourlyEmployee employee);
    }

    public interface ISalariedEmployeeVisitor
    {
        string Visit(SalariedEmployee employee);
    }

    public abstract class EmployeeVisitor
    {
    }

    public class HoursPayReport : EmployeeVisitor, IHourlyEmployeeVisitor
    {
        public string Visit(HourlyEmployee employee)
        {
            // generate the line of the report.
            return "100 Hours and $1000 in total.";
        }
    }

    public class SalariedPayReport : EmployeeVisitor, ISalariedEmployeeVisitor
    {
        public string Visit(SalariedEmployee employee)
        {
            return "Something";
        }
    }


    public class Client
    {
        public static void TestCase3()
        {
            Employee salariedEmployee = new SalariedEmployee();
            var result = salariedEmployee.Accept(new SalariedPayReport());
            Console.WriteLine(result);

            Employee hourlyEmployee = new HourlyEmployee();
            result = hourlyEmployee.Accept(new HoursPayReport());
            Console.WriteLine(result);


            Employee hourlyEmployee2 = new HourlyEmployee();
            result = hourlyEmployee2.Accept(new SalariedPayReport());
            Console.WriteLine(result);
        }
    }
}
