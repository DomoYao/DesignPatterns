using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//假设我们有一个 Employee 类，Employee 分为按时薪计算的 Employee 和按月薪计算的 Employee。
namespace DesignPatterns.Business.Visitor2
{

    public abstract class Employee
    {
        public abstract string Accept(EmployeeVisitor visitor);
    }

    public class HourlyEmployee : Employee
    {
        public override string Accept(EmployeeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class SalariedEmployee : Employee
    {
        public override string Accept(EmployeeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public abstract class EmployeeVisitor
    {
        public abstract string Visit(HourlyEmployee employee);
        public abstract string Visit(SalariedEmployee employee);
    }

    public class HoursPayReport : EmployeeVisitor
    {
        public override string Visit(HourlyEmployee employee)
        {
            // generate the line of the report.
            return "100 Hours and $1000 in total.";
        }

        public override string Visit(SalariedEmployee employee)
        {
            // do nothing
            return "100 Days and RMB1000 in total.";
        }
    }

    public class Client
    {
        public static void TestCase2()
        {
            Employee salariedEmployee = new SalariedEmployee();
            var result = salariedEmployee.Accept(new HoursPayReport());
            Console.WriteLine(result);

            Employee hourlyEmployee = new HourlyEmployee();
            result = hourlyEmployee.Accept(new HoursPayReport());
            Console.WriteLine(result);
            
        }
    }
}
