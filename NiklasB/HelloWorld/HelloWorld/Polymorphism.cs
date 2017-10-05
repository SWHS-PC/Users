//
// This module demonstrates inheritance and polymorphism, which are key concepts
// in Object Oriented Programming (OOP).
//
// Inheritance enables a class to be "based on" (or "derived from") another class.
// This results in a hierarchy of classes, with derived classes representing more
// specific concepts and their base classes more general concepts.
//
// Conceptually, a derived class usually has an "is a" relationship with its base
// class. For example, if Cat derives from Mammal then an object of type Cat "is a"
// Mammal as well. Because of this relationship, you can assign an object of type
// Cat to a variable of type Mammal. Conversely, a variable of type Mammal might
// refer to an object of type Cat, or Dog, or anything else that derives from Mammal.
// In this case, we say the "static type" of the variable is Mammal, but the
// "dynamic type" of the object it refers to may be some other class derived from
// Mammal.
//
// A derived class inherits all the members of its base class. For example, a Cat
// inherits all of the methods and properties of a Mammal, and may then define some
// additional methods and properties specific to cats. A base class method can be
// declared "virtual", which means derived classes can have a different implementation
// of the method than the base class. When you call a virtual method, the call is
// dispatched to the method belonging to the *dynamic* type of the object.
//
// C# also has the concept of interface. An interface is like a base class in which
// all members are implicitly abstract. An interface doesn't implement anything. It
// only declares members that derived classes must override. A class that derives
// from an interface is said to "implement" the interface. In C#, a class can only
// derive from one base class, but it can implement multiple interfaces.
//

using System;
using System.IO;

namespace HelloWorld
{
    /// <summary>
    /// IExpression is the interface implemented by any mathematical expression
    /// or sub-expression.
    /// </summary>
    interface IExpression
    {
        // Evaluate the expression, returning its value.
        double Evaluate();

        // Write the expression as text.
        void Write(TextWriter output);
    }

    /// <summary>
    /// NumberExpression is a mathematical expression comprising a single number.
    /// </summary>
    class NumberExpression : IExpression
    {
        // The Value property specifies the numeric value.
        public double Value { get; set; }

        // IExpression implementation for NumberExpression.
        public double Evaluate()
        {
            return Value;
        }

        public void Write(TextWriter output)
        {
            output.Write(Value);
        }
    }

    /// <summary>
    /// UnaryExpression is the abstract base class of unary expression types such as
    /// NegativeExpression and SquareRootExpression.
    /// </summary>
    abstract class UnaryExpression : IExpression
    {
        // Unary expressions have an Operand, which is another expression.
        public IExpression Operand { get; set; }

        // This abstract class doesn't implement the IExpression methods, but declares
        // them as abstract so a derived class must override them.
        public abstract double Evaluate();
        public abstract void Write(TextWriter output);
    }

    /// <summary>
    /// BinaryExpression is the abstract base class of unary expression types such as
    /// AddExpression and MultiplyExpression.
    /// </summary>
    abstract class BinaryExpression : IExpression
    {
        // Binary expressions have two operands.
        public IExpression LeftOperand { get; set; }
        public IExpression RightOperand { get; set; }

        // This abstract class doesn't implement the IExpression methods, but declares
        // them as abstract so a derived class must override them.
        public abstract double Evaluate();
        public abstract void Write(TextWriter output);

        // This helper method is used by derived classes to implement the Write method.
        // It is declared "protected", so it is only visible to derived classes.
        protected void WriteBinaryExpression(TextWriter output, char op)
        {
            output.Write('(');

            LeftOperand.Write(output);

            output.Write(' ');
            output.Write(op);
            output.Write(' ');

            RightOperand.Write(output);

            output.Write(')');
        }
    }

    /// <summary>
    /// NegativeExpression is a unary expression that yields the negative
    /// of its operand.
    /// </summary>
    class NegativeExpression : UnaryExpression
    {
        public override double Evaluate()
        {
            return -Operand.Evaluate();
        }

        public override void Write(TextWriter output)
        {
            output.Write('-');
            Operand.Write(output);
        }
    }

    /// <summary>
    /// SquareRootExpression is a unary expression that yields the square root
    /// of its operand.
    /// </summary>
    class SquareRootExpression : UnaryExpression
    {
        public override double Evaluate()
        {
            return Math.Sqrt(Operand.Evaluate());
        }

        public override void Write(TextWriter output)
        {
            output.Write("sqrt(");
            Operand.Write(output);
            output.Write(')');
        }
    }

    /// <summary>
    /// AddExpression is a binary expression that yields the sum of its operands.
    /// </summary>
    class AddExpression : BinaryExpression
    {
        public override double Evaluate()
        {
            return LeftOperand.Evaluate() + RightOperand.Evaluate();
        }

        public override void Write(TextWriter output)
        {
            WriteBinaryExpression(output, '+');
        }
    }

    /// <summary>
    /// SubtractExpression is a binary expression that yields the difference of its operands.
    /// </summary>
    class SubtractExpression : BinaryExpression
    {
        public override double Evaluate()
        {
            return LeftOperand.Evaluate() - RightOperand.Evaluate();
        }

        public override void Write(TextWriter output)
        {
            WriteBinaryExpression(output, '-');
        }
    }

    /// <summary>
    /// MultiplyExpression is a binary expression that yields the product of its operands.
    /// </summary>
    class MultiplyExpression : BinaryExpression
    {
        public override double Evaluate()
        {
            return LeftOperand.Evaluate() * RightOperand.Evaluate();
        }

        public override void Write(TextWriter output)
        {
            WriteBinaryExpression(output, '*');
        }
    }

    /// <summary>
    /// DivideExpression is a binary expression that yields the quotient of its operands.
    /// </summary>
    class DivideExpression : BinaryExpression
    {
        public override double Evaluate()
        {
            return LeftOperand.Evaluate() / RightOperand.Evaluate();
        }

        public override void Write(TextWriter output)
        {
            WriteBinaryExpression(output, '/');
        }
    }

    /// <summary>
    /// PowerExpression is a binary expression that yields the left operand to the
    /// power of the right operand.
    /// </summary>
    class PowerExpression : BinaryExpression
    {
        public override double Evaluate()
        {
            return Math.Pow(LeftOperand.Evaluate(), RightOperand.Evaluate());
        }

        public override void Write(TextWriter output)
        {
            WriteBinaryExpression(output, '^');
        }
    }

    class Polymorphism
    {
        public static void Run()
        {
            // Construct a somewhat complex expression.
            var expr = new DivideExpression
            {
                LeftOperand = new PowerExpression
                {
                    LeftOperand = new NumberExpression { Value = 10 },
                    RightOperand = new NumberExpression { Value = 2 }
                },
                RightOperand = new NumberExpression {  Value = 4 }
            };

            // Call the virtual Write method to write the text of the expression.
            expr.Write(Console.Out);

            // Call the virtual Evaluate method, and write the value of the expression.
            Console.WriteLine(" = {0}", expr.Evaluate());
        }
    }
}
