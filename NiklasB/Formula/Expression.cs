using System;
using System.IO;

namespace Formula
{
    abstract class Expression
    {
        /// <summary>
        /// Compute the value of the expression.
        /// </summary>
        public abstract double Evaluate();

        /// <summary>
        /// Write the expression text to a TextWriter, such as Console.Out.
        /// </summary>
        public abstract void Write(TextWriter output);
    }

    /// <summary>
    /// NumberExpression is a mathematical expression comprising a single number.
    /// </summary>
    class NumberExpression : Expression
    {
        // The Value property specifies the numeric value.
        public double Value { get; set; }

        // IExpression implementation for NumberExpression.
        public override double Evaluate()
        {
            return Value;
        }

        public override void Write(TextWriter output)
        {
            output.Write(Value);
        }
    }

    /// <summary>
    /// VariableExpression is a mathematical expression comprising a variable.
    /// It is like a NumberExpression except that has a name as well as a value.
    /// </summary>
    class VariableExpression : Expression
    {
        // Name of the variable.
        public string Name { get; set; }

        // Current value of the variable.
        public double Value { get; set; }

        // IExpression implementation for NumberExpression.
        public override double Evaluate()
        {
            return Value;
        }

        public override void Write(TextWriter output)
        {
            output.Write(Name);
        }
    }

    /// <summary>
    /// UnaryExpression is the abstract base class of unary expression types such as
    /// NegativeExpression and SquareRootExpression.
    /// </summary>
    abstract class UnaryExpression : Expression
    {
        // Unary expressions have an Operand, which is another expression.
        public Expression Operand { get; set; }
    }

    /// <summary>
    /// BinaryExpression is the abstract base class of unary expression types such as
    /// AddExpression and MultiplyExpression.
    /// </summary>
    abstract class BinaryExpression : Expression
    {
        // Binary expressions have two operands.
        public Expression LeftOperand { get; set; }
        public Expression RightOperand { get; set; }

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
}
